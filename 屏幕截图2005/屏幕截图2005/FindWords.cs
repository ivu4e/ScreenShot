using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    /// <summary>
    /// 用于实现文字提取功能
    /// </summary>
    public class FindWords
    {
        //鼠标事件处理程序
        public static void mouseHook_OnMouseActivity(object sender, MouseEventArgs e)
        {
            if (sender != null)
            {
                DebugTextBox.DebugText(sender.ToString());
                ExceptionLog.Log(sender.ToString());
            }
            if (e != null)
            {
                DebugTextBox.DebugText(e.Button.ToString());
                ExceptionLog.Log(e.Button.ToString());
            }
        }

        public static void keyboardHook_OnKeyUpEvent(object sender, KeyEventArgs e)
        {
            if (sender != null)
            {
                DebugTextBox.DebugText(sender.ToString());
            }
            if (e != null)
            {
                DebugTextBox.DebugText(e.ToString());
            }
        }

        public static void keyboardHook_OnKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (sender != null)
            {
                DebugTextBox.DebugText(sender.ToString());
            }
            if (e != null)
            {
                DebugTextBox.DebugText(e.ToString());
            }
        }
    }
}
