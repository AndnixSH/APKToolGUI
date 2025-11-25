namespace APKToolGUI
{
    partial class FormMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.mergePanel = new System.Windows.Forms.Panel();
            this.splitApkTxt = new System.Windows.Forms.Label();
            this.splitApkPathTxtBox = new System.Windows.Forms.TextBox();
            this.selSplitApkBtn = new System.Windows.Forms.Button();
            this.mergeApkBtn = new System.Windows.Forms.Button();
            this.comApkOpenDir = new System.Windows.Forms.Button();
            this.decOutOpenDirBtn = new System.Windows.Forms.Button();
            this.signApkOpenDirBtn = new System.Windows.Forms.Button();
            this.alignApkOpenDirBtn = new System.Windows.Forms.Button();
            this.decApkOpenDirBtn = new System.Windows.Forms.Button();
            this.compileOutputOpenDirBtn = new System.Windows.Forms.Button();
            this.button_OpenMainActivity = new System.Windows.Forms.Button();
            this.openApktoolYmlBtn = new System.Windows.Forms.Button();
            this.openAndroidMainfestBtn = new System.Windows.Forms.Button();
            this.signPanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_SIGN_InputFile = new System.Windows.Forms.TextBox();
            this.button_SIGN_BrowseInputFile = new System.Windows.Forms.Button();
            this.button_SIGN_Sign = new System.Windows.Forms.Button();
            this.zipalignPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.button_ZIPALIGN_Align = new System.Windows.Forms.Button();
            this.button_ZIPALIGN_BrowseInputFile = new System.Windows.Forms.Button();
            this.textBox_ZIPALIGN_InputFile = new System.Windows.Forms.TextBox();
            this.comPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.button_BUILD_BrowseInputProjectDir = new System.Windows.Forms.Button();
            this.button_BUILD_Build = new System.Windows.Forms.Button();
            this.textBox_BUILD_InputProjectDir = new System.Windows.Forms.TextBox();
            this.decPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_DECODE_InputAppPath = new System.Windows.Forms.TextBox();
            this.button_DECODE_Decode = new System.Windows.Forms.Button();
            this.button_DECODE_BrowseInputAppPath = new System.Windows.Forms.Button();
            this.tabPageApkInfo = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.basicInfoTabPage = new System.Windows.Forms.TabPage();
            this.sigTxtBox = new System.Windows.Forms.RichTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.launchActivityTxtBox = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.archSdkTxtBox = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.apkMirrorLinkBtn = new System.Windows.Forms.Button();
            this.apkSupportLinkBtn = new System.Windows.Forms.Button();
            this.apkGkLinkBtn = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.localsTxtBox = new System.Windows.Forms.RichTextBox();
            this.selApkFileInfoBtn = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.appTxtBox = new System.Windows.Forms.TextBox();
            this.permTxtBox = new System.Windows.Forms.RichTextBox();
            this.apkComboLinkBtn = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.fileTxtBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.densityTxtBox = new System.Windows.Forms.TextBox();
            this.packNameTxtBox = new System.Windows.Forms.TextBox();
            this.apkPureLinkBtn = new System.Windows.Forms.Button();
            this.verTxtBox = new System.Windows.Forms.TextBox();
            this.psLinkBtn = new System.Windows.Forms.Button();
            this.minSdkTxtBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.targetSdkTxtBox = new System.Windows.Forms.TextBox();
            this.screenTxtBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.buildTxtBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.apkIconPicBox = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.fullInfoTextBox = new System.Windows.Forms.RichTextBox();
            this.tabPageDecode = new System.Windows.Forms.TabPage();
            this.groupBox_DECODE_Options = new System.Windows.Forms.GroupBox();
            this.decJobsLvlUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.decApiLvlUpDown = new System.Windows.Forms.NumericUpDown();
            this.decSetApiLvlChkBox = new System.Windows.Forms.CheckBox();
            this.checkBox_DECODE_FixError = new System.Windows.Forms.CheckBox();
            this.checkBox_DECODE_OnlyMainClasses = new System.Windows.Forms.CheckBox();
            this.textBox_DECODE_FrameDir = new System.Windows.Forms.TextBox();
            this.button_DECODE_BrowseOutputDirectory = new System.Windows.Forms.Button();
            this.checkBox_DECODE_UseFramework = new System.Windows.Forms.CheckBox();
            this.button_DECODE_BrowseFrameDir = new System.Windows.Forms.Button();
            this.checkBox_DECODE_MatchOriginal = new System.Windows.Forms.CheckBox();
            this.checkBox_DECODE_OutputDirectory = new System.Windows.Forms.CheckBox();
            this.textBox_DECODE_OutputDirectory = new System.Windows.Forms.TextBox();
            this.checkBox_DECODE_KeepBrokenRes = new System.Windows.Forms.CheckBox();
            this.checkBox_DECODE_NoSrc = new System.Windows.Forms.CheckBox();
            this.checkBox_DECODE_Force = new System.Windows.Forms.CheckBox();
            this.checkBox_DECODE_NoRes = new System.Windows.Forms.CheckBox();
            this.checkBox_DECODE_NoDebugInfo = new System.Windows.Forms.CheckBox();
            this.tabPageBuild = new System.Windows.Forms.TabPage();
            this.groupBox_BUILD_Options = new System.Windows.Forms.GroupBox();
            this.comJobsLvlUpDown = new System.Windows.Forms.NumericUpDown();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox_BUILD_NetSecConf = new System.Windows.Forms.CheckBox();
            this.useAapt2ChkBox = new System.Windows.Forms.CheckBox();
            this.buildApiLvlUpDown = new System.Windows.Forms.NumericUpDown();
            this.buildSetApiLvlChkBox = new System.Windows.Forms.CheckBox();
            this.createUnsignApkChkBox = new System.Windows.Forms.CheckBox();
            this.signAfterBuildChkBox = new System.Windows.Forms.CheckBox();
            this.zipalignAfterBuildChkBox = new System.Windows.Forms.CheckBox();
            this.checkBox_BUILD_NoCrunch = new System.Windows.Forms.CheckBox();
            this.button_BUILD_BrowseOutputAppPath = new System.Windows.Forms.Button();
            this.checkBox_BUILD_ForceAll = new System.Windows.Forms.CheckBox();
            this.button_BUILD_BrowseFrameDir = new System.Windows.Forms.Button();
            this.button_BUILD_BrowseAaptPath = new System.Windows.Forms.Button();
            this.checkBox_BUILD_OutputAppPath = new System.Windows.Forms.CheckBox();
            this.checkBox_BUILD_CopyOriginal = new System.Windows.Forms.CheckBox();
            this.textBox_BUILD_OutputAppPath = new System.Windows.Forms.TextBox();
            this.checkBox_BUILD_UseAapt = new System.Windows.Forms.CheckBox();
            this.textBox_BUILD_AaptPath = new System.Windows.Forms.TextBox();
            this.textBox_BUILD_FrameDir = new System.Windows.Forms.TextBox();
            this.checkBox_BUILD_UseFramework = new System.Windows.Forms.CheckBox();
            this.tabPageSign = new System.Windows.Forms.TabPage();
            this.groupBox_SIGN_Options = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.autoDelIdsigChkBox = new System.Windows.Forms.CheckBox();
            this.schemev4ComboBox = new System.Windows.Forms.ComboBox();
            this.label27 = new System.Windows.Forms.Label();
            this.schemev3ComboBox = new System.Windows.Forms.ComboBox();
            this.label26 = new System.Windows.Forms.Label();
            this.schemev2ComboBox = new System.Windows.Forms.ComboBox();
            this.label25 = new System.Windows.Forms.Label();
            this.schemev1ComboBox = new System.Windows.Forms.ComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.selectKeyStoreFileBtn = new System.Windows.Forms.Button();
            this.aliasTxtBox = new System.Windows.Forms.TextBox();
            this.useAliasChkBox = new System.Windows.Forms.CheckBox();
            this.label22 = new System.Windows.Forms.Label();
            this.keyStoreFileTxtBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.useKeyStoreChkBox = new System.Windows.Forms.CheckBox();
            this.useSigningOutputDir = new System.Windows.Forms.CheckBox();
            this.label_SIGN_PrivateKey = new System.Windows.Forms.Label();
            this.label_SIGN_PublicKey = new System.Windows.Forms.Label();
            this.button_SIGN_BrowseOutputFile = new System.Windows.Forms.Button();
            this.textBox_SIGN_OutputFile = new System.Windows.Forms.TextBox();
            this.button_SIGN_BrowsePublicKey = new System.Windows.Forms.Button();
            this.button_SIGN_BrowsePrivateKey = new System.Windows.Forms.Button();
            this.textBox_SIGN_PublicKey = new System.Windows.Forms.TextBox();
            this.textBox_SIGN_PrivateKey = new System.Windows.Forms.TextBox();
            this.tabPageZipAlign = new System.Windows.Forms.TabPage();
            this.groupBox_ZIPALIGN_Options = new System.Windows.Forms.GroupBox();
            this.zipalignOutputDirChkBox = new System.Windows.Forms.CheckBox();
            this.signAfterZipalignChkBox = new System.Windows.Forms.CheckBox();
            this.checkBox_ZIPALIGN_Recompress = new System.Windows.Forms.CheckBox();
            this.label_ZIPALIGN_AlignmentBytes = new System.Windows.Forms.Label();
            this.button_ZIPALIGN_BrowseOutputFile = new System.Windows.Forms.Button();
            this.checkBox_ZIPALIGN_CheckAlignment = new System.Windows.Forms.CheckBox();
            this.textBox_ZIPALIGN_OutputFile = new System.Windows.Forms.TextBox();
            this.checkBox_ZIPALIGN_VerboseOutput = new System.Windows.Forms.CheckBox();
            this.numericUpDown_ZIPALIGN_AlignmentBytes = new System.Windows.Forms.NumericUpDown();
            this.checkBox_ZIPALIGN_OverwriteOutputFile = new System.Windows.Forms.CheckBox();
            this.tabPageInstallFramework = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.openFwFolderBtn = new System.Windows.Forms.Button();
            this.clearFwBtn = new System.Windows.Forms.Button();
            this.clearFwBeforeDecodeChkBox = new System.Windows.Forms.CheckBox();
            this.groupBox_IF_Options = new System.Windows.Forms.GroupBox();
            this.checkBox_IF_Tag = new System.Windows.Forms.CheckBox();
            this.checkBox_IF_FramePath = new System.Windows.Forms.CheckBox();
            this.textBox_IF_Tag = new System.Windows.Forms.TextBox();
            this.button_IF_InstallFramework = new System.Windows.Forms.Button();
            this.button_IF_BrowseFrameDir = new System.Windows.Forms.Button();
            this.button_IF_BrowseInputFramePath = new System.Windows.Forms.Button();
            this.textBox_IF_InputFramePath = new System.Windows.Forms.TextBox();
            this.textBox_IF_FrameDir = new System.Windows.Forms.TextBox();
            this.tabPageBaksmali = new System.Windows.Forms.TabPage();
            this.smaliGroupBox = new System.Windows.Forms.GroupBox();
            this.label29 = new System.Windows.Forms.Label();
            this.smaliUseOutputChkBox = new System.Windows.Forms.CheckBox();
            this.comSmaliBtn = new System.Windows.Forms.Button();
            this.smaliBrowseOutputBtn = new System.Windows.Forms.Button();
            this.smaliBrowseInputDirTxtBox = new System.Windows.Forms.TextBox();
            this.smaliBrowseOutputTxtBox = new System.Windows.Forms.TextBox();
            this.smaliBrowseInputDirBtn = new System.Windows.Forms.Button();
            this.bakSmaliGroupBox = new System.Windows.Forms.GroupBox();
            this.label28 = new System.Windows.Forms.Label();
            this.baksmaliUseOutputChkBox = new System.Windows.Forms.CheckBox();
            this.baksmaliBrowseOutputBtn = new System.Windows.Forms.Button();
            this.baksmaliBrowseOutputTxtBox = new System.Windows.Forms.TextBox();
            this.decSmaliBtn = new System.Windows.Forms.Button();
            this.baksmaliBrowseInputDexBtn = new System.Windows.Forms.Button();
            this.baksmaliBrowseInputDexTxtBox = new System.Windows.Forms.TextBox();
            this.tabPageAdb = new System.Windows.Forms.TabPage();
            this.overrideAbiComboBox = new System.Windows.Forms.ComboBox();
            this.selAdbDeviceLbl = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.killAdbBtn = new System.Windows.Forms.Button();
            this.installApkBtn = new System.Windows.Forms.Button();
            this.refreshDevicesBtn = new System.Windows.Forms.Button();
            this.selApkAdbBtn = new System.Windows.Forms.Button();
            this.overrideAbiCheckBox = new System.Windows.Forms.CheckBox();
            this.setVendorChkBox = new System.Windows.Forms.CheckBox();
            this.apkPathAdbTxtBox = new System.Windows.Forms.TextBox();
            this.label32 = new System.Windows.Forms.Label();
            this.devicesListBox = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelStateImage = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelStateText = new System.Windows.Forms.ToolStripStatusLabel();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.contextMenuStripLog = new System.Windows.Forms.ContextMenuStrip();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTxtBox = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newInsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveLogToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openTempFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearTempFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.useAPKEditorForDecompilingItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportAnIsuueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.apktoolIssuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.baksmaliIssuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlMain.SuspendLayout();
            this.tabPageMain.SuspendLayout();
            this.mergePanel.SuspendLayout();
            this.signPanel.SuspendLayout();
            this.zipalignPanel.SuspendLayout();
            this.comPanel.SuspendLayout();
            this.decPanel.SuspendLayout();
            this.tabPageApkInfo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.basicInfoTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apkIconPicBox)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPageDecode.SuspendLayout();
            this.groupBox_DECODE_Options.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decJobsLvlUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.decApiLvlUpDown)).BeginInit();
            this.tabPageBuild.SuspendLayout();
            this.groupBox_BUILD_Options.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comJobsLvlUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildApiLvlUpDown)).BeginInit();
            this.tabPageSign.SuspendLayout();
            this.groupBox_SIGN_Options.SuspendLayout();
            this.tabPageZipAlign.SuspendLayout();
            this.groupBox_ZIPALIGN_Options.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ZIPALIGN_AlignmentBytes)).BeginInit();
            this.tabPageInstallFramework.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox_IF_Options.SuspendLayout();
            this.tabPageBaksmali.SuspendLayout();
            this.smaliGroupBox.SuspendLayout();
            this.bakSmaliGroupBox.SuspendLayout();
            this.tabPageAdb.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStripLog.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.AllowDrop = true;
            this.tabControlMain.Controls.Add(this.tabPageMain);
            this.tabControlMain.Controls.Add(this.tabPageApkInfo);
            this.tabControlMain.Controls.Add(this.tabPageDecode);
            this.tabControlMain.Controls.Add(this.tabPageBuild);
            this.tabControlMain.Controls.Add(this.tabPageSign);
            this.tabControlMain.Controls.Add(this.tabPageZipAlign);
            this.tabControlMain.Controls.Add(this.tabPageInstallFramework);
            this.tabControlMain.Controls.Add(this.tabPageBaksmali);
            this.tabControlMain.Controls.Add(this.tabPageAdb);
            resources.ApplyResources(this.tabControlMain, "tabControlMain");
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.SelectedIndexChanged += new System.EventHandler(this.tabControlMain_SelectedIndexChanged);
            // 
            // tabPageMain
            // 
            this.tabPageMain.BackColor = System.Drawing.Color.White;
            this.tabPageMain.Controls.Add(this.mergePanel);
            this.tabPageMain.Controls.Add(this.comApkOpenDir);
            this.tabPageMain.Controls.Add(this.decOutOpenDirBtn);
            this.tabPageMain.Controls.Add(this.signApkOpenDirBtn);
            this.tabPageMain.Controls.Add(this.alignApkOpenDirBtn);
            this.tabPageMain.Controls.Add(this.decApkOpenDirBtn);
            this.tabPageMain.Controls.Add(this.compileOutputOpenDirBtn);
            this.tabPageMain.Controls.Add(this.button_OpenMainActivity);
            this.tabPageMain.Controls.Add(this.openApktoolYmlBtn);
            this.tabPageMain.Controls.Add(this.openAndroidMainfestBtn);
            this.tabPageMain.Controls.Add(this.signPanel);
            this.tabPageMain.Controls.Add(this.zipalignPanel);
            this.tabPageMain.Controls.Add(this.comPanel);
            this.tabPageMain.Controls.Add(this.decPanel);
            resources.ApplyResources(this.tabPageMain, "tabPageMain");
            this.tabPageMain.Name = "tabPageMain";
            // 
            // mergePanel
            // 
            this.mergePanel.AllowDrop = true;
            this.mergePanel.Controls.Add(this.splitApkTxt);
            this.mergePanel.Controls.Add(this.splitApkPathTxtBox);
            this.mergePanel.Controls.Add(this.selSplitApkBtn);
            this.mergePanel.Controls.Add(this.mergeApkBtn);
            resources.ApplyResources(this.mergePanel, "mergePanel");
            this.mergePanel.Name = "mergePanel";
            // 
            // splitApkTxt
            // 
            resources.ApplyResources(this.splitApkTxt, "splitApkTxt");
            this.splitApkTxt.Name = "splitApkTxt";
            // 
            // splitApkPathTxtBox
            // 
            this.splitApkPathTxtBox.AllowDrop = true;
            resources.ApplyResources(this.splitApkPathTxtBox, "splitApkPathTxtBox");
            this.splitApkPathTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "SplitApk_InputFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.splitApkPathTxtBox.Name = "splitApkPathTxtBox";
            this.splitApkPathTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.SplitApk_InputFile;
            // 
            // selSplitApkBtn
            // 
            resources.ApplyResources(this.selSplitApkBtn, "selSplitApkBtn");
            this.selSplitApkBtn.Name = "selSplitApkBtn";
            this.selSplitApkBtn.UseVisualStyleBackColor = true;
            // 
            // mergeApkBtn
            // 
            this.mergeApkBtn.AllowDrop = true;
            resources.ApplyResources(this.mergeApkBtn, "mergeApkBtn");
            this.mergeApkBtn.Name = "mergeApkBtn";
            this.mergeApkBtn.UseVisualStyleBackColor = true;
            // 
            // comApkOpenDir
            // 
            resources.ApplyResources(this.comApkOpenDir, "comApkOpenDir");
            this.comApkOpenDir.Name = "comApkOpenDir";
            this.comApkOpenDir.UseVisualStyleBackColor = true;
            // 
            // decOutOpenDirBtn
            // 
            resources.ApplyResources(this.decOutOpenDirBtn, "decOutOpenDirBtn");
            this.decOutOpenDirBtn.Name = "decOutOpenDirBtn";
            this.decOutOpenDirBtn.UseVisualStyleBackColor = true;
            // 
            // signApkOpenDirBtn
            // 
            resources.ApplyResources(this.signApkOpenDirBtn, "signApkOpenDirBtn");
            this.signApkOpenDirBtn.Name = "signApkOpenDirBtn";
            this.signApkOpenDirBtn.UseVisualStyleBackColor = true;
            // 
            // alignApkOpenDirBtn
            // 
            resources.ApplyResources(this.alignApkOpenDirBtn, "alignApkOpenDirBtn");
            this.alignApkOpenDirBtn.Name = "alignApkOpenDirBtn";
            this.alignApkOpenDirBtn.UseVisualStyleBackColor = true;
            // 
            // decApkOpenDirBtn
            // 
            resources.ApplyResources(this.decApkOpenDirBtn, "decApkOpenDirBtn");
            this.decApkOpenDirBtn.Name = "decApkOpenDirBtn";
            this.decApkOpenDirBtn.UseVisualStyleBackColor = true;
            // 
            // compileOutputOpenDirBtn
            // 
            resources.ApplyResources(this.compileOutputOpenDirBtn, "compileOutputOpenDirBtn");
            this.compileOutputOpenDirBtn.Name = "compileOutputOpenDirBtn";
            this.compileOutputOpenDirBtn.UseVisualStyleBackColor = true;
            // 
            // button_OpenMainActivity
            // 
            resources.ApplyResources(this.button_OpenMainActivity, "button_OpenMainActivity");
            this.button_OpenMainActivity.Name = "button_OpenMainActivity";
            this.button_OpenMainActivity.UseVisualStyleBackColor = true;
            // 
            // openApktoolYmlBtn
            // 
            resources.ApplyResources(this.openApktoolYmlBtn, "openApktoolYmlBtn");
            this.openApktoolYmlBtn.Name = "openApktoolYmlBtn";
            this.openApktoolYmlBtn.UseVisualStyleBackColor = true;
            // 
            // openAndroidMainfestBtn
            // 
            resources.ApplyResources(this.openAndroidMainfestBtn, "openAndroidMainfestBtn");
            this.openAndroidMainfestBtn.Name = "openAndroidMainfestBtn";
            this.openAndroidMainfestBtn.UseVisualStyleBackColor = true;
            // 
            // signPanel
            // 
            this.signPanel.AllowDrop = true;
            this.signPanel.Controls.Add(this.label4);
            this.signPanel.Controls.Add(this.textBox_SIGN_InputFile);
            this.signPanel.Controls.Add(this.button_SIGN_BrowseInputFile);
            this.signPanel.Controls.Add(this.button_SIGN_Sign);
            resources.ApplyResources(this.signPanel, "signPanel");
            this.signPanel.Name = "signPanel";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBox_SIGN_InputFile
            // 
            this.textBox_SIGN_InputFile.AllowDrop = true;
            resources.ApplyResources(this.textBox_SIGN_InputFile, "textBox_SIGN_InputFile");
            this.textBox_SIGN_InputFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Sign_InputFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_SIGN_InputFile.Name = "textBox_SIGN_InputFile";
            this.textBox_SIGN_InputFile.Text = global::APKToolGUI.Properties.Settings.Default.Sign_InputFile;
            // 
            // button_SIGN_BrowseInputFile
            // 
            resources.ApplyResources(this.button_SIGN_BrowseInputFile, "button_SIGN_BrowseInputFile");
            this.button_SIGN_BrowseInputFile.Name = "button_SIGN_BrowseInputFile";
            this.button_SIGN_BrowseInputFile.UseVisualStyleBackColor = true;
            // 
            // button_SIGN_Sign
            // 
            this.button_SIGN_Sign.AllowDrop = true;
            resources.ApplyResources(this.button_SIGN_Sign, "button_SIGN_Sign");
            this.button_SIGN_Sign.Name = "button_SIGN_Sign";
            this.button_SIGN_Sign.UseVisualStyleBackColor = true;
            // 
            // zipalignPanel
            // 
            this.zipalignPanel.AllowDrop = true;
            this.zipalignPanel.Controls.Add(this.label3);
            this.zipalignPanel.Controls.Add(this.button_ZIPALIGN_Align);
            this.zipalignPanel.Controls.Add(this.button_ZIPALIGN_BrowseInputFile);
            this.zipalignPanel.Controls.Add(this.textBox_ZIPALIGN_InputFile);
            resources.ApplyResources(this.zipalignPanel, "zipalignPanel");
            this.zipalignPanel.Name = "zipalignPanel";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // button_ZIPALIGN_Align
            // 
            this.button_ZIPALIGN_Align.AllowDrop = true;
            resources.ApplyResources(this.button_ZIPALIGN_Align, "button_ZIPALIGN_Align");
            this.button_ZIPALIGN_Align.Name = "button_ZIPALIGN_Align";
            this.button_ZIPALIGN_Align.UseVisualStyleBackColor = true;
            // 
            // button_ZIPALIGN_BrowseInputFile
            // 
            resources.ApplyResources(this.button_ZIPALIGN_BrowseInputFile, "button_ZIPALIGN_BrowseInputFile");
            this.button_ZIPALIGN_BrowseInputFile.Name = "button_ZIPALIGN_BrowseInputFile";
            this.button_ZIPALIGN_BrowseInputFile.UseVisualStyleBackColor = true;
            // 
            // textBox_ZIPALIGN_InputFile
            // 
            this.textBox_ZIPALIGN_InputFile.AllowDrop = true;
            resources.ApplyResources(this.textBox_ZIPALIGN_InputFile, "textBox_ZIPALIGN_InputFile");
            this.textBox_ZIPALIGN_InputFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Zipalign_InputFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_ZIPALIGN_InputFile.Name = "textBox_ZIPALIGN_InputFile";
            this.textBox_ZIPALIGN_InputFile.Text = global::APKToolGUI.Properties.Settings.Default.Zipalign_InputFile;
            // 
            // comPanel
            // 
            this.comPanel.AllowDrop = true;
            this.comPanel.Controls.Add(this.label2);
            this.comPanel.Controls.Add(this.button_BUILD_BrowseInputProjectDir);
            this.comPanel.Controls.Add(this.button_BUILD_Build);
            this.comPanel.Controls.Add(this.textBox_BUILD_InputProjectDir);
            resources.ApplyResources(this.comPanel, "comPanel");
            this.comPanel.Name = "comPanel";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // button_BUILD_BrowseInputProjectDir
            // 
            resources.ApplyResources(this.button_BUILD_BrowseInputProjectDir, "button_BUILD_BrowseInputProjectDir");
            this.button_BUILD_BrowseInputProjectDir.Name = "button_BUILD_BrowseInputProjectDir";
            this.button_BUILD_BrowseInputProjectDir.UseVisualStyleBackColor = true;
            // 
            // button_BUILD_Build
            // 
            this.button_BUILD_Build.AllowDrop = true;
            resources.ApplyResources(this.button_BUILD_Build, "button_BUILD_Build");
            this.button_BUILD_Build.Name = "button_BUILD_Build";
            this.button_BUILD_Build.UseVisualStyleBackColor = true;
            // 
            // textBox_BUILD_InputProjectDir
            // 
            this.textBox_BUILD_InputProjectDir.AllowDrop = true;
            resources.ApplyResources(this.textBox_BUILD_InputProjectDir, "textBox_BUILD_InputProjectDir");
            this.textBox_BUILD_InputProjectDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Build_InputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_BUILD_InputProjectDir.Name = "textBox_BUILD_InputProjectDir";
            this.textBox_BUILD_InputProjectDir.Text = global::APKToolGUI.Properties.Settings.Default.Build_InputDir;
            // 
            // decPanel
            // 
            this.decPanel.AllowDrop = true;
            this.decPanel.BackColor = System.Drawing.Color.White;
            this.decPanel.Controls.Add(this.label1);
            this.decPanel.Controls.Add(this.textBox_DECODE_InputAppPath);
            this.decPanel.Controls.Add(this.button_DECODE_Decode);
            this.decPanel.Controls.Add(this.button_DECODE_BrowseInputAppPath);
            resources.ApplyResources(this.decPanel, "decPanel");
            this.decPanel.Name = "decPanel";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // textBox_DECODE_InputAppPath
            // 
            this.textBox_DECODE_InputAppPath.AllowDrop = true;
            resources.ApplyResources(this.textBox_DECODE_InputAppPath, "textBox_DECODE_InputAppPath");
            this.textBox_DECODE_InputAppPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Decode_InputAppPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_DECODE_InputAppPath.Name = "textBox_DECODE_InputAppPath";
            this.textBox_DECODE_InputAppPath.Text = global::APKToolGUI.Properties.Settings.Default.Decode_InputAppPath;
            // 
            // button_DECODE_Decode
            // 
            this.button_DECODE_Decode.AllowDrop = true;
            resources.ApplyResources(this.button_DECODE_Decode, "button_DECODE_Decode");
            this.button_DECODE_Decode.Name = "button_DECODE_Decode";
            this.button_DECODE_Decode.UseVisualStyleBackColor = true;
            // 
            // button_DECODE_BrowseInputAppPath
            // 
            resources.ApplyResources(this.button_DECODE_BrowseInputAppPath, "button_DECODE_BrowseInputAppPath");
            this.button_DECODE_BrowseInputAppPath.Name = "button_DECODE_BrowseInputAppPath";
            this.button_DECODE_BrowseInputAppPath.UseVisualStyleBackColor = true;
            // 
            // tabPageApkInfo
            // 
            this.tabPageApkInfo.AllowDrop = true;
            this.tabPageApkInfo.BackColor = System.Drawing.Color.White;
            this.tabPageApkInfo.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.tabPageApkInfo, "tabPageApkInfo");
            this.tabPageApkInfo.Name = "tabPageApkInfo";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.basicInfoTabPage);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            // 
            // basicInfoTabPage
            // 
            this.basicInfoTabPage.AllowDrop = true;
            resources.ApplyResources(this.basicInfoTabPage, "basicInfoTabPage");
            this.basicInfoTabPage.Controls.Add(this.sigTxtBox);
            this.basicInfoTabPage.Controls.Add(this.label5);
            this.basicInfoTabPage.Controls.Add(this.launchActivityTxtBox);
            this.basicInfoTabPage.Controls.Add(this.label31);
            this.basicInfoTabPage.Controls.Add(this.archSdkTxtBox);
            this.basicInfoTabPage.Controls.Add(this.label30);
            this.basicInfoTabPage.Controls.Add(this.apkMirrorLinkBtn);
            this.basicInfoTabPage.Controls.Add(this.apkSupportLinkBtn);
            this.basicInfoTabPage.Controls.Add(this.apkGkLinkBtn);
            this.basicInfoTabPage.Controls.Add(this.label17);
            this.basicInfoTabPage.Controls.Add(this.localsTxtBox);
            this.basicInfoTabPage.Controls.Add(this.selApkFileInfoBtn);
            this.basicInfoTabPage.Controls.Add(this.label18);
            this.basicInfoTabPage.Controls.Add(this.appTxtBox);
            this.basicInfoTabPage.Controls.Add(this.permTxtBox);
            this.basicInfoTabPage.Controls.Add(this.apkComboLinkBtn);
            this.basicInfoTabPage.Controls.Add(this.label15);
            this.basicInfoTabPage.Controls.Add(this.fileTxtBox);
            this.basicInfoTabPage.Controls.Add(this.label14);
            this.basicInfoTabPage.Controls.Add(this.densityTxtBox);
            this.basicInfoTabPage.Controls.Add(this.packNameTxtBox);
            this.basicInfoTabPage.Controls.Add(this.apkPureLinkBtn);
            this.basicInfoTabPage.Controls.Add(this.verTxtBox);
            this.basicInfoTabPage.Controls.Add(this.psLinkBtn);
            this.basicInfoTabPage.Controls.Add(this.minSdkTxtBox);
            this.basicInfoTabPage.Controls.Add(this.label19);
            this.basicInfoTabPage.Controls.Add(this.targetSdkTxtBox);
            this.basicInfoTabPage.Controls.Add(this.screenTxtBox);
            this.basicInfoTabPage.Controls.Add(this.label7);
            this.basicInfoTabPage.Controls.Add(this.label9);
            this.basicInfoTabPage.Controls.Add(this.buildTxtBox);
            this.basicInfoTabPage.Controls.Add(this.label8);
            this.basicInfoTabPage.Controls.Add(this.apkIconPicBox);
            this.basicInfoTabPage.Controls.Add(this.label11);
            this.basicInfoTabPage.Controls.Add(this.label10);
            this.basicInfoTabPage.Controls.Add(this.label13);
            this.basicInfoTabPage.Controls.Add(this.label12);
            this.basicInfoTabPage.Name = "basicInfoTabPage";
            this.basicInfoTabPage.UseVisualStyleBackColor = true;
            // 
            // sigTxtBox
            // 
            resources.ApplyResources(this.sigTxtBox, "sigTxtBox");
            this.sigTxtBox.Name = "sigTxtBox";
            this.sigTxtBox.ReadOnly = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // launchActivityTxtBox
            // 
            resources.ApplyResources(this.launchActivityTxtBox, "launchActivityTxtBox");
            this.launchActivityTxtBox.Name = "launchActivityTxtBox";
            this.launchActivityTxtBox.ReadOnly = true;
            // 
            // label31
            // 
            resources.ApplyResources(this.label31, "label31");
            this.label31.Name = "label31";
            // 
            // archSdkTxtBox
            // 
            resources.ApplyResources(this.archSdkTxtBox, "archSdkTxtBox");
            this.archSdkTxtBox.Name = "archSdkTxtBox";
            this.archSdkTxtBox.ReadOnly = true;
            // 
            // label30
            // 
            resources.ApplyResources(this.label30, "label30");
            this.label30.Name = "label30";
            // 
            // apkMirrorLinkBtn
            // 
            resources.ApplyResources(this.apkMirrorLinkBtn, "apkMirrorLinkBtn");
            this.apkMirrorLinkBtn.Name = "apkMirrorLinkBtn";
            this.apkMirrorLinkBtn.UseVisualStyleBackColor = true;
            // 
            // apkSupportLinkBtn
            // 
            resources.ApplyResources(this.apkSupportLinkBtn, "apkSupportLinkBtn");
            this.apkSupportLinkBtn.Name = "apkSupportLinkBtn";
            this.apkSupportLinkBtn.UseVisualStyleBackColor = true;
            // 
            // apkGkLinkBtn
            // 
            resources.ApplyResources(this.apkGkLinkBtn, "apkGkLinkBtn");
            this.apkGkLinkBtn.Name = "apkGkLinkBtn";
            this.apkGkLinkBtn.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // localsTxtBox
            // 
            resources.ApplyResources(this.localsTxtBox, "localsTxtBox");
            this.localsTxtBox.Name = "localsTxtBox";
            this.localsTxtBox.ReadOnly = true;
            // 
            // selApkFileInfoBtn
            // 
            resources.ApplyResources(this.selApkFileInfoBtn, "selApkFileInfoBtn");
            this.selApkFileInfoBtn.Name = "selApkFileInfoBtn";
            this.selApkFileInfoBtn.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // appTxtBox
            // 
            resources.ApplyResources(this.appTxtBox, "appTxtBox");
            this.appTxtBox.Name = "appTxtBox";
            this.appTxtBox.ReadOnly = true;
            // 
            // permTxtBox
            // 
            resources.ApplyResources(this.permTxtBox, "permTxtBox");
            this.permTxtBox.Name = "permTxtBox";
            this.permTxtBox.ReadOnly = true;
            // 
            // apkComboLinkBtn
            // 
            resources.ApplyResources(this.apkComboLinkBtn, "apkComboLinkBtn");
            this.apkComboLinkBtn.Name = "apkComboLinkBtn";
            this.apkComboLinkBtn.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // fileTxtBox
            // 
            resources.ApplyResources(this.fileTxtBox, "fileTxtBox");
            this.fileTxtBox.Name = "fileTxtBox";
            this.fileTxtBox.ReadOnly = true;
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // densityTxtBox
            // 
            resources.ApplyResources(this.densityTxtBox, "densityTxtBox");
            this.densityTxtBox.Name = "densityTxtBox";
            this.densityTxtBox.ReadOnly = true;
            // 
            // packNameTxtBox
            // 
            resources.ApplyResources(this.packNameTxtBox, "packNameTxtBox");
            this.packNameTxtBox.Name = "packNameTxtBox";
            this.packNameTxtBox.ReadOnly = true;
            // 
            // apkPureLinkBtn
            // 
            resources.ApplyResources(this.apkPureLinkBtn, "apkPureLinkBtn");
            this.apkPureLinkBtn.Name = "apkPureLinkBtn";
            this.apkPureLinkBtn.UseVisualStyleBackColor = true;
            // 
            // verTxtBox
            // 
            resources.ApplyResources(this.verTxtBox, "verTxtBox");
            this.verTxtBox.Name = "verTxtBox";
            this.verTxtBox.ReadOnly = true;
            // 
            // psLinkBtn
            // 
            resources.ApplyResources(this.psLinkBtn, "psLinkBtn");
            this.psLinkBtn.Name = "psLinkBtn";
            this.psLinkBtn.UseVisualStyleBackColor = true;
            // 
            // minSdkTxtBox
            // 
            resources.ApplyResources(this.minSdkTxtBox, "minSdkTxtBox");
            this.minSdkTxtBox.Name = "minSdkTxtBox";
            this.minSdkTxtBox.ReadOnly = true;
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // targetSdkTxtBox
            // 
            resources.ApplyResources(this.targetSdkTxtBox, "targetSdkTxtBox");
            this.targetSdkTxtBox.Name = "targetSdkTxtBox";
            this.targetSdkTxtBox.ReadOnly = true;
            // 
            // screenTxtBox
            // 
            resources.ApplyResources(this.screenTxtBox, "screenTxtBox");
            this.screenTxtBox.Name = "screenTxtBox";
            this.screenTxtBox.ReadOnly = true;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // buildTxtBox
            // 
            resources.ApplyResources(this.buildTxtBox, "buildTxtBox");
            this.buildTxtBox.Name = "buildTxtBox";
            this.buildTxtBox.ReadOnly = true;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // apkIconPicBox
            // 
            this.apkIconPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.apkIconPicBox, "apkIconPicBox");
            this.apkIconPicBox.Name = "apkIconPicBox";
            this.apkIconPicBox.TabStop = false;
            this.apkIconPicBox.Click += new System.EventHandler(this.apkIconPicBox_Click);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.fullInfoTextBox);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // fullInfoTextBox
            // 
            resources.ApplyResources(this.fullInfoTextBox, "fullInfoTextBox");
            this.fullInfoTextBox.Name = "fullInfoTextBox";
            // 
            // tabPageDecode
            // 
            resources.ApplyResources(this.tabPageDecode, "tabPageDecode");
            this.tabPageDecode.BackColor = System.Drawing.SystemColors.Window;
            this.tabPageDecode.Controls.Add(this.groupBox_DECODE_Options);
            this.tabPageDecode.Name = "tabPageDecode";
            // 
            // groupBox_DECODE_Options
            // 
            this.groupBox_DECODE_Options.Controls.Add(this.decJobsLvlUpDown);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox3);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox7);
            this.groupBox_DECODE_Options.Controls.Add(this.decApiLvlUpDown);
            this.groupBox_DECODE_Options.Controls.Add(this.decSetApiLvlChkBox);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_FixError);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_OnlyMainClasses);
            this.groupBox_DECODE_Options.Controls.Add(this.textBox_DECODE_FrameDir);
            this.groupBox_DECODE_Options.Controls.Add(this.button_DECODE_BrowseOutputDirectory);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_UseFramework);
            this.groupBox_DECODE_Options.Controls.Add(this.button_DECODE_BrowseFrameDir);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_MatchOriginal);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_OutputDirectory);
            this.groupBox_DECODE_Options.Controls.Add(this.textBox_DECODE_OutputDirectory);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_KeepBrokenRes);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_NoSrc);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_Force);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_NoRes);
            this.groupBox_DECODE_Options.Controls.Add(this.checkBox_DECODE_NoDebugInfo);
            resources.ApplyResources(this.groupBox_DECODE_Options, "groupBox_DECODE_Options");
            this.groupBox_DECODE_Options.Name = "groupBox_DECODE_Options";
            this.groupBox_DECODE_Options.TabStop = false;
            // 
            // decJobsLvlUpDown
            // 
            this.decJobsLvlUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::APKToolGUI.Properties.Settings.Default, "Decode_Jobs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.decJobsLvlUpDown, "decJobsLvlUpDown");
            this.decJobsLvlUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.decJobsLvlUpDown.Name = "decJobsLvlUpDown";
            this.decJobsLvlUpDown.Value = global::APKToolGUI.Properties.Settings.Default.Decode_Jobs;
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_SetJobs;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_SetJobs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            resources.ApplyResources(this.checkBox7, "checkBox7");
            this.checkBox7.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_DontParseApkInfo;
            this.checkBox7.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_DontParseApkInfo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // decApiLvlUpDown
            // 
            this.decApiLvlUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::APKToolGUI.Properties.Settings.Default, "Decode_ApiLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.decApiLvlUpDown, "decApiLvlUpDown");
            this.decApiLvlUpDown.Name = "decApiLvlUpDown";
            this.decApiLvlUpDown.Value = global::APKToolGUI.Properties.Settings.Default.Decode_ApiLevel;
            // 
            // decSetApiLvlChkBox
            // 
            resources.ApplyResources(this.decSetApiLvlChkBox, "decSetApiLvlChkBox");
            this.decSetApiLvlChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_SetApiLevel;
            this.decSetApiLvlChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_SetApiLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.decSetApiLvlChkBox.Name = "decSetApiLvlChkBox";
            this.decSetApiLvlChkBox.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_FixError
            // 
            resources.ApplyResources(this.checkBox_DECODE_FixError, "checkBox_DECODE_FixError");
            this.checkBox_DECODE_FixError.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_FixError;
            this.checkBox_DECODE_FixError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DECODE_FixError.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_FixError", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_FixError.Name = "checkBox_DECODE_FixError";
            this.toolTip1.SetToolTip(this.checkBox_DECODE_FixError, resources.GetString("checkBox_DECODE_FixError.ToolTip"));
            this.checkBox_DECODE_FixError.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_OnlyMainClasses
            // 
            resources.ApplyResources(this.checkBox_DECODE_OnlyMainClasses, "checkBox_DECODE_OnlyMainClasses");
            this.checkBox_DECODE_OnlyMainClasses.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_OnlyMainClasses;
            this.checkBox_DECODE_OnlyMainClasses.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DECODE_OnlyMainClasses.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_OnlyMainClasses", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_OnlyMainClasses.Name = "checkBox_DECODE_OnlyMainClasses";
            this.toolTip1.SetToolTip(this.checkBox_DECODE_OnlyMainClasses, resources.GetString("checkBox_DECODE_OnlyMainClasses.ToolTip"));
            this.checkBox_DECODE_OnlyMainClasses.UseVisualStyleBackColor = true;
            // 
            // textBox_DECODE_FrameDir
            // 
            resources.ApplyResources(this.textBox_DECODE_FrameDir, "textBox_DECODE_FrameDir");
            this.textBox_DECODE_FrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Decode_UseFramework", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_DECODE_FrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Framework_FrameDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_DECODE_FrameDir.Enabled = global::APKToolGUI.Properties.Settings.Default.Decode_UseFramework;
            this.textBox_DECODE_FrameDir.Name = "textBox_DECODE_FrameDir";
            this.textBox_DECODE_FrameDir.Text = global::APKToolGUI.Properties.Settings.Default.Framework_FrameDir;
            // 
            // button_DECODE_BrowseOutputDirectory
            // 
            resources.ApplyResources(this.button_DECODE_BrowseOutputDirectory, "button_DECODE_BrowseOutputDirectory");
            this.button_DECODE_BrowseOutputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Decode_UseOutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button_DECODE_BrowseOutputDirectory.Enabled = global::APKToolGUI.Properties.Settings.Default.Decode_UseOutputDir;
            this.button_DECODE_BrowseOutputDirectory.Name = "button_DECODE_BrowseOutputDirectory";
            this.button_DECODE_BrowseOutputDirectory.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_UseFramework
            // 
            resources.ApplyResources(this.checkBox_DECODE_UseFramework, "checkBox_DECODE_UseFramework");
            this.checkBox_DECODE_UseFramework.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_UseFramework;
            this.checkBox_DECODE_UseFramework.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_UseFramework", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_UseFramework.Name = "checkBox_DECODE_UseFramework";
            this.checkBox_DECODE_UseFramework.UseVisualStyleBackColor = true;
            // 
            // button_DECODE_BrowseFrameDir
            // 
            resources.ApplyResources(this.button_DECODE_BrowseFrameDir, "button_DECODE_BrowseFrameDir");
            this.button_DECODE_BrowseFrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Decode_UseFramework", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button_DECODE_BrowseFrameDir.Enabled = global::APKToolGUI.Properties.Settings.Default.Decode_UseFramework;
            this.button_DECODE_BrowseFrameDir.Name = "button_DECODE_BrowseFrameDir";
            this.button_DECODE_BrowseFrameDir.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_MatchOriginal
            // 
            resources.ApplyResources(this.checkBox_DECODE_MatchOriginal, "checkBox_DECODE_MatchOriginal");
            this.checkBox_DECODE_MatchOriginal.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_MatchOriginal;
            this.checkBox_DECODE_MatchOriginal.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_MatchOriginal", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_MatchOriginal.Name = "checkBox_DECODE_MatchOriginal";
            this.checkBox_DECODE_MatchOriginal.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_OutputDirectory
            // 
            resources.ApplyResources(this.checkBox_DECODE_OutputDirectory, "checkBox_DECODE_OutputDirectory");
            this.checkBox_DECODE_OutputDirectory.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_UseOutputDir;
            this.checkBox_DECODE_OutputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_UseOutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_OutputDirectory.Name = "checkBox_DECODE_OutputDirectory";
            this.checkBox_DECODE_OutputDirectory.UseVisualStyleBackColor = true;
            // 
            // textBox_DECODE_OutputDirectory
            // 
            resources.ApplyResources(this.textBox_DECODE_OutputDirectory, "textBox_DECODE_OutputDirectory");
            this.textBox_DECODE_OutputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Decode_OutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_DECODE_OutputDirectory.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Decode_UseOutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_DECODE_OutputDirectory.Enabled = global::APKToolGUI.Properties.Settings.Default.Decode_UseOutputDir;
            this.textBox_DECODE_OutputDirectory.Name = "textBox_DECODE_OutputDirectory";
            this.textBox_DECODE_OutputDirectory.Text = global::APKToolGUI.Properties.Settings.Default.Decode_OutputDir;
            // 
            // checkBox_DECODE_KeepBrokenRes
            // 
            resources.ApplyResources(this.checkBox_DECODE_KeepBrokenRes, "checkBox_DECODE_KeepBrokenRes");
            this.checkBox_DECODE_KeepBrokenRes.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_KeepBrokenRes;
            this.checkBox_DECODE_KeepBrokenRes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_KeepBrokenRes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_KeepBrokenRes.Name = "checkBox_DECODE_KeepBrokenRes";
            this.checkBox_DECODE_KeepBrokenRes.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_NoSrc
            // 
            resources.ApplyResources(this.checkBox_DECODE_NoSrc, "checkBox_DECODE_NoSrc");
            this.checkBox_DECODE_NoSrc.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_NoSrc;
            this.checkBox_DECODE_NoSrc.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_NoSrc", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_NoSrc.Name = "checkBox_DECODE_NoSrc";
            this.checkBox_DECODE_NoSrc.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_Force
            // 
            resources.ApplyResources(this.checkBox_DECODE_Force, "checkBox_DECODE_Force");
            this.checkBox_DECODE_Force.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_Force;
            this.checkBox_DECODE_Force.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_DECODE_Force.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_Force", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_Force.Name = "checkBox_DECODE_Force";
            this.checkBox_DECODE_Force.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_NoRes
            // 
            resources.ApplyResources(this.checkBox_DECODE_NoRes, "checkBox_DECODE_NoRes");
            this.checkBox_DECODE_NoRes.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_NoRes;
            this.checkBox_DECODE_NoRes.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_NoRes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_NoRes.Name = "checkBox_DECODE_NoRes";
            this.checkBox_DECODE_NoRes.UseVisualStyleBackColor = true;
            // 
            // checkBox_DECODE_NoDebugInfo
            // 
            resources.ApplyResources(this.checkBox_DECODE_NoDebugInfo, "checkBox_DECODE_NoDebugInfo");
            this.checkBox_DECODE_NoDebugInfo.Checked = global::APKToolGUI.Properties.Settings.Default.Decode_NoDebugInfo;
            this.checkBox_DECODE_NoDebugInfo.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Decode_NoDebugInfo", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_DECODE_NoDebugInfo.Name = "checkBox_DECODE_NoDebugInfo";
            this.checkBox_DECODE_NoDebugInfo.UseVisualStyleBackColor = true;
            // 
            // tabPageBuild
            // 
            resources.ApplyResources(this.tabPageBuild, "tabPageBuild");
            this.tabPageBuild.BackColor = System.Drawing.Color.White;
            this.tabPageBuild.Controls.Add(this.groupBox_BUILD_Options);
            this.tabPageBuild.Name = "tabPageBuild";
            // 
            // groupBox_BUILD_Options
            // 
            this.groupBox_BUILD_Options.Controls.Add(this.comJobsLvlUpDown);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox4);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox_BUILD_NetSecConf);
            this.groupBox_BUILD_Options.Controls.Add(this.useAapt2ChkBox);
            this.groupBox_BUILD_Options.Controls.Add(this.buildApiLvlUpDown);
            this.groupBox_BUILD_Options.Controls.Add(this.buildSetApiLvlChkBox);
            this.groupBox_BUILD_Options.Controls.Add(this.createUnsignApkChkBox);
            this.groupBox_BUILD_Options.Controls.Add(this.signAfterBuildChkBox);
            this.groupBox_BUILD_Options.Controls.Add(this.zipalignAfterBuildChkBox);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox_BUILD_NoCrunch);
            this.groupBox_BUILD_Options.Controls.Add(this.button_BUILD_BrowseOutputAppPath);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox_BUILD_ForceAll);
            this.groupBox_BUILD_Options.Controls.Add(this.button_BUILD_BrowseFrameDir);
            this.groupBox_BUILD_Options.Controls.Add(this.button_BUILD_BrowseAaptPath);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox_BUILD_OutputAppPath);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox_BUILD_CopyOriginal);
            this.groupBox_BUILD_Options.Controls.Add(this.textBox_BUILD_OutputAppPath);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox_BUILD_UseAapt);
            this.groupBox_BUILD_Options.Controls.Add(this.textBox_BUILD_AaptPath);
            this.groupBox_BUILD_Options.Controls.Add(this.textBox_BUILD_FrameDir);
            this.groupBox_BUILD_Options.Controls.Add(this.checkBox_BUILD_UseFramework);
            resources.ApplyResources(this.groupBox_BUILD_Options, "groupBox_BUILD_Options");
            this.groupBox_BUILD_Options.Name = "groupBox_BUILD_Options";
            this.groupBox_BUILD_Options.TabStop = false;
            // 
            // comJobsLvlUpDown
            // 
            this.comJobsLvlUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::APKToolGUI.Properties.Settings.Default, "Build_Jobs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.comJobsLvlUpDown, "comJobsLvlUpDown");
            this.comJobsLvlUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.comJobsLvlUpDown.Name = "comJobsLvlUpDown";
            this.comJobsLvlUpDown.Value = global::APKToolGUI.Properties.Settings.Default.Build_Jobs;
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.Checked = global::APKToolGUI.Properties.Settings.Default.Build_SetJobs;
            this.checkBox4.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_SetJobs", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox_BUILD_NetSecConf
            // 
            resources.ApplyResources(this.checkBox_BUILD_NetSecConf, "checkBox_BUILD_NetSecConf");
            this.checkBox_BUILD_NetSecConf.Checked = global::APKToolGUI.Properties.Settings.Default.Build_NetSecConf;
            this.checkBox_BUILD_NetSecConf.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_NetSecConf", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_BUILD_NetSecConf.Name = "checkBox_BUILD_NetSecConf";
            this.checkBox_BUILD_NetSecConf.UseVisualStyleBackColor = true;
            // 
            // useAapt2ChkBox
            // 
            resources.ApplyResources(this.useAapt2ChkBox, "useAapt2ChkBox");
            this.useAapt2ChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Build_UseAapt2;
            this.useAapt2ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAapt2ChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_UseAapt2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.useAapt2ChkBox.Name = "useAapt2ChkBox";
            this.useAapt2ChkBox.UseVisualStyleBackColor = true;
            // 
            // buildApiLvlUpDown
            // 
            this.buildApiLvlUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::APKToolGUI.Properties.Settings.Default, "Build_ApiLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.buildApiLvlUpDown, "buildApiLvlUpDown");
            this.buildApiLvlUpDown.Name = "buildApiLvlUpDown";
            this.buildApiLvlUpDown.Value = global::APKToolGUI.Properties.Settings.Default.Build_ApiLevel;
            // 
            // buildSetApiLvlChkBox
            // 
            resources.ApplyResources(this.buildSetApiLvlChkBox, "buildSetApiLvlChkBox");
            this.buildSetApiLvlChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Build_SetApiLevel;
            this.buildSetApiLvlChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_SetApiLevel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.buildSetApiLvlChkBox.Name = "buildSetApiLvlChkBox";
            this.buildSetApiLvlChkBox.UseVisualStyleBackColor = true;
            // 
            // createUnsignApkChkBox
            // 
            resources.ApplyResources(this.createUnsignApkChkBox, "createUnsignApkChkBox");
            this.createUnsignApkChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Build_CreateUnsignedApk;
            this.createUnsignApkChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_CreateUnsignedApk", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.createUnsignApkChkBox.Name = "createUnsignApkChkBox";
            this.toolTip1.SetToolTip(this.createUnsignApkChkBox, resources.GetString("createUnsignApkChkBox.ToolTip"));
            this.createUnsignApkChkBox.UseVisualStyleBackColor = true;
            // 
            // signAfterBuildChkBox
            // 
            resources.ApplyResources(this.signAfterBuildChkBox, "signAfterBuildChkBox");
            this.signAfterBuildChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Build_SignAfterBuild;
            this.signAfterBuildChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.signAfterBuildChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_SignAfterBuild", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.signAfterBuildChkBox.Name = "signAfterBuildChkBox";
            this.signAfterBuildChkBox.UseVisualStyleBackColor = true;
            // 
            // zipalignAfterBuildChkBox
            // 
            resources.ApplyResources(this.zipalignAfterBuildChkBox, "zipalignAfterBuildChkBox");
            this.zipalignAfterBuildChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Build_ZipalignAfterBuild;
            this.zipalignAfterBuildChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.zipalignAfterBuildChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_ZipalignAfterBuild", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.zipalignAfterBuildChkBox.Name = "zipalignAfterBuildChkBox";
            this.zipalignAfterBuildChkBox.UseVisualStyleBackColor = true;
            // 
            // checkBox_BUILD_NoCrunch
            // 
            resources.ApplyResources(this.checkBox_BUILD_NoCrunch, "checkBox_BUILD_NoCrunch");
            this.checkBox_BUILD_NoCrunch.Checked = global::APKToolGUI.Properties.Settings.Default.Build_NoCrunch;
            this.checkBox_BUILD_NoCrunch.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_NoCrunch", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_BUILD_NoCrunch.Name = "checkBox_BUILD_NoCrunch";
            this.checkBox_BUILD_NoCrunch.UseVisualStyleBackColor = true;
            // 
            // button_BUILD_BrowseOutputAppPath
            // 
            resources.ApplyResources(this.button_BUILD_BrowseOutputAppPath, "button_BUILD_BrowseOutputAppPath");
            this.button_BUILD_BrowseOutputAppPath.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Build_UseOutputAppPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button_BUILD_BrowseOutputAppPath.Enabled = global::APKToolGUI.Properties.Settings.Default.Build_UseOutputAppPath;
            this.button_BUILD_BrowseOutputAppPath.Name = "button_BUILD_BrowseOutputAppPath";
            this.button_BUILD_BrowseOutputAppPath.UseVisualStyleBackColor = true;
            // 
            // checkBox_BUILD_ForceAll
            // 
            resources.ApplyResources(this.checkBox_BUILD_ForceAll, "checkBox_BUILD_ForceAll");
            this.checkBox_BUILD_ForceAll.Checked = global::APKToolGUI.Properties.Settings.Default.Build_ForceAll;
            this.checkBox_BUILD_ForceAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_BUILD_ForceAll.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_ForceAll", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_BUILD_ForceAll.Name = "checkBox_BUILD_ForceAll";
            this.checkBox_BUILD_ForceAll.UseVisualStyleBackColor = true;
            // 
            // button_BUILD_BrowseFrameDir
            // 
            resources.ApplyResources(this.button_BUILD_BrowseFrameDir, "button_BUILD_BrowseFrameDir");
            this.button_BUILD_BrowseFrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Build_UseFramework", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button_BUILD_BrowseFrameDir.Enabled = global::APKToolGUI.Properties.Settings.Default.Build_UseFramework;
            this.button_BUILD_BrowseFrameDir.Name = "button_BUILD_BrowseFrameDir";
            this.button_BUILD_BrowseFrameDir.UseVisualStyleBackColor = true;
            // 
            // button_BUILD_BrowseAaptPath
            // 
            resources.ApplyResources(this.button_BUILD_BrowseAaptPath, "button_BUILD_BrowseAaptPath");
            this.button_BUILD_BrowseAaptPath.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Build_UseAapt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button_BUILD_BrowseAaptPath.Enabled = global::APKToolGUI.Properties.Settings.Default.Build_UseAapt;
            this.button_BUILD_BrowseAaptPath.Name = "button_BUILD_BrowseAaptPath";
            this.button_BUILD_BrowseAaptPath.UseVisualStyleBackColor = true;
            // 
            // checkBox_BUILD_OutputAppPath
            // 
            resources.ApplyResources(this.checkBox_BUILD_OutputAppPath, "checkBox_BUILD_OutputAppPath");
            this.checkBox_BUILD_OutputAppPath.Checked = global::APKToolGUI.Properties.Settings.Default.Build_UseOutputAppPath;
            this.checkBox_BUILD_OutputAppPath.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_UseOutputAppPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_BUILD_OutputAppPath.Name = "checkBox_BUILD_OutputAppPath";
            this.toolTip1.SetToolTip(this.checkBox_BUILD_OutputAppPath, resources.GetString("checkBox_BUILD_OutputAppPath.ToolTip"));
            this.checkBox_BUILD_OutputAppPath.UseVisualStyleBackColor = true;
            // 
            // checkBox_BUILD_CopyOriginal
            // 
            resources.ApplyResources(this.checkBox_BUILD_CopyOriginal, "checkBox_BUILD_CopyOriginal");
            this.checkBox_BUILD_CopyOriginal.Checked = global::APKToolGUI.Properties.Settings.Default.Build_CopyOriginal;
            this.checkBox_BUILD_CopyOriginal.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_CopyOriginal", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_BUILD_CopyOriginal.Name = "checkBox_BUILD_CopyOriginal";
            this.checkBox_BUILD_CopyOriginal.UseVisualStyleBackColor = true;
            // 
            // textBox_BUILD_OutputAppPath
            // 
            resources.ApplyResources(this.textBox_BUILD_OutputAppPath, "textBox_BUILD_OutputAppPath");
            this.textBox_BUILD_OutputAppPath.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Build_UseOutputAppPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_BUILD_OutputAppPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Build_OutputAppPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_BUILD_OutputAppPath.Enabled = global::APKToolGUI.Properties.Settings.Default.Build_UseOutputAppPath;
            this.textBox_BUILD_OutputAppPath.Name = "textBox_BUILD_OutputAppPath";
            this.textBox_BUILD_OutputAppPath.Text = global::APKToolGUI.Properties.Settings.Default.Build_OutputAppPath;
            // 
            // checkBox_BUILD_UseAapt
            // 
            resources.ApplyResources(this.checkBox_BUILD_UseAapt, "checkBox_BUILD_UseAapt");
            this.checkBox_BUILD_UseAapt.Checked = global::APKToolGUI.Properties.Settings.Default.Build_UseAapt;
            this.checkBox_BUILD_UseAapt.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_UseAapt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_BUILD_UseAapt.Name = "checkBox_BUILD_UseAapt";
            this.checkBox_BUILD_UseAapt.UseVisualStyleBackColor = true;
            // 
            // textBox_BUILD_AaptPath
            // 
            resources.ApplyResources(this.textBox_BUILD_AaptPath, "textBox_BUILD_AaptPath");
            this.textBox_BUILD_AaptPath.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Build_UseAapt", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_BUILD_AaptPath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Build_AaptPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_BUILD_AaptPath.Enabled = global::APKToolGUI.Properties.Settings.Default.Build_UseAapt;
            this.textBox_BUILD_AaptPath.Name = "textBox_BUILD_AaptPath";
            this.textBox_BUILD_AaptPath.Text = global::APKToolGUI.Properties.Settings.Default.Build_AaptPath;
            // 
            // textBox_BUILD_FrameDir
            // 
            resources.ApplyResources(this.textBox_BUILD_FrameDir, "textBox_BUILD_FrameDir");
            this.textBox_BUILD_FrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Build_UseFramework", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_BUILD_FrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Framework_FrameDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_BUILD_FrameDir.Enabled = global::APKToolGUI.Properties.Settings.Default.Build_UseFramework;
            this.textBox_BUILD_FrameDir.Name = "textBox_BUILD_FrameDir";
            this.textBox_BUILD_FrameDir.Text = global::APKToolGUI.Properties.Settings.Default.Framework_FrameDir;
            // 
            // checkBox_BUILD_UseFramework
            // 
            resources.ApplyResources(this.checkBox_BUILD_UseFramework, "checkBox_BUILD_UseFramework");
            this.checkBox_BUILD_UseFramework.Checked = global::APKToolGUI.Properties.Settings.Default.Build_UseFramework;
            this.checkBox_BUILD_UseFramework.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Build_UseFramework", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_BUILD_UseFramework.Name = "checkBox_BUILD_UseFramework";
            this.checkBox_BUILD_UseFramework.UseVisualStyleBackColor = true;
            // 
            // tabPageSign
            // 
            resources.ApplyResources(this.tabPageSign, "tabPageSign");
            this.tabPageSign.BackColor = System.Drawing.Color.White;
            this.tabPageSign.Controls.Add(this.groupBox_SIGN_Options);
            this.tabPageSign.Name = "tabPageSign";
            // 
            // groupBox_SIGN_Options
            // 
            this.groupBox_SIGN_Options.Controls.Add(this.checkBox2);
            this.groupBox_SIGN_Options.Controls.Add(this.checkBox1);
            this.groupBox_SIGN_Options.Controls.Add(this.autoDelIdsigChkBox);
            this.groupBox_SIGN_Options.Controls.Add(this.schemev4ComboBox);
            this.groupBox_SIGN_Options.Controls.Add(this.label27);
            this.groupBox_SIGN_Options.Controls.Add(this.schemev3ComboBox);
            this.groupBox_SIGN_Options.Controls.Add(this.label26);
            this.groupBox_SIGN_Options.Controls.Add(this.schemev2ComboBox);
            this.groupBox_SIGN_Options.Controls.Add(this.label25);
            this.groupBox_SIGN_Options.Controls.Add(this.schemev1ComboBox);
            this.groupBox_SIGN_Options.Controls.Add(this.label24);
            this.groupBox_SIGN_Options.Controls.Add(this.textBox3);
            this.groupBox_SIGN_Options.Controls.Add(this.selectKeyStoreFileBtn);
            this.groupBox_SIGN_Options.Controls.Add(this.aliasTxtBox);
            this.groupBox_SIGN_Options.Controls.Add(this.useAliasChkBox);
            this.groupBox_SIGN_Options.Controls.Add(this.label22);
            this.groupBox_SIGN_Options.Controls.Add(this.keyStoreFileTxtBox);
            this.groupBox_SIGN_Options.Controls.Add(this.label21);
            this.groupBox_SIGN_Options.Controls.Add(this.label20);
            this.groupBox_SIGN_Options.Controls.Add(this.useKeyStoreChkBox);
            this.groupBox_SIGN_Options.Controls.Add(this.useSigningOutputDir);
            this.groupBox_SIGN_Options.Controls.Add(this.label_SIGN_PrivateKey);
            this.groupBox_SIGN_Options.Controls.Add(this.label_SIGN_PublicKey);
            this.groupBox_SIGN_Options.Controls.Add(this.button_SIGN_BrowseOutputFile);
            this.groupBox_SIGN_Options.Controls.Add(this.textBox_SIGN_OutputFile);
            this.groupBox_SIGN_Options.Controls.Add(this.button_SIGN_BrowsePublicKey);
            this.groupBox_SIGN_Options.Controls.Add(this.button_SIGN_BrowsePrivateKey);
            this.groupBox_SIGN_Options.Controls.Add(this.textBox_SIGN_PublicKey);
            this.groupBox_SIGN_Options.Controls.Add(this.textBox_SIGN_PrivateKey);
            resources.ApplyResources(this.groupBox_SIGN_Options, "groupBox_SIGN_Options");
            this.groupBox_SIGN_Options.Name = "groupBox_SIGN_Options";
            this.groupBox_SIGN_Options.TabStop = false;
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Checked = global::APKToolGUI.Properties.Settings.Default.Sign_InstallApkAfterSign;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Sign_InstallApkAfterSign", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Checked = global::APKToolGUI.Properties.Settings.Default.Sign_OverwriteInputFile;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Sign_OverwriteInputFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // autoDelIdsigChkBox
            // 
            resources.ApplyResources(this.autoDelIdsigChkBox, "autoDelIdsigChkBox");
            this.autoDelIdsigChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.AutoDeleteIdsigFile;
            this.autoDelIdsigChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autoDelIdsigChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "AutoDeleteIdsigFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.autoDelIdsigChkBox.Name = "autoDelIdsigChkBox";
            this.autoDelIdsigChkBox.UseVisualStyleBackColor = true;
            // 
            // schemev4ComboBox
            // 
            this.schemev4ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.schemev4ComboBox.FormattingEnabled = true;
            this.schemev4ComboBox.Items.AddRange(new object[] {
            resources.GetString("schemev4ComboBox.Items"),
            resources.GetString("schemev4ComboBox.Items1"),
            resources.GetString("schemev4ComboBox.Items2")});
            resources.ApplyResources(this.schemev4ComboBox, "schemev4ComboBox");
            this.schemev4ComboBox.Name = "schemev4ComboBox";
            // 
            // label27
            // 
            resources.ApplyResources(this.label27, "label27");
            this.label27.Name = "label27";
            // 
            // schemev3ComboBox
            // 
            this.schemev3ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.schemev3ComboBox.FormattingEnabled = true;
            this.schemev3ComboBox.Items.AddRange(new object[] {
            resources.GetString("schemev3ComboBox.Items"),
            resources.GetString("schemev3ComboBox.Items1"),
            resources.GetString("schemev3ComboBox.Items2")});
            resources.ApplyResources(this.schemev3ComboBox, "schemev3ComboBox");
            this.schemev3ComboBox.Name = "schemev3ComboBox";
            // 
            // label26
            // 
            resources.ApplyResources(this.label26, "label26");
            this.label26.Name = "label26";
            // 
            // schemev2ComboBox
            // 
            this.schemev2ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.schemev2ComboBox.FormattingEnabled = true;
            this.schemev2ComboBox.Items.AddRange(new object[] {
            resources.GetString("schemev2ComboBox.Items"),
            resources.GetString("schemev2ComboBox.Items1"),
            resources.GetString("schemev2ComboBox.Items2")});
            resources.ApplyResources(this.schemev2ComboBox, "schemev2ComboBox");
            this.schemev2ComboBox.Name = "schemev2ComboBox";
            // 
            // label25
            // 
            resources.ApplyResources(this.label25, "label25");
            this.label25.Name = "label25";
            // 
            // schemev1ComboBox
            // 
            this.schemev1ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.schemev1ComboBox.FormattingEnabled = true;
            this.schemev1ComboBox.Items.AddRange(new object[] {
            resources.GetString("schemev1ComboBox.Items"),
            resources.GetString("schemev1ComboBox.Items1"),
            resources.GetString("schemev1ComboBox.Items2")});
            resources.ApplyResources(this.schemev1ComboBox, "schemev1ComboBox");
            this.schemev1ComboBox.Name = "schemev1ComboBox";
            // 
            // label24
            // 
            resources.ApplyResources(this.label24, "label24");
            this.label24.Name = "label24";
            // 
            // textBox3
            // 
            resources.ApplyResources(this.textBox3, "textBox3");
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Sign_KeystorePassword", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox3.Name = "textBox3";
            this.textBox3.Text = global::APKToolGUI.Properties.Settings.Default.Sign_KeystorePassword;
            this.textBox3.UseSystemPasswordChar = true;
            // 
            // selectKeyStoreFileBtn
            // 
            resources.ApplyResources(this.selectKeyStoreFileBtn, "selectKeyStoreFileBtn");
            this.selectKeyStoreFileBtn.Name = "selectKeyStoreFileBtn";
            this.selectKeyStoreFileBtn.UseVisualStyleBackColor = true;
            // 
            // aliasTxtBox
            // 
            resources.ApplyResources(this.aliasTxtBox, "aliasTxtBox");
            this.aliasTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Sign_Alias", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.aliasTxtBox.Name = "aliasTxtBox";
            this.aliasTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.Sign_Alias;
            // 
            // useAliasChkBox
            // 
            resources.ApplyResources(this.useAliasChkBox, "useAliasChkBox");
            this.useAliasChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Sign_SetAlias;
            this.useAliasChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useAliasChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Sign_SetAlias", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.useAliasChkBox.Name = "useAliasChkBox";
            this.useAliasChkBox.UseVisualStyleBackColor = true;
            // 
            // label22
            // 
            this.label22.ForeColor = System.Drawing.Color.Maroon;
            resources.ApplyResources(this.label22, "label22");
            this.label22.Name = "label22";
            // 
            // keyStoreFileTxtBox
            // 
            resources.ApplyResources(this.keyStoreFileTxtBox, "keyStoreFileTxtBox");
            this.keyStoreFileTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Sign_KeystoreFilePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.keyStoreFileTxtBox.Name = "keyStoreFileTxtBox";
            this.keyStoreFileTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.Sign_KeystoreFilePath;
            // 
            // label21
            // 
            resources.ApplyResources(this.label21, "label21");
            this.label21.Name = "label21";
            // 
            // label20
            // 
            resources.ApplyResources(this.label20, "label20");
            this.label20.Name = "label20";
            // 
            // useKeyStoreChkBox
            // 
            resources.ApplyResources(this.useKeyStoreChkBox, "useKeyStoreChkBox");
            this.useKeyStoreChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Sign_UseKeystoreFile;
            this.useKeyStoreChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Sign_UseKeystoreFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.useKeyStoreChkBox.Name = "useKeyStoreChkBox";
            this.useKeyStoreChkBox.UseVisualStyleBackColor = true;
            // 
            // useSigningOutputDir
            // 
            resources.ApplyResources(this.useSigningOutputDir, "useSigningOutputDir");
            this.useSigningOutputDir.Checked = global::APKToolGUI.Properties.Settings.Default.Sign_UseOutputDir;
            this.useSigningOutputDir.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Sign_UseOutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.useSigningOutputDir.Name = "useSigningOutputDir";
            this.useSigningOutputDir.UseVisualStyleBackColor = true;
            // 
            // label_SIGN_PrivateKey
            // 
            resources.ApplyResources(this.label_SIGN_PrivateKey, "label_SIGN_PrivateKey");
            this.label_SIGN_PrivateKey.Name = "label_SIGN_PrivateKey";
            // 
            // label_SIGN_PublicKey
            // 
            resources.ApplyResources(this.label_SIGN_PublicKey, "label_SIGN_PublicKey");
            this.label_SIGN_PublicKey.Name = "label_SIGN_PublicKey";
            // 
            // button_SIGN_BrowseOutputFile
            // 
            resources.ApplyResources(this.button_SIGN_BrowseOutputFile, "button_SIGN_BrowseOutputFile");
            this.button_SIGN_BrowseOutputFile.Name = "button_SIGN_BrowseOutputFile";
            this.button_SIGN_BrowseOutputFile.UseVisualStyleBackColor = true;
            // 
            // textBox_SIGN_OutputFile
            // 
            resources.ApplyResources(this.textBox_SIGN_OutputFile, "textBox_SIGN_OutputFile");
            this.textBox_SIGN_OutputFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Sign_OutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_SIGN_OutputFile.Name = "textBox_SIGN_OutputFile";
            this.textBox_SIGN_OutputFile.Text = global::APKToolGUI.Properties.Settings.Default.Sign_OutputDir;
            // 
            // button_SIGN_BrowsePublicKey
            // 
            resources.ApplyResources(this.button_SIGN_BrowsePublicKey, "button_SIGN_BrowsePublicKey");
            this.button_SIGN_BrowsePublicKey.Name = "button_SIGN_BrowsePublicKey";
            this.button_SIGN_BrowsePublicKey.UseVisualStyleBackColor = true;
            // 
            // button_SIGN_BrowsePrivateKey
            // 
            resources.ApplyResources(this.button_SIGN_BrowsePrivateKey, "button_SIGN_BrowsePrivateKey");
            this.button_SIGN_BrowsePrivateKey.Name = "button_SIGN_BrowsePrivateKey";
            this.button_SIGN_BrowsePrivateKey.UseVisualStyleBackColor = true;
            // 
            // textBox_SIGN_PublicKey
            // 
            resources.ApplyResources(this.textBox_SIGN_PublicKey, "textBox_SIGN_PublicKey");
            this.textBox_SIGN_PublicKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Sign_PublicKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_SIGN_PublicKey.Name = "textBox_SIGN_PublicKey";
            this.textBox_SIGN_PublicKey.Text = global::APKToolGUI.Properties.Settings.Default.Sign_PublicKey;
            // 
            // textBox_SIGN_PrivateKey
            // 
            resources.ApplyResources(this.textBox_SIGN_PrivateKey, "textBox_SIGN_PrivateKey");
            this.textBox_SIGN_PrivateKey.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Sign_PrivateKey", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_SIGN_PrivateKey.Name = "textBox_SIGN_PrivateKey";
            this.textBox_SIGN_PrivateKey.Text = global::APKToolGUI.Properties.Settings.Default.Sign_PrivateKey;
            // 
            // tabPageZipAlign
            // 
            resources.ApplyResources(this.tabPageZipAlign, "tabPageZipAlign");
            this.tabPageZipAlign.BackColor = System.Drawing.Color.White;
            this.tabPageZipAlign.Controls.Add(this.groupBox_ZIPALIGN_Options);
            this.tabPageZipAlign.Name = "tabPageZipAlign";
            // 
            // groupBox_ZIPALIGN_Options
            // 
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.zipalignOutputDirChkBox);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.signAfterZipalignChkBox);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.checkBox_ZIPALIGN_Recompress);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.label_ZIPALIGN_AlignmentBytes);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.button_ZIPALIGN_BrowseOutputFile);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.checkBox_ZIPALIGN_CheckAlignment);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.textBox_ZIPALIGN_OutputFile);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.checkBox_ZIPALIGN_VerboseOutput);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.numericUpDown_ZIPALIGN_AlignmentBytes);
            this.groupBox_ZIPALIGN_Options.Controls.Add(this.checkBox_ZIPALIGN_OverwriteOutputFile);
            resources.ApplyResources(this.groupBox_ZIPALIGN_Options, "groupBox_ZIPALIGN_Options");
            this.groupBox_ZIPALIGN_Options.Name = "groupBox_ZIPALIGN_Options";
            this.groupBox_ZIPALIGN_Options.TabStop = false;
            // 
            // zipalignOutputDirChkBox
            // 
            resources.ApplyResources(this.zipalignOutputDirChkBox, "zipalignOutputDirChkBox");
            this.zipalignOutputDirChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Zipalign_UseOutputDir;
            this.zipalignOutputDirChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Zipalign_UseOutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.zipalignOutputDirChkBox.Name = "zipalignOutputDirChkBox";
            this.zipalignOutputDirChkBox.UseVisualStyleBackColor = true;
            // 
            // signAfterZipalignChkBox
            // 
            resources.ApplyResources(this.signAfterZipalignChkBox, "signAfterZipalignChkBox");
            this.signAfterZipalignChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Zipalign_SignAfterZipAlign;
            this.signAfterZipalignChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.signAfterZipalignChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Zipalign_SignAfterZipAlign", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.signAfterZipalignChkBox.Name = "signAfterZipalignChkBox";
            this.signAfterZipalignChkBox.UseVisualStyleBackColor = true;
            // 
            // checkBox_ZIPALIGN_Recompress
            // 
            resources.ApplyResources(this.checkBox_ZIPALIGN_Recompress, "checkBox_ZIPALIGN_Recompress");
            this.checkBox_ZIPALIGN_Recompress.Checked = global::APKToolGUI.Properties.Settings.Default.Zipalign_Recompress;
            this.checkBox_ZIPALIGN_Recompress.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Zipalign_Recompress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_ZIPALIGN_Recompress.Name = "checkBox_ZIPALIGN_Recompress";
            this.checkBox_ZIPALIGN_Recompress.UseVisualStyleBackColor = true;
            // 
            // label_ZIPALIGN_AlignmentBytes
            // 
            resources.ApplyResources(this.label_ZIPALIGN_AlignmentBytes, "label_ZIPALIGN_AlignmentBytes");
            this.label_ZIPALIGN_AlignmentBytes.Name = "label_ZIPALIGN_AlignmentBytes";
            // 
            // button_ZIPALIGN_BrowseOutputFile
            // 
            resources.ApplyResources(this.button_ZIPALIGN_BrowseOutputFile, "button_ZIPALIGN_BrowseOutputFile");
            this.button_ZIPALIGN_BrowseOutputFile.Name = "button_ZIPALIGN_BrowseOutputFile";
            this.button_ZIPALIGN_BrowseOutputFile.UseVisualStyleBackColor = true;
            // 
            // checkBox_ZIPALIGN_CheckAlignment
            // 
            resources.ApplyResources(this.checkBox_ZIPALIGN_CheckAlignment, "checkBox_ZIPALIGN_CheckAlignment");
            this.checkBox_ZIPALIGN_CheckAlignment.Checked = global::APKToolGUI.Properties.Settings.Default.Zipalign_CheckOnly;
            this.checkBox_ZIPALIGN_CheckAlignment.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Zipalign_CheckOnly", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_ZIPALIGN_CheckAlignment.Name = "checkBox_ZIPALIGN_CheckAlignment";
            this.checkBox_ZIPALIGN_CheckAlignment.UseVisualStyleBackColor = true;
            // 
            // textBox_ZIPALIGN_OutputFile
            // 
            resources.ApplyResources(this.textBox_ZIPALIGN_OutputFile, "textBox_ZIPALIGN_OutputFile");
            this.textBox_ZIPALIGN_OutputFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Zipalign_OutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_ZIPALIGN_OutputFile.Name = "textBox_ZIPALIGN_OutputFile";
            this.textBox_ZIPALIGN_OutputFile.Text = global::APKToolGUI.Properties.Settings.Default.Zipalign_OutputDir;
            // 
            // checkBox_ZIPALIGN_VerboseOutput
            // 
            resources.ApplyResources(this.checkBox_ZIPALIGN_VerboseOutput, "checkBox_ZIPALIGN_VerboseOutput");
            this.checkBox_ZIPALIGN_VerboseOutput.Checked = global::APKToolGUI.Properties.Settings.Default.Zipalign_Verbose;
            this.checkBox_ZIPALIGN_VerboseOutput.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Zipalign_Verbose", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_ZIPALIGN_VerboseOutput.Name = "checkBox_ZIPALIGN_VerboseOutput";
            this.checkBox_ZIPALIGN_VerboseOutput.UseVisualStyleBackColor = true;
            // 
            // numericUpDown_ZIPALIGN_AlignmentBytes
            // 
            this.numericUpDown_ZIPALIGN_AlignmentBytes.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::APKToolGUI.Properties.Settings.Default, "Zipalign_AlignmentInBytes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.numericUpDown_ZIPALIGN_AlignmentBytes, "numericUpDown_ZIPALIGN_AlignmentBytes");
            this.numericUpDown_ZIPALIGN_AlignmentBytes.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDown_ZIPALIGN_AlignmentBytes.Name = "numericUpDown_ZIPALIGN_AlignmentBytes";
            this.numericUpDown_ZIPALIGN_AlignmentBytes.Value = global::APKToolGUI.Properties.Settings.Default.Zipalign_AlignmentInBytes;
            // 
            // checkBox_ZIPALIGN_OverwriteOutputFile
            // 
            resources.ApplyResources(this.checkBox_ZIPALIGN_OverwriteOutputFile, "checkBox_ZIPALIGN_OverwriteOutputFile");
            this.checkBox_ZIPALIGN_OverwriteOutputFile.Checked = global::APKToolGUI.Properties.Settings.Default.Zipalign_OverwriteOutputFile;
            this.checkBox_ZIPALIGN_OverwriteOutputFile.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Zipalign_OverwriteOutputFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_ZIPALIGN_OverwriteOutputFile.Name = "checkBox_ZIPALIGN_OverwriteOutputFile";
            this.checkBox_ZIPALIGN_OverwriteOutputFile.UseVisualStyleBackColor = true;
            // 
            // tabPageInstallFramework
            // 
            this.tabPageInstallFramework.BackColor = System.Drawing.Color.White;
            this.tabPageInstallFramework.Controls.Add(this.groupBox1);
            this.tabPageInstallFramework.Controls.Add(this.groupBox_IF_Options);
            resources.ApplyResources(this.tabPageInstallFramework, "tabPageInstallFramework");
            this.tabPageInstallFramework.Name = "tabPageInstallFramework";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.openFwFolderBtn);
            this.groupBox1.Controls.Add(this.clearFwBtn);
            this.groupBox1.Controls.Add(this.clearFwBeforeDecodeChkBox);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // openFwFolderBtn
            // 
            resources.ApplyResources(this.openFwFolderBtn, "openFwFolderBtn");
            this.openFwFolderBtn.Name = "openFwFolderBtn";
            this.openFwFolderBtn.UseVisualStyleBackColor = true;
            // 
            // clearFwBtn
            // 
            resources.ApplyResources(this.clearFwBtn, "clearFwBtn");
            this.clearFwBtn.Name = "clearFwBtn";
            this.clearFwBtn.UseVisualStyleBackColor = true;
            // 
            // clearFwBeforeDecodeChkBox
            // 
            resources.ApplyResources(this.clearFwBeforeDecodeChkBox, "clearFwBeforeDecodeChkBox");
            this.clearFwBeforeDecodeChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Framework_ClearBeforeDecode;
            this.clearFwBeforeDecodeChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.clearFwBeforeDecodeChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Framework_ClearBeforeDecode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.clearFwBeforeDecodeChkBox.Name = "clearFwBeforeDecodeChkBox";
            this.clearFwBeforeDecodeChkBox.UseVisualStyleBackColor = true;
            // 
            // groupBox_IF_Options
            // 
            this.groupBox_IF_Options.Controls.Add(this.checkBox_IF_Tag);
            this.groupBox_IF_Options.Controls.Add(this.checkBox_IF_FramePath);
            this.groupBox_IF_Options.Controls.Add(this.textBox_IF_Tag);
            this.groupBox_IF_Options.Controls.Add(this.button_IF_InstallFramework);
            this.groupBox_IF_Options.Controls.Add(this.button_IF_BrowseFrameDir);
            this.groupBox_IF_Options.Controls.Add(this.button_IF_BrowseInputFramePath);
            this.groupBox_IF_Options.Controls.Add(this.textBox_IF_InputFramePath);
            this.groupBox_IF_Options.Controls.Add(this.textBox_IF_FrameDir);
            resources.ApplyResources(this.groupBox_IF_Options, "groupBox_IF_Options");
            this.groupBox_IF_Options.Name = "groupBox_IF_Options";
            this.groupBox_IF_Options.TabStop = false;
            // 
            // checkBox_IF_Tag
            // 
            resources.ApplyResources(this.checkBox_IF_Tag, "checkBox_IF_Tag");
            this.checkBox_IF_Tag.Checked = global::APKToolGUI.Properties.Settings.Default.InstallFramework_UseTag;
            this.checkBox_IF_Tag.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "InstallFramework_UseTag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_IF_Tag.Name = "checkBox_IF_Tag";
            this.checkBox_IF_Tag.UseVisualStyleBackColor = true;
            // 
            // checkBox_IF_FramePath
            // 
            resources.ApplyResources(this.checkBox_IF_FramePath, "checkBox_IF_FramePath");
            this.checkBox_IF_FramePath.Checked = global::APKToolGUI.Properties.Settings.Default.Framework_UseFrameDir;
            this.checkBox_IF_FramePath.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Framework_UseFrameDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox_IF_FramePath.Name = "checkBox_IF_FramePath";
            this.checkBox_IF_FramePath.UseVisualStyleBackColor = true;
            // 
            // textBox_IF_Tag
            // 
            resources.ApplyResources(this.textBox_IF_Tag, "textBox_IF_Tag");
            this.textBox_IF_Tag.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "InstallFramework_Tag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_IF_Tag.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "InstallFramework_UseTag", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_IF_Tag.Enabled = global::APKToolGUI.Properties.Settings.Default.InstallFramework_UseTag;
            this.textBox_IF_Tag.Name = "textBox_IF_Tag";
            this.textBox_IF_Tag.Text = global::APKToolGUI.Properties.Settings.Default.InstallFramework_Tag;
            // 
            // button_IF_InstallFramework
            // 
            resources.ApplyResources(this.button_IF_InstallFramework, "button_IF_InstallFramework");
            this.button_IF_InstallFramework.Name = "button_IF_InstallFramework";
            this.button_IF_InstallFramework.UseVisualStyleBackColor = true;
            // 
            // button_IF_BrowseFrameDir
            // 
            resources.ApplyResources(this.button_IF_BrowseFrameDir, "button_IF_BrowseFrameDir");
            this.button_IF_BrowseFrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Framework_UseFrameDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.button_IF_BrowseFrameDir.Enabled = global::APKToolGUI.Properties.Settings.Default.Framework_UseFrameDir;
            this.button_IF_BrowseFrameDir.Name = "button_IF_BrowseFrameDir";
            this.button_IF_BrowseFrameDir.UseVisualStyleBackColor = true;
            // 
            // button_IF_BrowseInputFramePath
            // 
            resources.ApplyResources(this.button_IF_BrowseInputFramePath, "button_IF_BrowseInputFramePath");
            this.button_IF_BrowseInputFramePath.Name = "button_IF_BrowseInputFramePath";
            this.button_IF_BrowseInputFramePath.UseVisualStyleBackColor = true;
            // 
            // textBox_IF_InputFramePath
            // 
            resources.ApplyResources(this.textBox_IF_InputFramePath, "textBox_IF_InputFramePath");
            this.textBox_IF_InputFramePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "InstallFramework_InputFramePath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_IF_InputFramePath.Name = "textBox_IF_InputFramePath";
            this.textBox_IF_InputFramePath.Text = global::APKToolGUI.Properties.Settings.Default.InstallFramework_InputFramePath;
            // 
            // textBox_IF_FrameDir
            // 
            resources.ApplyResources(this.textBox_IF_FrameDir, "textBox_IF_FrameDir");
            this.textBox_IF_FrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::APKToolGUI.Properties.Settings.Default, "Framework_UseFrameDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_IF_FrameDir.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Framework_FrameDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBox_IF_FrameDir.Enabled = global::APKToolGUI.Properties.Settings.Default.Framework_UseFrameDir;
            this.textBox_IF_FrameDir.Name = "textBox_IF_FrameDir";
            this.textBox_IF_FrameDir.Text = global::APKToolGUI.Properties.Settings.Default.Framework_FrameDir;
            // 
            // tabPageBaksmali
            // 
            this.tabPageBaksmali.AllowDrop = true;
            this.tabPageBaksmali.BackColor = System.Drawing.Color.White;
            this.tabPageBaksmali.Controls.Add(this.smaliGroupBox);
            this.tabPageBaksmali.Controls.Add(this.bakSmaliGroupBox);
            resources.ApplyResources(this.tabPageBaksmali, "tabPageBaksmali");
            this.tabPageBaksmali.Name = "tabPageBaksmali";
            // 
            // smaliGroupBox
            // 
            this.smaliGroupBox.Controls.Add(this.label29);
            this.smaliGroupBox.Controls.Add(this.smaliUseOutputChkBox);
            this.smaliGroupBox.Controls.Add(this.comSmaliBtn);
            this.smaliGroupBox.Controls.Add(this.smaliBrowseOutputBtn);
            this.smaliGroupBox.Controls.Add(this.smaliBrowseInputDirTxtBox);
            this.smaliGroupBox.Controls.Add(this.smaliBrowseOutputTxtBox);
            this.smaliGroupBox.Controls.Add(this.smaliBrowseInputDirBtn);
            resources.ApplyResources(this.smaliGroupBox, "smaliGroupBox");
            this.smaliGroupBox.Name = "smaliGroupBox";
            this.smaliGroupBox.TabStop = false;
            // 
            // label29
            // 
            resources.ApplyResources(this.label29, "label29");
            this.label29.Name = "label29";
            // 
            // smaliUseOutputChkBox
            // 
            resources.ApplyResources(this.smaliUseOutputChkBox, "smaliUseOutputChkBox");
            this.smaliUseOutputChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Smali_UseOutputDir;
            this.smaliUseOutputChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Smali_UseOutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.smaliUseOutputChkBox.Name = "smaliUseOutputChkBox";
            this.smaliUseOutputChkBox.UseVisualStyleBackColor = true;
            // 
            // comSmaliBtn
            // 
            resources.ApplyResources(this.comSmaliBtn, "comSmaliBtn");
            this.comSmaliBtn.Name = "comSmaliBtn";
            this.comSmaliBtn.UseVisualStyleBackColor = true;
            // 
            // smaliBrowseOutputBtn
            // 
            resources.ApplyResources(this.smaliBrowseOutputBtn, "smaliBrowseOutputBtn");
            this.smaliBrowseOutputBtn.Name = "smaliBrowseOutputBtn";
            this.smaliBrowseOutputBtn.UseVisualStyleBackColor = true;
            // 
            // smaliBrowseInputDirTxtBox
            // 
            resources.ApplyResources(this.smaliBrowseInputDirTxtBox, "smaliBrowseInputDirTxtBox");
            this.smaliBrowseInputDirTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Smali_InputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.smaliBrowseInputDirTxtBox.Name = "smaliBrowseInputDirTxtBox";
            this.smaliBrowseInputDirTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.Smali_InputDir;
            // 
            // smaliBrowseOutputTxtBox
            // 
            resources.ApplyResources(this.smaliBrowseOutputTxtBox, "smaliBrowseOutputTxtBox");
            this.smaliBrowseOutputTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Smali_OutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.smaliBrowseOutputTxtBox.Name = "smaliBrowseOutputTxtBox";
            this.smaliBrowseOutputTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.Smali_OutputDir;
            // 
            // smaliBrowseInputDirBtn
            // 
            resources.ApplyResources(this.smaliBrowseInputDirBtn, "smaliBrowseInputDirBtn");
            this.smaliBrowseInputDirBtn.Name = "smaliBrowseInputDirBtn";
            this.smaliBrowseInputDirBtn.UseVisualStyleBackColor = true;
            // 
            // bakSmaliGroupBox
            // 
            this.bakSmaliGroupBox.Controls.Add(this.label28);
            this.bakSmaliGroupBox.Controls.Add(this.baksmaliUseOutputChkBox);
            this.bakSmaliGroupBox.Controls.Add(this.baksmaliBrowseOutputBtn);
            this.bakSmaliGroupBox.Controls.Add(this.baksmaliBrowseOutputTxtBox);
            this.bakSmaliGroupBox.Controls.Add(this.decSmaliBtn);
            this.bakSmaliGroupBox.Controls.Add(this.baksmaliBrowseInputDexBtn);
            this.bakSmaliGroupBox.Controls.Add(this.baksmaliBrowseInputDexTxtBox);
            resources.ApplyResources(this.bakSmaliGroupBox, "bakSmaliGroupBox");
            this.bakSmaliGroupBox.Name = "bakSmaliGroupBox";
            this.bakSmaliGroupBox.TabStop = false;
            // 
            // label28
            // 
            resources.ApplyResources(this.label28, "label28");
            this.label28.Name = "label28";
            // 
            // baksmaliUseOutputChkBox
            // 
            resources.ApplyResources(this.baksmaliUseOutputChkBox, "baksmaliUseOutputChkBox");
            this.baksmaliUseOutputChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Baksmali_UseOutputDir;
            this.baksmaliUseOutputChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Baksmali_UseOutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.baksmaliUseOutputChkBox.Name = "baksmaliUseOutputChkBox";
            this.baksmaliUseOutputChkBox.UseVisualStyleBackColor = true;
            // 
            // baksmaliBrowseOutputBtn
            // 
            resources.ApplyResources(this.baksmaliBrowseOutputBtn, "baksmaliBrowseOutputBtn");
            this.baksmaliBrowseOutputBtn.Name = "baksmaliBrowseOutputBtn";
            this.baksmaliBrowseOutputBtn.UseVisualStyleBackColor = true;
            // 
            // baksmaliBrowseOutputTxtBox
            // 
            resources.ApplyResources(this.baksmaliBrowseOutputTxtBox, "baksmaliBrowseOutputTxtBox");
            this.baksmaliBrowseOutputTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Baksmali_OutputDir", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.baksmaliBrowseOutputTxtBox.Name = "baksmaliBrowseOutputTxtBox";
            this.baksmaliBrowseOutputTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.Baksmali_OutputDir;
            // 
            // decSmaliBtn
            // 
            resources.ApplyResources(this.decSmaliBtn, "decSmaliBtn");
            this.decSmaliBtn.Name = "decSmaliBtn";
            this.decSmaliBtn.UseVisualStyleBackColor = true;
            // 
            // baksmaliBrowseInputDexBtn
            // 
            resources.ApplyResources(this.baksmaliBrowseInputDexBtn, "baksmaliBrowseInputDexBtn");
            this.baksmaliBrowseInputDexBtn.Name = "baksmaliBrowseInputDexBtn";
            this.baksmaliBrowseInputDexBtn.UseVisualStyleBackColor = true;
            // 
            // baksmaliBrowseInputDexTxtBox
            // 
            resources.ApplyResources(this.baksmaliBrowseInputDexTxtBox, "baksmaliBrowseInputDexTxtBox");
            this.baksmaliBrowseInputDexTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Baksmali_InputDexFile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.baksmaliBrowseInputDexTxtBox.Name = "baksmaliBrowseInputDexTxtBox";
            this.baksmaliBrowseInputDexTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.Baksmali_InputDexFile;
            // 
            // tabPageAdb
            // 
            this.tabPageAdb.AllowDrop = true;
            this.tabPageAdb.Controls.Add(this.overrideAbiComboBox);
            this.tabPageAdb.Controls.Add(this.selAdbDeviceLbl);
            this.tabPageAdb.Controls.Add(this.label33);
            this.tabPageAdb.Controls.Add(this.killAdbBtn);
            this.tabPageAdb.Controls.Add(this.installApkBtn);
            this.tabPageAdb.Controls.Add(this.refreshDevicesBtn);
            this.tabPageAdb.Controls.Add(this.selApkAdbBtn);
            this.tabPageAdb.Controls.Add(this.overrideAbiCheckBox);
            this.tabPageAdb.Controls.Add(this.setVendorChkBox);
            this.tabPageAdb.Controls.Add(this.apkPathAdbTxtBox);
            this.tabPageAdb.Controls.Add(this.label32);
            this.tabPageAdb.Controls.Add(this.devicesListBox);
            resources.ApplyResources(this.tabPageAdb, "tabPageAdb");
            this.tabPageAdb.Name = "tabPageAdb";
            this.tabPageAdb.UseVisualStyleBackColor = true;
            // 
            // overrideAbiComboBox
            // 
            this.overrideAbiComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.overrideAbiComboBox.FormattingEnabled = true;
            this.overrideAbiComboBox.Items.AddRange(new object[] {
            resources.GetString("overrideAbiComboBox.Items"),
            resources.GetString("overrideAbiComboBox.Items1"),
            resources.GetString("overrideAbiComboBox.Items2"),
            resources.GetString("overrideAbiComboBox.Items3")});
            resources.ApplyResources(this.overrideAbiComboBox, "overrideAbiComboBox");
            this.overrideAbiComboBox.Name = "overrideAbiComboBox";
            // 
            // selAdbDeviceLbl
            // 
            this.selAdbDeviceLbl.ForeColor = System.Drawing.Color.ForestGreen;
            resources.ApplyResources(this.selAdbDeviceLbl, "selAdbDeviceLbl");
            this.selAdbDeviceLbl.Name = "selAdbDeviceLbl";
            // 
            // label33
            // 
            resources.ApplyResources(this.label33, "label33");
            this.label33.Name = "label33";
            // 
            // killAdbBtn
            // 
            resources.ApplyResources(this.killAdbBtn, "killAdbBtn");
            this.killAdbBtn.Name = "killAdbBtn";
            this.killAdbBtn.UseVisualStyleBackColor = true;
            // 
            // installApkBtn
            // 
            resources.ApplyResources(this.installApkBtn, "installApkBtn");
            this.installApkBtn.Name = "installApkBtn";
            this.installApkBtn.UseVisualStyleBackColor = true;
            // 
            // refreshDevicesBtn
            // 
            resources.ApplyResources(this.refreshDevicesBtn, "refreshDevicesBtn");
            this.refreshDevicesBtn.Name = "refreshDevicesBtn";
            this.refreshDevicesBtn.UseVisualStyleBackColor = true;
            // 
            // selApkAdbBtn
            // 
            resources.ApplyResources(this.selApkAdbBtn, "selApkAdbBtn");
            this.selApkAdbBtn.Name = "selApkAdbBtn";
            this.selApkAdbBtn.UseVisualStyleBackColor = true;
            // 
            // overrideAbiCheckBox
            // 
            resources.ApplyResources(this.overrideAbiCheckBox, "overrideAbiCheckBox");
            this.overrideAbiCheckBox.Checked = global::APKToolGUI.Properties.Settings.Default.Adb_SetOverrideAbi;
            this.overrideAbiCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Adb_SetOverrideAbi", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.overrideAbiCheckBox.Name = "overrideAbiCheckBox";
            this.overrideAbiCheckBox.UseVisualStyleBackColor = true;
            // 
            // setVendorChkBox
            // 
            resources.ApplyResources(this.setVendorChkBox, "setVendorChkBox");
            this.setVendorChkBox.Checked = global::APKToolGUI.Properties.Settings.Default.Adb_SetVendor;
            this.setVendorChkBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::APKToolGUI.Properties.Settings.Default, "Adb_SetVendor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.setVendorChkBox.Name = "setVendorChkBox";
            this.setVendorChkBox.UseVisualStyleBackColor = true;
            // 
            // apkPathAdbTxtBox
            // 
            resources.ApplyResources(this.apkPathAdbTxtBox, "apkPathAdbTxtBox");
            this.apkPathAdbTxtBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::APKToolGUI.Properties.Settings.Default, "Adb_SelectedApkPath", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.apkPathAdbTxtBox.Name = "apkPathAdbTxtBox";
            this.apkPathAdbTxtBox.Text = global::APKToolGUI.Properties.Settings.Default.Adb_SelectedApkPath;
            // 
            // label32
            // 
            resources.ApplyResources(this.label32, "label32");
            this.label32.Name = "label32";
            // 
            // devicesListBox
            // 
            resources.ApplyResources(this.devicesListBox, "devicesListBox");
            this.devicesListBox.FormattingEnabled = true;
            this.devicesListBox.Name = "devicesListBox";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelStateImage,
            this.toolStripStatusLabelStateText,
            this.progressBar});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.SizingGrip = false;
            // 
            // toolStripStatusLabelStateImage
            // 
            this.toolStripStatusLabelStateImage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripStatusLabelStateImage.Name = "toolStripStatusLabelStateImage";
            resources.ApplyResources(this.toolStripStatusLabelStateImage, "toolStripStatusLabelStateImage");
            // 
            // toolStripStatusLabelStateText
            // 
            this.toolStripStatusLabelStateText.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.toolStripStatusLabelStateText, "toolStripStatusLabelStateText");
            this.toolStripStatusLabelStateText.Name = "toolStripStatusLabelStateText";
            this.toolStripStatusLabelStateText.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripStatusLabelStateText.Spring = true;
            this.toolStripStatusLabelStateText.Click += new System.EventHandler(this.toolStripStatusLabelStateText_Click);
            // 
            // progressBar
            // 
            this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.MarqueeAnimationSpeed = 30;
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.Click += new System.EventHandler(this.toolStripProgressBar1_Click);
            // 
            // contextMenuStripLog
            // 
            this.contextMenuStripLog.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.clearLogToolStripMenuItem});
            this.contextMenuStripLog.Name = "contextMenuStripLog";
            resources.ApplyResources(this.contextMenuStripLog, "contextMenuStripLog");
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            resources.ApplyResources(this.copyToolStripMenuItem, "copyToolStripMenuItem");
            // 
            // clearLogToolStripMenuItem
            // 
            this.clearLogToolStripMenuItem.Name = "clearLogToolStripMenuItem";
            resources.ApplyResources(this.clearLogToolStripMenuItem, "clearLogToolStripMenuItem");
            // 
            // logTxtBox
            // 
            this.logTxtBox.ContextMenuStrip = this.contextMenuStripLog;
            resources.ApplyResources(this.logTxtBox, "logTxtBox");
            this.logTxtBox.HideSelection = false;
            this.logTxtBox.Name = "logTxtBox";
            this.logTxtBox.ReadOnly = true;
            this.logTxtBox.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem1,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newInsToolStripMenuItem,
            this.saveLogToFileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.openTempFolderToolStripMenuItem,
            this.clearTempFolderToolStripMenuItem,
            this.toolStripMenuItem2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // newInsToolStripMenuItem
            // 
            this.newInsToolStripMenuItem.Name = "newInsToolStripMenuItem";
            resources.ApplyResources(this.newInsToolStripMenuItem, "newInsToolStripMenuItem");
            // 
            // saveLogToFileToolStripMenuItem
            // 
            this.saveLogToFileToolStripMenuItem.Name = "saveLogToFileToolStripMenuItem";
            resources.ApplyResources(this.saveLogToFileToolStripMenuItem, "saveLogToFileToolStripMenuItem");
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // openTempFolderToolStripMenuItem
            // 
            this.openTempFolderToolStripMenuItem.Name = "openTempFolderToolStripMenuItem";
            resources.ApplyResources(this.openTempFolderToolStripMenuItem, "openTempFolderToolStripMenuItem");
            // 
            // clearTempFolderToolStripMenuItem
            // 
            this.clearTempFolderToolStripMenuItem.Name = "clearTempFolderToolStripMenuItem";
            resources.ApplyResources(this.clearTempFolderToolStripMenuItem, "clearTempFolderToolStripMenuItem");
            this.clearTempFolderToolStripMenuItem.Click += new System.EventHandler(this.clearTempFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            resources.ApplyResources(this.toolStripMenuItem2, "toolStripMenuItem2");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            // 
            // settingsToolStripMenuItem1
            // 
            this.settingsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.useAPKEditorForDecompilingItem,
            this.settingsToolStripMenuItem});
            this.settingsToolStripMenuItem1.Name = "settingsToolStripMenuItem1";
            resources.ApplyResources(this.settingsToolStripMenuItem1, "settingsToolStripMenuItem1");
            // 
            // useAPKEditorForDecompilingItem
            // 
            this.useAPKEditorForDecompilingItem.Checked = global::APKToolGUI.Properties.Settings.Default.UseApkeditor;
            this.useAPKEditorForDecompilingItem.CheckOnClick = true;
            this.useAPKEditorForDecompilingItem.Name = "useAPKEditorForDecompilingItem";
            resources.ApplyResources(this.useAPKEditorForDecompilingItem, "useAPKEditorForDecompilingItem");
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            resources.ApplyResources(this.settingsToolStripMenuItem, "settingsToolStripMenuItem");
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkForUpdateToolStripMenuItem,
            this.reportAnIsuueToolStripMenuItem,
            this.apktoolIssuesToolStripMenuItem,
            this.baksmaliIssuesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            resources.ApplyResources(this.checkForUpdateToolStripMenuItem, "checkForUpdateToolStripMenuItem");
            // 
            // reportAnIsuueToolStripMenuItem
            // 
            this.reportAnIsuueToolStripMenuItem.Name = "reportAnIsuueToolStripMenuItem";
            resources.ApplyResources(this.reportAnIsuueToolStripMenuItem, "reportAnIsuueToolStripMenuItem");
            // 
            // apktoolIssuesToolStripMenuItem
            // 
            this.apktoolIssuesToolStripMenuItem.Name = "apktoolIssuesToolStripMenuItem";
            resources.ApplyResources(this.apktoolIssuesToolStripMenuItem, "apktoolIssuesToolStripMenuItem");
            // 
            // baksmaliIssuesToolStripMenuItem
            // 
            this.baksmaliIssuesToolStripMenuItem.Name = "baksmaliIssuesToolStripMenuItem";
            resources.ApplyResources(this.baksmaliIssuesToolStripMenuItem, "baksmaliIssuesToolStripMenuItem");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.logTxtBox);
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Name = "FormMain";
            this.Activated += new System.EventHandler(this.FormMain_Activated);
            this.Shown += new System.EventHandler(this.FormMain_Shown);
            this.tabControlMain.ResumeLayout(false);
            this.tabPageMain.ResumeLayout(false);
            this.mergePanel.ResumeLayout(false);
            this.mergePanel.PerformLayout();
            this.signPanel.ResumeLayout(false);
            this.signPanel.PerformLayout();
            this.zipalignPanel.ResumeLayout(false);
            this.zipalignPanel.PerformLayout();
            this.comPanel.ResumeLayout(false);
            this.comPanel.PerformLayout();
            this.decPanel.ResumeLayout(false);
            this.decPanel.PerformLayout();
            this.tabPageApkInfo.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.basicInfoTabPage.ResumeLayout(false);
            this.basicInfoTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.apkIconPicBox)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPageDecode.ResumeLayout(false);
            this.groupBox_DECODE_Options.ResumeLayout(false);
            this.groupBox_DECODE_Options.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.decJobsLvlUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.decApiLvlUpDown)).EndInit();
            this.tabPageBuild.ResumeLayout(false);
            this.groupBox_BUILD_Options.ResumeLayout(false);
            this.groupBox_BUILD_Options.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comJobsLvlUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buildApiLvlUpDown)).EndInit();
            this.tabPageSign.ResumeLayout(false);
            this.groupBox_SIGN_Options.ResumeLayout(false);
            this.groupBox_SIGN_Options.PerformLayout();
            this.tabPageZipAlign.ResumeLayout(false);
            this.groupBox_ZIPALIGN_Options.ResumeLayout(false);
            this.groupBox_ZIPALIGN_Options.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_ZIPALIGN_AlignmentBytes)).EndInit();
            this.tabPageInstallFramework.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_IF_Options.ResumeLayout(false);
            this.groupBox_IF_Options.PerformLayout();
            this.tabPageBaksmali.ResumeLayout(false);
            this.smaliGroupBox.ResumeLayout(false);
            this.smaliGroupBox.PerformLayout();
            this.bakSmaliGroupBox.ResumeLayout(false);
            this.bakSmaliGroupBox.PerformLayout();
            this.tabPageAdb.ResumeLayout(false);
            this.tabPageAdb.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStripLog.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TabControl tabControlMain;
        internal System.Windows.Forms.TabPage tabPageDecode;
        internal System.Windows.Forms.TabPage tabPageSign;
        internal System.Windows.Forms.TabPage tabPageZipAlign;
        internal System.Windows.Forms.StatusStrip statusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStateText;
        internal System.Windows.Forms.ToolStripProgressBar progressBar;
        internal System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelStateImage;
        internal System.Windows.Forms.ContextMenuStrip contextMenuStripLog;
        internal System.Windows.Forms.ToolStripMenuItem clearLogToolStripMenuItem;
        internal System.Windows.Forms.TabPage tabPageBuild;
        internal System.Windows.Forms.TabPage tabPageInstallFramework;
        internal System.Windows.Forms.TextBox textBox_IF_InputFramePath;
        internal System.Windows.Forms.Button button_IF_BrowseInputFramePath;
        internal System.Windows.Forms.Button button_IF_InstallFramework;
        internal System.Windows.Forms.GroupBox groupBox_BUILD_Options;
        internal System.Windows.Forms.Button button_BUILD_BrowseOutputAppPath;
        internal System.Windows.Forms.CheckBox checkBox_BUILD_OutputAppPath;
        internal System.Windows.Forms.TextBox textBox_BUILD_OutputAppPath;
        internal System.Windows.Forms.CheckBox checkBox_BUILD_CopyOriginal;
        internal System.Windows.Forms.Button button_BUILD_BrowseFrameDir;
        internal System.Windows.Forms.CheckBox checkBox_BUILD_UseFramework;
        internal System.Windows.Forms.TextBox textBox_BUILD_FrameDir;
        internal System.Windows.Forms.TextBox textBox_BUILD_AaptPath;
        internal System.Windows.Forms.CheckBox checkBox_BUILD_UseAapt;
        internal System.Windows.Forms.Button button_BUILD_BrowseAaptPath;
        internal System.Windows.Forms.CheckBox checkBox_BUILD_ForceAll;
        internal System.Windows.Forms.GroupBox groupBox_IF_Options;
        internal System.Windows.Forms.CheckBox checkBox_IF_Tag;
        internal System.Windows.Forms.TextBox textBox_IF_Tag;
        internal System.Windows.Forms.TextBox textBox_IF_FrameDir;
        internal System.Windows.Forms.CheckBox checkBox_IF_FramePath;
        internal System.Windows.Forms.Button button_IF_BrowseFrameDir;
        internal System.Windows.Forms.GroupBox groupBox_ZIPALIGN_Options;
        internal System.Windows.Forms.Label label_ZIPALIGN_AlignmentBytes;
        internal System.Windows.Forms.NumericUpDown numericUpDown_ZIPALIGN_AlignmentBytes;
        internal System.Windows.Forms.CheckBox checkBox_ZIPALIGN_VerboseOutput;
        internal System.Windows.Forms.CheckBox checkBox_ZIPALIGN_CheckAlignment;
        internal System.Windows.Forms.Button button_ZIPALIGN_BrowseOutputFile;
        internal System.Windows.Forms.TextBox textBox_ZIPALIGN_OutputFile;
        internal System.Windows.Forms.CheckBox checkBox_ZIPALIGN_OverwriteOutputFile;
        internal System.Windows.Forms.CheckBox checkBox_ZIPALIGN_Recompress;
        internal System.Windows.Forms.GroupBox groupBox_SIGN_Options;
        internal System.Windows.Forms.Label label_SIGN_PrivateKey;
        internal System.Windows.Forms.Button button_SIGN_BrowsePrivateKey;
        internal System.Windows.Forms.TextBox textBox_SIGN_PrivateKey;
        internal System.Windows.Forms.Label label_SIGN_PublicKey;
        internal System.Windows.Forms.Button button_SIGN_BrowsePublicKey;
        internal System.Windows.Forms.TextBox textBox_SIGN_PublicKey;
        internal System.Windows.Forms.Button button_SIGN_BrowseOutputFile;
        internal System.Windows.Forms.TextBox textBox_SIGN_OutputFile;
        internal System.Windows.Forms.TabPage tabPageMain;
        internal System.Windows.Forms.GroupBox groupBox_DECODE_Options;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_MatchOriginal;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_KeepBrokenRes;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_Force;
        internal System.Windows.Forms.Panel signPanel;
        internal System.Windows.Forms.TextBox textBox_SIGN_InputFile;
        internal System.Windows.Forms.Button button_SIGN_BrowseInputFile;
        internal System.Windows.Forms.Button button_SIGN_Sign;
        internal System.Windows.Forms.Panel zipalignPanel;
        internal System.Windows.Forms.Button button_ZIPALIGN_Align;
        internal System.Windows.Forms.Button button_ZIPALIGN_BrowseInputFile;
        internal System.Windows.Forms.TextBox textBox_ZIPALIGN_InputFile;
        internal System.Windows.Forms.Panel comPanel;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Button button_BUILD_BrowseInputProjectDir;
        internal System.Windows.Forms.Button button_BUILD_Build;
        internal System.Windows.Forms.TextBox textBox_BUILD_InputProjectDir;
        internal System.Windows.Forms.Panel decPanel;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox textBox_DECODE_InputAppPath;
        internal System.Windows.Forms.Button button_DECODE_Decode;
        internal System.Windows.Forms.Button button_DECODE_BrowseInputAppPath;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox textBox_DECODE_FrameDir;
        internal System.Windows.Forms.Button button_DECODE_BrowseOutputDirectory;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_UseFramework;
        internal System.Windows.Forms.Button button_DECODE_BrowseFrameDir;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_OutputDirectory;
        internal System.Windows.Forms.TextBox textBox_DECODE_OutputDirectory;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_NoSrc;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_NoRes;
        internal System.Windows.Forms.Button button_OpenMainActivity;
        internal System.Windows.Forms.Button openAndroidMainfestBtn;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_OnlyMainClasses;
        internal System.Windows.Forms.CheckBox checkBox_BUILD_NoCrunch;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_FixError;
        internal System.Windows.Forms.TabPage tabPageApkInfo;
        internal System.Windows.Forms.RichTextBox permTxtBox;
        internal System.Windows.Forms.TextBox buildTxtBox;
        internal System.Windows.Forms.PictureBox apkIconPicBox;
        internal System.Windows.Forms.Label label10;
        internal System.Windows.Forms.Label label12;
        internal System.Windows.Forms.Label label15;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.Label label13;
        internal System.Windows.Forms.Label label11;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label9;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.TextBox densityTxtBox;
        internal System.Windows.Forms.TextBox screenTxtBox;
        internal System.Windows.Forms.TextBox minSdkTxtBox;
        internal System.Windows.Forms.TextBox verTxtBox;
        internal System.Windows.Forms.TextBox packNameTxtBox;
        internal System.Windows.Forms.TextBox appTxtBox;
        internal System.Windows.Forms.CheckBox checkBox_DECODE_NoDebugInfo;
        internal System.Windows.Forms.RichTextBox localsTxtBox;
        internal System.Windows.Forms.Label label18;
        internal System.Windows.Forms.Label label17;
        internal System.Windows.Forms.TextBox targetSdkTxtBox;
        internal System.Windows.Forms.TextBox fileTxtBox;
        internal System.Windows.Forms.Button comApkOpenDir;
        internal System.Windows.Forms.Button decApkOpenDirBtn;
        internal System.Windows.Forms.Button compileOutputOpenDirBtn;
        internal System.Windows.Forms.Button apkComboLinkBtn;
        internal System.Windows.Forms.Button apkPureLinkBtn;
        internal System.Windows.Forms.Button psLinkBtn;
        internal System.Windows.Forms.Label label19;
        internal System.Windows.Forms.CheckBox zipalignAfterBuildChkBox;
        internal System.Windows.Forms.CheckBox clearFwBeforeDecodeChkBox;
        internal System.Windows.Forms.Button clearFwBtn;
        internal System.Windows.Forms.GroupBox groupBox1;
        internal System.Windows.Forms.CheckBox signAfterBuildChkBox;
        internal System.Windows.Forms.CheckBox signAfterZipalignChkBox;
        internal System.Windows.Forms.CheckBox zipalignOutputDirChkBox;
        internal System.Windows.Forms.CheckBox useSigningOutputDir;
        internal System.Windows.Forms.Label label22;
        internal System.Windows.Forms.TextBox keyStoreFileTxtBox;
        internal System.Windows.Forms.Label label21;
        internal System.Windows.Forms.Label label20;
        internal System.Windows.Forms.CheckBox useKeyStoreChkBox;
        internal System.Windows.Forms.TextBox textBox3;
        internal System.Windows.Forms.Button selectKeyStoreFileBtn;
        internal System.Windows.Forms.TextBox aliasTxtBox;
        internal System.Windows.Forms.CheckBox useAliasChkBox;
        internal System.Windows.Forms.CheckBox createUnsignApkChkBox;
        internal System.Windows.Forms.ComboBox schemev4ComboBox;
        internal System.Windows.Forms.Label label27;
        internal System.Windows.Forms.ComboBox schemev3ComboBox;
        internal System.Windows.Forms.Label label26;
        internal System.Windows.Forms.ComboBox schemev2ComboBox;
        internal System.Windows.Forms.Label label25;
        internal System.Windows.Forms.ComboBox schemev1ComboBox;
        internal System.Windows.Forms.Label label24;
        internal System.Windows.Forms.NumericUpDown decApiLvlUpDown;
        internal System.Windows.Forms.CheckBox decSetApiLvlChkBox;
        internal System.Windows.Forms.NumericUpDown buildApiLvlUpDown;
        internal System.Windows.Forms.CheckBox buildSetApiLvlChkBox;
        internal System.Windows.Forms.TabPage tabPageBaksmali;
        internal System.Windows.Forms.Button openFwFolderBtn;
        internal System.Windows.Forms.Button selApkFileInfoBtn;
        internal System.Windows.Forms.GroupBox smaliGroupBox;
        internal System.Windows.Forms.GroupBox bakSmaliGroupBox;
        internal System.Windows.Forms.Button comSmaliBtn;
        internal System.Windows.Forms.TextBox smaliBrowseInputDirTxtBox;
        internal System.Windows.Forms.Button smaliBrowseInputDirBtn;
        internal System.Windows.Forms.Button decSmaliBtn;
        internal System.Windows.Forms.Button baksmaliBrowseInputDexBtn;
        internal System.Windows.Forms.TextBox baksmaliBrowseInputDexTxtBox;
        internal System.Windows.Forms.CheckBox smaliUseOutputChkBox;
        internal System.Windows.Forms.Button smaliBrowseOutputBtn;
        internal System.Windows.Forms.TextBox smaliBrowseOutputTxtBox;
        internal System.Windows.Forms.CheckBox baksmaliUseOutputChkBox;
        internal System.Windows.Forms.Button baksmaliBrowseOutputBtn;
        internal System.Windows.Forms.TextBox baksmaliBrowseOutputTxtBox;
        internal System.Windows.Forms.Button openApktoolYmlBtn;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label28;
        internal System.Windows.Forms.Button signApkOpenDirBtn;
        internal System.Windows.Forms.Button alignApkOpenDirBtn;
        internal System.Windows.Forms.Button decOutOpenDirBtn;
        private System.Windows.Forms.CheckBox useAapt2ChkBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.RichTextBox fullInfoTextBox;
        internal System.Windows.Forms.TabPage basicInfoTabPage;
        internal System.Windows.Forms.Button apkMirrorLinkBtn;
        internal System.Windows.Forms.Button apkSupportLinkBtn;
        internal System.Windows.Forms.Button apkGkLinkBtn;
        internal System.Windows.Forms.TextBox archSdkTxtBox;
        internal System.Windows.Forms.Label label30;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.CheckBox autoDelIdsigChkBox;
        private System.Windows.Forms.CheckBox checkBox1;
        internal System.Windows.Forms.TextBox launchActivityTxtBox;
        internal System.Windows.Forms.Label label31;
        internal System.Windows.Forms.RichTextBox logTxtBox;
        internal System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.Button selApkAdbBtn;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label33;
        internal System.Windows.Forms.TabPage tabPageAdb;
        internal System.Windows.Forms.Button installApkBtn;
        internal System.Windows.Forms.TextBox apkPathAdbTxtBox;
        internal System.Windows.Forms.ListBox devicesListBox;
        internal System.Windows.Forms.CheckBox setVendorChkBox;
        internal System.Windows.Forms.Button killAdbBtn;
        internal System.Windows.Forms.Button refreshDevicesBtn;
        internal System.Windows.Forms.Label selAdbDeviceLbl;
        private System.Windows.Forms.CheckBox checkBox2;
        internal System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem apktoolIssuesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem baksmaliIssuesToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem openTempFolderToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem reportAnIsuueToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem saveLogToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem clearTempFolderToolStripMenuItem;
        internal System.Windows.Forms.CheckBox checkBox_BUILD_NetSecConf;
        private System.Windows.Forms.CheckBox checkBox7;
        internal System.Windows.Forms.ToolStripMenuItem newInsToolStripMenuItem;
        internal System.Windows.Forms.Panel mergePanel;
        internal System.Windows.Forms.Label splitApkTxt;
        internal System.Windows.Forms.TextBox splitApkPathTxtBox;
        internal System.Windows.Forms.Button selSplitApkBtn;
        internal System.Windows.Forms.Button mergeApkBtn;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem1;
        internal System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem useAPKEditorForDecompilingItem;
        internal System.Windows.Forms.CheckBox overrideAbiCheckBox;
        internal System.Windows.Forms.ComboBox overrideAbiComboBox;
        internal System.Windows.Forms.RichTextBox sigTxtBox;
        internal System.Windows.Forms.Label label5;
        internal System.Windows.Forms.NumericUpDown decJobsLvlUpDown;
        internal System.Windows.Forms.CheckBox checkBox3;
        internal System.Windows.Forms.NumericUpDown comJobsLvlUpDown;
        internal System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

