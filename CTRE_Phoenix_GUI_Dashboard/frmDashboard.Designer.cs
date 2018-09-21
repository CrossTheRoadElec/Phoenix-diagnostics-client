namespace CTRE_Phoenix_GUI_Dashboard
{
    partial class frmDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDashboard));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.bottomStrip_L = new System.Windows.Forms.ToolStripStatusLabel();
            this.bottomStrip_ML = new System.Windows.Forms.ToolStripStatusLabel();
            this.bottomStrip_MR = new System.Windows.Forms.ToolStripStatusLabel();
            this.bottomStrip_R = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.menuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.devicePollingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableAutoRefreshMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.disableAutoRefreshMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.dataTransferMethodToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sFTPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pOSTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemTools = new System.Windows.Forms.ToolStripMenuItem();
            this.captureAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.cboHostSelectorPrt = new System.Windows.Forms.ComboBox();
            this.btnStopServer = new System.Windows.Forms.Button();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.rtbRioUpdateBox = new System.Windows.Forms.RichTextBox();
            this.btnRevertBinaries = new System.Windows.Forms.Button();
            this.btnUpdateBinaries = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.cboHostSelectorAddr = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlDeviceListInner = new System.Windows.Forms.Panel();
            this.lstDevices = new System.Windows.Forms.ListView();
            this.deviceName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.softStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hardwareName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.deviceID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.firmwareVers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.manDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bootRev = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.hardVers = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgLstDevices = new System.Windows.Forms.ImageList(this.components);
            this.btnRefreshDevices = new System.Windows.Forms.Button();
            this.pnlGenDevConfigOuter = new System.Windows.Forms.Panel();
            this.pnlGenDevConfigInner = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnChangeDevId = new System.Windows.Forms.Button();
            this.blinkButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDeviceNewName = new System.Windows.Forms.TextBox();
            this.label50 = new System.Windows.Forms.Label();
            this.btnNameChange = new System.Windows.Forms.Button();
            this.numNewDevId = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.pnlFirmUpgradeOuter = new System.Windows.Forms.Panel();
            this.pnlFirmUpgradeInner = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.txtDeviceCRFPath = new System.Windows.Forms.TextBox();
            this.lblDevFlashResults = new System.Windows.Forms.Label();
            this.prgCanDev = new System.Windows.Forms.ProgressBar();
            this.btnUpdateDevice = new System.Windows.Forms.Button();
            this.btnFirmwareDialog = new System.Windows.Forms.Button();
            this.panelSelfTestAndConfigControls = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnSaveConfigs = new System.Windows.Forms.Button();
            this.btnRefreshConfigs = new System.Windows.Forms.Button();
            this.btnSelfTest = new System.Windows.Forms.Button();
            this.groupedControls = new System.Windows.Forms.TabControl();
            this.txtSelfTestResults = new System.Windows.Forms.RichTextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.btnJumpToBottom = new System.Windows.Forms.Button();
            this.btnPauseTelem = new System.Windows.Forms.Button();
            this.btnClrDiagLog = new System.Windows.Forms.Button();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gridDiagnosticLog = new System.Windows.Forms.DataGridView();
            this.clmLineNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmResponse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.popupHttpLog = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectAllCtrlAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browserMessageDisp = new System.Windows.Forms.WebBrowser();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.tbUnitTesting = new System.Windows.Forms.TabPage();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.btnStopUnitTest = new System.Windows.Forms.Button();
            this.rtbUnitTestBox = new System.Windows.Forms.RichTextBox();
            this.unitTestingCheckboxes = new System.Windows.Forms.CheckedListBox();
            this.overnightTestButton = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.menuStripTop.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlDeviceListInner.SuspendLayout();
            this.pnlGenDevConfigOuter.SuspendLayout();
            this.pnlGenDevConfigInner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewDevId)).BeginInit();
            this.pnlFirmUpgradeOuter.SuspendLayout();
            this.pnlFirmUpgradeInner.SuspendLayout();
            this.panelSelfTestAndConfigControls.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDiagnosticLog)).BeginInit();
            this.popupHttpLog.SuspendLayout();
            this.tbUnitTesting.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bottomStrip_L,
            this.bottomStrip_ML,
            this.bottomStrip_MR,
            this.bottomStrip_R});
            this.statusStrip1.Location = new System.Drawing.Point(0, 533);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(967, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // bottomStrip_L
            // 
            this.bottomStrip_L.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bottomStrip_L.Name = "bottomStrip_L";
            this.bottomStrip_L.Size = new System.Drawing.Size(397, 17);
            this.bottomStrip_L.Spring = true;
            this.bottomStrip_L.Text = "????????????";
            // 
            // bottomStrip_ML
            // 
            this.bottomStrip_ML.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bottomStrip_ML.Name = "bottomStrip_ML";
            this.bottomStrip_ML.Size = new System.Drawing.Size(91, 17);
            this.bottomStrip_ML.Text = "????????????";
            this.bottomStrip_ML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bottomStrip_MR
            // 
            this.bottomStrip_MR.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bottomStrip_MR.Name = "bottomStrip_MR";
            this.bottomStrip_MR.Size = new System.Drawing.Size(397, 17);
            this.bottomStrip_MR.Spring = true;
            this.bottomStrip_MR.Text = "????????????";
            // 
            // bottomStrip_R
            // 
            this.bottomStrip_R.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bottomStrip_R.Name = "bottomStrip_R";
            this.bottomStrip_R.Size = new System.Drawing.Size(67, 17);
            this.bottomStrip_R.Text = "????????????";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStripTop
            // 
            this.menuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemOptions,
            this.menuItemTools});
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Size = new System.Drawing.Size(967, 24);
            this.menuStripTop.TabIndex = 1;
            this.menuStripTop.Text = "menuStripTop";
            // 
            // menuItemOptions
            // 
            this.menuItemOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.devicePollingToolStripMenuItem,
            this.dataTransferMethodToolStripMenuItem});
            this.menuItemOptions.Name = "menuItemOptions";
            this.menuItemOptions.Size = new System.Drawing.Size(61, 20);
            this.menuItemOptions.Text = "Options";
            // 
            // devicePollingToolStripMenuItem
            // 
            this.devicePollingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enableAutoRefreshMenuItem1,
            this.disableAutoRefreshMenuItem1});
            this.devicePollingToolStripMenuItem.Name = "devicePollingToolStripMenuItem";
            this.devicePollingToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.devicePollingToolStripMenuItem.Text = "Auto Refresh Devices";
            // 
            // enableAutoRefreshMenuItem1
            // 
            this.enableAutoRefreshMenuItem1.Name = "enableAutoRefreshMenuItem1";
            this.enableAutoRefreshMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.enableAutoRefreshMenuItem1.Text = "Enable";
            this.enableAutoRefreshMenuItem1.Click += new System.EventHandler(this.enableDisableDeviceAutoRefresh_Click);
            // 
            // disableAutoRefreshMenuItem1
            // 
            this.disableAutoRefreshMenuItem1.Name = "disableAutoRefreshMenuItem1";
            this.disableAutoRefreshMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.disableAutoRefreshMenuItem1.Text = "Disable";
            this.disableAutoRefreshMenuItem1.Click += new System.EventHandler(this.enableDisableDeviceAutoRefresh_Click);
            // 
            // dataTransferMethodToolStripMenuItem
            // 
            this.dataTransferMethodToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sFTPToolStripMenuItem,
            this.pOSTToolStripMenuItem});
            this.dataTransferMethodToolStripMenuItem.Name = "dataTransferMethodToolStripMenuItem";
            this.dataTransferMethodToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.dataTransferMethodToolStripMenuItem.Text = "Data Transfer Method";
            // 
            // sFTPToolStripMenuItem
            // 
            this.sFTPToolStripMenuItem.Name = "sFTPToolStripMenuItem";
            this.sFTPToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.sFTPToolStripMenuItem.Text = "SFTP";
            this.sFTPToolStripMenuItem.Click += new System.EventHandler(this.enableDisabledSftpDataTransfer_Click);
            // 
            // pOSTToolStripMenuItem
            // 
            this.pOSTToolStripMenuItem.Name = "pOSTToolStripMenuItem";
            this.pOSTToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.pOSTToolStripMenuItem.Text = "POST";
            this.pOSTToolStripMenuItem.Click += new System.EventHandler(this.enableDisabledSftpDataTransfer_Click);
            // 
            // menuItemTools
            // 
            this.menuItemTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureAllToolStripMenuItem});
            this.menuItemTools.Name = "menuItemTools";
            this.menuItemTools.Size = new System.Drawing.Size(47, 20);
            this.menuItemTools.Text = "Tools";
            this.menuItemTools.Click += new System.EventHandler(this.btnEasterEgg_Click);
            // 
            // captureAllToolStripMenuItem
            // 
            this.captureAllToolStripMenuItem.Name = "captureAllToolStripMenuItem";
            this.captureAllToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.captureAllToolStripMenuItem.Text = "Capture All (ZIP file)";
            this.captureAllToolStripMenuItem.Click += new System.EventHandler(this.captureAllToolStripMenuItem_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Controls.Add(this.tabPage6);
            this.tabControl.Controls.Add(this.tbUnitTesting);
            this.tabControl.Location = new System.Drawing.Point(0, 27);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(967, 503);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabOptions_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.cboHostSelectorPrt);
            this.tabPage2.Controls.Add(this.btnStopServer);
            this.tabPage2.Controls.Add(this.btnStartServer);
            this.tabPage2.Controls.Add(this.rtbRioUpdateBox);
            this.tabPage2.Controls.Add(this.btnRevertBinaries);
            this.tabPage2.Controls.Add(this.btnUpdateBinaries);
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Controls.Add(this.cboHostSelectorAddr);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(959, 477);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "Prepare the Target Robot Controller";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(760, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Port:";
            // 
            // cboHostSelectorPrt
            // 
            this.cboHostSelectorPrt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHostSelectorPrt.FormattingEnabled = true;
            this.cboHostSelectorPrt.Items.AddRange(new object[] {
            "1250 # Default Port"});
            this.cboHostSelectorPrt.Location = new System.Drawing.Point(795, 62);
            this.cboHostSelectorPrt.Name = "cboHostSelectorPrt";
            this.cboHostSelectorPrt.Size = new System.Drawing.Size(153, 21);
            this.cboHostSelectorPrt.TabIndex = 1;
            this.cboHostSelectorPrt.TextChanged += new System.EventHandler(this.cboHostSelectorPrt_TextChanged);
            // 
            // btnStopServer
            // 
            this.btnStopServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopServer.Location = new System.Drawing.Point(833, 122);
            this.btnStopServer.Name = "btnStopServer";
            this.btnStopServer.Size = new System.Drawing.Size(114, 23);
            this.btnStopServer.TabIndex = 5;
            this.btnStopServer.Text = "Force Stop Server";
            this.btnStopServer.UseVisualStyleBackColor = true;
            this.btnStopServer.Click += new System.EventHandler(this.btnStopServer_Click);
            // 
            // btnStartServer
            // 
            this.btnStartServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartServer.Location = new System.Drawing.Point(704, 122);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(123, 23);
            this.btnStartServer.TabIndex = 4;
            this.btnStartServer.Text = "Force Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // rtbRioUpdateBox
            // 
            this.rtbRioUpdateBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbRioUpdateBox.Location = new System.Drawing.Point(18, 93);
            this.rtbRioUpdateBox.Name = "rtbRioUpdateBox";
            this.rtbRioUpdateBox.Size = new System.Drawing.Size(680, 391);
            this.rtbRioUpdateBox.TabIndex = 11;
            this.rtbRioUpdateBox.Text = "";
            // 
            // btnRevertBinaries
            // 
            this.btnRevertBinaries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRevertBinaries.Location = new System.Drawing.Point(833, 93);
            this.btnRevertBinaries.Name = "btnRevertBinaries";
            this.btnRevertBinaries.Size = new System.Drawing.Size(115, 23);
            this.btnRevertBinaries.TabIndex = 3;
            this.btnRevertBinaries.Text = "Revert Binaries";
            this.btnRevertBinaries.UseVisualStyleBackColor = true;
            this.btnRevertBinaries.Click += new System.EventHandler(this.btnRevertBinaries_Click);
            // 
            // btnUpdateBinaries
            // 
            this.btnUpdateBinaries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateBinaries.Location = new System.Drawing.Point(704, 93);
            this.btnUpdateBinaries.Name = "btnUpdateBinaries";
            this.btnUpdateBinaries.Size = new System.Drawing.Size(123, 23);
            this.btnUpdateBinaries.TabIndex = 2;
            this.btnUpdateBinaries.Text = "Update Binaries";
            this.btnUpdateBinaries.UseVisualStyleBackColor = true;
            this.btnUpdateBinaries.Click += new System.EventHandler(this.btnUpdateBinaries_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.richTextBox1.Location = new System.Drawing.Point(8, 6);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(940, 49);
            this.richTextBox1.TabIndex = 8;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // cboHostSelectorAddr
            // 
            this.cboHostSelectorAddr.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboHostSelectorAddr.FormattingEnabled = true;
            this.cboHostSelectorAddr.Items.AddRange(new object[] {
            "172.22.11.2 # RoboRIO Over USB",
            "localhost # Local PC Development"});
            this.cboHostSelectorAddr.Location = new System.Drawing.Point(156, 62);
            this.cboHostSelectorAddr.Name = "cboHostSelectorAddr";
            this.cboHostSelectorAddr.Size = new System.Drawing.Size(595, 21);
            this.cboHostSelectorAddr.TabIndex = 0;
            this.cboHostSelectorAddr.TextChanged += new System.EventHandler(this.cboHostSelector_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Diagnostic Server Address:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.splitContainer1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(959, 477);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "CAN Devices";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.Color.AntiqueWhite;
            this.splitContainer1.Panel1.Controls.Add(this.pnlDeviceListInner);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnlGenDevConfigOuter);
            this.splitContainer1.Panel2.Controls.Add(this.pnlFirmUpgradeOuter);
            this.splitContainer1.Panel2.Controls.Add(this.panelSelfTestAndConfigControls);
            this.splitContainer1.Size = new System.Drawing.Size(953, 471);
            this.splitContainer1.SplitterDistance = 172;
            this.splitContainer1.TabIndex = 48;
            // 
            // pnlDeviceListInner
            // 
            this.pnlDeviceListInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDeviceListInner.BackColor = System.Drawing.Color.White;
            this.pnlDeviceListInner.Controls.Add(this.lstDevices);
            this.pnlDeviceListInner.Controls.Add(this.btnRefreshDevices);
            this.pnlDeviceListInner.Location = new System.Drawing.Point(3, 3);
            this.pnlDeviceListInner.Name = "pnlDeviceListInner";
            this.pnlDeviceListInner.Size = new System.Drawing.Size(943, 162);
            this.pnlDeviceListInner.TabIndex = 5;
            // 
            // lstDevices
            // 
            this.lstDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.deviceName,
            this.softStatus,
            this.hardwareName,
            this.deviceID,
            this.firmwareVers,
            this.manDate,
            this.bootRev,
            this.hardVers});
            this.lstDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDevices.ForeColor = System.Drawing.SystemColors.WindowText;
            this.lstDevices.FullRowSelect = true;
            this.lstDevices.GridLines = true;
            this.lstDevices.HideSelection = false;
            this.lstDevices.Location = new System.Drawing.Point(0, 0);
            this.lstDevices.MultiSelect = false;
            this.lstDevices.Name = "lstDevices";
            this.lstDevices.Size = new System.Drawing.Size(943, 132);
            this.lstDevices.SmallImageList = this.imgLstDevices;
            this.lstDevices.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstDevices.TabIndex = 3;
            this.lstDevices.UseCompatibleStateImageBehavior = false;
            this.lstDevices.View = System.Windows.Forms.View.Details;
            this.lstDevices.SelectedIndexChanged += new System.EventHandler(this.lstDevices_SelectedIndexChanged);
            // 
            // deviceName
            // 
            this.deviceName.Text = "Device Name";
            this.deviceName.Width = 149;
            // 
            // softStatus
            // 
            this.softStatus.Text = "Software Status";
            this.softStatus.Width = 111;
            // 
            // hardwareName
            // 
            this.hardwareName.Text = "Hardware";
            this.hardwareName.Width = 150;
            // 
            // deviceID
            // 
            this.deviceID.Text = "ID";
            this.deviceID.Width = 40;
            // 
            // firmwareVers
            // 
            this.firmwareVers.Text = "Firmware Version";
            this.firmwareVers.Width = 100;
            // 
            // manDate
            // 
            this.manDate.Text = "Manufacturer Date";
            this.manDate.Width = 134;
            // 
            // bootRev
            // 
            this.bootRev.Text = "Bootloader Revision";
            this.bootRev.Width = 117;
            // 
            // hardVers
            // 
            this.hardVers.Text = "Hardware Version";
            this.hardVers.Width = 125;
            // 
            // imgLstDevices
            // 
            this.imgLstDevices.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstDevices.ImageStream")));
            this.imgLstDevices.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstDevices.Images.SetKeyName(0, "Nothing.png");
            this.imgLstDevices.Images.SetKeyName(1, "PCM_48.png");
            this.imgLstDevices.Images.SetKeyName(2, "PDP_48.png");
            this.imgLstDevices.Images.SetKeyName(3, "Talon SRX_48.png");
            this.imgLstDevices.Images.SetKeyName(4, "Victor SPX_48.png");
            this.imgLstDevices.Images.SetKeyName(5, "Pigeon_48.png");
            this.imgLstDevices.Images.SetKeyName(6, "Pigeon_48 Ribbon.png");
            this.imgLstDevices.Images.SetKeyName(7, "Canifier_48.png");
            this.imgLstDevices.Images.SetKeyName(8, "Unknown.png");
            // 
            // btnRefreshDevices
            // 
            this.btnRefreshDevices.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnRefreshDevices.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefreshDevices.Location = new System.Drawing.Point(0, 132);
            this.btnRefreshDevices.Name = "btnRefreshDevices";
            this.btnRefreshDevices.Size = new System.Drawing.Size(943, 30);
            this.btnRefreshDevices.TabIndex = 0;
            this.btnRefreshDevices.Text = "Refresh Devices";
            this.btnRefreshDevices.UseVisualStyleBackColor = true;
            this.btnRefreshDevices.Click += new System.EventHandler(this.btnRefreshDevices_Click);
            // 
            // pnlGenDevConfigOuter
            // 
            this.pnlGenDevConfigOuter.BackColor = System.Drawing.Color.Cyan;
            this.pnlGenDevConfigOuter.Controls.Add(this.pnlGenDevConfigInner);
            this.pnlGenDevConfigOuter.Location = new System.Drawing.Point(3, 3);
            this.pnlGenDevConfigOuter.Name = "pnlGenDevConfigOuter";
            this.pnlGenDevConfigOuter.Size = new System.Drawing.Size(456, 130);
            this.pnlGenDevConfigOuter.TabIndex = 1;
            // 
            // pnlGenDevConfigInner
            // 
            this.pnlGenDevConfigInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGenDevConfigInner.BackColor = System.Drawing.Color.White;
            this.pnlGenDevConfigInner.Controls.Add(this.label4);
            this.pnlGenDevConfigInner.Controls.Add(this.btnChangeDevId);
            this.pnlGenDevConfigInner.Controls.Add(this.blinkButton);
            this.pnlGenDevConfigInner.Controls.Add(this.label1);
            this.pnlGenDevConfigInner.Controls.Add(this.txtDeviceNewName);
            this.pnlGenDevConfigInner.Controls.Add(this.label50);
            this.pnlGenDevConfigInner.Controls.Add(this.btnNameChange);
            this.pnlGenDevConfigInner.Controls.Add(this.numNewDevId);
            this.pnlGenDevConfigInner.Controls.Add(this.label15);
            this.pnlGenDevConfigInner.Location = new System.Drawing.Point(3, 3);
            this.pnlGenDevConfigInner.Name = "pnlGenDevConfigInner";
            this.pnlGenDevConfigInner.Size = new System.Drawing.Size(450, 124);
            this.pnlGenDevConfigInner.TabIndex = 48;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(142, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(174, 13);
            this.label4.TabIndex = 48;
            this.label4.Text = "General Device Configuration";
            // 
            // btnChangeDevId
            // 
            this.btnChangeDevId.Location = new System.Drawing.Point(345, 30);
            this.btnChangeDevId.Name = "btnChangeDevId";
            this.btnChangeDevId.Size = new System.Drawing.Size(85, 23);
            this.btnChangeDevId.TabIndex = 1;
            this.btnChangeDevId.Text = "Change ID";
            this.btnChangeDevId.UseVisualStyleBackColor = true;
            this.btnChangeDevId.Click += new System.EventHandler(this.idChangeButton_Click);
            // 
            // blinkButton
            // 
            this.blinkButton.Location = new System.Drawing.Point(345, 80);
            this.blinkButton.Name = "blinkButton";
            this.blinkButton.Size = new System.Drawing.Size(85, 23);
            this.blinkButton.TabIndex = 4;
            this.blinkButton.Text = "Blink";
            this.blinkButton.UseVisualStyleBackColor = true;
            this.blinkButton.Click += new System.EventHandler(this.blinkButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(270, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Press to animate device LEDs and confirm ID is correct.";
            // 
            // txtDeviceNewName
            // 
            this.txtDeviceNewName.Location = new System.Drawing.Point(145, 57);
            this.txtDeviceNewName.Name = "txtDeviceNewName";
            this.txtDeviceNewName.Size = new System.Drawing.Size(194, 20);
            this.txtDeviceNewName.TabIndex = 2;
            this.txtDeviceNewName.TextChanged += new System.EventHandler(this.txtDeviceNewName_TextChanged);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Location = new System.Drawing.Point(45, 60);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(94, 13);
            this.label50.TabIndex = 42;
            this.label50.Text = "Change the name:";
            // 
            // btnNameChange
            // 
            this.btnNameChange.Location = new System.Drawing.Point(345, 55);
            this.btnNameChange.Name = "btnNameChange";
            this.btnNameChange.Size = new System.Drawing.Size(85, 23);
            this.btnNameChange.TabIndex = 3;
            this.btnNameChange.Text = "Change Name";
            this.btnNameChange.UseVisualStyleBackColor = true;
            this.btnNameChange.Click += new System.EventHandler(this.nameChangeButton_Click);
            // 
            // numNewDevId
            // 
            this.numNewDevId.Location = new System.Drawing.Point(295, 33);
            this.numNewDevId.Maximum = new decimal(new int[] {
            63,
            0,
            0,
            0});
            this.numNewDevId.Name = "numNewDevId";
            this.numNewDevId.Size = new System.Drawing.Size(44, 20);
            this.numNewDevId.TabIndex = 1;
            this.numNewDevId.ValueChanged += new System.EventHandler(this.numNewDevId_ValueChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(216, 35);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(79, 13);
            this.label15.TabIndex = 41;
            this.label15.Text = "Change the ID:";
            // 
            // pnlFirmUpgradeOuter
            // 
            this.pnlFirmUpgradeOuter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pnlFirmUpgradeOuter.Controls.Add(this.pnlFirmUpgradeInner);
            this.pnlFirmUpgradeOuter.Location = new System.Drawing.Point(3, 136);
            this.pnlFirmUpgradeOuter.Name = "pnlFirmUpgradeOuter";
            this.pnlFirmUpgradeOuter.Size = new System.Drawing.Size(456, 160);
            this.pnlFirmUpgradeOuter.TabIndex = 46;
            // 
            // pnlFirmUpgradeInner
            // 
            this.pnlFirmUpgradeInner.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFirmUpgradeInner.BackColor = System.Drawing.Color.White;
            this.pnlFirmUpgradeInner.Controls.Add(this.label2);
            this.pnlFirmUpgradeInner.Controls.Add(this.label17);
            this.pnlFirmUpgradeInner.Controls.Add(this.txtDeviceCRFPath);
            this.pnlFirmUpgradeInner.Controls.Add(this.lblDevFlashResults);
            this.pnlFirmUpgradeInner.Controls.Add(this.prgCanDev);
            this.pnlFirmUpgradeInner.Controls.Add(this.btnUpdateDevice);
            this.pnlFirmUpgradeInner.Controls.Add(this.btnFirmwareDialog);
            this.pnlFirmUpgradeInner.Location = new System.Drawing.Point(3, 3);
            this.pnlFirmUpgradeInner.Name = "pnlFirmUpgradeInner";
            this.pnlFirmUpgradeInner.Size = new System.Drawing.Size(450, 153);
            this.pnlFirmUpgradeInner.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(135, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Field-Upgrade Device Firmware";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(25, 34);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(309, 13);
            this.label17.TabIndex = 46;
            this.label17.Text = "Select CRF and Press \"Update Firmware\" to flash new firmware.";
            // 
            // txtDeviceCRFPath
            // 
            this.txtDeviceCRFPath.Location = new System.Drawing.Point(27, 51);
            this.txtDeviceCRFPath.Name = "txtDeviceCRFPath";
            this.txtDeviceCRFPath.Size = new System.Drawing.Size(361, 20);
            this.txtDeviceCRFPath.TabIndex = 5;
            this.txtDeviceCRFPath.TextChanged += new System.EventHandler(this.txtDeviceCRFPath_TextChanged);
            // 
            // lblDevFlashResults
            // 
            this.lblDevFlashResults.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDevFlashResults.Location = new System.Drawing.Point(27, 104);
            this.lblDevFlashResults.Name = "lblDevFlashResults";
            this.lblDevFlashResults.Size = new System.Drawing.Size(406, 39);
            this.lblDevFlashResults.TabIndex = 45;
            // 
            // prgCanDev
            // 
            this.prgCanDev.Location = new System.Drawing.Point(27, 75);
            this.prgCanDev.Name = "prgCanDev";
            this.prgCanDev.Size = new System.Drawing.Size(267, 25);
            this.prgCanDev.TabIndex = 44;
            this.prgCanDev.Visible = false;
            // 
            // btnUpdateDevice
            // 
            this.btnUpdateDevice.Location = new System.Drawing.Point(303, 75);
            this.btnUpdateDevice.Name = "btnUpdateDevice";
            this.btnUpdateDevice.Size = new System.Drawing.Size(130, 25);
            this.btnUpdateDevice.TabIndex = 7;
            this.btnUpdateDevice.Text = "Update Device";
            this.btnUpdateDevice.UseVisualStyleBackColor = true;
            this.btnUpdateDevice.Click += new System.EventHandler(this.btnUpdateDevice_Click);
            // 
            // btnFirmwareDialog
            // 
            this.btnFirmwareDialog.Location = new System.Drawing.Point(394, 50);
            this.btnFirmwareDialog.Name = "btnFirmwareDialog";
            this.btnFirmwareDialog.Size = new System.Drawing.Size(36, 20);
            this.btnFirmwareDialog.TabIndex = 6;
            this.btnFirmwareDialog.Text = "...";
            this.btnFirmwareDialog.UseVisualStyleBackColor = true;
            this.btnFirmwareDialog.Click += new System.EventHandler(this.btnFirmwareDialog_Click);
            // 
            // panelSelfTestAndConfigControls
            // 
            this.panelSelfTestAndConfigControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSelfTestAndConfigControls.BackColor = System.Drawing.Color.DarkOrange;
            this.panelSelfTestAndConfigControls.Controls.Add(this.tableLayoutPanel1);
            this.panelSelfTestAndConfigControls.Controls.Add(this.groupedControls);
            this.panelSelfTestAndConfigControls.Controls.Add(this.txtSelfTestResults);
            this.panelSelfTestAndConfigControls.Location = new System.Drawing.Point(465, 3);
            this.panelSelfTestAndConfigControls.Name = "panelSelfTestAndConfigControls";
            this.panelSelfTestAndConfigControls.Size = new System.Drawing.Size(481, 289);
            this.panelSelfTestAndConfigControls.TabIndex = 48;
            this.panelSelfTestAndConfigControls.Resize += new System.EventHandler(this.panelSelfTestAndConfigControls_Resize);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Controls.Add(this.btnSaveConfigs, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnRefreshConfigs, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnSelfTest, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 256);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(475, 30);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // btnSaveConfigs
            // 
            this.btnSaveConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSaveConfigs.Location = new System.Drawing.Point(3, 3);
            this.btnSaveConfigs.Name = "btnSaveConfigs";
            this.btnSaveConfigs.Size = new System.Drawing.Size(152, 24);
            this.btnSaveConfigs.TabIndex = 10;
            this.btnSaveConfigs.Text = "Save Settings";
            this.btnSaveConfigs.UseVisualStyleBackColor = true;
            this.btnSaveConfigs.Click += new System.EventHandler(this.btnSaveConfigs_Click);
            // 
            // btnRefreshConfigs
            // 
            this.btnRefreshConfigs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefreshConfigs.Location = new System.Drawing.Point(161, 3);
            this.btnRefreshConfigs.Name = "btnRefreshConfigs";
            this.btnRefreshConfigs.Size = new System.Drawing.Size(152, 24);
            this.btnRefreshConfigs.TabIndex = 11;
            this.btnRefreshConfigs.Text = "Refresh Configs";
            this.btnRefreshConfigs.UseVisualStyleBackColor = true;
            this.btnRefreshConfigs.Click += new System.EventHandler(this.btnRefreshConfigs_Click);
            // 
            // btnSelfTest
            // 
            this.btnSelfTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSelfTest.Location = new System.Drawing.Point(319, 3);
            this.btnSelfTest.Name = "btnSelfTest";
            this.btnSelfTest.Size = new System.Drawing.Size(153, 24);
            this.btnSelfTest.TabIndex = 9;
            this.btnSelfTest.Text = "Self-Test";
            this.btnSelfTest.UseVisualStyleBackColor = true;
            this.btnSelfTest.Click += new System.EventHandler(this.btnSelfTest_Click);
            // 
            // groupedControls
            // 
            this.groupedControls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupedControls.Location = new System.Drawing.Point(3, 1);
            this.groupedControls.Name = "groupedControls";
            this.groupedControls.SelectedIndex = 0;
            this.groupedControls.Size = new System.Drawing.Size(475, 252);
            this.groupedControls.TabIndex = 2;
            // 
            // txtSelfTestResults
            // 
            this.txtSelfTestResults.BackColor = System.Drawing.Color.White;
            this.txtSelfTestResults.Location = new System.Drawing.Point(81, 63);
            this.txtSelfTestResults.Name = "txtSelfTestResults";
            this.txtSelfTestResults.Size = new System.Drawing.Size(183, 36);
            this.txtSelfTestResults.TabIndex = 0;
            this.txtSelfTestResults.Text = "This is the Self Test Box, select the device you wish to perform the self test on" +
    " and Press the \"Self Test\" button";
            this.txtSelfTestResults.Visible = false;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.btnJumpToBottom);
            this.tabPage6.Controls.Add(this.btnPauseTelem);
            this.tabPage6.Controls.Add(this.btnClrDiagLog);
            this.tabPage6.Controls.Add(this.splitContainer2);
            this.tabPage6.Controls.Add(this.richTextBox2);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(959, 477);
            this.tabPage6.TabIndex = 2;
            this.tabPage6.Text = "Web Diagnostics Log";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // btnJumpToBottom
            // 
            this.btnJumpToBottom.Location = new System.Drawing.Point(323, 69);
            this.btnJumpToBottom.Name = "btnJumpToBottom";
            this.btnJumpToBottom.Size = new System.Drawing.Size(167, 23);
            this.btnJumpToBottom.TabIndex = 2;
            this.btnJumpToBottom.Text = "Pause";
            this.btnJumpToBottom.UseVisualStyleBackColor = true;
            this.btnJumpToBottom.Click += new System.EventHandler(this.btnJumpToBottom_Click);
            // 
            // btnPauseTelem
            // 
            this.btnPauseTelem.Location = new System.Drawing.Point(150, 69);
            this.btnPauseTelem.Name = "btnPauseTelem";
            this.btnPauseTelem.Size = new System.Drawing.Size(167, 23);
            this.btnPauseTelem.TabIndex = 1;
            this.btnPauseTelem.Text = "Pause";
            this.btnPauseTelem.UseVisualStyleBackColor = true;
            this.btnPauseTelem.Click += new System.EventHandler(this.btnPauseTelem_Click);
            // 
            // btnClrDiagLog
            // 
            this.btnClrDiagLog.Location = new System.Drawing.Point(27, 69);
            this.btnClrDiagLog.Name = "btnClrDiagLog";
            this.btnClrDiagLog.Size = new System.Drawing.Size(115, 23);
            this.btnClrDiagLog.TabIndex = 0;
            this.btnClrDiagLog.Text = "Clear Log";
            this.btnClrDiagLog.UseVisualStyleBackColor = true;
            this.btnClrDiagLog.Click += new System.EventHandler(this.btnClrDiagLog_Click);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(8, 98);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.gridDiagnosticLog);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.splitContainer2.Panel2.Controls.Add(this.browserMessageDisp);
            this.splitContainer2.Size = new System.Drawing.Size(951, 376);
            this.splitContainer2.SplitterDistance = 650;
            this.splitContainer2.TabIndex = 3;
            // 
            // gridDiagnosticLog
            // 
            this.gridDiagnosticLog.AllowUserToAddRows = false;
            this.gridDiagnosticLog.AllowUserToDeleteRows = false;
            this.gridDiagnosticLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDiagnosticLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmLineNumber,
            this.clmUrl,
            this.clmResponse,
            this.clmType});
            this.gridDiagnosticLog.ContextMenuStrip = this.popupHttpLog;
            this.gridDiagnosticLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDiagnosticLog.Location = new System.Drawing.Point(0, 0);
            this.gridDiagnosticLog.Name = "gridDiagnosticLog";
            this.gridDiagnosticLog.ReadOnly = true;
            this.gridDiagnosticLog.Size = new System.Drawing.Size(648, 374);
            this.gridDiagnosticLog.TabIndex = 4;
            this.gridDiagnosticLog.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.diagnosticLog_CellClick);
            this.gridDiagnosticLog.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridDiagnosticLog_CellEnter);
            // 
            // clmLineNumber
            // 
            this.clmLineNumber.HeaderText = "Number";
            this.clmLineNumber.Name = "clmLineNumber";
            this.clmLineNumber.ReadOnly = true;
            this.clmLineNumber.Width = 60;
            // 
            // clmUrl
            // 
            this.clmUrl.HeaderText = "URL";
            this.clmUrl.Name = "clmUrl";
            this.clmUrl.ReadOnly = true;
            this.clmUrl.Width = 230;
            // 
            // clmResponse
            // 
            this.clmResponse.HeaderText = "JSON Response";
            this.clmResponse.Name = "clmResponse";
            this.clmResponse.ReadOnly = true;
            this.clmResponse.Width = 230;
            // 
            // clmType
            // 
            this.clmType.HeaderText = "Type";
            this.clmType.Name = "clmType";
            this.clmType.ReadOnly = true;
            this.clmType.Width = 80;
            // 
            // popupHttpLog
            // 
            this.popupHttpLog.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.selectAllCtrlAToolStripMenuItem});
            this.popupHttpLog.Name = "popupHttpLog";
            this.popupHttpLog.Size = new System.Drawing.Size(169, 48);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.copyToolStripMenuItem.Text = "Copy (Ctrl+C)";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.btnCopyHttpLog_Click);
            // 
            // selectAllCtrlAToolStripMenuItem
            // 
            this.selectAllCtrlAToolStripMenuItem.Name = "selectAllCtrlAToolStripMenuItem";
            this.selectAllCtrlAToolStripMenuItem.Size = new System.Drawing.Size(168, 22);
            this.selectAllCtrlAToolStripMenuItem.Text = "Select All (Ctrl+A)";
            this.selectAllCtrlAToolStripMenuItem.Click += new System.EventHandler(this.selectAllCtrlAToolStripMenuItem_Click);
            // 
            // browserMessageDisp
            // 
            this.browserMessageDisp.AllowNavigation = false;
            this.browserMessageDisp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browserMessageDisp.Location = new System.Drawing.Point(3, 3);
            this.browserMessageDisp.MinimumSize = new System.Drawing.Size(20, 20);
            this.browserMessageDisp.Name = "browserMessageDisp";
            this.browserMessageDisp.Size = new System.Drawing.Size(289, 368);
            this.browserMessageDisp.TabIndex = 4;
            // 
            // richTextBox2
            // 
            this.richTextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.richTextBox2.Location = new System.Drawing.Point(27, 6);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(928, 57);
            this.richTextBox2.TabIndex = 9;
            this.richTextBox2.Text = resources.GetString("richTextBox2.Text");
            // 
            // tbUnitTesting
            // 
            this.tbUnitTesting.Controls.Add(this.richTextBox3);
            this.tbUnitTesting.Controls.Add(this.btnStopUnitTest);
            this.tbUnitTesting.Controls.Add(this.rtbUnitTestBox);
            this.tbUnitTesting.Controls.Add(this.unitTestingCheckboxes);
            this.tbUnitTesting.Controls.Add(this.overnightTestButton);
            this.tbUnitTesting.Location = new System.Drawing.Point(4, 22);
            this.tbUnitTesting.Name = "tbUnitTesting";
            this.tbUnitTesting.Padding = new System.Windows.Forms.Padding(3);
            this.tbUnitTesting.Size = new System.Drawing.Size(959, 477);
            this.tbUnitTesting.TabIndex = 3;
            this.tbUnitTesting.Text = "Unit Testing";
            this.tbUnitTesting.UseVisualStyleBackColor = true;
            // 
            // richTextBox3
            // 
            this.richTextBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.richTextBox3.Location = new System.Drawing.Point(548, 38);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.ReadOnly = true;
            this.richTextBox3.Size = new System.Drawing.Size(385, 110);
            this.richTextBox3.TabIndex = 4;
            this.richTextBox3.Text = resources.GetString("richTextBox3.Text");
            // 
            // btnStopUnitTest
            // 
            this.btnStopUnitTest.Location = new System.Drawing.Point(548, 8);
            this.btnStopUnitTest.Name = "btnStopUnitTest";
            this.btnStopUnitTest.Size = new System.Drawing.Size(85, 23);
            this.btnStopUnitTest.TabIndex = 3;
            this.btnStopUnitTest.Text = "Stop Test";
            this.btnStopUnitTest.UseVisualStyleBackColor = true;
            this.btnStopUnitTest.Click += new System.EventHandler(this.btnStopUnitTest_Click);
            // 
            // rtbUnitTestBox
            // 
            this.rtbUnitTestBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.rtbUnitTestBox.Location = new System.Drawing.Point(183, 8);
            this.rtbUnitTestBox.Name = "rtbUnitTestBox";
            this.rtbUnitTestBox.ReadOnly = true;
            this.rtbUnitTestBox.Size = new System.Drawing.Size(358, 425);
            this.rtbUnitTestBox.TabIndex = 2;
            this.rtbUnitTestBox.Text = "";
            // 
            // unitTestingCheckboxes
            // 
            this.unitTestingCheckboxes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.unitTestingCheckboxes.FormattingEnabled = true;
            this.unitTestingCheckboxes.Items.AddRange(new object[] {
            "Check Process Running",
            "Check Version",
            "Check Number of Devices",
            "Check Firmware Flash",
            "Reboot RIO At End"});
            this.unitTestingCheckboxes.Location = new System.Drawing.Point(8, 35);
            this.unitTestingCheckboxes.Name = "unitTestingCheckboxes";
            this.unitTestingCheckboxes.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.unitTestingCheckboxes.Size = new System.Drawing.Size(169, 379);
            this.unitTestingCheckboxes.TabIndex = 1;
            // 
            // overnightTestButton
            // 
            this.overnightTestButton.Location = new System.Drawing.Point(8, 6);
            this.overnightTestButton.Name = "overnightTestButton";
            this.overnightTestButton.Size = new System.Drawing.Size(169, 23);
            this.overnightTestButton.TabIndex = 0;
            this.overnightTestButton.Text = "Begin Overnight Test";
            this.overnightTestButton.UseVisualStyleBackColor = true;
            this.overnightTestButton.Click += new System.EventHandler(this.overnightTestButton_Click);
            // 
            // frmDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 555);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStripTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripTop;
            this.Name = "frmDashboard";
            this.Text = "Dashboard";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStripTop.ResumeLayout(false);
            this.menuStripTop.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlDeviceListInner.ResumeLayout(false);
            this.pnlGenDevConfigOuter.ResumeLayout(false);
            this.pnlGenDevConfigInner.ResumeLayout(false);
            this.pnlGenDevConfigInner.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numNewDevId)).EndInit();
            this.pnlFirmUpgradeOuter.ResumeLayout(false);
            this.pnlFirmUpgradeInner.ResumeLayout(false);
            this.pnlFirmUpgradeInner.PerformLayout();
            this.panelSelfTestAndConfigControls.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDiagnosticLog)).EndInit();
            this.popupHttpLog.ResumeLayout(false);
            this.tbUnitTesting.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel bottomStrip_L;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripStatusLabel bottomStrip_MR;
        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.ToolStripMenuItem menuItemOptions;
        private System.Windows.Forms.ToolStripStatusLabel bottomStrip_R;
        private System.Windows.Forms.ToolStripMenuItem devicePollingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem enableAutoRefreshMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem disableAutoRefreshMenuItem1;
        private System.Windows.Forms.ToolStripStatusLabel bottomStrip_ML;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ComboBox cboHostSelectorAddr;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.WebBrowser browserMessageDisp;
        private System.Windows.Forms.RichTextBox txtSelfTestResults;
        private System.Windows.Forms.ListView lstDevices;
        private System.Windows.Forms.ColumnHeader deviceName;
        private System.Windows.Forms.ColumnHeader hardwareName;
        private System.Windows.Forms.ColumnHeader deviceID;
        private System.Windows.Forms.ColumnHeader firmwareVers;
        private System.Windows.Forms.ColumnHeader manDate;
        private System.Windows.Forms.ColumnHeader bootRev;
        private System.Windows.Forms.ColumnHeader softStatus;
        private System.Windows.Forms.TabControl groupedControls;
        private System.Windows.Forms.Button blinkButton;
        private System.Windows.Forms.Button btnChangeDevId;
        private System.Windows.Forms.NumericUpDown numNewDevId;
        private System.Windows.Forms.Button btnNameChange;
        private System.Windows.Forms.TextBox txtDeviceNewName;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.ToolStripMenuItem menuItemTools;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelfTest;
        private System.Windows.Forms.Button btnSaveConfigs;
        private System.Windows.Forms.ImageList imgLstDevices;
        private System.Windows.Forms.ContextMenuStrip popupHttpLog;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader hardVers;
        private System.Windows.Forms.Button btnRevertBinaries;
        private System.Windows.Forms.Button btnUpdateBinaries;
        private System.Windows.Forms.RichTextBox rtbRioUpdateBox;
        private System.Windows.Forms.TabPage tbUnitTesting;
        private System.Windows.Forms.CheckedListBox unitTestingCheckboxes;
        private System.Windows.Forms.Button overnightTestButton;
        private System.Windows.Forms.RichTextBox rtbUnitTestBox;
        private System.Windows.Forms.Button btnStopUnitTest;
        private System.Windows.Forms.Button btnStopServer;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView gridDiagnosticLog;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnClrDiagLog;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboHostSelectorPrt;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmLineNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmUrl;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmResponse;
        private System.Windows.Forms.DataGridViewTextBoxColumn clmType;
        private System.Windows.Forms.Button btnPauseTelem;
        private System.Windows.Forms.Button btnJumpToBottom;
        private System.Windows.Forms.Panel pnlFirmUpgradeOuter;
        private System.Windows.Forms.Panel pnlFirmUpgradeInner;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtDeviceCRFPath;
        private System.Windows.Forms.Label lblDevFlashResults;
        private System.Windows.Forms.ProgressBar prgCanDev;
        private System.Windows.Forms.Button btnUpdateDevice;
        private System.Windows.Forms.Button btnFirmwareDialog;
        private System.Windows.Forms.Panel pnlGenDevConfigOuter;
        private System.Windows.Forms.Panel pnlGenDevConfigInner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panelSelfTestAndConfigControls;
        private System.Windows.Forms.ToolStripMenuItem selectAllCtrlAToolStripMenuItem;
        private System.Windows.Forms.Button btnRefreshDevices;
        private System.Windows.Forms.Panel pnlDeviceListInner;
        private System.Windows.Forms.ToolStripMenuItem captureAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataTransferMethodToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sFTPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pOSTToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnRefreshConfigs;
    }
}

