using DMC.Models;
using System;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 修改子项或文件夹
    /// </summary>
    public partial class FrmEditChildOrFolder : BaseForm
    {
        private string projectLevelId = null;
        private string name = null;

        public FrmEditChildOrFolder(string objStr, string objStrName)
        {
            InitializeComponent();

            projectLevelId = objStr;

            textBox1.Text = name = objStrName;
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            var name = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                ShowErrorMsg("请输入名称！");
                return;
            }

            var para = new
            {
                projectLevelId = projectLevelId,
                name = name
            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.UpdateProjectLevelName, para, ref resultData))
            {
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
