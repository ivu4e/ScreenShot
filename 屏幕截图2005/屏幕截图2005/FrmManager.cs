using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace 屏幕截图2005
{
    public partial class FrmManager : Form
    {
        private Form1 mainForm = null;
        public FrmManager(IntPtr frmPtr)
        {
            InitializeComponent();
            this.mainForm = (Form1)Form1.FromHandle(frmPtr);
        }

        private void FrmManager_Load(object sender, EventArgs e)
        {
            if (this.mainForm == null)
            {
                return;
            }
            if (!Directory.Exists(this.mainForm.AutoSaveDirectory))
            {
                return;
            }
            listBox1.Items.Clear();
            String[] paths = Directory.GetDirectories(this.mainForm.AutoSaveDirectory, "*", SearchOption.AllDirectories);

            for (int i = 0; i < paths.Length; i++ )
            {
                int idx = paths[i].LastIndexOf('\\');
                String path = "";
                if (idx != -1)
                {
                    path = paths[i].Substring(idx + 1);
                    ComboBoxItem item = new ComboBoxItem(path, paths[i]);
                    listBox1.Items.Add(item);
                }
                
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            ComboBoxItem item = null;
            if (listBox1.SelectedItem != null)
            {
                item = (ComboBoxItem)listBox1.SelectedItem;
            }
            if (item == null)
            {
                return;
            }
            if (!Directory.Exists(item.Value))
            {
                return;
            }

            String[] paths = Directory.GetDirectories(item.Value);
            for (int i = 0; i < paths.Length; i++)
            {
                int idx = paths[i].LastIndexOf('\\');
                String path = "";
                if (idx != -1)
                {
                    path = paths[i].Substring(idx + 1);
                    ComboBoxItem itm = new ComboBoxItem(path, paths[i]);
                    listBox2.Items.Add(itm);
                }

            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
