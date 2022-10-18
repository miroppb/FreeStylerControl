namespace FSControl
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.BtnPowerOn = new System.Windows.Forms.Button();
            this.BtnPowerOff = new System.Windows.Forms.Button();
            this.BtnSundayStage = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TxtOutput = new System.Windows.Forms.TextBox();
            this.BtnToggleAll = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toggleAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sundayStageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerSchedule = new System.Windows.Forms.Timer(this.components);
            this.BtnChangeLights = new System.Windows.Forms.Button();
            this.CmbVariations = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnPowerOn
            // 
            this.BtnPowerOn.Location = new System.Drawing.Point(6, 22);
            this.BtnPowerOn.Name = "BtnPowerOn";
            this.BtnPowerOn.Size = new System.Drawing.Size(75, 23);
            this.BtnPowerOn.TabIndex = 0;
            this.BtnPowerOn.Text = "Power On";
            this.BtnPowerOn.UseVisualStyleBackColor = true;
            this.BtnPowerOn.Click += new System.EventHandler(this.BtnPowerOn_Click);
            // 
            // BtnPowerOff
            // 
            this.BtnPowerOff.Location = new System.Drawing.Point(87, 22);
            this.BtnPowerOff.Name = "BtnPowerOff";
            this.BtnPowerOff.Size = new System.Drawing.Size(75, 23);
            this.BtnPowerOff.TabIndex = 1;
            this.BtnPowerOff.Text = "Power Off";
            this.BtnPowerOff.UseVisualStyleBackColor = true;
            this.BtnPowerOff.Click += new System.EventHandler(this.BtnPowerOff_Click);
            // 
            // BtnSundayStage
            // 
            this.BtnSundayStage.Location = new System.Drawing.Point(271, 78);
            this.BtnSundayStage.Name = "BtnSundayStage";
            this.BtnSundayStage.Size = new System.Drawing.Size(97, 23);
            this.BtnSundayStage.TabIndex = 2;
            this.BtnSundayStage.Text = "Sunday Stage";
            this.BtnSundayStage.UseVisualStyleBackColor = true;
            this.BtnSundayStage.Click += new System.EventHandler(this.BtnSundayStage_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnPowerOn);
            this.groupBox1.Controls.Add(this.BtnPowerOff);
            this.groupBox1.Location = new System.Drawing.Point(190, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 60);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Power Controls";
            // 
            // TxtOutput
            // 
            this.TxtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TxtOutput.Location = new System.Drawing.Point(12, 135);
            this.TxtOutput.Multiline = true;
            this.TxtOutput.Name = "TxtOutput";
            this.TxtOutput.Size = new System.Drawing.Size(356, 145);
            this.TxtOutput.TabIndex = 4;
            // 
            // BtnToggleAll
            // 
            this.BtnToggleAll.Location = new System.Drawing.Point(12, 12);
            this.BtnToggleAll.Name = "BtnToggleAll";
            this.BtnToggleAll.Size = new System.Drawing.Size(97, 23);
            this.BtnToggleAll.TabIndex = 5;
            this.BtnToggleAll.Text = "Toggle All";
            this.BtnToggleAll.UseVisualStyleBackColor = true;
            this.BtnToggleAll.Click += new System.EventHandler(this.BtnToggleAll_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 36);
            this.label1.TabIndex = 6;
            this.label1.Text = "Use if clicking Power doesn\'t work correctly";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "FreeStyler Control";
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toggleAllToolStripMenuItem,
            this.powerOnToolStripMenuItem,
            this.powerOffToolStripMenuItem,
            this.sundayStageToolStripMenuItem,
            this.scheduleToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(146, 136);
            // 
            // toggleAllToolStripMenuItem
            // 
            this.toggleAllToolStripMenuItem.Name = "toggleAllToolStripMenuItem";
            this.toggleAllToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.toggleAllToolStripMenuItem.Text = "Toggle All";
            this.toggleAllToolStripMenuItem.Click += new System.EventHandler(this.toggleAllToolStripMenuItem_Click);
            // 
            // powerOnToolStripMenuItem
            // 
            this.powerOnToolStripMenuItem.Name = "powerOnToolStripMenuItem";
            this.powerOnToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.powerOnToolStripMenuItem.Text = "Power On";
            this.powerOnToolStripMenuItem.Click += new System.EventHandler(this.powerOnToolStripMenuItem_Click);
            // 
            // powerOffToolStripMenuItem
            // 
            this.powerOffToolStripMenuItem.Name = "powerOffToolStripMenuItem";
            this.powerOffToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.powerOffToolStripMenuItem.Text = "Power Off";
            this.powerOffToolStripMenuItem.Click += new System.EventHandler(this.powerOffToolStripMenuItem_Click);
            // 
            // sundayStageToolStripMenuItem
            // 
            this.sundayStageToolStripMenuItem.Name = "sundayStageToolStripMenuItem";
            this.sundayStageToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.sundayStageToolStripMenuItem.Text = "Sunday Stage";
            this.sundayStageToolStripMenuItem.Click += new System.EventHandler(this.sundayStageToolStripMenuItem_Click);
            // 
            // scheduleToolStripMenuItem
            // 
            this.scheduleToolStripMenuItem.Name = "scheduleToolStripMenuItem";
            this.scheduleToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.scheduleToolStripMenuItem.Text = "Schedule";
            this.scheduleToolStripMenuItem.Click += new System.EventHandler(this.scheduleToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // timerSchedule
            // 
            this.timerSchedule.Enabled = true;
            this.timerSchedule.Interval = 60000;
            this.timerSchedule.Tick += new System.EventHandler(this.timerSchedule_Tick);
            // 
            // BtnChangeLights
            // 
            this.BtnChangeLights.Location = new System.Drawing.Point(144, 106);
            this.BtnChangeLights.Name = "BtnChangeLights";
            this.BtnChangeLights.Size = new System.Drawing.Size(75, 23);
            this.BtnChangeLights.TabIndex = 7;
            this.BtnChangeLights.Text = "<- Change";
            this.BtnChangeLights.UseVisualStyleBackColor = true;
            this.BtnChangeLights.Click += new System.EventHandler(this.BtnChangeLights_Click);
            // 
            // CmbVariations
            // 
            this.CmbVariations.FormattingEnabled = true;
            this.CmbVariations.Items.AddRange(new object[] {
            "Blue-Purple",
            "Havest-Yellow"});
            this.CmbVariations.Location = new System.Drawing.Point(17, 106);
            this.CmbVariations.Name = "CmbVariations";
            this.CmbVariations.Size = new System.Drawing.Size(121, 23);
            this.CmbVariations.TabIndex = 8;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(381, 294);
            this.Controls.Add(this.CmbVariations);
            this.Controls.Add(this.BtnChangeLights);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnToggleAll);
            this.Controls.Add(this.TxtOutput);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.BtnSundayStage);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(397, 295);
            this.Name = "FrmMain";
            this.Text = "FreeStyler Control";
            this.Resize += new System.EventHandler(this.FrmMain_Resize);
            this.groupBox1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button BtnPowerOn;
        private Button BtnPowerOff;
        private Button BtnSundayStage;
        private GroupBox groupBox1;
        private Button BtnToggleAll;
        private Label label1;
        public TextBox TxtOutput;
        private NotifyIcon notifyIcon1;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toggleAllToolStripMenuItem;
        private ToolStripMenuItem powerOnToolStripMenuItem;
        private ToolStripMenuItem powerOffToolStripMenuItem;
        private ToolStripMenuItem sundayStageToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripMenuItem scheduleToolStripMenuItem;
        private System.Windows.Forms.Timer timerSchedule;
        private Button BtnChangeLights;
        private ComboBox CmbVariations;
    }
}