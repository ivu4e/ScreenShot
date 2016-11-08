namespace Screenshot
{
    partial class frmSetup
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
            this.chb_subDir = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tb_zoomBoxHeight = new System.Windows.Forms.TextBox();
            this.tb_zoomBoxWidth = new System.Windows.Forms.TextBox();
            this.ckb_ZoomBox = new System.Windows.Forms.CheckBox();
            this.ckb_CutCursor = new System.Windows.Forms.CheckBox();
            this.ckb_ToolBox = new System.Windows.Forms.CheckBox();
            this.ckb_InfoBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBox_upload = new System.Windows.Forms.CheckBox();
            this.textBox_uploadUrl = new System.Windows.Forms.TextBox();
            this.textBox_desc = new System.Windows.Forms.TextBox();
            this.textBox_fieldFile = new System.Windows.Forms.TextBox();
            this.textBox_fieldDesc = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_exmple = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox_Extn = new System.Windows.Forms.ComboBox();
            this.comboBox_fileName2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_fileName1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button_browse = new System.Windows.Forms.Button();
            this.textBox_saveDir = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox_autoSave = new System.Windows.Forms.CheckBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // chb_subDir
            // 
            this.chb_subDir.AutoSize = true;
            this.chb_subDir.Location = new System.Drawing.Point(68, 64);
            this.chb_subDir.Name = "chb_subDir";
            this.chb_subDir.Size = new System.Drawing.Size(240, 16);
            this.chb_subDir.TabIndex = 13;
            this.chb_subDir.Text = "启用（按日期命名，格式：2013_02_22）";
            this.chb_subDir.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPage3);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(456, 200);
            this.tabControl.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(448, 174);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "基本设置";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox2);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Controls.Add(this.tb_zoomBoxHeight);
            this.groupBox2.Controls.Add(this.tb_zoomBoxWidth);
            this.groupBox2.Controls.Add(this.ckb_ZoomBox);
            this.groupBox2.Controls.Add(this.ckb_CutCursor);
            this.groupBox2.Controls.Add(this.ckb_ToolBox);
            this.groupBox2.Controls.Add(this.ckb_InfoBox);
            this.groupBox2.Location = new System.Drawing.Point(185, 17);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 140);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "截图选项";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::Screenshot.Properties.Resources.Lock;
            this.pictureBox2.Location = new System.Drawing.Point(179, 53);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(40, 27);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(117, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 18);
            this.label10.TabIndex = 7;
            this.label10.Text = "尺寸:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Screenshot.Properties.Resources.X;
            this.pictureBox1.Location = new System.Drawing.Point(189, 80);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // tb_zoomBoxHeight
            // 
            this.tb_zoomBoxHeight.Location = new System.Drawing.Point(208, 80);
            this.tb_zoomBoxHeight.Name = "tb_zoomBoxHeight";
            this.tb_zoomBoxHeight.Size = new System.Drawing.Size(35, 21);
            this.tb_zoomBoxHeight.TabIndex = 5;
            this.tb_zoomBoxHeight.Text = "100";
            this.tb_zoomBoxHeight.TextChanged += new System.EventHandler(this.tb_zoomBoxHeight_TextChanged);
            // 
            // tb_zoomBoxWidth
            // 
            this.tb_zoomBoxWidth.Location = new System.Drawing.Point(154, 80);
            this.tb_zoomBoxWidth.Name = "tb_zoomBoxWidth";
            this.tb_zoomBoxWidth.Size = new System.Drawing.Size(35, 21);
            this.tb_zoomBoxWidth.TabIndex = 4;
            this.tb_zoomBoxWidth.Text = "120";
            this.tb_zoomBoxWidth.TextChanged += new System.EventHandler(this.tb_zoomBoxWidth_TextChanged);
            // 
            // ckb_ZoomBox
            // 
            this.ckb_ZoomBox.AutoSize = true;
            this.ckb_ZoomBox.Checked = true;
            this.ckb_ZoomBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckb_ZoomBox.Location = new System.Drawing.Point(16, 80);
            this.ckb_ZoomBox.Name = "ckb_ZoomBox";
            this.ckb_ZoomBox.Size = new System.Drawing.Size(108, 16);
            this.ckb_ZoomBox.TabIndex = 3;
            this.ckb_ZoomBox.Text = "显示放大镜 — ";
            this.ckb_ZoomBox.UseVisualStyleBackColor = true;
            // 
            // ckb_CutCursor
            // 
            this.ckb_CutCursor.AutoSize = true;
            this.ckb_CutCursor.Checked = true;
            this.ckb_CutCursor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckb_CutCursor.Location = new System.Drawing.Point(16, 108);
            this.ckb_CutCursor.Name = "ckb_CutCursor";
            this.ckb_CutCursor.Size = new System.Drawing.Size(156, 16);
            this.ckb_CutCursor.TabIndex = 2;
            this.ckb_CutCursor.Text = "截图中包含鼠标指针形状";
            this.ckb_CutCursor.UseVisualStyleBackColor = true;
            // 
            // ckb_ToolBox
            // 
            this.ckb_ToolBox.AutoSize = true;
            this.ckb_ToolBox.Checked = true;
            this.ckb_ToolBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckb_ToolBox.Location = new System.Drawing.Point(16, 51);
            this.ckb_ToolBox.Name = "ckb_ToolBox";
            this.ckb_ToolBox.Size = new System.Drawing.Size(108, 16);
            this.ckb_ToolBox.TabIndex = 1;
            this.ckb_ToolBox.Text = "显示编辑工具栏";
            this.ckb_ToolBox.UseVisualStyleBackColor = true;
            // 
            // ckb_InfoBox
            // 
            this.ckb_InfoBox.AutoSize = true;
            this.ckb_InfoBox.Checked = true;
            this.ckb_InfoBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckb_InfoBox.Location = new System.Drawing.Point(16, 24);
            this.ckb_InfoBox.Name = "ckb_InfoBox";
            this.ckb_InfoBox.Size = new System.Drawing.Size(108, 16);
            this.ckb_InfoBox.TabIndex = 0;
            this.ckb_InfoBox.Text = "显示截图信息栏";
            this.ckb_InfoBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Location = new System.Drawing.Point(8, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 140);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "热键";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(12, 37);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(107, 16);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Ctrl + Alt + A";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(12, 82);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(119, 16);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Ctrl + Shift + A";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBox_upload);
            this.tabPage2.Controls.Add(this.textBox_uploadUrl);
            this.tabPage2.Controls.Add(this.textBox_desc);
            this.tabPage2.Controls.Add(this.textBox_fieldFile);
            this.tabPage2.Controls.Add(this.textBox_fieldDesc);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(448, 174);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "自动上传";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBox_upload
            // 
            this.checkBox_upload.AutoSize = true;
            this.checkBox_upload.Location = new System.Drawing.Point(86, 153);
            this.checkBox_upload.Name = "checkBox_upload";
            this.checkBox_upload.Size = new System.Drawing.Size(96, 16);
            this.checkBox_upload.TabIndex = 5;
            this.checkBox_upload.Text = "启用自动上传";
            this.checkBox_upload.UseVisualStyleBackColor = true;
            // 
            // textBox_uploadUrl
            // 
            this.textBox_uploadUrl.Location = new System.Drawing.Point(86, 115);
            this.textBox_uploadUrl.Name = "textBox_uploadUrl";
            this.textBox_uploadUrl.Size = new System.Drawing.Size(356, 21);
            this.textBox_uploadUrl.TabIndex = 4;
            this.textBox_uploadUrl.Text = "http://localhost/pms/Handler/FileUploadHandler.ashx";
            // 
            // textBox_desc
            // 
            this.textBox_desc.Location = new System.Drawing.Point(86, 83);
            this.textBox_desc.Name = "textBox_desc";
            this.textBox_desc.Size = new System.Drawing.Size(356, 21);
            this.textBox_desc.TabIndex = 4;
            // 
            // textBox_fieldFile
            // 
            this.textBox_fieldFile.Location = new System.Drawing.Point(128, 49);
            this.textBox_fieldFile.Name = "textBox_fieldFile";
            this.textBox_fieldFile.Size = new System.Drawing.Size(187, 21);
            this.textBox_fieldFile.TabIndex = 4;
            this.textBox_fieldFile.Text = "upfile";
            // 
            // textBox_fieldDesc
            // 
            this.textBox_fieldDesc.Location = new System.Drawing.Point(128, 14);
            this.textBox_fieldDesc.Name = "textBox_fieldDesc";
            this.textBox_fieldDesc.Size = new System.Drawing.Size(187, 21);
            this.textBox_fieldDesc.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "上传地址：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "图片描述：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "图片文件参数名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图片描述参数名称：";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chb_subDir);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.textBox_exmple);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.comboBox_Extn);
            this.tabPage3.Controls.Add(this.comboBox_fileName2);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.textBox_fileName1);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.button_browse);
            this.tabPage3.Controls.Add(this.textBox_saveDir);
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.checkBox_autoSave);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(448, 174);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "自动保存";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 65);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 12;
            this.label11.Text = "子目录：";
            // 
            // textBox_exmple
            // 
            this.textBox_exmple.Location = new System.Drawing.Point(68, 129);
            this.textBox_exmple.Name = "textBox_exmple";
            this.textBox_exmple.ReadOnly = true;
            this.textBox_exmple.Size = new System.Drawing.Size(312, 21);
            this.textBox_exmple.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(29, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 10;
            this.label9.Text = "示例：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(281, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "+";
            // 
            // comboBox_Extn
            // 
            this.comboBox_Extn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Extn.FormattingEnabled = true;
            this.comboBox_Extn.Items.AddRange(new object[] {
            ".png",
            ".jpg",
            ".gif",
            ".bmp",
            ".tif"});
            this.comboBox_Extn.Location = new System.Drawing.Point(293, 97);
            this.comboBox_Extn.Name = "comboBox_Extn";
            this.comboBox_Extn.Size = new System.Drawing.Size(87, 20);
            this.comboBox_Extn.TabIndex = 8;
            this.comboBox_Extn.SelectedIndexChanged += new System.EventHandler(this.comboBox_Extn_SelectedIndexChanged);
            // 
            // comboBox_fileName2
            // 
            this.comboBox_fileName2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_fileName2.FormattingEnabled = true;
            this.comboBox_fileName2.Items.AddRange(new object[] {
            "日期时间",
            "日期_序号",
            "序号"});
            this.comboBox_fileName2.Location = new System.Drawing.Point(166, 97);
            this.comboBox_fileName2.Name = "comboBox_fileName2";
            this.comboBox_fileName2.Size = new System.Drawing.Size(112, 20);
            this.comboBox_fileName2.TabIndex = 7;
            this.comboBox_fileName2.SelectedIndexChanged += new System.EventHandler(this.comboBox_fileName2_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(152, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(11, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "+";
            // 
            // textBox_fileName1
            // 
            this.textBox_fileName1.Location = new System.Drawing.Point(68, 97);
            this.textBox_fileName1.Name = "textBox_fileName1";
            this.textBox_fileName1.Size = new System.Drawing.Size(81, 21);
            this.textBox_fileName1.TabIndex = 5;
            this.textBox_fileName1.Text = "屏幕截图";
            this.textBox_fileName1.TextChanged += new System.EventHandler(this.textBox_fileName1_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 100);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "文件名称：";
            // 
            // button_browse
            // 
            this.button_browse.Location = new System.Drawing.Point(367, 30);
            this.button_browse.Name = "button_browse";
            this.button_browse.Size = new System.Drawing.Size(69, 23);
            this.button_browse.TabIndex = 3;
            this.button_browse.Text = "浏览...";
            this.button_browse.UseVisualStyleBackColor = true;
            this.button_browse.Click += new System.EventHandler(this.button_browse_Click);
            // 
            // textBox_saveDir
            // 
            this.textBox_saveDir.Location = new System.Drawing.Point(68, 31);
            this.textBox_saveDir.Name = "textBox_saveDir";
            this.textBox_saveDir.Size = new System.Drawing.Size(293, 21);
            this.textBox_saveDir.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "存储目录：";
            // 
            // checkBox_autoSave
            // 
            this.checkBox_autoSave.AutoSize = true;
            this.checkBox_autoSave.Checked = true;
            this.checkBox_autoSave.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_autoSave.Enabled = false;
            this.checkBox_autoSave.Location = new System.Drawing.Point(6, 6);
            this.checkBox_autoSave.Name = "checkBox_autoSave";
            this.checkBox_autoSave.Size = new System.Drawing.Size(156, 16);
            this.checkBox_autoSave.TabIndex = 0;
            this.checkBox_autoSave.Text = "自动保存屏幕截图到磁盘";
            this.checkBox_autoSave.UseVisualStyleBackColor = true;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(247, 205);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(91, 28);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "确 定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(355, 205);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(91, 28);
            this.button_cancel.TabIndex = 3;
            this.button_cancel.Text = "取 消";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // frmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 238);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_cancel);
            this.Name = "frmSetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.frmSetup_Load);
            this.tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chb_subDir;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tb_zoomBoxHeight;
        private System.Windows.Forms.TextBox tb_zoomBoxWidth;
        private System.Windows.Forms.CheckBox ckb_ZoomBox;
        private System.Windows.Forms.CheckBox ckb_CutCursor;
        private System.Windows.Forms.CheckBox ckb_ToolBox;
        private System.Windows.Forms.CheckBox ckb_InfoBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBox_upload;
        private System.Windows.Forms.TextBox textBox_uploadUrl;
        private System.Windows.Forms.TextBox textBox_desc;
        private System.Windows.Forms.TextBox textBox_fieldFile;
        private System.Windows.Forms.TextBox textBox_fieldDesc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_exmple;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox_Extn;
        private System.Windows.Forms.ComboBox comboBox_fileName2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_fileName1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button_browse;
        private System.Windows.Forms.TextBox textBox_saveDir;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox_autoSave;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
    }
}