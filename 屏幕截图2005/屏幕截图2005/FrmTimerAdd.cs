using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    public partial class FrmTimerAdd : Form
    {
        public FrmTimerAdd()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            Program.TimerItemLists.Add(new TimerItem(DateTime.Now.AddSeconds(10), "该吃饭了！"));
            this.Close();
        }
    }
}