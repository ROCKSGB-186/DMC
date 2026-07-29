using DMC.Helper;
using DMC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 项目管理
    /// </summary>
    public partial class FrmProject : BaseForm
    {
        /// <summary>
        /// 所有项目列表
        /// </summary>
        private List<ProjectResultModel> projectAllList = null;
        /// <summary>
        /// 创建的项目
        /// </summary>
        private List<ProjectResultModel> projectCreateList = null;
        /// <summary>
        /// 未发布的项目
        /// </summary>
        private List<ProjectResultModel> projectUnreleasedList = null;
        /// <summary>
        /// 
        /// </summary>
        private List<string> authList = new List<string>();
        /// <summary>
        /// 项目管理窗口
        /// </summary>
        public FrmProject()
        {
            InitializeComponent();
            dataGridView_项目列表.AutoGenerateColumns = false;
            dataGridView_我创建的项目.AutoGenerateColumns = false;
            dataGridView_未发布项目.AutoGenerateColumns = false;
            // 项目编号列绑定到 ProjectResultModel.identifier
            ProjectNo.DataPropertyName = nameof(ProjectResultModel.identifier);
            MyProjectNo.DataPropertyName = nameof(ProjectResultModel.identifier);
            No_ProjectNo.DataPropertyName = nameof(ProjectResultModel.identifier);
        }
        /// <summary>
        /// 加载组织架构
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProject_Load(object sender, EventArgs e)
        {
            //加载组织架构
            if (AppGlobalModel.DeptList != null && AppGlobalModel.DeptList.Any())
            {
                foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == "0"))
                {
                    TreeNode root = new TreeNode();
                    ///根目录名称
                    root.Text = item.deptName;
                    root.Tag = item;
                    treeView_组织架构.Nodes.Add(root);

                    LoadTreeView(root);
                }

                treeView_组织架构.ExpandAll();
                treeView_组织架构.SelectedNode = treeView_组织架构.Nodes[0];
            }
        }

        /// <summary>
        /// 加载组织架构
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
                if (item.deptType == "3")
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
        /// 加载项目列表
        /// </summary>
        private void LoadProjectList()
        {
            button_修改.Enabled = false;
            button_删除.Enabled = false;
            button_停用.Enabled = false;

            if (treeView_组织架构.SelectedNode.Tag != null)
            {
                var deptInfo = (DeptInfoResultModel)treeView_组织架构.SelectedNode.Tag;
                if (deptInfo.deptType == "2")
                {
                    #region 项目列表
                    //项目列表,deptId：部门Id
                    if (tabControl_项目.SelectedIndex == 0)
                    {
                        if (authList.Exists(o => o == "promanage:list"))
                        {
                            if (HttpGet(AppGlobalModel.GetProjectList + $"?deptId={deptInfo.deptId}&table=1", ref projectAllList))
                            {
                                dataGridView_项目列表.DataSource = new SortableBindingList<ProjectResultModel>(projectAllList);
                                dataGridView_项目列表.ClearSelection();
                            }
                        }
                    }
                    //我创建的项目
                    else if (tabControl_项目.SelectedIndex == 1)
                    {
                        if (HttpGet(AppGlobalModel.GetProjectMyCreateList + $"?deptId={deptInfo.deptId}", ref projectCreateList))
                        {
                            dataGridView_我创建的项目.DataSource = new SortableBindingList<ProjectResultModel>(projectCreateList);
                            dataGridView_我创建的项目.ClearSelection();
                        }
                    }
                    //未发布的项目
                    else
                    {
                        if (HttpGet(AppGlobalModel.GetProjectNoReleaseList + $"?deptId={deptInfo.deptId}", ref projectUnreleasedList))
                        {
                            dataGridView_未发布项目.DataSource = new SortableBindingList<ProjectResultModel>(projectUnreleasedList);
                            dataGridView_未发布项目.ClearSelection();
                        }
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_搜索_Click(object sender, EventArgs e)
        {
            var selectName = textBox_搜索.Text.Trim();
            if (!string.IsNullOrWhiteSpace(selectName))
            {
                var selectIndex = tabControl_项目.SelectedIndex;
                if (selectIndex == 0)
                {
                    var list = projectAllList?.Where(o => o.name.Contains(selectName)).ToList();
                    dataGridView_项目列表.DataSource = new SortableBindingList<ProjectResultModel>(list);
                }
                else if (selectIndex == 1)
                {
                    var list = projectCreateList?.Where(o => o.name.Contains(selectName)).ToList();
                    dataGridView_我创建的项目.DataSource = new SortableBindingList<ProjectResultModel>(list);
                }
                else
                {
                    var list = projectUnreleasedList?.Where(o => o.name.Contains(selectName)).ToList();
                    dataGridView_未发布项目.DataSource = new SortableBindingList<ProjectResultModel>(list);
                }
            }
        }

        /// <summary>
        /// 组织机构选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_组织机构_AfterSelect(object sender, TreeViewEventArgs e)
        {
            button_搜索.Enabled = false;
            textBox_搜索.Enabled = false;
            button_新建.Enabled = false;
            button_修改.Enabled = false;
            button_下载模板.Enabled = false;
            button_删除.Enabled = false;
            button_导入.Enabled = false;
            button_停用.Enabled = false;
            dataGridView_项目列表.DataSource = null;
            dataGridView_项目列表.ClearSelection();
            dataGridView_我创建的项目.DataSource = null;
            dataGridView_我创建的项目.ClearSelection();
            dataGridView_未发布项目.DataSource = null;
            dataGridView_未发布项目.ClearSelection();

            var deptInfo = (DeptInfoResultModel)e.Node.Tag;

            #region 获取部门权限
            if (!HttpPost(AppGlobalModel.GetDeptMenu, $"deptId={deptInfo.deptId}", ref authList))
            {
                authList = new List<string>();
            }
            #endregion

            NodeButtonAuth(deptInfo);
        }

        private void NodeButtonAuth(DeptInfoResultModel deptInfo)
        {
            if (deptInfo.deptType == "2")
            {
                //新建
                button_新建.Enabled = authList.Exists(o => o == "promanage:add");
                //下载模板
                button_下载模板.Enabled = authList.Exists(o => o == "promanage:down");
                //导入
                button_导入.Enabled = authList.Exists(o => o == "promanage:import");

                if (tabControl_项目.SelectedIndex == 0)
                {
                    if (authList.Exists(o => o == "promanage:list"))
                    {
                        //搜索
                        button_搜索.Enabled = true;
                        textBox_搜索.Enabled = true;
                    }
                }
                else
                {
                    //搜索
                    button_搜索.Enabled = true;
                    textBox_搜索.Enabled = true;
                }

                LoadProjectList();
            }
            else
            {
                //下载模板
                button_下载模板.Enabled = authList.Exists(o => o == "promanage:down");
            }
        }

        /// <summary>
        /// 选项卡切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProjectList();
        }

        /// <summary>
        /// 节点点击
        /// </summary>
        private bool treeViewNodeMouseClick = true;
        private void treeView_组织机构_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewNodeMouseClick)
            {
                e.Node.Expand();
            }

            treeViewNodeMouseClick = true;
        }

        private void treeView_组织机构_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }

        private void treeView_组织机构_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView_组织架构.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView_组织架构.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView_组织架构.SelectedNode.ForeColor = Color.Black;
            }
        }

        private void treeView_组织机构_Leave(object sender, EventArgs e)
        {
            if (treeView_组织架构.SelectedNode != null)
            {
                //让选中项背景色呈现红色
                treeView_组织架构.SelectedNode.BackColor = SystemColors.Highlight;
                //前景色为白色
                treeView_组织架构.SelectedNode.ForeColor = Color.White;
            }
        }

        #region dataGridView事件
        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                               e.RowBounds.Location.Y,
                                               dataGridView_项目列表.RowHeadersWidth - 4,
                                               e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView_项目列表.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView_项目列表.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);

            var dataGridView = (DataGridView)sender;
            if (dataGridView.Rows.Count > 0)
            {
                var dataList = ((BindingList<ProjectResultModel>)dataGridView.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //if (dataList[i].userId == AppGlobalModel.UseInfo.id)
                    //{
                    //    dataGridView.Rows[i].Cells[0].Style.ForeColor = SystemColors.Highlight;
                    //    dataGridView.Rows[i].Cells[0].Style.Font = new Font("微软雅黑", 12F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(134)));
                    //}
                    if (authList.Exists(o => o == "promanage:edit"))
                    {
                        dataGridView.Rows[i].Cells[0].Style.ForeColor = SystemColors.Highlight;
                        dataGridView.Rows[i].Cells[0].Style.Font = new Font("微软雅黑", 12F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(134)));
                    }
                }
            }

        }

        #region 单元格格式化事件
        /// <summary>
        /// 单元格格式化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var selectIndex = tabControl_项目.SelectedIndex;
            if (selectIndex == 0)
            {
                if (dataGridView_项目列表.Columns["Column7"].Index == e.ColumnIndex)
                {
                    CellFormatting(e);
                }
            }
            else if (selectIndex == 1)
            {
                if (dataGridView_我创建的项目.Columns["Column5"].Index == e.ColumnIndex)
                {
                    CellFormatting(e);
                }
            }
            else if(selectIndex == 2)
            {
                if (dataGridView_未发布项目.Columns["Column6"].Index == e.ColumnIndex)
                {
                    CellFormatting(e);
                }
            }
        }

        private void CellFormatting(DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value == null)
            {
                return;
            }
            if (e.Value.ToString().Equals("0"))
            {
                e.Value = "启用";
            }
            else if (e.Value.ToString().Equals("1"))
            {
                e.Value = "停用";
            }
            else if (e.Value.ToString().Equals("2"))
            {
                e.Value = "未发布";
            }
            else
            {
                e.Value = "删除";
            }
        }
        #endregion

        /// <summary>
        /// 选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            button_修改.Enabled = false;
            button_删除.Enabled = false;
            button_停用.Enabled = false;

            if (e.RowIndex > -1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                var projectInfo = ((BindingList<ProjectResultModel>)dataGridView.DataSource)[e.RowIndex];
                if (projectInfo.userId == AppGlobalModel.UseInfo.id)
                {
                    button_修改.Enabled = true;
                    button_删除.Enabled = true;

                    if (projectInfo.status == 0)
                    {
                        button_停用.Enabled = true;
                        button_停用.Text = "停用";
                    }
                    else if (projectInfo.status == 1)
                    {
                        button_停用.Enabled = true;
                        button_停用.Text = "启用";
                    }
                    else
                    {
                        button_停用.Visible = false;
                    }

                    if (e.ColumnIndex == 0)
                    {
                        var frm = new FrmProjectEdit(projectInfo.parentId, projectInfo.id);
                        frm.Owner = this;
                        frm.Show();
                        LoadProjectList();
                        //if (frm.ShowDialog() == DialogResult.OK)
                        //{
                        //    LoadProjectList();
                        //}
                    }
                }
                else
                {
                    button_修改.Enabled = authList.Exists(o => o == "promanage:edit");
                    button_删除.Enabled = authList.Exists(o => o == "promanage:del");

                    if (projectInfo.status == 0)
                    {
                        button_停用.Enabled = authList.Exists(o => o == "promanage:disable");
                        button_停用.Text = "停用";
                    }
                    else if (projectInfo.status == 1)
                    {
                        button_停用.Enabled = authList.Exists(o => o == "promanage:disable");
                        button_停用.Text = "启用";
                    }
                    else
                    {
                        button_停用.Visible = false;
                    }

                    if (authList.Exists(o => o == "promanage:edit") && e.ColumnIndex == 0)
                    {
                        var frm = new FrmProjectEdit(projectInfo.parentId, projectInfo.id);
                        frm.Owner = this;
                        frm.Show();
                        LoadProjectList();
                        //if (frm.ShowDialog() == DialogResult.OK)
                        //{
                        //    LoadProjectList();
                        //}
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_新建_Click(object sender, EventArgs e)
        {
            var selectDeptInfo = (DeptInfoResultModel)treeView_组织架构.SelectedNode.Tag;
            //var frm = openChildFrom(new FrmProjectEdit(selectDeptInfo.deptId, null));
            var frm = new FrmProjectEdit(selectDeptInfo.deptId, null);
            frm.Owner = this;
            frm.Show();
            LoadProjectList();
          
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    LoadProjectList();
            //}
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_修改_Click(object sender, EventArgs e)
        {
            var selectDeptInfo = (DeptInfoResultModel)treeView_组织架构.SelectedNode.Tag;
            var selectIndex = tabControl_项目.SelectedIndex;
            DataGridView dataGridView;
            if (selectIndex == 0)
            {
                dataGridView = dataGridView_项目列表;
            }
            else if (selectIndex == 1)
            {
                dataGridView = dataGridView_我创建的项目;
            }
            else
            {
                dataGridView = dataGridView_未发布项目;
            }

            var selectRowIndex = dataGridView.SelectedRows[0].Index;
            var modelInfo = ((BindingList<ProjectResultModel>)dataGridView.DataSource)[selectRowIndex];
            var frm = new FrmProjectEdit(selectDeptInfo.deptId, modelInfo.id);
            frm.Owner = this;
            frm.Show();
            LoadProjectList();
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    LoadProjectList();
            //}
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_删除_Click(object sender, EventArgs e)
        {
            var selectIndex = tabControl_项目.SelectedIndex;
            DataGridView dataGridView;
            if (selectIndex == 0)
            {
                dataGridView = dataGridView_项目列表;
            }
            else if (selectIndex == 1)
            {
                dataGridView = dataGridView_我创建的项目;
            }
            else
            {
                dataGridView = dataGridView_未发布项目;
            }

            var selectRowIndex = dataGridView.SelectedRows[0].Index;
            var list = (BindingList<ProjectResultModel>)dataGridView.DataSource;
            var modelInfo = list[selectRowIndex];

            if (ShowSuccessOKCancelMsg($"是否确定删除【{modelInfo.name}】！") == DialogResult.OK)
            {
                var resultData = string.Empty;
                if (HttpGet(AppGlobalModel.DelProjectLevel + "?projectLevelId=" + modelInfo.id, ref resultData))
                {
                    list.RemoveAt(selectRowIndex);
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = list;
                }
            }
        }

        /// <summary>
        /// 下载模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_下载模板_Click(object sender, EventArgs e)
        {
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.GetProjectImport, ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.Owner = this;
                frm.Show();
                LoadProjectList();
                //frm.ShowDialog();
            }
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_导入_Click(object sender, EventArgs e)
        {
            var selectDeptInfo = (DeptInfoResultModel)treeView_组织架构.SelectedNode.Tag;
            if (selectDeptInfo != null)
            {
                var frm = new FrmSelectProjectType(selectDeptInfo.deptId);
                frm.Owner = this;
                frm.Show();
                LoadProjectList();
                //if (frm.ShowDialog() == DialogResult.OK)
                //{
                //    LoadProjectList();
                //}
            }
        }

        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_停用_Click(object sender, EventArgs e)
        {
            var selectIndex = tabControl_项目.SelectedIndex;
            DataGridView dataGridView;
            if (selectIndex == 0)
            {
                dataGridView = dataGridView_项目列表;
            }
            else if (selectIndex == 1)
            {
                dataGridView = dataGridView_我创建的项目;
            }
            else
            {
                dataGridView = dataGridView_未发布项目;
            }

            var selectRowIndex = dataGridView.SelectedRows[0].Index;
            var list = (BindingList<ProjectResultModel>)dataGridView.DataSource;
            var modelInfo = list[selectRowIndex];

            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.ProjectStartOrEnd + "?orderId=" + modelInfo.id + "&status=" + (modelInfo.status == 0 ? 1 : 0), ref resultData))
            {
                LoadProjectList();
            }
        }

        

    }
}