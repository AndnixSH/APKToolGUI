using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using APKToolGUI.ApkTool;
using APKToolGUI.Controls;
using APKToolGUI.Properties;
using APKToolGUI.Utils;
using Ionic.Zip;
using Java;
using Microsoft.WindowsAPICodePack.Taskbar;
using Ookii.Dialogs.WinForms;
using Lang = APKToolGUI.Languages.Language;
using WinForms = System.Windows.Forms;
using DColor = System.Drawing.Color;
using Res = APKToolGUI.Properties.Resources;

namespace APKToolGUI.Forms
{
    /// <summary>
    /// The main application window. This file holds the constructor, settings bindings,
    /// event wiring, window lifecycle and menu; <c>MainWindow.Logic.cs</c> holds the
    /// apktool operation logic, the control event handlers, localization and drag-drop.
    /// </summary>
    public partial class MainWindow : Window
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
        private readonly System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        private string lastStartedDate;
        private System.Drawing.Bitmap previousApkIcon;
        private IntPtr _hwnd = IntPtr.Zero;

        internal static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            Instance = this;
            Program.SetLanguage();

            InitializeComponent();

            Theme theme = (Theme)Settings.Default.Theme;
            WpfTheme.Apply(this, Program.IsDarkTheme());
            NativeDarkMode.ApplyTheme(this, theme);

            Title = "APK Tool GUI - v" + WinForms.Application.ProductVersion;
            menuUseApkEditor.IsChecked = Settings.Default.UseApkeditor;

            Log.Output = ToLog;
            ApplyLocalization();
            aapt = new AaptParser();

            // Validate stored paths (mirrors the old FormMain constructor)
            if (!File.Exists(Settings.Default.Decode_InputAppPath)) Settings.Default.Decode_InputAppPath = "";
            if (!Directory.Exists(Settings.Default.Build_InputDir)) Settings.Default.Build_InputDir = "";
            if (!File.Exists(Settings.Default.Sign_InputFile)) Settings.Default.Sign_InputFile = "";
            if (!File.Exists(Settings.Default.Zipalign_InputFile)) Settings.Default.Zipalign_InputFile = "";
            if (!File.Exists(Settings.Default.Sign_PrivateKey) || String.IsNullOrEmpty(Settings.Default.Sign_PrivateKey)) Settings.Default.Sign_PrivateKey = Program.SIGNAPK_KEYPRIVATE;
            if (!File.Exists(Settings.Default.Sign_PublicKey) || String.IsNullOrEmpty(Settings.Default.Sign_PublicKey)) Settings.Default.Sign_PublicKey = Program.SIGNAPK_KEYPUBLIC;

            BindSettings();

            schemev1ComboBox.SelectedIndex = Clamp(Settings.Default.Sign_Schemev1, schemev1ComboBox.Items.Count);
            schemev2ComboBox.SelectedIndex = Clamp(Settings.Default.Sign_Schemev2, schemev2ComboBox.Items.Count);
            schemev3ComboBox.SelectedIndex = Clamp(Settings.Default.Sign_Schemev3, schemev3ComboBox.Items.Count);
            schemev4ComboBox.SelectedIndex = (Settings.Default.Sign_Schemev4 >= 0 && Settings.Default.Sign_Schemev4 < schemev4ComboBox.Items.Count) ? Settings.Default.Sign_Schemev4 : 2;
            overrideAbiComboBox.SelectedIndex = Clamp(Settings.Default.Adb_OverrideAbi, overrideAbiComboBox.Items.Count);

            WireEvents();

            Loaded += MainWindow_Loaded;
            Closing += MainWindow_Closing;
            Activated += (s, e) => { if (!isRunning && _hwnd != IntPtr.Zero) TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.NoProgress, _hwnd); };
        }

        private static int Clamp(int value, int count)
        {
            return (value >= 0 && value < count) ? value : 0;
        }

        #region Settings bindings

        private void BindSettings()
        {
            BindText(textBox_DECODE_InputAppPath, "Decode_InputAppPath");
            BindChecked(checkBox_DECODE_NoSrc, "Decode_NoSrc");
            BindChecked(checkBox_DECODE_NoRes, "Decode_NoRes");
            BindChecked(checkBox_DECODE_Force, "Decode_Force");
            BindChecked(checkBox_DECODE_KeepBrokenRes, "Decode_KeepBrokenRes");
            BindChecked(checkBox_DECODE_MatchOriginal, "Decode_MatchOriginal");
            BindChecked(checkBox_DECODE_OnlyMainClasses, "Decode_OnlyMainClasses");
            BindChecked(checkBox_DECODE_NoDebugInfo, "Decode_NoDebugInfo");
            BindChecked(checkBox_DECODE_FixError, "Decode_FixError");
            BindChecked(checkBox7, "Decode_DontParseApkInfo");
            BindChecked(checkBox_DECODE_UseFramework, "Decode_UseFramework");
            BindChecked(checkBox_DECODE_OutputDirectory, "Decode_UseOutputDir");
            BindText(textBox_DECODE_FrameDir, "Framework_FrameDir");
            BindText(textBox_DECODE_OutputDirectory, "Decode_OutputDir");
            BindChecked(decSetApiLvlChkBox, "Decode_SetApiLevel");
            BindValue(decApiLvlUpDown, "Decode_ApiLevel");
            BindChecked(checkBox3, "Decode_SetJobs");
            BindValue(decJobsLvlUpDown, "Decode_Jobs");
            BindEnabled(textBox_DECODE_FrameDir, "Decode_UseFramework");
            BindEnabled(button_DECODE_BrowseFrameDir, "Decode_UseFramework");
            BindEnabled(textBox_DECODE_OutputDirectory, "Decode_UseOutputDir");
            BindEnabled(button_DECODE_BrowseOutputDirectory, "Decode_UseOutputDir");

            BindText(textBox_BUILD_InputProjectDir, "Build_InputDir");
            BindChecked(checkBox_BUILD_ForceAll, "Build_ForceAll");
            BindChecked(checkBox_BUILD_UseAapt, "Build_UseAapt");
            BindText(textBox_BUILD_AaptPath, "Build_AaptPath");
            BindChecked(checkBox_BUILD_UseFramework, "Build_UseFramework");
            BindText(textBox_BUILD_FrameDir, "Framework_FrameDir");
            BindChecked(checkBox_BUILD_OutputAppPath, "Build_UseOutputAppPath");
            BindText(textBox_BUILD_OutputAppPath, "Build_OutputAppPath");
            BindChecked(checkBox_BUILD_NoCrunch, "Build_NoCrunch");
            BindChecked(checkBox_BUILD_NetSecConf, "Build_NetSecConf");
            BindChecked(zipalignAfterBuildChkBox, "Build_ZipalignAfterBuild");
            BindChecked(signAfterBuildChkBox, "Build_SignAfterBuild");
            BindChecked(createUnsignApkChkBox, "Build_CreateUnsignedApk");
            BindChecked(useAapt2ChkBox, "Build_UseAapt2");
            BindChecked(checkBox_BUILD_CopyOriginal, "Build_CopyOriginal");
            BindChecked(buildSetApiLvlChkBox, "Build_SetApiLevel");
            BindValue(buildApiLvlUpDown, "Build_ApiLevel");
            BindChecked(checkBox4, "Build_SetJobs");
            BindValue(comJobsLvlUpDown, "Build_Jobs");
            BindEnabled(textBox_BUILD_AaptPath, "Build_UseAapt");
            BindEnabled(button_BUILD_BrowseAaptPath, "Build_UseAapt");
            BindEnabled(textBox_BUILD_FrameDir, "Build_UseFramework");
            BindEnabled(button_BUILD_BrowseFrameDir, "Build_UseFramework");
            BindEnabled(textBox_BUILD_OutputAppPath, "Build_UseOutputAppPath");
            BindEnabled(button_BUILD_BrowseOutputAppPath, "Build_UseOutputAppPath");

            BindText(textBox_SIGN_InputFile, "Sign_InputFile");
            BindText(textBox_SIGN_PublicKey, "Sign_PublicKey");
            BindText(textBox_SIGN_PrivateKey, "Sign_PrivateKey");
            BindChecked(useAliasChkBox, "Sign_SetAlias");
            BindText(aliasTxtBox, "Sign_Alias");
            BindChecked(useKeyStoreChkBox, "Sign_UseKeystoreFile");
            BindText(keyStoreFileTxtBox, "Sign_KeystoreFilePath");
            // textBox3 / textBox4 are PasswordBoxes (masked); synced to Settings in WireEvents
            // (PasswordBox.Password is not a bindable DependencyProperty).
            BindChecked(useSigningOutputDir, "Sign_UseOutputDir");
            BindText(textBox_SIGN_OutputFile, "Sign_OutputDir");
            BindChecked(autoDelIdsigChkBox, "AutoDeleteIdsigFile");
            BindChecked(checkBox1, "Sign_OverwriteInputFile");
            BindChecked(checkBox2, "Sign_InstallApkAfterSign");

            BindText(textBox_ZIPALIGN_InputFile, "Zipalign_InputFile");
            BindValue(numericUpDown_ZIPALIGN_AlignmentBytes, "Zipalign_AlignmentInBytes");
            BindChecked(checkBox_ZIPALIGN_CheckAlignment, "Zipalign_CheckOnly");
            BindChecked(checkBox_ZIPALIGN_VerboseOutput, "Zipalign_Verbose");
            BindChecked(checkBox_ZIPALIGN_Recompress, "Zipalign_Recompress");
            BindChecked(checkBox_ZIPALIGN_OverwriteOutputFile, "Zipalign_OverwriteOutputFile");
            BindChecked(signAfterZipalignChkBox, "Zipalign_SignAfterZipAlign");
            BindChecked(zipalignOutputDirChkBox, "Zipalign_UseOutputDir");
            BindText(textBox_ZIPALIGN_OutputFile, "Zipalign_OutputDir");

            BindChecked(checkBox_IF_FramePath, "Framework_UseFrameDir");
            BindText(textBox_IF_FrameDir, "Framework_FrameDir");
            BindChecked(checkBox_IF_Tag, "InstallFramework_UseTag");
            BindText(textBox_IF_Tag, "InstallFramework_Tag");
            BindText(textBox_IF_InputFramePath, "InstallFramework_InputFramePath");
            BindChecked(clearFwBeforeDecodeChkBox, "Framework_ClearBeforeDecode");
            BindEnabled(textBox_IF_FrameDir, "Framework_UseFrameDir");
            BindEnabled(button_IF_BrowseFrameDir, "Framework_UseFrameDir");
            BindEnabled(textBox_IF_Tag, "InstallFramework_UseTag");

            BindChecked(baksmaliUseOutputChkBox, "Baksmali_UseOutputDir");
            BindText(baksmaliBrowseOutputTxtBox, "Baksmali_OutputDir");
            BindText(baksmaliBrowseInputDexTxtBox, "Baksmali_InputDexFile");
            BindChecked(smaliUseOutputChkBox, "Smali_UseOutputDir");
            BindText(smaliBrowseOutputTxtBox, "Smali_OutputDir");
            BindText(smaliBrowseInputDirTxtBox, "Smali_InputDir");

            BindChecked(setVendorChkBox, "Adb_SetVendor");
            BindChecked(overrideAbiCheckBox, "Adb_SetOverrideAbi");
            BindText(apkPathAdbTxtBox, "Adb_SelectedApkPath");

            BindText(splitApkPathTxtBox, "SplitApk_InputFile");

            BindChecked(keyGenAutoFillChkBox, "KeyGen_AutoFillSign");
        }

        private static void BindChecked(CheckBox c, string prop)
        {
            c.SetBinding(System.Windows.Controls.Primitives.ToggleButton.IsCheckedProperty,
                new Binding(prop) { Source = Settings.Default, Mode = BindingMode.TwoWay });
        }
        private static void BindText(TextBox t, string prop)
        {
            t.SetBinding(TextBox.TextProperty,
                new Binding(prop) { Source = Settings.Default, Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        }
        private static void BindValue(NumericUpDown n, string prop)
        {
            n.SetBinding(NumericUpDown.ValueProperty,
                new Binding(prop) { Source = Settings.Default, Mode = BindingMode.TwoWay });
        }
        private static void BindEnabled(Control c, string prop)
        {
            c.SetBinding(UIElement.IsEnabledProperty,
                new Binding(prop) { Source = Settings.Default, Mode = BindingMode.OneWay });
        }

        #endregion

        #region Event wiring

        private void WireEvents()
        {
            // Decode
            button_DECODE_BrowseFrameDir.Click += Button_DECODE_BrowseFrameDir_Click;
            button_DECODE_BrowseOutputDirectory.Click += Button_DECODE_BrowseOutputDirectory_Click;
            button_DECODE_BrowseInputAppPath.Click += Button_DECODE_BrowseInputAppPath_Click;
            button_DECODE_Decode.Click += Button_DECODE_Decode_Click;

            // Build
            button_BUILD_BrowseAaptPath.Click += Button_BUILD_BrowseAaptPath_Click;
            button_BUILD_BrowseFrameDir.Click += Button_BUILD_BrowseFrameDir_Click;
            button_BUILD_BrowseOutputAppPath.Click += Button_BUILD_BrowseOutputAppPath_Click;
            button_BUILD_BrowseInputProjectDir.Click += Button_BUILD_BrowseInputProjectDir_Click;
            button_BUILD_Build.Click += Button_BUILD_Build_Click;

            // Sign
            button_SIGN_BrowsePublicKey.Click += Button_SIGN_BrowsePublicKey_Click;
            button_SIGN_BrowsePrivateKey.Click += Button_SIGN_BrowsePrivateKey_Click;
            button_SIGN_BrowseInputFile.Click += Button_SIGN_BrowseInputFile_Click;
            button_SIGN_BrowseOutputFile.Click += Button_SIGN_BrowseOutputFile_Click;
            selectKeyStoreFileBtn.Click += SelectKeyStoreFileBtn_Click;
            button_SIGN_Sign.Click += Button_SIGN_Sign_Click;
            schemev1ComboBox.SelectionChanged += SchemeComboBox_Changed;
            schemev2ComboBox.SelectionChanged += SchemeComboBox_Changed;
            schemev3ComboBox.SelectionChanged += SchemeComboBox_Changed;
            useKeyStoreChkBox.Click += (s, e) => ApplySignControlStates();
            useAliasChkBox.Click += (s, e) => ApplySignControlStates();
            ApplySignControlStates();

            // Masked password fields: PasswordBox.Password isn't bindable, so load + sync manually.
            textBox3.Password = Settings.Default.Sign_KeystorePassword ?? "";
            textBox4.Password = Settings.Default.Sign_KeyPassword ?? "";
            textBox3.PasswordChanged += (s, e) => Settings.Default.Sign_KeystorePassword = textBox3.Password;
            textBox4.PasswordChanged += (s, e) => Settings.Default.Sign_KeyPassword = textBox4.Password;

            // Zipalign
            checkBox_ZIPALIGN_CheckAlignment.Click += (s, e) => ApplyZipalignCheckSwitch();
            button_ZIPALIGN_BrowseOutputFile.Click += Button_ZIPALIGN_BrowseOutputFile_Click;
            button_ZIPALIGN_BrowseInputFile.Click += Button_ZIPALIGN_BrowseInputFile_Click;
            button_ZIPALIGN_Align.Click += Button_ZIPALIGN_Align_Click;

            // Framework
            button_IF_BrowseFrameDir.Click += Button_IF_BrowseFrameDir_Click;
            button_IF_BrowseInputFramePath.Click += Button_IF_BrowseInputFramePath_Click;
            button_IF_InstallFramework.Click += Button_IF_InstallFramework_Click;
            clearFwBtn.Click += ClearFwBtn_Click;
            openFwFolderBtn.Click += OpenFwFolderBtn_Click;

            // Baksmali / Smali
            baksmaliBrowseOutputBtn.Click += BaksmaliBrowseOutputBtn_Click;
            baksmaliBrowseInputDexBtn.Click += BaksmaliBrowseInputDexBtn_Click;
            decSmaliBtn.Click += DecSmaliBtn_Click;
            smaliBrowseOutputBtn.Click += SmaliBrowseOutputBtn_Click;
            smaliBrowseInputDirBtn.Click += SmaliBrowseInputDirBtn_Click;
            comSmaliBtn.Click += ComSmaliBtn_Click;

            // ADB
            killAdbBtn.Click += KillAdbBtn_Click;
            installApkBtn.Click += InstallApkBtn_Click;
            refreshDevicesBtn.Click += RefreshDevicesBtn_Click;
            selApkAdbBtn.Click += SelApkAdbBtn_Click;
            devicesListBox.SelectionChanged += DevicesListBox_SelectionChanged;
            overrideAbiComboBox.SelectionChanged += OverrideAbiComboBox_Changed;

            // APK info
            selApkFileInfoBtn.Click += SelApkFileInfoBtn_Click;
            apkIconPicBox.MouseLeftButtonUp += ApkIcon_Click;
            psLinkBtn.Click += (s, e) => { if (aapt != null) Process.Start(aapt.PlayStoreLink); };
            apkComboLinkBtn.Click += (s, e) => { if (aapt != null) Process.Start(aapt.ApkComboLink); };
            apkPureLinkBtn.Click += (s, e) => { if (aapt != null) Process.Start(aapt.ApkPureLink); };
            apkGkLinkBtn.Click += (s, e) => { if (aapt != null) Process.Start(aapt.ApkGkLink); };
            apkSupportLinkBtn.Click += (s, e) => { if (aapt != null) Process.Start(aapt.ApkSupportLink); };
            apkMirrorLinkBtn.Click += (s, e) => { if (aapt != null) Process.Start(aapt.ApkMirrorLink); };

            // Main tab shortcut buttons
            mergeApkBtn.Click += MergeApkBtn_Click;
            selSplitApkBtn.Click += SelSplitApkBtn_Click;
            openAndroidMainfestBtn.Click += OpenAndroidMainfestBtn_Click;
            openApktoolYmlBtn.Click += OpenApktoolYmlBtn_Click;
            compileOutputOpenDirBtn.Click += CompiledApkOpenDirBtn_Click;
            button_OpenMainActivity.Click += Button_OpenMainActivity_Click;
            decApkOpenDirBtn.Click += DecApkOpenDirBtn_Click;
            decOutOpenDirBtn.Click += DecOutOpenDirBtn_Click;
            comApkOpenDir.Click += ComApkOpenDir_Click;
            signApkOpenDirBtn.Click += SignApkOpenDirBtn_Click;
            alignApkOpenDirBtn.Click += AlignApkOpenDirBtn_Click;

            // Menu extras
            menuUseApkEditor.Click += (s, e) => Settings.Default.UseApkeditor = menuUseApkEditor.IsChecked;

            // Cancel by clicking status text / progress bar
            statusText.MouseLeftButtonUp += (s, e) => PromptCancel();
            progressBar.MouseLeftButtonUp += (s, e) => PromptCancel();

            WireDragDrop();
            ApplyZipalignCheckSwitch();
            InitializeKeyGenerator();
        }

        #endregion

        #region Window lifecycle

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _hwnd = new WindowInteropHelper(this).Handle;

            try { new TaskBarJumpList(_hwnd); } catch (Exception ex) { Debug.WriteLine(ex); }

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

                        if (!String.IsNullOrWhiteSpace(apktool.Version) && !Regex.IsMatch(apktool.Version, @"\r\n?|\n"))
                            ToLog(ApktoolEventType.None, $"{Lang.APKToolVersion} {apktool.Version}");
                        else
                            ToLog(ApktoolEventType.Error, Lang.CantDetectApktoolVersion);

                        string apkeditorVersion = apkeditor.GetVersion();
                        if (!String.IsNullOrWhiteSpace(apkeditorVersion))
                            ToLog(ApktoolEventType.None, apkeditorVersion);
                        else
                            ToLog(ApktoolEventType.Error, Lang.CantDetectApkeditorVersion);
                    }
                    else
                        ToLog(ApktoolEventType.Error, Lang.ErrorJavaDetect);
                }
                else
                {
                    ToLog(ApktoolEventType.Error, Lang.ErrorJavaDetect);
                    BeginInvokeOnUIThread(() =>
                    {
                        tabMain.IsEnabled = false;
                        tabBaksmali.IsEnabled = false;
                        tabFramework.IsEnabled = false;
                    });
                }

                InitializeAdb();

                if (AdminUtils.IsAdministrator())
                    ToLog(ApktoolEventType.Warning, Lang.DragDropNotSupported);
                else
                    ToLog(ApktoolEventType.None, Lang.DragDropSupported);

                ToLog(ApktoolEventType.None, String.Format(Lang.TempDirectory, Program.TEMP_PATH));

                TimeSpan updateInterval = DateTime.Now - Settings.Default.LastUpdateCheck;
                if (updateInterval.Days > 0 && Settings.Default.CheckForUpdateAtStartup)
                    updateCheker.CheckAsync(true);
            });

            ToStatus(Lang.Done, Res.done);
            RunCmdArgs();
            await ListDevices();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Save();
            try { previousApkIcon?.Dispose(); previousApkIcon = null; } catch { }
            try
            {
                adb?.Dispose(); zipalign?.Dispose(); apktool?.Dispose(); signapk?.Dispose();
                baksmali?.Dispose(); smali?.Dispose(); apkeditor?.Dispose();
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            DirectoryUtils.Delete(Program.TEMP_PATH);
        }

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
                            if (file.ContainsAny(".xapk", ".zip", ".apks", ".apkm")) { if (await MergeAndDecompile(file) == 0) Close(); }
                            else { if (await Decompile(file) == 0) Close(); }
                            break;
                        case "comapk": if (await Build(file) == 0) Close(); break;
                        case "sign": if (await Sign(file) == 0) Close(); break;
                        case "zipalign": if (await Align(file) == 0) Close(); break;
                        case "baksmali": if (await Baksmali(file) == 0) Close(); break;
                        case "smali": if (await Smali(file) == 0) Close(); break;
                        case "viewinfo": tabControlMain.SelectedIndex = 1; await GetApkInfo(file); break;
                        default: IgnoreOutputDirContextMenu = false; break;
                    }
                }
            }
            catch (Exception ex) { ToLog(ApktoolEventType.Error, ex.Message); }
        }

        #endregion

        #region Menu

        private void NewInstance_Click(object sender, RoutedEventArgs e) => Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);

        private void SaveLog_Click(object sender, RoutedEventArgs e)
        {
            using (var sfd = new WinForms.SaveFileDialog())
            {
                sfd.FileName = "APK Tool GUI logs";
                sfd.Filter = Lang.TextFile + " (*.txt)|*.txt";
                if (sfd.ShowDialog() == WinForms.DialogResult.OK)
                    File.WriteAllText(sfd.FileName, GetLogText());
            }
        }

        private void OpenTempFolder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Program.TEMP_PATH)) return;
            if (!Directory.Exists(Program.TEMP_PATH)) Directory.CreateDirectory(Program.TEMP_PATH);
            Process.Start("explorer.exe", Program.TEMP_PATH);
        }

        private async void ClearTempFolder_Click(object sender, RoutedEventArgs e)
        {
            Running(Lang.ClearTempFolder);
            try
            {
                await Task.Run(() =>
                {
                    foreach (var subDir in new DirectoryInfo(Program.TEMP_MAIN).EnumerateDirectories())
                    {
                        ToLog(ApktoolEventType.None, String.Format(Lang.DeletingFolder, subDir));
                        DirectoryUtils.Delete(subDir.FullName);
                    }
                    Directory.CreateDirectory(Program.TEMP_PATH);
                });
                Done();
            }
            catch (Exception ex) { Error(ex); }
        }

        private void Exit_Click(object sender, RoutedEventArgs e) => Close();
        private void OpenSettings_Click(object sender, RoutedEventArgs e) => new SettingsWindow { Owner = this }.ShowDialog();
        private void CheckUpdate_Click(object sender, RoutedEventArgs e) => updateCheker?.CheckAsync();
        private void ReportIssue_Click(object sender, RoutedEventArgs e) => Process.Start("https://github.com/AndnixSH/APKToolGUI/issues/new/choose");
        private void ApktoolIssues_Click(object sender, RoutedEventArgs e) => Process.Start("https://github.com/iBotPeaches/Apktool/issues?q=is%3Aissue");
        private void BaksmaliIssues_Click(object sender, RoutedEventArgs e) => Process.Start("https://github.com/JesusFreke/smali/issues?q=is%3Aissue");
        private void About_Click(object sender, RoutedEventArgs e) => new AboutWindow { Owner = this }.ShowDialog();

        private string GetLogText()
        {
            FlushLogNow();
            return logTxtBox.GetText();
        }

        private void LogCopy_Click(object sender, RoutedEventArgs e)
        {
            FlushLogNow();
            logTxtBox.CopySelection();
        }

        private void LogCopyAll_Click(object sender, RoutedEventArgs e)
        {
            FlushLogNow();
            logTxtBox.CopyAll();
        }

        private void LogClear_Click(object sender, RoutedEventArgs e)
        {
            ClearLogQueue();
            logTxtBox.Clear();
        }

        #endregion
    }
}
