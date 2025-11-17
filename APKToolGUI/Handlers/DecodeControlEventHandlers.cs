using APKToolGUI.Languages;
using APKToolGUI.Properties;
using APKToolGUI.Utils;
using Ookii.Dialogs.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APKToolGUI.Handlers
{
    class DecodeControlEventHandlers
    {
        private static FormMain main;
        public DecodeControlEventHandlers(FormMain Main)
        {
            main = Main;
            main.button_DECODE_BrowseFrameDir.Click += Button_DECODE_BrowseFrameDir_Click;
            main.button_DECODE_BrowseOutputDirectory.Click += Button_DECODE_BrowseOutputDirectory_Click;
            main.button_DECODE_BrowseInputAppPath.Click += Button_DECODE_BrowseInputAppPath_Click;
            main.button_DECODE_Decode.Click += Button_DECODE_Decode_Click;
        }

        internal void Button_DECODE_BrowseFrameDir_Click(object sender, EventArgs e)
        {
            using (VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog())
            {
                if (!String.IsNullOrWhiteSpace(main.textBox_DECODE_FrameDir.Text))
                    fbd.SelectedPath = main.textBox_DECODE_FrameDir.Text;
                if (fbd.ShowDialog() == DialogResult.OK)
                    main.textBox_DECODE_FrameDir.Text = fbd.SelectedPath;
            }
        }

        internal void Button_DECODE_BrowseOutputDirectory_Click(object sender, EventArgs e)
        {
            using (VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog())
            {
                if (!String.IsNullOrWhiteSpace(main.textBox_DECODE_OutputDirectory.Text))
                    fbd.SelectedPath = main.textBox_DECODE_OutputDirectory.Text;
                else
                    if (!String.IsNullOrWhiteSpace(main.textBox_DECODE_InputAppPath.Text))
                    fbd.SelectedPath = Path.GetDirectoryName(main.textBox_DECODE_InputAppPath.Text);
                if (fbd.ShowDialog() == DialogResult.OK)
                    main.textBox_DECODE_OutputDirectory.Text = fbd.SelectedPath;
            }
        }

        internal async void Button_DECODE_BrowseInputAppPath_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Android Package (*.apk;*.xapk;*.zip;*.apkm;*.apks)|*.apk;*.xapk;*.zip;*.apkm;*.apks";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    main.textBox_DECODE_InputAppPath.Text = ofd.FileName;

                    if (!Settings.Default.Decode_DontParseApkInfo)
                        await main.GetApkInfo(ofd.FileName);

                    if (main.checkBox_DECODE_OutputDirectory.Checked)
                    {
                        main.textBox_DECODE_OutputDirectory.Text = Path.Combine(Path.GetDirectoryName(main.textBox_DECODE_InputAppPath.Text), Path.GetFileNameWithoutExtension(main.textBox_DECODE_InputAppPath.Text));
                    }
                }
            }
        }

        internal async void Button_DECODE_Decode_Click(object sender, EventArgs e)
        {
            string inputFile = main.textBox_DECODE_InputAppPath.Text;
            if (File.Exists(inputFile))
            {
                if (main.checkBox_DECODE_UseFramework.Checked && !Directory.Exists(main.textBox_DECODE_FrameDir.Text))
                {
                    main.ShowMessage(Language.DecodeSelectedFrameworkNotExist, MessageBoxIcon.Warning);
                    return;
                }
                if (main.checkBox_DECODE_OutputDirectory.Checked)
                {
                    if (String.IsNullOrWhiteSpace(Settings.Default.Decode_OutputDir))
                    {
                        main.ShowMessage(Language.DecodeDirNotSelected, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                        if (!PathUtils.IsValidPath(Settings.Default.Decode_OutputDir))
                    {
                        main.ShowMessage(Language.DecodeCouldNotCreate, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (inputFile.ContainsAny(".xapk", ".zip", ".apks", ".apkm"))
                {
                    await main.MergeAndDecompile(inputFile);
                }
                else
                    await main.Decompile(inputFile);
            }
            else
                MessageBox.Show(Language.WarningFileForDecodingNotSelected, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
