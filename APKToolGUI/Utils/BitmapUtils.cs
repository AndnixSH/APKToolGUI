using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APKToolGUI.Utils
{
    internal class BitmapUtils
    {
        public static Bitmap LoadBitmap(string path)
        {
            if (File.Exists(path))
            {
                // open file in read only mode
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                // get a binary reader for the file stream
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    // copy the content of the file into a memory stream
                    var memoryStream = new MemoryStream(reader.ReadBytes((int)stream.Length));
                    // make a new Bitmap object the owner of the MemoryStream
                    return new Bitmap(memoryStream);
                }
            }
            else
            {
                return null;
            }
        }

        // Decodes raw image bytes (e.g. an APK launcher icon resolved in memory) into a Bitmap.
        // Returns a standalone copy so the backing MemoryStream can be disposed immediately and
        // the result remains usable for saving. Returns null on null/empty input or a format
        // GDI+ can't decode (e.g. WebP).
        public static Bitmap LoadBitmap(byte[] data)
        {
            if (data == null || data.Length == 0)
                return null;
            try
            {
                using (var memoryStream = new MemoryStream(data))
                using (var temp = new Bitmap(memoryStream))
                {
                    return new Bitmap(temp);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
