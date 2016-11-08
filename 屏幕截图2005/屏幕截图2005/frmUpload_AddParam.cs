using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    public partial class frmUpload_AddParam : Form
    {
        private IntPtr frmUploadPtr = IntPtr.Zero;

        public frmUpload_AddParam(IntPtr frmUpload_Handle)
        {
            InitializeComponent();
            this.frmUploadPtr = frmUpload_Handle;
        }

        private void frmUpload_AddParam_Load(object sender, EventArgs e)
        {

        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (textBox_name.Text.Trim().Length == 0)
            {
                MessageBox.Show("您还没有输入参数名称！");
                return;
            }
            if (textBox_value.Text.Trim().Length == 0)
            {
                MessageBox.Show("您还没有输入参数值！");
                return;
            }

            frmUpload frm = (frmUpload)Form.FromHandle(this.frmUploadPtr);
            if (frm != null)
            {
                frm.AddParameter(textBox_name.Text.Trim(), textBox_value.Text.Trim());
            }
            else
            {
                MessageBox.Show("frmUpload窗口不可用！");
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            if (textBox_name.Text.Trim().Length > 0 && textBox_value.Text.Trim().Length > 0)
            {
                frmUpload frm = (frmUpload)Form.FromHandle(this.frmUploadPtr);
                if (frm != null)
                {
                    frm.AddParameter(textBox_name.Text.Trim(), textBox_value.Text.Trim());
                }
            }
            this.Close();
        }
    }
}