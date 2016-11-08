using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;
using System.Threading;

namespace 屏幕截图2005
{
    /// <summary>
    /// 屏幕截图主窗体
    /// </summary>
    public partial class Form1 : Form
    {

        #region DLLImport
        [StructLayout(LayoutKind.Sequential)]
        struct CURSORINFO
        {
            public int cbSize;
            public int flags;
            public IntPtr hCursor;
            public Point ptScreenPos;
        }

        public struct IconInfo
        {
            public bool fIcon;
            public int xHotspot;
            public int yHotspot;
            public IntPtr hbmMask;
            public IntPtr hbmColor;
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorInfo(out CURSORINFO pci);

        private const int CURSOR_SHOWING = 0x00000001;

        [DllImport("user32.dll")]
        private static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo icon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref IconInfo pIconInfo);



        #endregion

        #region prammater
        /// <summary>
        /// 用于判断是否已经开始截图，控制信息框是否显示。
        /// </summary>
        private bool isCuting;
        /// <summary>
        /// 是否为录制模式
        /// </summary>
        private bool isRecordMode;
        /// <summary>
        /// 当前的录制模式
        /// </summary>
        private RecordMode recordMode;
        /// <summary>
        /// 鼠标按下的点
        /// </summary>
        private Point beginPoint;
        /// <summary>
        /// 最终确定的绘图基点
        /// </summary>
        private Point endPoint;
        /// <summary>
        /// 截图区域的大小
        /// </summary>
        private Size areaSize;
        /// <summary>
        /// 用于截取的屏幕图片
        /// </summary>
        protected Bitmap screenImage;
        /// <summary>
        /// 截屏区域信息框的显示位置
        /// </summary>
        private Point infoLocation;
        /// <summary>
        /// 鼠标按下的位置
        /// </summary>
        private Point downPoint;
        /// <summary>
        /// 鼠标抬起的位置
        /// </summary>
        private Point upPoint;
        /// <summary>
        /// 鼠标所在的点的颜色
        /// </summary>
        private Color cRGB;
        /// <summary>
        /// 记录鼠标状态，用于截图区域的移动、放大、缩小。
        /// </summary>
        private CursorType cursorType = CursorType.None;
        /// <summary>
        /// 鼠标上一次移动的时间
        /// </summary>
        private long lastMouseMoveTime = System.DateTime.Now.Ticks;
        /// <summary>
        /// 用于记录截图显示区域的大小（包括调整块的区域，调整区域边框宽度2px）
        /// </summary>
        private Rectangle cutImageRect = new Rectangle(0, 0, 5, 5);
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
        public static int hotKeyId = 100;
        /*
        #define MOD_ALT         0x0001
        #define MOD_CONTROL     0x0002
        #define MOD_SHIFT       0x0004
        #define MOD_WIN         0x0008
        */
        #endregion

        #region 截图设置
        /// <summary>
        /// 热键模式:0=Ctrl + Alt + A, 1=Ctrl + Shift + A
        /// </summary>
        public int HotKeyMode = 1;

        //图片上传参数
        public string PicDescFieldName = "pictitle";
        public string ImageFieldName = "upfile";
        public string PicDesc = "cutImage";
        public string UploadUrl = "http://";
        public bool DoUpload = false;

        #region 自动保存参数
        /// <summary>
        /// 是否自动保存到硬盘
        /// </summary>
        public bool AutoSaveToDisk = true;
        /// <summary>
        /// 自动保存目录
        /// </summary>
        public string AutoSaveDirectory = string.Empty;
        /// <summary>
        /// 是否启用日期格式“2013_02_22”的子目录
        /// </summary>
        public bool AutoSaveSubDir = true;
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
        private int ZoomBoxWidth = 122;

        public int ZoomBoxWidth1
        {
            get { return ZoomBoxWidth; }
            set { ZoomBoxWidth = value; }
        }
        /// <summary>
        /// 放大镜的尺寸——高度
        /// </summary>
        private int ZoomBoxHeight = 122;

        public int ZoomBoxHeight1
        {
            get { return ZoomBoxHeight; }
            set { ZoomBoxHeight = value; }
        }

        #endregion

        #region ImageEditParammter
        private Point mouseInToolBoxLocation;
        private Point mouseDownToolBoxLocation;
        private ImageEditMode imageEditMode = ImageEditMode.None;
        private ImageSubEditMode imageSubEditMode = ImageSubEditMode.Rectangle;
        private int toolHeight = 20;

        private bool IsEditing = false;
        private Point mouseLocation;
        private Rectangle editDrawRect;
        private Rectangle editSelectRect;
        private System.Collections.ArrayList pointList = new System.Collections.ArrayList();
        private System.Collections.ArrayList imageEditList = new System.Collections.ArrayList();

        System.Windows.Forms.Cursor cursorCross = null;
        System.Windows.Forms.Cursor cursorDefault = null;
        System.Windows.Forms.Cursor cursorText = null;
        System.Windows.Forms.Cursor cursorColor = null;

        #endregion

        #region 运行时样式记录
        //矩形
        private ToolsRuntimeStyle trsRectangle = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Red, 2, "solid");
        //圆角矩形
        private ToolsRuntimeStyle trsCircular = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Red, 2, "solid");
        //椭圆
        private ToolsRuntimeStyle trsEllipse = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Red, 2, "solid");
        //箭头
        private ToolsRuntimeStyle trsArrowhead = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Red, 5, "solid");
        //画刷
        private ToolsRuntimeStyle trsBrush = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Red, 15, "solid");
        //多边形
        private ToolsRuntimeStyle trsPolygon = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Red, 2, "solid");
        //L形
        private ToolsRuntimeStyle trsLShapeTool = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Red, 2, "solid");
        //提示框
        private ToolsRuntimeStyle trsToolTips = new ToolsRuntimeStyle(Color.White, "solid", Color.Red, 2, "solid");

        private ToolsRuntimeStyle currentToolStyle = new ToolsRuntimeStyle(Color.Transparent, "solid", Color.Transparent, 1, "solid");
        /// <summary>
        /// 记录编辑文字的样式
        /// </summary>
        private EditTextFont editTextFont = new EditTextFont();
        #endregion

        #region 查找所有可见窗口
        /// <summary>
        /// 该函数返回桌面窗口的句柄。桌面窗口覆盖整个屏幕。桌面窗口是一个要在其上绘制所有的图标和其他窗口的区域。
        /// 【说明】获得代表整个屏幕的一个窗口（桌面窗口）句柄.
        /// </summary>
        /// <returns>返回值：函数返回桌面窗口的句柄。</returns>
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr GetDesktopWindow();

        /// <summary>
        /// 该函数将指定窗口的标题条文本（如果存在）拷贝到一个缓存区内。
        /// 如果指定的窗口是一个控件，则拷贝控件的文本。
        /// 但是，GetWindowText不能接收其他应用程序中控件的文本。
        /// 函数原型：Int GetWindowText(HWND hWnd,LPTSTR lpString,Int nMaxCount);
        /// </summary>
        /// <param name="hWnd">带文本的窗口或控件的句柄。</param>
        /// <param name="lpString">指向接收文本的缓冲区的指针。</param>
        /// <param name="nMaxCount">指定要保存在缓冲区内的字符的最大个数，其中包含NULL字符。如果文本超过界限，它就被截断。</param>
        /// <returns>如果函数成功，返回值是拷贝的字符串的字符个数，不包括中断的空字符；
        /// 如果窗口无标题栏或文本，或标题栏为空，或窗口或控制的句柄无效，则返回值为零。
        /// 若想获得更多错误信息，请调用GetLastError函数。
        /// 函数不能返回在其他应用程序中的编辑控件的文本。</returns>
        [DllImport("user32.dll", EntryPoint = "GetWindowText", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 该函数获得指定窗口所属的类的类名。
        /// 函数原型:int GetClassName(HWND hWnd, LPTSTR IpClassName, int nMaxCount)；
        /// </summary>
        /// <param name="hWnd">窗口的句柄及间接给出的窗口所属的类。</param>
        /// <param name="lpClassName">指向接收窗口类名字符串的缓冲区的指针。</param>
        /// <param name="nMaxCount">指定由参数lpClassName指示的缓冲区的字节数。如果类名字符串大于缓冲区的长度，则多出的部分被截断。</param>
        /// <returns>如果函数成功，返回值为拷贝到指定缓冲区的字符个数：如果函数失败，返回值为0。若想获得更多错误信息，请调用GetLastError函数。</returns>
        [DllImport("user32.dll", EntryPoint = "GetClassName", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetClassName(IntPtr hWnd, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        /// WINDOWINFO结构，用于保存窗口信息。
        /// </summary>
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            /// DWORD->unsigned int
            public uint cbSize;

            /// RECT->tagRECT
            public tagRECT rcWindow;

            /// RECT->tagRECT
            public tagRECT rcClient;

            /// DWORD->unsigned int
            public uint dwStyle;

            /// DWORD->unsigned int
            public uint dwExStyle;

            /// DWORD->unsigned int
            public uint dwWindowStatus;

            /// UINT->unsigned int
            public uint cxWindowBorders;

            /// UINT->unsigned int
            public uint cyWindowBorders;

            /// ATOM->WORD->unsigned short
            public ushort atomWindowType;

            /// WORD->unsigned short
            public ushort wCreatorVersion;
        }

        /// <summary>
        /// RECT结构，用于保存窗口或客户区的位置及大小。
        /// </summary>
        [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct tagRECT
        {
            /// LONG->int
            public int left;

            /// LONG->int
            public int top;

            /// LONG->int
            public int right;

            /// LONG->int
            public int bottom;
        }

        /// <summary>
        /// 检索有关指定窗口的信息。
        /// </summary>
        /// <param name="hWnd">要检索信息的窗口的句柄。</param>
        /// <param name="pwi">指向一个接收信息的 PWINDOWINFO 结构，注意，在调用该函数之前必须设置 cbSize 成员为sizeof(WINDOWINFO)。</param>
        /// <returns>
        /// 如果函数成功，则返回值为非零值。
        /// 如果该函数失败，则返回值为零。要获取扩展的错误的信息，请调用GetLastError .
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hWnd, out WINDOWINFO pwi);

        /// <summary>
        /// 该函数返回与指定窗口有特定关系（如Z序或所有者）的窗口句柄。
        /// 函数原型：HWND GetWindow（HWND hWnd，UNIT nCmd）；
        /// </summary>
        /// <param name="hWnd">窗口句柄。要获得的窗口句柄是依据nCmd参数值相对于这个窗口的句柄。</param>
        /// <param name="uCmd">说明指定窗口与要获得句柄的窗口之间的关系。该参数值参考GetWindowCmd枚举。</param>
        /// <returns>返回值：如果函数成功，返回值为窗口句柄；如果与指定窗口有特定关系的窗口不存在，则返回值为NULL。
        /// 若想获得更多错误信息，请调用GetLastError函数。
        /// 备注：在循环体中调用函数EnumChildWindow比调用GetWindow函数可靠。调用GetWindow函数实现该任务的应用程序可能会陷入死循环或退回一个已被销毁的窗口句柄。
        /// 速查：Windows NT：3.1以上版本；Windows：95以上版本；Windows CE：1.0以上版本；头文件：winuser.h；库文件：user32.lib。
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, GetWindowCmd uCmd);

        /// <summary>
        /// 窗口与要获得句柄的窗口之间的关系。
        /// </summary>
        enum GetWindowCmd : uint
        {
            /// <summary>
            /// 返回的句柄标识了在Z序最高端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在Z序最高端的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最高端的顶层窗口：
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最高端的同属窗口。
            /// </summary>
            GW_HWNDFIRST = 0,
            /// <summary>
            /// 返回的句柄标识了在z序最低端的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该柄标识了在z序最低端的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在z序最低端的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在Z序最低端的同属窗口。
            /// </summary>
            GW_HWNDLAST = 1,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口下的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口下的最高端窗口：
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口下的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口下的同属窗口。
            /// </summary>
            GW_HWNDNEXT = 2,
            /// <summary>
            /// 返回的句柄标识了在Z序中指定窗口上的相同类型的窗口。
            /// 如果指定窗口是最高端窗口，则该句柄标识了在指定窗口上的最高端窗口；
            /// 如果指定窗口是顶层窗口，则该句柄标识了在指定窗口上的顶层窗口；
            /// 如果指定窗口是子窗口，则句柄标识了在指定窗口上的同属窗口。
            /// </summary>
            GW_HWNDPREV = 3,
            /// <summary>
            /// 返回的句柄标识了指定窗口的所有者窗口（如果存在）。
            /// GW_OWNER与GW_CHILD不是相对的参数，没有父窗口的含义，如果想得到父窗口请使用GetParent()。
            /// 例如：例如有时对话框的控件的GW_OWNER，是不存在的。
            /// </summary>
            GW_OWNER = 4,
            /// <summary>
            /// 如果指定窗口是父窗口，则获得的是在Tab序顶端的子窗口的句柄，否则为NULL。
            /// 函数仅检查指定父窗口的子窗口，不检查继承窗口。
            /// </summary>
            GW_CHILD = 5,
            /// <summary>
            /// （WindowsNT 5.0）返回的句柄标识了属于指定窗口的处于使能状态弹出式窗口（检索使用第一个由GW_HWNDNEXT 查找到的满足前述条件的窗口）；
            /// 如果无使能窗口，则获得的句柄与指定窗口相同。
            /// </summary>
            GW_ENABLEDPOPUP = 6
        }

        /*GetWindowCmd指定结果窗口与源窗口的关系，它们建立在下述常数基础上：
              GW_CHILD
              寻找源窗口的第一个子窗口
              GW_HWNDFIRST
              为一个源子窗口寻找第一个兄弟（同级）窗口，或寻找第一个顶级窗口
              GW_HWNDLAST
              为一个源子窗口寻找最后一个兄弟（同级）窗口，或寻找最后一个顶级窗口
              GW_HWNDNEXT
              为源窗口寻找下一个兄弟窗口
              GW_HWNDPREV
              为源窗口寻找前一个兄弟窗口
              GW_OWNER
              寻找窗口的所有者
         */

        /// <summary>
        /// 窗口样式
        /// </summary>
        [FlagsAttribute] /* 指示可以将枚举作为位域（即一组标志）处理, 只有添加此属性才能使用&与运算进行判断。*/
        public enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,

            //Extended Window Styles

            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020,

            //#if(WINVER >= 0x0400)

            WS_EX_MDICHILD = 0x00000040,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_CONTEXTHELP = 0x00000400,

            WS_EX_RIGHT = 0x00001000,
            WS_EX_LEFT = 0x00000000,
            WS_EX_RTLREADING = 0x00002000,
            WS_EX_LTRREADING = 0x00000000,
            WS_EX_LEFTSCROLLBAR = 0x00004000,
            WS_EX_RIGHTSCROLLBAR = 0x00000000,

            WS_EX_CONTROLPARENT = 0x00010000,
            WS_EX_STATICEDGE = 0x00020000,
            WS_EX_APPWINDOW = 0x00040000,

            WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
            WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),

            //#endif /* WINVER >= 0x0400 */

            //#if(WIN32WINNT >= 0x0500)

            WS_EX_LAYERED = 0x00080000,

            //#endif /* WIN32WINNT >= 0x0500 */

            //#if(WINVER >= 0x0500)

            WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
            WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring

            //#endif /* WINVER >= 0x0500 */

            //#if(WIN32WINNT >= 0x0500)

            WS_EX_COMPOSITED = 0x02000000,
            WS_EX_NOACTIVATE = 0x08000000

            //#endif /* WIN32WINNT >= 0x0500 */

        }

        /// <summary>
        /// 可见窗体信息结构
        /// </summary>
        public struct VisibleWindow
        {
            public Rectangle rect;
            public WINDOWINFO info;

            public VisibleWindow(Rectangle rectWindow, WINDOWINFO windowInfo)
            {
                rect = rectWindow;
                info = windowInfo;
            }
        }

        /// <summary>
        /// 用于保存可见窗口的区域列表，用于窗口自动发现功能
        /// 在显示主窗口之前保存该列表
        /// </summary>
        List<VisibleWindow> visibleWinList = new List<VisibleWindow>();

        /// <summary>
        /// 当前鼠标所处位置的窗口区域信息
        /// </summary>
        Rectangle rect_WindowFromPoint = Rectangle.Empty;

        /// <summary>
        /// 记录鼠标按下的时间，用于延迟响应MouseMove事件
        /// 主要是延迟清除rect_WindowFromPoint的值
        /// </summary>
        long mouseDownTimeTick = 0;

        /// <summary>
        /// 保存任务栏的窗口区域，用于其它窗口比较大，但显示在任务栏之下的情况
        /// 目前发现有些问题的程序是VS2010
        /// </summary>
        Rectangle rect_Shell_TrayWnd = Rectangle.Empty;

        #endregion

        /// <summary>
        /// 窗口构造函数
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            // 解决窗口闪烁的问题
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            Form1_Init();
            Load_Config();

            ExceptionLog.Log("Form1()");
        }
        /// <summary>
        /// 窗口加载事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.lbl_Info.Visible = false;
            this.lbl_CutImage.Visible = false;
            this.lbl_ToolBox.Size = new Size(Properties.Resources.ToolsBox.Size.Width, 26);
            this.lbl_ToolBox.Visible = false;

            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = Properties.Resources.cutImage;
            this.notifyIcon1.Visible = true;
            this.Hide();

            // 启动时首先显示设置对话框
            //frmSetup frms = new frmSetup(this.Handle);
            //frms.ShowDialog();

            if (this.HotKeyMode == 0)
            {
                RegisterHotKey(Handle, hotKeyId, (uint)KeyModifiers.Control | (uint)KeyModifiers.Alt, Keys.A);
            }
            else
            {
                RegisterHotKey(Handle, hotKeyId, (uint)KeyModifiers.Control | (uint)KeyModifiers.Shift, Keys.A);
            }

            // 填充字体下拉框
            for (int i = 0; i < FontFamily.Families.Length; i++)
            {
                this.cbb_FontFamily.Items.Add(FontFamily.Families[i].Name);
            }
            this.cbb_FontFamily.SelectedItem = "宋体";
            this.cbb_FontSize.SelectedIndex = 4;
            this.lbl_TextProColor.BackColor = Color.Red;

            for (int i = 1; i < 101; i++)
            {
                this.cbb_penSize.Items.Add(new ComboBoxItem(i.ToString(), i.ToString()));
            }
            this.cbb_penSize.SelectedIndex = 1;
            this.cbb_penStyle.Items.Add("实线");
            this.cbb_penStyle.SelectedIndex = 0;
            this.lbl_penColor.BackColor = Color.Red;

            // 填充填充类别下拉框
            this.cbb_fillStyle.Items.Add(new ComboBoxItem("无", ""));
            this.cbb_fillStyle.Items.Add(new ComboBoxItem("实心", "1"));
            this.cbb_fillStyle.Items.Add(new ComboBoxItem("渐变", "2"));
            this.cbb_fillStyle.Items.Add(new ComboBoxItem("图案", "3"));
            this.cbb_fillStyle.SelectedIndex = 0;

            //设置放大镜的大小
            this.pictureBox_zoom.Width = this.ZoomBoxWidth;
            this.pictureBox_zoom.Height = this.ZoomBoxHeight;

            this.toolTip1.SetToolTip(this.lbl_fillColor, "填充颜色");
            this.toolTip1.SetToolTip(this.cbb_fillStyle, "填充类别");
            this.toolTip1.SetToolTip(this.lbl_penColor, "画笔颜色");
            this.toolTip1.SetToolTip(this.cbb_penSize, "笔尖大小");
            this.toolTip1.SetToolTip(this.cbb_penStyle, "描边种类");

            this.toolTip1.SetToolTip(this.cbb_FontFamily, "字体");
            this.toolTip1.SetToolTip(this.cbb_FontSize, "字号");
            this.toolTip1.SetToolTip(this.lbl_TextProColor, "颜色");
            this.toolTip1.SetToolTip(this.lbl_fontBold, "粗体");
            this.toolTip1.SetToolTip(this.lbl_fontItalic, "斜体");
            this.toolTip1.SetToolTip(this.lbl_fontUnderline, "下划线");

            try
            {
                //安装鼠标钩子
                MouseHook mouseHook = new MouseHook();
                mouseHook.OnMouseActivity += new MouseEventHandler(FindWords.mouseHook_OnMouseActivity);
                mouseHook.Start();
            }
            catch (System.Exception ex)
            {
                ExceptionLog.Log(ex);
                Application.DoEvents();
            }

            try
            {
                //安装键盘钩子
                KeyboardHook keyboardHook = new KeyboardHook();
                keyboardHook.OnKeyDownEvent += new KeyEventHandler(FindWords.keyboardHook_OnKeyDownEvent);
                keyboardHook.OnKeyUpEvent += new KeyEventHandler(FindWords.keyboardHook_OnKeyUpEvent);
                keyboardHook.Start();
            }
            catch (System.Exception ex)
            {
                ExceptionLog.Log(ex);
                Application.DoEvents();
            }
        }
        /// <summary>
        /// 窗口初始化事件处理程序
        /// </summary>
        private void Form1_Init()
        {
            this.isCuting = false;
            this.isRecordMode = false;
            this.beginPoint = new Point(0, 0);
            this.endPoint = new Point(0, 0);
            this.areaSize = new Size();
            this.infoLocation = new Point();
            this.downPoint = new Point();
            this.upPoint = new Point();
            this.cRGB = Color.Transparent;
            this.mouseInToolBoxLocation = new Point();
            this.mouseDownToolBoxLocation = new Point();

            cursorDefault = getCursorFromResource(Properties.Resources.Cursor_Default);
            cursorCross = getCursorFromResource(Properties.Resources.Cursor_Cross);
            cursorText = getCursorFromResource(Properties.Resources.Cursor_Text);
            cursorColor = getCursorFromResource(Properties.Resources.Cursor_Color);

            //根据Debug模式，确定是否显示Debug信息输出框
            //this.tb_debug.MouseDown += new MouseEventHandler(DebugTextBox.onMouseDown);
            //this.tb_debug.KeyDown += new KeyEventHandler(DebugTextBox.onKeyDown);
            //this.tb_debug.GotFocus += new EventHandler(DebugTextBox.onGotFocus);
            //DebugTextBox.debugTextBox = this.tb_debug;

            if (!Program.debugMode)
            {
                //this.tb_debug.Hide();
            }

            try
            {
                Program.timerThread = new Thread(new ThreadStart(Program.TimerThreadStart));
                Program.timerThread.Start();
            }
            catch (System.Exception ex)
            {
                ExceptionLog.Log(ex);
            }
        }

        /// <summary>
        /// 从配置文件加载配置信息
        /// </summary>
        private void Load_Config()
        {
            //热键注册
            string strHotKeyMode = GetConfigAppSetting(AppSettingKeys.HotKeyMode);
            if (!string.IsNullOrEmpty(strHotKeyMode) && strHotKeyMode == "0")
            {
                this.HotKeyMode = 0;
            }
            else
            {
                this.HotKeyMode = 1;
            }

            string strInfoBoxVisible = GetConfigAppSetting(AppSettingKeys.InfoBoxVisible);
            if (!string.IsNullOrEmpty(strInfoBoxVisible) && strInfoBoxVisible == "0")
            {
                this.InfoBoxVisible = false;
            }
            else
            {
                this.InfoBoxVisible = true;
            }

            // 截图时是否显示编辑工具栏
            string strToolBoxVisible = GetConfigAppSetting(AppSettingKeys.ToolBoxVisible);
            if (!string.IsNullOrEmpty(strToolBoxVisible) && strToolBoxVisible == "0")
            {
                this.ToolBoxVisible = false;
            }
            else
            {
                this.ToolBoxVisible = true;
            }
            // 截图中是否包含鼠标指针形状
            string strIsCutCursor = GetConfigAppSetting(AppSettingKeys.IsCutCursor);
            if (!string.IsNullOrEmpty(strIsCutCursor) && strIsCutCursor == "0")
            {
                this.IsCutCursor = false;
            }
            else
            {
                this.IsCutCursor = true;
            }
            // 截图时是否显示放大镜
            string strZoomBoxVisible = GetConfigAppSetting(AppSettingKeys.ZoomBoxVisible);
            if (!string.IsNullOrEmpty(strZoomBoxVisible) && strZoomBoxVisible == "0")
            {
                this.ZoomBoxVisible = false;
            }
            else
            {
                this.ZoomBoxVisible = true;
            }
            //放大镜宽度
            string strZoomBoxWidth = GetConfigAppSetting(AppSettingKeys.ZoomBoxWidth);
            if (!string.IsNullOrEmpty(strZoomBoxWidth) && strZoomBoxWidth != "0")
            {
                this.ZoomBoxWidth = Convert.ToInt32(strZoomBoxWidth);
                if (this.ZoomBoxWidth < 122)
                {
                    this.ZoomBoxWidth = 122;
                }
            }
            else
            {
                this.ZoomBoxWidth = 122;
            }
            //放大镜高度
            string strZoomBoxHeight = GetConfigAppSetting(AppSettingKeys.ZoomBoxHeight);
            if (!string.IsNullOrEmpty(strZoomBoxHeight) && strZoomBoxHeight != "0")
            {
                this.ZoomBoxHeight = Convert.ToInt32(strZoomBoxHeight);
                if (this.ZoomBoxHeight < 122)
                {
                    this.ZoomBoxHeight = 122;
                }
            }
            else
            {
                this.ZoomBoxHeight = 122;
            }

            //图片上传参数
            this.PicDescFieldName = GetConfigAppSetting(AppSettingKeys.PicDescFieldName);
            if (string.IsNullOrEmpty(this.PicDescFieldName))
            {
                PicDescFieldName = "pictitle";
            }

            this.ImageFieldName = GetConfigAppSetting(AppSettingKeys.ImageFieldName);
            if (string.IsNullOrEmpty(this.ImageFieldName))
            {
                this.ImageFieldName = "upfile";
            }

            this.PicDesc = GetConfigAppSetting(AppSettingKeys.PicDesc);
            if (string.IsNullOrEmpty(this.PicDesc))
            {
                this.PicDesc = "cutImage";

            }

            this.UploadUrl = GetConfigAppSetting(AppSettingKeys.UploadUrl);
            if (string.IsNullOrEmpty(this.UploadUrl))
            {
                this.UploadUrl = "http://";

            }

            string strDoUpload = GetConfigAppSetting(AppSettingKeys.DoUpload);
            if (!string.IsNullOrEmpty(strDoUpload) && strDoUpload == "1")
            {
                this.DoUpload = true;
            }
            else
            {
                this.DoUpload = false;
            }


            //自动保存参数
            //string strAutoSaveToDisk = GetConfigAppSetting(AppSettingKeys.AutoSaveToDisk);
            //if (string.IsNullOrEmpty(strAutoSaveToDisk) && strAutoSaveToDisk == "1")
            //{
            //    this.AutoSaveToDisk = true;

            //}
            //else
            //{
            //    this.AutoSaveToDisk = false;
            //}

            //为了后期编辑管理功能，始终自动保存截图到文件
            this.AutoSaveToDisk = true;

            string strAutoSaveSubDir = GetConfigAppSetting(AppSettingKeys.AutoSaveSubDir);
            if (!string.IsNullOrEmpty(strAutoSaveSubDir) && strAutoSaveSubDir == "0")
            {
                this.AutoSaveSubDir = false;
            }
            else
            {
                this.AutoSaveSubDir = true;
            }

            this.AutoSaveDirectory = GetConfigAppSetting(AppSettingKeys.AutoSaveDirectory);
            if (string.IsNullOrEmpty(this.AutoSaveDirectory))
            {
                this.AutoSaveDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\屏幕截图";
            }
            if (!System.IO.Directory.Exists(this.AutoSaveDirectory))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(this.AutoSaveDirectory);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("创建目录“" + this.AutoSaveDirectory + "”失败！\n" + ex.Message);
                }
            }

            this.AutoSaveFileName1 = GetConfigAppSetting(AppSettingKeys.AutoSaveFileName1);
            if (string.IsNullOrEmpty(this.AutoSaveFileName1))
            {
                this.AutoSaveFileName1 = "屏幕截图";

            }

            this.AutoSaveFileName2 = GetConfigAppSetting(AppSettingKeys.AutoSaveFileName2);
            if (string.IsNullOrEmpty(this.AutoSaveFileName2))
            {
                this.AutoSaveFileName2 = "日期时间";

            }

            this.AutoSaveFileName3 = GetConfigAppSetting(AppSettingKeys.AutoSaveFileName3);
            if (string.IsNullOrEmpty(this.AutoSaveFileName3))
            {
                this.AutoSaveFileName3 = ".png";
            }
        }

        /// <summary>
        /// 根据Key获取配置信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigAppSetting(string key)
        {
            try
            {
                if (System.Configuration.ConfigurationManager.AppSettings[key] != null)
                {
                    return System.Configuration.ConfigurationManager.AppSettings[key];
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Source + ex.StackTrace);
            }
            return string.Empty;
        }

        /// <summary>
        /// 从已有资源中获得光标
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public static Cursor getCursorFromResource(byte[] resource)
        {
            /*
            MemoryStream ms = new MemoryStream(resource);
            Cursor cur = new Cursor(ms);
            return cur;
             * 以上方法会丢失颜色；
             */

            string filePath = ".\\cursorData.cur";
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            fileStream.Write(resource, 0, resource.Length);
            fileStream.Close();
            fileStream.Dispose();
            IntPtr hcur = LoadCursorFromFile(filePath);
            Cursor cur = new Cursor(hcur);
            File.Delete(filePath);
            return cur;
        }

        public const int WM_QUERYENDSESSION = 0x0011;

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
                case WM_QUERYENDSESSION:
                    if (Program.workOvertimeStarted)
                    {
                        WorkOvertime.OverEnd();
                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 截图显示主窗口之前保存可见的窗口区域列表，用于窗口自动发现功能
        /// </summary>
        protected void SaveVisibleWindonwList()
        {
            //清空之前的列表
            visibleWinList = new List<VisibleWindow>();

            //1、获取桌面窗口的句柄
            IntPtr desktopPtr = GetDesktopWindow();
            //2、获得一个子窗口（这通常是一个顶层窗口，当前活动的窗口）
            IntPtr winPtr = GetWindow(desktopPtr, GetWindowCmd.GW_CHILD);

            //3、循环取得桌面下的所有子窗口
            while (winPtr != IntPtr.Zero)
            {
                StringBuilder lpStr;
                String winText = string.Empty; //窗口标题
                String clsName = string.Empty; //窗口类名

                ////获得窗口标题
                //lpStr = new StringBuilder(512);
                //if (GetWindowText(winPtr, lpStr, lpStr.Capacity) > 0)
                //{
                //    winText = lpStr.ToString();
                //}

                //获取窗口信息
                WINDOWINFO info = new WINDOWINFO();
                info.cbSize = (uint)Marshal.SizeOf(info);
                bool ret = GetWindowInfo(winPtr, out info);

                if (ret)
                {
                    //获得窗口类名
                    lpStr = new StringBuilder(512);
                    if (GetClassName(winPtr, lpStr, lpStr.Capacity) > 0)
                    {
                        clsName = lpStr.ToString();
                        if (clsName.Equals("Shell_TrayWnd", StringComparison.CurrentCultureIgnoreCase))
                        {
                            rect_Shell_TrayWnd = new Rectangle(info.rcWindow.left, info.rcWindow.top, info.rcWindow.right - info.rcWindow.left, info.rcWindow.bottom - info.rcWindow.top);
                        }
                    }

                    WindowStyles winStyle = (WindowStyles)info.dwStyle;

                    //窗口是否可见
                    bool winVisible = (winStyle & WindowStyles.WS_VISIBLE) == WindowStyles.WS_VISIBLE;

                    SaveVisibleWindonwList(winText, clsName, ref info, winVisible, true, true, true);
                }

                //4、继续获取下一个子窗口
                winPtr = GetWindow(winPtr, GetWindowCmd.GW_HWNDNEXT);
            }

            //显示所有可见窗口的信息
            //for (int i = 0; i < visibleWinList.Count; i++)
            //{
            //    label1.Text += visibleWinList[i].ToString() + "\n";
            //}
        }

        /// <summary>
        /// 将窗口信息保存到visibleWinList变量中
        /// </summary>
        /// <param name="winText">窗口标题</param>
        /// <param name="clsName">窗口类名</param>
        /// <param name="info">窗口信息</param>
        /// <param name="winVisible">是否可见</param>
        /// <param name="checkedVisible">判断是否可见</param>
        /// <param name="checkedLocation">根据位置判断是否可见</param>
        /// <param name="checkedSize">根据大小判断是否可见</param>
        private void SaveVisibleWindonwList(String winText, String clsName, ref WINDOWINFO info, bool winVisible, bool checkedVisible, bool checkedLocation, bool checkedSize)
        {
            if (checkedVisible)
            {
                //仅显示可见窗口
                if (!winVisible)
                {
                    return;
                }
            }

            if (checkedSize)
            {
                //过虑 0 Size 窗口
                if (info.rcWindow.right - info.rcWindow.left == 0 || info.rcWindow.bottom - info.rcWindow.top == 0)
                {
                    return;
                }
            }

            if (checkedLocation)
            {
                //过虑屏幕之外的窗口
                if (info.rcWindow.right < 0 && info.rcWindow.bottom < 0)
                {
                    return;
                }
                if (info.rcWindow.left > Screen.PrimaryScreen.Bounds.Width && info.rcWindow.top > Screen.PrimaryScreen.Bounds.Height)
                {
                    return;
                }
            }

            int x = info.rcWindow.left;
            int y = info.rcWindow.top;
            int width = info.rcWindow.right - info.rcWindow.left;
            int height = info.rcWindow.bottom - info.rcWindow.top;
            if (x < 0)
            {
                width = width - Math.Abs(x);
                x = 0;
            }
            if (y < 0)
            {
                height = height - Math.Abs(y);
                y = 0;
            }

            width = x + width > Screen.PrimaryScreen.Bounds.Width ? Screen.PrimaryScreen.Bounds.Width - x : width;
            height = y + height > Screen.PrimaryScreen.Bounds.Height ? Screen.PrimaryScreen.Bounds.Height - y : height;

            if (!rect_Shell_TrayWnd.IsEmpty && !clsName.Equals("Shell_TrayWnd", StringComparison.CurrentCultureIgnoreCase)
                && (!winText.Equals("Program Manager", StringComparison.CurrentCultureIgnoreCase) && !clsName.Equals("Progman", StringComparison.CurrentCultureIgnoreCase)))
            {
                if (y + height > Screen.PrimaryScreen.Bounds.Height - rect_Shell_TrayWnd.Height)
                {
                    height = Screen.PrimaryScreen.Bounds.Height - y - rect_Shell_TrayWnd.Height;
                }
            }

            Rectangle rect = new Rectangle(x, y, width, height);
            //解决像通达信的窗口，获取的窗口大小小了一圈的问题
            //但是却没有找到直接相关的样式
            //if (((WindowStyles)info.dwExStyle & WindowStyles.WS_EX_WINDOWEDGE) == WindowStyles.WS_EX_WINDOWEDGE)
            //{
            //    rect.Inflate(6, 6);
            //}
            VisibleWindow win = new VisibleWindow(rect, info);
            visibleWinList.Add(win);
        }

        /// <summary>
        /// 如果窗口为可见状态，则隐藏窗口；
        /// 否则则显示窗口
        /// </summary>
        protected void ShowForm()
        {
            DebugTextBox.clearDebugInfo();

            if (this.Visible)
            {
                this.Hide();
            }
            else
            {
                //声明一个和屏幕窗口大小相同的Bitmap，用于保存屏幕图像
                Bitmap bkImage = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
                Graphics g = Graphics.FromImage(bkImage);

                Bitmap safeBmp = null;

                if (Program.safemode)
                {
                    safeBmp = PrintScreen.GetFullScreen();
                }
                else
                {
                    //非安全模式下，先判断剪贴板中是否存在使用键盘的Print Screen键截取的图片；
                    //你可先使用键盘的Print Screen键截取全屏的图片，然后再调用截图工具截取需要的部分。
                    //因为有时候需要快速捕捉某一画面，按一个键比按三个键方便多了。
                    safeBmp = PrintScreen.GetScreenImage();
                    if (safeBmp != null && !safeBmp.Size.Equals(Screen.AllScreens[0].Bounds.Size))
                    {
                        safeBmp = null;
                    }
                }
                if (safeBmp != null)
                {
                    g.DrawImage(safeBmp, 0, 0, safeBmp.Width, safeBmp.Height);
                    safeBmp.Dispose();
                    safeBmp = null;
                }
                else
                {
                    g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size, CopyPixelOperation.SourceCopy);
                }

                DrawCursorImageToScreenImage(ref g);
                screenImage = (Bitmap)bkImage.Clone();


                if (!this.isRecordMode)
                {
                    //如果不是录制模式，则绘制灰色图层
                    g.FillRectangle(new SolidBrush(Color.FromArgb(70, Color.Black)), Screen.PrimaryScreen.Bounds);
                }
                this.BackgroundImage = bkImage;

                this.ShowInTaskbar = false;
                this.Cursor = cursorDefault;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Width = Screen.PrimaryScreen.Bounds.Width;
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                this.Location = Screen.PrimaryScreen.Bounds.Location;

                this.WindowState = FormWindowState.Maximized;

                SaveVisibleWindonwList();

                this.Show();
                //UpdateCutInfoLabel(this.ZoomBoxVisible ? UpdateUIMode.ShowZoomBox : UpdateUIMode.None);
                UpdateCutInfoLabel(UpdateUIMode.None);
            }
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

        /// <summary>
        /// 当用户按下Esc键时,退出截图过程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //按下Ctrl+D，显示DebugTextBox，用于显示调试信息；
            if (e.Control && e.KeyCode == Keys.D)
            {
                //this.tb_debug.Show();
                //隐藏DebugTextBox需要使用Esc键
            }
            if (e.KeyCode == Keys.Escape)
            {
                ExitCutImage(true);
                // 如果不加这一句，热键只能在窗口隐藏后使用一次，之后就不起作用了。
                //RegisterHotKey(Handle, 100, 2 | 1, Keys.A);
            }
            if (e.Shift && e.KeyCode == Keys.Enter)
            {
                if (!this.lbl_CutImage.Visible)
                {
                    InvalidateWinSelectFrame(rect_WindowFromPoint);

                    this.isCuting = true;
                    this.beginPoint = MousePosition;
                    this.endPoint = MousePosition;
                    SaveCutImageSize(MousePosition, MousePosition);
                    UpdateCutInfoLabel(UpdateUIMode.ShowInfoBox | UpdateUIMode.ShowCutImage);
                }
            }
            if (e.KeyCode == Keys.Left)
            {
                if (this.lbl_CutImage.Visible)
                {
                    if (e.Shift)
                    {
                        if (this.cutImageRect.Width > 1)
                        {
                            this.cutImageRect.Width -= 1;
                            Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                    else
                    {
                        if (this.cutImageRect.Left > -1)
                        {
                            this.cutImageRect.X -= 1;
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                }
                else
                {
                    if (Cursor.Position.X > -1)
                    {
                        Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y);
                    }
                }
            }
            if (e.KeyCode == Keys.Right)
            {
                if (this.lbl_CutImage.Visible)
                {
                    if (e.Shift)
                    {
                        if (this.cutImageRect.Right < this.Width + 1)
                        {
                            this.cutImageRect.Width += 1;
                            Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                    else
                    {
                        if (this.cutImageRect.Right < this.Width + 1)
                        {
                            this.cutImageRect.X += 1;
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                }
                else
                {
                    if (Cursor.Position.X < this.Width + 1)
                    {
                        Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y);
                    }
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (this.lbl_CutImage.Visible)
                {
                    if (e.Shift)
                    {
                        if (this.cutImageRect.Height > 1)
                        {
                            this.cutImageRect.Height -= 1;
                            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 1);
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                    else
                    {
                        if (this.cutImageRect.Top > -1)
                        {
                            this.cutImageRect.Y -= 1;
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                }
                else
                {
                    if (Cursor.Position.Y > -1)
                    {
                        Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y - 1);
                    }
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                if (this.lbl_CutImage.Visible)
                {
                    if (e.Shift)
                    {
                        if (this.cutImageRect.Bottom < this.Height + 1)
                        {
                            this.cutImageRect.Height += 1;
                            Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                    else
                    {
                        if (this.cutImageRect.Bottom < this.Height + 1)
                        {
                            this.cutImageRect.Y += 1;
                            UpdateCutInfoLabel(UpdateUIMode.None);
                        }
                    }
                }
                else
                {
                    if (Cursor.Position.Y < this.Height + 1)
                    {
                        Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + 1);
                    }
                }
            }
        }

        /// <summary>
        /// 退出截图过程
        /// </summary>
        private void ExitCutImage(bool hideWindow) //  = true
        {
            this.lbl_Info.Visible = false;
            this.lbl_CutImage.Visible = false;
            this.lbl_ToolBox.Visible = false;
            this.isCuting = false;
            this.Cursor = cursorDefault;
            this.IsEditing = false;
            this.editDrawRect = new Rectangle();
            this.editSelectRect = new Rectangle();
            this.imageEditMode = ImageEditMode.None;
            this.mouseDownToolBoxLocation = new Point(-1, -1);
            this.mouseInToolBoxLocation = new Point(-1, -1);
            this.lbl_CutImage.Controls.Clear();
            imageEditList = new System.Collections.ArrayList();
            this.pnl_TextPro.Hide();
            this.pnl_penStyle.Hide();
            this.pnl_Palette.Hide();
            if (hideWindow)
            {
                this.screenImage.Dispose();
                this.Hide();
            }
            else
            {
                if (this.ZoomBoxVisible) { this.pictureBox_zoom.Show(); }
            }
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
        /// 处理编辑的区域框
        /// </summary>
        private void HandleEditDrawRect(Point beginPoint, Point endPoint)
        {
            // 保存最终的绘图基点，用于截取选中的区域
            this.endPoint = beginPoint;

            // 计算截取图片的大小
            int imgWidth = Math.Abs(endPoint.X - beginPoint.X);
            int imgHeight = Math.Abs(endPoint.Y - beginPoint.Y);

            // 设置截图区域的位置和大小
            this.editDrawRect.Location = new Point(beginPoint.X, beginPoint.Y);
            this.editDrawRect.Width = imgWidth;
            this.editDrawRect.Height = imgHeight;
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

            ExecDrawImageEdit(ref g);

            Clipboard.SetImage(bmp);

            #region 保存到文件
            if (saveToDisk)
            {
                SaveFileDialog sfDlg = new SaveFileDialog();
                sfDlg.AddExtension = true;
                sfDlg.DefaultExt = "jpg";
                sfDlg.FileName = "屏幕截图" + DateTime.Now.ToString("yyyyMMddHHmmss");
                sfDlg.Filter = "Png files (*.png)|*.png";
                sfDlg.Filter += "|Jpg files (*.jpg)|*.jpg";
                sfDlg.Filter += "|Gif files (*.gif)|*.gif";
                sfDlg.Filter += "|Bitmap files (*.bmp)|*.bmp";
                sfDlg.Filter += "|Tiff files (*.tif)|*.tif";
                sfDlg.Filter += "|Icon files (*.ico)|*.ico";

                sfDlg.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfDlg.OverwritePrompt = true;
                sfDlg.RestoreDirectory = false;
                sfDlg.Title = "保存屏幕截图为";
                if (sfDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    int pos = sfDlg.FileName.LastIndexOf('.');
                    string extn = ".png";
                    if (pos != -1)
                    {
                        extn = sfDlg.FileName.Substring(pos).ToLower();
                    }
                    switch (extn)
                    {
                        case ".jpg":
                            ImageCodecInfo myImageCodecInfo;
                            System.Drawing.Imaging.Encoder myEncoder1, myEncoder2;
                            EncoderParameter myEncoderParameter1, myEncoderParameter2;
                            EncoderParameters myEncoderParameters;

                            // Get an ImageCodecInfo object that represents the JPEG codec.
                            myImageCodecInfo = GetEncoderInfo("image/jpg");

                            // Create an Encoder object based on the GUID
                            // for the Quality parameter category.
                            myEncoder1 = System.Drawing.Imaging.Encoder.Quality;
                            myEncoder2 = System.Drawing.Imaging.Encoder.Compression;
                            // Create an EncoderParameters object.
                            // An EncoderParameters object has an array of EncoderParameter
                            // objects. In this case, there is only one
                            // EncoderParameter object in the array.
                            myEncoderParameters = new EncoderParameters(2);

                            // Save the bitmap as a JPEG file with quality level 100.
                            myEncoderParameter1 = new EncoderParameter(myEncoder1, 100L);
                            myEncoderParameter2 = new EncoderParameter(myEncoder2, (long)EncoderValue.CompressionLZW);
                            myEncoderParameters.Param[0] = myEncoderParameter1;
                            //myEncoderParameters.Param[1] = myEncoderParameter2;
                            bmp.Save(sfDlg.FileName, myImageCodecInfo, myEncoderParameters);
                            break;
                        case ".gif":
                            bmp.Save(sfDlg.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case ".bmp":
                            bmp.Save(sfDlg.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case ".tif":
                            bmp.Save(sfDlg.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                            break;
                        case ".ico":
                            //保存的文件根本不是图标格式，只不过扩展名是:.ico
                            bmp.Save(sfDlg.FileName, System.Drawing.Imaging.ImageFormat.Icon);
                            break;
                        default:
                            bmp.Save(sfDlg.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                }
            }
            #endregion //保存到文件

            #region //上传文件
            if (uploadImage)
            {
                frmUpload frm = new frmUpload(this.Handle, ref bmp);
                frm.ShowDialog();
            }
            #endregion //上传文件

            #region //自动保存文件
            if (AutoSaveToDisk)
            {
                string filePath = this.AutoSaveDirectory;
                string lastWord = string.Empty;
                if (this.AutoSaveDirectory.Length > 0)
                {
                    lastWord = this.AutoSaveDirectory.Substring(this.AutoSaveDirectory.Length - 1);
                }
                if (lastWord != "\\")
                {
                    filePath += "\\";
                }
                if (this.AutoSaveSubDir)
                {
                    filePath += System.DateTime.Now.ToString("yyyy_MM_dd") + "\\";
                }
                if (!System.IO.Directory.Exists(filePath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(filePath);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                string fileName = string.Empty;

                switch (AutoSaveFileName2)
                {
                    case "日期_序号":
                        fileName = AutoSaveFileName1 + DateTime.Now.ToString("yyyy-MM-dd_") + autoSaveFileIndex.ToString("0000") + AutoSaveFileName3;
                        autoSaveFileIndex++;
                        while (File.Exists(filePath + fileName))
                        {
                            fileName = AutoSaveFileName1 + DateTime.Now.ToString("yyyy-MM-dd_") + autoSaveFileIndex.ToString("0000") + AutoSaveFileName3;
                            autoSaveFileIndex++;
                        }
                        break;
                    case "序号":
                        fileName = AutoSaveFileName1 + autoSaveFileIndex.ToString("0000") + AutoSaveFileName3;
                        autoSaveFileIndex++;
                        while (File.Exists(filePath + fileName))
                        {
                            fileName = AutoSaveFileName1 + autoSaveFileIndex.ToString("0000") + AutoSaveFileName3;
                            autoSaveFileIndex++;
                        }
                        break;
                    default:
                        fileName = AutoSaveFileName1 + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + AutoSaveFileName3;
                        while (File.Exists(filePath + fileName))
                        {
                            fileName = AutoSaveFileName1 + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + AutoSaveFileName3;
                        }
                        break;
                }


                filePath += fileName;

                bool doSave = true;
                if (System.IO.File.Exists(filePath))
                {
                    if (MessageBox.Show("您确定要替换文件“" + fileName + "”吗？", "同名的文件已存在", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                    {
                        doSave = false;
                    }
                }
                if (doSave)
                {
                    try
                    {
                        switch (AutoSaveFileName3)
                        {
                            case ".jpg":
                                ImageCodecInfo myImageCodecInfo;
                                System.Drawing.Imaging.Encoder myEncoder1, myEncoder2;
                                EncoderParameter myEncoderParameter1, myEncoderParameter2;
                                EncoderParameters myEncoderParameters;

                                // Get an ImageCodecInfo object that represents the JPEG codec.
                                myImageCodecInfo = GetEncoderInfo("image/jpg");

                                // Create an Encoder object based on the GUID
                                // for the Quality parameter category.
                                myEncoder1 = System.Drawing.Imaging.Encoder.Quality;
                                myEncoder2 = System.Drawing.Imaging.Encoder.Compression;
                                // Create an EncoderParameters object.
                                // An EncoderParameters object has an array of EncoderParameter
                                // objects. In this case, there is only one
                                // EncoderParameter object in the array.
                                myEncoderParameters = new EncoderParameters(2);

                                // Save the bitmap as a JPEG file with quality level 100.
                                myEncoderParameter1 = new EncoderParameter(myEncoder1, 100L);
                                myEncoderParameter2 = new EncoderParameter(myEncoder2, (long)EncoderValue.CompressionLZW);
                                myEncoderParameters.Param[0] = myEncoderParameter1;
                                //myEncoderParameters.Param[1] = myEncoderParameter2;
                                bmp.Save(filePath, myImageCodecInfo, myEncoderParameters);
                                break;
                            case ".gif":
                                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Gif);
                                break;
                            case ".bmp":
                                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Bmp);
                                break;
                            case ".tif":
                                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Tiff);
                                break;
                            case ".ico":
                                //保存的文件根本不是图标格式，只不过扩展名是:.ico
                                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Icon);
                                break;
                            default:
                                bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("自动保存文件失败:\r\n" + ex.Message + "\r\n" + ex.Source + "\r\n" + ex.StackTrace);
                    }
                }
            }
            #endregion //自动保存文件

            #region //自动上传文件
            if (this.DoUpload)
            {
                FileUpload fu = new FileUpload(this.UploadUrl, bmp);
                fu.Start();
                if (fu.LastException != null)
                {
                    MessageBox.Show("自动上传文件失败:\r\n" + fu.LastException.Message + "\r\n" + fu.LastException.Source + "\r\n" + fu.LastException.StackTrace);
                }
            }
            #endregion //自动上传文件

            ExitCutImage(true);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// 执行图片编辑项的绘制过程
        /// </summary>
        /// <param name="g"></param>
        private void ExecDrawImageEdit(ref Graphics g)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            g.TextContrast = 5;

            for (int i = 0; i < this.imageEditList.Count; i++)
            {
                ImageEditObject editObject = (ImageEditObject)this.imageEditList[i];
                if (editObject == null) { continue; }
                //if (editObject.Selected) { continue; }

                switch (editObject.EditMode)
                {
                    case ImageEditMode.Rectangle:
                        switch (this.imageSubEditMode)
                        {
                            case ImageSubEditMode.Rectangle:
                                g.FillRectangle(editObject.BackBrush, editObject.EditRect);
                                g.DrawRectangle(editObject.EditPen, editObject.EditRect);
                                break;
                            case ImageSubEditMode.CircularRectangle:
                                g.FillRectangle(editObject.BackBrush, editObject.EditRect);
                                g.DrawRectangle(editObject.EditPen, editObject.EditRect);
                                break;
                            case ImageSubEditMode.Ellipse:
                                g.FillEllipse(editObject.BackBrush, editObject.EditRect);
                                g.DrawEllipse(editObject.EditPen, editObject.EditRect);
                                break;
                            case ImageSubEditMode.Arrowhead:
                                editObject.EditPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                                editObject.EditPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                                break;
                            case ImageSubEditMode.Polygon:
                                g.FillRectangle(editObject.BackBrush, editObject.EditRect);
                                g.DrawRectangle(editObject.EditPen, editObject.EditRect);
                                break;
                            case ImageSubEditMode.L_Shape:
                                g.FillRectangle(editObject.BackBrush, editObject.EditRect);
                                g.DrawRectangle(editObject.EditPen, editObject.EditRect);
                                break;
                            default:
                                g.FillRectangle(editObject.BackBrush, editObject.EditRect);
                                g.DrawRectangle(editObject.EditPen, editObject.EditRect);
                                break;
                        }

                        break;
                    case ImageEditMode.Ellipse:
                        g.FillEllipse(editObject.BackBrush, editObject.EditRect);
                        g.DrawEllipse(editObject.EditPen, editObject.EditRect);
                        break;
                    case ImageEditMode.Arrowhead:
                        Pen penb = new Pen(lbl_penColor.BackColor);
                        float penSize = editObject.EditPen.Width;
                        ArrowheadHelper.ComputeHypotenuse(editObject.BeginPoint, editObject.EndPoint, penSize);
                        penb.CustomStartCap = new System.Drawing.Drawing2D.AdjustableArrowCap(ArrowheadHelper.PenWidth, ArrowheadHelper.HypotenuseLength, true);
                        penb.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(ArrowheadHelper.ArrowCapWidth, ArrowheadHelper.ArrowCapHeight, true);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLine(penb, editObject.BeginPoint, editObject.EndPoint);
                        penb.Dispose();
                        break;
                    case ImageEditMode.Brush:
                        int penWidth = (int)editObject.EditPen.Width;
                        for (int iPoints = 0; iPoints < editObject.Points.Length; iPoints++)
                        {
                            System.Drawing.Point point = editObject.Points[iPoints];
                            g.FillEllipse(editObject.EditPen.Brush, point.X, point.Y, penWidth, penWidth);
                        }
                        break;
                    case ImageEditMode.Text:
                        int charFit = 0;
                        int lineFill = 0;
                        try
                        {
                            //解决：如果用户不手动输入换行符，则绘制文字时不能自动换行的问题。
                            g.MeasureString(editObject.Text, editObject.TextFont, new SizeF(editObject.EditRect.Width, editObject.EditRect.Height), StringFormat.GenericDefault, out charFit, out lineFill);

                            if (editObject.EditRect.Height < lineFill * editObject.TextFont.Height)
                            {
                                editObject.EditRect.Height = lineFill * editObject.TextFont.Height;
                            }

                            g.DrawString(editObject.Text, editObject.TextFont, editObject.EditPen.Brush, editObject.EditRect);

                        }
                        catch (System.Exception ex)
                        {
                            ExceptionLog.Log(ex);
                        }
                        break;
                    case ImageEditMode.ToolTips:
                        DrawToolTips(ref g, editObject);
                        break;
                    default: break;
                }
            }
        }

        /// <summary>
        /// 截图窗口鼠标按下事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.pnl_Palette.Visible)
            {
                this.pnl_Palette.Hide();
            }

            handelEdit_StateChanged();

            // 左键单击事件
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (!this.lbl_CutImage.Visible)
                {
                    InvalidateWinSelectFrame(rect_WindowFromPoint);
                    
                    this.isCuting = true;
                    this.beginPoint = e.Location;
                    this.endPoint = e.Location;
                    SaveCutImageSize(e.Location, e.Location);

                    UpdateCutInfoLabel(UpdateUIMode.ShowCutImage | UpdateUIMode.ShowInfoBox);

                    mouseDownTimeTick = System.DateTime.Now.Ticks;
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

                    //如果自动发现窗口的区域不为空，则说明用户选择的是自动发现窗口的区域
                    if (!rect_WindowFromPoint.IsEmpty)
                    {
                        SaveCutImageSize(rect_WindowFromPoint.Location, new Point(rect_WindowFromPoint.Right - 1, rect_WindowFromPoint.Bottom -1));
                    }

                    if (this.ToolBoxVisible)
                    {
                        this.lbl_ToolBox.Show();
                        //向工具栏发送鼠标按下和抬起事件，设置默认进入矩形编辑状态
                        MouseEventArgs arg = new MouseEventArgs(MouseButtons.Left, 1, 15, 15, 0);
                        this.lbl_ToolBox_MouseDown(this.lbl_ToolBox, arg);
                        this.lbl_ToolBox_MouseUp(this.lbl_ToolBox, arg);
                    }
                    this.pictureBox_zoom.Hide();

                    this.lastMouseMoveTime = 0;
                    UpdateCutInfoLabel(UpdateUIMode.None);
                }
            }
        }

        /// <summary>
        /// 根据鼠标位置返回指定窗口区域
        /// </summary>
        /// <param name="mousePos">鼠标位置</param>
        /// <returns></returns>
        private VisibleWindow WindowFromPoint(Point mousePos)
        {
            VisibleWindow win = new VisibleWindow();

            for (int i = 0; i < visibleWinList.Count; i++)
            {
                if (visibleWinList[i].rect.Contains(mousePos))
                {
                    win = visibleWinList[i];
                    break;
                }
            }

            return win;
        }

        /// <summary>
        /// 使自动选中的窗体边框部分失效
        /// </summary>
        /// <param name="winRect"></param>
        private void InvalidateWinSelectFrame(Rectangle winRect)
        {
            if (winRect.IsEmpty)
            {
                return;
            }
            //Region regnBig = new Region(
            //            new Rectangle(
            //                winRect.X - 2,
            //                winRect.Y - 2,
            //                winRect.Width + 5,
            //                winRect.Height + 5
            //            )
            //        );
            //Region regnSmall = new Region(
            //    new Rectangle(
            //        winRect.X + 3,
            //        winRect.Y + 3,
            //        winRect.Width - 5,
            //        winRect.Height - 5
            //    )
            //);
            //regnBig.Exclude(regnSmall);
            this.Invalidate(winRect);
            this.Update();
        }

        public void DrawWindowFromPoint()
        {
            if (!rect_WindowFromPoint.IsEmpty)
            {
                Bitmap bmp = new Bitmap(rect_WindowFromPoint.Width, rect_WindowFromPoint.Height);
                Graphics gbmp = Graphics.FromImage(bmp);
                Rectangle rectBmp = new Rectangle(0, 0, bmp.Width, bmp.Height);
                gbmp.DrawImage(this.screenImage, rectBmp, rect_WindowFromPoint, GraphicsUnit.Pixel);
                gbmp.DrawRectangle(new Pen(Color.FromArgb(0, 174, 255), 5.0F), rectBmp);
                Graphics g = this.CreateGraphics();
                g.DrawImage(bmp, rect_WindowFromPoint, rectBmp, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// 截图窗口鼠标移动事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = rect_WindowFromPoint.ToString();

            // 如果截取区域不可见,则退出处理过程
            if (!this.lbl_CutImage.Visible)
            {
                UpdateCutInfoLabel(UpdateUIMode.None);

                //根据鼠标指针的位置，自动发现窗口
                VisibleWindow win = WindowFromPoint(e.Location);

                if (!win.rect.IsEmpty && win.rect != rect_WindowFromPoint)
                {
                    InvalidateWinSelectFrame(rect_WindowFromPoint);
                    
                    rect_WindowFromPoint = win.rect;

                    //这段代码放到if条件的内部，
                    //只有当鼠标指针移动到不同的窗体上，才重绘发现的窗体区域
                    //由于取消了放大镜的显示，所以只需要重绘一次就可以了。
                    DrawWindowFromPoint();
                }
                
                return;
            }

            long mouseDownTimeStep = System.DateTime.Now.Ticks - mouseDownTimeTick;
            if (mouseDownTimeStep > 1000000)
            {
                rect_WindowFromPoint = Rectangle.Empty;
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

            //SolidBrush brush = new SolidBrush(this.currentToolStyle.FillColor);
            //Pen pen = new Pen(this.currentToolStyle.BorderColor, this.currentToolStyle.BorderWidth);
            SolidBrush brush = new SolidBrush(Color.FromArgb(10, 124, 202));
            Pen pen = new Pen(brush, 1.0F);

            // 绘制编辑内容
            ExecDrawImageEdit(ref g);

            // 绘制编辑的拖动框
            DrawEditSelected(ref g, ref pen, ref brush);

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

            g.SmoothingMode = SmoothingMode.None;
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
        /// 绘制编辑的拖动框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        private void DrawEditSelected(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;

            switch (this.imageEditMode)
            {
                case ImageEditMode.Rectangle:
                    switch (this.imageSubEditMode)
                    {
                        case ImageSubEditMode.Rectangle:
                            DrawEditSelected_Rectangle(ref g, ref pen, ref brush);
                            break;
                        case ImageSubEditMode.CircularRectangle:
                            DrawEditSelected_CircularRectangle(ref g, ref pen, ref brush);
                            break;
                        case ImageSubEditMode.Ellipse:
                            DrawEditSelected_Ellipse(ref g, ref pen, ref brush);
                            break;
                        case ImageSubEditMode.Arrowhead:
                            DrawEditSelected_Arrowhead(ref g, ref pen, ref brush);
                            break;
                        case ImageSubEditMode.Polygon:
                            DrawEditSelected_Rectangle(ref g, ref pen, ref brush);
                            break;
                        case ImageSubEditMode.L_Shape:
                            DrawEditSelected_Rectangle(ref g, ref pen, ref brush);
                            break;
                        default:
                            DrawEditSelected_Rectangle(ref g, ref pen, ref brush);
                            break;
                    }

                    break;
                case ImageEditMode.Ellipse:
                    DrawEditSelected_Ellipse(ref g, ref pen, ref brush);
                    break;
                case ImageEditMode.Arrowhead:
                    DrawEditSelected_Arrowhead(ref g, ref pen, ref brush);
                    break;
                case ImageEditMode.Brush:
                    ImageEditRender.DrawSelectedBrush(ref g, ref this.currentToolStyle, this.pointList);
                    //DrawEditSelected_Brush(ref g, ref pen, ref brush);
                    break;
                case ImageEditMode.Text:
                    DrawEditSelected_Text(ref g, ref pen, ref brush);
                    break;
                case ImageEditMode.ToolTips:
                    DrawEditSelected_ToolTips(ref g, ref pen, ref brush);
                    break;
                default: break;
            }

        }

        /// <summary>
        /// 绘制截图编辑选中的矩形框
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        private void DrawEditSelected_Rectangle(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        {
            if (editDrawRect.Width != 0 && editDrawRect.Height != 0)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(0, 168, 255)), editDrawRect);
            }

            if (this.editSelectRect.Width != 0 && this.editSelectRect.Height != 0)
            {
                // 绘制四个角的调整块
                g.SmoothingMode = SmoothingMode.None;
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);
            }
        }

        private void DrawEditSelected_CircularRectangle(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        {
            if (editDrawRect.Width != 0 && editDrawRect.Height != 0)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(0, 168, 255)), editDrawRect);
                g.DrawArc(new Pen(Color.FromArgb(0, 168, 255)), editDrawRect, 35.0f, 75.0f);
            }

            if (this.editSelectRect.Width != 0 && this.editSelectRect.Height != 0)
            {
                // 绘制四个角的调整块
                g.SmoothingMode = SmoothingMode.None;
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);
            }
        }

        /// <summary>
        /// 绘制截图编辑选中的椭圆
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        private void DrawEditSelected_Ellipse(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        {
            if (editDrawRect.Width != 0 && editDrawRect.Height != 0)
            {
                g.DrawEllipse(new Pen(Color.FromArgb(0, 168, 255)), editDrawRect);
            }

            if (this.editSelectRect.Width != 0 && this.editSelectRect.Height != 0)
            {
                // 绘制中间的四个调整块
                g.SmoothingMode = SmoothingMode.None;
                int blockX = this.editSelectRect.X + this.editSelectRect.Width / 2 - 2;
                int blockY = this.editSelectRect.Y + this.editSelectRect.Height / 2 - 2;
                g.FillRectangle(brush, blockX, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X - 2, blockY, 5, 5);
                g.FillRectangle(brush, blockX, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, blockY, 5, 5);
            }
        }

        /// <summary>
        /// 绘制截图编辑选中的箭头
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        private void DrawEditSelected_Arrowhead(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        {
            if (editDrawRect.Width != 0 || editDrawRect.Height != 0)
            {
                float penSize = 1.0f;
                if (cbb_penSize.SelectedItem != null && cbb_penSize.SelectedItem.ToString().Length != 0)
                {
                    float.TryParse(cbb_penSize.SelectedItem.ToString(), out penSize);
                }

                Pen penb = new Pen(lbl_penColor.BackColor);
                ArrowheadHelper.ComputeHypotenuse(this.downPoint, this.mouseLocation, penSize);
                penb.CustomStartCap = new System.Drawing.Drawing2D.AdjustableArrowCap(ArrowheadHelper.PenWidth, ArrowheadHelper.HypotenuseLength, true);
                penb.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(ArrowheadHelper.ArrowCapWidth, ArrowheadHelper.ArrowCapHeight, true);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(penb, this.downPoint, this.mouseLocation);
            }
        }

        /// <summary>
        /// 绘制截图编辑选中的画笔
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        //private void DrawEditSelected_Brush(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        //{
        //    if (this.pointList.Count == 0)
        //    {
        //        return;
        //    }
        //    g.SmoothingMode = SmoothingMode.AntiAlias;

        //    int penWidth = this.currentToolStyle.BorderWidth;
        //    SolidBrush sbrush = new SolidBrush(this.currentToolStyle.BorderColor);


        //    DebugTextBox.DebugText("DrawEditSelected_Brush>this.pointList.Count=" + this.pointList.Count.ToString());
        //    for (int i = 0; i < this.pointList.Count; i++)
        //    {
        //        System.Drawing.Point point = (Point)this.pointList[i];
        //        g.FillEllipse(sbrush, point.X, point.Y, penWidth, penWidth);
        //    }
        //}

        /// <summary>
        /// 绘制截图编辑选中的文字
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        private void DrawEditSelected_Text(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        {
            if (editDrawRect.Width != 0 && editDrawRect.Height != 0)
            {
                g.DrawRectangle(new Pen(Color.FromArgb(0, 168, 255)), editDrawRect);
            }

            if (this.editSelectRect.Width != 0 && this.editSelectRect.Height != 0)
            {
                g.SmoothingMode = SmoothingMode.None;
                g.DrawRectangle(pen, editSelectRect);

                // 绘制四个角的调整块
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(Brushes.White, this.editSelectRect.X + this.editSelectRect.Width - 1, this.editSelectRect.Y - 1, 3, 3);
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);

                // 绘制中间的四个调整块
                int blockX = this.editSelectRect.X + this.editSelectRect.Width / 2 - 2;
                int blockY = this.editSelectRect.Y + this.editSelectRect.Height / 2 - 2;
                g.FillRectangle(brush, this.editSelectRect.X - 2, blockY, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, blockY, 5, 5);
            }
        }

        /// <summary>
        /// 绘制截图编辑选中的提示
        /// </summary>
        /// <param name="g"></param>
        /// <param name="pen"></param>
        /// <param name="brush"></param>
        private void DrawEditSelected_ToolTips(ref Graphics g, ref Pen pen, ref SolidBrush brush)
        {
            SolidBrush backBrush = new SolidBrush(lbl_fillColor.BackColor);
            if (editDrawRect.Width != 0 && editDrawRect.Height != 0)
            {
                DrawToolTips(ref g, editDrawRect, ref pen, ref backBrush);
            }

            if (this.editSelectRect.Width != 0 && this.editSelectRect.Height != 0)
            {
                //DrawToolTips(ref g, editSelectRect, ref pen, ref backBrush);

                // 绘制四个角的调整块
                g.SmoothingMode = SmoothingMode.None;
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y - 2, 5, 5);
                g.FillRectangle(Brushes.White, this.editSelectRect.X + this.editSelectRect.Width - 1, this.editSelectRect.Y - 1, 3, 3);
                g.FillRectangle(brush, this.editSelectRect.X - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);
                g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, this.editSelectRect.Y + this.editSelectRect.Height - 2, 5, 5);

                // 绘制中间的四个调整块
                //int blockX = this.editSelectRect.X + this.editSelectRect.Width / 2 - 2;
                //int blockY = this.editSelectRect.Y + this.editSelectRect.Height / 2 - 2;
                //g.FillRectangle(brush, this.editSelectRect.X - 2, blockY, 5, 5);
                //g.FillRectangle(brush, this.editSelectRect.X + this.editSelectRect.Width - 2, blockY, 5, 5);
            }
        }

        private void DrawToolTips(ref Graphics gRef, Rectangle editDrawRect, ref Pen borderPen, ref SolidBrush backBrush)
        {
            if (editDrawRect.Width == 0 || editDrawRect.Height == 0)
            {
                return;
            }
            int width = editDrawRect.Width;
            int height = editDrawRect.Height;
            try
            {
                Bitmap bmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                //int sblitHeight = (int)(height * 0.7);

                //Point[] tipsPoints = new Point[]{
                //    new Point(0, 3),
                //    new Point(1, 1),
                //    new Point(3, 0),

                //    new Point(width - 4, 0),
                //    new Point(width - 2, 1),
                //    new Point(width - 1, 3),

                //    new Point(width - 1, sblitHeight - 3),
                //    new Point(width - 2, sblitHeight - 1),
                //    new Point(width - 4, sblitHeight),

                //    new Point(width / 3, sblitHeight),
                //    new Point(width / 30, height - 1),
                //    new Point(width / 5, sblitHeight),

                //    new Point(3, sblitHeight),
                //    new Point(1, sblitHeight - 1),
                //    new Point(0, sblitHeight - 3),
                //    new Point(0, 3)
                //};

                Rectangle rect = new Rectangle(0, 0, width, height);

                GraphicsPath gicPath = CreateRoundToolTipsPath(rect, 7, borderPen.Width);

                g.FillPath(backBrush, gicPath);

                borderPen.Alignment = PenAlignment.Left;

                g.DrawPath(borderPen, gicPath);

                g.Dispose();

                if (this.mouseLocation.X > this.beginPoint.X && this.mouseLocation.Y > this.beginPoint.Y)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                }

                if (this.mouseLocation.X < this.beginPoint.X && this.mouseLocation.Y < this.beginPoint.Y)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                }

                if (this.mouseLocation.X < this.beginPoint.X && this.mouseLocation.Y > this.beginPoint.Y)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                }

                gRef.DrawImage(bmp, editDrawRect.Location);


            }
            catch (System.Exception ex)
            {
                ExceptionLog.Log(ex);
            }
        }

        /// <summary>
        /// 绘制提示框
        /// </summary>
        /// <param name="gRef"></param>
        /// <param name="editObject"></param>
        private void DrawToolTips(ref Graphics gRef, ImageEditObject editObject)
        {
            if (editObject.EditRect.Width == 0 || editObject.EditRect.Height == 0)
            {
                return;
            }

            int width = editObject.EditRect.Width;
            int height = editObject.EditRect.Height;
            try
            {
                Bitmap bmp = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                Pen borderPen = new Pen(Color.Red);
                if (editObject.EditPen != null)
                {
                    borderPen = editObject.EditPen;
                }

                SolidBrush backBrush = new SolidBrush(Color.White);
                if (editObject.BackBrush != null)
                {
                    backBrush = editObject.BackBrush;
                }

                //int sblitHeight = editObject.EditRect.Top + (int)(height * 0.7);

                //Point[] tipsPoints = new Point[]{
                //    new Point(editObject.EditRect.Left, editObject.EditRect.Top + 3),
                //    new Point(editObject.EditRect.Left + 3, editObject.EditRect.Top),

                //    new Point(editObject.EditRect.Left + editObject.EditRect.Width - 4, editObject.EditRect.Top),
                //    new Point(editObject.EditRect.Left + editObject.EditRect.Width - 1, editObject.EditRect.Top + 3),

                //    new Point(editObject.EditRect.Left + editObject.EditRect.Width - 1, sblitHeight - 3),
                //    new Point(editObject.EditRect.Left + editObject.EditRect.Width - 4, sblitHeight),

                //    new Point(editObject.EditRect.Left + editObject.EditRect.Width / 3, sblitHeight),
                //    new Point(editObject.EditRect.Left + editObject.EditRect.Width / 30, editObject.EditRect.Top + editObject.EditRect.Height - 1),
                //    new Point(editObject.EditRect.Left + editObject.EditRect.Width / 5, sblitHeight),

                //    new Point(editObject.EditRect.Left + 3, sblitHeight),
                //    new Point(editObject.EditRect.Left, sblitHeight - 3),
                //    new Point(editObject.EditRect.Left, editObject.EditRect.Top + 3)
                //};

                //int sblitHeight = (int)(height * 0.7);

                //int x = (int)(borderPen.Width / 2);
                //int x1 = (int)borderPen.Width;
                //int x2 = (int)borderPen.Width + 1;
                //int x3 = (int)borderPen.Width + 2;
                //int x4 = (int)borderPen.Width + 3;
                //int x5 = (int)borderPen.Width + 4;

                //Point[] tipsPoints = new Point[]{
                //    new Point(x, x3),
                //    new Point(x1, x1),
                //    new Point(x3, x),

                //    new Point(width - x5, x),
                //    new Point(width - x2, x1),
                //    new Point(width - x1, x3),

                //    new Point(width - x1, sblitHeight - x4),
                //    new Point(width - x2, sblitHeight - x1),
                //    new Point(width - x5, sblitHeight),

                //    new Point(width / 3, sblitHeight),
                //    new Point(width / 30, height - x1),

                //    new Point(width / 5 , sblitHeight),

                //    new Point(x3, sblitHeight),
                //    new Point(x1, sblitHeight - x1),
                //    new Point(x, sblitHeight - x4),

                //    new Point(x, x3)
                //};

                //borderPen.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;
                //borderPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                //borderPen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                //borderPen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
                //borderPen.Brush = new System.Drawing.Drawing2D.PathGradientBrush();

                borderPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                borderPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                borderPen.Alignment = PenAlignment.Left;

                Rectangle rect = new Rectangle(0, 0, width, height);
                GraphicsPath gicPath = CreateRoundToolTipsPath(rect, 7, borderPen.Width);

                g.FillPath(backBrush, gicPath);

                g.DrawPath(borderPen, gicPath);

                g.Dispose();

                if (editObject.EndPoint.X > editObject.BeginPoint.X && editObject.EndPoint.Y > editObject.BeginPoint.Y)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                }

                if (editObject.EndPoint.X < editObject.BeginPoint.X && editObject.EndPoint.Y < editObject.BeginPoint.Y)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                }

                if (editObject.EndPoint.X < editObject.BeginPoint.X && editObject.EndPoint.Y > editObject.BeginPoint.Y)
                {
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipY);
                    bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                }

                gRef.DrawImage(bmp, editObject.EditRect.Location);
            }
            catch (System.Exception ex)
            {
                ExceptionLog.Log(ex);
            }
        }

        internal static GraphicsPath CreateRoundToolTipsPath(Rectangle rect, int cornerRadius, float fPenWidth)
        {
            int penWidth = (int)fPenWidth;
            int halfPenWidth = penWidth / 2;
            cornerRadius += penWidth;
            int splitHeight = (int)(rect.Height * 0.7);

            GraphicsPath roundedRect = new GraphicsPath();
            //左上角
            roundedRect.AddArc(rect.X + halfPenWidth, rect.Y + halfPenWidth, cornerRadius * 2, cornerRadius * 2, 180, 90);
            //上边框
            roundedRect.AddLine(rect.X + halfPenWidth + cornerRadius, rect.Y + halfPenWidth, rect.Right - cornerRadius * 2 - penWidth, rect.Y + halfPenWidth);
            //右上角
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2 - penWidth, rect.Y + halfPenWidth, cornerRadius * 2, cornerRadius * 2, 270, 90);
            //右边框
            roundedRect.AddLine(rect.Right - penWidth, rect.Y + cornerRadius * 2, rect.Right - penWidth, rect.Y + splitHeight - cornerRadius * 2);
            //右下角
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2 - penWidth, rect.Y + splitHeight - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            //下边框
            roundedRect.AddLine(rect.Right - cornerRadius * 2 - penWidth, rect.Y + splitHeight, rect.X + cornerRadius * 2 + rect.Width / 3, rect.Y + splitHeight);
            roundedRect.AddLine(rect.X + cornerRadius * 2 + rect.Width / 3, rect.Y + splitHeight, rect.X + rect.Width / 30, rect.Y + rect.Height - penWidth);
            roundedRect.AddLine(rect.X + rect.Width / 30, rect.Y + rect.Height - penWidth, rect.X + cornerRadius * 2 + rect.Width / 5, rect.Y + splitHeight);
            roundedRect.AddLine(rect.X + cornerRadius * 2 + rect.Width / 5, rect.Y + splitHeight, rect.X + cornerRadius * 2, rect.Y + splitHeight);

            //左下角
            roundedRect.AddArc(rect.X + halfPenWidth, rect.Y + splitHeight - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            //左边框
            roundedRect.AddLine(rect.X + halfPenWidth, rect.Y + splitHeight - cornerRadius * 2, rect.X + halfPenWidth, rect.Y + cornerRadius * 2);

            roundedRect.CloseFigure();
            return roundedRect;
        }


        internal static GraphicsPath CreateRoundRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        /// <summary>
        /// 截图区域信息显示框的绘制事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_Info_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp_lbl = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
            Graphics g = Graphics.FromImage(bmp_lbl);

            SolidBrush brush = new SolidBrush(Color.FromArgb(200, Color.DimGray));
            g.FillRectangle(brush, 0, 0, e.ClipRectangle.Width, e.ClipRectangle.Height);

            this.areaSize = new System.Drawing.Size(this.lbl_CutImage.Width - 4, this.lbl_CutImage.Height - 4);
            if (this.areaSize.Width < 1) { this.areaSize.Width = 1; }
            if (this.areaSize.Height < 1) { this.areaSize.Height = 1; }
            brush = new SolidBrush(Color.White);
            Font font = new System.Drawing.Font(FontFamily.GenericSerif, 12.0F, GraphicsUnit.Pixel);
            string str = "大小: " + this.areaSize.Width + "*" + this.areaSize.Height;
            g.DrawString(str, font, brush, new PointF(4.0F, 4.0F));
            e.Graphics.DrawImage(bmp_lbl, 0, 0);
            g.Dispose();
            bmp_lbl.Dispose();
        }

        /// <summary>
        /// 处理编辑状态的改变，主要是处理文字编辑框“TextBox”
        /// </summary>
        private void handelEdit_StateChanged()
        {
            ImageEditObject editObject = null;
            //如果存在文字编辑框“TextBox”，则查找对应的EditObject进行赋值
            if (this.lbl_CutImage.Controls.Count == 1) //this.imageEditMode == ImageEditMode.Text && 
            {
                for (int i = this.imageEditList.Count - 1; i > -1; i--)
                {
                    editObject = (ImageEditObject)this.imageEditList[i];
                    if (editObject != null && editObject.Selected)
                    {
                        break;
                    }
                }
                TextBox tb = (TextBox)this.lbl_CutImage.Controls[0];
                if (tb != null && editObject != null)
                {
                    editObject.Text = tb.Text;
                }
                editObject = null;
                this.lbl_CutImage.Controls.Clear();
            }

            editDrawRect = new Rectangle(new Point(0, 0), new Size(0, 0));
            editSelectRect = new Rectangle(new Point(0, 0), new Size(0, 0));
            IsEditing = false;

            this.lbl_CutImage.Refresh();
        }

        /// <summary>
        /// 处理画刷的落笔点，当鼠标只是点一下，不移动的情况下，以鼠标点为中心画一个圆点；
        /// </summary>
        /// <param name="mouseLocation">当前鼠标的位置</param>
        /// <returns>返回以鼠标点为中心Point</returns>
        private Point handleEdit_getBrushPoint(Point mouseLocation)
        {
            int brushWidth = 1;
            //取得当前编辑工具样式中的边框宽度，也就是画笔的粗细
            if (this.currentToolStyle.BorderWidth > 2)
            {
                brushWidth = this.currentToolStyle.BorderWidth / 2;
            }
            //返回以鼠标点为中心Point
            return new Point(mouseLocation.X - brushWidth, mouseLocation.Y - brushWidth);
        }

        /// <summary>
        /// 处理编辑截取区域时鼠标按下的事件
        /// </summary>
        /// <param name="mouseLocation"></param>
        private void handleEdit_CutImageMouseDown(Point mouseLocation)
        {
            if (this.imageEditMode != ImageEditMode.None)
            {
                handelEdit_StateChanged();

                if (IsEditing)
                {
                    editDrawRect = new Rectangle(mouseLocation, new Size(0, 0));
                    editSelectRect = new Rectangle(mouseLocation, new Size(0, 0));
                    pointList = new System.Collections.ArrayList();
                    IsEditing = false;
                }
                else
                {
                    IsEditing = true;
                    this.beginPoint = mouseLocation;
                    this.endPoint = mouseLocation;
                    editDrawRect = new Rectangle(mouseLocation, new Size(0, 0));
                    editSelectRect = new Rectangle(mouseLocation, new Size(0, 0));
                    pointList = new System.Collections.ArrayList();

                    //处理画刷的落笔点，当鼠标只是点一下，不移动的情况下，以鼠标点为中心画一个圆点；
                    pointList.Add(handleEdit_getBrushPoint(mouseLocation));
                }
                //清除编辑选中项
                ImageEditObject editObject;
                for (int i = 0; i < this.imageEditList.Count; i++)
                {
                    editObject = (ImageEditObject)this.imageEditList[i];
                    if (editObject != null)
                    {
                        editObject.Selected = false;
                    }
                }
                this.lbl_CutImage.Refresh();
                return;
            }
            //非编辑状态清空编辑数据
            editDrawRect = new Rectangle(mouseLocation, new Size(0, 0));
            editSelectRect = new Rectangle(mouseLocation, new Size(0, 0));
            pointList = new System.Collections.ArrayList();
        }

        /// <summary>
        /// 截取区域图片的鼠标按下事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_CutImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.pnl_Palette.Visible)
            {
                this.pnl_Palette.Hide();
                UpdateCutInfoLabel(UpdateUIMode.None);
            }

            if (e.Button == System.Windows.Forms.MouseButtons.Left && e.Clicks == 1)
            {
                handleEdit_CutImageMouseDown(e.Location);

                this.downPoint = e.Location;
                this.upPoint = e.Location;
                this.mouseLocation = e.Location;

                CursorType csType = CursorType.None;
                int resizeWidth = 5;
                if (e.X < resizeWidth)
                {
                    csType = CursorType.SizeLeft;
                }
                if (e.X > this.lbl_CutImage.Width - resizeWidth)
                {
                    csType = CursorType.SizeRight;
                }
                if (e.Y < resizeWidth)
                {
                    csType = CursorType.SizeTop;
                }
                if (e.Y > this.lbl_CutImage.Height - resizeWidth)
                {
                    csType = CursorType.SizeBottom;
                }
                if (e.X < resizeWidth && e.Y < resizeWidth)
                {
                    csType = CursorType.SizeTopLeft;
                }
                if (e.X > this.lbl_CutImage.Width - resizeWidth && e.Y > this.lbl_CutImage.Height - resizeWidth)
                {
                    csType = CursorType.SizeBottomRight;
                }
                if (e.X < resizeWidth && e.Y > this.lbl_CutImage.Height - resizeWidth)
                {
                    csType = CursorType.SizeBottomLeft;
                }
                if (e.X > this.lbl_CutImage.Width - resizeWidth && e.Y < resizeWidth)
                {
                    csType = CursorType.SizeTopRight;
                }
                if (csType == CursorType.None)
                {
                    csType = CursorType.SizeAll;
                }
                this.cursorType = csType;
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
                this.lbl_CutImage.ContextMenuStrip = this.contextMenuStrip2;
            }
        }

        /// <summary>
        /// 截取区域图片的鼠标抬起事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_CutImage_MouseUp(object sender, MouseEventArgs e)
        {
            //鼠标是左键抬起的情况才进行处理,右键单击是显示右键菜单
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.cursorType = CursorType.None;
                ImageEditObject editObject = null;
                if (this.IsEditing && (this.editDrawRect.Width != 0 || this.editDrawRect.Height != 0 || this.pointList.Count != 0))
                {
                    float penSize = 1.0f;
                    if (cbb_penSize.SelectedItem != null && cbb_penSize.SelectedItem.ToString().Length != 0)
                    {
                        float.TryParse(cbb_penSize.SelectedItem.ToString(), out penSize);
                    }
                    switch (this.imageEditMode)
                    {
                        case ImageEditMode.Rectangle:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(lbl_penColor.BackColor, penSize);
                            editObject.BackBrush = new SolidBrush(lbl_fillColor.BackColor);
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Ellipse:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(lbl_penColor.BackColor, penSize);
                            editObject.BackBrush = new SolidBrush(lbl_fillColor.BackColor);
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Arrowhead:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(lbl_penColor.BackColor, penSize);
                            editObject.BeginPoint = this.downPoint;
                            editObject.EndPoint = this.mouseLocation;
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Brush:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(lbl_penColor.BackColor, penSize);
                            editObject.SetBrushPoints(this.pointList);
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Text:
                            if (this.editDrawRect.Width == 0 && this.editDrawRect.Height == 0)
                            {
                                if (this.imageEditList.Count != 0)
                                {
                                    ImageEditObject prevEditObject = (ImageEditObject)this.imageEditList[this.imageEditList.Count - 1];
                                    if (prevEditObject != null && prevEditObject.EditMode == ImageEditMode.Text && prevEditObject.Text.Length == 0)
                                    {
                                        this.imageEditList.Remove(prevEditObject);
                                        break;
                                    }
                                }
                            }

                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(this.lbl_TextProColor.BackColor, 1.0f);

                            TextBox tb = new TextBox();

                            //Graphics g = Graphics.FromImage(bmpTextBox);
                            //g.Clear(Color.Blue);
                            //g.FillEllipse(Brushes.Beige, 10, 10, 500, 400);
                            //g.Dispose();
                            //tb.BackgroundImage = bmpTextBox;

                            tb.Name = "";

                            editObject.TextFont = this.editTextFont.GetFont();
                            //如果用户只是单击鼠标，并没有拖动，则根据字体的宽高来设置编辑框的大小
                            if (this.editDrawRect.Width == 0 && this.editDrawRect.Height == 0)
                            {
                                tb.Name = "0";
                                editObject.EditRect.Width = (int)editObject.TextFont.Size;
                                editObject.EditRect.Height = editObject.TextFont.Height;
                                editObject.EditRect.Location = new Point(editObject.EditRect.Left, editObject.EditRect.Top - editObject.TextFont.Height / 2);

                                this.editDrawRect.Width = editObject.EditRect.Width;
                                this.editDrawRect.Height = editObject.EditRect.Height + 3;
                                this.editDrawRect.Location = editObject.EditRect.Location;
                            }
                            else
                            {
                                //如果编辑框小于字体的宽度，则将其设置为字体的宽度
                                if (this.editDrawRect.Width < (int)editObject.TextFont.Size)
                                {
                                    editObject.EditRect.Width = (int)editObject.TextFont.Size;
                                    this.editDrawRect.Width = editObject.EditRect.Width;
                                }
                                //如果编辑框小于字体的高度，则将其设置为字体的高度
                                if (this.editDrawRect.Height < (int)editObject.TextFont.Height)
                                {
                                    editObject.EditRect.Height = editObject.TextFont.Height;
                                    this.editDrawRect.Height = editObject.EditRect.Height + 3;
                                }
                            }

                            this.imageEditList.Add(editObject);

                            tb.Multiline = true;
                            tb.ScrollBars = ScrollBars.None;
                            tb.Left = this.editDrawRect.Left + 3;
                            tb.Top = this.editDrawRect.Top + 3;
                            tb.Width = this.editDrawRect.Width - 5;
                            tb.Height = this.editDrawRect.Height - 5;
                            tb.BorderStyle = BorderStyle.None;
                            tb.Font = editObject.TextFont;
                            tb.ForeColor = this.lbl_TextProColor.BackColor;

                            //使用System.Windows.Forms.ImeMode.NoControl解决不能输入中文的问题；
                            tb.ImeMode = System.Windows.Forms.ImeMode.NoControl;
                            tb.TextChanged += new EventHandler(tb_TextChanged);

                            this.lbl_CutImage.Controls.Add(tb);
                            tb.Show();
                            tb.Focus();
                            break;
                        case ImageEditMode.ToolTips:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(lbl_penColor.BackColor, penSize);
                            editObject.BackBrush = new SolidBrush(lbl_fillColor.BackColor);
                            editObject.BeginPoint = this.downPoint;
                            editObject.EndPoint = e.Location;
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Undo:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(Color.Red, 1.0f);
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Save:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(Color.Red, 1.0f);
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Upload:
                            editObject = new ImageEditObject(this.imageEditMode, this.editDrawRect);
                            editObject.EditPen = new Pen(Color.Red, 1.0f);
                            this.imageEditList.Add(editObject);
                            break;
                        case ImageEditMode.Cancel:
                            ExitCutImage(true);
                            break;
                        case ImageEditMode.Ok:
                            ExecCutImage(false, false);
                            break;
                        default: break;
                    }
                }

                this.IsEditing = false;
                this.editSelectRect = this.editDrawRect;
                this.editDrawRect = new Rectangle(e.Location, new Size(0, 0));
                this.pointList = new System.Collections.ArrayList();
                this.lbl_CutImage.Refresh();
            }
        }

        /// <summary>
        /// 文本编辑框内容改变事件处理程序
        /// 根据内容处理编辑框的大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tb_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb == null) { return; }

            if (this.imageEditList.Count == 0)
            {
                return;
            }

            ImageEditObject editObject = (ImageEditObject)this.imageEditList[this.imageEditList.Count - 1];
            if (editObject == null || editObject.EditMode != ImageEditMode.Text)
            {
                return;
            }

            bool changeFlag = false;

            if (tb.Name == "0")
            {
                int maxWidth = (int)tb.Font.Size;
                for (int i = 0; i < tb.Lines.Length; i++)
                {
                    int lineLength = (int)(tb.Lines[i].Length * tb.Font.Size);
                    if (maxWidth < lineLength)
                    {
                        maxWidth = lineLength;
                    }
                }

                if (tb.Width < maxWidth)
                {
                    tb.Width = maxWidth;
                    editObject.EditRect.Width = tb.Width + 10;
                    editSelectRect.Width = tb.Width + 5;
                    changeFlag = true;
                }

                if (tb.Height < tb.Lines.Length * editObject.TextFont.Height)
                {
                    tb.Height = tb.Lines.Length * editObject.TextFont.Height;
                    editObject.EditRect.Height = tb.Height + 3;
                    editSelectRect.Height = tb.Height + 3;
                    changeFlag = true;
                }
            }
            else
            {
                Bitmap bmp_lbl = new Bitmap(this.lbl_CutImage.Width, this.lbl_CutImage.Height);
                Graphics gic = Graphics.FromImage(bmp_lbl);
                int charFit = 0;
                int lineFill = 0;
                gic.MeasureString(tb.Text, editObject.TextFont, new SizeF(editObject.EditRect.Width, editObject.EditRect.Height * 10), StringFormat.GenericDefault, out charFit, out lineFill);
                gic.Dispose();

                //label1.Text = "charFit = " + charFit + "\r\n";
                //label1.Text += "lineFill = " + lineFill + "\r\n";
                //label1.Text += "tb.Lines.Length = " + tb.Lines.Length + "\r\n";
                //label1.Text += "editObject.EditRect.Width = " + editObject.EditRect.Width + "\r\n";
                //label1.Text += "editObject.EditRect.Height = " + editObject.EditRect.Height + "\r\n";
                //label1.Show();

                if (tb.Height < lineFill * editObject.TextFont.Height)
                {
                    tb.Height = lineFill * editObject.TextFont.Height;
                    editObject.EditRect.Height = tb.Height + 3;
                    editSelectRect.Height = tb.Height + 3;
                    changeFlag = true;
                }
            }

            if (changeFlag)
            {
                lbl_CutImage.Refresh();
            }

        }

        /// <summary>
        /// 截取区域图片的鼠标移动事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_CutImage_MouseMove(object sender, MouseEventArgs e)
        {
            #region 处理编辑状态的图标
            if (this.imageEditMode != ImageEditMode.None)
            {
                // 如果截取区域不可见,则退出处理过程
                if (!this.IsEditing) { return; }

                if (this.imageEditMode == ImageEditMode.Brush)
                {
                    //如果是使用画刷编辑工具,则添加鼠标经过的点;
                    this.pointList.Add(handleEdit_getBrushPoint(e.Location));
                }

                this.mouseLocation = e.Location;

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

                HandleEditDrawRect(pntBgn, pntEnd);

                long mouseMoveTimeStep = System.DateTime.Now.Ticks - lastMouseMoveTime;
                if (mouseMoveTimeStep > 200)
                {
                    this.lbl_CutImage.Refresh();
                }
                lastMouseMoveTime = System.DateTime.Now.Ticks;

                return;
            }
            #endregion

            #region // 如果鼠标没有按下,则只处理鼠标形状

            if (this.cursorType == CursorType.None)
            {
                int resizeWidth = 5; //边框区域鼠标移入后变为Resize图标的宽度
                this.Cursor = Cursors.SizeAll;
                if (e.X < resizeWidth)
                {
                    this.Cursor = Cursors.SizeWE;

                }
                if (e.X > this.lbl_CutImage.Width - resizeWidth)
                {
                    this.Cursor = Cursors.SizeWE;

                }
                if (e.Y < resizeWidth)
                {
                    this.Cursor = Cursors.SizeNS;

                }
                if (e.Y > this.lbl_CutImage.Height - resizeWidth)
                {
                    this.Cursor = Cursors.SizeNS;

                }
                if (e.X < resizeWidth && e.Y < resizeWidth)
                {
                    this.Cursor = Cursors.SizeNWSE;

                }
                if (e.X > this.lbl_CutImage.Width - resizeWidth && e.Y > this.lbl_CutImage.Height - resizeWidth)
                {
                    this.Cursor = Cursors.SizeNWSE;

                }
                if (e.X < resizeWidth && e.Y > this.lbl_CutImage.Height - resizeWidth)
                {
                    this.Cursor = Cursors.SizeNESW;

                }
                if (e.X > this.lbl_CutImage.Width - resizeWidth && e.Y < resizeWidth)
                {
                    this.Cursor = Cursors.SizeNESW;

                }

                return; // 如果鼠标没有按下,则只处理鼠标形状
            }
            #endregion

            #region // 根据鼠标按下时的光标状态,对图片截取区域执行相关的操作
            switch (this.cursorType)
            {
                case CursorType.SizeAll:
                    CutImage_SizeAll(e);
                    break;
                case CursorType.SizeTop:
                    CutImage_SizeTop(e);
                    break;
                case CursorType.SizeRight:
                    CutImage_SizeRight(e);
                    break;
                case CursorType.SizeBottom:
                    CutImage_SizeBottom(e);
                    break;
                case CursorType.SizeLeft:
                    CutImage_SizeLeft(e);
                    break;
                case CursorType.SizeTopRight:
                    CutImage_SizeTopRight(e);
                    break;
                case CursorType.SizeBottomRight:
                    CutImage_SizeBottomRight(e);
                    break;
                case CursorType.SizeBottomLeft:
                    CutImage_SizeBottomLeft(e);
                    break;
                case CursorType.SizeTopLeft:
                    CutImage_SizeTopLeft(e);
                    break;
                default: break;
            }
            #endregion // 根据鼠标按下时的光标状态,对图片截取区域执行相关的操作
        }

        /// <summary>
        /// 更新截图信息显示框，截图编辑工具框
        /// </summary>
        private void UpdateCutInfoLabel(UpdateUIMode updateUIMode) // UpdateUIMode updateUIMode = UpdateUIMode.None
        {
            long mouseMoveTimeStep = System.DateTime.Now.Ticks - lastMouseMoveTime;
            if (mouseMoveTimeStep < 50 && updateUIMode == UpdateUIMode.None) { return; }
            lastMouseMoveTime = System.DateTime.Now.Ticks;

            if (this.lbl_CutImage.Visible || (updateUIMode & UpdateUIMode.ShowCutImage) != UpdateUIMode.None)
            {
                this.lbl_CutImage.SetBounds(this.cutImageRect.Left, this.cutImageRect.Top, this.cutImageRect.Width, this.cutImageRect.Height, BoundsSpecified.All);
                if (!this.lbl_CutImage.Visible)
                {
                    this.lbl_CutImage.Show();
                }
            }

            if (this.lbl_Info.Visible || (updateUIMode & UpdateUIMode.ShowInfoBox) != UpdateUIMode.None)
            {
                int top = this.lbl_CutImage.Top - this.lbl_Info.Height;
                if (top > 0)
                {
                    this.infoLocation.Y = top;

                    if (this.lbl_CutImage.Left + this.lbl_Info.Width < this.Width)
                    {
                        this.infoLocation.X = this.lbl_CutImage.Left;
                    }
                    else
                    {
                        this.infoLocation.X = this.lbl_CutImage.Left - this.lbl_Info.Width;
                    }
                }
                else
                {
                    this.infoLocation.Y = this.lbl_CutImage.Top + 5;

                    if (this.lbl_CutImage.Left + this.lbl_Info.Width < this.Width)
                    {
                        this.infoLocation.X = this.lbl_CutImage.Left + 5;
                    }
                    else
                    {
                        this.infoLocation.X = this.lbl_CutImage.Left - this.lbl_Info.Width;
                    }
                }

                this.lbl_Info.Location = this.infoLocation;
                this.lbl_Info.Refresh();
                if (!this.lbl_Info.Visible)
                {
                    this.lbl_Info.Show();
                }
            }

            if (this.lbl_ToolBox.Visible || (updateUIMode & UpdateUIMode.ShowToolBox) != UpdateUIMode.None)
            {
                Point toolBoxLocation = new Point(this.lbl_CutImage.Right - this.lbl_ToolBox.Width - 2, this.lbl_CutImage.Top + this.lbl_CutImage.Height);
                if (toolBoxLocation.X < 0) { toolBoxLocation.X = 0; }
                if (this.lbl_CutImage.Bottom + this.lbl_ToolBox.Height + 35 > this.Height)
                {
                    toolBoxLocation = new Point(this.lbl_CutImage.Right - this.lbl_ToolBox.Width, this.lbl_CutImage.Top - this.lbl_ToolBox.Height);
                    if (toolBoxLocation.Y - 35 < 0)
                    {
                        toolBoxLocation = new Point(this.lbl_CutImage.Left - this.lbl_ToolBox.Width, this.lbl_CutImage.Top);
                        if (toolBoxLocation.X < 0)
                        {
                            toolBoxLocation = new Point(this.lbl_CutImage.Right - this.lbl_ToolBox.Width - 5, this.lbl_CutImage.Top + 5);
                        }
                    }
                    if (toolBoxLocation.X < 0) { toolBoxLocation.X = 0; }

                }
                this.lbl_ToolBox.Location = toolBoxLocation;
                this.lbl_ToolBox.Refresh();
                if (!this.lbl_ToolBox.Visible)
                {
                    this.lbl_ToolBox.Show();
                }

                Point panelLocation;
                if (pnl_TextPro.Visible || (updateUIMode & UpdateUIMode.ShowTextPro) != UpdateUIMode.None)
                {
                    panelLocation = new Point(this.lbl_ToolBox.Left, this.lbl_ToolBox.Bottom + 2);
                    if (this.lbl_ToolBox.Top < this.lbl_CutImage.Top)
                    {
                        panelLocation = new Point(this.lbl_ToolBox.Left, this.lbl_ToolBox.Top - this.pnl_TextPro.Height - 2);
                    }
                    pnl_TextPro.Location = panelLocation;
                    if (!this.pnl_TextPro.Visible)
                    {
                        this.pnl_TextPro.Show();
                    }
                }
                if (pnl_penStyle.Visible || (updateUIMode & UpdateUIMode.ShowPenStyle) != UpdateUIMode.None)
                {
                    panelLocation = new Point(this.lbl_ToolBox.Left, this.lbl_ToolBox.Bottom + 2);
                    if (this.lbl_ToolBox.Top < this.lbl_CutImage.Top)
                    {
                        panelLocation = new Point(this.lbl_ToolBox.Left, this.lbl_ToolBox.Top - this.pnl_TextPro.Height - 2);
                    }
                    pnl_penStyle.Location = panelLocation;
                    if (!pnl_penStyle.Visible)
                    {
                        pnl_penStyle.Show();
                    }
                }
            }

            if (this.pictureBox_zoom.Visible || (updateUIMode & UpdateUIMode.ShowZoomBox) != UpdateUIMode.None)
            {
                Point zoomLocation = new Point(MousePosition.X + 15, MousePosition.Y + 22);
                if (zoomLocation.Y + this.pictureBox_zoom.Height > this.Height)
                {
                    if (zoomLocation.X + this.pictureBox_zoom.Width > this.Width)
                    {
                        zoomLocation = new Point(MousePosition.X - this.pictureBox_zoom.Width - 10, MousePosition.Y - this.pictureBox_zoom.Height - 10);
                    }
                    else
                    {
                        zoomLocation = new Point(MousePosition.X + 15, MousePosition.Y - this.pictureBox_zoom.Height - 15);
                    }
                }
                else
                {
                    if (zoomLocation.X + this.pictureBox_zoom.Width > this.Width)
                    {
                        zoomLocation = new Point(MousePosition.X - this.pictureBox_zoom.Width - 15, MousePosition.Y);
                    }
                }
                this.pictureBox_zoom.Location = zoomLocation;
                if (!this.pictureBox_zoom.Visible)
                {
                    this.pictureBox_zoom.Show();
                }
            }
        }

        /// <summary>
        /// 截图区域移动处理程序
        /// </summary>
        /// <param name="e"></param>
        private void CutImage_SizeAll(MouseEventArgs e)
        {
            this.endPoint = new Point(this.endPoint.X + e.X - this.downPoint.X, this.endPoint.Y + e.Y - this.downPoint.Y);
            if (this.endPoint.X < 0) { this.endPoint.X = -1; }
            if (this.endPoint.Y < 0) { this.endPoint.Y = -1; }
            if (this.endPoint.X + this.lbl_CutImage.Width > screenImage.Width + 5) { this.endPoint.X = screenImage.Width - this.lbl_CutImage.Width + 5; }
            if (this.endPoint.Y + this.lbl_CutImage.Height > screenImage.Height + 5) { this.endPoint.Y = screenImage.Height - this.lbl_CutImage.Height + 5; }
            this.cutImageRect.Location = new Point(this.endPoint.X - 2, this.endPoint.Y - 2);
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        private void CutImage_SizeTop(MouseEventArgs e)
        {
            int bottomY = this.lbl_CutImage.Location.Y + this.lbl_CutImage.Height;
            int endPointY = this.endPoint.Y + e.Y - this.downPoint.Y;

            if (endPointY > bottomY - 3)
            {
                this.downPoint = e.Location;
                this.cursorType = CursorType.SizeBottom;
                return;
            }
            this.endPoint.Y = endPointY;
            this.cutImageRect.Height = Math.Abs(this.cutImageRect.Height - e.Y + this.downPoint.Y);
            if (this.cutImageRect.Height < 5) { this.cutImageRect.Height = 5; }
            this.cutImageRect.Location = new Point(this.endPoint.X - 2, this.endPoint.Y - 2);
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        private void CutImage_SizeBottom(MouseEventArgs e)
        {
            if (e.Y < 3)
            {
                this.cursorType = CursorType.SizeTop;
                this.downPoint = e.Location;
                return;
            }

            this.cutImageRect.Height = e.Y + 3;
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        private void CutImage_SizeLeft(MouseEventArgs e)
        {
            int bottomX = this.lbl_CutImage.Location.X + this.lbl_CutImage.Width;
            int endPointX = this.endPoint.X + e.X - this.downPoint.X;

            if (endPointX > bottomX - 3)
            {
                this.downPoint = e.Location;
                this.cursorType = CursorType.SizeRight;
                return;
            }
            this.endPoint.X = endPointX;
            this.cutImageRect.Width = Math.Abs(this.cutImageRect.Width - e.X + this.downPoint.X);
            if (this.cutImageRect.Width < 5) { this.cutImageRect.Width = 5; }
            this.cutImageRect.Location = new Point(this.endPoint.X - 2, this.endPoint.Y - 2);
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        private void CutImage_SizeRight(MouseEventArgs e)
        {
            if (e.X < 3)
            {
                this.cursorType = CursorType.SizeLeft;
                this.downPoint = e.Location;
                return;
            }

            //this.lbl_CutImage.Width = e.X + 3;
            this.cutImageRect.Width = e.X + 3;
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        /// <summary>
        /// SizeTopLeft
        /// </summary>
        /// <param name="e"></param>
        private void CutImage_SizeTopLeft(MouseEventArgs e)
        {
            int bottomX = this.lbl_CutImage.Location.X + this.lbl_CutImage.Width;
            int bottomY = this.lbl_CutImage.Location.Y + this.lbl_CutImage.Height;
            int endPointX = this.endPoint.X + e.X - this.downPoint.X;
            int endPointY = this.endPoint.Y + e.Y - this.downPoint.Y;

            if (endPointX > bottomX - 3)
            {
                this.downPoint = e.Location;
                this.cursorType = CursorType.SizeTopRight;
                return;
            }
            if (endPointY > bottomY - 3)
            {
                this.downPoint = e.Location;
                this.cursorType = CursorType.SizeBottomLeft;
                return;
            }

            this.endPoint.X = endPointX;
            this.endPoint.Y = endPointY;
            this.cutImageRect.Width = Math.Abs(this.cutImageRect.Width - e.X + this.downPoint.X);
            if (this.cutImageRect.Width < 5) { this.cutImageRect.Width = 5; }
            this.cutImageRect.Height = Math.Abs(this.cutImageRect.Height - e.Y + this.downPoint.Y);
            if (this.cutImageRect.Height < 5) { this.cutImageRect.Height = 5; }
            this.cutImageRect.Location = new Point(this.endPoint.X - 2, this.endPoint.Y - 2);
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        /// <summary>
        /// SizeTopRight
        /// </summary>
        /// <param name="e"></param>
        private void CutImage_SizeTopRight(MouseEventArgs e)
        {
            int bottomY = this.lbl_CutImage.Location.Y + this.lbl_CutImage.Height;
            int endPointY = this.endPoint.Y + e.Y - this.downPoint.Y;

            if (e.X < 3)
            {
                this.cursorType = CursorType.SizeTopLeft;
                this.downPoint = e.Location;
                return;
            }
            if (endPointY > bottomY - 3)
            {
                this.downPoint = e.Location;
                this.cursorType = CursorType.SizeBottomRight;
                return;
            }
            this.endPoint.Y = endPointY;
            this.cutImageRect.Width = e.X + 3;
            this.cutImageRect.Height = Math.Abs(this.cutImageRect.Height - e.Y + this.downPoint.Y);
            if (this.cutImageRect.Height < 5) { this.cutImageRect.Height = 5; }
            this.cutImageRect.Location = new Point(this.endPoint.X - 2, this.endPoint.Y - 2);
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        /// <summary>
        /// SizeBottomLeft
        /// </summary>
        /// <param name="e"></param>
        private void CutImage_SizeBottomLeft(MouseEventArgs e)
        {
            int bottomX = this.lbl_CutImage.Location.X + this.lbl_CutImage.Width;
            int endPointX = this.endPoint.X + e.X - this.downPoint.X;

            if (endPointX > bottomX - 3)
            {
                this.downPoint = e.Location;
                this.cursorType = CursorType.SizeBottomRight;
                return;
            }
            if (e.Y < 3)
            {
                this.cursorType = CursorType.SizeTopLeft;
                this.downPoint = e.Location;
                return;
            }
            this.endPoint.X = endPointX;
            this.cutImageRect.Height = e.Y + 3;
            this.cutImageRect.Width = Math.Abs(this.cutImageRect.Width - e.X + this.downPoint.X);
            if (this.cutImageRect.Width < 5) { this.cutImageRect.Width = 5; }
            this.cutImageRect.Location = new Point(this.endPoint.X - 2, this.endPoint.Y - 2);

            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        /// <summary>
        /// SizeBottomRight
        /// </summary>
        /// <param name="e"></param>
        private void CutImage_SizeBottomRight(MouseEventArgs e)
        {
            if (e.X < 3 && e.Y < 3)
            {
                this.cursorType = CursorType.SizeTopLeft;
                this.downPoint = e.Location;
                return;
            }
            else
            {
                if (e.X < 3)
                {
                    this.cursorType = CursorType.SizeBottomLeft;
                    this.downPoint = e.Location;
                    return;
                }
                if (e.Y < 3)
                {
                    this.cursorType = CursorType.SizeTopRight;
                    this.downPoint = e.Location;
                    return;
                }
            }

            this.cutImageRect.Width = e.X + 3;
            this.cutImageRect.Height = e.Y + 3;
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        /// <summary>
        /// 记录鼠标状态，用于截图区域的移动、放大、缩小。
        /// </summary>
        private enum CursorType
        {
            None = 0,
            SizeAll = 1,
            SizeTop = 2,
            SizeTopRight = 3,
            SizeRight = 4,
            SizeBottomRight = 5,
            SizeBottom = 6,
            SizeBottomLeft = 7,
            SizeLeft = 8,
            SizeTopLeft = 9
        }

        /// <summary>
        /// 托盘菜单退出事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_exit_Click(object sender, EventArgs e)
        {
            Program.timerThreadRun = false;
            Program.timerThread.Abort();
            Application.Exit();
        }

        /// <summary>
        /// 托盘菜单截图事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_cutImage_Click(object sender, EventArgs e)
        {
            ShowForm();
        }

        /// <summary>
        /// 工具栏按钮的宽度
        /// </summary>
        private int[] toolBoxsWidth = new int[] { 22, 22, 22, 22, 22, 22, 22, 22, 35, 22, 54 };

        /// <summary>
        /// 工具样式按钮的左侧边距
        /// </summary>
        private int[] toolBoxsLeft = new int[] { 7, 33, 59, 85, 111, 137, 171, 197, 223, 268, 294 };

        /// <summary>
        /// 工具栏按钮提示信息
        /// </summary>
        private string[] toolBoxsTip = new string[] { "矩形工具", "椭圆工具", "箭头工具", "画刷工具", "文字工具", "提示工具", "撤消编辑", "保存截图", "上传截图", "退出截图", "完成截图" };

        /// <summary>
        /// 工具栏绘制事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_ToolBox_Paint(object sender, PaintEventArgs e)
        {
            Bitmap bmp_lbl = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height);
            Graphics g = Graphics.FromImage(bmp_lbl);

            g.DrawImage(Properties.Resources.ToolsBox, e.ClipRectangle, e.ClipRectangle, GraphicsUnit.Pixel);

            bool tipFlag = false;
            for (int i = 0; i < toolBoxsLeft.Length; i++)
            {
                Rectangle fcs_Rect = new Rectangle(toolBoxsLeft[i], 3, toolBoxsWidth[i], toolHeight);
                Rectangle sld_Rect = new Rectangle(toolBoxsLeft[i], 29, toolBoxsWidth[i], toolHeight);

                if (fcs_Rect.Contains(this.mouseDownToolBoxLocation))
                {
                    g.DrawImage(Properties.Resources.ToolsBox, fcs_Rect, sld_Rect, GraphicsUnit.Pixel);


                }
                else
                {
                    if (fcs_Rect.Contains(this.mouseInToolBoxLocation))
                    {
                        g.DrawImage(Properties.Resources.ToolsBox, fcs_Rect, sld_Rect, GraphicsUnit.Pixel);
                    }
                }
                //如果选中的是矩形工具，则根据子类型绘制按钮的图标
                if (i == 0)
                {
                    Rectangle src_Rect = new Rectangle(0, 0, 22, 20);
                    //绘制工具按钮的背景
                    if (fcs_Rect.Contains(this.mouseDownToolBoxLocation))
                    {
                        g.DrawImage(Properties.Resources.ToolBox_Selected, fcs_Rect, src_Rect, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        if (fcs_Rect.Contains(this.mouseInToolBoxLocation))
                        {
                            g.DrawImage(Properties.Resources.ToolBox_Selected, fcs_Rect, src_Rect, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            g.DrawImage(Properties.Resources.ToolBox_Normal, fcs_Rect, src_Rect, GraphicsUnit.Pixel);
                        }
                    }

                    Rectangle dst_Rect = new Rectangle(fcs_Rect.Left + 3, fcs_Rect.Top + 2, 16, 16);
                    src_Rect = new Rectangle(0, 0, 16, 16);
                    //根据子类型绘制按钮的图标
                    switch (this.imageSubEditMode)
                    {
                        case ImageSubEditMode.Rectangle:
                            g.DrawImage(Properties.Resources.Icon_Rectangle, dst_Rect, src_Rect, GraphicsUnit.Pixel);

                            break;
                        case ImageSubEditMode.CircularRectangle:
                            g.DrawImage(Properties.Resources.Icon_Circular, dst_Rect, src_Rect, GraphicsUnit.Pixel);
                            break; ;
                        case ImageSubEditMode.Ellipse:
                            g.DrawImage(Properties.Resources.Icon_Ellipse, dst_Rect, src_Rect, GraphicsUnit.Pixel);
                            break;
                        case ImageSubEditMode.Arrowhead:
                            g.DrawImage(Properties.Resources.Icon_Arrowhead, dst_Rect, src_Rect, GraphicsUnit.Pixel);
                            break;
                        default: break;
                    }

                    //绘制三角块
                    Rectangle dstRect_SanJiaoKuai = new Rectangle(fcs_Rect.Left + 15, fcs_Rect.Top + 15, 5, 3);
                    Rectangle srcRect_SanJiaoKuai = new Rectangle(0, 0, 5, 3);
                    g.DrawImage(Properties.Resources.SanJiaoKuai, dstRect_SanJiaoKuai, srcRect_SanJiaoKuai, GraphicsUnit.Pixel);
                }
                //==============================================================================================================

                //如果选中的是提示工具，则根据子类型绘制按钮的图标
                if (i == 5)
                {
                    Rectangle src_Rect = new Rectangle(0, 0, 22, 20);
                    //绘制工具按钮的背景
                    if (fcs_Rect.Contains(this.mouseDownToolBoxLocation))
                    {
                        g.DrawImage(Properties.Resources.ToolBox_Selected, fcs_Rect, src_Rect, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        if (fcs_Rect.Contains(this.mouseInToolBoxLocation))
                        {
                            g.DrawImage(Properties.Resources.ToolBox_Selected, fcs_Rect, src_Rect, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            g.DrawImage(Properties.Resources.ToolBox_Normal, fcs_Rect, src_Rect, GraphicsUnit.Pixel);
                        }
                    }

                    Rectangle dst_Rect = new Rectangle(fcs_Rect.Left + 3, fcs_Rect.Top + 2, 16, 16);
                    src_Rect = new Rectangle(0, 0, 16, 16);
                    //根据子类型绘制按钮的图标
                    switch (this.imageSubEditMode)
                    {
                        case ImageSubEditMode.Rectangle:
                            g.DrawImage(Properties.Resources.Icon_ToolTips, dst_Rect, src_Rect, GraphicsUnit.Pixel);

                            break;
                        case ImageSubEditMode.CircularRectangle:
                            g.DrawImage(Properties.Resources.Icon_ToolTips, dst_Rect, src_Rect, GraphicsUnit.Pixel);
                            break; ;
                        case ImageSubEditMode.Ellipse:
                            g.DrawImage(Properties.Resources.Icon_ToolTips, dst_Rect, src_Rect, GraphicsUnit.Pixel);
                            break;
                        case ImageSubEditMode.Arrowhead:
                            g.DrawImage(Properties.Resources.Icon_ToolTips, dst_Rect, src_Rect, GraphicsUnit.Pixel);
                            break;
                        default: break;
                    }

                    //绘制三角块
                    Rectangle dstRect_SanJiaoKuai = new Rectangle(fcs_Rect.Left + 15, fcs_Rect.Top + 15, 5, 3);
                    Rectangle srcRect_SanJiaoKuai = new Rectangle(0, 0, 5, 3);
                    g.DrawImage(Properties.Resources.SanJiaoKuai, dstRect_SanJiaoKuai, srcRect_SanJiaoKuai, GraphicsUnit.Pixel);
                }
                //==============================================================================================================

                if (fcs_Rect.Contains(this.mouseInToolBoxLocation))
                {
                    this.toolTip1.SetToolTip(this.lbl_ToolBox, toolBoxsTip[i]);
                    tipFlag = true;
                }
            }
            if (!tipFlag)
            {
                this.toolTip1.SetToolTip(this.lbl_ToolBox, "");
            }

            e.Graphics.DrawImage(bmp_lbl, 0, 0);
            g.Dispose();
            bmp_lbl.Dispose();
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

        /// <summary>
        /// 延时5秒截图计时时间到了事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_cutImage_Tick(object sender, EventArgs e)
        {
            timer_cutImage.Stop();

            ShowForm();
        }

        /// <summary>
        /// 托盘菜单延时5秒截图事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_cutImage5_Click(object sender, EventArgs e)
        {
            timer_cutImage.Start();
        }

        /// <summary>
        /// 工具栏单击事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_ToolBox_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 工具栏鼠标移入事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_ToolBox_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 工具栏鼠标移动事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_ToolBox_MouseMove(object sender, MouseEventArgs e)
        {
            this.mouseInToolBoxLocation = e.Location;

            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        /// <summary>
        /// 工具栏鼠标离开事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_ToolBox_MouseLeave(object sender, EventArgs e)
        {
            this.mouseInToolBoxLocation = new Point(-1, -1);
            this.toolTip1.SetToolTip(this.lbl_ToolBox, "");
            if (this.ToolBoxVisible) { this.lbl_ToolBox.Refresh(); }
        }

        /// <summary>
        /// 工具栏鼠标按下事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_ToolBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.pnl_Palette.Visible)
            {
                this.pnl_Palette.Hide();
            }

            handelEdit_StateChanged();

            this.mouseDownToolBoxLocation = e.Location;
            for (int i = 0; i < toolBoxsLeft.Length; i++)
            {
                Rectangle fcs_Rect = new Rectangle(toolBoxsLeft[i], 3, toolBoxsWidth[i], toolHeight);
                if (fcs_Rect.Contains(e.Location))
                {
                    // 如果鼠标在相同的编辑模式(工具按钮)上按下,则取消该编辑模式;
                    // 比如用户选中了矩形工具后,又点击矩形工具按钮,这将取消矩形工具的选中状态，
                    // 退出矩形工具编辑模式
                    if (this.imageEditMode == (ImageEditMode)(i + 1))
                    {
                        this.imageEditMode = ImageEditMode.None;
                        this.mouseDownToolBoxLocation = new Point(-1, -1);
                        this.pnl_TextPro.Hide();
                        this.pnl_penStyle.Hide();
                    }
                    else //否则将进行编辑模式、按钮状态的设置
                    {
                        //设置编辑模式
                        this.imageEditMode = (ImageEditMode)(i + 1);

                        //设置按钮状态及工具选项是否显示
                        //如果用户选中的不是文字工具，则隐藏文字选项设置框
                        if (this.imageEditMode != ImageEditMode.Text)
                        {
                            this.pnl_TextPro.Hide();
                        }
                        //如果用户选中的不是矩形工具、椭圆工具、箭头工具、画刷工具、提示工具，
                        //则隐藏对应的属性设置对话框
                        if (this.imageEditMode != ImageEditMode.Rectangle
                            && this.imageEditMode != ImageEditMode.Ellipse
                            && this.imageEditMode != ImageEditMode.Arrowhead
                            && this.imageEditMode != ImageEditMode.Brush
                            && this.imageEditMode != ImageEditMode.ToolTips)
                        {
                            this.pnl_penStyle.Hide();
                        }

                        //如果用户选中的是矩形工具，则开启弹出菜单计时器，实现鼠标按下一秒显示菜单的功能
                        if (this.imageEditMode == ImageEditMode.Rectangle)
                        {
                            timer_ToolBox.Start();
                        }
                    }
                }
            }

            UpdateUIMode updateUiMode = UpdateUIMode.None;

            switch (this.imageEditMode)
            {
                case ImageEditMode.Rectangle:
                    switch (this.imageSubEditMode)
                    {
                        case ImageSubEditMode.Rectangle:
                            SetToolsRuntimeStyle(this.trsRectangle);
                            break;
                        case ImageSubEditMode.CircularRectangle:
                            SetToolsRuntimeStyle(this.trsCircular);
                            break;
                        case ImageSubEditMode.Ellipse:
                            SetToolsRuntimeStyle(this.trsEllipse);
                            break;
                        case ImageSubEditMode.Arrowhead:
                            SetToolsRuntimeStyle(this.trsArrowhead);
                            break;
                        case ImageSubEditMode.Polygon:
                            SetToolsRuntimeStyle(this.trsPolygon);
                            break;
                        case ImageSubEditMode.L_Shape:
                            SetToolsRuntimeStyle(this.trsLShapeTool);
                            break;
                        default:
                            SetToolsRuntimeStyle(this.trsRectangle);
                            break;
                    }
                    updateUiMode = UpdateUIMode.ShowPenStyle;
                    break;
                case ImageEditMode.Ellipse:
                    SetToolsRuntimeStyle(this.trsEllipse);
                    updateUiMode = UpdateUIMode.ShowPenStyle;
                    break;
                case ImageEditMode.Arrowhead:
                    SetToolsRuntimeStyle(this.trsArrowhead);
                    updateUiMode = UpdateUIMode.ShowPenStyle;
                    break;
                case ImageEditMode.Brush:
                    SetToolsRuntimeStyle(this.trsBrush);
                    updateUiMode = UpdateUIMode.ShowPenStyle;
                    break;
                case ImageEditMode.ToolTips:
                    SetToolsRuntimeStyle(this.trsToolTips);
                    updateUiMode = UpdateUIMode.ShowPenStyle;
                    break;
                case ImageEditMode.Text:
                    //pnl_TextPro.Show();
                    updateUiMode = UpdateUIMode.ShowTextPro;
                    break;
                case ImageEditMode.Undo:
                    ExecUndoEdit();
                    break;
                case ImageEditMode.Save:
                    handelEdit_StateChanged();
                    ExecCutImage(false, false);
                    break;
                case ImageEditMode.Upload:
                    handelEdit_StateChanged();
                    ExecCutImage(false, true);
                    break;
                case ImageEditMode.Cancel:
                    ExitCutImage(true);
                    break;
                case ImageEditMode.Ok:
                    handelEdit_StateChanged();
                    ExecCutImage(false, false);
                    break;
                default:
                    //if (this.ToolBoxVisible) { this.lbl_ToolBox.Refresh(); }
                    break;
            }
            UpdateCutInfoLabel(updateUiMode);
        }

        private void SetToolsRuntimeStyle(ToolsRuntimeStyle toolsRuntimeStyle)
        {
            this.currentToolStyle = toolsRuntimeStyle;
            lbl_fillColor.BackColor = toolsRuntimeStyle.FillColor;
            cbb_fillStyle.SelectedText = toolsRuntimeStyle.FillType;
            lbl_penColor.BackColor = toolsRuntimeStyle.BorderColor;
            for (int i = 0; i < cbb_penSize.Items.Count; i++)
            {
                ComboBoxItem cbItem = (ComboBoxItem)cbb_penSize.Items[i];
                if (cbItem != null && cbItem.Value.Equals(toolsRuntimeStyle.BorderWidth.ToString()))
                {
                    cbb_penSize.SelectedItem = cbItem;
                    break;
                }
            }
            cbb_penStyle.SelectedText = toolsRuntimeStyle.BorderStyle;
        }

        /// <summary>
        /// 执行取消编辑事件处理
        /// </summary>
        private void ExecUndoEdit()
        {
            handelEdit_StateChanged();

            this.imageEditMode = ImageEditMode.None;
            this.mouseDownToolBoxLocation = new Point(-1, -1);
            if (this.imageEditList.Count != 0)
            {
                this.imageEditList.RemoveAt(this.imageEditList.Count - 1);
            }
            this.lbl_CutImage.Refresh();
        }

        /// <summary>
        /// 截图区域鼠标移入事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_CutImage_MouseEnter(object sender, EventArgs e)
        {
            if (this.imageEditMode == ImageEditMode.None) { return; }

            switch (this.imageEditMode)
            {
                case ImageEditMode.Rectangle:

                    //Properties.Resources.Cursor_Cross
                    this.Cursor = cursorCross;
                    break;
                case ImageEditMode.Ellipse:
                    this.Cursor = cursorCross;
                    break;
                case ImageEditMode.Arrowhead:
                    this.Cursor = cursorCross;
                    break;
                case ImageEditMode.Brush:
                    this.Cursor = cursorCross;
                    break;
                case ImageEditMode.Text:
                    this.Cursor = cursorText;
                    break;
                case ImageEditMode.ToolTips:
                    this.Cursor = cursorCross;
                    break;
                default:
                    this.Cursor = Cursors.Default;
                    break;
            }
        }

        /// <summary>
        /// 截图区域鼠标离开事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_CutImage_MouseLeave(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 文字颜色选择框绘制事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_TextProColor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.colorButton, 0, 0);
        }

        /// <summary>
        /// 文字颜色选择框鼠标按下事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbl_TextProColor_MouseDown(object sender, MouseEventArgs e)
        {
            Point paletteLocation = new Point(this.pnl_TextPro.Left + this.lbl_TextProColor.Left, this.pnl_TextPro.Top + this.lbl_TextProColor.Bottom);
            if (this.pnl_TextPro.Top + paletteLocation.Y + this.lbl_TextProColor.Height > this.Height)
            {
                paletteLocation = new Point(this.pnl_TextPro.Left + this.lbl_TextProColor.Left, this.pnl_TextPro.Top + this.lbl_TextProColor.Top - this.pnl_Palette.Height);
                PointToScreen(paletteLocation);
            }
            PaletteTargetLabel = this.lbl_TextProColor;
            this.pnl_Palette.Location = paletteLocation;
            this.pnl_Palette.BringToFront();
            this.pnl_Palette.Show();
        }

        private void pnl_Palette_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = cursorColor;
        }

        private void pnl_Palette_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        //用于记录调色板当前要改变背景色的Label
        private Label PaletteTargetLabel = null;

        private void pnl_Palette_MouseDown(object sender, MouseEventArgs e)
        {
            //if (pnl_TextPro.Visible)
            //{
            //    lbl_TextProColor.BackColor = Properties.Resources.colorPalette.GetPixel(e.X, e.Y);
            //}
            //if (pnl_penStyle.Visible)
            //{
            //    lbl_penColor.BackColor = Properties.Resources.colorPalette.GetPixel(e.X, e.Y);
            //}
            if (PaletteTargetLabel != null)
            {
                PaletteTargetLabel.BackColor = Properties.Resources.colorPalette.GetPixel(e.X, e.Y);
                PaletteTargetLabel = null;
            }
            pnl_Palette.Hide();
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        private void pnl_Palette_MouseMove(object sender, MouseEventArgs e)
        {
            lbl_SelectedColor.BackColor = Properties.Resources.colorPalette.GetPixel(e.X, e.Y);
            tb_colorText.Text = "#" + lbl_SelectedColor.BackColor.R.ToString("X2") + lbl_SelectedColor.BackColor.G.ToString("X2") + lbl_SelectedColor.BackColor.B.ToString("X2");
        }

        private void lbl_penColor_MouseDown(object sender, MouseEventArgs e)
        {
            Point paletteLocation = new Point(this.pnl_penStyle.Left + this.lbl_penColor.Left, this.pnl_penStyle.Top + this.lbl_penColor.Bottom);
            if (this.pnl_penStyle.Top + paletteLocation.Y + this.lbl_TextProColor.Height > this.Height)
            {
                paletteLocation = new Point(this.pnl_penStyle.Left + this.lbl_penColor.Left, this.pnl_penStyle.Top + this.lbl_penColor.Top - this.pnl_Palette.Height);
                PointToScreen(paletteLocation);
            }
            PaletteTargetLabel = this.lbl_penColor;
            this.pnl_Palette.Location = paletteLocation;
            this.pnl_Palette.BringToFront();
            this.pnl_Palette.Show();
        }

        private void lbl_penColor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.colorButton, 0, 0);
        }

        private void lbl_Info_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

            if (this.imageEditMode != ImageEditMode.None)
            {
                if (this.lbl_Info.Left == this.lbl_CutImage.Left + 5 && this.lbl_Info.Top == this.lbl_CutImage.Top + 5)
                {
                    this.lbl_Info.Location = new Point(this.lbl_CutImage.Left + 5, this.lbl_CutImage.Bottom - this.lbl_Info.Height - 5);
                }
                else
                {
                    this.lbl_Info.Location = new Point(this.lbl_CutImage.Left + 5, this.lbl_CutImage.Top + 5);
                }
            }
        }

        /// <summary>
        /// 将鼠标的光标图像画到屏幕截图上
        /// </summary>
        /// <param name="g"></param>
        private void DrawCursorImageToScreenImage(ref Graphics g)
        {
            if (!this.IsCutCursor) { return; }

            CURSORINFO vCurosrInfo;
            vCurosrInfo.cbSize = Marshal.SizeOf(typeof(CURSORINFO));
            GetCursorInfo(out vCurosrInfo);
            if ((vCurosrInfo.flags & CURSOR_SHOWING) != CURSOR_SHOWING) return;
            Cursor vCursor = new Cursor(vCurosrInfo.hCursor);
            Rectangle vRectangle = new Rectangle(new Point(vCurosrInfo.ptScreenPos.X - vCursor.HotSpot.X, vCurosrInfo.ptScreenPos.Y - vCursor.HotSpot.Y), vCursor.Size);

            vCursor.Draw(g, vRectangle);
        }

        public static Cursor CreateCursor(Bitmap bmp, int xHotSpot, int yHotSpot)
        {
            IconInfo tmp = new IconInfo();
            GetIconInfo(bmp.GetHicon(), ref tmp);
            tmp.xHotspot = xHotSpot;
            tmp.yHotspot = yHotSpot;
            tmp.fIcon = false;
            return new Cursor(CreateIconIndirect(ref tmp));
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = cursorDefault;
        }

        private void pnl_TextPro_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void pnl_penStyle_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void tsmi_ExitCut_Click(object sender, EventArgs e)
        {
            ExitCutImage(true);
        }

        private void tsmi_HideToolBox_Click(object sender, EventArgs e)
        {
            if (this.ToolBoxVisible)
            {
                this.ToolBoxVisible = false;
                this.lbl_ToolBox.Hide();
                tsmi_HideToolBox.Text = "显示编辑工具栏";
            }
            else
            {
                this.ToolBoxVisible = true;
                this.lbl_ToolBox.Show();
                tsmi_HideToolBox.Text = "隐藏编辑工具栏";
            }
        }

        private void tsmi_Save_Click(object sender, EventArgs e)
        {
            handelEdit_StateChanged();
            ExecCutImage(false, false);
        }

        private void tsmi_Done_Click(object sender, EventArgs e)
        {
            handelEdit_StateChanged();
            ExecCutImage(false, false);
        }

        private void tsmi_Reset_Click(object sender, EventArgs e)
        {
            ExitCutImage(false);
        }

        private void tsmi_Undo_Click(object sender, EventArgs e)
        {
            ExecUndoEdit();
        }

        private void tsmi_Rectangle_Click(object sender, EventArgs e)
        {
            lbl_ToolBox_MouseDown(this.lbl_ToolBox, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 16, 12, 0));
        }

        private void tsmi_Ellipse_Click(object sender, EventArgs e)
        {
            lbl_ToolBox_MouseDown(this.lbl_ToolBox, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 42, 12, 0));
        }

        private void tsmi_Arrowhead_Click(object sender, EventArgs e)
        {
            lbl_ToolBox_MouseDown(this.lbl_ToolBox, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 68, 12, 0));
        }

        private void tsmi_Brush_Click(object sender, EventArgs e)
        {
            lbl_ToolBox_MouseDown(this.lbl_ToolBox, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 94, 12, 0));
        }

        private void tsmi_Text_Click(object sender, EventArgs e)
        {
            lbl_ToolBox_MouseDown(this.lbl_ToolBox, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 120, 12, 0));
        }

        private void tsmi_Filter_Click(object sender, EventArgs e)
        {
            lbl_ToolBox_MouseDown(this.lbl_ToolBox, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 147, 12, 0));
        }

        private void pictureBox_zoom_Paint(object sender, PaintEventArgs e)
        {
            if (e == null || e.ClipRectangle.IsEmpty)
            {
                return;
            }
            if (e.ClipRectangle.X != 0 || e.ClipRectangle.Y != 0 || e.ClipRectangle.Width < this.ZoomBoxWidth - 2 || e.ClipRectangle.Height < this.ZoomBoxHeight - 2)
            {
                return;
            }

            try
            {
                int infoAreaHeight = 32;
                Bitmap bmp_lbl = new Bitmap(e.ClipRectangle.Width, e.ClipRectangle.Height - infoAreaHeight);
                int srcWidth = (int)(this.ZoomBoxWidth / 10);
                int srcHeight = (int)(this.ZoomBoxHeight / 10);

                Bitmap bmp = new Bitmap(srcWidth, srcHeight);
                Rectangle srcRect = new Rectangle(MousePosition.X - 5, MousePosition.Y - 4, srcWidth, srcHeight);
                if (!isCuting)
                {
                    srcRect = new Rectangle(MousePosition.X - 6, MousePosition.Y - 5, srcWidth, srcHeight);
                }
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(screenImage, 0, 0, srcRect, GraphicsUnit.Pixel);
                g.Dispose();

                //Zoom
                int x, y;
                for (int row = 0; row < bmp.Height; row++)
                {
                    for (int col = 0; col < bmp.Width; col++)
                    {
                        Color pc = bmp.GetPixel(col, row);
                        for (int h = 0; h < 10; h++)
                        {
                            for (int w = 0; w < 10; w++)
                            {
                                x = col * 10 + w;
                                y = row * 10 + h;
                                if (x < bmp_lbl.Width && y < bmp_lbl.Height)
                                {
                                    bmp_lbl.SetPixel(x, y, pc);
                                }
                            }
                        }
                    }
                }

                e.Graphics.DrawImage(bmp_lbl, 0, 0);
                bmp_lbl.Dispose();

                int blockX = e.ClipRectangle.Width / 2;
                int blockY = (e.ClipRectangle.Height - infoAreaHeight) / 2;

                SolidBrush brush = new SolidBrush(Color.FromArgb(10, 124, 202));
                Pen pen = new Pen(brush, 2.0F);
                e.Graphics.DrawLine(pen, new Point(0, blockY), new Point(e.ClipRectangle.Width, blockY));
                e.Graphics.DrawLine(pen, new Point(blockX, 0), new Point(blockX, e.ClipRectangle.Height - infoAreaHeight));

                Rectangle rectInfo = new Rectangle(0, e.ClipRectangle.Height - infoAreaHeight, e.ClipRectangle.Width, infoAreaHeight);
                brush = new SolidBrush(Color.FromArgb(51, 51, 51));
                e.Graphics.FillRectangle(brush, rectInfo);

                if (this.lbl_CutImage.Visible)
                {
                    this.areaSize = new System.Drawing.Size(this.lbl_CutImage.Width - 4, this.lbl_CutImage.Height - 4);
                    if (this.areaSize.Width < 1) { this.areaSize.Width = 1; }
                    if (this.areaSize.Height < 1) { this.areaSize.Height = 1; }
                }
                else
                {
                    this.areaSize = rect_WindowFromPoint.Size;
                }

                //绘制截取区域大小
                brush = new SolidBrush(Color.White);
                Font font = new System.Drawing.Font(FontFamily.GenericSerif, 12.0F, GraphicsUnit.Pixel);
                string str = this.areaSize.Width + " x " + this.areaSize.Height;
                e.Graphics.DrawString(str, font, brush, new PointF(5, rectInfo.Top + 1));

                //绘制鼠标指针位置的颜色
                this.cRGB = screenImage.GetPixel(MousePosition.X, MousePosition.Y);
                str = "RGB:#" + this.cRGB.R.ToString("X").PadLeft(2, '0') + this.cRGB.G.ToString("X").PadLeft(2, '0') + this.cRGB.B.ToString("X").PadLeft(2, '0');
                e.Graphics.DrawString(str, font, brush, new PointF(5, rectInfo.Top + 15));

            }
            catch (System.Exception ex)
            {
                this.Hide();
                MessageBox.Show(sender.ToString() + "\n" + e.ClipRectangle.ToString());
                MessageBox.Show(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
            }
            
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                if (this.isCuting)
                {
                    this.isCuting = false;
                    if (this.ToolBoxVisible) { this.lbl_ToolBox.Show(); }
                    this.pictureBox_zoom.Hide();

                    this.lastMouseMoveTime = 0;
                    UpdateCutInfoLabel(UpdateUIMode.None);
                }
            }
        }

        private void tsmi_openSaveDir_Click(object sender, EventArgs e)
        {
            string filePath = this.AutoSaveDirectory;
            string lastWord = string.Empty;
            if (this.AutoSaveDirectory.Length > 0)
            {
                lastWord = this.AutoSaveDirectory.Substring(this.AutoSaveDirectory.Length - 1);
            }
            if (lastWord != "\\")
            {
                filePath += "\\";
            }
            if (this.AutoSaveSubDir)
            {
                filePath += System.DateTime.Now.ToString("yyyy_MM_dd") + "\\";
            }
            if (!System.IO.Directory.Exists(filePath))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(filePath);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            System.Diagnostics.Process.Start("explorer.exe", filePath);
        }

        private void lbl_fillColor_MouseDown(object sender, MouseEventArgs e)
        {
            Point paletteLocation = new Point(this.pnl_penStyle.Left + this.lbl_fillColor.Left, this.pnl_penStyle.Top + this.lbl_fillColor.Bottom);
            if (this.pnl_penStyle.Top + paletteLocation.Y + this.lbl_TextProColor.Height > this.Height)
            {
                paletteLocation = new Point(this.pnl_penStyle.Left + this.lbl_fillColor.Left, this.pnl_penStyle.Top + this.lbl_fillColor.Top - this.pnl_Palette.Height);
                PointToScreen(paletteLocation);
            }
            PaletteTargetLabel = this.lbl_fillColor;
            this.pnl_Palette.Location = paletteLocation;
            this.pnl_Palette.BringToFront();
            this.pnl_Palette.Show();
        }

        private void lbl_fillColor_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(Properties.Resources.colorButton, 0, 0);
        }

        /// <summary>
        /// 用户改变编辑文字事件处理
        /// </summary>
        private void EditTextFontChange()
        {
            ImageEditObject editObject = null;
            for (int i = 0; i < this.imageEditList.Count; i++)
            {
                ImageEditObject tmpObject = (ImageEditObject)this.imageEditList[i];
                if (tmpObject.Selected && tmpObject.EditMode == ImageEditMode.Text)
                {
                    editObject = tmpObject;
                    break;
                }
            }

            if (lbl_CutImage.Controls.Count != 0)
            {
                TextBox tb = (TextBox)this.lbl_CutImage.Controls[0];
                if (tb != null)
                {
                    tb.Font = this.editTextFont.GetFont();
                    tb.ForeColor = this.editTextFont.Color;
                    tb_TextChanged(tb, new EventArgs());
                }
            }

            if (editObject != null)
            {
                editObject.TextFont = this.editTextFont.GetFont();
            }
        }

        private void cbb_FontFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            string fontFamily = "宋体";

            if (cbb_FontFamily.SelectedItem != null)
            {
                fontFamily = cbb_FontFamily.SelectedItem.ToString();
            }

            this.editTextFont.Name = fontFamily;

            EditTextFontChange();
        }

        private void cbb_FontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            float fontSize = 12f;
            if (cbb_FontSize.SelectedItem != null && cbb_FontSize.SelectedItem.ToString().Length != 0)
            {
                float.TryParse(cbb_FontSize.SelectedItem.ToString(), out fontSize);
            }
            this.editTextFont.Size = fontSize;

            EditTextFontChange();
        }

        private void lbl_TextProColor_BackColorChanged(object sender, EventArgs e)
        {
            this.editTextFont.Color = lbl_TextProColor.BackColor;

            EditTextFontChange();
        }

        private void lbl_fontBold_MouseDown(object sender, MouseEventArgs e)
        {
            this.editTextFont.Bold = !this.editTextFont.Bold;
            lbl_fontBold.Refresh();

            EditTextFontChange();
        }

        private void lbl_fontBold_Paint(object sender, PaintEventArgs e)
        {
            if (this.editTextFont.Bold)
            {
                e.Graphics.DrawImage(Properties.Resources.B_Selected, 0, 0);
            }
            else
            {
                e.Graphics.DrawImage(Properties.Resources.B_Nomal, 0, 0);
            }
        }

        private void lbl_fontItalic_MouseDown(object sender, MouseEventArgs e)
        {
            this.editTextFont.Italic = !this.editTextFont.Italic;
            lbl_fontItalic.Refresh();

            EditTextFontChange();
        }

        private void lbl_fontItalic_Paint(object sender, PaintEventArgs e)
        {
            if (this.editTextFont.Italic)
            {
                e.Graphics.DrawImage(Properties.Resources.I_Selected, 0, 0);
            }
            else
            {
                e.Graphics.DrawImage(Properties.Resources.I_Normal, 0, 0);
            }
        }

        private void lbl_fontUnderline_MouseDown(object sender, MouseEventArgs e)
        {
            this.editTextFont.Underline = !this.editTextFont.Underline;
            lbl_fontUnderline.Refresh();

            EditTextFontChange();
        }

        private void lbl_fontUnderline_Paint(object sender, PaintEventArgs e)
        {
            if (this.editTextFont.Underline)
            {
                e.Graphics.DrawImage(Properties.Resources.U_Selected, 0, 0);
            }
            else
            {
                e.Graphics.DrawImage(Properties.Resources.U_Normal, 0, 0);
            }
        }

        private void lbl_colorTransparent_MouseEnter(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
            this.toolTip1.SetToolTip(this.lbl_colorTransparent, "透明色");
            this.lbl_SelectedColor.BackColor = Color.Transparent;
            tb_colorText.Text = "#FFFFFF";
        }

        private void lbl_colorTransparent_Click(object sender, EventArgs e)
        {
            if (PaletteTargetLabel != null)
            {
                PaletteTargetLabel.BackColor = Color.Transparent;
                PaletteTargetLabel = null;
            }
            pnl_Palette.Hide();
            UpdateCutInfoLabel(UpdateUIMode.None);
        }

        private void cbb_fillStyle_MouseDown(object sender, MouseEventArgs e)
        {
            //cbb_fillStyle.ContextMenuStrip = contextMenuFill;
            //cbb_fillStyle.ContextMenuStrip.Show();
        }

        private void timer_ToolBox_Tick(object sender, EventArgs e)
        {
            timer_ToolBox.Stop();
            lbl_ToolBox.ContextMenuStrip = this.contextMenuRectangle;
            lbl_ToolBox.ContextMenuStrip.Show(Control.MousePosition);
        }

        private void lbl_ToolBox_MouseUp(object sender, MouseEventArgs e)
        {
            timer_ToolBox.Stop();
        }

        private void tsmi_RectangleToolBox_Click(object sender, EventArgs e)
        {
            this.imageSubEditMode = ImageSubEditMode.Rectangle;
            lbl_ToolBox.Refresh();
        }

        private void tsmi_CircularToolBox_Click(object sender, EventArgs e)
        {
            this.imageSubEditMode = ImageSubEditMode.CircularRectangle;
            lbl_ToolBox.Refresh();
        }

        private void tsmi_EllipseToolBox_Click(object sender, EventArgs e)
        {
            this.imageSubEditMode = ImageSubEditMode.Ellipse;
            lbl_ToolBox.Refresh();
        }

        private void tsmi_ArrowheadToolBox_Click(object sender, EventArgs e)
        {
            this.imageSubEditMode = ImageSubEditMode.Arrowhead;
            lbl_ToolBox.Refresh();
        }

        private void tsmi_PolygonToolBox_Click(object sender, EventArgs e)
        {
            this.imageSubEditMode = ImageSubEditMode.Polygon;
            lbl_ToolBox.Refresh();
        }

        private void tsmi_L_ShapeToolBox_Click(object sender, EventArgs e)
        {
            this.imageSubEditMode = ImageSubEditMode.L_Shape;
            lbl_ToolBox.Refresh();
        }

        private void lbl_fillColor_BackColorChanged(object sender, EventArgs e)
        {
            if (this.currentToolStyle != null)
            {
                this.currentToolStyle.FillColor = lbl_fillColor.BackColor;
            }
        }

        private void cbb_fillStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.currentToolStyle != null)
            {
                this.currentToolStyle.FillType = cbb_fillStyle.SelectedText;
            }
        }

        private void lbl_penColor_BackColorChanged(object sender, EventArgs e)
        {
            if (this.currentToolStyle != null)
            {
                this.currentToolStyle.BorderColor = lbl_penColor.BackColor;
            }
        }

        private void cbb_penSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.currentToolStyle != null)
            {
                ComboBoxItem cbItem = (ComboBoxItem)cbb_penSize.SelectedItem;
                if (cbItem != null && cbItem.Value != null)
                {
                    int width = 1;
                    int.TryParse(cbItem.Value, out width);
                    this.currentToolStyle.BorderWidth = width;
                }
            }

        }

        private void cbb_penStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.currentToolStyle != null)
            {
                this.currentToolStyle.BorderStyle = cbb_penStyle.SelectedText;
            }
        }

        private void tsmi_recordGif_Click(object sender, EventArgs e)
        {
            isRecordMode = true;
            recordMode = RecordMode.Gif;
            ExecRecordGif();
        }

        private void tsmi_recordFlash_Click(object sender, EventArgs e)
        {
            isRecordMode = true;
            recordMode = RecordMode.Flash;
            ExecRecordFlash();
        }

        private void ExecRecordGif()
        {
            if (!isRecordMode)
            {
                MakeRecordGif();
            }


        }

        private void ExecRecordFlash()
        {
            if (!isRecordMode)
            {
                MakeRecordFlash();
            }


        }

        private void MakeRecordGif()
        {

        }

        private void MakeRecordFlash()
        {

        }

        private void tsmi_Timer_Click(object sender, EventArgs e)
        {
            FrmTimerManager ftm = new FrmTimerManager();
            ftm.ShowDialog();
        }

        /// <summary>
        /// 鼠标单击托盘图标显示右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Program.frmManager == null)
            {
                Program.frmManager = new FrmManager(this.Handle);
            }
            if (!Program.frmManager.Visible)
            {
                Program.frmManager.Show();
            }
            Program.frmManager.Activate();
        }

        /// <summary>
        /// 托盘右键菜单》安全模式切换处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmi_safemode_Click(object sender, EventArgs e)
        {
            Program.safemode = (!Program.safemode);
            if (Program.safemode)
            {
                tsmi_safemode.Text = "退出安全模式";
                tsmi_safemode.ToolTipText = "点击退出安全模式";
            }
            else
            {
                tsmi_safemode.Text = "安全模式";
                tsmi_safemode.ToolTipText = "点击进入安全模式";
            }
        }

        /// <summary>
        /// 左键单击托盘图标，打开管理窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            if (Program.frmManager == null)
            {
                Program.frmManager = new FrmManager(this.Handle);
            }
            if (!Program.frmManager.Visible)
            {
                Program.frmManager.Show();
            }
            Program.frmManager.Activate();
        }

        public void OpenWorkOvertimeForm()
        {
            if (Program.frmWorkOvertime == null)
            {
                Program.frmWorkOvertime = new FrmWorkOvertime();
            }

            Program.frmWorkOvertime.TopMost = true;
            Program.frmWorkOvertime.Show(this);
            Program.frmWorkOvertime.Activate();

        }




        //end////////////////////////////////////////////////////////////////////////
    }
}