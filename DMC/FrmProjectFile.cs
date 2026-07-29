using DMC.Helper;
using DMC.Models;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace DMC
{

    /// <summary>
    /// 项目文件
    /// </summary>
    public partial class FrmProjectFile : BaseForm
    {
        /// <summary>
        /// 树形结构上面的右键菜单  label1
        /// </summary>
        private ContextMenuStrip stripTreeView = new ContextMenuStrip();

        /// <summary>
        /// 项目文件列表右键菜单
        /// </summary>
        private ContextMenuStrip stripDataGridView = new ContextMenuStrip();

        /// <summary>
        /// 节点点击事件
        /// </summary>
        private List<TreeNode> listCheckNodes = new List<TreeNode>();

        /// <summary>
        /// 选择文件列表
        /// </summary>
        private IEnumerable<GetProjectFileListModel> selectFileList = null;

        /// <summary>
        /// 列宽自适应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;

            // 查找列名为"文件名称"的列的索引
            int fileNameColumnIndex = -1;
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (dgv.Columns[i].HeaderText == "文件名称")
                {
                    fileNameColumnIndex = i;
                    break;
                }
            }

            // 如果没有找到"文件名称"列，则退出
            if (fileNameColumnIndex == -1)
                return;

            // 保存所有列(除"文件名称"列外)的原始宽度
            Dictionary<int, int> originalWidths = new Dictionary<int, int>();
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (i != fileNameColumnIndex)
                {
                    originalWidths[i] = dgv.Columns[i].Width;
                }
            }

            // 设置所有列(除"文件名称"列外)不参与自动调整,保持固定宽度
            for (int i = 0; i < dgv.Columns.Count; i++)
            {
                if (i != fileNameColumnIndex)
                {
                    dgv.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    dgv.Columns[i].Width = originalWidths[i];
                    dgv.Columns[i].Resizable = DataGridViewTriState.True; // 允许用户调整
                }
            }

            // 计算"文件名称"列应该占用的宽度
            int usedWidth = dgv.RowHeadersWidth;
            foreach (KeyValuePair<int, int> pair in originalWidths)
            {
                usedWidth += pair.Value;
            }

            // 考虑可能出现的垂直滚动条
            int scrollBarWidth = (dgv.DisplayedRowCount(true) < dgv.RowCount) ? SystemInformation.VerticalScrollBarWidth : 0;

            // 计算"文件名称"列可用宽度
            int fileNameColumnWidth = dgv.Width - usedWidth - scrollBarWidth - 2; // 2为边框宽度

            // 确保"文件名称"列宽度不小于某个最小值
            int minFileNameColumnWidth = 100; // 设置最小宽度为100像素
            fileNameColumnWidth = Math.Max(fileNameColumnWidth, minFileNameColumnWidth);

            // 设置"文件名称"列宽度
            dgv.Columns[fileNameColumnIndex].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dgv.Columns[fileNameColumnIndex].Width = fileNameColumnWidth;
            dgv.Columns[fileNameColumnIndex].Resizable = DataGridViewTriState.True; // 允许用户调整
        }

        /// <summary>
        /// 节点点击
        /// </summary>
        private bool treeViewNodeMouseClick = true;

        /// <summary>
        /// 权限列表
        /// </summary>
        private List<string> authList = new List<string>();

        /// <summary>
        /// 文件的上传权限List
        /// </summary>
        private List<string> uploadAndAuthority = new List<string>() { "审图过程资料", "审图版图纸", "审图资料", "(01)一审", "(02)二审", "(03)三审" };

        /// <summary>
        /// 用户名List
        /// </summary>
        private List<string> userNameList = new List<string>() { "陈卓", "王艳" };

        /// <summary>
        /// 定义一个bool变量，检查文件夹下是不是还有文件夹
        /// </summary>
        private bool emptyDir = true;

        /// <summary>
        /// 项目文件主页面
        /// </summary>
        public FrmProjectFile()
        {
            InitializeComponent();

            dataGridView_objectFile.AutoGenerateColumns = false;
            if (GlobalVariables.companyName == "吉林医药设计有限公司")
            {
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
            }
        }

        #region 项目文件加载

        /// <summary>
        /// 双击打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void FrmProjectFile_Load(object sender, EventArgs e)
        {
            //加载组织架构
            if (AppGlobalModel.DeptList != null && AppGlobalModel.DeptList.Any())
            {
                foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == "0"))
                {
                    var projectFileTreeView = new ProjectFileTreeViewModel();
                    projectFileTreeView.id = item.deptId;
                    projectFileTreeView.name = item.deptName;
                    projectFileTreeView.deptType = item.deptType;
                    projectFileTreeView.proType = -1;
                    projectFileTreeView.parentId = item.parentId;

                    TreeNode root = new TreeNode();
                    //根目录名称
                    root.Text = projectFileTreeView.name;
                    root.Tag = projectFileTreeView;
                    treeView_ProjectFileTreeView.Nodes.Add(root);

                    LoadTreeView(root);
                }
            }
        }
        #endregion

        /// <summary>
        /// 加载组织架构
        /// </summary>
        /// <param name="treeNode">架构结点</param>
        private void LoadTreeView(TreeNode treeNode)
        {
            /// 获取父节点 ID  
            var parentId = ((ProjectFileTreeViewModel)treeNode.Tag).id;

            /// 遍历所有子节点，判断是否属于当前节点的子节点  
            foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == parentId))
            {
                // 跳过非法节点  
                if (item.deptType == null || item.deptType == "3")
                {
                    continue;
                }

                // 创建新的项目树视图模型  
                var projectFileTreeView = new ProjectFileTreeViewModel
                {
                    id = item.deptId,
                    name = item.deptName,
                    deptType = item.deptType,
                    proType = -1,
                    parentId = item.parentId
                };

                // 创建新的树节点  
                TreeNode root = new TreeNode
                {
                    // 设置树节点名称  
                    Text = projectFileTreeView.name,
                    Tag = projectFileTreeView
                };

                // 将新节点添加到当前节点的子节点列表中  
                treeNode.Nodes.Add(root);

                // 递归加载子节点  
                LoadTreeView(root);
            }

            // 默认展开所有节点  
            treeView_ProjectFileTreeView.ExpandAll();
            // 选择第一个节点  

            // 确保TreeView有节点  
            if (treeView_ProjectFileTreeView.Nodes.Count > 0)
            {
                // 选中第一个节点  
                treeView_ProjectFileTreeView.SelectedNode = treeView_ProjectFileTreeView.Nodes[0];
                treeView_ProjectFileTreeView.Focus();
                treeView_ProjectFileTreeView.SelectedNode.Expand();

                // 将滚动条移动到最顶层  
                treeView_ProjectFileTreeView.Nodes[0].EnsureVisible();
            }
        }

        /// <summary>
        /// 加载项目
        /// </summary>
        /// <param name="treeNode"></param>
        private void LoadProjectTreeView(TreeNode treeNode, ProjectResultModel data)
        {
            // 创建新的项目树视图模型  
            var projectFileTreeView = new ProjectFileTreeViewModel
            {
                id = data.id,
                name = data.type == 0 ? data.identifier + "-" + data.name : data.name,
                deptType = "-1",
                proType = data.type,
                parentId = data.parentId,
                projectId = data.projectId
            };

            // 创建新的树节点  
            TreeNode root = new TreeNode
            {
                // 设置树节点名称  
                Text = projectFileTreeView.name,
                Tag = projectFileTreeView
            };

            // 将新节点添加到当前节点的子节点列表中  
            treeNode.Nodes.Add(root);

            // 对所有子节点进行排序  
            //SortTreeViewNodes(treeNode.Nodes);

        }

        /// <summary>
        /// 项目排序
        /// </summary>
        /// <param name="nodes"></param>
        private void SortTreeViewNodes(TreeNodeCollection nodes)
        {
            // 将节点转换为列表，进行排序后替换原节点  
            var nodeList = nodes.Cast<TreeNode>().ToList();
            nodeList.Sort(new TreeNodeComparer()); // 使用自定义比较器  
            nodes.Clear(); // 清空当前节点集合  
            foreach (var node in nodeList)
            {
                nodes.Add(node); // 重新添加排序后的节点  
            }

            // 递归排序所有子节点  
            foreach (TreeNode node in nodeList)
            {
                SortTreeViewNodes(node.Nodes);
            }
        }

        /// <summary>
        /// 排序比较
        /// </summary>
        private class TreeNodeComparer : IComparer<TreeNode>
        {
            public int Compare(TreeNode x, TreeNode y)
            {
                // 按照节点名称排序  
                return string.Compare(y.Text, x.Text);
            }
        }



        #region 加载项目层级
        /// <summary>
        /// 加载项目层级
        /// </summary>
        private void LoadProjectLevel(string parentId = null)
        {
            TreeNode treeNode;
            if (string.IsNullOrWhiteSpace(parentId))
            {
                treeNode = treeView_ProjectFileTreeView.SelectedNode;
                parentId = ((ProjectFileTreeViewModel)treeNode.Tag).id;
            }
            else
            {
                treeNode = treeView_ProjectFileTreeView.SelectedNode.Parent;
            }

            var resultData = new List<ProjectResultModel>();
            if (HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={parentId}", ref resultData))
            {
                treeNode.Nodes.Clear();

                foreach (var item in resultData)
                {
                    LoadProjectTreeView(treeNode, item);
                }

                treeNode.Expand();
            }
        }
        #endregion

        #region 树结构右键菜单
        /// <summary>
        /// 树结构右键菜单
        /// </summary>
        private void LoadStripTreeView()
        {
            //清理选项
            stripTreeView.Items.Clear();
            //获取右键选的节点标记；
            var selectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;
            //判断先定的节点proType, 类型（0项目，1阶段，2专业，3子项，4文件夹，5文件）
            if (selectInfo.proType >= 1 && listCheckNodes.Count <= 1)
            {
                if (selectInfo.proType >= 2 && listCheckNodes.Count <= 1)
                {
                    if (authList.Exists(o => o == "profile:folder:add"))
                    {
                        ToolStripItem ts_newAdd = new ToolStripMenuItem("新建文件夹");
                        ts_newAdd.Click += new EventHandler(AddDir);
                        stripTreeView.Items.Add(ts_newAdd);
                    }

                    if (selectInfo.proType > 2)
                    {
                        //重命名
                        if (authList.Exists(o => o == "profile:folder:edit"))
                        {
                            ToolStripItem ts_changeName = new ToolStripMenuItem("重命名");
                            ts_changeName.Click += new EventHandler(ChangeDir);
                            stripTreeView.Items.Add(ts_changeName);
                        }
                        //删除
                        if (authList.Exists(o => o == "profile:folder:del"))
                        {
                            ToolStripItem ts_delete = new ToolStripMenuItem("删除");
                            ts_delete.Click += new EventHandler(DelDir);
                            stripTreeView.Items.Add(ts_delete);
                        }
                        //复制新建
                        if (stripTreeView.Items.Count > 0)
                        {
                            stripTreeView.Items.Add("-");
                            ToolStripItem ts_copy = new ToolStripMenuItem("复制新建");
                            ts_copy.Click += new EventHandler(CopyDir);
                            stripTreeView.Items.Add(ts_copy);
                            stripTreeView.Items.Add("-");
                        }
                        //粘贴文件
                        if (stripTreeView.Items.Count > 0 && selectFileList != null)
                        {
                            stripTreeView.Items.Add("-");
                            ToolStripItem file_copy = new ToolStripMenuItem("粘贴文件");
                            file_copy.Click += new EventHandler(SelectFileToList_Paste);
                            stripTreeView.Items.Add(file_copy);
                            stripTreeView.Items.Add("-");
                        }
                    }
                    // "上传文件夹" : "上传文件"
                    if (authList.Exists(o => o == "profile:upload"))
                    {
                        //判断是不是有后加的审版图纸上传文件的条件
                        if (uploadAndAuthority.Contains(selectInfo.name))
                        {
                            //"上传文件夹" : "上传文件"
                            if (userNameList.Contains(AppGlobalModel.UseInfo.realName))
                            {
                                ToolStripItem ts_uploadFile = new ToolStripMenuItem(selectInfo.proType == 2 ? "上传文件夹" : "上传文件");
                                ts_uploadFile.Click += new EventHandler(btn上传文件_Click);
                                stripTreeView.Items.Add(ts_uploadFile);
                            }
                            else
                            {
                                btn上传文件.Enabled = false;
                            }
                        }
                        else
                        {
                            ToolStripItem ts_uploadFile = new ToolStripMenuItem(selectInfo.proType == 2 ? "上传文件夹" : "上传文件");
                            ts_uploadFile.Click += new EventHandler(btn上传文件_Click);
                            stripTreeView.Items.Add(ts_uploadFile);
                        }
                    }
                }

                if (stripTreeView.Items.Count > 0)
                {
                    stripTreeView.Items.Add("-");
                }

                ToolStripItem ts_download = new ToolStripMenuItem("下载文件");
                ts_download.Click += new EventHandler(DownloadFile);
                stripTreeView.Items.Add(ts_download);
                stripTreeView.Items.Add("-");
            }

            if (authList.Exists(o => o == "profile:cart:add"))
            {
                ToolStripItem ts_selectFileToList = new ToolStripMenuItem("选定文件加入列表");
                ts_selectFileToList.Click += new EventHandler(SelectFolderToList);
                stripTreeView.Items.Add(ts_selectFileToList);
            }

            if (authList.Exists(o => o == "profile:apply"))
            {
                ToolStripItem ts_allFileApproval = new ToolStripMenuItem("全文件-发起审批");
                ts_allFileApproval.Click += new EventHandler(AllFileApproval);
                stripTreeView.Items.Add(ts_allFileApproval);
            }
        }

        /// <summary>
        /// 新增文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDir(object sender, EventArgs e)
        {
            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;
            var frm = new FrmAddChildOrFolder(projectInfo.id, projectInfo.proType);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadProjectLevel();
            }
        }

        /// <summary>
        /// 修改文件夹名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeDir(object sender, EventArgs e)
        {
            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;
            var frm = new FrmEditChildOrFolder(projectInfo.id, projectInfo.name);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadProjectLevel(projectInfo.parentId);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelDir(object sender, EventArgs e)
        {
            if (ShowSuccessOKCancelMsg("是否确定删除！") == DialogResult.OK)
            {
                if (dataGridView_objectFile.Rows.Count > 0)
                {
                    btn全选_Click(sender, e);
                    btn删除文件_Click(sender, e);
                }

                if (dataGridView_objectFile.Rows.Count == 0 && emptyDir)
                {
                    var projectId = ((ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag).id;
                    var resultData = string.Empty;
                    if (HttpGet(AppGlobalModel.DelProjectLevel + "?projectLevelId=" + projectId, ref resultData))
                    {
                        treeView_ProjectFileTreeView.SelectedNode.Remove();
                    }
                }
                else
                {
                    MessageBox.Show("文件夹不为空，请删除子文件夹与文件后 再删除！");
                }
            }
            //if (userName.Contains(AppGlobalModel.UseInfo.realName))
            //{
            //    if (ShowSuccessOKCancelMsg("是否确定删除！") == DialogResult.OK)
            //    {

            //        if (dataGridView1.Rows.Count > 0)
            //        {
            //            btn全选_Click(sender, e);
            //            btn删除文件_Click(sender, e);
            //        }

            //        if (dataGridView1.Rows.Count == 0 && emptyDir)
            //        {
            //            //Splasher.Show(typeof(FrmLoading));
            //            var projectId = ((ProjectFileTreeViewModel)treeView1.SelectedNode.Tag).id;
            //            var resultData = string.Empty;
            //            if (HttpGet(AppGlobalModel.DelProjectLevel + "?projectLevelId=" + projectId, ref resultData))
            //            {
            //                treeView1.SelectedNode.Remove();
            //            }
            //        }
            //        else 
            //        {
            //            MessageBox.Show("文件夹不为空，请删除子文件夹与文件后 再删除！");
            //        }

            //    }
            //    //closeFrm();
            //}
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void CloseFrm()
        {
            Splasher.Show(typeof(FrmLoading));
            Splasher.Close();
        }

        /// <summary>
        /// 复制项目
        /// </summary>
        private List<ProjectResultModel> copyProjectList = null;

        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyDir(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));

            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;

            copyProjectList = new List<ProjectResultModel>();
            GetProjectLevel(projectInfo.id);

            var projectParentInfo = (ProjectFileTreeViewModel)(treeView_ProjectFileTreeView.SelectedNode.Parent.Tag);
            var obj = new
            {
                parentId = projectInfo.parentId,
                name = projectInfo.name + "01"
            };

            var resultData = string.Empty;
            var postData = $"projectInfo={JsonConvert.SerializeObject(obj)}";
            string url = projectParentInfo.proType == 2 ? AppGlobalModel.AddProjectSubitem : AppGlobalModel.AddProjectDir;
            if (HttpPost(url, postData, ref resultData))
            {
                if (copyProjectList.Any())
                {
                    SaveProjectDir(projectInfo.id, resultData);
                }

                Splasher.Close();
                LoadProjectLevel(projectInfo.parentId);
            }
        }

        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveDir(object sender, EventArgs e)
        {
            //Splasher.Show(typeof(FrmLoading));

            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;

            copyProjectList = new List<ProjectResultModel>();
            GetProjectLevel(projectInfo.id);

            var projectParentInfo = (ProjectFileTreeViewModel)(treeView_ProjectFileTreeView.SelectedNode.Parent.Tag);
            var obj = new
            {
                parentId = projectInfo.parentId,
                name = projectInfo.name
            };

            var resultData = string.Empty;
            var postData = $"projectInfo={JsonConvert.SerializeObject(obj)}";
            string url = projectParentInfo.proType == 2 ? AppGlobalModel.AddProjectSubitem : AppGlobalModel.AddProjectDir;
            if (HttpPost(url, postData, ref resultData))
            {
                if (copyProjectList.Any())
                {
                    SaveProjectDir(projectInfo.id, resultData);
                }

                Splasher.Close();
                LoadProjectLevel(projectInfo.parentId);
            }
        }

        /// <summary>
        /// 保存项目文件夹
        /// </summary>
        /// <param name="oldParentId"></param>
        /// <param name="parentId"></param>
        private void SaveProjectDir(string oldParentId, string parentId)
        {
            var queryList = copyProjectList.Where(o => o.parentId == oldParentId);
            foreach (var item in queryList)
            {
                var obj = new
                {
                    parentId = parentId,
                    name = item.name
                };

                var resultData = string.Empty;
                var postData = $"projectInfo={JsonConvert.SerializeObject(obj)}";
                if (HttpPost(AppGlobalModel.AddProjectDir, postData, ref resultData))
                {
                    SaveProjectDir(item.id, resultData);
                }
            }
        }

        /// <summary>
        /// 创建文件层级
        /// </summary>
        /// <param name="projectInfoId"></param>
        private void GetProjectLevel(string projectInfoId)
        {
            var resultData = new List<ProjectResultModel>();
            if (HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={projectInfoId}", ref resultData))
            {
                if (resultData != null && resultData.Any())
                {
                    copyProjectList.AddRange(resultData);
                    foreach (var item in resultData)
                    {
                        GetProjectLevel(item.id);
                    }
                }
            }
        }

        /// <summary>
        /// 目录结构下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadFile(object sender, EventArgs e)
        {
            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;

            Splasher.Show(typeof(FrmLoading));

            var para = new { fileIds = projectInfo.id, type = 1 };
            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.ProjectFileDownload, para, ref resultData))
            {
                Splasher.Close();

                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
            Splasher.Close();
        }

        /// <summary>
        /// 左侧架构树中的文件夹加入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFolderToList(object sender, EventArgs e)
        {
            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;

            string folderIds;
            if (listCheckNodes != null && listCheckNodes.Any())
            {
                folderIds = string.Join(",", listCheckNodes.Select(o => ((ProjectFileTreeViewModel)o.Tag).id));
            }
            else
            {
                folderIds = projectInfo.id;
            }
            var para = new
            {
                proId = projectInfo.proType == 0 ? projectInfo.id : projectInfo.projectId,//项目id(必填 所属项目)
                fileType = "0",//文件类型0项目区 1归档区
                fileIds = "",//文件id 逗号分割
                folderIds = projectInfo.proType == 0 ? "" : folderIds,//文件夹id 逗号分割
                proIds = projectInfo.proType == 0 ? projectInfo.id : ""//项目id(如果添加项目)
            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.AddFileCart, para, ref resultData))
            {
                ShowSuccessMsg("添加成功！");
            }
        }

        /// <summary>
        /// 全文件发起审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllFileApproval(object sender, EventArgs e)
        {
            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;
            string folderIds;
            if (listCheckNodes != null && listCheckNodes.Any())
            {
                folderIds = string.Join(",", listCheckNodes.Select(o => ((ProjectFileTreeViewModel)o.Tag).id));
            }
            else
            {
                folderIds = projectInfo.id;
            }

            var resultData = new GetProjectAttributeModel();
            if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={(projectInfo.proType == 0 ? projectInfo.id : projectInfo.projectId)}", ref resultData))
            {
                var frm = new FrmInitApproval(resultData, (projectInfo.proType == 0 ? 2 : 1), folderIds, 0);
                frm.ShowDialog();
            }
        }
        #endregion

        #region 项目文件列表右键菜单

        /// <summary>
        /// 项目文件列表右键菜单
        /// </summary>
        private void LoadStripDataGridView()
        {
            stripDataGridView.Items.Clear();

            if (authList.Exists(o => o == "profile:open"))
            {
                ToolStripItem ts_open = new ToolStripMenuItem("打开");
                ts_open.Click += new EventHandler(OpenFile);
                stripDataGridView.Items.Add(ts_open);
            }

            if (authList.Exists(o => o == "profile:rename"))
            {
                ToolStripItem ts_changeName = new ToolStripMenuItem("重命名");
                ts_changeName.Click += new EventHandler(ChangeName);
                stripDataGridView.Items.Add(ts_changeName);
            }
            ///
            //////////////////////////////////////////////////////////////////////////////////////////////////////
            ///
            if (authList.Exists(o => o == "profile:del"))
            {
                ToolStripItem ts_delete = new ToolStripMenuItem("删除");
                ts_delete.Click += new EventHandler(btn删除文件_Click);
                stripDataGridView.Items.Add(ts_delete);
            }

            if (stripDataGridView.Items.Count > 0)
            {
                stripDataGridView.Items.Add("-");
            }

            if (authList.Exists(o => o == "profile:upload"))
            {
                ToolStripItem ts_uploadFile = new ToolStripMenuItem("上传文件");
                ts_uploadFile.Click += new EventHandler(btn上传文件_Click);
                stripDataGridView.Items.Add(ts_uploadFile);
            }

            if (authList.Exists(o => o == "profile:upload"))
            {
                ToolStripItem ts_uploadFile = new ToolStripMenuItem("剪切文件");
                ts_uploadFile.Click += new EventHandler(SelectFileToList_Copy);
                stripDataGridView.Items.Add(ts_uploadFile);
            }

            if (authList.Exists(o => o == "profile:version"))
            {
                ToolStripItem ts_viewVersion = new ToolStripMenuItem("查看版本");
                ts_viewVersion.Click += new EventHandler(ViewVersion);
                stripDataGridView.Items.Add(ts_viewVersion);
            }

            if (authList.Exists(o => o == "profile:replace"))
            {
                ToolStripItem ts_replaceFile = new ToolStripMenuItem("替换文件");
                ts_replaceFile.Click += new EventHandler(ReplaceFile);
                stripDataGridView.Items.Add(ts_replaceFile);
            }

            if (stripDataGridView.Items.Count > 0)
            {
                stripDataGridView.Items.Add("-");
            }

            if (authList.Exists(o => o == "profile:cart:add"))
            {
                ToolStripItem ts_selectFileToList = new ToolStripMenuItem("选定文件加入列表");
                ts_selectFileToList.Click += new EventHandler(SelectFileToList);
                stripDataGridView.Items.Add(ts_selectFileToList);
            }

            if (authList.Exists(o => o == "profile:apply"))
            {
                ToolStripItem ts_allFileApproval = new ToolStripMenuItem("选定文件-发起审批");
                ts_allFileApproval.Click += new EventHandler(btn发起审批_Click);
                stripDataGridView.Items.Add(ts_allFileApproval);
            }

            if (stripDataGridView.Items.Count > 0)
            {
                stripDataGridView.Items.Add("-");
            }

            if (authList.Exists(o => o == "profile:signature"))//判断是不是有这个profile:signature权限；
            {
                ToolStripItem ts_VisualSignature = new ToolStripMenuItem("发起可视化签名签章");
                ts_VisualSignature.Click += new EventHandler(VisualSignature_Click);
                stripDataGridView.Items.Add(ts_VisualSignature);
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFile(object sender, EventArgs e)
        {
            var index = dataGridView_objectFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;
                var selectModel = list[index];
                var listUrl = list.Select(o => new PreviewAreaViewModel() { filePath = o.filePath, name = o.name }).ToList();
                var frm = new FrmPreviewArea(selectModel.filePath, 0, listUrl);
                frm.Show();
            }
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeName(object sender, EventArgs e)
        {
            var index = dataGridView_objectFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;
                var selectModel = list[index];
                var frm = new FrmFileRename(selectModel);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadProjectFileList();
                }
            }
        }

        /// <summary>
        /// 查看版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewVersion(object sender, EventArgs e)
        {
            var index = dataGridView_objectFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;
                var selectModel = list[index];
                var frm = new FrmSeeHistoryFile(selectModel.id, authList);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReplaceFile(object sender, EventArgs e)
        {
            var index = dataGridView_objectFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;
                var selectModel = list[index];

                var resultJiSuanFrameData = new List<JiSuanFrameModel>();
                if (HttpGet(AppGlobalModel.JiSuanFrame, ref resultJiSuanFrameData))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = AppGlobalModel.InitialDirectory;
                    openFileDialog.Filter = "所有文件(*.*)|*.*";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        #region 保存打开的文件目录
                        AppGlobalModel.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                        ConfigHelper.SaveConfigInfo("InitialDirectory", AppGlobalModel.InitialDirectory);
                        #endregion

                        var resultData = string.Empty;
                        var paras = new Dictionary<string, string>();
                        var fileUpload = new FileUploadModel();

                        fileUpload.isPdf = "1";

                        string fileNameEx = Path.GetExtension(openFileDialog.FileName);
                        if (fileNameEx.ToLower().Equals(".pdf"))
                        {
                            fileUpload.isPdf = "0";
                            PdfReader pdfReader = new PdfReader(openFileDialog.FileName);
                            //总页数
                            int iPageNum = pdfReader.NumberOfPages;

                            fileUpload.pageAll = iPageNum.ToString();
                            fileUpload.pageInfo = new List<PageInfoItem>();

                            PageInfoItem pageInfoItem;
                            JiSuanFrameModel jiSuanFrame;
                            for (var i = 0; i < iPageNum; i++)
                            {
                                var pdfPage = pdfReader.GetPageSizeWithRotation(i + 1);
                                jiSuanFrame = resultJiSuanFrameData.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

                                if (jiSuanFrame != null)
                                {
                                    pageInfoItem = new PageInfoItem();
                                    pageInfoItem.page = (i + 1).ToString();
                                    pageInfoItem.width = (pdfPage.Width).ToString();
                                    pageInfoItem.height = (pdfPage.Height).ToString();
                                    pageInfoItem.frameName = jiSuanFrame.name;
                                    pageInfoItem.folded = jiSuanFrame.folded;

                                    fileUpload.pageInfo.Add(pageInfoItem);
                                }
                                else
                                {
                                    ShowErrorMsg($"此文件{openFileDialog.FileName}，没有对应的图幅，请联系管理员！");
                                    return;
                                }
                            }

                            fileUpload.frameName = fileUpload.pageInfo.First().frameName;
                            fileUpload.folded = (fileUpload.pageInfo.Sum(o => Convert.ToDecimal(o.folded))).ToString();
                        }

                        paras.Add("fileDetails", JsonConvert.SerializeObject(fileUpload));
                        paras.Add("fileId", selectModel.id);
                        if (HttpUploadFile(AppGlobalModel.AgainFileUpload, openFileDialog.FileName, ref resultData, paras))
                        {
                            LoadProjectFileList();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 选定文件加入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFileToList(object sender, EventArgs e)
        {
            var index = dataGridView_objectFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;

                var selectModel = list[index];

                var selectFile = list.Where(o => o.isCheck || o.id == selectModel.id).Select(o => o.id).ToList();

                var para = new
                {
                    proId = selectModel.projectId,//项目id(必填 所属项目)
                    fileType = "0",//文件类型0项目区 1归档区
                    fileIds = string.Join(",", selectFile),//文件id 逗号分割
                    folderIds = "",//文件夹id 逗号分割
                    proIds = ""//项目id(如果添加项目)
                };

                var resultData = string.Empty;
                if (HttpPost(AppGlobalModel.AddFileCart, para, ref resultData))
                {
                    ShowSuccessMsg("添加成功！");
                }
            }
        }


        /// <summary>
        /// 选定文件复制操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFileToList_Copy(object sender, EventArgs e)
        {
            //清理选定文件变量
            selectFileList = null;
            //获取当前表里的行数
            var index = dataGridView_objectFile.CurrentRow.Index;

            if (index > -1)
            {
                //所有文件列队
                var allFileList = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;
                //
                var selectModel = allFileList[index];

                selectFileList = allFileList.Where(o => o.isCheck || o.id == selectModel.id);

                //var selectFileNum = selectFileList.Count();
            }
            if (selectFileList.Count() != 0)
                MessageBox.Show("剪切文件成功！");
        }

        /// <summary>
        /// 粘贴文件操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFileToList_Paste(object sender, EventArgs e)
        {
            //获取右键选的节点标记；
            var selectFolderInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;
            //获取到选择文件夹的信息
            var selectFolderInfoMysqlData = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{selectFolderInfo.id}");
            //选择的文件夹的ancestors列下的内容
            var selectFolderInfo_AncestorsStr = selectFolderInfoMysqlData.Rows[0]["ancestors"].ToString();
            //循环选定的文件
            foreach (var selectFileItem in selectFileList)
            {
                #region 原方法

                ////用选择文件的id取mysql里qz_project
                //var selectInfoMysqlData = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{selectInfo.id}");
                ////每行的ancestors列下的内容
                //var selectInfo_AncestorsStr = selectInfoMysqlData.Rows[0]["ancestors"].ToString();
                ////分割ancestors内的字符串
                //string[] selectInfo_AncestorsStrS = selectInfo_AncestorsStr.Split(',');

                ////用选择文件的id取mysql里qz_project
                //var fileMysqlData = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{selectFileItem.id}");
                ////赋值新的选择文件夹id
                //fileMysqlData.Rows[0]["parent_id"] = selectInfo.id;
                ////每行的ancestors列下的内容
                //var ancestorsStr = fileMysqlData.Rows[0]["ancestors"].ToString();
                ////分割ancestors内的字符串
                //string[] ancestorsStrS = ancestorsStr.Split(',');

                //if (selectInfo_AncestorsStrS[0] == ancestorsStrS[0] && selectInfo_AncestorsStrS[1] == ancestorsStrS[1])
                //{
                //    // 遍历数组，替换匹配的字符串
                //    Array.ForEach(ancestorsStrS, s =>
                //    {
                //        if (s == selectFileItem.parentId)
                //        {
                //            s = selectInfo.id;
                //        }
                //    });
                //    // 使用String.Join方法将数组重新组合成一个字符串
                //    string modifiedString = string.Join(",", ancestorsStrS);

                //    fileMysqlData.Rows[0]["ancestors"] = modifiedString;

                //    // 使用加密安全的随机数生成器  
                //    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                //    {
                //        byte[] randomBytes = new byte[16]; // 16字节数组（128位）  
                //        rng.GetBytes(randomBytes); // 填充随机字节  

                //        // 转换为十六进制字符串  
                //        StringBuilder hexStringBuilder = new StringBuilder();
                //        foreach (byte b in randomBytes)
                //        {
                //            hexStringBuilder.Append(b.ToString("X2")); // 转换为两位十六进制  
                //        }
                //        fileMysqlData.Rows[0]["id"] = hexStringBuilder.ToString();
                //    }

                //    SQLiteDataBase.InsertRowMysql("qz_project", fileMysqlData);
                //    //把原的文件在客户端不显示"is_show"改为"1"
                //    SQLiteDataBase.UpdateDataToMysql("qz_project", "id", $"{selectFileItem.id}", "is_show", "1");
                //}
                //else
                //{
                //    MessageBox.Show("剪切文件需要在同一个项目内操作！");
                //    break;
                //}
                #endregion

                //用选择文件的id取mysql里qz_project
                //var fileMysqlData = SQLiteDataBase.GetDataFromMysql("qz_project", "id", $"{selectFileItem.id}");
                //把选择的文件的parent_id赋值新的选择文件夹id
                //fileMysqlData.Rows[0]["parent_id"] = selectFolderInfo.id;
                //把选择的文件的ancestors赋值新的选择文件夹ancestors+parent_id
                //fileMysqlData.Rows[0]["ancestors"] = selectFolderInfo_AncestorsStr+selectFolderInfo.id+",";
                //写入数据库
                //SQLiteDataBase.InsertRowMysql("qz_project", fileMysqlData);
                //把原的文件在客户端不显示"is_show"改为"1"
                SQLiteDataBase.UpdateDataToMysql("qz_project", "id", $"{selectFileItem.id}", "parent_id", selectFolderInfo.id);
                SQLiteDataBase.UpdateDataToMysql("qz_project", "id", $"{selectFileItem.id}", "ancestors", selectFolderInfo_AncestorsStr + selectFolderInfo.id + ",");
            }
            selectFileList = null;
            LoadProjectFileList();
            MessageBox.Show("文件粘贴成功！");
        }
        #endregion

        #region 加载项目文件列表
        /// <summary>
        /// 加载项目文件列表与读取文件数量、A1数
        /// </summary>
        private void LoadProjectFileList()
        {
            if (authList.Exists(o => o == "profile:list"))//先判断权限
            {
                //获取项目文件列表
                var resultData = new List<GetProjectFileListModel>();
                //选定的节点（标记）对像的项目信息；
                var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;
                //通过项目iD 获取 项目文件列表判断
                if (HttpGet(AppGlobalModel.GetProjectFileList + $"?parentId={projectInfo.id}&tab=0", ref resultData))
                {
                    //绑定获取到的项目文件到datagridviewl中
                    dataGridView_objectFile.DataSource = new SortableBindingList<GetProjectFileListModel>(resultData.OrderBy(o => o.name, new StringRankComparer()).ToList());
                    //清除选中项
                    dataGridView_objectFile.ClearSelection();

                    label1.Text = $"文件数量：{resultData.Count()}";
                    if (GlobalVariables.companyName != "吉林医药设计院有限公司")
                    {
                        label2.Text = $"总A1数量：{resultData.Where(o => !string.IsNullOrWhiteSpace(o.folded)).Sum(o => Convert.ToDecimal(o.folded))}   A1";
                    }
                    else { label2.Visible = false; }

                    btn全选.Text = "全选";
                    btn删除文件.Enabled = false;
                    btn下载.Enabled = false;
                    btn发起审批.Enabled = false;
                }
            }
        }

        #endregion

        public static List<ProjectResultModel> treeNodeSelectProjectInto;

        #region 节点事件
        /// <summary>
        /// 目录树节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_ProjectFile_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                
                Splasher.Show(typeof(FrmLoading));//显示加载动画
                treeView_ProjectFileTreeView.SelectedNode = e.Node;//选中这个节点
                var level = e.Node.Level;//获取节点的层级
                //鼠标右键
                if (e.Button == MouseButtons.Right)
                {
                    if (level >= 3)
                    {
                        if (e.Node.Checked)
                        {
                            var checkInfo = (ProjectFileTreeViewModel)e.Node.Tag;//获取节点的标记对像
                            if (listCheckNodes != null && listCheckNodes.Any())//判断选定的节点集合是不是有值
                            {
                                var oldInfo = (ProjectFileTreeViewModel)listCheckNodes[0].Tag;//获取集合中第一个节点的标记对像

                                if (checkInfo.parentId != oldInfo.parentId || checkInfo.proType != oldInfo.proType)//判断两个节点的父级ID是否一致
                                {
                                    InitTreeViewCheckNodes();//初始化选中的节点不一致就清空集合
                                }
                            }
                        }
                        else
                        {
                            InitTreeViewCheckNodes();//初始化选中的节点不一致就清空集合初始化选中的节点
                        }

                        LoadStripTreeView();//加载右键菜单

                        e.Node.ContextMenuStrip = stripTreeView;//绑定右键菜单
                    }
                }
                else if (e.Button == MouseButtons.Left)//鼠标左键
                {
                    //项目归档
                    btn项目归档.Enabled = false;
                    //选定文件清单
                    btn文件清单.Visible = false;
                    //右侧列表
                    dataGridView_objectFile.DataSource = null;
                    dataGridView_objectFile.ClearSelection();
                    label1.Text = $"文件数量：0";
                    if (GlobalVariables.companyName != "吉林医药设计院有限公司")
                    {
                        label2.Text = $"总A1数量：0   A1";
                    }
                    else
                    {
                        label2.Visible = false;
                    }

                    //空文件夹
                    emptyDir = true;

                    //ProjectFileTreeViewModel 项目文件树结构/1、id /2、name,名称 /3、parentId,上级id /4、proType, 类型（0项目，1阶段，2专业，3子项，4文件夹，5文件）/5、deptType 部门类型（0集团，1院，2所，3专业）没有值就是专业下面的/6、projectId 项目ID
                    var selectInfo = (ProjectFileTreeViewModel)e.Node.Tag;
                    //判断是选定的是不是机构中的 所 层级或是不是-1文件夹级；
                    if (selectInfo.deptType == "2" || selectInfo.deptType == "-1")
                    {
                        /*
                         这段代码涉及根据条件动态生成 URL 地址的操作。下面是代码的详细解释：
                            var url = selectInfo.deptType == "2" ? AppGlobalModel.GetProjectList + $"?deptId={selectInfo.id}&table={(AppGlobalModel.OverallSituationMenu.Exists(o => o == "profile:all:list") ? 0 : 1)}" : AppGlobalModel.GetProjectLevelDetails + $"?parentId={selectInfo.id}";：
                            这是一个条件三元运算符表达式，在这里根据条件动态构建 URL 地址：
                            如果 selectInfo.deptType 的值等于 "2"，则执行 AppGlobalModel.GetProjectList + $"?deptId={selectInfo.id}&table={(AppGlobalModel.OverallSituationMenu.Exists(o => o == "profile:all:list") ? 0 : 1)}".
                            否则，执行 AppGlobalModel.GetProjectLevelDetails + $"?parentId={selectInfo.id}".
                            AppGlobalModel.GetProjectList 和 AppGlobalModel.GetProjectLevelDetails 是两个可能是 URL 基础部分的字符串。
                            {selectInfo.id} 是一个占位符，代表要插入的变量。
                            {(AppGlobalModel.OverallSituationMenu.Exists(o => o == "profile:all:list") ? 0 : 1)} 是一个条件表达式，根据 OverallSituationMenu 集合中是否存在满足条件的元素来决定插入 0 还是 1。
                            综合起来，根据 selectInfo.deptType 的值和 OverallSituationMenu 集合的情况，动态生成不同的 URL 地址赋值给 url 变量。这种方式可以根据条件灵活地构建不同的请求地址，以便根据具体情况获取不同的数据或资源。
                         */
                        //如是所，那么获取到这个所下面的项目列表，如果是项目，那获取这个项目的id： 返回给url字符串；
                        var url = selectInfo.deptType == "2" ? AppGlobalModel.GetProjectList + $"?deptId={selectInfo.id}" +
                            $"&table={(AppGlobalModel.OverallSituationMenu.Exists(o => o == "profile:all:list") ? 0 : 1)}" : AppGlobalModel.GetProjectLevelDetails + 
                            $"?parentId={selectInfo.id}";
                        //resultData是所有项目属性数据列表 或 是项目的属性数据 或 是专业文件夹数据；
                        //var resultData = new List<ProjectResultModel>();
                        if (HttpGet(url, ref treeNodeSelectProjectInto))
                        {
                            /*
                              这段代码片段涉及操作树形控件(TreeView)中节点的清除和重新加载。下面是代码的详细解释：

                              e.Node.Nodes.Clear();：
                              e.Node 表示当前操作的树形控件节点。
                              Nodes.Clear() 方法用于清除该节点下的所有子节点。
                              foreach (var item in resultData)
                              { LoadProjectTreeView(e.Node, item); }：
                              这是一个循环遍历 resultData 集合的操作，对集合中的每个元素执行以下操作：
                              调用 LoadProjectTreeView(e.Node, item) 方法，传递当前树形控件节点和集合中的元素 item 作为参数。
                              LoadProjectTreeView(e.Node, item) 方法：

                              这个方法用于加载树形控件节点的子节点数据。
                              可能的作用是根据集合中的元素 item 创建对应的子节点，并添加到当前节点 e.Node 下，用来重新构建树形结构中的某些部分。
                              综合起来，这段代码的作用是清除当前树形控件节点 e.Node 的所有子节点，然后根据 resultData 集合的内容重新加载该节点的子节点数据。在 LoadProjectTreeView 方法中可能会对子节点进行定制化加载和设置操作，以便更新树形结构的显示内容。
                            */
                            e.Node.Nodes.Clear();
                            foreach (var item in treeNodeSelectProjectInto)
                            {
                                LoadProjectTreeView(e.Node, item);
                            }
                            if (treeNodeSelectProjectInto.Count != 0)
                            {
                                emptyDir = false;
                                // 对所有子节点进行排序  
                                SortTreeViewNodes(e.Node.Nodes);
                            }
                        }
                        #region 获取项目权限
                        if (selectInfo.deptType == "-1")
                        {
                            if (!HttpPost(AppGlobalModel.GetProjectMenu, $"projectId={selectInfo.id}", ref authList))
                            {
                                authList = new List<string>();
                            }

                            btn文件清单.Visible = authList.Exists(o => o == "profile:cart:list");
                        }
                        #endregion

                        //判断是不是项目
                         if (selectInfo.proType == 0)
                        {
                            //项目归档
                            btn项目归档.Enabled = authList.Exists(o => o == "profile:archive");
                            
                        }

                        //判断子项以下获取项目文件
                        if (selectInfo.proType >= 3)
                        {
                            LoadProjectFileList();
                        }
                    }
                    if (treeViewNodeMouseClick)
                    {
                        e.Node.Expand();
                    }
                }
                treeViewNodeMouseClick = true;
                Splasher.Close();
            }
            catch
            {
                Splasher.Close();
            }
        }

        /// <summary>
        /// 树结构选择内容发生变化时事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_ProjectFile_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //选择的节点标注
            var selectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;

            //var boolRen = Ren();
            //判断子项以下获取项目文件

            if (userNameList.Contains(AppGlobalModel.UseInfo.realName))
            {
                if (selectInfo.proType >= 3)
                {
                    //上传文件按钮
                    btn上传文件.Enabled = authList.Exists(o => o == "profile:upload");
                    //全选
                    btn全选.Enabled = true;
                }
                else
                {
                    //上传文件按钮
                    btn上传文件.Enabled = false;
                    //全选
                    btn全选.Enabled = false;
                }
            }
            else
            {
                if (!uploadAndAuthority.Contains(selectInfo.name))
                {
                    if (selectInfo.proType >= 3)
                    {
                        //上传文件按钮
                        btn上传文件.Enabled = authList.Exists(o => o == "profile:upload");
                        //全选
                        btn全选.Enabled = true;
                    }
                    else
                    {
                        //上传文件按钮
                        btn上传文件.Enabled = false;
                        //全选
                        btn全选.Enabled = false;
                    }
                }
                else
                {
                    //上传文件按钮
                    btn上传文件.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 是否显示复选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_ProjectFile_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            //隐藏节点前的checkbox
            var selectInfo = (ProjectFileTreeViewModel)e.Node.Tag;
            if (selectInfo.proType < 1)//隐藏文本名称为“数据集集合”的TreeView控件节点
                TreeViewHelper.HideCheckBox(treeView_ProjectFileTreeView, e.Node);
            e.DrawDefault = true;
        }

        /// <summary>
        /// 折叠节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_ProjectFile_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }

        /// <summary>
        /// 节点Check选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_ProjectFile_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)//节点操作判断是不是代码触发的事件
            {
                treeViewNodeMouseClick = true;//节点操作判断是不是代码触发的事件
                if (e.Node.Checked)
                {
                    var checkInfo = (ProjectFileTreeViewModel)e.Node.Tag;//获取节点数据
                    if (listCheckNodes != null && listCheckNodes.Any())//存在数据判断集合里有没有数据
                    {
                        var oldInfo = (ProjectFileTreeViewModel)listCheckNodes[0].Tag;//获取节点数据获取集合里第一个节点数据

                        if (checkInfo.parentId != oldInfo.parentId || checkInfo.proType != oldInfo.proType)//节点数据不一致 判断当前节点的上级id和类型和集合里第一个节点的上级id和类型是否相等
                        {
                            InitTreeViewCheckNodes();//清空集合不相等就初始化树结构勾选的数据
                        }
                    }

                    listCheckNodes.Add(e.Node);//添加节点数据到集合
                }
                else
                {
                    listCheckNodes.Remove(e.Node);//移除节点数据
                }
            }
        }

        /// <summary>
        /// 初始化树结构勾选的数据
        /// </summary>
        private void InitTreeViewCheckNodes()
        {
            foreach (var itemNode in listCheckNodes)
            {
                itemNode.Checked = false;
            }

            listCheckNodes = new List<TreeNode>();
        }

        #endregion

        /// <summary>
        /// 数据表设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                               e.RowBounds.Location.Y,
                                               dataGridView_objectFile.RowHeadersWidth - 4,
                                               e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView_objectFile.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView_objectFile.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 表格复选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                if (e.ColumnIndex == 0)
                {
                    var curValue = Convert.ToBoolean(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !curValue;
                }

                var list = ((BindingList<GetProjectFileListModel>)dataGridView.DataSource).ToList();
                if (list.Exists(o => o.isCheck))
                {
                    btn删除文件.Enabled = authList.Exists(o => o == "profile:del");
                    btn下载.Enabled = authList.Exists(o => o == "profile:down");
                    btn发起审批.Enabled = authList.Exists(o => o == "profile:apply");

                    if (list.Count == list.Count(o => o.isCheck))
                    {
                        btn全选.Text = "取消";
                    }
                    else
                    {
                        btn全选.Text = "全选";
                    }
                }
                else
                {
                    btn全选.Text = "全选";
                    btn删除文件.Enabled = false;
                    btn下载.Enabled = false;
                    btn发起审批.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 选中一行时右键出现菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView_objectFile.ClearSelection();
                    dataGridView_objectFile.Rows[e.RowIndex].Selected = true;
                    dataGridView_objectFile.CurrentCell = dataGridView_objectFile.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    LoadStripDataGridView();

                    stripDataGridView.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        /// <summary>
        /// 点鼠标两次的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                OpenFile(sender, e);
            }
        }

        /// <summary>
        /// 还原之前选择项的颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_object_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView_ProjectFileTreeView.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView_ProjectFileTreeView.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView_ProjectFileTreeView.SelectedNode.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 选中项目的颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_object_Leave(object sender, EventArgs e)
        {
            if (treeView_ProjectFileTreeView.SelectedNode != null)
            {
                //让选中项背景色呈现高亮
                treeView_ProjectFileTreeView.SelectedNode.BackColor = SystemColors.Highlight;
                //前景色为白色
                treeView_ProjectFileTreeView.SelectedNode.ForeColor = Color.White;
            }
        }

        /// <summary>
        /// 用于记录，鼠标是否已按下
        /// </summary>
        bool isMouseDown = false;

        /// <summary>
        /// 用于鼠标拖动多选，标记是否记录开始行
        /// </summary>
        bool isSetStartRow = false;

        /// <summary>
        /// 鼠标点击的操作；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
            }
        }

        /// <summary>
        /// 鼠标移动的操作；
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                if (!isSetStartRow)
                {
                    isSetStartRow = true;
                }
            }
        }

        /// <summary>
        /// 鼠标点击松开后的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown && isSetStartRow)
            {
                var selectRows = dataGridView_objectFile.SelectedRows;

                for (var i = 0; i < selectRows.Count; i++)
                {
                    dataGridView_objectFile.Rows[selectRows[i].Index].Cells[0].Value = true;
                }
                //加入点击空白区时不报错；
                if (((BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource) == null) { return; }
                var list = ((BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource).ToList();
                if (list.Count == list.Count(o => o.isCheck))
                {
                    btn全选.Text = "取消";
                }
                else
                {
                    btn全选.Text = "全选";
                }

                btn删除文件.Enabled = authList.Exists(o => o == "profile:del");
                btn下载.Enabled = authList.Exists(o => o == "profile:down");
                btn发起审批.Enabled = authList.Exists(o => o == "profile:apply");
            }

            isMouseDown = false;
            isSetStartRow = false;
        }


        #region 按键设置
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn上传文件_Click(object sender, EventArgs e)
        {
            // 1. 从当前选中的树节点读取 `ProjectFileTreeViewModel`（`projectInfo`）。
            var projectInfo = (ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag;
            // 2. 打开 `FrmUploadFile` 对话框进行文件上传，构造时传入 `projectInfo.id` 与 `projectInfo.proType`。
            var frm = new FrmUploadFile(projectInfo.id, projectInfo.proType);
            // 3. 如果用户在上传对话框中确认（`DialogResult.OK`），则：
            //    - 当 `projectInfo.proType == 2` 时，调用后端接口 `AppGlobalModel.GetProjectLevelDetails` 获取该节点下的子层级（`ProjectResultModel` 列表），
            //      清空当前树节点的子节点并用 `LoadProjectTreeView` 重新加载（刷新树结构）。  
            //    - 无论 `proType` 是否为 2，都会调用 `LoadProjectFileList()` 刷新右侧的项目文件列表。
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (projectInfo.proType == 2)
                {
                    var resultData = new List<ProjectResultModel>();
                    if (HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={projectInfo.id}", ref resultData))
                    {
                        treeView_ProjectFileTreeView.SelectedNode.Nodes.Clear();
                        foreach (var item in resultData)
                        {
                            LoadProjectTreeView(treeView_ProjectFileTreeView.SelectedNode, item);
                        }
                    }
                }
                LoadProjectFileList();
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn删除文件_Click(object sender, EventArgs e)
        {
            //拿到选中的文件序号；
            var index = dataGridView_objectFile.CurrentRow.Index;
            if (index > -1)
            {
                //获取到了文件列表List
                var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;
                //拿到了选中的序号文件，就是要删除的文件
                var selectModel = list[index];
                //拿到了要删除文件的ID；isCheck属性是代表这个文件有没有走流程（true，flase）；
                /*  具体解释这段代码的意思 
                 这行代码是使用 LINQ（Language Integrated Query）对一个集合进行筛选和映射操作，下面是代码的详细解释：

                    list：这是一个集合（可以是 List、Array 等），包含了一系列对象 o，每个对象具有属性 isCheck 和 id。

                    Where(o => o.isCheck || o.id == selectModel.id)：这部分是 LINQ 的筛选操作，使用 Where 方法根据条件筛选集合中的元素。在这里，条件为 o.isCheck || o.id == selectModel.id，表示要找出满足条件的对象。具体解释如下：

                    o.isCheck 表示对象 o 的 isCheck 属性为真。
                    o.id == selectModel.id 表示对象 o 的 id 属性等于 selectModel 对象的 id 属性。
                    Select(o => o.id)：这部分是 LINQ 的映射操作，使用 Select 方法将满足条件的对象映射为指定的属性值。在这里，将满足条件的对象的 id 属性提取出来。

                    ToList()：最后调用 ToList() 方法将 LINQ 查询结果转换为一个新的 List 对象，并返回该列表。

                    因此，这行代码的作用是从集合 list 中筛选出满足条件 o.isCheck || o.id == selectModel.id 的对象，并提取这些对象的 id 属性，最终返回一个包含符合条件的对象的 id 属性值的列表。
                 */
                var selectFile = list.Where(o => o.isCheck || o.id == selectModel.id).Select(o => o.id).ToList();

                if (ShowSuccessOKCancelMsg($"是否确定删除文件！") == DialogResult.OK)
                {
                    //加载等待窗口
                    //Splasher.Show 是一个方法，用于显示一个加载窗口或者 SplashScreen。在这里，typeof(FrmLoading) 表示获取 FrmLoading 类型的信息，即加载窗口的类型。
                    //整个语句的作用是显示一个加载窗口，通常用于在执行耗时操作时给用户一个等待提示，以提高用户体验。
                    //Splasher.Show(typeof(FrmLoading));
                    /*
                     var para = new { }：这表示创建一个匿名对象，其中 {} 中包含属性和对应的值。
                     projectFileId = string.Join(",", selectFile)：这是一个属性赋值操作，projectFileId 是对象的一个属性，值为将 selectFile 集合中的元素用逗号连接起来的字符串。这样做通常是将集合内容合并为一串字符形式的操作。
                     isDel = authList.Exists(o => o == "profile:del:all")：这也是一个属性赋值操作，isDel 是对象的另一个属性，值为一个布尔值，表示 authList 集合中是否存在满足条件 o == "profile:del:all" 的元素。Exists 方法用于判断集合中是否存在满足条件的元素，返回布尔值。
                     */
                    var para = new
                    {
                        //获取这个文件的项目文件Id
                        projectFileId = string.Join(",", selectFile),
                        //获取是不是管理员的删除权限；
                        isDel = authList.Exists(o => o == "profile:del:all")
                    };

                    var resultData = string.Empty;
                    //判断是不是可以删除；
                    if (HttpPost(AppGlobalModel.DelProjectFile, para, ref resultData))
                    {
                        dataGridView_objectFile.DataSource = null;
                        dataGridView_objectFile.DataSource = new SortableBindingList<GetProjectFileListModel>(list.Where(o => !selectFile.Contains(o.id)).ToList());

                        btn全选.Text = "全选";
                        btn删除文件.Enabled = false;
                        btn下载.Enabled = false;
                        btn发起审批.Enabled = false;
                        //关闭等待的画面窗口
                        Splasher.Close();
                    }
                    Splasher.Close();
                }
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn全选_Click(object sender, EventArgs e)
        {
            var list = ((BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource).ToList();
            if (list.Exists(o => !o.isCheck))
            {
                for (var i = 0; i < dataGridView_objectFile.Rows.Count; i++)
                {
                    dataGridView_objectFile.Rows[i].Cells[0].Value = true;
                }

                btn全选.Text = "取消";
                btn删除文件.Enabled = authList.Exists(o => o == "profile:del");
                btn下载.Enabled = authList.Exists(o => o == "profile:down");
                btn发起审批.Enabled = authList.Exists(o => o == "profile:apply");
            }
            else
            {

                for (var i = 0; i < dataGridView_objectFile.Rows.Count; i++)
                {
                    dataGridView_objectFile.Rows[i].Cells[0].Value = false;
                }

                btn全选.Text = "全选";
                btn删除文件.Enabled = false;
                btn下载.Enabled = false;
                btn发起审批.Enabled = false;
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn下载_Click(object sender, EventArgs e)
        {
            var list = ((BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource).ToList();

            var selectFile = list.Where(o => o.isCheck).Select(o => o.id).ToList();
            if (selectFile != null && selectFile.Any())
            {
                Splasher.Show(typeof(FrmLoading));

                var para = new { fileIds = string.Join(",", selectFile), type = 0 };
                var resultData = string.Empty;
                //调用JAVA下载接口
                if (HttpPost(AppGlobalModel.ProjectFileDownload, para, ref resultData))
                {
                    Splasher.Close();

                    var frm = new FrmDownloadFile(resultData);
                    frm.ShowDialog();
                }
                Splasher.Close();
            }
        }

        /// <summary>
        /// 项目归档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn项目归档_Click(object sender, EventArgs e)
        {
            var projectId = ((ProjectFileTreeViewModel)treeView_ProjectFileTreeView.SelectedNode.Tag).id;
            var frm = new FrmProjectArchive(projectId);
            frm.ShowDialog();
        }

        /// <summary>
        /// 发起审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn发起审批_Click(object sender, EventArgs e)
        {
            var index = dataGridView_objectFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;

                var selectModel = list[index];

                var selectFile = list.Where(o => o.isCheck || o.id == selectModel.id).Select(o => o.id).ToList();

                #region 获取项目属性信息
                var resultData = new GetProjectAttributeModel();
                if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={selectModel.projectId}", ref resultData))
                {
                    var frm = new FrmInitApproval(resultData, 3, string.Join(",", selectFile), 0, (selectFile.Count == 1 ? (string.IsNullOrWhiteSpace(selectModel.pageAll) ? 0 : Convert.ToInt32(selectModel.pageAll)) : 0));
                    frm.ShowDialog();
                }
                #endregion      
            }
        }

        /// <summary>
        /// 发起审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VisualSignature_Click(object sender, EventArgs e)
        {
            if (AppGlobalModel.qzSealList != null && AppGlobalModel.qzSealList.Any())
            {
                var index = dataGridView_objectFile.CurrentRow.Index;
                if (index > -1)
                {
                    var list = (BindingList<GetProjectFileListModel>)dataGridView_objectFile.DataSource;
                    var selectModel = list[index];
                    var frm = new FrmVisualSignature(selectModel);
                    frm.ShowDialog();
                }
            }
            else
            {
                ShowErrorMsg("没有签名签章，请联系管理员配置！");
            }
        }

        /// <summary>
        /// 选定文件清单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn文件清单_Click(object sender, EventArgs e)
        {
            var frm = new FrmFileShoppingCart();
            frm.ShowDialog();
        }

        #endregion
        /// <summary>
        /// 项目搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_ProjectSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string keyword = comboBox_ProjectSearch.Text.Trim();

                if (!string.IsNullOrEmpty(keyword) && !comboBox_ProjectSearch.Items.Contains(keyword))
                {
                    comboBox_ProjectSearch.Items.Add(keyword);
                }

                btn_searchProject.PerformClick();
            }
        }

        /// <summary>
        /// 项目搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_searchProject_Click(object sender, EventArgs e)
        {
            ///拿到关键字
            string keyword = comboBox_ProjectSearch.Text.Trim();
            /// 如果关键字不为空，则进行搜索  
            if (!string.IsNullOrEmpty(keyword))
            {
                /// 将关键字保存到ComboBox的列表项中，避免重复保存  
                if (!comboBox_ProjectSearch.Items.Contains(keyword))
                {
                    comboBox_ProjectSearch.Items.Add(keyword);
                }
                /// 获取所有项目及其父节点列表  
                List<TreeNode> allTreeNodes = new List<TreeNode>();
                GetAllTreeNodesRecursive(treeView_ProjectFileTreeView.Nodes, allTreeNodes);

                /// 过滤出与关键字匹配的项目节点  
                var filteredNodes = allTreeNodes.Where(node => ((ProjectFileTreeViewModel)node.Tag).name.Contains(keyword)).ToList();



                /// 清理之前的搜索结果  
                ClearPreviousSearchResults(treeView_ProjectFileTreeView.Nodes);

                /// 定位并展开显示这些搜索结果  
                LocateSearchResults(filteredNodes);
            }


        }

        /// <summary>
        /// 递归遍历所有树节点的方法  
        /// </summary>
        /// <param name="nodes">组织架构树节点</param>
        /// <param name="allNodes">所有节点</param>
        private void GetAllTreeNodesRecursive(TreeNodeCollection nodes, List<TreeNode> allNodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.EnsureVisible();
                /// 添加当前节点到节点列表  
                allNodes.Add(node);
                /// 递归查找子节点  
                GetAllTreeNodesRecursive(node.Nodes, allNodes);
            }
        }

        /// <summary>
        /// 清理之前的搜索结果，将颜色恢复为默认  
        /// </summary>
        /// <param name="nodes"></param>
        private void ClearPreviousSearchResults(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                // 将之前的搜索结果颜色恢复为黑色  
                if (node.ForeColor == Color.Blue)
                {
                    node.ForeColor = Color.Black;
                    node.BackColor = Color.White;
                }

                // 递归清理子节点  
                ClearPreviousSearchResults(node.Nodes);
            }
        }

        /// <summary>
        /// 定位并展开显示搜索结果  
        /// </summary>
        /// <param name="searchResults"></param>
        private void LocateSearchResults(List<TreeNode> searchResults)
        {
            foreach (var node in searchResults)
            {
                // 定位到匹配节点，并展开父节点  
                TreeNode currentNode = node;

                while (currentNode != null)
                {

                    currentNode.Expand();
                    currentNode = currentNode.Parent;
                }

                // 高亮显示匹配节点  
                node.EnsureVisible();
                node.ForeColor = Color.Blue;
                node.BackColor = Color.LightGray;
            }
        }

        /// <summary>
        /// 删除补充文件（静态方法）
        /// </summary>
        /// <param name="fileId">要删除的文件ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteSupplementaryFile(string fileId)
        {
            try
            {
                var para = new { id = fileId }; // 根据您的API参数要求调整
                var resultData = new object(); // 根据您的API返回类型调整

                // 调用删除接口
                bool result = HttpPost(AppGlobalModel.DelProjectFile, para, ref resultData);

                if (result)
                {
                    Console.WriteLine($"成功删除文件: {fileId}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"删除文件失败: {fileId}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除文件时发生错误: {ex.Message}");
                return false;
            }
        }
    }
}