using DMC.Models;
using System;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 文件重命名
    /// </summary>
    public partial class FrmFileRename : BaseForm
    {
        private GetProjectFileListModel projectFile = null;
        public FrmFileRename(GetProjectFileListModel objInfo)
        {
            InitializeComponent();

            projectFile = objInfo;

            textBox1.Text = projectFile.name;
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

            /*
            string fileNameEx = Path.GetExtension(projectFile.name);

            if (name.EndsWith(fileNameEx))
            {

            }
            else
            {
                name += fileNameEx;
            }
            */

            var projectInfo = new
            {
                id = projectFile.id,
                parentId = projectFile.parentId,
                name = name
            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.UpdateProjectFileName, projectInfo, ref resultData))
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
