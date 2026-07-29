using DMC.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 选择项目
    /// </summary>
    public partial class FrmSelectProject : BaseForm
    {
        public ProjectResultModel selectInfo = null;
        private string projectOrArchive = null;
        public FrmSelectProject(string projectOrArchive)
        {
            InitializeComponent();
            this.projectOrArchive = projectOrArchive;
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
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (listView3.SelectedItems.Count > 0)
            {
                selectInfo = (ProjectResultModel)listView3.SelectedItems[0].Tag;
                DialogResult = DialogResult.OK;
            }
            else
            {
                ShowErrorMsg("请选择项目！");
            }
        }

        /// <summary>
        /// 搜索项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
            var userNick = textBox_name.Text.Trim();

            if (!string.IsNullOrWhiteSpace(userNick))
            {

                var resultData = new List<ProjectResultModel>();
                if (projectOrArchive == "archive")
                {
                    if (HttpGet(AppGlobalModel.SelectKeepProject + "?name=" + userNick, ref resultData))
                    {
                        for (int i = 0; i < resultData.Count; i++)
                        {
                            ListViewItem lineLeft = new ListViewItem((i + 1).ToString());
                            lineLeft.SubItems.Add(resultData[i].name);
                            lineLeft.SubItems.Add(resultData[i].identifier);
                            lineLeft.SubItems.Add(resultData[i].unit);
                            lineLeft.Tag = resultData[i];
                            listView3.Items.Add(lineLeft);
                        }
                    }

                }
                else
                {
                    if (HttpGet(AppGlobalModel.SelectKeepProject + "?name=" + userNick, ref resultData))
                    {
                        for (int i = 0; i < resultData.Count; i++)
                        {
                            ListViewItem lineLeft = new ListViewItem((i + 1).ToString());
                            lineLeft.SubItems.Add(resultData[i].name);
                            lineLeft.SubItems.Add(resultData[i].identifier);
                            lineLeft.SubItems.Add(resultData[i].unit);
                            lineLeft.Tag = resultData[i];
                            listView3.Items.Add(lineLeft);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// 回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox_username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_Click(sender, e);
            }
        }
    }
}
