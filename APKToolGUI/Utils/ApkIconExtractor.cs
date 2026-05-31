using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Ionic.Zip; // DotNetZip NuGet: Install-Package DotNetZip

namespace APKToolGUI.Utils
{
    // Resolves an APK's launcher icon by parsing resources.arsc directly. Handles apps whose
    // resource names have been optimized/obfuscated (so the aapt-reported path no longer maps to a
    // real entry) and adaptive icons (defined only as anydpi XML, no pixels) by falling back to the
    // foreground layer raster. Returns the raw PNG/WebP/JPG bytes, or null when nothing usable is found.
    public static class ApkIconExtractor
    {
        private const int DENSITY_MDPI = 160;
        private const int DENSITY_HDPI = 240;
        private const int DENSITY_XHDPI = 320;
        private const int DENSITY_XXHDPI = 480;
        private const int DENSITY_XXXHDPI = 640;

        private const ushort CHUNK_STRING_POOL = 0x0001;
        private const ushort CHUNK_TABLE = 0x0002;
        private const ushort CHUNK_PACKAGE = 0x0200;
        private const ushort CHUNK_TYPE_SPEC = 0x0202;
        private const ushort CHUNK_TYPE = 0x0201;

        private const byte VALUE_TYPE_STRING = 0x03;
        private const uint NO_ENTRY = 0xFFFFFFFF;

        // Launcher-icon entry-key names to match exactly (the real app icon).
        private static readonly string[] PrimaryIconKeys =
            { "ic_launcher", "icon", "app_icon", "ic_launcher_round", "icon_round", "launcher_icon" };

        // Raster extensions we can actually decode into a Bitmap. Adaptive-icon XML
        // definitions (.xml) are intentionally excluded — they carry no pixels.
        private static readonly string[] RasterExtensions = { ".png", ".webp", ".jpg", ".jpeg" };

        // Best → worst density bucket. Anything outside the standard set (tvdpi, nodpi,
        // anydpi, default, …) gets the lowest rank but is still accepted as a last resort.
        private static int DensityRank(int density)
        {
            switch (density)
            {
                case DENSITY_XXXHDPI: return 6;
                case DENSITY_XXHDPI: return 5;
                case DENSITY_XHDPI: return 4;
                case DENSITY_HDPI: return 3;
                case DENSITY_MDPI: return 2;
                default: return 1;
            }
        }

        // ── Public API ───────────────────────────────────────────────

        public static byte[] ExtractIcon(string apkPath)
        {
            if (string.IsNullOrEmpty(apkPath) || !File.Exists(apkPath))
                return null;

            try
            {
                using (var zip = ZipFile.Read(apkPath)) // Ionic.Zip API
                {
                    byte[] arsc = ReadEntry(zip, "resources.arsc");
                    if (arsc == null)
                    {
                        Debug.WriteLine("resources.arsc not present in " + Path.GetFileName(apkPath));
                        return null;
                    }

                    string iconPath = FindBestIconPath(arsc);
                    if (string.IsNullOrEmpty(iconPath))
                    {
                        Debug.WriteLine("No launcher icon raster found in resources.arsc of " + Path.GetFileName(apkPath));
                        return null;
                    }

                    Debug.WriteLine($"[ApkIconExtractor] {Path.GetFileName(apkPath)} → {iconPath}");
                    return ReadEntry(zip, iconPath); // null if listed but physically absent (e.g. in a split)
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("ApkIconExtractor failed for " + Path.GetFileName(apkPath) + ": " + ex.Message);
                return null;
            }
        }

        // ── Ionic.Zip entry reader ───────────────────────────────────

        /// <summary>
        /// Reads a ZIP entry by name into a byte array.
        /// Ionic.Zip uses forward-slash paths and is case-sensitive on most platforms.
        /// </summary>
        private static byte[] ReadEntry(ZipFile zip, string entryName)
        {
            // Ionic.Zip indexer returns null if not found (no exception)
            ZipEntry entry = zip[entryName];
            if (entry == null)
            {
                // Try case-insensitive fallback (some APKs use mixed casing)
                foreach (ZipEntry e in zip)
                {
                    if (string.Equals(e.FileName, entryName, StringComparison.OrdinalIgnoreCase))
                    {
                        entry = e;
                        break;
                    }
                }
            }

            if (entry == null)
                return null;

            using (var ms = new MemoryStream())
            {
                entry.Extract(ms); // Ionic.Zip extracts directly into a stream
                return ms.ToArray();
            }
        }

        // ── resources.arsc parser ────────────────────────────────────

        // Scans the resource table for the app's launcher icon and returns the best raster
        // file path. Prefers a real launcher-icon raster (ic_launcher/icon/...); if the icon
        // is an adaptive icon (defined only as anydpi XML, so no pixels), falls back to its
        // foreground layer, which is a real PNG/WebP. Returns null if nothing usable is found.
        private static string FindBestIconPath(byte[] d)
        {
            if (d == null || d.Length < 12 || ReadU16(d, 0) != CHUNK_TABLE)
                return null;

            ushort tableHdrSize = ReadU16(d, 2);
            uint packageCount = ReadU32(d, 8);
            int pos = tableHdrSize;

            var globalStrings = ReadStringPool(d, pos, out int globalPoolSize);
            pos += globalPoolSize;

            string bestPrimary = null; int bestPrimaryRank = 0;
            string bestForeground = null; int bestForegroundRank = 0;

            for (int p = 0; p < packageCount && pos + 8 <= d.Length; p++)
            {
                int pkgStart = pos;
                ushort pkgHdrSize = ReadU16(d, pkgStart + 2);
                uint pkgSize = ReadU32(d, pkgStart + 4);
                uint typeStrOff = ReadU32(d, pkgStart + 268);
                uint keyStrOff = ReadU32(d, pkgStart + 276);

                var typeStrings = ReadStringPool(d, pkgStart + (int)typeStrOff, out _);
                var keyStrings = ReadStringPool(d, pkgStart + (int)keyStrOff, out _);

                // Launcher icons live under the mipmap or drawable type.
                int mipmapTypeId = FindIndex(typeStrings, "mipmap") + 1;
                int drawableTypeId = FindIndex(typeStrings, "drawable") + 1;

                // Pre-resolve the key-pool indices we care about so per-entry checks are O(1).
                var primaryKeyIdx = new HashSet<int>();
                var foregroundKeyIdx = new HashSet<int>();
                for (int i = 0; i < keyStrings.Count; i++)
                {
                    string k = keyStrings[i];
                    if (string.IsNullOrEmpty(k)) continue;
                    if (Array.IndexOf(PrimaryIconKeys, k) >= 0)
                        primaryKeyIdx.Add(i);
                    else if (k.IndexOf("foreground", StringComparison.OrdinalIgnoreCase) >= 0 &&
                             (k.IndexOf("launcher", StringComparison.OrdinalIgnoreCase) >= 0 ||
                              k.IndexOf("icon", StringComparison.OrdinalIgnoreCase) >= 0))
                        foregroundKeyIdx.Add(i);
                }

                if ((mipmapTypeId == 0 && drawableTypeId == 0) ||
                    (primaryKeyIdx.Count == 0 && foregroundKeyIdx.Count == 0))
                {
                    pos = pkgStart + (int)pkgSize;
                    continue;
                }

                int chunkPos = pkgStart + pkgHdrSize;
                int pkgEnd = pkgStart + (int)pkgSize;

                while (chunkPos + 8 <= d.Length && chunkPos < pkgEnd)
                {
                    ushort chunkType = ReadU16(d, chunkPos);
                    ushort chunkHdrSize = ReadU16(d, chunkPos + 2);
                    uint chunkSize = ReadU32(d, chunkPos + 4);
                    if (chunkSize == 0) break;

                    if (chunkType == CHUNK_TYPE)
                    {
                        byte typeId = d[chunkPos + 8];
                        if (typeId == mipmapTypeId || typeId == drawableTypeId)
                            ScanTypeChunk(d, chunkPos, chunkHdrSize, globalStrings,
                                          primaryKeyIdx, foregroundKeyIdx,
                                          ref bestPrimary, ref bestPrimaryRank,
                                          ref bestForeground, ref bestForegroundRank);
                    }

                    chunkPos += (int)chunkSize;
                }

                pos = pkgStart + (int)pkgSize;
            }

            // A genuine launcher-icon raster wins; otherwise use the adaptive foreground layer.
            return bestPrimary ?? bestForeground;
        }

        // Inspects every entry in one TYPE (config) chunk, updating the best primary/foreground
        // raster seen so far. Entries are matched by their `key` pool index, not by position.
        private static void ScanTypeChunk(
            byte[] d, int chunkPos, ushort chunkHdrSize,
            List<string> globalStrings,
            HashSet<int> primaryKeyIdx, HashSet<int> foregroundKeyIdx,
            ref string bestPrimary, ref int bestPrimaryRank,
            ref string bestForeground, ref int bestForegroundRank)
        {
            byte typeFlags = d[chunkPos + 9];          // FLAG_SPARSE = 0x01
            uint entryCount = ReadU32(d, chunkPos + 12);
            uint entriesStart = ReadU32(d, chunkPos + 16);
            int density = ReadU16(d, chunkPos + 20 + 14);
            int rank = DensityRank(density);

            // The offset table starts right after the chunk header (which includes the
            // ResTable_config block). Each slot is a dense uint32 offset, or — when
            // FLAG_SPARSE is set — a packed { uint16 entryIdx; uint16 offset/4 } pair.
            int offsetTablePos = chunkPos + chunkHdrSize;
            bool sparse = (typeFlags & 0x01) != 0;

            for (int i = 0; i < (int)entryCount; i++)
            {
                uint entryOffset;
                if (sparse)
                {
                    entryOffset = (uint)ReadU16(d, offsetTablePos + i * 4 + 2) * 4;
                }
                else
                {
                    entryOffset = ReadU32(d, offsetTablePos + i * 4);
                    if (entryOffset == NO_ENTRY) continue;
                }

                int entryPos = chunkPos + (int)entriesStart + (int)entryOffset;
                if (entryPos + 8 > d.Length) continue;

                ushort entryFlags = ReadU16(d, entryPos + 2);
                int entryKey = (int)ReadU32(d, entryPos + 4);

                bool isPrimary = primaryKeyIdx.Contains(entryKey);
                bool isForeground = !isPrimary && foregroundKeyIdx.Contains(entryKey);
                if (!isPrimary && !isForeground) continue;

                if ((entryFlags & 0x0001) != 0) continue;   // complex (<adaptive-icon>) — no file path

                int valuePos = entryPos + 8;
                if (valuePos + 8 > d.Length) continue;

                byte dataType = d[valuePos + 3];
                uint dataVal = ReadU32(d, valuePos + 4);
                if (dataType != VALUE_TYPE_STRING || dataVal >= (uint)globalStrings.Count) continue;

                string path = globalStrings[(int)dataVal];
                if (string.IsNullOrEmpty(path) || !HasRasterExtension(path)) continue; // skip .xml adaptive defs

                if (isPrimary && rank > bestPrimaryRank)
                {
                    bestPrimary = path;
                    bestPrimaryRank = rank;
                }
                else if (isForeground && rank > bestForegroundRank)
                {
                    bestForeground = path;
                    bestForegroundRank = rank;
                }
            }
        }

        private static bool HasRasterExtension(string path)
        {
            foreach (var ext in RasterExtensions)
                if (path.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        // ── String pool parser ───────────────────────────────────────

        private static List<string> ReadStringPool(byte[] d, int pos, out int chunkSizeOut)
        {
            ushort headerSize = ReadU16(d, pos + 2);
            uint chunkSize = ReadU32(d, pos + 4);
            uint stringCount = ReadU32(d, pos + 8);
            uint flags = ReadU32(d, pos + 16);
            uint stringsStart = ReadU32(d, pos + 20);

            bool isUtf8 = (flags & 0x100) != 0;
            int offsetsBase = pos + headerSize;
            int stringsBase = pos + (int)stringsStart;

            var strings = new List<string>((int)stringCount);

            for (int i = 0; i < (int)stringCount; i++)
            {
                uint strOffset = ReadU32(d, offsetsBase + i * 4);
                int strPos = stringsBase + (int)strOffset;

                try
                {
                    strings.Add(isUtf8
                        ? DecodeUtf8String(d, strPos)
                        : DecodeUtf16String(d, strPos));
                }
                catch
                {
                    strings.Add(string.Empty);
                }
            }

            chunkSizeOut = (int)chunkSize;
            return strings;
        }

        private static string DecodeUtf8String(byte[] d, int pos)
        {
            if ((d[pos] & 0x80) != 0) pos += 2; else pos++;

            int byteLen;
            if ((d[pos] & 0x80) != 0)
            {
                byteLen = ((d[pos] & 0x7F) << 8) | d[pos + 1];
                pos += 2;
            }
            else
            {
                byteLen = d[pos];
                pos++;
            }

            return Encoding.UTF8.GetString(d, pos, byteLen);
        }

        private static string DecodeUtf16String(byte[] d, int pos)
        {
            int charLen;
            if ((ReadU16(d, pos) & 0x8000) != 0)
            {
                charLen = ((ReadU16(d, pos) & 0x7FFF) << 16) | ReadU16(d, pos + 2);
                pos += 4;
            }
            else
            {
                charLen = ReadU16(d, pos);
                pos += 2;
            }

            return Encoding.Unicode.GetString(d, pos, charLen * 2);
        }

        // ── Low-level helpers ────────────────────────────────────────

        private static int FindIndex(List<string> list, string value)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i] == value) return i;
            return -1;
        }

        private static ushort ReadU16(byte[] d, int pos) =>
            (ushort)(d[pos] | (d[pos + 1] << 8));

        private static uint ReadU32(byte[] d, int pos) =>
            (uint)(d[pos] | (d[pos + 1] << 8) | (d[pos + 2] << 16) | (d[pos + 3] << 24));
    }
}
