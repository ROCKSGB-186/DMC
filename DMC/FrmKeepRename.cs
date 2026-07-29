using DMC.Models;
using System;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 归档区重命名
    /// </summary>
    public partial class FrmKeepRename : BaseForm
    {
        private EditKeepProjectNameModel editKeepProjectName = null;
        public FrmKeepRename(EditKeepProjectNameModel obj)
        {
            InitializeComponent();

            editKeepProjectName = obj;
            textBox1.Text = editKeepProjectName.newName;
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

            editKeepProjectName.newName = name;
            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.EditKeepProjectName, editKeepProjectName, ref resultData))
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
