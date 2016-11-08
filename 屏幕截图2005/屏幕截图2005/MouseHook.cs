using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    /// <summary>
    /// 这个类可以让你得到一个在运行中程序的所有鼠标事件 
    /// 并且引发一个带MouseEventArgs参数的.NET鼠标事件以便你很容易使用这些信息 
    /// </summary>
    public class MouseHook
    {
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;
        //全局的事件 
        public event MouseEventHandler OnMouseActivity;
        static int hMouseHook = 0;   //鼠标钩子句柄 
        //鼠标常量 
        public const int WH_MOUSE_LL = 14;   //mouse   hook   constant 
        HookProc MouseHookProcedure;   //声明鼠标钩子事件类型. 
        //声明一个Point的封送类型 
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        //声明鼠标钩子的封送结构类型 
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hWnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        //装置钩子的函数 
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        //卸下钩子的函数 
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //下一个钩挂的函数 
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        static public extern int FormatMessage(
        uint dwFlags,
        IntPtr lpSource,
        int dwMessageId,
        int dwLanguageZId,
        string lpBuffer,
        int nSize,
        IntPtr Arguments);

        const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        ///   <summary> 
        ///   墨认的构造函数构造当前类的实例. 
        ///   </summary> 
        public MouseHook()
        {
            //Start(); 
        }
        //析构函数. 
        ~MouseHook()
        {
            Stop();
        }
        public void Start()
        {
            //安装鼠标钩子 
            if (hMouseHook == 0)
            {
                //生成一个HookProc的实例. 
                MouseHookProcedure = new HookProc(MouseHookProc);
                hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                //如果装置失败停止钩子 
                if (hMouseHook == 0)
                {
                    int errNum = Marshal.GetLastWin32Error();
                    //函数内调用
                    int sLen = 0x100;
                    uint dwFlags = FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;

                    string lpBuffer = new string(' ', sLen);

                    int count = FormatMessage(dwFlags, IntPtr.Zero, errNum, 0, lpBuffer, sLen, IntPtr.Zero);
                    Stop();
                    throw new Exception("SetWindowsHookEx failed." + lpBuffer);
                }
            }
        }
        public void Stop()
        {
            bool retMouse = true;
            if (hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }

            //如果卸下钩子失败 
            if (!(retMouse)) throw new Exception("UnhookWindowsHookEx   failed. ");
        }
        private int MouseHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            //如果正常运行并且用户要监听鼠标的消息 
            if ((nCode >= 0) && (OnMouseActivity != null))
            {
                MouseButtons button = MouseButtons.None;
                int clickCount = 0;
                switch (wParam)
                {
                    case WM_LBUTTONDOWN:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONUP:
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case WM_RBUTTONDOWN:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONUP:
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                }
                //从回调函数中得到鼠标的信息 
                MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));
                MouseEventArgs e = new MouseEventArgs(button, clickCount, MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y, 0);
                OnMouseActivity(this, e);
            }
            return CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }
    }
}
