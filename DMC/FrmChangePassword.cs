using DMC.Models;
using System;
using System.Diagnostics;

namespace DMC
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public partial class FrmChangePassword : BaseForm
    {
        public FrmChangePassword()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox_old_pass.Text))
            {
                ShowErrorMsg("请填写旧密码！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_new_pass.Text))
            {
                ShowErrorMsg("请填写新密码！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_confirm_pass.Text))
            {
                ShowErrorMsg("请填写确认密码！");
                return;
            }
            if (textBox_new_pass.Text != textBox_confirm_pass.Text)
            {
                ShowErrorMsg("新密码与确认密码不一致！");
                return;
            }

            var para = new
            {
                newPassword = textBox_new_pass.Text,
                password = textBox_old_pass.Text
            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.EditPassword, para, ref resultData))
            {
                ShowSuccessMsg("修改成功，请重新登录！");
                Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
                //关闭所有的线程
                Process.GetCurrentProcess().Kill();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
