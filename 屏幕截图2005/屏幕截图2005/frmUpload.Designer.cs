namespace 屏幕截图2005
{
    partial class frmUpload
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
            this.button_Start = new System.Windows.Forms.Button();
            this.button_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_parameters = new System.Windows.Forms.TextBox();
            this.button_add = new System.Windows.Forms.Button();
            this.button_clear = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_uploadURL = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_remoteUrl = new System.Windows.Forms.TextBox();
            this.label_status = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(374, 257);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(92, 28);
            this.button_Start.TabIndex = 0;
            this.button_Start.Text = "开始上传";
            this.button_Start.UseVisualStyleBackColor = true;
            this.button_Start.Click += new System.EventHandler(this.button_Start_Click);
            // 
            // button_Cancel
            // 
            this.button_Cancel.Location = new System.Drawing.Point(475, 257);
            this.button_Cancel.Name = "button_Cancel";
            this.button_Cancel.Size = new System.Drawing.Size(92, 28);
            this.button_Cancel.TabIndex = 0;
            this.button_Cancel.Text = "关 闭";
            this.button_Cancel.UseVisualStyleBackColor = true;
            this.button_Cancel.Click += new System.EventHandler(this.button_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "自定义上传参数：";
            // 
            // textBox_parameters
            // 
            this.textBox_parameters.Location = new System.Drawing.Point(14, 33);
            this.textBox_parameters.Multiline = true;
            this.textBox_parameters.Name = "textBox_parameters";
            this.textBox_parameters.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_parameters.Size = new System.Drawing.Size(553, 116);
            this.textBox_parameters.TabIndex = 2;
            // 
            // button_add
            // 
            this.button_add.Location = new System.Drawing.Point(378, 155);
            this.button_add.Name = "button_add";
            this.button_add.Size = new System.Drawing.Size(88, 25);
            this.button_add.TabIndex = 3;
            this.button_add.Text = "添加参数";
            this.button_add.UseVisualStyleBackColor = true;
            this.button_add.Click += new System.EventHandler(this.button_add_Click);
            // 
            // button_clear
            // 
            this.button_clear.Location = new System.Drawing.Point(479, 155);
            this.button_clear.Name = "button_clear";
            this.button_clear.Size = new System.Drawing.Size(88, 25);
            this.button_clear.TabIndex = 3;
            this.button_clear.Text = "清空参数";
            this.button_clear.UseVisualStyleBackColor = true;
            this.button_clear.Click += new System.EventHandler(this.button_clear_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "上传地址：";
            // 
            // textBox_uploadURL
            // 
            this.textBox_uploadURL.Location = new System.Drawing.Point(14, 189);
            this.textBox_uploadURL.Name = "textBox_uploadURL";
            this.textBox_uploadURL.Size = new System.Drawing.Size(552, 21);
            this.textBox_uploadURL.TabIndex = 8;
            this.textBox_uploadURL.Text = "http://localhost/pms/Handler/FileUploadHandler.ashx";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 222);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "远程地址：";
            // 
            // textBox_remoteUrl
            // 
            this.textBox_remoteUrl.Location = new System.Drawing.Point(73, 219);
            this.textBox_remoteUrl.Name = "textBox_remoteUrl";
            this.textBox_remoteUrl.ReadOnly = true;
            this.textBox_remoteUrl.Size = new System.Drawing.Size(493, 21);
            this.textBox_remoteUrl.TabIndex = 10;
            // 
            // label_status
            // 
            this.label_status.BackColor = System.Drawing.SystemColors.Info;
            this.label_status.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label_status.Location = new System.Drawing.Point(14, 258);
            this.label_status.Name = "label_status";
            this.label_status.Padding = new System.Windows.Forms.Padding(5);
            this.label_status.Size = new System.Drawing.Size(348, 27);
            this.label_status.TabIndex = 11;
            // 
            // frmUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 297);
            this.Controls.Add(this.label_status);
            this.Controls.Add(this.textBox_remoteUrl);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_uploadURL);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_clear);
            this.Controls.Add(this.button_add);
            this.Controls.Add(this.textBox_parameters);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_Cancel);
            this.Controls.Add(this.button_Start);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpload";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "上传文件";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmUpload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Cancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_parameters;
        private System.Windows.Forms.Button button_add;
        private System.Windows.Forms.Button button_clear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_uploadURL;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_remoteUrl;
        private System.Windows.Forms.Label label_status;
    }
}