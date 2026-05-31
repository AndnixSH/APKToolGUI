using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using APKToolGUI.Controls;
using APKToolGUI.Properties;
using APKToolGUI.Utils;
using Ookii.Dialogs.WinForms;
using Lang = APKToolGUI.Languages.Language;
using WinForms = System.Windows.Forms;

namespace APKToolGUI.Forms
{
    /// <summary>
    /// WPF replacement for the former WinForms <c>FormSettings</c>. Behaviour is kept
    /// identical: settings are loaded into the controls on open and written back on OK
    /// (the old form used live two-way data bindings; a modal dialog makes load/save on
    /// OK equivalent), the language list is discovered from embedded satellite resources,
    /// and a restart is offered when the language or theme changes.
    /// </summary>
    public partial class SettingsWindow : Window
    {
        // Captured at load to detect changes that require a restart / apktool refresh.
        private string currentCulture;
        private int currentTheme;
        private bool currentUseApktoolChk;
        private string currentApktoolPath;

        /// <summary>Language list entry: a display caption plus the stored culture value.</summary>
        private sealed class LanguageItem
        {
            public string Display { get; }
            public string Culture { get; } // "Auto", "" (English) or a culture name
            public LanguageItem(string display, string culture) { Display = display; Culture = culture; }
            public override string ToString() => Display;
        }

        public SettingsWindow()
        {
            InitializeComponent();

            Theme theme = (Theme)Settings.Default.Theme;
            WpfTheme.Apply(this, Program.IsDarkTheme());
            NativeDarkMode.ApplyTheme(this, theme);

            ApplyLocalizedText();
            LoadFromSettings();
            PopulateLanguages();

            if (!AdminUtils.IsAdministrator())
            {
                SetShield(btnInstallCM);
                SetShield(btnUninstallCM);
            }
        }

        #region Localization

        private void ApplyLocalizedText()
        {
            var rm = Lang.ResourceManager;

            Title = Lang.Settings;
            groupGeneral.Header = rm.GetString("SettingsGeneral");
            groupLanguage.Header = rm.GetString("SettingsLanguage");
            groupContextMenu.Header = rm.GetString("SettingsContextMenu");

            chkCheckUpdate.Content = rm.GetString("SettingsCheckUpdate");
            chkClearLog.Content = rm.GetString("SettingsClearLog");
            chkPlaySound.Content = rm.GetString("SettingsPlaySound");
            chkCustomTemp.Content = rm.GetString("SettingsCustomTemp");
            chkCustomJava.Content = rm.GetString("SettingsCustomJava");
            chkCustomJvmArgs.Content = rm.GetString("SettingsCustomJvmArgs");
            chkUtf8.Content = rm.GetString("SettingsUtf8");
            lblTempNote.Text = rm.GetString("SettingsTempNote");
            chkCustomApktool.Content = rm.GetString("SettingsCustomApktool");
            lblTheme.Content = rm.GetString("SettingsTheme");
            chkDebug.Content = Lang.DebugMode;
            chkIgnoreOutputCM.Content = rm.GetString("SettingsIgnoreOutputCM");

            lblAdminRights.Content = rm.GetString("SettingsAdminRights");
            btnInstallCM.Content = rm.GetString("SettingsInstall");
            btnUninstallCM.Content = rm.GetString("SettingsUninstall");

            btnOK.Content = rm.GetString("AboutOK");
            btnCancel.Content = rm.GetString("SettingsCancel");

            cboTheme.Items.Add(rm.GetString("ThemeAuto"));
            cboTheme.Items.Add(rm.GetString("ThemeLight"));
            cboTheme.Items.Add(rm.GetString("ThemeDark"));
        }

        #endregion

        #region Load / Save

        private void LoadFromSettings()
        {
            var s = Settings.Default;

            chkClearLog.IsChecked = s.ClearLogBeforeAction;
            chkPlaySound.IsChecked = s.PlaySoundWhenDone;
            chkCheckUpdate.IsChecked = s.CheckForUpdateAtStartup;
            chkCustomTemp.IsChecked = s.UseCustomTempDir;
            txtTempDir.Text = s.TempDir;
            chkCustomJava.IsChecked = s.UseCustomJavaExe;
            txtJavaExe.Text = s.JavaExe;
            chkCustomJvmArgs.IsChecked = s.UseCustomJVMArgs;
            txtJvmArgs.Text = s.CustomJVMArgs;
            chkUtf8.IsChecked = s.Utf8FilenameSupport;
            chkDebug.IsChecked = s.DebugMode;
            chkCustomApktool.IsChecked = s.UseCustomApktool;
            txtApktoolPath.Text = s.ApktoolPath;
            chkIgnoreOutputCM.IsChecked = s.IgnoreOutputDirContextMenu;

            int t = s.Theme;
            if (t < 0 || t >= cboTheme.Items.Count) t = 0;
            cboTheme.SelectedIndex = t;

            currentTheme = t;
            currentUseApktoolChk = chkCustomApktool.IsChecked == true;
            currentApktoolPath = txtApktoolPath.Text;
        }

        private void SaveToSettings()
        {
            try
            {
                var s = Settings.Default;

                s.ClearLogBeforeAction = chkClearLog.IsChecked == true;
                s.PlaySoundWhenDone = chkPlaySound.IsChecked == true;
                s.CheckForUpdateAtStartup = chkCheckUpdate.IsChecked == true;
                s.UseCustomTempDir = chkCustomTemp.IsChecked == true;
                s.TempDir = txtTempDir.Text;
                s.UseCustomJavaExe = chkCustomJava.IsChecked == true;
                s.JavaExe = txtJavaExe.Text;
                s.UseCustomJVMArgs = chkCustomJvmArgs.IsChecked == true;
                s.CustomJVMArgs = txtJvmArgs.Text;
                s.Utf8FilenameSupport = chkUtf8.IsChecked == true;
                s.DebugMode = chkDebug.IsChecked == true;
                s.UseCustomApktool = chkCustomApktool.IsChecked == true;
                s.ApktoolPath = txtApktoolPath.Text;
                s.IgnoreOutputDirContextMenu = chkIgnoreOutputCM.IsChecked == true;

                string newCulture = (cboLanguage.SelectedItem as LanguageItem)?.Culture ?? currentCulture;
                s.Culture = newCulture;
                s.Theme = cboTheme.SelectedIndex;
                s.Save();

                bool languageChanged = newCulture != currentCulture;
                bool themeChanged = cboTheme.SelectedIndex != currentTheme;
                if (languageChanged || themeChanged)
                {
                    if (WinForms.MessageBox.Show(Lang.RestartApplicationPrompt, WinForms.Application.ProductName,
                            WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question) == WinForms.DialogResult.Yes)
                    {
                        // This is a WPF app (no WinForms message loop), so WinForms.Application.Restart()
                        // would start a new instance but never shut this one down. Do it explicitly.
                        Process.Start(WinForms.Application.ExecutablePath);
                        System.Windows.Application.Current.Shutdown();
                        return;
                    }
                }

                if (currentUseApktoolChk != (chkCustomApktool.IsChecked == true) || currentApktoolPath != txtApktoolPath.Text)
                    MainWindow.Instance?.SetApktoolPath();
            }
            catch (Exception ex)
            {
                Log.e(ex.ToString());
            }
        }

        #endregion

        #region Language list

        private void PopulateLanguages()
        {
            string sysLang = Lang.SystemLanguage;
            string culture = Settings.Default.Culture;
            currentCulture = culture;

            cboLanguage.Items.Add(new LanguageItem(sysLang, "Auto"));
            cboLanguage.Items.Add(new LanguageItem(CultureInfo.GetCultureInfo("en").NativeName, ""));

            // Discover the languages we ship by scanning the embedded satellite resources
            // (e.g. "APKToolGUI.de.resources.dll" -> "de").
            foreach (string resourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {
                if (!resourceName.EndsWith(".resources.dll", StringComparison.OrdinalIgnoreCase))
                    continue;

                string[] parts = resourceName.Split('.');
                if (parts.Length < 2) continue;

                try
                {
                    CultureInfo c = CultureInfo.GetCultureInfo(parts[1]);
                    cboLanguage.Items.Add(new LanguageItem($"{c.DisplayName} [{c.Name}]", c.Name));
                }
                catch (CultureNotFoundException) { }
            }

            // Select the entry matching the stored culture.
            int select = 1; // default: English
            if (culture == "Auto")
                select = 0;
            else if (!string.IsNullOrEmpty(culture))
            {
                for (int i = 0; i < cboLanguage.Items.Count; i++)
                    if (((LanguageItem)cboLanguage.Items[i]).Culture == culture) { select = i; break; }
            }
            cboLanguage.SelectedIndex = select;
        }

        #endregion

        #region Buttons / pickers

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            SaveToSettings();
            Close();
        }

        private void BrowseJava_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = "java.exe|java.exe";
                if (ofd.ShowDialog() == WinForms.DialogResult.OK)
                    txtJavaExe.Text = Program.GetPortablePath(ofd.FileName);
            }
        }

        private void BrowseTemp_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new VistaFolderBrowserDialog())
            {
                if (!string.IsNullOrWhiteSpace(txtTempDir.Text))
                    fbd.SelectedPath = txtTempDir.Text;
                if (fbd.ShowDialog() == WinForms.DialogResult.OK)
                {
                    txtTempDir.Text = fbd.SelectedPath;

                    // Move the working temp folder to the new location, as the old form did.
                    DirectoryUtils.Delete(Program.TEMP_PATH);
                    Program.TEMP_PATH = Program.RandTempDirectory();
                    Directory.CreateDirectory(Program.TEMP_PATH);
                }
            }
        }

        private void BrowseApktool_Click(object sender, RoutedEventArgs e)
        {
            using (var ofd = new WinForms.OpenFileDialog())
            {
                ofd.Filter = "Apktool (*.jar)|*.jar";
                if (ofd.ShowDialog() == WinForms.DialogResult.OK)
                    txtApktoolPath.Text = ofd.FileName;
            }
        }

        private void InstallContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (WinForms.MessageBox.Show(Lang.DoYouRealyWantToInstallCM, WinForms.Application.ProductName,
                    WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question) == WinForms.DialogResult.Yes)
                RunAsAdmin(WinForms.Application.ExecutablePath, "ccm");
        }

        private void UninstallContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (WinForms.MessageBox.Show(Lang.DoYouRealyWantToRemoveCM, WinForms.Application.ProductName,
                    WinForms.MessageBoxButtons.YesNo, WinForms.MessageBoxIcon.Question) == WinForms.DialogResult.Yes)
                RunAsAdmin(WinForms.Application.ExecutablePath, "rcm");
        }

        private static void RunAsAdmin(string fileName, string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = true,
                Verb = "runas"
            };
            try
            {
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                WinForms.MessageBox.Show(ex.Message, WinForms.Application.ProductName,
                    WinForms.MessageBoxButtons.OK, WinForms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// WPF equivalent of the old BCM_SETSHIELD: prepends the UAC shield icon to a
        /// button's caption to signal the action needs elevation.
        /// </summary>
        private void SetShield(Button btn)
        {
            try
            {
                var source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                    System.Drawing.SystemIcons.Shield.Handle,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromWidthAndHeight(16, 16));

                var panel = new StackPanel { Orientation = Orientation.Horizontal };
                panel.Children.Add(new Image
                {
                    Source = source,
                    Width = 16,
                    Height = 16,
                    Margin = new Thickness(0, 0, 6, 0),
                    VerticalAlignment = VerticalAlignment.Center
                });
                panel.Children.Add(new TextBlock
                {
                    Text = btn.Content as string,
                    VerticalAlignment = VerticalAlignment.Center
                });
                btn.Content = panel;
            }
            catch
            {
                // Shield is cosmetic; ignore if the icon can't be created.
            }
        }

        #endregion
    }
}
