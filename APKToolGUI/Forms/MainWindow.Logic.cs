using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using APKToolGUI.ApkTool;
using APKToolGUI.Properties;
using APKToolGUI.Utils;
using Ionic.Zip;
using Microsoft.WindowsAPICodePack.Taskbar;
using Ookii.Dialogs.WinForms;
using Lang = APKToolGUI.Languages.Language;
using WinForms = System.Windows.Forms;
using DColor = System.Drawing.Color;
using Res = APKToolGUI.Properties.Resources;

namespace APKToolGUI.Forms
{
    public partial class MainWindow
    {
        #region Log & status

        internal void ToStatus(string message, System.Drawing.Image statusImage)
        {
            BeginInvokeOnUIThread(() =>
            {
                statusText.Text = message?.Replace("\n", "").Replace("\r", "");
                statusIcon.Source = ToBitmapSource(statusImage);
            });
        }

        internal void ToLog(string time, string message, DColor backColor)
        {
            Debug.WriteLine(time + " " + message);
            EnqueueLog(time + " " + message, GetBrush(backColor));
        }

        #region Log batching

        // Tool output can stream thousands of lines; queue them and flush in coalesced batches on the
        // UI thread (one Background dispatcher hop per burst instead of per line). The LogView itself
        // virtualizes rendering, so total cost stays O(visible lines).
        private readonly System.Collections.Concurrent.ConcurrentQueue<KeyValuePair<string, Brush>> _logQueue =
            new System.Collections.Concurrent.ConcurrentQueue<KeyValuePair<string, Brush>>();
        private int _logFlushQueued;

        private static readonly Dictionary<int, Brush> _brushCache = new Dictionary<int, Brush>();

        private static Brush GetBrush(DColor c)
        {
            int key = c.ToArgb();
            lock (_brushCache)
            {
                Brush b;
                if (!_brushCache.TryGetValue(key, out b))
                {
                    b = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
                    b.Freeze();
                    _brushCache[key] = b;
                }
                return b;
            }
        }

        private void EnqueueLog(string text, Brush brush)
        {
            _logQueue.Enqueue(new KeyValuePair<string, Brush>(text, brush));
            if (System.Threading.Interlocked.Exchange(ref _logFlushQueued, 1) == 0)
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(FlushLog));
        }

        private void FlushLog()
        {
            System.Threading.Interlocked.Exchange(ref _logFlushQueued, 0);
            KeyValuePair<string, Brush> item;
            while (_logQueue.TryDequeue(out item))
                logTxtBox.AppendLine(item.Key, item.Value);
        }

        private void FlushLogNow()
        {
            if (Dispatcher.CheckAccess()) FlushLog();
            else Dispatcher.Invoke(new Action(FlushLog));
        }

        private void ClearLogQueue()
        {
            KeyValuePair<string, Brush> _;
            while (_logQueue.TryDequeue(out _)) { }
        }

        #endregion

        internal void ToLog(ApktoolEventType eventType, string message)
        {
            if (String.IsNullOrWhiteSpace(message) || message.Contains("_JAVA_OPTIONS"))
                return;

            bool dark = Program.IsDarkTheme();
            DColor color = DColor.Black;
            switch (eventType)
            {
                case ApktoolEventType.None: if (dark) color = DColor.White; break;
                case ApktoolEventType.Success: color = dark ? DColor.LightGreen : DColor.DarkGreen; break;
                case ApktoolEventType.Infomation: color = dark ? DColor.LightBlue : DColor.Blue; break;
                case ApktoolEventType.Error: color = dark ? DColor.LightPink : DColor.Red; break;
                case ApktoolEventType.Warning: color = dark ? DColor.DarkOrange : DColor.Orange; break;
                case ApktoolEventType.Unknown: if (dark) color = DColor.White; break;
            }
            ToLog(DateTime.Now.ToString("[HH:mm:ss]"), message, color);
        }

        internal void Running(string msg)
        {
            InvokeOnUIThread(() =>
            {
                if (_hwnd != IntPtr.Zero) TaskbarManager.Instance.SetProgressState(TaskbarProgressBarState.Indeterminate, _hwnd);
                progressBar.IsIndeterminate = true;
                progressBar.Visibility = Visibility.Visible;
                ClearLog();
            });
            ActionButtonsEnabled = false;

            isRunning = true;
            stopwatch.Reset();
            stopwatch.Start();
            lastStartedDate = DateTime.Now.ToString("HH:mm:ss");

            ToLog(ApktoolEventType.Infomation, "=====[ " + msg + " ]=====");
            ToStatus(msg, Res.waiting);
        }

        internal void Done(string msg = null)
        {
            isRunning = false;
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            ToLog(ApktoolEventType.Success, "=====[ " + Lang.AllDone + " ]=====");
            if (msg != null) ToLog(ApktoolEventType.Success, msg);
            ToLog(ApktoolEventType.None, String.Format(Lang.TimeStarted, lastStartedDate));
            ToLog(ApktoolEventType.None, String.Format(Lang.TimeEnded, DateTime.Now.ToString("HH:mm:ss") + " (" + ts.ToString("mm\\:ss") + ")"));

            if (Settings.Default.PlaySoundWhenDone) SystemSounds.Beep.Play();

            InvokeOnUIThread(() =>
            {
                if (_hwnd != IntPtr.Zero) TaskbarManager.Instance.SetProgressValue(1, 1, _hwnd);
                progressBar.IsIndeterminate = false;
                progressBar.Visibility = Visibility.Collapsed;
            });
            ActionButtonsEnabled = true;
            ToStatus(Lang.Done, Res.done);
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

            ToLog(ApktoolEventType.Error, "=====[ " + Lang.Error + " ]=====");
            ToLog(ApktoolEventType.Error, msg);
            ToLog(ApktoolEventType.None, "Time started: " + lastStartedDate);
            ToLog(ApktoolEventType.None, "Time elapsed: " + ts.ToString("mm\\:ss"));

            if (Settings.Default.PlaySoundWhenDone) SystemSounds.Beep.Play();

            InvokeOnUIThread(() =>
            {
                if (_hwnd != IntPtr.Zero) TaskbarManager.Instance.SetProgressValue(1, 1, _hwnd);
                progressBar.IsIndeterminate = false;
                progressBar.Visibility = Visibility.Collapsed;
            });
            ActionButtonsEnabled = true;
            ToStatus(status ?? msg, Res.error);
        }

        internal void ClearLog()
        {
            if (Settings.Default.ClearLogBeforeAction)
            {
                ClearLogQueue();
                logTxtBox.Clear();
            }
        }

        internal void ShowMessage(string message, WinForms.MessageBoxIcon status)
        {
            WinForms.MessageBox.Show(message, WinForms.Application.ProductName, WinForms.MessageBoxButtons.OK, status);
        }

        private bool ActionButtonsEnabled
        {
            set
            {
                BeginInvokeOnUIThread(() =>
                {
                    button_BUILD_Build.IsEnabled = value;
                    button_DECODE_Decode.IsEnabled = value;
                    button_IF_InstallFramework.IsEnabled = value;
                    button_ZIPALIGN_Align.IsEnabled = value;
                    button_SIGN_Sign.IsEnabled = value;
                    decSmaliBtn.IsEnabled = value;
                    comSmaliBtn.IsEnabled = value;
                    mergeApkBtn.IsEnabled = value;
                });
            }
        }

        private bool AdbActionButtonsEnabled
        {
            set
            {
                InvokeOnUIThread(() =>
                {
                    killAdbBtn.IsEnabled = value;
                    refreshDevicesBtn.IsEnabled = value;
                    installApkBtn.IsEnabled = value;
                    devicesListBox.IsEnabled = value;
                    apkPathAdbTxtBox.IsEnabled = value;
                    selApkAdbBtn.IsEnabled = value;
                    setVendorChkBox.IsEnabled = value;
                    overrideAbiCheckBox.IsEnabled = value;
                    overrideAbiComboBox.IsEnabled = value;
                });
            }
        }

        private void PromptCancel()
        {
            if (WinForms.MessageBox.Show(Lang.CancelProcess, WinForms.Application.ProductName, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question) == WinForms.DialogResult.Yes)
                CancelProcess();
        }

        private void CancelProcess()
        {
            try
            {
                ToStatus(Lang.PleaseWait, Res.waiting);
                apkeditor?.Cancel();
                apktool?.Cancel();
                baksmali?.Cancel();
                smali?.Cancel();
                zipalign?.Cancel();
                signapk?.Cancel();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                ActionButtonsEnabled = true;
            }
        }

        internal void Save()
        {
            Settings.Default.Sign_Schemev1 = schemev1ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev2 = schemev2ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev3 = schemev3ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev4 = schemev4ComboBox.SelectedIndex;
            Settings.Default.Adb_OverrideAbi = overrideAbiComboBox.SelectedIndex;
            Settings.Default.UseApkeditor = menuUseApkEditor.IsChecked;
            Settings.Default.Save();
        }

        private void InvokeOnUIThread(Action action)
        {
            if (Dispatcher.CheckAccess()) action();
            else Dispatcher.Invoke(action);
        }
        private void BeginInvokeOnUIThread(Action action)
        {
            if (Dispatcher.CheckAccess()) action();
            else Dispatcher.BeginInvoke(action);
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private static BitmapSource ToBitmapSource(System.Drawing.Image image)
        {
            if (image == null) return null;
            using (var bmp = new System.Drawing.Bitmap(image))
            {
                IntPtr h = bmp.GetHbitmap();
                try
                {
                    var src = Imaging.CreateBitmapSourceFromHBitmap(h, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    src.Freeze();
                    return src;
                }
                finally { DeleteObject(h); }
            }
        }

        private static void SetRich(RichTextBox rtb, string text)
        {
            rtb.Document.Blocks.Clear();
            rtb.Document.Blocks.Add(new Paragraph(new Run(text ?? "")) { Margin = new Thickness(0) });
        }

        #endregion

        #region Tool init + data received

        private void InitializeUpdateChecker()
        {
            updateCheker = new UpdateChecker("https://repo.andnixsh.com/tools/APKToolGUI/version.txt", Version.Parse(WinForms.Application.ProductVersion));
            updateCheker.Completed += UpdateCheker_Completed;
        }

        private void UpdateCheker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is UpdateChecker.Result result)
            {
                switch (result.State)
                {
                    case UpdateChecker.State.NeedUpdate:
                        if (WinForms.MessageBox.Show(Lang.UpdateNewVersion + "\n\n" + result.Changelog, WinForms.Application.ProductName, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question) == WinForms.DialogResult.Yes)
                            Process.Start("https://repo.andnixsh.com/tools/APKToolGUI/APKToolGUI.zip");
                        break;
                    case UpdateChecker.State.NoUpdate:
                        if (!result.Silently)
                            WinForms.MessageBox.Show(Lang.UpdateNoUpdates, WinForms.Application.ProductName, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Information);
                        break;
                    case UpdateChecker.State.Error:
                        if (!result.Silently)
                            WinForms.MessageBox.Show(Lang.ErrorUpdateChecking + " " + Environment.NewLine + result.Message, WinForms.Application.ProductName, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
                        break;
                }
                Settings.Default.LastUpdateCheck = DateTime.Now;
            }
        }

        private void InitializeApkEditor()
        {
            apkeditor = new ApkEditor(javaPath, Program.APKEDITOR_PATH);
            apkeditor.ApkEditorOutputDataRecieved += (s, e) => ToLog(ApktoolEventType.None, e.Message);
            apkeditor.ApkEditorErrorDataRecieved += (s, e) => ToLog(ApktoolEventType.Error, e.Message);
        }

        private void InitializeAPKTool()
        {
            string apktoolPath = Settings.Default.UseCustomApktool ? Settings.Default.ApktoolPath : Program.APKTOOL_PATH;
            apktool = new Apktool(javaPath, apktoolPath);
            apktool.ApktoolOutputDataRecieved += (s, e) => ToLog(e.EventType, e.Message);
            apktool.ApktoolErrorDataRecieved += (s, e) => ToLog(e.EventType == ApktoolEventType.Unknown ? ApktoolEventType.Error : e.EventType, e.Message);
        }

        private void InitializeBaksmali()
        {
            baksmali = new Baksmali(javaPath, Program.BAKSMALI_PATH);
            baksmali.BaksmaliOutputDataRecieved += (s, e) => ToLog(ApktoolEventType.None, e.Message);
            baksmali.BaksmaliErrorDataRecieved += (s, e) => ToLog(ApktoolEventType.Error, e.Message);
        }

        private void InitializeSmali()
        {
            smali = new Smali(javaPath, Program.SMALI_PATH);
            smali.SmaliOutputDataRecieved += (s, e) => ToLog(ApktoolEventType.None, e.Message);
            smali.SmaliErrorDataRecieved += (s, e) => ToLog(ApktoolEventType.Error, e.Message);
        }

        private void InitializeZipalign()
        {
            zipalign = new Zipalign(Program.ZIPALIGN_PATH);
            zipalign.OutputDataReceived += (s, e) => ToLog(ApktoolEventType.None, e.Data);
            zipalign.ErrorDataReceived += (s, e) => ToLog(ApktoolEventType.Error, e.Data);
        }

        private void InitializeSignapk()
        {
            signapk = new Signapk(javaPath, Program.APKSIGNER_PATH);
            signapk.SignapkOutputDataRecieved += (s, e) => ToLog(ApktoolEventType.None, e.Message);
            signapk.SignapkErrorDataRecieved += (s, e) => ToLog(ApktoolEventType.Error, e.Message);
        }

        private void InitializeAdb()
        {
            adb = new Adb(Program.ADB_PATH);
            adb.OutputDataReceived += (s, e) => ToLog(ApktoolEventType.None, e.Data);
            adb.ErrorDataReceived += (s, e) => ToLog(ApktoolEventType.Error, e.Data);
        }

        public async void SetApktoolPath()
        {
            InitializeAPKTool();
            if (!String.IsNullOrWhiteSpace(apktool.Version))
                ToLog(ApktoolEventType.None, $"{Lang.APKToolVersion} \"{apktool.Version}\"");
            else
                ToLog(ApktoolEventType.Error, Lang.CantDetectApktoolVersion);

            if (WinForms.MessageBox.Show(Lang.ClearFrameworkPrompt, WinForms.Application.ProductName, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Information) == WinForms.DialogResult.Yes)
                await ClearFramework();
        }

        #endregion

        #region Operations

        internal async Task<int> ClearFramework()
        {
            int code = 0;
            ToLog(ApktoolEventType.Infomation, "=====[ " + Lang.ClearingFramework + " ]=====");
            ToStatus(Lang.ClearingFramework, Res.waiting);
            try
            {
                await Task.Run(() =>
                {
                    if (apktool.ClearFramework() == 0) Done(Lang.FrameworkCacheCleared);
                    else Error(Lang.ErrorClearingFw);
                });
            }
            catch (Exception ex) { Error(ex); code = 1; }
            return code;
        }

        internal async Task<int> MergeAndDecompile(string inputSplitApk)
        {
            int code = 0;
            Running(Lang.MergingApk);
            bool useApkEditor = menuUseApkEditor.IsChecked == true;

            string tempApk = Path.Combine(Program.TEMP_PATH, "dec.apk");
            string tempDecApk = Path.Combine(Program.TEMP_PATH, "dec");
            string splitDir = Path.Combine(Program.TEMP_PATH, "SplitTmp");
            string extractedDir = Path.Combine(splitDir, "ExtractedApks");

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
                        ToLog(ApktoolEventType.Infomation, Lang.ClearingFramework);
                        if (apktool.ClearFramework() == 0) ToLog(ApktoolEventType.Success, Lang.FrameworkCacheCleared);
                        else ToLog(ApktoolEventType.Error, Lang.ErrorClearingFw);
                    }

                    ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, inputSplitApk));
                    ToLog(ApktoolEventType.None, Lang.ExtractingAllApkFiles);
                    ZipUtils.ExtractAll(inputSplitApk, extractedDir, true);

                    ToLog(ApktoolEventType.None, Lang.MergingApkEditor);
                    code = apkeditor.Merge(extractedDir, tempApk);
                    if (code == 0)
                    {
                        code = useApkEditor ? apkeditor.Decompile(tempApk, tempDecApk) : apktool.Decompile(tempApk, tempDecApk);
                        if (code == 0)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Lang.MoveTempApkFileToOutput, tempDecApk, outputDir));
                            DirectoryUtils.Delete(outputDir);
                            DirectoryUtils.Copy(tempDecApk, outputDir);
                            BeginInvokeOnUIThread(() => textBox_BUILD_InputProjectDir.Text = outputDir);

                            ToLog(ApktoolEventType.None, String.Format(Lang.DecompilingSuccessfullyCompleted, outputDir));
                            if (Settings.Default.Decode_FixError)
                            {
                                if (ApkFixer.FixAndroidManifest(outputDir)) ToLog(ApktoolEventType.None, Lang.FixAndroidManifest);
                                if (ApkFixer.FixApktoolYml(outputDir)) ToLog(ApktoolEventType.None, Lang.FixApktoolYml);
                                if (ApkFixer.RemoveApkToolDummies(outputDir)) ToLog(ApktoolEventType.None, Lang.RemoveApkToolDummies);
                            }
                            ToLog(ApktoolEventType.None, String.Format(Lang.MergeFinishedMoveDir, outputDir));
                            Done();
                        }
                        else Error(Lang.ErrorDecompiling);
                    }
                    else Error(Lang.ErrorMerging);
                });
            }
            catch (Exception ex) { code = 1; Error(ex); }
            return code;
        }

        internal async Task<int> Merge(string inputSplitApk)
        {
            int code = 0;
            Running(Lang.MergingApk);
            string tempFile = Path.Combine(Program.TEMP_PATH, "tempsplit");
            string tempFileMerged = Path.Combine(Program.TEMP_PATH, "tempsplitmerged");
            string outputFile = PathUtils.GetDirectoryNameWithoutExtension(inputSplitApk) + " merged.apk";
            try
            {
                await Task.Run(() =>
                {
                    ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, inputSplitApk));
                    ToLog(ApktoolEventType.None, Lang.MergingApkEditor);
                    ToLog(ApktoolEventType.None, String.Format(Lang.CopyFileToTemp, inputSplitApk, tempFile));
                    FileUtils.Copy(inputSplitApk, tempFile, true);
                    code = apkeditor.Merge(tempFile, tempFileMerged);
                    if (code == 0)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Lang.MoveTempApkToOutput, tempFile, outputFile));
                        FileUtils.Move(tempFileMerged, outputFile, true);
                        Done();
                    }
                    else Error(Lang.ErrorMerging);
                });
            }
            catch (Exception ex) { code = 1; Error(ex); }
            return code;
        }

        internal async Task<int> Decompile(string inputApk)
        {
            int code = 0;
            bool useApkEditor = menuUseApkEditor.IsChecked == true;
            Running(Lang.Decoding);

            string outputDir = PathUtils.GetDirectoryNameWithoutExtension(inputApk);
            if (Settings.Default.Decode_UseOutputDir && !IgnoreOutputDirContextMenu)
                outputDir = Path.Combine(Settings.Default.Decode_OutputDir, Path.GetFileNameWithoutExtension(inputApk));

            string tempApk = Path.Combine(Program.TEMP_PATH, "dec.apk");
            string outputTempDir = tempApk.Replace(".apk", "");
            string outputDecDir = outputDir;

            try
            {
                if (!Settings.Default.Decode_Force && Directory.Exists(outputDir))
                {
                    ToLog(ApktoolEventType.Error, String.Format(Lang.DecodeDesDirExists, outputDir));
                    Done();
                    return 1;
                }
                await Task.Run(() =>
                {
                    if (Settings.Default.Framework_ClearBeforeDecode && !Settings.Default.UseApkeditor)
                    {
                        ToLog(ApktoolEventType.Infomation, Lang.ClearingFramework);
                        if (apktool.ClearFramework() == 0) ToLog(ApktoolEventType.Success, Lang.FrameworkCacheCleared);
                        else ToLog(ApktoolEventType.Error, Lang.ErrorClearingFw);
                    }

                    if (Settings.Default.Utf8FilenameSupport)
                    {
                        DirectoryUtils.Delete(outputTempDir);
                        ToLog(ApktoolEventType.None, String.Format(Lang.CopyFileToTemp, inputApk, tempApk));
                        FileUtils.Copy(inputApk, tempApk, true);
                        inputApk = tempApk;
                        outputDecDir = outputTempDir;
                    }

                    code = useApkEditor ? apkeditor.Decompile(inputApk, outputDecDir) : apktool.Decompile(inputApk, outputDecDir);

                    if (code == 0)
                    {
                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Lang.MoveTempApkFileToOutput, outputTempDir, outputDir));
                            DirectoryUtils.Delete(outputDir);
                            DirectoryUtils.Copy(outputTempDir, outputDir);
                        }
                        BeginInvokeOnUIThread(() => textBox_BUILD_InputProjectDir.Text = outputDir);

                        ToLog(ApktoolEventType.None, String.Format(Lang.DecompilingSuccessfullyCompleted, outputDir));
                        if (Settings.Default.Decode_FixError && !useApkEditor)
                        {
                            if (ApkFixer.FixAndroidManifest(outputDir)) ToLog(ApktoolEventType.None, Lang.FixAndroidManifest);
                            if (ApkFixer.FixApktoolYml(outputDir)) ToLog(ApktoolEventType.None, Lang.FixApktoolYml);
                            if (ApkFixer.RemoveApkToolDummies(outputDir)) ToLog(ApktoolEventType.None, Lang.RemoveApkToolDummies);
                        }
                        Done();
                    }
                    else Error(Lang.ErrorDecompiling);
                });
            }
            catch (Exception ex) { code = 1; Error(ex.ToString(), Lang.ErrorDecompiling); }
            return code;
        }

        internal async Task<int> Build(string inputFolder)
        {
            int code = 0;
            Running(Lang.Build);
            ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, inputFolder));
            string device = selAdbDeviceLbl.Text;

            try
            {
                await Task.Factory.StartNew(() =>
                {
                    string outputFile = inputFolder + " compiled.apk";
                    string outputUnsignedApk = inputFolder + " unsigned.apk";
                    if (Settings.Default.Build_SignAfterBuild) outputFile = inputFolder + " signed.apk";
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
                        ToLog(ApktoolEventType.None, String.Format(Lang.CopyFolderToTemp, inputFolder, tempDecApkFolder));
                        DirectoryUtils.Delete(tempDecApkFolder);
                        DirectoryUtils.Copy(inputFolder, tempDecApkFolder);
                        inputFolder = tempDecApkFolder;
                        outputFile = outputTempApk;
                    }

                    code = isDecompiledUsingApkEditor ? apkeditor.Build(inputFolder, outputFile) : apktool.Build(inputFolder, outputFile);

                    if (code == 0)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Lang.CompilingSuccessfullyCompleted, outputFile));

                        if (Settings.Default.Build_CreateUnsignedApk)
                        {
                            ToStatus(Lang.CreateUnsignedApk, Res.waiting);
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Lang.CreateUnsignedApk + " ]=====");
                            if (Directory.Exists(Path.Combine(inputFolder, "original", "META-INF")))
                            {
                                string unsignedApkPath = Path.Combine(Path.GetDirectoryName(outputCompiledApkFile), Path.GetFileName(outputUnsignedApk));
                                ZipUtils.UpdateDirectory(outputFile, Path.Combine(inputFolder, "original", "META-INF"), "META-INF");
                                if (File.Exists(Path.Combine(inputFolder, "original", "stamp-cert-sha256")))
                                    ZipUtils.UpdateFile(outputFile, Path.Combine(inputFolder, "original", "stamp-cert-sha256"));
                                ToLog(ApktoolEventType.Infomation, String.Format(Lang.CopyFileTo, outputFile, unsignedApkPath));
                                File.Copy(outputFile, unsignedApkPath, true);
                            }
                            else ToLog(ApktoolEventType.Warning, Lang.MetainfNotExist);
                        }

                        if (Settings.Default.Build_ZipalignAfterBuild)
                        {
                            ToStatus(Lang.Aligning, Res.waiting);
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Lang.Aligning + " ]=====");
                            ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, inputFolder));
                            if (zipalign.Align(outputFile, outputFile) == 0) ToLog(ApktoolEventType.None, Lang.Done);
                            else { Error(Lang.ErrorZipalign); return; }
                        }

                        if (Settings.Default.Build_SignAfterBuild)
                        {
                            ToStatus(Lang.Signing, Res.waiting);
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Lang.Signing + " ]=====");
                            ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, inputFolder));
                            if (signapk.Sign(outputFile, outputFile) == 0)
                            {
                                ToLog(ApktoolEventType.None, Lang.Done);
                                if (Settings.Default.AutoDeleteIdsigFile)
                                {
                                    ToLog(ApktoolEventType.None, String.Format(Lang.DeleteFile, outputFile + ".idsig"));
                                    FileUtils.Delete(outputFile + ".idsig");
                                }
                                if (Settings.Default.Sign_InstallApkAfterSign)
                                {
                                    if (!String.IsNullOrEmpty(device))
                                    {
                                        ToStatus(Lang.InstallingApk, Res.waiting);
                                        ToLog(ApktoolEventType.Infomation, "=====[ " + Lang.InstallingApk + " ]=====");
                                        if (adb.Install(device, outputFile) == 0) ToLog(ApktoolEventType.None, Lang.InstallApkSuccessful);
                                        else ToLog(ApktoolEventType.Error, Lang.InstallApkFailed);
                                    }
                                    else ToLog(ApktoolEventType.Error, String.Format(Lang.DeviceNotSelected, outputFile));
                                }
                            }
                            else ToLog(ApktoolEventType.Error, Lang.ErrorSigning);
                        }

                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Lang.MoveTempApkToOutput, outputTempApk, outputCompiledApkFile));
                            FileUtils.Move(outputTempApk, outputCompiledApkFile, true);
                        }
                        Done();
                    }
                    else Error(Lang.ErrorCompiling);
                });
            }
            catch (Exception ex) { Error(ex); code = 1; }
            return code;
        }

        internal async Task<int> Baksmali(string inputFile)
        {
            int code = 0;
            try
            {
                Running(Lang.DecompilingDex);
                ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, inputFile));
                await Task.Run(() =>
                {
                    string outputDir = Path.Combine(Path.GetDirectoryName(inputFile), "dexout", Path.GetFileNameWithoutExtension(inputFile));
                    if (Settings.Default.Baksmali_UseOutputDir && !IgnoreOutputDirContextMenu)
                        outputDir = Path.Combine(Settings.Default.Baksmali_OutputDir, Path.GetFileNameWithoutExtension(inputFile));

                    code = baksmali.Disassemble(inputFile, outputDir);
                    if (code == 0)
                    {
                        BeginInvokeOnUIThread(() => smaliBrowseInputDirTxtBox.Text = outputDir);
                        Done(String.Format(Lang.DecompilingSuccessfullyCompleted, outputDir));
                    }
                    else Error(Lang.ErrorDecompiling);
                });
            }
            catch (Exception ex) { code = 1; Error(ex); }
            return code;
        }

        internal async Task<int> Smali(string inputDir)
        {
            int code = 0;
            try
            {
                Running(Lang.CompilingDex);
                ToLog(ApktoolEventType.None, String.Format(Lang.InputDirectory, inputDir));
                await Task.Run(() =>
                {
                    string outputDir = String.Format("{0}.dex", inputDir);
                    if (Settings.Default.Smali_UseOutputDir && !IgnoreOutputDirContextMenu)
                        outputDir = String.Format("{0}.dex", Path.Combine(Settings.Default.Smali_OutputDir, Path.GetFileNameWithoutExtension(inputDir)));

                    code = smali.Assemble(inputDir, outputDir);
                    if (code == 0) Done(String.Format(Lang.CompilingSuccessfullyCompleted, outputDir));
                    else Error(Lang.ErrorCompiling);
                });
            }
            catch (Exception ex) { Error(ex); code = 1; }
            return code;
        }

        internal async Task<int> Align(string inputFile)
        {
            int code = 0;
            Running(Lang.Aligning);
            ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, inputFile));

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
                        ToLog(ApktoolEventType.None, String.Format(Lang.CopyFileToTemp, inputFile, tempApk));
                        FileUtils.Copy(inputFile, tempApk, true);
                        inputFile = tempApk;
                        outputDir = tempApk;
                    }

                    code = zipalign.Align(inputFile, outputDir);
                    if (code == 0)
                    {
                        if (Settings.Default.Zipalign_SignAfterZipAlign)
                        {
                            ToLog(ApktoolEventType.Infomation, "=====[ " + Lang.Signing + " ]=====");
                            if (signapk.Sign(outputDir, outputDir) == 0)
                            {
                                ToLog(ApktoolEventType.None, Lang.Done);
                                if (Settings.Default.AutoDeleteIdsigFile)
                                {
                                    ToLog(ApktoolEventType.None, String.Format(Lang.DeleteFile, outputDir + ".idsig"));
                                    FileUtils.Delete(outputDir + ".idsig");
                                }
                            }
                            else ToLog(ApktoolEventType.Error, Lang.ErrorSigning);
                        }
                        ToLog(ApktoolEventType.None, String.Format(Lang.ZipalignFileSavedTo, outputDir));
                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Lang.MoveTempApkToOutput, tempApk, outputApkFile));
                            FileUtils.Move(tempApk, outputApkFile, true);
                        }
                        Done();
                    }
                    else Error(Lang.ErrorZipalign);
                });
            }
            catch (Exception ex) { Error(ex); code = 1; }
            return code;
        }

        internal async Task<int> Sign(string input)
        {
            int code = 0;
            Running(Lang.Signing);
            string device = selAdbDeviceLbl.Text;

            string outputFile = input;
            if (Settings.Default.Sign_UseOutputDir && !IgnoreOutputDirContextMenu)
                outputFile = Path.Combine(Settings.Default.Sign_OutputDir, Path.GetFileName(input));
            if (!Settings.Default.Sign_OverwriteInputFile)
                outputFile = PathUtils.GetDirectoryNameWithoutExtension(outputFile) + "_signed.apk";

            string tempApk = Path.Combine(Program.TEMP_PATH, "tempapk.apk");
            string outputApkFile = outputFile;
            ToLog(ApktoolEventType.None, String.Format(Lang.InputFile, input));

            try
            {
                await Task.Run(() =>
                {
                    if (Settings.Default.Utf8FilenameSupport)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Lang.CopyFileToTemp, input, tempApk));
                        FileUtils.Copy(input, tempApk, true);
                        input = tempApk;
                        outputFile = tempApk;
                    }

                    code = signapk.Sign(input, outputFile);
                    if (code == 0)
                    {
                        ToLog(ApktoolEventType.None, String.Format(Lang.SignSuccessfullyCompleted, outputFile));
                        if (Settings.Default.Sign_InstallApkAfterSign)
                        {
                            if (!string.IsNullOrEmpty(device))
                            {
                                ToLog(ApktoolEventType.Infomation, "=====[ " + Lang.InstallingApk + " ]=====");
                                if (adb.Install(device, outputFile) == 0) ToLog(ApktoolEventType.Success, Lang.InstallApkSuccessful);
                                else ToLog(ApktoolEventType.Error, Lang.InstallApkFailed);
                            }
                            else ToLog(ApktoolEventType.Error, String.Format(Lang.DeviceNotSelected, outputFile));
                        }
                        if (Settings.Default.AutoDeleteIdsigFile)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Lang.DeleteFile, outputFile + ".idsig"));
                            FileUtils.Delete(outputFile + ".idsig");
                        }
                        if (Settings.Default.Utf8FilenameSupport)
                        {
                            ToLog(ApktoolEventType.None, String.Format(Lang.MoveTempApkToOutput, tempApk, outputApkFile));
                            FileUtils.Move(tempApk, outputApkFile, true);
                        }
                        Done();
                    }
                    else Error(String.Format(Lang.ErrorSigning, outputFile));
                });
            }
            catch (Exception ex) { code = 1; Error(ex); }
            return code;
        }

        internal async Task<int> ListDevices()
        {
            int code = 0;
            AdbActionButtonsEnabled = false;
            ToLog(ApktoolEventType.None, Lang.GettingDevices);
            ToStatus(Lang.GettingDevices, Res.waiting);

            string devices = null;
            int numOfDevices = 0;
            try
            {
                devicesListBox.Items.Clear();
                await Task.Run(() => { devices = adb.GetDevices(); });
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
            catch (Exception ex) { code = 1; ToLog(ApktoolEventType.Error, ex.ToString()); }

            ToLog(ApktoolEventType.None, numOfDevices != 0 ? String.Format(Lang.DevicesFound, numOfDevices) : Lang.NoDevicesFound);
            ToStatus(Lang.Done, Res.done);
            AdbActionButtonsEnabled = true;
            return code;
        }

        internal async Task<int> Install(string inputApk)
        {
            string device = selAdbDeviceLbl.Text;
            if (String.IsNullOrEmpty(device))
            {
                ToLog(ApktoolEventType.Error, String.Format(Lang.DeviceNotSelected, inputApk));
                return 1;
            }
            int code = 0;
            Running(Lang.InstallingApk);
            AdbActionButtonsEnabled = false;
            ToLog(ApktoolEventType.None, String.Format(Lang.InstallingApkPath, inputApk));
            try
            {
                await Task.Run(() =>
                {
                    code = adb.Install(device, inputApk);
                    if (code == 0) Done(Lang.InstallApkSuccessful);
                    else Error(Lang.InstallApkFailed);
                });
            }
            catch (Exception ex) { code = 1; Error(ex); }
            AdbActionButtonsEnabled = true;
            return code;
        }

        #endregion

        #region APK info

        internal async Task GetApkInfo(string file)
        {
            if (!File.Exists(file)) return;

            ToLog(ApktoolEventType.None, Lang.ParsingApkInfo);
            ToStatus(Lang.ParsingApkInfo, Res.waiting);
            try
            {
                string splitPath = Path.Combine(Program.TEMP_PATH, "SplitInfo");
                var parseResult = await ParseApkInBackgroundAsync(file, splitPath);
                if (parseResult.Success)
                {
                    aapt = parseResult.Aapt;
                    UpdateApkInfoUI(parseResult);
                    var signature = await Task.Run(() => signapk.GetSignature(parseResult.ActualFilePath));
                    InvokeOnUIThread(() => SetRich(sigTxtBox, signature));
                }
                ToLog(ApktoolEventType.Success, Lang.Done);
                ToStatus(Lang.Done, Res.done);
            }
#if DEBUG
            catch (Exception ex) { ToLog(ApktoolEventType.Warning, Lang.ErrorGettingApkInfo + "\n" + ex.ToString()); }
#else
            catch (Exception) { ToLog(ApktoolEventType.Warning, Lang.ErrorGettingApkInfo); }
#endif
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
                                    string extractPath = Path.Combine(splitPath, entry.FileName);
                                    Directory.CreateDirectory(Path.GetDirectoryName(extractPath));
                                    entry.Extract(splitPath, ExtractExistingFileAction.OverwriteSilently);
                                    actualFile = extractPath;
                                    mainApkFound = true;
                                }
                                if (entry.FileName.Contains("lib/armeabi-v7a") && !archList.Contains("armeabi-v7a")) archList.Add("armeabi-v7a");
                                if (entry.FileName.Contains("lib/arm64-v8a") && !archList.Contains("arm64-v8a")) archList.Add("arm64-v8a");
                                if (entry.FileName.Contains("lib/x86") && !archList.Contains("x86")) archList.Add("x86");
                                if (entry.FileName.Contains("lib/x86_64") && !archList.Contains("x86_64")) archList.Add("x86_64");
                            }
                        }
                    }

                    var aaptParser = new AaptParser();
                    var parsed = aaptParser.Parse(actualFile);
                    DirectoryUtils.Delete(splitPath);

                    return new ApkParseResult { Success = parsed, Aapt = aaptParser, Architecture = string.Join(", ", archList), ActualFilePath = actualFile };
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
            try { previousApkIcon?.Dispose(); previousApkIcon = null; } catch { }
            apkIconPicBox.Source = null;

            fileTxtBox.Text = result.Aapt.ApkFile;
            appTxtBox.Text = result.Aapt.AppName;
            packNameTxtBox.Text = result.Aapt.PackageName;
            verTxtBox.Text = result.Aapt.VersionName;
            buildTxtBox.Text = result.Aapt.VersionCode;
            minSdkTxtBox.Text = result.Aapt.MinSdkVersionDetailed;
            targetSdkTxtBox.Text = result.Aapt.TargetSdkVersionDetailed;
            screenTxtBox.Text = result.Aapt.Screens;
            densityTxtBox.Text = result.Aapt.Densities;
            launchActivityTxtBox.Text = result.Aapt.LaunchableActivity;
            archSdkTxtBox.Text = !String.IsNullOrEmpty(result.Aapt.NativeCode) ? result.Aapt.NativeCode : result.Architecture;

            SetRich(permTxtBox, result.Aapt.Permissions);
            SetRich(localsTxtBox, result.Aapt.Locales);
            SetRich(fullInfoTextBox, result.Aapt.FullInfo);
            SetRich(sigTxtBox, Lang.Loading);

            previousApkIcon = BitmapUtils.LoadBitmap(result.Aapt.GetIcon(result.ActualFilePath));
            apkIconPicBox.Source = ToBitmapSource(previousApkIcon);
        }

        private class ApkParseResult
        {
            public bool Success { get; set; }
            public AaptParser Aapt { get; set; }
            public string Architecture { get; set; }
            public string ActualFilePath { get; set; }
        }

        #endregion

        #region Handlers - Decode

        private async void Button_DECODE_BrowseInputAppPath_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = string.Format(Lang.FilterAndroidPackage, "*.apk;*.xapk;*.zip;*.apkm;*.apks");
                if (ofd.ShowDialog() == WinForms.DialogResult.OK)
                {
                    textBox_DECODE_InputAppPath.Text = ofd.FileName;
                    if (!Settings.Default.Decode_DontParseApkInfo) await GetApkInfo(ofd.FileName);
                    if (checkBox_DECODE_OutputDirectory.IsChecked == true)
                        textBox_DECODE_OutputDirectory.Text = Path.Combine(Path.GetDirectoryName(textBox_DECODE_InputAppPath.Text), Path.GetFileNameWithoutExtension(textBox_DECODE_InputAppPath.Text));
                }
            }
        }

        private void Button_DECODE_BrowseFrameDir_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new VistaFolderBrowserDialog())
            {
                if (!String.IsNullOrWhiteSpace(textBox_DECODE_FrameDir.Text)) fbd.SelectedPath = textBox_DECODE_FrameDir.Text;
                if (fbd.ShowDialog() == WinForms.DialogResult.OK) textBox_DECODE_FrameDir.Text = fbd.SelectedPath;
            }
        }

        private void Button_DECODE_BrowseOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new VistaFolderBrowserDialog())
            {
                if (!String.IsNullOrWhiteSpace(textBox_DECODE_OutputDirectory.Text)) fbd.SelectedPath = textBox_DECODE_OutputDirectory.Text;
                else if (!String.IsNullOrWhiteSpace(textBox_DECODE_InputAppPath.Text)) fbd.SelectedPath = Path.GetDirectoryName(textBox_DECODE_InputAppPath.Text);
                if (fbd.ShowDialog() == WinForms.DialogResult.OK) textBox_DECODE_OutputDirectory.Text = fbd.SelectedPath;
            }
        }

        private async void Button_DECODE_Decode_Click(object sender, RoutedEventArgs e)
        {
            string inputFile = textBox_DECODE_InputAppPath.Text;
            if (File.Exists(inputFile))
            {
                if (checkBox_DECODE_UseFramework.IsChecked == true && !Directory.Exists(textBox_DECODE_FrameDir.Text))
                { ShowMessage(Lang.DecodeSelectedFrameworkNotExist, WinForms.MessageBoxIcon.Warning); return; }
                if (checkBox_DECODE_OutputDirectory.IsChecked == true)
                {
                    if (String.IsNullOrWhiteSpace(Settings.Default.Decode_OutputDir)) { ShowMessage(Lang.DecodeDirNotSelected, WinForms.MessageBoxIcon.Warning); return; }
                    if (!PathUtils.IsValidPath(Settings.Default.Decode_OutputDir)) { ShowMessage(Lang.DecodeCouldNotCreate, WinForms.MessageBoxIcon.Warning); return; }
                }
                if (inputFile.ContainsAny(".xapk", ".zip", ".apks", ".apkm")) await MergeAndDecompile(inputFile);
                else await Decompile(inputFile);
            }
            else WinForms.MessageBox.Show(Lang.WarningFileForDecodingNotSelected, WinForms.Application.ProductName, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
        }

        #endregion

        #region Handlers - Build

        private void Button_BUILD_BrowseAaptPath_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = Lang.ExecutableFile + "|*.exe";
                if (!String.IsNullOrWhiteSpace(textBox_BUILD_AaptPath.Text))
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(textBox_BUILD_AaptPath.Text);
                    ofd.FileName = Path.GetFileName(textBox_BUILD_AaptPath.Text);
                }
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) textBox_BUILD_AaptPath.Text = ofd.FileName;
            }
        }

        private void Button_BUILD_BrowseFrameDir_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new VistaFolderBrowserDialog())
            {
                if (!String.IsNullOrWhiteSpace(textBox_BUILD_FrameDir.Text)) fbd.SelectedPath = textBox_BUILD_FrameDir.Text;
                if (fbd.ShowDialog() == WinForms.DialogResult.OK) textBox_BUILD_FrameDir.Text = fbd.SelectedPath;
            }
        }

        private void Button_BUILD_BrowseOutputAppPath_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) textBox_BUILD_OutputAppPath.Text = dlg.SelectedPath;
        }

        private void Button_BUILD_BrowseInputProjectDir_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) textBox_BUILD_InputProjectDir.Text = dlg.SelectedPath;
        }

        private async void Button_BUILD_Build_Click(object sender, RoutedEventArgs e)
        {
            string decApkDir = textBox_BUILD_InputProjectDir.Text;
            if (Directory.Exists(decApkDir)) await Build(decApkDir);
            else WinForms.MessageBox.Show(Lang.WarningDecodingFolderNotSelected, WinForms.Application.ProductName, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
        }

        #endregion

        #region Handlers - Sign

        private void Button_SIGN_BrowsePublicKey_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = string.Format(Lang.FilterPublicKey, "*.pem");
                if (File.Exists(textBox_SIGN_PublicKey.Text))
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(textBox_SIGN_PublicKey.Text);
                    ofd.FileName = Path.GetFileNameWithoutExtension(textBox_SIGN_PublicKey.Text);
                }
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) textBox_SIGN_PublicKey.Text = Program.GetPortablePath(ofd.FileName);
            }
        }

        private void Button_SIGN_BrowsePrivateKey_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = string.Format(Lang.FilterPrivateKey, "*.pk8");
                if (File.Exists(textBox_SIGN_PrivateKey.Text))
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(textBox_SIGN_PrivateKey.Text);
                    ofd.FileName = Path.GetFileNameWithoutExtension(textBox_SIGN_PrivateKey.Text);
                }
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) textBox_SIGN_PrivateKey.Text = Program.GetPortablePath(ofd.FileName);
            }
        }

        private void Button_SIGN_BrowseOutputFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) textBox_SIGN_OutputFile.Text = dlg.SelectedPath;
        }

        private async void Button_SIGN_BrowseInputFile_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = string.Format(Lang.FilterApkJarZip, "*.apk;*.jar;*.zip");
                if (ofd.ShowDialog() == WinForms.DialogResult.OK)
                {
                    textBox_SIGN_InputFile.Text = ofd.FileName;
                    await GetApkInfo(ofd.FileName);
                    textBox_SIGN_OutputFile.Text = String.Format("{0}{1}{2}_signed{3}",
                        Path.GetDirectoryName(textBox_SIGN_InputFile.Text), Path.DirectorySeparatorChar,
                        Path.GetFileNameWithoutExtension(textBox_SIGN_InputFile.Text), Path.GetExtension(textBox_SIGN_InputFile.Text));
                }
            }
        }

        private async void Button_SIGN_Sign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
                if (!File.Exists(Settings.Default.Sign_PublicKey)) { ShowMessage(Lang.SignPublicKeyNotFound, WinForms.MessageBoxIcon.Warning); return; }
                if (!File.Exists(Settings.Default.Sign_PrivateKey)) { ShowMessage(Lang.SignPrivateKeyNotFound, WinForms.MessageBoxIcon.Warning); return; }
                if (!File.Exists(textBox_SIGN_InputFile.Text)) { ShowMessage(Lang.SignInputFileNotFound, WinForms.MessageBoxIcon.Warning); return; }
                await Sign(Settings.Default.Sign_InputFile);
            }
            catch (Exception ex) { ToLog(ApktoolEventType.Error, ex.Message); }
        }

        private void SelectKeyStoreFileBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = string.Format(Lang.FilterKeystore, "*.keystore;*.jks");
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) keyStoreFileTxtBox.Text = ofd.FileName;
            }
        }

        private void SchemeComboBox_Changed(object sender, SelectionChangedEventArgs e)
        {
            Settings.Default.Sign_Schemev1 = schemev1ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev2 = schemev2ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev3 = schemev3ComboBox.SelectedIndex;
            Settings.Default.Sign_Schemev4 = schemev4ComboBox.SelectedIndex;
        }

        #endregion

        #region Handlers - Zipalign

        private void ApplyZipalignCheckSwitch()
        {
            bool enabled = !(checkBox_ZIPALIGN_CheckAlignment.IsChecked == true);
            checkBox_ZIPALIGN_Recompress.IsEnabled = enabled;
            checkBox_ZIPALIGN_OverwriteOutputFile.IsEnabled = enabled;
        }

        private void Button_ZIPALIGN_BrowseOutputFile_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) textBox_ZIPALIGN_OutputFile.Text = dlg.SelectedPath;
        }

        private async void Button_ZIPALIGN_BrowseInputFile_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = Lang.ZIPArchives + " (*.apk)|*.apk";
                if (File.Exists(textBox_ZIPALIGN_InputFile.Text))
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(textBox_ZIPALIGN_InputFile.Text);
                    ofd.FileName = Path.GetFileName(textBox_ZIPALIGN_InputFile.Text);
                }
                if (ofd.ShowDialog() == WinForms.DialogResult.OK)
                {
                    textBox_ZIPALIGN_InputFile.Text = ofd.FileName;
                    await GetApkInfo(ofd.FileName);
                    if (!(checkBox_ZIPALIGN_CheckAlignment.IsChecked == true))
                        textBox_ZIPALIGN_OutputFile.Text = String.Format("{0}\\{1}_zipaligned{2}", Path.GetDirectoryName(ofd.FileName), Path.GetFileNameWithoutExtension(ofd.FileName), Path.GetExtension(ofd.FileName));
                }
            }
        }

        private async void Button_ZIPALIGN_Align_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(textBox_ZIPALIGN_InputFile.Text)) { ShowMessage(Lang.ErrorSelectedFileNotExist, WinForms.MessageBoxIcon.Warning); return; }
            await Align(Settings.Default.Zipalign_InputFile);
        }

        #endregion

        #region Handlers - Framework

        private void Button_IF_BrowseFrameDir_Click(object sender, RoutedEventArgs e)
        {
            clearFwBeforeDecodeChkBox.IsChecked = false;
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) textBox_IF_FrameDir.Text = dlg.SelectedPath;
        }

        private void Button_IF_BrowseInputFramePath_Click(object sender, RoutedEventArgs e)
        {
            clearFwBeforeDecodeChkBox.IsChecked = false;
            using (var ofd = new WinForms.OpenFileDialog())
            {
                if (File.Exists(textBox_IF_InputFramePath.Text))
                {
                    ofd.InitialDirectory = Path.GetDirectoryName(textBox_IF_InputFramePath.Text);
                    ofd.FileName = Path.GetFileNameWithoutExtension(textBox_IF_InputFramePath.Text);
                }
                ofd.Filter = string.Format(Lang.FilterApk, "*.apk");
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) textBox_IF_InputFramePath.Text = ofd.FileName;
            }
        }

        private async void Button_IF_InstallFramework_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_IF_FramePath.IsChecked == true && (String.IsNullOrWhiteSpace(textBox_IF_FrameDir.Text) || !Directory.Exists(textBox_IF_FrameDir.Text)))
            { ShowMessage(Lang.ErrorSelectingFrameworkDirectory, WinForms.MessageBoxIcon.Warning); return; }
            if (checkBox_IF_Tag.IsChecked == true && String.IsNullOrWhiteSpace(textBox_IF_Tag.Text))
            { ShowMessage(Lang.ErrorEnteringFrameworkTag, WinForms.MessageBoxIcon.Warning); return; }
            if (!File.Exists(textBox_IF_InputFramePath.Text))
            { ShowMessage(Lang.ErrorSelectingFrameworkFile, WinForms.MessageBoxIcon.Warning); return; }

            Running(Lang.InstallingFramework);
            ToLog(ApktoolEventType.None, Lang.InstallingFramework + " " + Path.GetFileName(textBox_IF_InputFramePath.Text));
            await Task.Factory.StartNew(() =>
            {
                if (apktool.InstallFramework() == 0) Done(Lang.FrameworkInstalled);
                else Error(Lang.FrameworkInstallationNotStarted);
            });
        }

        private async void ClearFwBtn_Click(object sender, RoutedEventArgs e)
        {
            Running(Lang.ClearingFramework);
            await ClearFramework();
        }

        private void OpenFwFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            if (checkBox_IF_FramePath.IsChecked == true && Directory.Exists(textBox_IF_FrameDir.Text)) Process.Start("explorer.exe", textBox_IF_FrameDir.Text);
            else if (Directory.Exists(Program.FRAMEWORK_DIR)) Process.Start("explorer.exe", Program.FRAMEWORK_DIR);
            else ToLog(ApktoolEventType.Error, Lang.ErrorSelectedFolderNotExist);
        }

        #endregion

        #region Handlers - Baksmali / Smali

        private void BaksmaliBrowseOutputBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) baksmaliBrowseOutputTxtBox.Text = dlg.SelectedPath;
        }

        private void BaksmaliBrowseInputDexBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = string.Format(Lang.FilterDex, "*.dex");
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) baksmaliBrowseInputDexTxtBox.Text = ofd.FileName;
            }
        }

        private async void DecSmaliBtn_Click(object sender, RoutedEventArgs e)
        {
            if (baksmaliUseOutputChkBox.IsChecked == true && (String.IsNullOrWhiteSpace(baksmaliBrowseOutputTxtBox.Text) || !Directory.Exists(baksmaliBrowseOutputTxtBox.Text)))
            { ShowMessage(Lang.ErrorSelectedOutputFolderNotExist, WinForms.MessageBoxIcon.Warning); return; }
            if (!File.Exists(baksmaliBrowseInputDexTxtBox.Text)) { ShowMessage(Lang.ErrorSelectedFileNotExist, WinForms.MessageBoxIcon.Warning); return; }
            await Baksmali(Settings.Default.Baksmali_InputDexFile);
        }

        private void SmaliBrowseOutputBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) smaliBrowseOutputTxtBox.Text = dlg.SelectedPath;
        }

        private void SmaliBrowseInputDirBtn_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new VistaFolderBrowserDialog { ShowNewFolderButton = true };
            if (dlg.ShowDialog() == WinForms.DialogResult.OK) smaliBrowseInputDirTxtBox.Text = dlg.SelectedPath;
        }

        private async void ComSmaliBtn_Click(object sender, RoutedEventArgs e)
        {
            if (smaliUseOutputChkBox.IsChecked == true && (String.IsNullOrWhiteSpace(smaliBrowseOutputTxtBox.Text) || !Directory.Exists(smaliBrowseOutputTxtBox.Text)))
            { ShowMessage(Lang.ErrorSelectedOutputFolderNotExist, WinForms.MessageBoxIcon.Warning); return; }
            if (!Directory.Exists(smaliBrowseInputDirTxtBox.Text)) { ShowMessage(Lang.ErrorSelectedFileNotExist, WinForms.MessageBoxIcon.Warning); return; }
            await Smali(Settings.Default.Smali_InputDir);
        }

        #endregion

        #region Handlers - ADB

        private void OverrideAbiComboBox_Changed(object sender, SelectionChangedEventArgs e) => Settings.Default.Adb_OverrideAbi = overrideAbiComboBox.SelectedIndex;
        private async void RefreshDevicesBtn_Click(object sender, RoutedEventArgs e) => await ListDevices();

        private async void KillAdbBtn_Click(object sender, RoutedEventArgs e)
        {
            if (WinForms.MessageBox.Show(Lang.ConfirmKillingAdbServer, WinForms.Application.ProductName, WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question) == WinForms.DialogResult.Yes)
            {
                adb.KillProcess();
                await ListDevices();
            }
        }

        private async void InstallApkBtn_Click(object sender, RoutedEventArgs e)
        {
            string inputFile = apkPathAdbTxtBox.Text;
            if (File.Exists(inputFile)) await Install(inputFile);
            else WinForms.MessageBox.Show(Lang.ErrorSelectedFileNotExist, WinForms.Application.ProductName, WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
        }

        private void SelApkAdbBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) apkPathAdbTxtBox.Text = ofd.FileName;
            }
        }

        private void DevicesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (devicesListBox.SelectedItem == null) return;
            ToLog(ApktoolEventType.None, String.Format(Lang.DeviceSelected, devicesListBox.SelectedItem));
            selAdbDeviceLbl.Text = devicesListBox.SelectedItem.ToString();
        }

        #endregion

        #region Handlers - APK info

        private async void SelApkFileInfoBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) await GetApkInfo(ofd.FileName);
            }
        }

        private void ApkIcon_Click(object sender, MouseButtonEventArgs e)
        {
            if (previousApkIcon == null) return;
            using (var sfd = new WinForms.SaveFileDialog())
            {
                sfd.Filter = Lang.PngImage + "|*.png";
                sfd.Title = Lang.SaveImageTitle;
                sfd.FileName = appTxtBox.Text;
                if (sfd.ShowDialog() == WinForms.DialogResult.OK && !String.IsNullOrEmpty(sfd.FileName))
                    previousApkIcon.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        #endregion

        #region Handlers - Main tab shortcuts

        private void SelSplitApkBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = string.Format(Lang.FilterSplitApk, "*.xapk;*.zip;*.apkm;*.apks");
                if (ofd.ShowDialog() == WinForms.DialogResult.OK) splitApkPathTxtBox.Text = ofd.FileName;
            }
        }

        private async void MergeApkBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Save();
                if (!File.Exists(Settings.Default.SplitApk_InputFile)) { ShowMessage(Lang.SplitApkNotFound, WinForms.MessageBoxIcon.Warning); return; }
                await Merge(Settings.Default.SplitApk_InputFile);
            }
            catch (Exception ex) { ToLog(ApktoolEventType.Error, ex.Message); }
        }

        private void DecApkOpenDirBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(textBox_BUILD_InputProjectDir.Text)) Process.Start("explorer.exe", textBox_BUILD_InputProjectDir.Text);
            else ToLog(ApktoolEventType.Error, Lang.ErrorSelectedFileNotExist);
        }

        private void DecOutOpenDirBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Settings.Default.Decode_OutputDir)) Process.Start("explorer.exe", Settings.Default.Decode_OutputDir);
            else ToLog(ApktoolEventType.Error, Lang.ErrorSelectedOutputFolderNotExist);
        }

        private void OpenAndroidMainfestBtn_Click(object sender, RoutedEventArgs e)
        {
            string manifest = Path.Combine(textBox_BUILD_InputProjectDir.Text, "AndroidManifest.xml");
            if (File.Exists(manifest)) Process.Start("explorer.exe", manifest);
            else ToLog(ApktoolEventType.Error, Lang.AndroidManifestNotExist);
        }

        private void OpenApktoolYmlBtn_Click(object sender, RoutedEventArgs e)
        {
            string yml = Path.Combine(textBox_BUILD_InputProjectDir.Text, "apktool.yml");
            if (File.Exists(yml)) Process.Start("explorer.exe", yml);
            else ToLog(ApktoolEventType.Error, Lang.AndroidManifestNotExist);
        }

        private void CompiledApkOpenDirBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Settings.Default.Build_OutputAppPath)) Process.Start("explorer.exe", Settings.Default.Build_OutputAppPath);
            else ToLog(ApktoolEventType.Error, Lang.ErrorSelectedFileNotExist);
        }

        private void Button_OpenMainActivity_Click(object sender, RoutedEventArgs e)
        {
            string decPath = textBox_BUILD_InputProjectDir.Text;
            if (!Directory.Exists(decPath)) { ToLog(ApktoolEventType.Error, Lang.DecompiledAPKNotExist); return; }

            var launchActivityList = new List<string>
            {
                aapt != null ? aapt.LaunchableActivity : CommonUtils.GetActivityFromManifest(decPath),
                "com\\unity3d\\player\\UnityPlayerActivity",
                CommonUtils.GetApplicationNameFromManifest(decPath)
            };

            foreach (string launchActivity in launchActivityList)
            {
                if (String.IsNullOrEmpty(launchActivity)) continue;
                string path = null;
                bool activityFound = false;
                for (int i = 1; i < 100; i++)
                {
                    string smaliFolder = (i == 1) ? "smali" : "smali_classes" + i;
                    path = Path.Combine(decPath, smaliFolder, launchActivity.Replace(".", "\\") + ".smali");
                    if (File.Exists(path)) { activityFound = true; break; }
                }
                if (activityFound && !CommonUtils.OnCreateExists(path)) continue;
                if (activityFound)
                {
                    ToLog(ApktoolEventType.None, String.Format(Lang.MainActivityFound, path));
                    Process.Start("explorer.exe", path);
                    return;
                }
            }
            ToLog(ApktoolEventType.Warning, Lang.MainActivityNotFoundPleaseFindManually);
        }

        private void ComApkOpenDir_Click(object sender, RoutedEventArgs e)
        {
            string decApkDir = textBox_BUILD_InputProjectDir.Text;
            string outputFile = decApkDir + " compiled.apk";
            if (Settings.Default.Build_SignAfterBuild) outputFile = decApkDir + " signed.apk";
            if (Settings.Default.Build_UseOutputAppPath)
            {
                outputFile = Path.Combine(Settings.Default.Build_OutputAppPath, Path.GetFileName(decApkDir)) + ".apk";
                if (Settings.Default.Build_SignAfterBuild) outputFile = Path.Combine(Settings.Default.Build_OutputAppPath, Path.GetFileName(decApkDir)) + " signed.apk";
            }
            if (File.Exists(outputFile)) Process.Start("explorer.exe", string.Format("/select,\"{0}\"", outputFile));
            else ToLog(ApktoolEventType.Error, Lang.ErrorSelectedFileNotExist);
        }

        private void SignApkOpenDirBtn_Click(object sender, RoutedEventArgs e)
        {
            string inputFile = Settings.Default.Sign_InputFile;
            string outputFile = inputFile;
            if (Settings.Default.Sign_UseOutputDir) outputFile = Path.Combine(Settings.Default.Sign_OutputDir, Path.GetFileName(inputFile));
            if (File.Exists(outputFile)) Process.Start("explorer.exe", string.Format("/select,\"{0}\"", outputFile));
            else ToLog(ApktoolEventType.Error, Lang.ErrorSelectedFileNotExist);
        }

        private void AlignApkOpenDirBtn_Click(object sender, RoutedEventArgs e)
        {
            string inputFile = Settings.Default.Zipalign_InputFile;
            string outputFile = inputFile;
            if (!String.IsNullOrEmpty(outputFile))
            {
                if (Settings.Default.Zipalign_UseOutputDir) outputFile = Path.Combine(Settings.Default.Zipalign_OutputDir, Path.GetFileName(inputFile));
                if (!Settings.Default.Zipalign_OverwriteOutputFile) outputFile = PathUtils.GetDirectoryNameWithoutExtension(outputFile) + " aligned.apk";
            }
            if (File.Exists(outputFile)) Process.Start("explorer.exe", string.Format("/select,\"{0}\"", outputFile));
            else ToLog(ApktoolEventType.Error, Lang.ErrorSelectedFileNotExist);
        }

        #endregion

        #region Drag & drop

        private static readonly string[] ApkExts = { ".apk", ".xapk", ".zip", ".apks", ".apkm" };

        private void WireDragDrop()
        {
            EnableDrop(textBox_DECODE_InputAppPath, ApkExts, DropDecode);
            EnableDrop(button_DECODE_Decode, ApkExts, DropDecode);

            EnableDrop(textBox_BUILD_InputProjectDir, null, DropCompile);
            EnableDrop(button_BUILD_Build, null, DropCompile);

            EnableDrop(textBox_ZIPALIGN_InputFile, new[] { ".apk" }, DropAlign);
            EnableDrop(button_ZIPALIGN_Align, new[] { ".apk" }, DropAlign);

            EnableDrop(textBox_SIGN_InputFile, new[] { ".apk" }, DropSign);
            EnableDrop(button_SIGN_Sign, new[] { ".apk" }, DropSign);

            EnableDrop(splitApkPathTxtBox, ApkExts, DropMerge);
            EnableDrop(mergeApkBtn, ApkExts, DropMerge);

            EnableDrop(bakSmaliGroupBox, new[] { ".dex" }, DropBaksmali);
            EnableDrop(smaliGroupBox, null, DropSmali);

            EnableDrop(basicInfoTabPage, ApkExts, DropApkInfo);
            EnableDrop(fileTxtBox, ApkExts, DropApkInfo);

            EnableDrop(tabAdb, new[] { ".apk" }, DropInstall);
            EnableDrop(installApkBtn, new[] { ".apk" }, DropInstall);
        }

        // exts == null accepts dropped folders; otherwise files ending with one of exts.
        private void EnableDrop(UIElement el, string[] exts, Func<string[], Task> action)
        {
            el.AllowDrop = true;
            el.PreviewDragEnter += (s, e) => DragEffect(e, exts);
            el.PreviewDragOver += (s, e) => DragEffect(e, exts);
            el.PreviewDrop += async (s, e) =>
            {
                e.Handled = true;
                string[] items = MatchingItems(e, exts);
                if (items.Length > 0) await action(items);
            };
        }

        private static void DragEffect(System.Windows.DragEventArgs e, string[] exts)
        {
            e.Effects = MatchingItems(e, exts).Length > 0 ? System.Windows.DragDropEffects.Copy : System.Windows.DragDropEffects.None;
            e.Handled = true;
        }

        private static string[] MatchingItems(System.Windows.DragEventArgs e, string[] exts)
        {
            if (!e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop))
                return Array.Empty<string>();
            var paths = e.Data.GetData(System.Windows.DataFormats.FileDrop) as string[];
            if (paths == null) return Array.Empty<string>();
            if (exts == null)
                return paths.Where(Directory.Exists).ToArray();
            return paths.Where(p => File.Exists(p) && exts.Any(x => p.EndsWith(x, StringComparison.OrdinalIgnoreCase))).ToArray();
        }

        private async Task DropDecode(string[] files)
        {
            foreach (var apkFile in files)
            {
                textBox_DECODE_InputAppPath.Text = apkFile;
                if (!Settings.Default.Decode_DontParseApkInfo) await GetApkInfo(apkFile);
                if (apkFile.ContainsAny(".xapk", ".zip", ".apks", ".apkm")) await MergeAndDecompile(apkFile);
                else await Decompile(apkFile);
            }
        }

        private async Task DropCompile(string[] folders)
        {
            foreach (var folder in folders)
            {
                if (File.Exists(Path.Combine(folder, "AndroidManifest.xml")))
                {
                    textBox_BUILD_InputProjectDir.Text = folder;
                    await Build(folder);
                }
                else ToLog(ApktoolEventType.Error, Lang.ErrorNotAnApk);
            }
        }

        private async Task DropAlign(string[] files)
        {
            foreach (var apkFile in files) { textBox_ZIPALIGN_InputFile.Text = apkFile; await Align(apkFile); }
        }

        private async Task DropSign(string[] files)
        {
            foreach (var apkFile in files) { textBox_SIGN_InputFile.Text = apkFile; await Sign(apkFile); }
        }

        private async Task DropMerge(string[] files)
        {
            foreach (var apkFile in files) { splitApkPathTxtBox.Text = apkFile; await Merge(apkFile); }
        }

        private async Task DropBaksmali(string[] files)
        {
            baksmaliBrowseInputDexTxtBox.Text = files[0];
            await Baksmali(files[0]);
        }

        private async Task DropSmali(string[] folders)
        {
            smaliBrowseInputDirTxtBox.Text = folders[0];
            await Smali(folders[0]);
        }

        private async Task DropApkInfo(string[] files)
        {
            fileTxtBox.Text = files[0];
            await GetApkInfo(files[0]);
        }

        private async Task DropInstall(string[] files)
        {
            apkPathAdbTxtBox.Text = files[0];
            await Install(files[0]);
        }

        #endregion

        #region Localization

        // UI captions live in the shared Language.*.resx table, keyed by the original WinForms
        // control names (e.g. "tabPageDecode.Text") — which match this window's x:Names.
        private void LocKey(FrameworkElement el, string key)
        {
            string v;
            try { v = Lang.ResourceManager.GetString(key); } catch { return; }
            if (v == null) return;
            if (el is TextBlock tb) tb.Text = v;
            else if (el is HeaderedContentControl hcc) hcc.Header = v;   // GroupBox, TabItem
            else if (el is MenuItem mi) mi.Header = v;
            else if (el is ContentControl cc) cc.Content = v;            // Button, Label, CheckBox
        }

        // Controls whose x:Name equals the original WinForms control name (= the resource key).
        private void Loc(FrameworkElement el) => LocKey(el, el.Name + ".Text");

        private void ApplyLocalization()
        {
            // Menu (x:Names differ from the WinForms ToolStripMenuItem names)
            LocKey(menuFile, "fileToolStripMenuItem.Text");
            LocKey(menuNewInstance, "newInsToolStripMenuItem.Text");
            LocKey(menuSaveLog, "saveLogToFileToolStripMenuItem.Text");
            LocKey(menuOpenTemp, "openTempFolderToolStripMenuItem.Text");
            LocKey(menuClearTemp, "clearTempFolderToolStripMenuItem.Text");
            LocKey(menuExit, "exitToolStripMenuItem.Text");
            LocKey(menuSettings, "settingsToolStripMenuItem1.Text");
            LocKey(menuUseApkEditor, "useAPKEditorForDecompilingItem.Text");
            LocKey(menuOpenSettings, "settingsToolStripMenuItem.Text");
            LocKey(menuHelp, "helpToolStripMenuItem.Text");
            LocKey(menuCheckUpdate, "checkForUpdateToolStripMenuItem.Text");
            LocKey(menuReportIssue, "reportAnIsuueToolStripMenuItem.Text");
            LocKey(menuApktoolIssues, "apktoolIssuesToolStripMenuItem.Text");
            LocKey(menuBaksmaliIssues, "baksmaliIssuesToolStripMenuItem.Text");
            LocKey(menuAbout, "aboutToolStripMenuItem.Text");
            LocKey(menuLogCopy, "copyToolStripMenuItem.Text");
            LocKey(menuLogClear, "clearLogToolStripMenuItem.Text");
            menuLogCopyAll.Header = Lang.CopyAll;
            statusText.Text = Lang.Ready;

            // Tabs (x:Names differ from the WinForms TabPage names)
            LocKey(tabMain, "tabPageMain.Text");
            LocKey(tabApkInfo, "tabPageApkInfo.Text");
            LocKey(tabDecode, "tabPageDecode.Text");
            LocKey(tabBuild, "tabPageBuild.Text");
            LocKey(tabSign, "tabPageSign.Text");
            LocKey(tabZipalign, "tabPageZipAlign.Text");
            LocKey(tabFramework, "tabPageInstallFramework.Text");
            LocKey(tabBaksmali, "tabPageBaksmali.Text");
            LocKey(tabAdb, "tabPageAdb.Text");
            Loc(basicInfoTabPage); LocKey(tabPage3, "AaptDump");

            // Main tab
            Loc(label1); Loc(label2); Loc(label3); Loc(label4); Loc(splitApkTxt);
            Loc(button_DECODE_Decode); Loc(button_BUILD_Build); Loc(button_ZIPALIGN_Align); Loc(button_SIGN_Sign); Loc(mergeApkBtn);
            Loc(decOutOpenDirBtn); Loc(decApkOpenDirBtn); Loc(alignApkOpenDirBtn);
            Loc(compileOutputOpenDirBtn); Loc(comApkOpenDir); Loc(signApkOpenDirBtn);
            Loc(openAndroidMainfestBtn); Loc(openApktoolYmlBtn); Loc(button_OpenMainActivity);

            // Decode
            Loc(groupBox_DECODE_Options);
            Loc(decSetApiLvlChkBox); Loc(checkBox_DECODE_NoSrc); Loc(checkBox_DECODE_NoRes); Loc(checkBox_DECODE_Force);
            Loc(checkBox_DECODE_KeepBrokenRes); Loc(checkBox_DECODE_MatchOriginal); Loc(checkBox_DECODE_UseFramework);
            Loc(checkBox_DECODE_OutputDirectory); Loc(checkBox_DECODE_OnlyMainClasses); Loc(checkBox_DECODE_FixError);
            Loc(checkBox_DECODE_NoDebugInfo); Loc(checkBox7); Loc(checkBox3);

            // Build
            Loc(groupBox_BUILD_Options);
            Loc(buildSetApiLvlChkBox); Loc(checkBox_BUILD_ForceAll); Loc(checkBox_BUILD_UseAapt); Loc(checkBox_BUILD_UseFramework);
            Loc(checkBox_BUILD_OutputAppPath); Loc(checkBox_BUILD_NoCrunch); Loc(checkBox_BUILD_NetSecConf); Loc(zipalignAfterBuildChkBox);
            Loc(signAfterBuildChkBox); Loc(createUnsignApkChkBox); Loc(useAapt2ChkBox); Loc(checkBox_BUILD_CopyOriginal); Loc(checkBox4);

            // Sign
            Loc(groupBox_SIGN_Options);
            Loc(label22); Loc(label_SIGN_PublicKey); Loc(label_SIGN_PrivateKey); Loc(label20); Loc(label21); Loc(label23);
            Loc(label24); Loc(label25); Loc(label26); Loc(label27);
            Loc(useAliasChkBox); Loc(useKeyStoreChkBox); Loc(useSigningOutputDir); Loc(autoDelIdsigChkBox); Loc(checkBox1); Loc(checkBox2);

            // Zip align
            Loc(groupBox_ZIPALIGN_Options); Loc(label_ZIPALIGN_AlignmentBytes);
            Loc(checkBox_ZIPALIGN_CheckAlignment); Loc(checkBox_ZIPALIGN_VerboseOutput); Loc(checkBox_ZIPALIGN_Recompress);
            Loc(checkBox_ZIPALIGN_OverwriteOutputFile); Loc(signAfterZipalignChkBox); Loc(zipalignOutputDirChkBox);

            // Framework
            Loc(groupBox_IF_Options); Loc(groupBox1);
            Loc(checkBox_IF_FramePath); Loc(checkBox_IF_Tag); Loc(clearFwBeforeDecodeChkBox);
            Loc(button_IF_InstallFramework); Loc(clearFwBtn); Loc(openFwFolderBtn);

            // Baksmali / Smali
            Loc(bakSmaliGroupBox); Loc(smaliGroupBox);
            Loc(baksmaliUseOutputChkBox); Loc(smaliUseOutputChkBox); Loc(label28); Loc(label29);
            Loc(decSmaliBtn); Loc(comSmaliBtn);

            // ADB
            Loc(label33); Loc(label32); Loc(setVendorChkBox); Loc(overrideAbiCheckBox);
            Loc(killAdbBtn); Loc(refreshDevicesBtn); Loc(installApkBtn);

            // APK info
            Loc(label17); Loc(label7); Loc(label9); Loc(label19); Loc(label31); Loc(label8); Loc(label10);
            Loc(label11); Loc(label12); Loc(label13); Loc(label30); Loc(label14); Loc(label18); Loc(label15);
            Loc(psLinkBtn); Loc(apkComboLinkBtn); Loc(apkPureLinkBtn); Loc(apkGkLinkBtn); Loc(apkSupportLinkBtn); Loc(apkMirrorLinkBtn);

            // Signature now has its own subtab; reuse the old "Signature:" label string (trimmed) for its header.
            string sig = Lang.ResourceManager.GetString("label5.Text");
            if (sig != null) signatureTabPage.Header = sig.TrimEnd(' ', ':', '：');

            // Sign scheme combo items (Default / True / False)
            LocSchemeCombo(schemev1ComboBox); LocSchemeCombo(schemev2ComboBox);
            LocSchemeCombo(schemev3ComboBox); LocSchemeCombo(schemev4ComboBox);
        }

        private static void LocSchemeCombo(ComboBox c)
        {
            if (c.Items.Count < 3) return;
            ((ComboBoxItem)c.Items[0]).Content = Lang.SchemeDefault;
            ((ComboBoxItem)c.Items[1]).Content = Lang.SchemeTrue;
            ((ComboBoxItem)c.Items[2]).Content = Lang.SchemeFalse;
        }

        #endregion
    }
}
