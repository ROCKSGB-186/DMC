using DMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 选择项目经理
    /// </summary>
    public partial class FrmSelectProjectManager : BaseForm
    {
        public GetProjectLevelUserModel SelectUser = null;
        public FrmSelectProjectManager()
        {
            InitializeComponent();
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
                var userInfo = (QzUserResultModel)listView3.SelectedItems[0].Tag;
                SelectUser = new GetProjectLevelUserModel();
                SelectUser.userName = userInfo.userName;
                SelectUser.realName = userInfo.realName;
                SelectUser.userId = userInfo.id;

                DialogResult = DialogResult.OK;
            }
            else
            {
                ShowErrorMsg("请选择项目经理！");
            }
        }
        /// <summary>
        /// 选择项目管理员加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmSelectProjectManager_Load(object sender, EventArgs e)
        {
            if (AppGlobalModel.DeptList != null && AppGlobalModel.DeptList.Any())
            {
                foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == "0"))
                {
                    TreeNode root = new TreeNode();
                    //根目录名称
                    root.Text = item.deptName;
                    root.Tag = item;
                    treeView1.Nodes.Add(root);

                    LoadTreeView(root);
                }

                treeView1.ExpandAll();
                treeView1.SelectedNode = treeView1.Nodes[0];
            }
        }
        /// <summary>
        /// 加载组织架构树
        /// </summary>
        /// <param name="treeNode"></param>
        private void LoadTreeView(TreeNode treeNode)
        {
            var parentId = ((DeptInfoResultModel)treeNode.Tag).deptId;
            foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == parentId))
            {
                if (item.deptType == null)
                {
                    continue;
                }

                TreeNode root = new TreeNode();
                //根目录名称
                root.Text = item.deptName;
                root.Tag = item;
                treeNode.Nodes.Add(root);

                LoadTreeView(root);
            }
        }
        /// <summary>
        /// 组织架构树点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            listView3.Items.Clear();
            var deptId = ((DeptInfoResultModel)e.Node.Tag).deptId;
            if (deptId != null)
            {
                var resultData = new List<QzUserResultModel>();
                if (HttpGet(AppGlobalModel.GetDeptUserList + "?deptId=" + deptId, ref resultData))
                {
                    for (int i = 0; i < resultData.Count; i++)
                    {
                        ListViewItem lineLeft = new ListViewItem((i + 1).ToString());
                        lineLeft.SubItems.Add(resultData[i].realName);
                        lineLeft.SubItems.Add(resultData[i].userName);
                        lineLeft.SubItems.Add(resultData[i].deptName);
                        lineLeft.Tag = resultData[i];
                        listView3.Items.Add(lineLeft);
                    }
                }
            }
        }

        /// <summary>
        /// 搜索人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            listView3.Items.Clear();
            var userNick = textBox_username.Text.Trim();

            if (!string.IsNullOrWhiteSpace(userNick))
            {
                var resultData = new List<QzUserResultModel>();
                if (HttpGet(AppGlobalModel.GetDeptUserList + "?realName=" + userNick, ref resultData))
                {
                    for (int i = 0; i < resultData.Count; i++)
                    {
                        ListViewItem lineLeft = new ListViewItem((i + 1).ToString());
                        lineLeft.SubItems.Add(resultData[i].realName);
                        lineLeft.SubItems.Add(resultData[i].userName);
                        lineLeft.SubItems.Add(resultData[i].deptName);
                        lineLeft.Tag = resultData[i];
                        listView3.Items.Add(lineLeft);
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

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
