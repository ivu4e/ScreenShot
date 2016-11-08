using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace 屏幕截图2005
{
    /// <summary>
    /// Debug信息输出框的相关事件处理程序
    /// </summary>
    public static class DebugTextBox
    {
        [DllImport("user32", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);

        [DllImport("user32", EntryPoint = "ShowCaret")]
        private static extern bool ShowCaret(IntPtr hWnd);

        //用于保存Debug信息输出框的引用
        public static TextBox debugTextBox = null;

        /// <summary>
        /// 如果用户同时按下Ctrl+鼠标左键则隐藏点击的对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void onMouseDown(object sender, MouseEventArgs e)
        {
            //用户按下的是否为鼠标左键
            if (e.Button == MouseButtons.Left)
            {
                //用户是否按下了键盘的Ctrl键
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
                {
                    TextBox tb = (TextBox)sender;
                    if (tb != null)
                    {
                        tb.Hide();
                    }
                }
            }
        }

        /// <summary>
        /// 如果用户按下Esc键,则隐藏DebugTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void onKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                TextBox tb = (TextBox)sender;
                if (tb != null)
                {
                    tb.Hide();
                }
            }
        }

        /// <summary>
        /// 清除Debug信息
        /// </summary>
        public static void clearDebugInfo()
        {
            if (debugTextBox != null)
            {
                debugTextBox.Clear();
            }
        }

        public static void DebugText(String msg)
        {
            if (debugTextBox != null)
            {
                debugTextBox.Text += "\r\n" + msg;
                debugTextBox.Select();
            }
        }

        public static void onGotFocus(Object sender, System.EventArgs e)
        {
            if (ShowCaret(((TextBox)sender).Handle)){
                HideCaret(((TextBox)sender).Handle);
            }
        }
    }
}
