using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Screenshot
{
    public partial class frmSetup : Form
    {
        /// <summary>
        /// 保存Form1的句柄
        /// </summary>
        private IntPtr frm1Handle = IntPtr.Zero;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="frm1_Handle"></param>
        public frmSetup(IntPtr frm1_Handle)
        {
            InitializeComponent();
            this.frm1Handle = frm1_Handle;
        }

        /// <summary>
        /// 确定按钮单击事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_ok_Click(object sender, EventArgs e)
        {
            if (checkBox_autoSave.Checked && textBox_saveDir.Text.Trim().Length == 0)
            {
                MessageBox.Show("您选择了“自动保存屏幕截图到磁盘”\n但还没有设置存储目录！");
                return;
            }
            if (checkBox_autoSave.Checked && textBox_saveDir.Text.Trim().Length > 0)
            {
                if (!System.Text.RegularExpressions.Regex.IsMatch(textBox_saveDir.Text.Trim(), "^[a-zA-Z]:\\\\[^/:\\*\\?\"<>\\|]*$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    MessageBox.Show("您选择了“自动保存屏幕截图到磁盘”\n但设置的存储目录不是有效的目录！");
                    return;
                }
                if (!System.IO.Directory.Exists(textBox_saveDir.Text.Trim()))
                {
                    MessageBox.Show("您选择了“自动保存屏幕截图到磁盘”\n但设置的存储目录不存在！");
                    return;
                }
            }
            Form1 frm = (Form1)Form.FromHandle(frm1Handle);
            if (frm != null)
            {
                //基本设置
                if (radioButton1.Checked) // && frm.HotKeyMode != 0 无论是否改变都重新注册热键，解决有时热键失效的问题
                {
                    Form1.UnregisterHotKey(frm1Handle, frm.hotKeyId);
                    Form1.RegisterHotKey(frm1Handle, frm.hotKeyId, (uint)KeyModifiers.Control | (uint)KeyModifiers.Alt, Keys.A);
                    frm.HotKeyMode = 0;
                }

                if (radioButton2.Checked) // && frm.HotKeyMode != 1 无论是否改变都重新注册热键，解决有时热键失效的问题
                {
                    Form1.UnregisterHotKey(frm1Handle, frm.hotKeyId);
                    Form1.RegisterHotKey(frm1Handle, frm.hotKeyId, (uint)KeyModifiers.Control | (uint)KeyModifiers.Shift, Keys.A);
                    frm.HotKeyMode = 1;
                }

                frm.InfoBoxVisible = ckb_InfoBox.Checked;
                frm.ToolBoxVisible = ckb_ToolBox.Checked;
                frm.IsCutCursor = ckb_CutCursor.Checked;
                frm.ZoomBoxVisible = ckb_ZoomBox.Checked;


                frm.ZoomBoxWidth1 = Convert.ToInt32(tb_zoomBoxWidth.Text);
                frm.ZoomBoxHeight1 = Convert.ToInt32(tb_zoomBoxHeight.Text);

                if (frm.ZoomBoxWidth1 < 120)
                {
                    frm.ZoomBoxWidth1 = 120;
                    tb_zoomBoxWidth.Text = frm.ZoomBoxWidth1.ToString();
                }
                if (frm.ZoomBoxHeight1 < 100)
                {
                    frm.ZoomBoxHeight1 = 100;
                    tb_zoomBoxHeight.Text = frm.ZoomBoxHeight1.ToString();
                }

                //图片上传
                frm.PicDescFieldName = textBox_fieldDesc.Text;
                frm.ImageFieldName = textBox_fieldFile.Text;
                frm.PicDesc = textBox_desc.Text;
                frm.UploadUrl = textBox_uploadUrl.Text;
                frm.DoUpload = checkBox_upload.Checked;

                //自动保存
                frm.AutoSaveToDisk = checkBox_autoSave.Checked;
                frm.AutoSaveSubDir = chb_subDir.Checked;
                frm.AutoSaveDirectory = textBox_saveDir.Text;

                frm.AutoSaveFileName1 = textBox_fileName1.Text;
                if (comboBox_fileName2.SelectedItem != null)
                {
                    frm.AutoSaveFileName2 = comboBox_fileName2.Text;
                }
                else
                {
                    frm.AutoSaveFileName2 = "日期时间";
                }
                if (comboBox_Extn.SelectedItem != null)
                {
                    frm.AutoSaveFileName3 = comboBox_Extn.Text;
                }
                else
                {
                    frm.AutoSaveFileName3 = ".png";
                }
            }

            SaveConfiguration();

            this.Close();
        }

        /// <summary>
        /// 保存配置信息到配置文件
        /// </summary>
        private void SaveConfiguration()
        {
            System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(null);

            //基本设置
            SetConfigAppSetting(ref config, AppSettingKeys.HotKeyMode, radioButton1.Checked ? "1" : "0");
            SetConfigAppSetting(ref config, AppSettingKeys.InfoBoxVisible, ckb_InfoBox.Checked ? "1" : "0");
            SetConfigAppSetting(ref config, AppSettingKeys.ToolBoxVisible, ckb_ToolBox.Checked ? "1" : "0");
            SetConfigAppSetting(ref config, AppSettingKeys.IsCutCursor, ckb_CutCursor.Checked ? "1" : "0");
            SetConfigAppSetting(ref config, AppSettingKeys.ZoomBoxVisible, ckb_ZoomBox.Checked ? "1" : "0");
            SetConfigAppSetting(ref config, AppSettingKeys.ZoomBoxWidth, tb_zoomBoxWidth.Text);
            SetConfigAppSetting(ref config, AppSettingKeys.ZoomBoxHeight, tb_zoomBoxHeight.Text);

            //图片上传
            SetConfigAppSetting(ref config, AppSettingKeys.PicDescFieldName, textBox_fieldDesc.Text.Trim());
            SetConfigAppSetting(ref config, AppSettingKeys.ImageFieldName, textBox_fieldFile.Text.Trim());
            SetConfigAppSetting(ref config, AppSettingKeys.PicDesc, textBox_desc.Text.Trim());
            SetConfigAppSetting(ref config, AppSettingKeys.UploadUrl, textBox_uploadUrl.Text.Trim());
            SetConfigAppSetting(ref config, AppSettingKeys.DoUpload, checkBox_upload.Checked ? "1" : "0");

            //自动保存
            SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveToDisk, checkBox_autoSave.Checked ? "1" : "0");
            SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveSubDir, chb_subDir.Checked ? "1" : "0");
            SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveDirectory, textBox_saveDir.Text.Trim());
            SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveFileName1, textBox_fileName1.Text.Trim());
            if (comboBox_fileName2.SelectedItem != null)
            {
                SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveFileName2, comboBox_fileName2.Text);
            }
            else
            {
                SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveFileName2, "日期时间");
            }
            if (comboBox_Extn.SelectedItem != null)
            {
                SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveFileName3, comboBox_Extn.Text);
            }
            else
            {
                SetConfigAppSetting(ref config, AppSettingKeys.AutoSaveFileName3, ".png");
            }

            config.Save(System.Configuration.ConfigurationSaveMode.Modified);
        }

        /// <summary>
        /// 设置配置信息
        /// </summary>
        /// <param name="config"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool SetConfigAppSetting(ref System.Configuration.Configuration config, string key, string value)
        {
            try
            {
                if (config.AppSettings.Settings[key] != null)
                {
                    config.AppSettings.Settings[key].Value = value;
                }
                else
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Source + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="config"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetConfigAppSetting(ref System.Configuration.Configuration config, string key)
        {
            try
            {
                if (config.AppSettings.Settings[key] != null)
                {
                    return config.AppSettings.Settings[key].Value;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.Source + ex.StackTrace);
            }
            return string.Empty;
        }

        /// <summary>
        /// 取消按钮单击事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 窗口加载事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSetup_Load(object sender, EventArgs e)
        {
            chb_subDir.Text = "启用（按日期命名，格式：" + DateTime.Now.Date.ToString("yyyy_MM_dd") + "）";

            Form1 frm = (Form1)Form.FromHandle(frm1Handle);
            if (frm != null)
            {
                //基本设置
                if (frm.HotKeyMode == 0)
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }

                ckb_InfoBox.Checked = frm.InfoBoxVisible;
                ckb_ToolBox.Checked = frm.ToolBoxVisible;
                ckb_CutCursor.Checked = frm.IsCutCursor;
                ckb_ZoomBox.Checked = frm.ZoomBoxVisible;

                //图片上传
                textBox_fieldDesc.Text = frm.PicDescFieldName;
                textBox_fieldFile.Text = frm.ImageFieldName;
                textBox_desc.Text = frm.PicDesc;
                textBox_uploadUrl.Text = frm.UploadUrl;
                checkBox_upload.Checked = frm.DoUpload;

                //自动保存
                checkBox_autoSave.Checked = frm.AutoSaveToDisk;
                chb_subDir.Checked = frm.AutoSaveSubDir;
                textBox_saveDir.Text = frm.AutoSaveDirectory;
                textBox_fileName1.Text = frm.AutoSaveFileName1;
                comboBox_fileName2.SelectedItem = frm.AutoSaveFileName2;
                comboBox_Extn.SelectedItem = frm.AutoSaveFileName3;


            }
        }
        /// <summary>
        /// 浏览按钮事件处理程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_browse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请选择屏幕截图的保存目录：";
            fbd.ShowNewFolderButton = true;
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            fbd.SelectedPath = textBox_saveDir.Text;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox_saveDir.Text = fbd.SelectedPath;
            }
        }
        /// <summary>
        /// 更新自动保存文件名称示例
        /// </summary>
        private void UpdateFileNameExmple()
        {
            string AutoSaveFileName2 = string.Empty;
            if (comboBox_fileName2.SelectedItem != null)
            {
                AutoSaveFileName2 = comboBox_fileName2.Text;
            }
            string AutoSaveFileName3 = ".png";
            if (comboBox_Extn.SelectedItem != null)
            {
                AutoSaveFileName3 = comboBox_Extn.Text;
            }

            switch (AutoSaveFileName2)
            {
                case "日期_序号":
                    textBox_exmple.Text = textBox_fileName1.Text + DateTime.Now.ToString("yyyy-MM-dd_") + "0001" + AutoSaveFileName3;
                    break;
                case "序号":
                    textBox_exmple.Text = textBox_fileName1.Text + "0001" + AutoSaveFileName3;
                    break;
                default:
                    textBox_exmple.Text = textBox_fileName1.Text + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + AutoSaveFileName3;
                    break;
            }
        }

        private void comboBox_fileName2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFileNameExmple();
        }

        private void comboBox_Extn_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFileNameExmple();
        }

        private void textBox_fileName1_TextChanged(object sender, EventArgs e)
        {
            UpdateFileNameExmple();
        }

        // Boolean flag used to determine when a character other than a number is entered.
        private bool nonNumberEntered = false;

        private void tb_zoomBoxWidth_KeyDown(object sender, KeyEventArgs e)
        {
            // Initialize the flag to false.
            nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = true;
                    }
                }
            }
            //If shift key was pressed, it's not a number.
            if (Control.ModifierKeys == Keys.Shift)
            {
                nonNumberEntered = true;
            }
        }

        private void tb_zoomBoxHeight_KeyDown(object sender, KeyEventArgs e)
        {
            // Initialize the flag to false.
            nonNumberEntered = false;

            // Determine whether the keystroke is a number from the top of the keyboard.
            if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
            {
                // Determine whether the keystroke is a number from the keypad.
                if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
                {
                    // Determine whether the keystroke is a backspace.
                    if (e.KeyCode != Keys.Back)
                    {
                        // A non-numerical keystroke was pressed.
                        // Set the flag to true and evaluate in KeyPress event.
                        nonNumberEntered = true;
                    }
                }
            }
            //If shift key was pressed, it's not a number.
            if (Control.ModifierKeys == Keys.Shift)
            {
                nonNumberEntered = true;
            }
        }

        private void tb_zoomBoxWidth_KeyPress(object sender, KeyPressEventArgs e)
        {

            // Check for the flag being set in the KeyDown event.
            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }


        private void tb_zoomBoxHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check for the flag being set in the KeyDown event.
            if (nonNumberEntered == true)
            {
                // Stop the character from being entered into the control since it is non-numerical.
                e.Handled = true;
            }
        }

        /// <summary>
        /// 放大镜宽度改变事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_zoomBoxWidth_TextChanged(object sender, EventArgs e)
        {
            int zoomWidth = Convert.ToInt32(tb_zoomBoxWidth.Text);
            if (zoomWidth < 120) { zoomWidth = 120; }

            tb_zoomBoxHeight.Text = ((int)(zoomWidth * 100 / 120)).ToString();
        }

        /// <summary>
        /// 放大镜高度改变事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tb_zoomBoxHeight_TextChanged(object sender, EventArgs e)
        {
            int zoomHeight = Convert.ToInt32(tb_zoomBoxHeight.Text);
            if (zoomHeight < 100) { zoomHeight = 100; }

            tb_zoomBoxWidth.Text = ((int)(zoomHeight * 120 / 100)).ToString();
        }
    }
}
