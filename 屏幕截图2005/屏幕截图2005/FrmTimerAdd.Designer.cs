namespace 屏幕截图2005
{
    partial class FrmTimerAdd
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(472, 248);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(125, 41);
            this.button_ok.TabIndex = 0;
            this.button_ok.Text = "确 定";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // FrmTimerAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 307);
            this.Controls.Add(this.button_ok);
            this.Name = "FrmTimerAdd";
            this.Text = "FrmTimerAdd";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_ok;
    }
}