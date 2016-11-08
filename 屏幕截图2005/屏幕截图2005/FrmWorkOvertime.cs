using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    public partial class FrmWorkOvertime : Form
    {
        public FrmWorkOvertime()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.workOvertimeStarted = true;
            WorkOvertime.OverStart();
            this.Close();
        }
    }
}
