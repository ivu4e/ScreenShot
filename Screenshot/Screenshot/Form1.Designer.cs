namespace Screenshot
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开保存目录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.录制Flash动画ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.录制GIFt动画ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.延时5秒截图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.截图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_Set = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.lbl_CutImage = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(333, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(73, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "关闭";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "屏幕截图工具";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开保存目录ToolStripMenuItem,
            this.录制Flash动画ToolStripMenuItem,
            this.录制GIFt动画ToolStripMenuItem,
            this.延时5秒截图ToolStripMenuItem,
            this.截图ToolStripMenuItem,
            this.tsmi_Set,
            this.toolStripMenuItem1,
            this.tsmi_exit});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 186);
            // 
            // 打开保存目录ToolStripMenuItem
            // 
            this.打开保存目录ToolStripMenuItem.Name = "打开保存目录ToolStripMenuItem";
            this.打开保存目录ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.打开保存目录ToolStripMenuItem.Text = "打开保存目录";
            // 
            // 录制Flash动画ToolStripMenuItem
            // 
            this.录制Flash动画ToolStripMenuItem.Name = "录制Flash动画ToolStripMenuItem";
            this.录制Flash动画ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.录制Flash动画ToolStripMenuItem.Text = "录制Flash动画";
            // 
            // 录制GIFt动画ToolStripMenuItem
            // 
            this.录制GIFt动画ToolStripMenuItem.Name = "录制GIFt动画ToolStripMenuItem";
            this.录制GIFt动画ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.录制GIFt动画ToolStripMenuItem.Text = "录制GIFt动画";
            // 
            // 延时5秒截图ToolStripMenuItem
            // 
            this.延时5秒截图ToolStripMenuItem.Name = "延时5秒截图ToolStripMenuItem";
            this.延时5秒截图ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.延时5秒截图ToolStripMenuItem.Text = "延时5秒截图";
            // 
            // 截图ToolStripMenuItem
            // 
            this.截图ToolStripMenuItem.Name = "截图ToolStripMenuItem";
            this.截图ToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.截图ToolStripMenuItem.Text = "截图";
            // 
            // tsmi_Set
            // 
            this.tsmi_Set.Name = "tsmi_Set";
            this.tsmi_Set.Size = new System.Drawing.Size(153, 22);
            this.tsmi_Set.Text = "设置";
            this.tsmi_Set.Click += new System.EventHandler(this.tsmi_Set_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 6);
            // 
            // tsmi_exit
            // 
            this.tsmi_exit.Name = "tsmi_exit";
            this.tsmi_exit.Size = new System.Drawing.Size(153, 22);
            this.tsmi_exit.Text = "退出";
            this.tsmi_exit.Click += new System.EventHandler(this.tsmi_exit_Click);
            // 
            // lbl_CutImage
            // 
            this.lbl_CutImage.BackColor = System.Drawing.Color.Black;
            this.lbl_CutImage.Location = new System.Drawing.Point(302, 144);
            this.lbl_CutImage.Name = "lbl_CutImage";
            this.lbl_CutImage.Size = new System.Drawing.Size(103, 90);
            this.lbl_CutImage.TabIndex = 1;
            this.lbl_CutImage.Paint += new System.Windows.Forms.PaintEventHandler(this.lbl_CutImage_Paint);
            this.lbl_CutImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_CutImage_MouseDown);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 294);
            this.Controls.Add(this.lbl_CutImage);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开保存目录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 录制Flash动画ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 录制GIFt动画ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 延时5秒截图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 截图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmi_Set;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem tsmi_exit;
        private System.Windows.Forms.Label lbl_CutImage;
    }
}

