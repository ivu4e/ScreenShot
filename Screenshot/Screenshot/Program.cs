using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Screenshot
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
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
}
