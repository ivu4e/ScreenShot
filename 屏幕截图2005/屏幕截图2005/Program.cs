using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

namespace 屏幕截图2005
{
    static class Program
    {
        /// <summary>
        /// Debug模式，显示调试信息，便于解决问题；
        /// </summary>
        public static bool debugMode = false;
        /// <summary>
        /// 安全模式，用于解决截取Windows照片查看器中的图片出现杂点的情况
        /// </summary>
        public static bool safemode = false;

        public static Form1 form1 = null;

        public static FrmManager frmManager = null;

        public static FrmWorkOvertime frmWorkOvertime = null;

        public static List<TimerItem> TimerItemLists = new List<TimerItem>();

        public static Thread timerThread = null;

        public static bool timerThreadRun = true;

        public static bool workOvertimeStarted = false;
        public static bool workOvertimeAsked = false;

        public static void workOvertimeTimerCallback(object obj)
        {
            if (!workOvertimeAsked && !workOvertimeStarted)
            {
                DateTime dt = DateTime.Now;
                if (dt.Hour == 18 && dt.Minute > 0)
                {
                    workOvertimeAsked = true;
                    MethodInvoker mi = new MethodInvoker(form1.OpenWorkOvertimeForm);
                    form1.BeginInvoke(mi);
                }
            }
        }

        public static System.Threading.Timer workOvertimeTimer = null;

        //定时提醒功能
        public static void TimerThreadStart()
        {
            while (timerThreadRun)
            {
                if (TimerItemLists != null && TimerItemLists.Count != 0)
                {
                    DateTime now = DateTime.Now;
                    for (int i = TimerItemLists.Count - 1; i > -1; i-- )
                    {
                        if (TimerItemLists[i].TickTime != null && TimerItemLists[i].TickTime.Value != null)
                        {
                            DateTime item = TimerItemLists[i].TickTime.Value;
                            if (now.Year == item.Year && 
                                now.Month == item.Month &&
                                now.Day == item.Day &&
                                now.Hour == item.Hour && 
                                now.Minute == item.Minute && 
                                now.Second >= item.Second)
                            {
                                FrmTips ftip = new FrmTips();
                                ftip.TopLevel = true;
                                ftip.SetTickTime(item);
                                ftip.SetTips(TimerItemLists[i].Tip);
                                ftip.ShowDialog();
                                TimerItemLists.RemoveAt(i);
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] Args)
        {
            try
            {
                workOvertimeTimer = new System.Threading.Timer(new TimerCallback(workOvertimeTimerCallback), null, 1000, 10000);

                /** 
             * 当前用户是管理员的时候，直接启动应用程序 
             * 如果不是管理员，则使用启动对象启动程序，以确保使用管理员身份运行 
             */
                //获得当前登录的Windows用户标示   
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                //创建Windows用户主题   
                Application.EnableVisualStyles();

                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                //判断当前登录用户是否为管理员   
                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    //如果是管理员，则直接运行   
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    form1 = new Form1();
                    Application.Run(form1);
                }
                else
                {
                    //创建启动对象   
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    //设置运行文件   
                    startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
                    //设置启动参数   
                    startInfo.Arguments = String.Join(" ", Args);
                    //设置启动动作,确保以管理员身份运行   
                    startInfo.Verb = "runas";
                    //如果不是管理员，则启动UAC   
                    System.Diagnostics.Process.Start(startInfo);
                    //退出   
                    System.Windows.Forms.Application.Exit();
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
            }
        }
    }

    public enum TimerItemType : uint
    {
        Tips = 0,
        command = 1
    }

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
    /// 图片编辑类型
    /// </summary>
    public enum ImageEditMode : uint
    {
        None = 0,
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle,
        /// <summary>
        /// 椭圆
        /// </summary>
        Ellipse,
        /// <summary>
        /// 箭头
        /// </summary>
        Arrowhead,
        /// <summary>
        /// 画刷
        /// </summary>
        Brush,
        /// <summary>
        /// 文字
        /// </summary>
        Text,
        /// <summary>
        /// 提示框
        /// </summary>
        ToolTips,
        /// <summary>
        /// 撤消
        /// </summary>
        Undo,
        /// <summary>
        /// 保存
        /// </summary>
        Save,
        /// <summary>
        /// 上传
        /// </summary>
        Upload,
        /// <summary>
        /// 取消
        /// </summary>
        Cancel,
        /// <summary>
        /// 确定
        /// </summary>
        Ok
    }

    /// <summary>
    /// 图片编辑类型》子类型
    /// </summary>
    public enum ImageSubEditMode : uint
    {
        /// <summary>
        /// 矩形
        /// </summary>
        Rectangle = 0,
        /// <summary>
        /// 圆角矩形
        /// </summary>
        CircularRectangle,
        /// <summary>
        /// 椭圆
        /// </summary>
        Ellipse,
        /// <summary>
        /// 箭头
        /// </summary>
        Arrowhead,
        /// <summary>
        /// 多边形
        /// </summary>
        Polygon,
        /// <summary>
        /// L 形
        /// </summary>
        L_Shape

    }

    /// <summary>
    /// 更新UI的模式，用于标记哪些需要显示，哪些需要隐藏；
    /// </summary>
    [FlagsAttribute]
    public enum UpdateUIMode : uint
    {
        //值得注意的是，如果要使用组合值，那么就不能用连接的数字表示，必须是倍数形式！
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
    /// 录制模式，0=录制Gif动画，1=录制Flash动画，录制MP4视频
    /// </summary>
    [FlagsAttribute]
    public enum RecordMode : uint
    {
        Gif = 0,
        Flash = 1,
        MP4 = 2
    }

    /// <summary>
    /// 矢量绘图工具类型
    /// </summary>
    [FlagsAttribute]
    public enum VectorDrawToolsType : uint
    {
        /// <summary>
        /// 直线
        /// </summary>
        Line = 0,
        /// <summary>
        /// 形状
        /// </summary>
        Shape = 1
    }

}