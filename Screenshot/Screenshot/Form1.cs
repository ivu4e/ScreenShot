using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Screenshot
{
    public partial class Form1 : Form
    {
        #region 热键及背景
        /// <summary>
        /// 向全局原子表添加一个字符串，并返回这个字符串的唯一标识符（原子ATOM）。
        /// </summary>
        /// <param name="lpString">自己设定的一个字符串</param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("Kernel32.dll")]
        public static extern Int32 GlobalAddAtom(string lpString);

        /// <summary>
        /// 注册热键
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <param name="fsModifiers"></param>
        /// <param name="vk"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, Keys vk);

        /// <summary>
        /// 取消热键注册
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// 热键ID
        /// </summary>
        public int hotKeyId = 100;

        /// <summary>
        /// 热键模式:0=Ctrl + Alt + A, 1=Ctrl + Shift + A
        /// </summary>
        public int HotKeyMode = 1;

        /// <summary>
        /// 控制键的类型
        /// </summary>
        public enum KeyModifiers : uint
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        /// <summary>
        /// 用于保存截取的整个屏幕的图片
        /// </summary>
        protected Bitmap screenImage;
        #endregion

        #region 截图基本变量
        /// <summary>
        /// 用于判断是否已经开始截图，控制信息框是否显示。
        /// </summary>
        private bool isCuting;
        /// <summary>
        /// 鼠标按下的点
        /// </summary>
        private Point beginPoint;
        /// <summary>
        /// 最终确定的绘图基点
        /// </summary>
        private Point endPoint;
        /// <summary>
        /// 用于记录截图显示区域的大小（包括调整块的区域，调整区域边框宽度2px）
        /// </summary>
        private Rectangle cutImageRect = new Rectangle(0, 0, 5, 5);
        /// <summary>
        /// 记录鼠标上一次移动的时间
        /// </summary>
        private long lastMouseMoveTime = System.DateTime.Now.Ticks;
        #endregion

        #region 基本设置参数
        /// <summary>
        /// 截图时是否显示截图信息栏
        /// </summary>
        public bool InfoBoxVisible = true;
        /// <summary>
        /// 截图时是否显示编辑工具栏
        /// </summary>
        public bool ToolBoxVisible = true;
        /// <summary>
        /// 截图中是否包含鼠标指针形状
        /// </summary>
        public bool IsCutCursor = true;
        /// <summary>
        /// 截图时是否显示放大镜
        /// </summary>
        public bool ZoomBoxVisible = true;
        /// <summary>
        /// 放大镜的尺寸——宽度
        /// </summary>
        private int ZoomBoxWidth = 120;

        public int ZoomBoxWidth1
        {
            get { return ZoomBoxWidth; }
            set { ZoomBoxWidth = value; }
        }
        /// <summary>
        /// 放大镜的尺寸——高度
        /// </summary>
        private int ZoomBoxHeight = 100;

        public int ZoomBoxHeight1
        {
            get { return ZoomBoxHeight; }
            set { ZoomBoxHeight = value; }
        }
        #endregion

        #region 图片上传参数
        public string PicDescFieldName = "pictitle";
        public string ImageFieldName = "upfile";
        public string PicDesc = "cutImage";
        public string UploadUrl = "http://";
        public bool DoUpload = false;
        #endregion

        #region 自动保存参数
        /// <summary>
        /// 是否自动保存到硬盘
        /// </summary>
        public bool AutoSaveToDisk = false;
        /// <summary>
        /// 自动保存目录
        /// </summary>
        public string AutoSaveDirectory = string.Empty;
        /// <summary>
        /// 是否启用日期格式“2013_02_22”的子目录
        /// </summary>
        public bool AutoSaveSubDir = false;
        /// <summary>
        /// 自动保存文件名前缀
        /// </summary>
        public string AutoSaveFileName1 = "屏幕截图";
        /// <summary>
        /// 自动文件名规则：日期时间，日期_序号，序号
        /// </summary>
        public string AutoSaveFileName2 = "日期时间";
        /// <summary>
        /// 自动保存文件格式：.png, .jpg, .jpeg, .gif, .bmp
        /// </summary>
        public string AutoSaveFileName3 = ".png";
        /// <summary>
        /// 自动保存文件名序号
        /// </summary>
        public int autoSaveFileIndex = 0;
        #endregion 自动保存参数

        

        public Form1()
        {
            InitializeComponent();
            // 解决窗口闪烁的问题
            SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = Properties.Resources.cutImage;
            this.notifyIcon1.Visible = true;
            //隐藏窗口
            this.Hide();

            //注册快捷键
            //注：HotKeyId的合法取之范围是0x0000到0xBFFF之间，GlobalAddAtom函数得到的值在0xC000到0xFFFF之间，所以减掉0xC000来满足调用要求。
            this.hotKeyId = GlobalAddAtom("Screenshot") - 0xC000;
            if (this.hotKeyId == 0)
            {
                //如果获取失败，设定一个默认值；
                this.hotKeyId = 0xBFFE; 
            }

            if (this.HotKeyMode == 0)
            {
                RegisterHotKey(Handle, hotKeyId, (uint)KeyModifiers.Control | (uint)KeyModifiers.Alt, Keys.A);
            }
            else
            {
                RegisterHotKey(Handle, hotKeyId, (uint)KeyModifiers.Control | (uint)KeyModifiers.Shift, Keys.A);
            }

            this.lbl_CutImage.Hide();
        }

        /// <summary>
        /// 截图窗口鼠标按下事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // 左键单击事件
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (!this.lbl_CutImage.Visible)
                {
                    this.isCuting = true;
                    this.beginPoint = e.Location;
                    this.endPoint = e.Location;
                    SaveCutImageSize(e.Location, e.Location);

                    UpdateCutInfoLabel(UpdateUIMode.ShowCutImage | UpdateUIMode.ShowInfoBox);
                }
            }
            // 左键双击事件
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                if (this.lbl_CutImage.Visible)
                {
                    ExecCutImage(false, false);
                }

            }
            // 右键单击事件
            if (e.Button == MouseButtons.Right)
            {
                ExitCutImage(!this.lbl_CutImage.Visible);
            }

        }

        /// <summary>
        /// 更新UI的模式，用于标记哪些需要显示，哪些需要隐藏；
        /// </summary>
        [FlagsAttribute]
        public enum UpdateUIMode : uint
        {
            //值得注意的是，如果要使用组合值，那么就不能用连续的数字表示，必须是几何级增长！
            None = 0,
            ShowTextPro = 1,
            ShowPenStyle = 2,
            ShowToolBox = 4,
            ShowInfoBox = 8,
            ShowZoomBox = 16,
            ShowCutImage = 32,
            HideTextPro = 64,
            HidePenStyle = 128,
            HideToolBox = 256,
            HideInfoBox = 512
        }

        /// <summary>
        /// 更新截图信息显示框，截图编辑工具框
        /// </summary>
        private void UpdateCutInfoLabel(UpdateUIMode updateUIMode) // UpdateUIMode updateUIMode = UpdateUIMode.None
        {
            //大于300毫秒或有组件显示或隐藏才进行重绘
            long mouseMoveTimeStep = System.DateTime.Now.Ticks - lastMouseMoveTime;
            if (mouseMoveTimeStep < 300 && updateUIMode == UpdateUIMode.None) { return; }
            lastMouseMoveTime = System.DateTime.Now.Ticks;

            if (this.lbl_CutImage.Visible || (updateUIMode & UpdateUIMode.ShowCutImage) != UpdateUIMode.None)
            {
                this.lbl_CutImage.SetBounds(this.cutImageRect.Left, this.cutImageRect.Top, this.cutImageRect.Width, this.cutImageRect.Height, BoundsSpecified.All);
                if (!this.lbl_CutImage.Visible)
                {
                    this.lbl_CutImage.Show();
                }
            }
        }

        /// <summary>
        /// 截图窗口鼠标抬起事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.isCuting)
                {
                    this.isCuting = false;

                    UpdateCutInfoLabel(UpdateUIMode.None);
                }
            }
        }

        /// <summary>
        /// 截图窗口鼠标移动事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            // 如果截取区域不可见,则退出处理过程
            if (!this.lbl_CutImage.Visible)
            {
                UpdateCutInfoLabel(UpdateUIMode.None);
                return;
            }

            Point pntBgn = this.beginPoint;
            Point pntEnd = e.Location;

            // 如果是反向拖动，重新设置起始点
            if (e.Location.X < this.beginPoint.X && e.Location.Y < this.beginPoint.Y)
            {
                pntBgn = e.Location;
                pntEnd = this.beginPoint;
            }
            else
            {
                if (e.Location.X < this.beginPoint.X)
                {
                    pntBgn = new Point(e.Location.X, this.beginPoint.Y);
                    pntEnd = new Point(this.beginPoint.X, e.Location.Y);
                }
                else
                {
                    if (e.Location.Y < this.beginPoint.Y)
                    {
                        pntBgn = new Point(this.beginPoint.X, e.Location.Y);
                        pntEnd = new Point(e.Location.X, this.beginPoint.Y);
                    }
                }
            }

            if (this.isCuting)
            {
                SaveCutImageSize(pntBgn, pntEnd);
            }

            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        /// <summary>
        /// 计算并保存截图的区域框的大小
        /// </summary>
        private void SaveCutImageSize(Point beginPoint, Point endPoint)
        {
            // 保存最终的绘图基点，用于截取选中的区域
            this.endPoint = beginPoint;

            // 计算截取图片的大小
            int imgWidth = Math.Abs(endPoint.X - beginPoint.X) + 1;
            int imgHeight = Math.Abs(endPoint.Y - beginPoint.Y) + 1;
            int lblWidth = imgWidth + 4;
            int lblHeight = imgHeight + 4;

            // 设置截图区域的位置和大小
            this.cutImageRect = new Rectangle(beginPoint.X - 2, beginPoint.Y - 2, lblWidth, lblHeight);
        }

        /// <summary>
        /// 执行截图,将选定区域的图片保存到剪贴板
        /// </summary>
        /// <param name="saveToDisk">
        /// 是否将图片保存到磁盘
        /// </param>
        private void ExecCutImage(bool saveToDisk, bool uploadImage) //bool saveToDisk = false, bool uploadImage = false
        {
            // 如果图片获取区域不可见,则退出保存图片过程
            if (!this.lbl_CutImage.Visible) { return; }
            Rectangle srcRect = new Rectangle();
            srcRect.X = this.lbl_CutImage.Location.X + 2;
            srcRect.Y = this.lbl_CutImage.Location.Y + 2;
            srcRect.Width = this.lbl_CutImage.Width - 4;
            srcRect.Height = this.lbl_CutImage.Height - 4;
            Rectangle destRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
            Bitmap bmp = new Bitmap(srcRect.Width, srcRect.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawImage(this.screenImage, destRect, srcRect, GraphicsUnit.Pixel);

            Clipboard.SetImage(bmp);

            ExitCutImage(true);
        }

        /// <summary>
        /// 退出截图过程
        /// </summary>
        private void ExitCutImage(bool hideWindow) //  = true
        {
            this.lbl_CutImage.Visible = false;
            this.isCuting = false;

            if (hideWindow)
            {
                this.screenImage.Dispose();
                this.Hide();
            }
        }


        /// <summary>
        /// 处理快捷键事件
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            //if (m.Msg == 0x0014)
            //{
            //    return; // 禁掉清除背景消息
            //}
            const int WM_HOTKEY = 0x0312;
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    ShowForm();
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }
        /// <summary>
        /// 如果窗口为可见状态，则隐藏窗口；
        /// 否则则显示窗口
        /// </summary>
        protected void ShowForm()
        {
            if (this.Visible)
            {
                this.Hide();
            }
            else
            {
                Bitmap bkImage = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
                Graphics g = Graphics.FromImage(bkImage);
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size, CopyPixelOperation.SourceCopy);
                screenImage = (Bitmap)bkImage.Clone();
                g.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.Gray)), Screen.PrimaryScreen.Bounds);
                this.BackgroundImage = bkImage;

                this.ShowInTaskbar = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Location = Screen.PrimaryScreen.Bounds.Location;

                this.WindowState = FormWindowState.Maximized;
                this.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 当窗口正在关闭时进行验证
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                e.Cancel = false;
                UnregisterHotKey(this.Handle, hotKeyId);
            }
            else
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void tsmi_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// 截取区域图片的绘制事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_CutImage_Paint(object sender, PaintEventArgs e)
        {
            int imgWidth = this.lbl_CutImage.Width - 4;
            int imgHeight = this.lbl_CutImage.Height - 4;
            if (imgWidth < 1) { imgWidth = 1; }
            if (imgHeight < 1) { imgHeight = 1; }

            // 创建缓存图像，先将要绘制的内容全部绘制到缓存中，最后再一次性绘制到 Label 上，
            // 这样可以提高性能，并且可以防止屏幕闪烁的问题
            Bitmap bmp_lbl = new Bitmap(this.lbl_CutImage.Width, this.lbl_CutImage.Height);
            Graphics g = Graphics.FromImage(bmp_lbl);

            // 将要截取的部分绘制到缓存
            Rectangle destRect = new Rectangle(2, 2, imgWidth, imgHeight);
            Point srcPoint = this.lbl_CutImage.Location;
            srcPoint.Offset(2, 2);
            Rectangle srcRect = new Rectangle(srcPoint, new System.Drawing.Size(imgWidth, imgHeight));
            g.DrawImage(this.screenImage, destRect, srcRect, GraphicsUnit.Pixel);

            SolidBrush brush = new SolidBrush(Color.FromArgb(10, 124, 202));
            Pen pen = new Pen(brush, 1.0F);

            //以下部分（边框和调整块）的绘制放在（编辑内容）的后面，是解决绘制编辑内容会覆盖（边框和调整块）的问题

            // 绘制边框外的区域，解决会被编辑内容覆盖的问题
            // 上边
            destRect = new Rectangle(0, 0, this.lbl_CutImage.Width, 2);
            srcPoint = this.lbl_CutImage.Location;
            //srcPoint.Offset(2, 2);
            srcRect = new Rectangle(srcPoint, new System.Drawing.Size(this.lbl_CutImage.Width, 2));
            g.DrawImage(this.BackgroundImage, destRect, srcRect, GraphicsUnit.Pixel);

            // 下边
            destRect = new Rectangle(0, this.lbl_CutImage.Height - 2, this.lbl_CutImage.Width, 2);
            srcPoint = this.lbl_CutImage.Location;
            srcPoint.Offset(0, this.lbl_CutImage.Height - 2);
            srcRect = new Rectangle(srcPoint, new System.Drawing.Size(this.lbl_CutImage.Width, 2));
            g.DrawImage(this.BackgroundImage, destRect, srcRect, GraphicsUnit.Pixel);

            // 左边
            destRect = new Rectangle(0, 2, 2, this.lbl_CutImage.Height - 4);
            srcPoint = this.lbl_CutImage.Location;
            srcPoint.Offset(0, 2);
            srcRect = new Rectangle(srcPoint, new System.Drawing.Size(2, this.lbl_CutImage.Height - 4));
            g.DrawImage(this.BackgroundImage, destRect, srcRect, GraphicsUnit.Pixel);

            // 右边
            destRect = new Rectangle(this.lbl_CutImage.Width - 2, 2, 2, this.lbl_CutImage.Height - 4);
            srcPoint = this.lbl_CutImage.Location;
            srcPoint.Offset(this.lbl_CutImage.Width - 2, 2);
            srcRect = new Rectangle(srcPoint, new System.Drawing.Size(2, this.lbl_CutImage.Height - 4));
            g.DrawImage(this.BackgroundImage, destRect, srcRect, GraphicsUnit.Pixel);

            // 绘制边框
            g.DrawLine(pen, 2, 2, this.lbl_CutImage.Width - 3, 2);
            g.DrawLine(pen, 2, 2, 2, this.lbl_CutImage.Height - 3);
            g.DrawLine(pen, this.lbl_CutImage.Width - 3, 2, this.lbl_CutImage.Width - 3, this.lbl_CutImage.Height - 3);
            g.DrawLine(pen, 2, this.lbl_CutImage.Height - 3, this.lbl_CutImage.Width - 3, this.lbl_CutImage.Height - 3);

            // 绘制四个角的调整块
            g.FillRectangle(brush, 0, 0, 4, 5);
            g.FillRectangle(brush, this.lbl_CutImage.Width - 4, 0, 4, 5);
            g.FillRectangle(brush, 0, this.lbl_CutImage.Height - 5, 4, 5);
            g.FillRectangle(brush, this.lbl_CutImage.Width - 4, this.lbl_CutImage.Height - 5, 4, 5);

            // 绘制中间的四个调整块
            int blockX = this.lbl_CutImage.Width / 2 - 2;
            int blockY = this.lbl_CutImage.Height / 2 - 2;
            g.FillRectangle(brush, blockX, 0, 4, 5);
            g.FillRectangle(brush, 0, blockY, 4, 5);
            g.FillRectangle(brush, blockX, this.lbl_CutImage.Height - 5, 4, 5);
            g.FillRectangle(brush, this.lbl_CutImage.Width - 4, blockY, 4, 5);

            // 绘制到 Label 上
            e.Graphics.DrawImage(bmp_lbl, 0, 0);
            bmp_lbl.Dispose();
        }

        /// <summary>
        /// 截取区域图片的鼠标按下事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_CutImage_MouseDown(object sender, MouseEventArgs e)
        {
            // 左键双击事件
            if (e.Button == MouseButtons.Left && e.Clicks == 2)
            {
                if (this.lbl_CutImage.Visible)
                {
                    ExecCutImage(false, false);
                }
            }
        }

        /// <summary>
        /// 托盘菜单设置事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_Set_Click(object sender, EventArgs e)
        {
            frmSetup frm = new frmSetup(this.Handle);
            frm.ShowDialog();
        }

    }
}
