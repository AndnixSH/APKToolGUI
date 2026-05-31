using System;
using System.Reflection;
using System.Windows;
using APKToolGUI.Controls;
using APKToolGUI.Properties;
// Alias the resource class: a bare "Language" would otherwise bind to the inherited
// FrameworkElement.Language property (System.Windows.Markup.XmlLanguage).
using Lang = APKToolGUI.Languages.Language;

namespace APKToolGUI.Forms
{
    /// <summary>
    /// WPF replacement for the former WinForms <c>FormAboutBox</c>. Behaviour is kept
    /// identical: the product name, version, copyright and description come from the
    /// assembly attributes, the title / version / copyright / link captions are
    /// localised, and the title bar follows the app's dark/light theme.
    /// </summary>
    public partial class AboutWindow : Window
    {
        private const string RepoUrl = "https://github.com/AndnixSH/APKToolGUI";

        public AboutWindow()
        {
            InitializeComponent();

            Theme theme = (Theme)Settings.Default.Theme;
            WpfTheme.Apply(this, Program.IsDarkTheme());

            // Our own immersive dark title bar (replaces DarkNet), now for WPF windows.
            NativeDarkMode.ApplyTheme(this, theme);

            ApplyLocalizedText();
        }

        private void ApplyLocalizedText()
        {
            var rm = Lang.ResourceManager;

            Title = String.Format("{0} {1}", Lang.About, AssemblyTitle);
            productNameText.Text = AssemblyProduct;
            versionText.Text = String.Format("{0} {1}", rm.GetString("AboutVersion"), AssemblyVersion);
            copyrightText.Text = String.Format("{0} {1}", rm.GetString("AboutCopyright"), AssemblyCopyright);
            repoLinkText.Text = rm.GetString("AboutGithubRepo");
            descriptionText.Text = AssemblyDescription;
            okButton.Content = rm.GetString("AboutOK");
        }

        private void RepoLink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(RepoUrl);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Assembly attribute accessors

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                    return "";
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        #endregion
    }
}
