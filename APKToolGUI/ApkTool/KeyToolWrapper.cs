using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using APKToolGUI.Properties;
using Java;

namespace APKToolGUI.ApkTool
{
    public class KeyToolWrapper
    {
        public static string FindKeyTool()
        {
            // Priority 1: Saved custom keytool path
            if (!string.IsNullOrWhiteSpace(Settings.Default.KeyTool_CustomPath) && File.Exists(Settings.Default.KeyTool_CustomPath))
            {
                return Settings.Default.KeyTool_CustomPath;
            }

            // Priority 2: Same directory as configured Java executable
            string javaPath = JavaUtils.GetJavaPath();
            if (!string.IsNullOrWhiteSpace(javaPath))
            {
                string javaDir = Path.GetDirectoryName(javaPath);
                if (!string.IsNullOrEmpty(javaDir))
                {
                    string candidate = Path.Combine(javaDir, "keytool.exe");
                    if (File.Exists(candidate))
                        return candidate;
                }
            }

            // Priority 3: JAVA_HOME environment variable
            string javaHome = Environment.GetEnvironmentVariable("JAVA_HOME");
            if (!string.IsNullOrWhiteSpace(javaHome))
            {
                string candidate = Path.Combine(javaHome, "bin", "keytool.exe");
                if (File.Exists(candidate))
                    return candidate;
            }

            // Priority 4: Search system PATH using `where keytool`
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = "where";
                    p.StartInfo.Arguments = "keytool";
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.RedirectStandardOutput = true;
                    p.Start();
                    string output = p.StandardOutput.ReadToEnd();
                    p.WaitForExit();

                    if (!string.IsNullOrWhiteSpace(output))
                    {
                        string[] lines = output.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string line in lines)
                        {
                            if (File.Exists(line.Trim()))
                                return line.Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[KeyToolWrapper] Where command failed: {ex.Message}");
            }

            return null;
        }

        public static string BuildDName(KeyProfile profile)
        {
            var parts = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(profile.CN)) parts.Append($"CN={EscapeDName(profile.CN)}, ");
            if (!string.IsNullOrWhiteSpace(profile.OU)) parts.Append($"OU={EscapeDName(profile.OU)}, ");
            if (!string.IsNullOrWhiteSpace(profile.O))  parts.Append($"O={EscapeDName(profile.O)}, ");
            if (!string.IsNullOrWhiteSpace(profile.L))  parts.Append($"L={EscapeDName(profile.L)}, ");
            if (!string.IsNullOrWhiteSpace(profile.ST)) parts.Append($"ST={EscapeDName(profile.ST)}, ");
            if (!string.IsNullOrWhiteSpace(profile.C))  parts.Append($"C={EscapeDName(profile.C)}, ");

            string dname = parts.ToString().TrimEnd(' ', ',');
            return string.IsNullOrEmpty(dname) ? "CN=Android App" : dname;
        }

        private static string EscapeDName(string val)
        {
            if (string.IsNullOrEmpty(val)) return "";
            return val.Replace(",", "\\,").Replace("=", "\\=");
        }

        public static string BuildCommandArgs(KeyProfile profile)
        {
            string alias = string.IsNullOrWhiteSpace(profile.Alias) ? "key0" : profile.Alias;
            string keyPass = string.IsNullOrWhiteSpace(profile.KeyPass) ? profile.StorePass : profile.KeyPass;
            int days = Math.Max(1, profile.ValidityYears) * 365;

            string keyAlg = string.IsNullOrWhiteSpace(profile.Algorithm) ? "RSA" : profile.Algorithm;
            int keySize = profile.KeySize > 0 ? profile.KeySize : (keyAlg == "EC" ? 256 : 2048);

            string dname = BuildDName(profile);

            return $"-genkeypair -v -keystore \"{profile.OutputPath}\" -alias \"{alias}\" -keyalg {keyAlg} -keysize {keySize} -validity {days} -storepass \"{profile.StorePass}\" -keypass \"{keyPass}\" -dname \"{dname}\"";
        }

        public static async Task<(int ExitCode, string Output)> GenerateKeyAsync(KeyProfile profile, string keyToolPath)
        {
            if (string.IsNullOrWhiteSpace(keyToolPath) || !File.Exists(keyToolPath))
                return (-1, "keytool.exe binary not found.");

            string args = BuildCommandArgs(profile);

            return await Task.Run(() =>
            {
                var sbOutput = new StringBuilder();
                try
                {
                    using (Process proc = new Process())
                    {
                        proc.StartInfo.FileName = keyToolPath;
                        proc.StartInfo.Arguments = args;
                        proc.StartInfo.CreateNoWindow = true;
                        proc.StartInfo.UseShellExecute = false;
                        proc.StartInfo.RedirectStandardOutput = true;
                        proc.StartInfo.RedirectStandardError = true;

                        proc.OutputDataReceived += (s, e) => { if (e.Data != null) sbOutput.AppendLine(e.Data); };
                        proc.ErrorDataReceived += (s, e) => { if (e.Data != null) sbOutput.AppendLine(e.Data); };

                        proc.Start();
                        proc.BeginOutputReadLine();
                        proc.BeginErrorReadLine();
                        proc.WaitForExit();

                        return (proc.ExitCode, sbOutput.ToString());
                    }
                }
                catch (Exception ex)
                {
                    return (-1, "Failed to launch keytool: " + ex.Message);
                }
            });
        }
    }
}
