using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace 屏幕截图2005
{
    public partial class frmUpload : Form
    {
        /// <summary>
        /// 保存Form1的句柄
        /// </summary>
        private IntPtr frm1Handle = IntPtr.Zero;

        private Bitmap uploadImage = null;

        public frmUpload(IntPtr frm1_Handle, ref Bitmap uploadImg)
        {
            InitializeComponent();
            this.frm1Handle = frm1_Handle;
            this.uploadImage = uploadImg;
        }

        private void frmUpload_Load(object sender, EventArgs e)
        {

        }

        public void AddParameter(string paramName, string paramValue)
        {
            if (this.textBox_parameters.Text.Trim().Length == 0)
            {
                this.textBox_parameters.Text = paramName + "=" + paramValue;
            }
            else
            {
                this.textBox_parameters.Text += "&" + paramName + "=" + paramValue;
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            this.textBox_parameters.Text = string.Empty;
        }

        private void button_add_Click(object sender, EventArgs e)
        {
            frmUpload_AddParam frm = new frmUpload_AddParam(this.Handle);
            frm.ShowDialog();

        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            if (this.textBox_uploadURL.Text.Trim().Length == 0)
            {
                MessageBox.Show("您还没有输入上传地址！");
                return;
            }

            if (!Regex.IsMatch(this.textBox_uploadURL.Text.Trim(), "", RegexOptions.IgnoreCase))
            {
                MessageBox.Show("您输入的上传地址不是有效的URL！");
                return;
            }

            button_Start.Enabled = false;
            button_Cancel.Enabled = false;
            label_status.Text = "正在上传,请稍候……";

            FileUpload fu = new FileUpload(this.textBox_uploadURL.Text.Trim(), uploadImage);
            fu.Start();

            label_status.Text = fu.Status;
            textBox_remoteUrl.Text = fu.RemoteUrl;

            if (fu.LastException != null)
            {
                MessageBox.Show(fu.LastException.Message + "\r\n" + fu.LastException.Source + "\r\n" + fu.LastException.StackTrace);
            }

            button_Start.Enabled = true;
            button_Cancel.Enabled = true;
        }

        //===============================================================================================
    }
}