

using APKToolGUI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace APKToolGUI.ApkTool
{
    public class ApkFixer
    {
        public static bool FixAndroidManifest(string decompilePath)
        {
            string manifestPath = Path.Combine(decompilePath, "AndroidManifest.xml");
            if (!File.Exists(manifestPath))
                return false;

            try
            {
                string manifestText = File.ReadAllText(manifestPath);
                manifestText = manifestText.Replace("\\ ", "\\u003");
                manifestText = manifestText.Replace("android:isSplitRequired=\"true\"", "");
                manifestText = manifestText.Replace("android:extractNativeLibs=\"false\"", "");
                manifestText = manifestText.Replace("android:useEmbeddedDex=\"true\"", "");
                manifestText = manifestText.Replace("android:manageSpace=\"true\"", "");
                manifestText = manifestText.Replace("android:localeConfig=\"@xml/locales_config\"", "");
                manifestText = manifestText.Replace("STAMP_TYPE_DISTRIBUTION_APK", "STAMP_TYPE_STANDALONE_APK");
                manifestText = manifestText.Replace("android:requiredSplitTypes=\"(.*?)\"", "");
                manifestText = manifestText.Replace("android:splitTypes=\"(.*?)\"", "");

                File.WriteAllText(manifestPath, manifestText);
                return true;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Failed to fix AndroidManifest.xml: {ex.Message}");
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Access denied to AndroidManifest.xml: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Unexpected error fixing AndroidManifest.xml: {ex.Message}");
                return false;
            }
        }

        public static bool FixApktoolYml(string decompilePath)
        {
            string ymlPath = Path.Combine(decompilePath, "apktool.yml");
            if (!File.Exists(ymlPath))
                return false;

            try
            {
                string yml = File.ReadAllText(ymlPath);
                yml = yml.Replace("sparseResources: true", "sparseResources: false");

                File.WriteAllText(ymlPath, yml);
                return true;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Failed to fix apktool.yml: {ex.Message}");
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Access denied to apktool.yml: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Unexpected error fixing apktool.yml: {ex.Message}");
                return false;
            }
        }

        public static bool RemoveApkToolDummies(string path)
        {
            string resPath = Path.Combine(path, "res", "values");
            if (!Directory.Exists(resPath))
                return false;

            try
            {
                DirectoryUtils.ReplaceinFilesRegex(resPath, "(.*(?:APKTOOL_DUMMY).*)", "");
                return true;
            }
            catch (IOException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Failed to remove APKTOOL_DUMMY: {ex.Message}");
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Access denied while removing APKTOOL_DUMMY: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ApkFixer] Unexpected error removing APKTOOL_DUMMY: {ex.Message}");
                return false;
            }
        }
    }
}
