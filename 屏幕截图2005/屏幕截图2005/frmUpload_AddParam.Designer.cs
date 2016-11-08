namespace 屏幕截图2005
{
    partial class frmUpload_AddParam
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.textBox_value = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "参数名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "参 数 值：";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(70, 6);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.Size = new System.Drawing.Size(429, 21);
            this.textBox_name.TabIndex = 2;
            // 
            // textBox_value
            // 
            this.textBox_value.Location = new System.Drawing.Point(70, 34);
            this.textBox_value.Multiline = true;
            this.textBox_value.Name = "textBox_value";
            this.textBox_value.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_value.Size = new System.Drawing.Size(429, 146);
            this.textBox_value.TabIndex = 3;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(315, 186);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(89, 27);
            this.button_ok.TabIndex = 4;
            this.button_ok.Text = "添 加";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(410, 186);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(89, 27);
            this.button_cancel.TabIndex = 4;
            this.button_cancel.Text = "完 成";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // frmUpload_AddParam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(511, 222);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_value);
            this.Controls.Add(this.textBox_name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpload_AddParam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "添加自定义参数";
            this.Load += new System.EventHandler(this.frmUpload_AddParam_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.TextBox textBox_value;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
    }
}