using DMC.Models;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 新增子项或文件夹
    /// </summary>
    public partial class FrmAddChildOrFolder : BaseForm
    {
        private string parentId = null;
        private int type = 0;

        public FrmAddChildOrFolder(string objStr, int objInt)
        {
            InitializeComponent();

            parentId = objStr;
            type = objInt;
        }
        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;

        private void WindowMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                #region 通过Left、Top计算直接+=变化即可
                Left += e.Location.X - originLocation.X;
                Top += e.Location.Y - originLocation.Y;
                #endregion
            }
        }

        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion

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

            //type 10 代表归档
            if (type == 10)
            {
                var projectInfo = new
                {
                    parentId = parentId,
                    name = name
                };

                var resultData = string.Empty;

                if (HttpPost(AppGlobalModel.AddKeepDept, projectInfo, ref resultData))
                {
                    DialogResult = DialogResult.OK;
                }
            }
            else
            {
                var projectInfo = new
                {
                    parentId = parentId,
                    name = name
                };

                var resultData = string.Empty;
                var postData = $"projectInfo={JsonConvert.SerializeObject(projectInfo)}";
                string url = type == 2 ? AppGlobalModel.AddProjectSubitem : AppGlobalModel.AddProjectDir;
                if (HttpPost(url, postData, ref resultData))
                {
                    DialogResult = DialogResult.OK;
                }
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
