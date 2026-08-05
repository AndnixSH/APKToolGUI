using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace APKToolGUI.ApkTool
{
    public static class KeyProfileManager
    {
        public static string ProfilesFilePath => Program.SAVED_KEYS_PATH;

        public static List<KeyProfile> LoadAll()
        {
            var list = new List<KeyProfile>();
            try
            {
                string path = ProfilesFilePath;
                if (!File.Exists(path)) return list;

                string json = File.ReadAllText(path, Encoding.UTF8);
                list = DeserializeProfiles(json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[KeyProfileManager] Failed to load profiles: {ex.Message}");
            }
            return list;
        }

        public static void SaveAll(List<KeyProfile> profiles)
        {
            try
            {
                string dir = Path.GetDirectoryName(ProfilesFilePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string json = SerializeProfiles(profiles);
                File.WriteAllText(ProfilesFilePath, json, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[KeyProfileManager] Failed to save profiles: {ex.Message}");
            }
        }

        public static void Upsert(KeyProfile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.OutputPath)) return;

            var profiles = LoadAll();
            int idx = profiles.FindIndex(p => string.Equals(p.ProfileName, profile.ProfileName, StringComparison.OrdinalIgnoreCase)
                                            || string.Equals(p.OutputPath, profile.OutputPath, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
                profiles[idx] = profile;
            else
                profiles.Add(profile);

            SaveAll(profiles);
        }

        public static bool Delete(string profileName)
        {
            var profiles = LoadAll();
            int removed = profiles.RemoveAll(p => string.Equals(p.ProfileName, profileName, StringComparison.OrdinalIgnoreCase));
            if (removed > 0)
            {
                SaveAll(profiles);
                return true;
            }
            return false;
        }

        #region DPAPI Password Encryption

        public static string EncryptPassword(string rawPass)
        {
            if (string.IsNullOrEmpty(rawPass)) return "";
            try
            {
                byte[] rawBytes = Encoding.UTF8.GetBytes(rawPass);
                byte[] encrypted = ProtectedData.Protect(rawBytes, null, DataProtectionScope.CurrentUser);
                return "DPAPI:" + Convert.ToBase64String(encrypted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[KeyProfileManager] DPAPI encrypt error: {ex.Message}");
                return rawPass;
            }
        }

        public static string DecryptPassword(string encPass)
        {
            if (string.IsNullOrEmpty(encPass)) return "";
            if (!encPass.StartsWith("DPAPI:")) return encPass; // Fallback if plain text

            try
            {
                string b64 = encPass.Substring("DPAPI:".Length);
                byte[] encrypted = Convert.FromBase64String(b64);
                byte[] decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[KeyProfileManager] DPAPI decrypt error: {ex.Message}");
                return "";
            }
        }

        #endregion

        #region Custom JSON Serialization (No External Dependencies)

        private static string SerializeProfiles(List<KeyProfile> profiles)
        {
            var sb = new StringBuilder();
            sb.AppendLine("[");
            for (int i = 0; i < profiles.Count; i++)
            {
                var p = profiles[i];
                sb.AppendLine("  {");
                sb.AppendLine($"    \"ProfileName\": \"{EscapeJson(p.ProfileName)}\",");
                sb.AppendLine($"    \"KeyType\": \"{EscapeJson(p.KeyType)}\",");
                sb.AppendLine($"    \"OutputPath\": \"{EscapeJson(p.OutputPath)}\",");
                sb.AppendLine($"    \"Alias\": \"{EscapeJson(p.Alias)}\",");
                sb.AppendLine($"    \"StorePass\": \"{EscapeJson(EncryptPassword(p.StorePass))}\",");
                sb.AppendLine($"    \"KeyPass\": \"{EscapeJson(EncryptPassword(p.KeyPass))}\",");
                sb.AppendLine($"    \"Algorithm\": \"{EscapeJson(p.Algorithm)}\",");
                sb.AppendLine($"    \"KeySize\": {p.KeySize},");
                sb.AppendLine($"    \"ValidityYears\": {p.ValidityYears},");
                sb.AppendLine($"    \"CN\": \"{EscapeJson(p.CN)}\",");
                sb.AppendLine($"    \"OU\": \"{EscapeJson(p.OU)}\",");
                sb.AppendLine($"    \"O\": \"{EscapeJson(p.O)}\",");
                sb.AppendLine($"    \"L\": \"{EscapeJson(p.L)}\",");
                sb.AppendLine($"    \"ST\": \"{EscapeJson(p.ST)}\",");
                sb.AppendLine($"    \"C\": \"{EscapeJson(p.C)}\",");
                sb.AppendLine($"    \"CreatedAt\": \"{p.CreatedAt:o}\"");
                sb.Append("  }");
                if (i < profiles.Count - 1) sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("]");
            return sb.ToString();
        }

        private static List<KeyProfile> DeserializeProfiles(string json)
        {
            var result = new List<KeyProfile>();
            if (string.IsNullOrWhiteSpace(json)) return result;

            // Simple regex parser for array of objects
            var objectMatches = Regex.Matches(json, @"\{[^{}]*\}");
            foreach (Match match in objectMatches)
            {
                string objStr = match.Value;
                var profile = new KeyProfile();

                profile.ProfileName = GetJsonVal(objStr, "ProfileName");
                profile.KeyType     = GetJsonVal(objStr, "KeyType", "JKS");
                profile.OutputPath  = GetJsonVal(objStr, "OutputPath");
                profile.Alias       = GetJsonVal(objStr, "Alias");
                profile.StorePass   = DecryptPassword(GetJsonVal(objStr, "StorePass"));
                profile.KeyPass     = DecryptPassword(GetJsonVal(objStr, "KeyPass"));
                profile.Algorithm   = GetJsonVal(objStr, "Algorithm", "RSA");
                
                if (int.TryParse(GetJsonVal(objStr, "KeySize"), out int ks)) profile.KeySize = ks;
                if (int.TryParse(GetJsonVal(objStr, "ValidityYears"), out int val)) profile.ValidityYears = val;

                profile.CN = GetJsonVal(objStr, "CN");
                profile.OU = GetJsonVal(objStr, "OU");
                profile.O  = GetJsonVal(objStr, "O");
                profile.L  = GetJsonVal(objStr, "L");
                profile.ST = GetJsonVal(objStr, "ST");
                profile.C  = GetJsonVal(objStr, "C", "IN");

                if (DateTime.TryParse(GetJsonVal(objStr, "CreatedAt"), out DateTime dt))
                    profile.CreatedAt = dt;

                result.Add(profile);
            }

            return result;
        }

        private static string GetJsonVal(string jsonObj, string key, string defaultVal = "")
        {
            var match = Regex.Match(jsonObj, $"\"{key}\"\\s*:\\s*(?:\"([^\"]*)\"|([0-9]+))");
            if (match.Success)
            {
                return match.Groups[1].Success ? UnescapeJson(match.Groups[1].Value) : match.Groups[2].Value;
            }
            return defaultVal;
        }

        private static string EscapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private static string UnescapeJson(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("\\r", "\r").Replace("\\n", "\n").Replace("\\\"", "\"").Replace("\\\\", "\\");
        }

        #endregion
    }
}
