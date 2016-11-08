namespace 屏幕截图2005
{
    partial class FrmTips
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button_wait = new System.Windows.Forms.Button();
            this.button_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(607, 111);
            this.label1.TabIndex = 0;
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("宋体", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(607, 122);
            this.label2.TabIndex = 1;
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_wait
            // 
            this.button_wait.Location = new System.Drawing.Point(302, 294);
            this.button_wait.Name = "button_wait";
            this.button_wait.Size = new System.Drawing.Size(150, 47);
            this.button_wait.TabIndex = 2;
            this.button_wait.Text = "等一会再提醒我";
            this.button_wait.UseVisualStyleBackColor = true;
            this.button_wait.Click += new System.EventHandler(this.button_wait_Click);
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(469, 294);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(150, 47);
            this.button_ok.TabIndex = 2;
            this.button_ok.Text = "我知道了";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // FrmTips
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 378);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.button_wait);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmTips";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "定时提醒";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmTips_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_wait;
        private System.Windows.Forms.Button button_ok;
    }
}