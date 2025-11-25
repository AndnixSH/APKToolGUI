using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Java;
using APKToolGUI.Languages;
using APKToolGUI.Properties;
using APKToolGUI.ApkTool;
using APKToolGUI.Utils;
using System.Threading.Tasks;
using APKToolGUI.Handlers;
using Microsoft.WindowsAPICodePack.Taskbar;
using System.Media;
using System.Linq;
using APKToolGUI.Controls;
using Ionic.Zip;
using System.Text.RegularExpressions;

namespace APKToolGUI
{
    public partial class FormMain : Form
    {
        internal Adb adb;
        internal ApkEditor apkeditor;
        internal Apktool apktool;
        internal Signapk signapk;
        internal Baksmali baksmali;
        internal Smali smali;
        internal Zipalign zipalign;
        internal UpdateChecker updateCheker;
        internal AaptParser aapt;

        private bool IgnoreOutputDirContextMenu;
        private bool isRunning;

        private string javaPath;

        private Stopwatch stopwatch = new Stopwatch();
        private string lastStartedDate;

        private Image previousApkIcon;

        internal static FormMain Instance { get; private set; }

        public FormMain()
        {
            Instance = this;

            Program.SetLanguage();

            InitializeComponent();

            if (Program.IsDarkTheme())
                DarkTheme.SetTheme(Controls, this);

            Text += " - v" + ProductVersion;
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);

            aapt = new AaptParser();

            if (!File.Exists(Settings.Default.Decode_InputAppPath))
                Settings.Default.Decode_InputAppPath = "";
            if (!Directory.Exists(Settings.Default.Build_InputDir))
                Settings.Default.Build_InputDir = "";
            if (!File.Exists(Settings.Default.Sign_InputFile))
                Settings.Default.Sign_InputFile = "";
            if (!File.Exists(Settings.Default.Zipalign_InputFile))
                Settings.Default.Zipalign_InputFile = "";

            if (!File.Exists(Settings.Default.Sign_PrivateKey) || String.IsNullOrEmpty(Settings.Default.Sign_PrivateKey))
                Settings.Default.Sign_PrivateKey = Program.SIGNAPK_KEYPRIVATE;
            if (!File.Exists(Settings.Default.Sign_PublicKey) || String.IsNullOrEmpty(Settings.Default.Sign_PublicKey))
                Settings.Default.Sign_PublicKey = Program.SIGNAPK_KEYPUBLIC;

            int v1 = (schemev1ComboBox.Items.Count + 1 > Settings.Default.Sign_Schemev1) ? Settings.Default.Sign_Schemev1 : 0;
            schemev1ComboBox.SelectedIndex = v1;
            Settings.Default.Sign_Schemev1 = v1;

            int v2 = (schemev2ComboBox.Items.Count + 1 > Settings.Default.Sign_Schemev2) ? Settings.Default.Sign_Schemev2 : 0;
            schemev2ComboBox.SelectedIndex = v2;
            Settings.Default.Sign_Schemev2 = v2;

            int v3 = (schemev3ComboBox.Items.Count + 1 > Settings.Default.Sign_Schemev3) ? Settings.Default.Sign_Schemev3 : 0;
            schemev3ComboBox.SelectedIndex = v3;
            Settings.Default.Sign_Schemev3 = v3;

            int v4 = (schemev4ComboBox.Items.Count + 1 > Settings.Default.Sign_Schemev4) ? Settings.Default.Sign_Schemev4 : 2;
            schemev4ComboBox.SelectedIndex = v4;
            Settings.Default.Sign_Schemev4 = v4;

            int overrideAbi = (overrideAbiComboBox.Items.Count + 1 > Settings.Default.Adb_OverrideAbi) ? Settings.Default.Adb_OverrideAbi : 0;
            overrideAbiComboBox.SelectedIndex = overrideAbi;
            Settings.Default.Adb_OverrideAbi = overrideAbi;

            useAPKEditorForDecompilingItem.Checked = Settings.Default.UseApkeditor;

            new DecodeControlEventHandlers(this);
            new BuildControlEventHandlers(this);
            new SignControlEventHandlers(this);
            new ZipalignControlEventHandlers(this);
            new FrameworkControlEventHandlers(this);
            new BaksmaliControlEventHandlers(this);
            new SmaliControlEventHandlers(this);
            new AdbControlEventHandlers(this);
            new DragDropHandlers(this);
            new ApkinfoControlEventHandlers(this);
            new MainWindowEventHandlers(this);
            new MenuItemHandlers(this);
            new TaskBarJumpList(Handle);
        }

        #region Context menu args
        private async void RunCmdArgs()
        {
            try
            {
                if (Environment.GetCommandLineArgs().Length == 3)
                {
                    if (Settings.Default.IgnoreOutputDirContextMenu)
                        IgnoreOutputDirContextMenu = true;

                    string file = Environment.GetCommandLineArgs()[2];
                    switch (Environment.GetCommandLineArgs()[1])
                    {
                        case "decapk":
                            if (file.ContainsAny(".xapk", ".zip", ".apks", ".apkm"))
                            {
                                if (await MergeAndDecompile(file) == 0)
                                    Close();
                            }
                            else
                            {
                                if (await Decompile(file) == 0)
                                    Close();
                            }
                            break;
                        case "comapk":
                            if (await Build(file) == 0)
                                Close();
                            break;
                        case "sign":
                            if (await Sign(file) == 0)
                                Close();
                            break;
                        case "zipalign":
                            if (await Align(file) == 0)
                                Close();
                            break;
                        case "baksmali":
                            if (await Baksmali(file) == 0)
                                Close();
                            break;
                        case "smali":
                            if (await Smali(file) == 0)
                                Close();
                            break;
                        case "viewinfo":
                            tabControlMain.SelectedIndex = 1;
                            await GetApkInfo(file);
                            break;
                        default:
                            IgnoreOutputDirContextMenu = false;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ToLog(ApktoolEventType.Error, ex.Message);
            }
        }
        #endregion

        #region Get APK Info
        internal async Task GetApkInfo(string file)
        {
            if (!File.Exists(file))
                return;

            ToLog(ApktoolEventType.None, Language.ParsingApkInfo);
            ToStatus(Language.ParsingApkInfo, Resources.waiting);

            try
            {
                string splitPath = Path.Combine(Program.TEMP_PATH, "SplitInfo");
                
                // Parse APK in background
                var parseResult = await ParseApkInBackgroundAsync(file, splitPath);

                if (parseResult.Success)
                {
                    // UI update is automatically executed on UI thread
                    UpdateApkInfoUI(parseResult);
                    
                    // Get signature info in background
                    var signature = await Task.Run(() => signapk.GetSignature(parseResult.ActualFilePath));
                    
                    // Update signature info UI
                    InvokeOnUIThread(() => sigTxtBox.Text = signature);
                }

                ToLog(ApktoolEventType.Success, Language.Done);
                ToStatus(Language.Done, Resources.done);
            }
            catch (Exception ex)
            {
#if DEBUG
                ToLog(ApktoolEventType.Warning, Language.ErrorGettingApkInfo + "\n" + ex.ToString());
#else
                ToLog(ApktoolEventType.Warning, Language.ErrorGettingApkInfo);
#endif
            }
        }

        private async Task<ApkParseResult> ParseApkInBackgroundAsync(string file, string splitPath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    DirectoryUtils.Delete(splitPath);
                    List<string> archList = new List<string>();
                    string actualFile = file;

                    if (file.ContainsAny(".xapk", ".zip", ".apks", ".apkm"))
                    {
                        Directory.CreateDirectory(splitPath);

                        using (ZipFile zipDest = ZipFile.Read(file))
                        {
                            bool mainApkFound = false;

                            foreach (ZipEntry entry in zipDest.Entries)
                            {
                                if (!mainApkFound && !entry.FileName.Contains("config.") && entry.FileName.EndsWith(".apk"))
                                {
                                    Debug.WriteLine("Found main APK: " + entry.FileName);
                                    string extractPath = Path.Combine(splitPath, entry.FileName);
                                    Directory.CreateDirectory(Path.GetDirectoryName(extractPath));
                                    entry.Extract(splitPath, ExtractExistingFileAction.OverwriteSilently);
                                    actualFile = extractPath;
                                    mainApkFound = true;
                                }

                                if (entry.FileName.Contains("lib/armeabi-v7a") && !archList.Contains("armeabi-v7a"))
                                    archList.Add("armeabi-v7a");
                                if (entry.FileName.Contains("lib/arm64-v8a") && !archList.Contains("arm64-v8a"))
                                    archList.Add("arm64-v8a");
                                if (entry.FileName.Contains("lib/x86") && !archList.Contains("x86"))
                                    archList.Add("x86");
                                if (entry.FileName.Contains("lib/x86_64") && !archList.Contains("x86_64"))
                                    archList.Add("x86_64");
                            }
                        }
                    }

                    var aaptParser = new AaptParser();
                    var parsed = aaptParser.Parse(actualFile);

                    DirectoryUtils.Delete(splitPath);

                    return new ApkParseResult
                    {
                        Success = parsed,
                        Aapt = aaptParser,
                        Architecture = string.Join(", ", archList),
                        ActualFilePath = actualFile
                    };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error parsing APK: {ex.Message}");
                    DirectoryUtils.Delete(splitPath);
                    return new ApkParseResult { Success = false };
                }
            });
        }

        private void UpdateApkInfoUI(ApkParseResult result)
        {
            // Explicitly dispose previous image
            if (previousApkIcon != null)
            {
                previousApkIcon.Dispose();
                previousApkIcon = null;
                Debug.WriteLine("[FormMain] Disposed previous APK icon");
            }

            // Remove PictureBox image reference
            if (apkIconPicBox.Image != null)
            {
                apkIconPicBox.Image = null;
            }

            fileTxtBox.Text = result.Aapt.ApkFile;
            appTxtBox.Text = result.Aapt.AppName;
            packNameTxtBox.Text = result.Aapt.PackageName;
            verTxtBox.Text = result.Aapt.VersionName;
            buildTxtBox.Text = result.Aapt.VersionCode;
            minSdkTxtBox.Text = result.Aapt.MinSdkVersionDetailed;
            targetSdkTxtBox.Text = result.Aapt.TargetSdkVersionDetailed;
            screenTxtBox.Text = result.Aapt.Screens;
            densityTxtBox.Text = result.Aapt.Densities;
            permTxtBox.Text = result.Aapt.Permissions;
            localsTxtBox.Text = result.Aapt.Locales;
            fullInfoTextBox.Text = result.Aapt.FullInfo;
            launchActivityTxtBox.Text = result.Aapt.LaunchableActivity;

            if (!String.IsNullOrEmpty(result.Aapt.NativeCode))
                archSdkTxtBox.Text = result.Aapt.NativeCode;
            else
                archSdkTxtBox.Text = result.Architecture;

            // Load new image and save reference
            previousApkIcon = BitmapUtils.LoadBitmap(result.Aapt.GetIcon(result.ActualFilePath));
            apkIconPicBox.Image = previousApkIcon;
            
            sigTxtBox.Text = "Loading...";
        }

        private class ApkParseResult
        {
            public bool Success { get; set; }
            public AaptParser Aapt { get; set; }
            public string Architecture { get; set; }
            public string ActualFilePath { get; set; }
        }
        #endregion

        #region Update checker
        private void InitializeUpdateChecker()
        {
            updateCheker = new UpdateChecker("https://repo.andnixsh.com/tools/APKToolGUI/version.txt", Version.Parse(Application.ProductVersion));
            updateCheker.Completed += new RunWorkerCompletedEventHandler(updateCheker_Completed);
        }

        private void updateCheker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is UpdateChecker.Result)
            {
                UpdateChecker.Result result = (UpdateChecker.Result)e.Result;

                switch (result.State)
                {
                    case UpdateChecker.State.NeedUpdate:
                        if (MessageBox.Show(Language.UpdateNewVersion + "\n\n" + result.Changelog, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                            Process.Start("https://repo.andnixsh.com/tools/APKToolGUI/APKToolGUI.zip");
                        break;
                    case UpdateChecker.State.NoUpdate:
                        if (!result.Silently)
                            MessageBox.Show(Language.UpdateNoUpdates, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case UpdateChecker.State.Error:
                        if (!result.Silently)
                            MessageBox.Show(Language.ErrorUpdateChecking + " " + Environment.NewLine + result.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }

                Settings.Default.LastUpdateCheck = DateTime.Now;
            }
        }
        #endregion

        #region Log & Status
        internal void ToStatus(string message, Image statusImage)
        {
            BeginInvokeOnUIThread(() =>
            {
                toolStripStatusLabelStateText.Text = message.Replace("\n", "").Replace("\r", "");
                toolStripStatusLabelStateImage.Image = statusImage;
            });
        }

        internal void ToLog(string time, string message, Color backColor)
        {
            Debug.WriteLine(time + " " + message);

            InvokeOnUIThread(() =>
            {
                logTxtBox.SelectionColor = backColor;
                logTxtBox.AppendText(time + " " + message + Environment.NewLine);
            });
        }

        internal void ToLog(ApktoolEventType eventType, string message)
        {
            if (String.IsNullOrWhiteSpace(message) || message.Contains("_JAVA_OPTIONS"))
                return;

            Color color = Color.Black;

            switch (eventType)
            {
                case ApktoolEventType.None:
                    if (Program.IsDarkTheme())
                        color = Color.White;
                    break;
                case ApktoolEventType.Success:
                    if (Program.IsDarkTheme())
                        color = Color.LightGreen;
                    else
                        color = Color.DarkGreen;
                    break;
                case ApktoolEventType.Infomation:
                    if (Program.IsDarkTheme())
                        color = Color.LightBlue;
                    else
                        color = Color.Blue;
                    break;
                case ApktoolEventType.Error:
                    if (Program.IsDarkTheme())
                        color = Color.LightPink;
                    else
                        color = Color.Red;
                    break;
                case ApktoolEventType.Warning:
                    if (Program.IsDarkTheme())
                        color = Color.DarkOrange;
                    else
                        color = Color.Orange;
                        color = Color.White;
                    break;
                case ApktoolEventType.Unknown:
                    if (Program.IsDarkTheme())
                        color = Color.White;
                    break;
            }

            ToLog(DateTime.Now.ToString("[HH:mm:ss]"), message, color);
        }

        internal void Running(string msg)
        {
            Invoke(new Action(delegate ()
            {
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate, Handle);
                progressBar.Style = ProgressBarStyle.Marquee;
                progressBar.Visible = true;
                ActionButtonsEnabled = false;
                ClearLog();
            }));

            isRunning = true;
            stopwatch.Reset();
            stopwatch.Start();
            lastStartedDate = DateTime.Now.ToString("HH:mm:ss");

            ToLog(ApktoolEventType.Infomation, "=====[ " + msg + " ]=====");
            ToStatus(msg, Resources.waiting);
        }

        internal void Done(string msg = null)
        {
            isRunning = false;

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            ToLog(ApktoolEventType.Success, "=====[ " + Language.AllDone + " ]=====");
            if (msg != null)
                ToLog(ApktoolEventType.Success, msg);
            ToLog(ApktoolEventType.None, String.Format(Language.TimeStarted, lastStartedDate));
            ToLog(ApktoolEventType.None, String.Format(Language.TimeEnded, DateTime.Now.ToString("HH:mm:ss") + " (" + ts.ToString("mm\\:ss") + ")"));

            if (Settings.Default.PlaySoundWhenDone)
                SystemSounds.Beep.Play();

            TaskbarManager.Instance.SetProgressValue(1, 1);
            if (statusStrip1.InvokeRequired)
                statusStrip1.BeginInvoke(new Action(delegate { progressBar.Style = ProgressBarStyle.Continuous; }));
            else
                progressBar.Style = ProgressBarStyle.Continuous;

            Invoke(new Action(delegate ()
            {
                progressBar.Visible = false;
            }));

            ActionButtonsEnabled = true;

            ToStatus(Language.Done, Resources.done);
        }

        internal void Error(Exception ex)
        {
#if DEBUG
            Error(ex.ToString());
#else
            Error(ex.Message);
#endif
        }

        internal void Error(string msg, string status = null)
        {
            isRunning = false;

            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            ToLog(ApktoolEventType.Error, "=====[ " + Language.Error + " ]=====");
            ToLog(ApktoolEventType.Error, msg);
            ToLog(ApktoolEventType.None, "Time started: " + lastStartedDate);
            ToLog(ApktoolEventType.None, "Time elapsed: " + ts.ToString("mm\\:ss"));

            if (Settings.Default.PlaySoundWhenDone)
                SystemSounds.Beep.Play();

            TaskbarManager.Instance.SetProgressValue(1, 1);
            if (statusStrip1.InvokeRequired)
                statusStrip1.BeginInvoke(new Action(delegate { progressBar.Style = ProgressBarStyle.Continuous; }));
            else
                progressBar.Style = ProgressBarStyle.Continuous;

            Invoke(new Action(delegate ()
            {
                progressBar.Visible = false;
            }));

            ActionButtonsEnabled = true;

            if (status == null)
                ToStatus(msg, Resources.error);
            else
                ToStatus(status, Resources.error);
        }

        internal void ClearLog()
        {
            if (Settings.Default.ClearLogBeforeAction)
                logTxtBox.Text = "";
        }
        #endregion

        #region ApkEditor
        private void InitializeApkEditor()
        {
            apkeditor = new ApkEditor(javaPath, Program.APKEDITOR_PATH);
            apkeditor.ApkEditorOutputDataRecieved += ApkEditorOutputDataRecieved;
            apkeditor.ApkEditorErrorDataRecieved += ApkEditorErrorDataRecieved;
        }

        void ApkEditorErrorDataRecieved(object sender, ApkEditorDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.Error, e.Message);
        }

        void ApkEditorOutputDataRecieved(object sender, ApkEditorDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.None, e.Message);
        }

        internal async Task<int> MergeAndDecompile(string inputSplitApk)
        {
            int code = 0;

            Running(Language.MergingApk);

            string apkFileName = Path.GetFileName(inputSplitApk);

            string tempApk = Path.Combine(Program.TEMP_PATH, "dec.apk");
            string tempDecApk = Path.Combine(Program.TEMP_PATH, "dec");

            string splitDir = Path.Combine(Program.TEMP_PATH, "SplitTmp");
            string extractedDir = Path.Combine(splitDir, "ExtractedApks");
            string mergedDir = Path.Combine(splitDir, "Merged");

            string outputDir = PathUtils.GetDirectoryNameWithoutExtension(inputSplitApk);
            if (Settings.Default.Decode_UseOutputDir && !IgnoreOutputDirContextMenu)
                outputDir = Path.Combine(Settings.Default.Decode_OutputDir, Path.GetFileNameWithoutExtension(inputSplitApk));

            try
            {
                DirectoryUtils.Delete(splitDir);
                Directory.CreateDirectory(splitDir);

                await Task.Run(() =>
                {
                    if (Settings.Default.Framework_ClearBeforeDecode)
                    {
                        ToLog(ApktoolEventType.Infomation, Language.ClearingFramework);
                        if (apktool.ClearFramework() == 0)
                        {
                            ToLog(ApktoolEventType.Success, Language.FrameworkCacheCleared);
                        }
                        else
                            ToLog(ApktoolEventType.Error, Language.ErrorClearingFw);
                    }

                    ToLog(ApktoolEventType.None, String.Format(Language.InputFile, inputSplitApk));

                    //Extract all apk files
                    ToLog(ApktoolEventType.None, Language.ExtractingAllApkFiles);
                    ZipUtils.ExtractAll(inputSplitApk, extractedDir, true);

                    var apkfiles = Directory.EnumerateFiles(extractedDir, "*.apk");

                    ToLog(ApktoolEventType.None, Language.MergingApkEditor);

                    code = apkeditor.Merge(extractedDir, tempApk);
                    if (code == 0)
                    {
                        if (useAPKEditorForDecompilingItem.Checked)
                            code = apkeditor.Decompile(tempApk, tempDecApk);
                        else
                            code = apktool.Decompile(tempApk, tempDecApk);

                        if (code == 0)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Language.MoveTempApkFileToOutput, tempDecApk, outputDir));
                            DirectoryUtils.Delete(outputDir);
                            DirectoryUtils.Copy(tempDecApk, outputDir);

                            BeginInvokeOnUIThread(() =>
                            {
                                textBox_BUILD_InputProjectDir.Text = outputDir;
                            });

                            ToLog(ApktoolEventType.None, String.Format(Language.DecompilingSuccessfullyCompleted, outputDir));
                            if (Settings.Default.Decode_FixError)
                            {
                                if (ApkFixer.FixAndroidManifest(outputDir))
                                    ToLog(ApktoolEventType.None, Language.FixAndroidManifest);
                                if (ApkFixer.FixApktoolYml(outputDir))
                                    ToLog(ApktoolEventType.None, Language.FixApktoolYml);
                                if (ApkFixer.RemoveApkToolDummies(outputDir))
                                    ToLog(ApktoolEventType.None, Language.RemoveApkToolDummies);
                            }
                            ToLog(ApktoolEventType.None, String.Format(Language.MergeFinishedMoveDir, outputDir));

                            Done();
                        }
                        else
                        {
                            Error(Language.ErrorDecompiling);
                        }
                    }
                    else
                    {
                        Error(Language.ErrorMerging);
                    }
                });
            }
            catch (Exception ex)
            {
                code = 1;
                Error(ex);
            }

            return code;
        }

        internal async Task<int> Merge(string inputSplitApk)
        {
            int code = 0;

            Running(Language.MergingApk);

            string apkFileName = Path.GetFileName(inputSplitApk);
            string tempFile = Path.Combine(Program.TEMP_PATH, "tempsplit");
            string tempFileMerged = Path.Combine(Program.TEMP_PATH, "tempsplitmerged");

            string outputFile = PathUtils.GetDirectoryNameWithoutExtension(inputSplitApk) + " merged.apk";

            try
            {
                await Task.Run(() =>
                {
                    ToLog(ApktoolEventType.None, String.Format(Language.InputFile, inputSplitApk));

                    ToLog(ApktoolEventType.None, Language.MergingApkEditor);

                    ToLog(ApktoolEventType.None, String.Format(Language.CopyFileToTemp, inputSplitApk, tempFile));
                    FileUtils.Copy(inputSplitApk, tempFile, true);

                    code = apkeditor.Merge(tempFile, tempFileMerged);
                    if (code == 0)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Language.MoveTempApkToOutput, tempFile, outputFile));
                        FileUtils.Move(tempFileMerged, outputFile, true);
                        Done();
                    }
                    else
                    {
                        Error(Language.ErrorMerging);
                    }
                });
            }
            catch (Exception ex)
            {
                code = 1;
                Error(ex);
            }

            return code;
        }
        #endregion

        #region Apktool
        public async void SetApktoolPath()
        {
            apktool.JarPath = Program.APKTOOL_PATH;
            if (Settings.Default.UseCustomApktool)
            {
                apktool.JarPath = Settings.Default.ApktoolPath;
            }

            string apktoolVersion = apktool.GetVersion();
            string apktoolVersionOld = apktool.GetVersionOld();
            if (!String.IsNullOrWhiteSpace(apktoolVersion) && !Regex.IsMatch(apktoolVersion, @"\r\n?|\n"))
                ToLog(ApktoolEventType.None, $"{Language.APKToolVersion} \"{apktoolVersion}\"");
            else if (!String.IsNullOrWhiteSpace(apktoolVersionOld) && !Regex.IsMatch(apktoolVersionOld, @"\r\n?|\n"))
                ToLog(ApktoolEventType.None, $"{Language.APKToolVersion} \"{apktoolVersionOld}\"");
            else
                ToLog(ApktoolEventType.Error, Language.CantDetectApktoolVersion);

            if (MessageBox.Show(Language.ClearFrameworkPrompt, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                await ClearFramework();
            }
        }

        private void InitializeAPKTool()
        {
            string apktoolPath = Program.APKTOOL_PATH;
            if (Settings.Default.UseCustomApktool)
            {
                apktoolPath = Settings.Default.ApktoolPath;
            }
            apktool = new Apktool(javaPath, apktoolPath);
            apktool.ApktoolOutputDataRecieved += apktool_ApktoolOutputDataRecieved;
            apktool.ApktoolErrorDataRecieved += apktool_ApktoolErrorDataRecieved;
        }

        void apktool_ApktoolErrorDataRecieved(object sender, ApktoolDataReceivedEventArgs e)
        {
            if (e.EventType == ApktoolEventType.Unknown)
                ToLog(ApktoolEventType.Error, e.Message);
            else
                ToLog(e.EventType, e.Message);
        }

        void apktool_ApktoolOutputDataRecieved(object sender, ApktoolDataReceivedEventArgs e)
        {
            ToLog(e.EventType, e.Message);
        }

        internal async Task<int> ClearFramework()
        {
            int code = 0;

            ToLog(ApktoolEventType.Infomation, "=====[ " + Language.ClearingFramework + " ]=====");
            ToStatus(Language.ClearingFramework, Resources.waiting);

            try
            {
                await Task.Run(() =>
                {
                    if (apktool.ClearFramework() == 0)
                    {
                        Done(Language.FrameworkCacheCleared);
                    }
                    else
                        Error(Language.ErrorClearingFw);
                });
            }
            catch (Exception ex)
            {
                Error(ex);
                code = 1;
            }

            return code;
        }

        internal async Task<int> Decompile(string inputApk)
        {
            Debug.WriteLine(useAPKEditorForDecompilingItem.Checked);

            int code = 0;

            Running(Language.Decoding);

            string apkFileName = Path.GetFileName(inputApk);
            string outputDir = PathUtils.GetDirectoryNameWithoutExtension(inputApk);
            if (Settings.Default.Decode_UseOutputDir && !IgnoreOutputDirContextMenu)
                outputDir = Path.Combine(Settings.Default.Decode_OutputDir, Path.GetFileNameWithoutExtension(inputApk));

            string tempApk = Path.Combine(Program.TEMP_PATH, "dec.apk");
            string tempDecApk = Path.Combine(Program.TEMP_PATH, "dec");
            string outputTempDir = tempApk.Replace(".apk", "");
            string outputDecDir = outputDir;
            string decOrigDir = Path.Combine(tempDecApk, "original");

            try
            {
                if (!Settings.Default.Decode_Force && Directory.Exists(outputDir))
                {
                    ToLog(ApktoolEventType.Error, String.Format(Language.DecodeDesDirExists, outputDir));
                    return 1;
                }
                await Task.Run(() =>
                {
                    if (Settings.Default.Framework_ClearBeforeDecode && !Settings.Default.UseApkeditor)
                    {
                        ToLog(ApktoolEventType.Infomation, Language.ClearingFramework);
                        if (apktool.ClearFramework() == 0)
                        {
                            ToLog(ApktoolEventType.Success, Language.FrameworkCacheCleared);
                        }
                        else
                            ToLog(ApktoolEventType.Error, Language.ErrorClearingFw);
                    }

                    if (Settings.Default.Utf8FilenameSupport)
                    {
                        DirectoryUtils.Delete(outputTempDir);

                        ToLog(ApktoolEventType.None, String.Format(Language.CopyFileToTemp, inputApk, tempApk));

                        FileUtils.Copy(inputApk, tempApk, true);

                        inputApk = tempApk;
                        outputDecDir = outputTempDir;
                    }

                    if (useAPKEditorForDecompilingItem.Checked)
                        code = apkeditor.Decompile(inputApk, outputDecDir);
                    else
                        code = apktool.Decompile(inputApk, outputDecDir);

                    if (code == 0)
                    {
                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Language.MoveTempApkFileToOutput, outputTempDir, outputDir));
                            DirectoryUtils.Delete(outputDir);
                            DirectoryUtils.Copy(outputTempDir, outputDir);
                        }

                        BeginInvokeOnUIThread(() =>
                        {
                            textBox_BUILD_InputProjectDir.Text = outputDir;
                        });

                        ToLog(ApktoolEventType.None, String.Format(Language.DecompilingSuccessfullyCompleted, outputDir));
                        if (Settings.Default.Decode_FixError && !useAPKEditorForDecompilingItem.Checked)
                        {
                            if (ApkFixer.FixAndroidManifest(outputDir))
                                ToLog(ApktoolEventType.None, Language.FixAndroidManifest);
                            if (ApkFixer.FixApktoolYml(outputDir))
                                ToLog(ApktoolEventType.None, Language.FixApktoolYml);
                            if (ApkFixer.RemoveApkToolDummies(outputDir))
                                ToLog(ApktoolEventType.None, Language.RemoveApkToolDummies);
                        }

                        Done();
                    }
                    else
                        Error(Language.ErrorDecompiling);
                });
            }
            catch (Exception ex)
            {
                code = 1;
                Error(ex.ToString(), Language.ErrorDecompiling);
            }

            return code;
        }

        internal async Task<int> Build(string inputFolder)
        {
            int code = 0;

            Running(Language.Build);
            ToLog(ApktoolEventType.None, String.Format(Language.InputFile, inputFolder));

            try
            {
                await Task.Factory.StartNew(() =>
                {
                    string outputFile = inputFolder + " compiled.apk";
                    string outputUnsignedApk = inputFolder + " unsigned.apk";
                    if (Settings.Default.Build_SignAfterBuild)
                        outputFile = inputFolder + " signed.apk";
                    if (Settings.Default.Build_UseOutputAppPath && !IgnoreOutputDirContextMenu)
                    {
                        outputFile = Path.Combine(Settings.Default.Build_OutputAppPath, Path.GetFileName(inputFolder)) + ".apk";
                        if (Settings.Default.Build_SignAfterBuild)
                            outputFile = Path.Combine(Settings.Default.Build_OutputAppPath, Path.GetFileName(inputFolder)) + " signed.apk";
                    }

                    string outputCompiledApkFile = outputFile;

                    string tempDecApkFolder = Path.Combine(Program.TEMP_PATH, "dec");
                    string outputTempApk = tempDecApkFolder + ".apk";

                    bool isDecompiledUsingApkEditor = File.Exists(Path.Combine(inputFolder, "path-map.json"));

                    if (Settings.Default.Utf8FilenameSupport)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Language.CopyFolderToTemp, inputFolder, tempDecApkFolder));
                        DirectoryUtils.Delete(tempDecApkFolder);
                        DirectoryUtils.Copy(inputFolder, tempDecApkFolder);

                        inputFolder = tempDecApkFolder;
                        outputFile = outputTempApk;
                    }

                    if (isDecompiledUsingApkEditor)
                        code = apkeditor.Build(inputFolder, outputFile);
                    else
                        code = apktool.Build(inputFolder, outputFile);

                    if (code == 0)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Language.CompilingSuccessfullyCompleted, outputFile));

                        if (Settings.Default.Build_CreateUnsignedApk)
                        {
                            ToStatus(Language.CreateUnsignedApk, Resources.waiting);
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Language.CreateUnsignedApk + " ]=====");

                            if (Directory.Exists(Path.Combine(inputFolder, "original", "META-INF")))
                            {
                                string unsignedApkPath = Path.Combine(Path.GetDirectoryName(outputCompiledApkFile), Path.GetFileName(outputUnsignedApk));
                                ZipUtils.UpdateDirectory(outputFile, Path.Combine(inputFolder, "original", "META-INF"), "META-INF");
                                if (File.Exists(Path.Combine(inputFolder, "original", "stamp-cert-sha256")))
                                    ZipUtils.UpdateFile(outputFile, Path.Combine(inputFolder, "original", "stamp-cert-sha256"));
                                ToLog(ApktoolEventType.Infomation, String.Format(Language.CopyFileTo, outputFile, unsignedApkPath));
                                File.Copy(outputFile, unsignedApkPath, true);
                            }
                            else
                                ToLog(ApktoolEventType.Warning, Language.MetainfNotExist);
                        }

                        if (Settings.Default.Build_ZipalignAfterBuild)
                        {
                            ToStatus(Language.Aligning, Resources.waiting);
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Language.Aligning + " ]=====");
                            ToLog(ApktoolEventType.None, String.Format(Language.InputFile, inputFolder));

                            if (zipalign.Align(outputFile, outputFile) == 0)
                            {
                                ToLog(ApktoolEventType.None, Language.Done);
                            }
                            else
                            {
                                Error(Language.ErrorZipalign);
                                return;
                            }
                        }

                        if (Settings.Default.Build_SignAfterBuild)
                        {
                            ToStatus(Language.Signing, Resources.waiting);
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Language.Signing + " ]=====");
                            ToLog(ApktoolEventType.None, String.Format(Language.InputFile, inputFolder));

                            if (signapk.Sign(outputFile, outputFile) == 0)
                            {
                                ToLog(ApktoolEventType.None, Language.Done);

                                if (Settings.Default.AutoDeleteIdsigFile)
                                {
                                    ToLog(ApktoolEventType.None, String.Format(Language.DeleteFile, outputFile + ".idsig"));
                                    FileUtils.Delete(outputFile + ".idsig");
                                }

                                string device = selAdbDeviceLbl.Text;
                                if (Settings.Default.Sign_InstallApkAfterSign)
                                {
                                    if (!String.IsNullOrEmpty(device))
                                    {
                                        ToStatus(Language.InstallingApk, Resources.waiting);
                                        ToLog(ApktoolEventType.Infomation, "=====[ " + Language.InstallingApk + " ]=====");

                                        if (adb.Install(device, outputFile) == 0)
                                            ToLog(ApktoolEventType.None, Language.InstallApkSuccessful);
                                        else
                                            ToLog(ApktoolEventType.Error, Language.InstallApkFailed);
                                    }
                                    else
                                        ToLog(ApktoolEventType.Error, String.Format(Language.DeviceNotSelected, outputFile));
                                }
                            }
                            else
                                ToLog(ApktoolEventType.Error, Language.ErrorSigning);
                        }

                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Language.MoveTempApkToOutput, outputTempApk, outputCompiledApkFile));
                            FileUtils.Move(outputTempApk, outputCompiledApkFile, true);
                        }

                        Done();
                    }
                    else
                    {
                        Error(Language.ErrorCompiling);
                    }
                });
            }
            catch (Exception ex)
            {
                Error(ex);
                code = 1;
            }

            return code;
        }
        #endregion

        #region Baksmali
        private void InitializeBaksmali()
        {
            baksmali = new Baksmali(javaPath, Program.BAKSMALI_PATH);
            baksmali.BaksmaliOutputDataRecieved += BaksmaliOutputDataRecieved;
            baksmali.BaksmaliErrorDataRecieved += BaksmaliErrorDataRecieved;
        }

        void BaksmaliErrorDataRecieved(object sender, BaksmaliDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.Error, e.Message);
        }

        void BaksmaliOutputDataRecieved(object sender, BaksmaliDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.None, e.Message);
        }

        internal async Task<int> Baksmali(string inputFile)
        {
            int code = 0;
            try
            {
                Running(Language.DecompilingDex);
                ToLog(ApktoolEventType.None, String.Format(Language.InputFile, inputFile));

                await Task.Run(() =>
                {
                    string outputDir = String.Format("{0}", Path.Combine(Path.GetDirectoryName(inputFile), "dexout", Path.GetFileNameWithoutExtension(inputFile)));
                    if (Settings.Default.Baksmali_UseOutputDir && !IgnoreOutputDirContextMenu)
                        outputDir = String.Format("{0}", Path.Combine(Settings.Default.Baksmali_OutputDir, Path.GetFileNameWithoutExtension(inputFile)));

                    code = baksmali.Disassemble(inputFile, outputDir);
                    if (code == 0)
                    {
                        BeginInvokeOnUIThread(() =>
                        {
                            smaliBrowseInputDirTxtBox.Text = outputDir;
                        });
                        Done(String.Format(Language.DecompilingSuccessfullyCompleted, outputDir));
                    }
                    else
                        Error(Language.ErrorDecompiling);
                });
            }
            catch (Exception ex)
            {
                code = 1;
                Error(ex);
            }

            return code;
        }
        #endregion

        #region Smali
        private void InitializeSmali()
        {
            smali = new Smali(javaPath, Program.SMALI_PATH);
            smali.SmaliOutputDataRecieved += SmaliOutputDataRecieved;
            smali.SmaliErrorDataRecieved += SmaliErrorDataRecieved;
        }

        void SmaliErrorDataRecieved(object sender, SmaliDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.Error, e.Message);
        }

        void SmaliOutputDataRecieved(object sender, SmaliDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.None, e.Message);
        }

        internal async Task<int> Smali(string inputDir)
        {
            int code = 0;
            try
            {
                Running(Language.CompilingDex);

                ToLog(ApktoolEventType.None, String.Format(Language.InputDirectory, inputDir));

                await Task.Run(() =>
                {
                    string outputDir = String.Format("{0}.dex", inputDir);
                    if (Settings.Default.Smali_UseOutputDir && !IgnoreOutputDirContextMenu)
                        outputDir = String.Format("{0}.dex", Path.Combine(Settings.Default.Smali_OutputDir, Path.GetFileNameWithoutExtension(inputDir)));

                    code = smali.Assemble(inputDir, outputDir);
                    if (code == 0)
                        Done(String.Format(Language.CompilingSuccessfullyCompleted, outputDir));
                    else
                        Error(Language.ErrorCompiling);
                });
            }
            catch (Exception ex)
            {
                Error(ex);
                code = 1;
            }

            return code;
        }
        #endregion

        #region Zipalign
        private void InitializeZipalign()
        {
            zipalign = new Zipalign(Program.ZIPALIGN_PATH);
            zipalign.OutputDataReceived += zipalign_OutputDataReceived;
            zipalign.ErrorDataReceived += zipalign_ErrorDataReceived;
        }

        void zipalign_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.None, e.Data);
        }

        void zipalign_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.Error, e.Data);
        }

        internal async Task<int> Align(string inputFile)
        {
            int code = 0;

            Running(Language.Aligning);
            ToLog(ApktoolEventType.None, String.Format(Language.InputFile, inputFile));

            string outputDir = inputFile;
            if (Settings.Default.Zipalign_UseOutputDir && !IgnoreOutputDirContextMenu)
                outputDir = Path.Combine(Settings.Default.Zipalign_OutputDir, Path.GetFileName(inputFile));

            if (!Settings.Default.Zipalign_OverwriteOutputFile)
                outputDir = PathUtils.GetDirectoryNameWithoutExtension(outputDir) + " aligned.apk";

            try
            {
                await Task.Run(() =>
                {
                    string tempApk = Path.Combine(Program.TEMP_PATH, "tempapk.apk");
                    string outputApkFile = outputDir;

                    if (Settings.Default.Utf8FilenameSupport)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Language.CopyFileToTemp, inputFile, tempApk));
                        FileUtils.Copy(inputFile, tempApk, true);
                        inputFile = tempApk;
                        outputDir = tempApk;
                    }

                    code = zipalign.Align(inputFile, outputDir);
                    if (code == 0)
                    {
                        if (Settings.Default.Zipalign_SignAfterZipAlign)
                        {
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Language.Signing + " ]=====");
                            if (signapk.Sign(outputDir, outputDir) == 0)
                            {
                                ToLog(ApktoolEventType.None, Language.Done);

                                if (Settings.Default.AutoDeleteIdsigFile)
                                {
                                    ToLog(ApktoolEventType.None, String.Format(Language.DeleteFile, outputDir + ".idsig"));
                                    FileUtils.Delete(outputDir + ".idsig");
                                }
                            }
                            else
                                ToLog(ApktoolEventType.Error, Language.ErrorSigning);
                        }

                        ToLog(ApktoolEventType.None, String.Format(Language.ZipalignFileSavedTo, outputDir));
                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Language.MoveTempApkToOutput, tempApk, outputApkFile));
                            FileUtils.Move(tempApk, outputApkFile, true);
                        }

                        Done();
                    }
                    else
                        Error(Language.ErrorZipalign);
                });
            }
            catch (Exception ex)
            {
                Error(ex);
                code = 1;
            }

            return code;
        }
        #endregion

        #region Signapk
        private void InitializeSignapk()
        {
            signapk = new Signapk(javaPath, Program.APKSIGNER_PATH);
            signapk.SignapkOutputDataRecieved += SignApkOutputDataRecieved;
            signapk.SignapkErrorDataRecieved += SignApkErrorDataRecieved;
        }

        void SignApkErrorDataRecieved(object sender, SignapkDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.Error, e.Message);
        }

        void SignApkOutputDataRecieved(object sender, SignapkDataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.None, e.Message);
        }

        internal async Task<int> Sign(string input)
        {
            int code = 0;

            Running(Language.Signing);

            string outputFile = input;
            if (Settings.Default.Zipalign_UseOutputDir && !IgnoreOutputDirContextMenu)
                outputFile = Path.Combine(Settings.Default.Sign_OutputDir, Path.GetFileName(input));
            if (!Settings.Default.Sign_OverwriteInputFile)
                outputFile = PathUtils.GetDirectoryNameWithoutExtension(outputFile) + "_signed.apk";

            string tempApk = Path.Combine(Program.TEMP_PATH, "tempapk.apk");
            string outputApkFile = outputFile;

            ToLog(ApktoolEventType.None, String.Format(Language.InputFile, input));

            try
            {
                await Task.Run(() =>
                {
                    if (Settings.Default.Utf8FilenameSupport)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Language.CopyFileToTemp, input, tempApk));
                        FileUtils.Copy(input, tempApk, true);
                        input = tempApk;
                        outputFile = tempApk;
                    }

                    code = signapk.Sign(input, outputFile);

                    if (code == 0)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Language.SignSuccessfullyCompleted, outputFile));

                        string device = selAdbDeviceLbl.Text;

                        if (Settings.Default.Sign_InstallApkAfterSign)
                        {
                            if (!string.IsNullOrEmpty(device))
                            {
                                ToLog(ApktoolEventType.Infomation, "=====[ " + Language.InstallingApk + " ]=====");
                                if (adb.Install(device, outputFile) == 0)
                                {
                                    ToLog(ApktoolEventType.Success, Language.InstallApkSuccessful);
                                }
                                else
                                    ToLog(ApktoolEventType.Error, Language.InstallApkFailed);
                            }
                            else
                                ToLog(ApktoolEventType.Error, String.Format(Language.DeviceNotSelected, outputFile));
                        }

                        if (Settings.Default.AutoDeleteIdsigFile)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Language.DeleteFile, outputFile + ".idsig"));
                            FileUtils.Delete(outputFile + ".idsig");
                        }

                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Language.MoveTempApkToOutput, tempApk, outputApkFile));
                            FileUtils.Move(tempApk, outputApkFile, true);
                        }

                        Done();
                    }
                    else
                        Error(String.Format(Language.ErrorSigning, outputFile));
                });
            }
            catch (Exception ex)
            {
                code = 1;
                Error(ex);
            }

            return code;
        }
        #endregion

        #region Adb
        private void InitializeAdb()
        {
            adb = new Adb(Program.ADB_PATH);
            adb.OutputDataReceived += AdbOutputDataReceived;
            adb.ErrorDataReceived += AdbErrorDataReceived;
        }

        void AdbErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.Error, e.Data);
        }

        void AdbOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ToLog(ApktoolEventType.None, e.Data);
        }

        internal async Task<int> ListDevices()
        {
            int code = 0;
            AdbActionButtonsEnabled = false;
            ToLog(ApktoolEventType.None, Language.GettingDevices);
            ToStatus(Language.GettingDevices, Resources.waiting);

            string devices = null;
            int numOfDevices = 0;

            try
            {
                devicesListBox.Items.Clear();

                await Task.Run(() =>
                {
                    devices = adb.GetDevices();
                });
                if (!String.IsNullOrEmpty(devices))
                {
                    string[] deviceLines = devices.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in deviceLines.Skip(1))
                    {
                        numOfDevices++;
                        devicesListBox.Items.Add(line);
                    }
                }
            }
            catch (Exception ex)
            {
                code = 1;
                ToLog(ApktoolEventType.Error, ex.ToString());
            }

            if (numOfDevices != 0)
                ToLog(ApktoolEventType.None, String.Format(Language.DevicesFound, numOfDevices));
            else
                ToLog(ApktoolEventType.None, Language.NoDevicesFound);

            ToStatus(Language.Done, Resources.done);
            AdbActionButtonsEnabled = true;

            return code;
        }

        internal async Task<int> Install(string inputApk)
        {
            string device = selAdbDeviceLbl.Text;
            if (String.IsNullOrEmpty(device))
            {
                ToLog(ApktoolEventType.Error, String.Format(Language.DeviceNotSelected, inputApk));
                return 1;
            }

            int code = 0;

            Running(Language.InstallingApk);

            AdbActionButtonsEnabled = false;

            ToLog(ApktoolEventType.None, String.Format(Language.InstallingApkPath, inputApk));

            try
            {
                await Task.Run(() =>
                {
                    code = adb.Install(device, inputApk);
                    if (code == 0)
                    {
                        Done(Language.InstallApkSuccessful);
                    }
                    else
                        Error(Language.InstallApkFailed);
                });
            }
            catch (Exception ex)
            {
                code = 1;
                Error(ex);
            }

            AdbActionButtonsEnabled = true;

            return code;
        }
        #endregion

        #region Form handlers
        private async void FormMain_Shown(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                InitializeUpdateChecker();
                InitializeZipalign();

                javaPath = JavaUtils.GetJavaPath();
                if (javaPath != null)
                {
                    InitializeBaksmali();
                    InitializeSmali();
                    InitializeAPKTool();
                    InitializeSignapk();
                    InitializeApkEditor();

                    string javaVersion = apktool.GetJavaVersion();
                    if (javaVersion != null)
                    {
                        ToLog(ApktoolEventType.None, javaVersion);
                        string apktoolVersion = apktool.GetVersion();
                        string apktoolVersionOld = apktool.GetVersionOld();

                        if (!String.IsNullOrWhiteSpace(apktoolVersion) && !Regex.IsMatch(apktoolVersion, @"\r\n?|\n"))
                            ToLog(ApktoolEventType.None, $"{Language.APKToolVersion} {apktoolVersion}");
                        else if (!String.IsNullOrWhiteSpace(apktoolVersionOld) && !Regex.IsMatch(apktoolVersionOld, @"\r\n?|\n"))
                            ToLog(ApktoolEventType.None, $"{Language.APKToolVersion} {apktoolVersionOld}");
                        else
                            ToLog(ApktoolEventType.Error, Language.CantDetectApktoolVersion);

                        string apkeditorVersion = apkeditor.GetVersion();
                        if (!String.IsNullOrWhiteSpace(apkeditorVersion))
                            ToLog(ApktoolEventType.None, apkeditorVersion);
                        else
                            ToLog(ApktoolEventType.Error, Language.CantDetectApkeditorVersion);
                    }
                    else
                        ToLog(ApktoolEventType.Error, Language.ErrorJavaDetect);
                }
                else
                {
                    ToLog(ApktoolEventType.Error, Language.ErrorJavaDetect);
                    BeginInvoke(new MethodInvoker(delegate
                    {
                        tabPageMain.Enabled = false;
                        tabPageBaksmali.Enabled = false;
                        tabPageInstallFramework.Enabled = false;
                    }));
                }

                InitializeAdb();

                if (AdminUtils.IsAdministrator())
                    ToLog(ApktoolEventType.Warning, Language.DragDropNotSupported);
                else
                    ToLog(ApktoolEventType.None, Language.DragDropSupported);

                ToLog(ApktoolEventType.None, String.Format(Language.TempDirectory, Program.TEMP_PATH));

                TimeSpan updateInterval = DateTime.Now - Settings.Default.LastUpdateCheck;
                if (updateInterval.Days > 0 && Settings.Default.CheckForUpdateAtStartup)
                    updateCheker.CheckAsync(true);
            });
            ToStatus(Language.Done, Resources.done);

            RunCmdArgs();

            await ListDevices();
        }

        private async void clearTempFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Running(Language.ClearTempFolder);
            try
            {
                await Task.Run(() =>
                {
                    foreach (var subDir in new DirectoryInfo(Program.TEMP_MAIN).EnumerateDirectories())
                    {
                        ToLog(ApktoolEventType.None, String.Format(Language.DeletingFolder, subDir));
                        DirectoryUtils.Delete(subDir.FullName);
                    }
                    Directory.CreateDirectory(Program.TEMP_PATH);
                });
                Done();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        private async void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControlMain.SelectedIndex == 1 && String.IsNullOrEmpty(appTxtBox.Text) && Environment.GetCommandLineArgs().Length == 1)
            {
                await GetApkInfo(Settings.Default.Decode_InputAppPath);
            }
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            if (!isRunning)
                TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress, Handle);
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            Save();

            // Dispose APK icon image
            try
            {
                if (previousApkIcon != null)
                {
                    previousApkIcon.Dispose();
                    previousApkIcon = null;
                    Debug.WriteLine("[FormMain] Cleaned up APK icon on exit");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FormMain] Error disposing APK icon: {ex.Message}");
            }

            // Dispose all tool instances
            try
            {
                adb?.Dispose();
                zipalign?.Dispose();
                apktool?.Dispose();
                signapk?.Dispose();
                baksmali?.Dispose();
                smali?.Dispose();
                apkeditor?.Dispose();
                
                Debug.WriteLine("[FormMain] All tool instances disposed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FormMain] Error disposing resources: {ex.Message}");
            }

            DirectoryUtils.Delete(Program.TEMP_PATH);
        }

        private bool ActionButtonsEnabled
        {
            set
            {
                BeginInvokeOnUIThread(() =>
                {
                    button_BUILD_Build.Enabled = value;
                    button_DECODE_Decode.Enabled = value;
                    button_IF_InstallFramework.Enabled = value;
                    button_ZIPALIGN_Align.Enabled = value;
                    button_SIGN_Sign.Enabled = value;
                    decSmaliBtn.Enabled = value;
                    comSmaliBtn.Enabled = value;
                    mergeApkBtn.Enabled = value;
                });
            }
        }

        private bool AdbActionButtonsEnabled
        {
            set
            {
                InvokeOnUIThread(() =>
                {
                    killAdbBtn.Enabled = value;
                    refreshDevicesBtn.Enabled = value;
                    installApkBtn.Enabled = value;
                    devicesListBox.Enabled = value;
                    apkPathAdbTxtBox.Enabled = value;
                    selApkAdbBtn.Enabled = value;
                    setVendorChkBox.Enabled = value;
                    overrideAbiCheckBox.Enabled = value;
                    overrideAbiComboBox.Enabled = value;
                });
            }
        }

        internal void ShowMessage(string message, MessageBoxIcon status)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, status);
        }

        #region UI Thread Helpers
        /// <summary>
        /// Execute action synchronously on UI thread
        /// </summary>
        private void InvokeOnUIThread(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Execute action asynchronously on UI thread (Fire and forget)
        /// </summary>
        private void BeginInvokeOnUIThread(Action action)
        {
            if (InvokeRequired)
            {
                BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Execute function on UI thread and return result
        /// </summary>
        private T InvokeOnUIThread<T>(Func<T> func)
        {
            if (InvokeRequired)
            {
                return (T)Invoke(func);
            }
            else
            {
                return func();
            }
        }
        #endregion
        #endregion

        #region Config
        internal void Save()
        {
            Settings.Default.Sign_Schemev1 = schemev1ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev2 = schemev2ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev3 = schemev3ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev4 = schemev4ComboBox.SelectedIndex;
            Settings.Default.Adb_OverrideAbi = overrideAbiComboBox.SelectedIndex;
            Settings.Default.UseApkeditor = useAPKEditorForDecompilingItem.Checked;
            Settings.Default.Save();
        }
        #endregion

        #region Cancel
        private void toolStripStatusLabelStateText_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Language.CancelProcess, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                CancelProcess();
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Language.CancelProcess, Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                CancelProcess();
        }

        private void CancelProcess()
        {
            try
            {
                ToStatus(Language.PleaseWait, Resources.waiting);

                apkeditor.Cancel();
                apktool.Cancel();
                baksmali.Cancel();
                smali.Cancel();
                zipalign.Cancel();
                signapk.Cancel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ActionButtonsEnabled = true;
            }
        }
        #endregion

        private void apkIconPicBox_Click(object sender, EventArgs e)
        {
            if (apkIconPicBox.Image != null)
            {
                using (SaveFileDialog saveFile = new SaveFileDialog())
                {
                    saveFile.Filter = "PNG Image|*.png";
                    saveFile.Title = "Save an Image File";
                    saveFile.FileName = appTxtBox.Text; // Set default filename to app name

                    if (saveFile.ShowDialog() == DialogResult.OK && !String.IsNullOrEmpty(saveFile.FileName))
                    {
                        apkIconPicBox.Image.Save(saveFile.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }
    }
}