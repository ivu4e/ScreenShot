using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    public partial class FrmTimerManager : Form
    {
        public FrmTimerManager()
        {
            InitializeComponent();
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            FrmTimerAdd ftadd = new FrmTimerAdd();
            ftadd.StartPosition = FormStartPosition.CenterScreen;
            ftadd.ShowDialog();
        }
    }
}