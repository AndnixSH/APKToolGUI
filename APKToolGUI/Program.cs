using APKToolGUI.Languages;
using APKToolGUI.Properties;
using APKToolGUI.Utils;
using Bluegrams.Application;
using Dark.Net;
using OSVersionExtension;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace APKToolGUI
{
    static class Program
    {
        /// <summary>
        /// Main entry point for the application.
        /// </summary>
        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        [STAThread]
        static void Main(String[] arg)
        {
            try
            {
                //Debug.WriteLine(Application.ProductName);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                if (Environment.OSVersion.Version.Major == 6)
                {
                    SetProcessDPIAware();
                }

                PortableSettingsProvider.SettingsFileName = "config.xml";
                PortableSettingsProvider.ApplyProvider(Settings.Default);

                if (arg.Length == 1)
                {
                    switch (arg[0])
                    {
                        case "ccm":
                            ExplorerContextMenuMethod(ExplorerContextMenu.Action.Create);
                            break;
                        case "rcm":
                            ExplorerContextMenuMethod(ExplorerContextMenu.Action.Remove);
                            break;
                        case "opendecfolder":
                            if (Settings.Default.Decode_UseOutputDir)
                            {
                                string outDir = Settings.Default.Decode_OutputDir;
                                if (Directory.Exists(outDir))
                                    Process.Start(outDir);
                                else
                                    MessageBox.Show(String.Format(Language.DirectoryNotExist, outDir), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                        case "opencomfolder":
                            if (Settings.Default.Build_UseOutputAppPath)
                            {
                                string outDir = Settings.Default.Build_OutputAppPath;
                                if (Directory.Exists(outDir))
                                    Process.Start(outDir);
                                else
                                    MessageBox.Show(String.Format(Language.DirectoryNotExist, outDir), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            break;
                    }
                }
                else
                {
                    if (arg.Length == 2)
                    {
                        switch (arg[0])
                        {
                            case "comapk":
                                if (!File.Exists(Path.Combine(arg[1], "AndroidManifest.xml")))
                                {
                                    MessageBox.Show(Language.NotDecompiledApk, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return;
                                }
                                break;
                        }
                    }
                    if (FilesCheck() == true)
                    {
                        TEMP_PATH = RandTempDirectory();
                        TEMP_MAIN = TempDirectory();
                        Directory.CreateDirectory(TEMP_PATH);

                        Theme theme = (Theme)Settings.Default.Theme;
                        if (IsWin10OrAbove())
                            DarkNet.Instance.SetCurrentProcessTheme(theme);

                        Form mainForm = new FormMain();

                        if (IsWin10OrAbove())
                            DarkNet.Instance.SetWindowThemeForms(mainForm, theme);

                        Application.Run(mainForm);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool IsWin10OrAbove()
        {
            // Check if the operating system is Windows 10 or above
            return OSVersion.GetOSVersion().Version.Major >= 10;
        }


        public static bool IsDarkTheme()
        {
            if (IsWin10OrAbove())
                return DarkNet.Instance.EffectiveCurrentProcessThemeIsDark;
            else if (Settings.Default.Theme == 2)
                return true;
            return false;
        }

        public static void SetLanguage()
        {
            String settingsCulture = Settings.Default.Culture;
            if (settingsCulture.Equals("Auto"))
            {
                // Let .NET handle the resource fallback process.
                // It will automatically use the system's language if a satellite assembly is available,
                // otherwise it will fall back to the neutral language defined in the main assembly (English).
                return;
            }

            try
            {
                System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.GetCultureInfo(settingsCulture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            }
            catch (System.Globalization.CultureNotFoundException ex)
            {
                Debug.WriteLine($"[Program] Invalid culture '{settingsCulture}': {ex.Message}");
                // Fall back to system default culture
            }
            catch (ArgumentException ex)
            {
                Debug.WriteLine($"[Program] Failed to set culture: {ex.Message}");
            }
        }

        private static bool FilesCheck()
        {
            // File verification
            List<String> missigFiles = MissingFilesCheck();
            if (missigFiles.Count > 0)
            {
                StringBuilder filesBuilder = new StringBuilder();
                filesBuilder.AppendLine();
                foreach (String file in missigFiles)
                {
                    filesBuilder.AppendLine(file);
                }
                MessageBox.Show(Language.RequiredFilesMissing + filesBuilder.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Application.Exit();
                return false;
            }
            else
                return true;
        }

        private static List<String> MissingFilesCheck()
        {
            List<String> missingFiles = new List<string>();
            String[] fileList = new String[]{
                APKTOOL_PATH,
                ZIPALIGN_PATH,
                APKSIGNER_PATH,
                BAKSMALI_PATH,
                SMALI_PATH,
                AAPT_PATH,
                AAPT2_PATH,
                SIGNAPK_KEYPRIVATE,
                SIGNAPK_KEYPUBLIC,
                APKEDITOR_PATH,
                ADB_PATH,
                ADBWINAPI_PATH,
                ADBWINUSBAPI_PATH,
                LIBWINP_PATH,
            };
            for (int i = 0; i < fileList.Length; i++)
                if (!File.Exists(fileList[i]))
                    missingFiles.Add(Path.GetFileName(fileList[i]));
            return missingFiles;
        }

        private static void ExplorerContextMenuMethod(ExplorerContextMenu.Action action)
        {
            ExplorerContextMenu.Status status = null;

            switch (action)
            {
                case ExplorerContextMenu.Action.Create:
                    status = ExplorerContextMenu.Create();
                    break;
                case ExplorerContextMenu.Action.Remove:
                    status = ExplorerContextMenu.Remove();
                    break;
                default:
                    return;
            }

            if (status.Result)
                MessageBox.Show(status.Message, Application.ProductName);
            else
                MessageBox.Show(status.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        public static string GetPortablePath(string path)
        {
            string startupPath = Application.StartupPath + Path.DirectorySeparatorChar;
            if (path.Contains(startupPath))
                return path.Replace(startupPath, String.Empty);
            else
                return path;
        }

        public static string TempDirectory()
        {
            //Generate new every new instance to avoid conflict
            //We want to keep obfuscated path short as possible to prevent long path error
            if (Settings.Default.UseCustomTempDir)
                return Path.Combine(Settings.Default.TempDir);
            else
                return Path.Combine(LOCAL_APPDATA_PATH, ASSEMBLY_NAME, "Temp");
        }

        public static string RandTempDirectory()
        {
            return Path.Combine(TempDirectory(), StringExt.RandStrWithCaps(5));
        }

        public static string ASSEMBLY_NAME { get { return AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Name; } }
        public static string TEMP_PATH { get; set; }
        public static string TEMP_MAIN { get; set; }
        public static string LOCAL_APPDATA_PATH { get { return Environment.GetEnvironmentVariable("LocalAppData"); } }
        public static string APP_PATH { get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); } }
        public static string RES_PATH { get { return Path.Combine(APP_PATH, "Resources"); } }
        public static string APKTOOL_PATH { get { return Path.Combine(RES_PATH, "apktool.jar"); } }
        public static string APKSIGNER_PATH { get { return Path.Combine(RES_PATH, "apksigner.jar"); } }
        public static string BAKSMALI_PATH { get { return Path.Combine(RES_PATH, "baksmali.jar"); } }
        public static string SMALI_PATH { get { return Path.Combine(RES_PATH, "smali.jar"); } }
        public static string SIGNAPK_KEYPRIVATE { get { return Path.Combine(RES_PATH, "testkey.pk8"); } }
        public static string SIGNAPK_KEYPUBLIC { get { return Path.Combine(RES_PATH, "testkey.x509.pem"); } }
        public static string ZIPALIGN_PATH { get { return Path.Combine(RES_PATH, "zipalign.exe"); } }
        public static string AAPT_PATH { get { return Path.Combine(RES_PATH, "aapt.exe"); } }
        public static string AAPT2_PATH { get { return Path.Combine(RES_PATH, "aapt2.exe"); } }
        public static string APKEDITOR_PATH { get { return Path.Combine(RES_PATH, "apkeditor.jar"); } }
        public static string ADB_PATH { get { return Path.Combine(RES_PATH, "adb.exe"); } }
        public static string ADBWINAPI_PATH { get { return Path.Combine(RES_PATH, "AdbWinApi.dll"); } }
        public static string ADBWINUSBAPI_PATH { get { return Path.Combine(RES_PATH, "AdbWinUsbApi.dll"); } }
        public static string LIBWINP_PATH { get { return Path.Combine(RES_PATH, "libwinpthread-1.dll"); } }
        public static string FRAMEWORK_DIR { get { return Path.Combine(LOCAL_APPDATA_PATH, "apktool", "framework"); } }
        public static string STANDALONE_FRAMEWORK_DIR { get { return Path.Combine(LOCAL_APPDATA_PATH, ASSEMBLY_NAME, "framework"); } }
    }
}
