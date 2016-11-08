using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    public partial class FrmTips : Form
    {
        public FrmTips()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_wait_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmTips_Load(object sender, EventArgs e)
        {

        }

        public void SetTickTime(Nullable<DateTime> tickTime)
        {
            if (tickTime != null)
            {
                this.label1.Text = tickTime.Value.ToString();
            }
        }

        public void SetTips(string tip)
        {
            this.label2.Text = tip;
        }
        
    }
}