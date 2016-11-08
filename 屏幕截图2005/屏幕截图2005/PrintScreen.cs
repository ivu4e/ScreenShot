using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace 屏幕截图2005
{
    /// <summary>
    /// 模拟Print Screen键盘消息，截取全屏图片
    /// </summary>
    public class PrintScreen
    {
        [DllImport("user32.dll")]
        static extern void keybd_event
        (
            byte bVk,// 虚拟键值  
            byte bScan,// 硬件扫描码  
            uint dwFlags,// 动作标识  
            IntPtr dwExtraInfo// 与键盘动作关联的辅加信息  
        );

        /// <summary>
        /// 模拟Print Screen键盘消息，截取全屏图片。
        /// </summary>
        public static void DoPrintScreen()
        {
            keybd_event((byte)0x2c, 0, 0x0, IntPtr.Zero);//down
            Application.DoEvents();
            keybd_event((byte)0x2c, 0, 0x2, IntPtr.Zero);//up
            Application.DoEvents();
        }

        /// <summary>
        /// 模拟Alt Print Screen键盘消息，截取当前窗口图片。
        /// </summary>
        public static void AltPrintScreen()
        {
            keybd_event((byte)Keys.Menu, 0, 0x0, IntPtr.Zero);
            keybd_event((byte)0x2c, 0, 0x0, IntPtr.Zero);//down
            Application.DoEvents();
            Application.DoEvents();
            keybd_event((byte)0x2c, 0, 0x2, IntPtr.Zero);//up
            keybd_event((byte)Keys.Menu, 0, 0x2, IntPtr.Zero);
            Application.DoEvents();
            Application.DoEvents();
        }

        /// <summary>
        /// 从剪贴板获取图片
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetScreenImage()
        {
            IDataObject newobject = null;
            Bitmap NewBitmap = null;

            try
            {
                Application.DoEvents();
                newobject = Clipboard.GetDataObject();

                if (Clipboard.ContainsImage())
                {
                    NewBitmap = (Bitmap)(Clipboard.GetImage().Clone());
                }

                return NewBitmap;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 模拟PrintScreen按键，截取屏幕图片
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetFullScreen()
        {
            DoPrintScreen();
            return GetScreenImage();
        }

        /// <summary>
        /// 模拟Alt Print Screen键盘消息，截取当前窗口图片
        /// </summary>
        /// <returns></returns>
        public static Bitmap GetActiveWindow()
        {
            DoPrintScreen();
            return GetScreenImage();
        }
    }
}
