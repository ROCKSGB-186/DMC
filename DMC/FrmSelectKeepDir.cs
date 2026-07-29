using DMC.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 选择归档目录
    /// </summary>
    public partial class FrmSelectKeepDir : BaseForm
    {
        public string archiveDirId = string.Empty;
        public FrmSelectKeepDir()
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

        private void FrmSelectKeepDir_Load(object sender, EventArgs e)
        {
            //加载组织架构
            var resultData = new List<SelectKeepDeptModel>();
            if (HttpGet(AppGlobalModel.SelectKeepDept + "?parentId=0", ref resultData))
            {
                foreach (var item in resultData)
                {
                    TreeNode root = new TreeNode();
                    //根目录名称
                    root.Text = item.name;
                    root.Tag = item;
                    treeView1.Nodes.Add(root);
                }
            }
            else
            {
                this.Close();
            }
        }

        /// <summary>
        /// 加载归档目录层级
        /// </summary>
        private void LoadKeepDept(string parentId)
        {
            var resultData = new List<SelectKeepDeptModel>();
            if (HttpGet(AppGlobalModel.SelectKeepDept + "?parentId=" + parentId, ref resultData))
            {
                foreach (var item in resultData)
                {
                    TreeNode root = new TreeNode();
                    //根目录名称
                    root.Text = item.name;
                    root.Tag = item;
                    treeView1.SelectedNode.Nodes.Add(root);
                }

                treeView1.SelectedNode.Expand();
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            var selectObject = (SelectKeepDeptModel)e.Node.Tag;

            if (!e.Node.IsExpanded && e.Node.Nodes.Count > 0)
            {
                return;
            }

            e.Node.Nodes.Clear();
            LoadKeepDept(selectObject.id);
        }

        private void btn取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn确定_Click(object sender, EventArgs e)
        {
            var selectObject = treeView1.SelectedNode.Tag;

            if (selectObject == null)
            {
                ShowErrorMsg("请选择归档目录！");
            }
            else
            {
                if (selectObject is SelectKeepDeptModel)
                {
                    var deptInfo = (SelectKeepDeptModel)selectObject;
                    archiveDirId = deptInfo.id;
                }
                else
                {
                    var deptInfo = (GetKeepProjectDirModel)selectObject;
                    archiveDirId = deptInfo.id;
                }

                DialogResult = DialogResult.OK;
            }
        }


        //var selectObject = treeView1.SelectedNode.Tag;
        //if (selectObject == null)
        //{
        //    ShowErrorMsg("请选择归档目录！");
        //}
        //else
        //{
        //    if (selectObject is SelectKeepDeptModel)
        //    {
        //        var deptInfo = (SelectKeepDeptModel)selectObject;
        //        archiveDirId = deptInfo.id;
        //    }
        //    else
        //    {
        //        var deptInfo = (GetKeepProjectDirModel)selectObject;
        //        archiveDirId = deptInfo.id;
        //     }
        //        DialogResult = DialogResult.OK;
        //}

       



        private void btn关闭_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
