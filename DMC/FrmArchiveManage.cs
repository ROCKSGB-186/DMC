using DMC.Helper;
using DMC.Models;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Documents;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 归档管理
    /// </summary>
    public partial class FrmArchiveManage : BaseForm
    {
        //LoadKeepDept   SortTreeViewNodes  TreeNodeComparer

        /// <summary>
        /// 树形结构上面的右键菜单
        /// </summary>
        private ContextMenuStrip stripTreeView = new ContextMenuStrip();
        /// <summary>
        /// 项目文件列表右键菜单
        /// </summary>
        private ContextMenuStrip stripDataGridView = new ContextMenuStrip();
        /// <summary>
        /// 总条数
        /// </summary>
        private int total = 0;
        /// <summary>
        /// 查询档案项目文件
        /// </summary>
        private QueryKeepProjectFile queryInfo = null;
        /// <summary>
        /// 树状架构节点点击事件
        /// </summary>
        private bool treeViewNodeMouseClick = true;

        //private List<string> authList = new List<string>();
        /// <summary>
        /// 节点事件
        /// </summary>
        private List<TreeNode> listCheckNodes = new List<TreeNode>();

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
        /// 窗体加载
        /// </summary>
        public FrmArchiveManage()
        {
            InitializeComponent();
            //列宽自适应
            dataGridView_archiveFile.DoubleBufferedDataGirdView(true);
            //列不生成
            dataGridView_archiveFile.AutoGenerateColumns = false;
            //滚动条滚动到底部事件
            dataGridView_archiveFile.RegistScrollToEndEvent(dataGrid_OnScrollToEnd);
            //查询信息
            queryInfo = new QueryKeepProjectFile()
            {
                pageNum = 1,
                pageSize = 100
            };
        }
        /// <summary>
        /// 滚动条滚动到底部事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_OnScrollToEnd(object sender, EventArgs e)
        {
            //判断项目文件总数是不是与表内的行数相同
            if (total != dataGridView_archiveFile.Rows.Count)
            {
                //加载下一页
                queryInfo.pageNum = queryInfo.pageNum + 1;
                //加载下一页列表
                LoadList();
            }
        }
        /// <summary>
        /// 加载项目文件的load事件，左侧架构树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProjectFile_Load(object sender, EventArgs e)
        {
            //搜索按钮
            btn搜索.Visible = AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:search");
            // 建立档案组织架构变量
            var keepDeptData = new List<SelectKeepDeptModel>();
            //获取组织架构数据加载归档目录层级
            if (HttpGet(AppGlobalModel.SelectKeepDept + "?parentId=0", ref keepDeptData))
            {
                foreach (var keepDeptItem in keepDeptData)
                {
                    TreeNode treeNode = new TreeNode();
                    //根目录名称
                    treeNode.Text = keepDeptItem.name;//根目录名称
                    treeNode.Tag = keepDeptItem;//根目录名称
                    treeView_Archive.Nodes.Add(treeNode);//添加根节点
                }
                btn上传文件.Enabled = false;
            }
            else
            {
                this.Close();
            }
            // 对所有子节点进行排序  
            SortTreeViewNodes(treeView_Archive.Nodes);
            treeView_Archive.ExpandAll();
        }

        /// <summary>
        /// 项目排序
        /// </summary>
        /// <param name="nodes"></param>
        public static void SortTreeViewNodes(TreeNodeCollection nodes)
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
                return string.Compare(x.Text, y.Text);
            }
        }

        /// <summary>
        /// 加载右侧文件列表
        /// </summary>
        private void LoadList()
        {
            //数据源初始化为null
            dataGridView_archiveFile.DataSource = null;
            //建立档案区查询返回数据变量
            var keepProjectResultData = new List<GetKeepProjectDirModel>();
            //获取档案区项目数据
            if (HttpGet(AppGlobalModel.GetKeepProjectFile +
                $"?parentId={queryInfo.parentId}" +
                $"&pageNum={queryInfo.pageNum}&pageSize={queryInfo.pageSize}",
                ref keepProjectResultData, ref total))
            {
                //项目文件页码大于1时，将数据添加到datagridview中
                if (queryInfo.pageNum != 1)
                {
                    //建立档案区项目变量
                    var list = ((BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource).ToList();
                    //添加项目文件
                    list.AddRange(keepProjectResultData);
                    //绑定数据源
                    dataGridView_archiveFile.DataSource = new SortableBindingList<GetKeepProjectDirModel>(list);
                }
                else
                {
                    //排序后绑定获取到的项目文件到datagridviewl中,
                    dataGridView_archiveFile.DataSource = new SortableBindingList<GetKeepProjectDirModel>(keepProjectResultData.OrderBy(o => o.name, new StringRankComparer()).ToList());
                }
                //清空选择
                dataGridView_archiveFile.ClearSelection();

                var dataList = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;

                label1.Text = $"文件数量：{dataList.Count()}";
                label2.Text = $"总A1数量：{dataList.Where(o => !string.IsNullOrWhiteSpace(o.folded)).Sum(o => Convert.ToDecimal(o.folded))}   A1";

                if (dataList.Count > 0)
                {
                    btn全选.Enabled = true;
                }
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

                    if (parentId == "0")
                    {
                        treeView_Archive.Nodes.Add(root);
                    }
                    else
                    {
                        treeView_Archive.SelectedNode.Nodes.Add(root);
                    }
                }

                if (parentId == "0")
                {
                    btn上传文件.Enabled = false;

                    treeView_Archive.ExpandAll();
                }
                else
                {
                    treeView_Archive.SelectedNode.Expand();
                }
            }
        }

        /// <summary>
        /// 加载归档项目层级
        /// </summary>
        private void LoadKeepProjectDir(string parentId)
        {
            var resultData = new List<GetKeepProjectDirModel>();
            if (HttpGet(AppGlobalModel.GetKeepProjectDir + "?parentId=" + parentId, ref resultData))
            {
                foreach (var item in resultData)
                {
                    TreeNode root = new TreeNode();

                    if (item.name.Contains(item.identifier))
                    {
                        root.Text = item.name;
                    }
                    else
                    {
                        //根目录名称
                        root.Text = item.identifier + "-" + item.name;//项目号+项目名称组成在树节点名称
                    }                    
                    //root.Text = item.type == 0 ? item.identifier + "-" + item.name : item.name; ;
                    root.Tag = item;
                    treeView_Archive.SelectedNode.Nodes.Add(root);
                }
                treeView_Archive.SelectedNode.Expand();
            }
        }
        private bool copyProjectStatics = false;

        #region 树结构菜单
        /// <summary>
        /// 树结构右键菜单
        /// </summary>
        private void LoadStripTreeView()
        {
            stripTreeView.Items.Clear();

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:add"))
            {
                ToolStripItem ts_newAdd = new ToolStripMenuItem("新增");
                ts_newAdd.Click += new EventHandler(addDir);
                stripTreeView.Items.Add(ts_newAdd);
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:edit"))
            {
                ToolStripItem ts_changeName = new ToolStripMenuItem("修改");
                ts_changeName.Click += new EventHandler(changeDir);
                stripTreeView.Items.Add(ts_changeName);
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:del"))
            {
                ToolStripItem ts_delete = new ToolStripMenuItem("删除");
                ts_delete.Click += new EventHandler(delDir);
                stripTreeView.Items.Add(ts_delete);
            }
            if (copyProjectStatics)
            {
                if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:del"))
                {
                    ToolStripItem ts_delete = new ToolStripMenuItem("剪切项目");
                    ts_delete.Click += new EventHandler(copyProject);
                    stripTreeView.Items.Add(ts_delete);

                }
                if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:del"))
                {
                    if (keepProjectDirModel != null)
                    {
                        ToolStripItem ts_delete = new ToolStripMenuItem("粘贴项目");
                        ts_delete.Click += new EventHandler(moveProject);
                        stripTreeView.Items.Add(ts_delete);
                    }
                }
            }
            else
            {
                if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:del"))
                {
                    ToolStripItem ts_delete = new ToolStripMenuItem("剪切项目");
                    ts_delete.Click += new EventHandler(copyProject);
                    stripTreeView.Items.Add(ts_delete);
                }

            }


            if (stripTreeView.Items.Count > 0)
            {
                stripTreeView.Items.Add("-");
            }//分割线

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "profile:cart:add"))
            {
                ToolStripItem ts_selectFolderToList = new ToolStripMenuItem("选定文件加入列表");
                ts_selectFolderToList.Click += new EventHandler(selectFolderToList);
                stripTreeView.Items.Add(ts_selectFolderToList);
            }

            if (stripTreeView.Items.Count > 0)
            {
                stripTreeView.Items.Add("-");
            }//分割线
            // 项目导出
            //if (treeView1.SelectedNode.Level == 0 && AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:export:i"))
            //{
            //    //只有项目有这个菜单
            //    ToolStripItem ts_exportProjectAll = new ToolStripMenuItem("导出项目总目录");
            //    ts_exportProjectAll.Click += new EventHandler(exportProjectAll);
            //    stripTreeView.Items.Add(ts_exportProjectAll);

            //    if (stripTreeView.Items.Count > 0)
            //    {
            //        stripTreeView.Items.Add("-");
            //    }

            //    #region 新加重新选择归档路径
            //    if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:del"))
            //    {
            //        ToolStripItem ts_delete = new ToolStripMenuItem("重新选择归档路径");
            //        var frm = new FrmSelectKeepDir();
            //        if (frm.ShowDialog() == DialogResult.OK)
            //        {
            //            this.Close();
            //        }
            //        //ts_delete.Click += new EventHandler(delDir);
            //        //stripTreeView.Items.Add(ts_delete);
            //        //ts_changeName.Click += new EventHandler(changeDir);
            //        //stripTreeView.Items.Add(ts_changeName);
            //    }
            //    #endregion

            //}
            if (treeView_Archive.SelectedNode.Level == 1)
            {
                //只有项目有这个菜单
                ToolStripItem ts_exportProjectAll = new ToolStripMenuItem("导出项目总目录");
                ts_exportProjectAll.Click += new EventHandler(exportProjectAll);
                stripTreeView.Items.Add(ts_exportProjectAll);

                if (stripTreeView.Items.Count > 0)
                {
                    stripTreeView.Items.Add("-");
                }

                #region 新加重新选择归档路径
                //if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:folder:del"))
                //{
                //    ToolStripItem ts_delete = new ToolStripMenuItem("重新选择归档路径");
                //    var frm = new FrmSelectKeepDir();
                //    if (frm.ShowDialog() == DialogResult.OK)
                //    {
                //        this.Close();
                //    }                
                //}
                #endregion

            }
            // 项目导出
            if (treeView_Archive.SelectedNode.Level == 1 && AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:export:i"))
            {
                //只有项目有这个菜单
                ToolStripItem ts_exportClassifyProject = new ToolStripMenuItem("按部门分类导出归档项目");
                //按部门分类导出归档项目
                ts_exportClassifyProject.Click += new EventHandler(exportClassifyProject);
                //添加菜单
                stripTreeView.Items.Add(ts_exportClassifyProject);

                if (stripTreeView.Items.Count > 0)
                {
                    stripTreeView.Items.Add("-");
                }
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:upload"))
            {
                ToolStripItem ts_uploadFile = new ToolStripMenuItem("上传文件");
                ts_uploadFile.Click += new EventHandler(btn上传文件_Click);
                stripTreeView.Items.Add(ts_uploadFile);
            }

            var selectObject = treeView_Archive.SelectedNode.Tag;
            if (selectObject is GetKeepProjectDirModel)
            {
                if (stripTreeView.Items.Count > 0)
                {
                    stripTreeView.Items.Add("-");
                }

                if (((GetKeepProjectDirModel)selectObject).type == 0)
                {
                    if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:compilation:i"))
                    {
                        //只有项目有这个菜单
                        ToolStripItem ts_addOrEditCompilation = new ToolStripMenuItem("项目档案编研");
                        ts_addOrEditCompilation.Click += new EventHandler(addOrEditCompilation);
                        stripTreeView.Items.Add(ts_addOrEditCompilation);

                        if (stripTreeView.Items.Count > 0)
                        {
                            stripTreeView.Items.Add("-");
                        }
                    }

                    if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:export:i"))
                    {
                        //只有项目有这个菜单
                        ToolStripItem ts_exportProjectMaterial = new ToolStripMenuItem("导出项目技术资料");
                        ts_exportProjectMaterial.Click += new EventHandler(exportProjectMaterial);
                        stripTreeView.Items.Add(ts_exportProjectMaterial);

                        //只有项目有这个菜单
                        ToolStripItem ts_exportProjectSubItem = new ToolStripMenuItem("导出项目子项");
                        ts_exportProjectSubItem.Click += new EventHandler(exportProjectSubItem);
                        stripTreeView.Items.Add(ts_exportProjectSubItem);

                        //只有项目有这个菜单
                        ToolStripItem ts_exportProjectSpine = new ToolStripMenuItem("导出项目书脊");
                        ts_exportProjectSpine.Click += new EventHandler(exportProjectSpine);
                        stripTreeView.Items.Add(ts_exportProjectSpine);

                        if (stripTreeView.Items.Count > 0)
                        {
                            stripTreeView.Items.Add("-");
                        }
                    }
                }

                if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:apply"))
                {
                    ToolStripItem ts_allFileApproval = new ToolStripMenuItem("全文件-发起审批");
                    ts_allFileApproval.Click += new EventHandler(allFileApproval);
                    stripTreeView.Items.Add(ts_allFileApproval);
                }
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addDir(object sender, EventArgs e)
        {
            var selectObject = treeView_Archive.SelectedNode.Tag;
            string parentId;
            if (selectObject is SelectKeepDeptModel)
            {
                var deptInfo = (SelectKeepDeptModel)selectObject;
                parentId = deptInfo.id;
            }
            else
            {
                var deptInfo = (GetKeepProjectDirModel)selectObject;
                parentId = deptInfo.id;
            }

            var frm = new FrmAddChildOrFolder(parentId, 10);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                treeView_Archive.SelectedNode.Nodes.Clear();
                if (selectObject is SelectKeepDeptModel)
                {
                    LoadKeepDept(parentId);
                }

                LoadKeepProjectDir(parentId);
            }
        }

        #region 新加移动项目

        private GetKeepProjectDirModel keepProjectDirModel = new GetKeepProjectDirModel();
        private SelectKeepDeptModel selectKeepDeptModel = new SelectKeepDeptModel();

        /// <summary>
        /// 复制项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyProject(object sender, EventArgs e)
        {
            keepProjectDirModel = null;
            selectKeepDeptModel = null;
            //获取右键选的节点标记；

            var treeViewSelectNode = treeView_Archive.SelectedNode.Tag;

            if (treeViewSelectNode is SelectKeepDeptModel)
            {
                selectKeepDeptModel = (SelectKeepDeptModel)treeViewSelectNode;

                if (selectKeepDeptModel.identifier == null)
                {
                    selectKeepDeptModel = null;
                    copyProjectStatics = false;
                    MessageBox.Show("选中的不是项目，不能复制！");
                    return;
                }
            }
            else if (treeViewSelectNode is GetKeepProjectDirModel)
            {
                keepProjectDirModel = (GetKeepProjectDirModel)treeViewSelectNode;
                copyProjectStatics = true;
                MessageBox.Show("项目复制完成！");
                if (keepProjectDirModel.identifier == "")
                {
                    keepProjectDirModel = null;
                    copyProjectStatics = false;
                    MessageBox.Show("选中的不是项目，不能复制！");
                    return;
                }
            }
        }
        /// <summary>
        /// 粘贴移动项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void moveProject(object sender, EventArgs e)
        {
            //拿到选定的文件夹信息
            var treeViewSelectNode = treeView_Archive.SelectedNode.Tag;
            //判断选定的文件夹是不是 1：Id/ 2:parentId 父id/ 3:name 名称/ 4：createTime 创建时间/ 5：ancestors值/ 6：status 状态/ 7：identifier 标识符
            if (treeViewSelectNode is SelectKeepDeptModel)
            {
                //拿到鼠标选定的架构树
                var moveSelectKeepDeptModel = (SelectKeepDeptModel)treeViewSelectNode;
                //拿到mysql内的档案架构
                var mysql_KeepDept = SQLiteDataBase.GetDataFromMysql("qz_keep_dept", "status", "0");
                //一个判断是不是所有行的变量
                int itemNum = 0;
                foreach (DataRow keepDeptItem in mysql_KeepDept.Rows)
                {
                    //变量自加
                    itemNum++;
                    //判断选定架构树是不是档案架构内的
                    if (keepDeptItem["id"].ToString() == moveSelectKeepDeptModel.id)
                    {
                        SQLiteDataBase.UpdateDataToMysql("qz_keep_project", "id", $"{keepProjectDirModel.id}", "parent_id", $"{moveSelectKeepDeptModel.id}");

                        if (MessageBox.Show("项目移动完成！") == DialogResult.OK)
                        {
                            treeView_Archive.SelectedNode.Nodes.Clear();

                            //刷新当前结构树
                            LoadKeepProjectDir(moveSelectKeepDeptModel.id);

                        }
                        keepProjectDirModel = null;
                        copyProjectStatics = false;
                        // 对所有子节点进行排序  
                        SortTreeViewNodes(treeView_Archive.SelectedNode.Nodes);
                        break;
                    }
                }
                if (itemNum == mysql_KeepDept.Rows.Count)
                {
                    MessageBox.Show("选中的不是组织架构，不能粘贴项目！");
                }
            }
            else if (treeViewSelectNode is GetKeepProjectDirModel)
            {
                //拿到鼠标选定的架构树
                var moveSelectKeepDeptModel = (GetKeepProjectDirModel)treeViewSelectNode;
                //拿到mysql内的档案架构
                var mysql_KeepDept = SQLiteDataBase.GetDataFromMysql("qz_keep_dept", "status", "0");
                //一个判断是不是所有行的变量
                int itemNum = 0;
                foreach (DataRow keepDeptItem in mysql_KeepDept.Rows)
                {
                    //变量自加
                    itemNum++;
                    //判断选定架构树是不是档案架构内的
                    if (keepDeptItem["id"].ToString() == moveSelectKeepDeptModel.id)
                    {
                        SQLiteDataBase.UpdateDataToMysql("qz_keep_project", "id", $"{keepProjectDirModel.id}", "parent_id", $"{moveSelectKeepDeptModel.id}");
                        keepProjectDirModel = null;
                        copyProjectStatics = false;
                        MessageBox.Show("项目移动完成！");
                        break;
                    }
                }
                if (itemNum == mysql_KeepDept.Rows.Count)
                {
                    MessageBox.Show("选中的不是组织架构，不能粘贴项目！");
                }
            }
        }
        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private List<ProjectResultModel> copyProjectList = null;
        private void moveDir(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));

            var projectInfo = (ProjectFileTreeViewModel)treeView_Archive.SelectedNode.Tag;

            copyProjectList = new List<ProjectResultModel>();
            GetProjectLevel(projectInfo.id);

            var projectParentInfo = (ProjectFileTreeViewModel)(treeView_Archive.SelectedNode.Parent.Tag);
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


        #endregion
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeDir(object sender, EventArgs e)
        {
            var selectObject = treeView_Archive.SelectedNode.Tag;
            var editKeepProjectName = new EditKeepProjectNameModel();
            if (selectObject is SelectKeepDeptModel)
            {
                var deptInfo = (SelectKeepDeptModel)selectObject;
                editKeepProjectName.id = deptInfo.id;
                editKeepProjectName.parentId = deptInfo.parentId;
                editKeepProjectName.newName = deptInfo.name;
                editKeepProjectName.type = "0";
            }
            else
            {
                var deptInfo = (GetKeepProjectDirModel)selectObject;
                editKeepProjectName.id = deptInfo.id;
                editKeepProjectName.parentId = deptInfo.parentId;
                editKeepProjectName.newName = deptInfo.name;
                editKeepProjectName.type = "1";
            }

            var frm = new FrmKeepRename(editKeepProjectName);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (editKeepProjectName.parentId == "0")
                {
                    treeView_Archive.Nodes.Clear();
                }
                else
                {
                    treeView_Archive.SelectedNode.Parent.Nodes.Clear();
                }

                if (selectObject is SelectKeepDeptModel)
                {
                    LoadKeepDept(editKeepProjectName.parentId);
                }

                if (editKeepProjectName.parentId != "0")
                {
                    LoadKeepProjectDir(editKeepProjectName.parentId);
                }
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delDir(object sender, EventArgs e)
        {
            if (ShowSuccessOKCancelMsg("是否确定删除！") == DialogResult.OK)
            {
                var selectObject = treeView_Archive.SelectedNode.Tag;
                if (selectObject is SelectKeepDeptModel)
                {
                    var deptInfo = (SelectKeepDeptModel)selectObject;
                    var param = new
                    {
                        id = deptInfo.id
                    };

                    var resultData = string.Empty;
                    if (HttpPost(AppGlobalModel.DelKeepDept, param, ref resultData))
                    {
                        treeView_Archive.SelectedNode.Remove();
                    }
                }
                else
                {
                    var deptInfo = (GetKeepProjectDirModel)selectObject;
                    var resultData = string.Empty;
                    if (HttpGet(AppGlobalModel.DelKeepProject + $"?id={deptInfo.id}", ref resultData))
                    {
                        treeView_Archive.SelectedNode.Remove();
                    }
                }
            }
        }

        /// <summary>
        /// 导出项目总目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportProjectAll(object sender, EventArgs e)
        {
            var selectObject = (SelectKeepDeptModel)treeView_Archive.SelectedNode.Tag;
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.ExportProjectAll + $"?deptId={selectObject.id}", ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 按部门分类导出归档项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportClassifyProject(object sender, EventArgs e)
        {
            var selectObject = (SelectKeepDeptModel)treeView_Archive.SelectedNode.Tag;
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.ExportClassifyProject + $"?deptId={selectObject.id}", ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 左侧架构树中的文件夹加入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectFolderToList(object sender, EventArgs e)
        {
            var projectInfo = (GetKeepProjectDirModel)treeView_Archive.SelectedNode.Tag;

            string folderIds;
            if (listCheckNodes != null && listCheckNodes.Any())
            {
                folderIds = string.Join(",", listCheckNodes.Select(o => ((GetKeepProjectDirModel)o.Tag).id));
            }
            else
            {
                folderIds = projectInfo.id;
            }
            var para = new
            {
                proId = projectInfo.type == 0 ? projectInfo.id : projectInfo.projectId,//项目id(必填 所属项目)
                fileType = "1",//文件类型0项目区 1归档区
                fileIds = "",//文件id 逗号分割
                folderIds = folderIds,//文件夹id 逗号分割
                proIds = projectInfo.id //项目id(如果添加项目)

            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.AddFileCart, para, ref resultData))
            {
                ShowSuccessMsg("添加成功！");
            }
        }

        /// <summary>
        /// 导出项目技术资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportProjectMaterial(object sender, EventArgs e)
        {
            var selectObject = (GetKeepProjectDirModel)treeView_Archive.SelectedNode.Tag;
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.ExportProjectMaterial + $"?projectId={selectObject.id}", ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 导出项目子项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportProjectSubItem(object sender, EventArgs e)
        {
            var selectObject = (GetKeepProjectDirModel)treeView_Archive.SelectedNode.Tag;
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.ExportProjectSubItem + $"?projectId={selectObject.id}", ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 导出项目书脊
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportProjectSpine(object sender, EventArgs e)
        {
            var selectObject = (GetKeepProjectDirModel)treeView_Archive.SelectedNode.Tag;
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.ExportProjectSpine + $"?projectId={selectObject.id}", ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 项目档案编研
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addOrEditCompilation(object sender, EventArgs e)
        {
            var selectObject = (GetKeepProjectDirModel)treeView_Archive.SelectedNode.Tag;
            var frm = new FrmAddOrEditCompilation(selectObject.id);
            frm.ShowDialog();
        }

        /// <summary>
        /// 全文件发起审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allFileApproval(object sender, EventArgs e)
        {
            var projectInfo = (GetKeepProjectDirModel)treeView_Archive.SelectedNode.Tag;
            string folderIds = projectInfo.id;

            #region 获取项目属性信息
            var resultData = new GetProjectAttributeModel();
            if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={(projectInfo.type == 0 ? projectInfo.id : projectInfo.projectId)}", ref resultData))
            {
                var frm = new FrmInitApproval(resultData, (projectInfo.type == 0 ? 2 : 1), folderIds, 1);
                frm.ShowDialog();
            }
            #endregion
        }
        #endregion

        #region 右侧项目文件区 文件列表菜单
        /// <summary>
        /// 右侧项目文件区列表右键菜单
        /// </summary>
        private void LoadStripDataGridView()
        {
            stripDataGridView.Items.Clear();

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:open"))
            {
                ToolStripItem ts_open = new ToolStripMenuItem("打开");
                ts_open.Click += new EventHandler(openFile);
                stripDataGridView.Items.Add(ts_open);
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:rename"))
            {
                ToolStripItem ts_changeName = new ToolStripMenuItem("重命名");
                ts_changeName.Click += new EventHandler(changeName);
                stripDataGridView.Items.Add(ts_changeName);
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:del"))
            {
                ToolStripItem ts_delete = new ToolStripMenuItem("删除");
                ts_delete.Click += new EventHandler(btn删除文件_Click);
                stripDataGridView.Items.Add(ts_delete);
            }

            if (stripDataGridView.Items.Count > 0)
            {
                stripDataGridView.Items.Add("-");
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:version"))
            {
                ToolStripItem ts_viewVersion = new ToolStripMenuItem("查看版本");
                ts_viewVersion.Click += new EventHandler(viewVersion);
                stripDataGridView.Items.Add(ts_viewVersion);
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:replace"))
            {
                ToolStripItem ts_replaceFile = new ToolStripMenuItem("替换文件");
                ts_replaceFile.Click += new EventHandler(replaceFile);
                stripDataGridView.Items.Add(ts_replaceFile);
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:upload"))
            {
                ToolStripItem ts_uploadFile = new ToolStripMenuItem("补充上传归档文件");
                ts_uploadFile.Click += new EventHandler(btn上传文件_Click);
                stripDataGridView.Items.Add(ts_uploadFile);
            }

            if (stripDataGridView.Items.Count > 0)
            {
                stripDataGridView.Items.Add("-");
            }

            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "profile:cart:add"))
            {
                ToolStripItem ts_selectFileToList = new ToolStripMenuItem("选定文件加入列表");
                ts_selectFileToList.Click += new EventHandler(selectFileToList);
                stripDataGridView.Items.Add(ts_selectFileToList);
            }


            if (AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:apply"))
            {
                ToolStripItem ts_allFileApproval = new ToolStripMenuItem("选定文件-发起审批");
                ts_allFileApproval.Click += new EventHandler(btn发起审批_Click);
                stripDataGridView.Items.Add(ts_allFileApproval);
            }
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFile(object sender, EventArgs e)
        {
            var index = dataGridView_archiveFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;
                var selectModel = list[index];
                var listUrl = list.Select(o => new PreviewAreaViewModel { filePath = o.filePath, name = o.name }).ToList();
                var frm = new FrmPreviewArea(selectModel.filePath, 1, listUrl);
                frm.Show();
            }
        }

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void changeName(object sender, EventArgs e)
        {
            var index = dataGridView_archiveFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;
                var selectModel = list[index];
                var editKeepProjectName = new EditKeepProjectNameModel();
                editKeepProjectName.id = selectModel.id;
                editKeepProjectName.parentId = selectModel.parentId;
                editKeepProjectName.newName = selectModel.name;
                editKeepProjectName.type = "2";
                var frm = new FrmKeepRename(editKeepProjectName);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadList();
                }
            }
        }

        /// <summary>
        /// 查看版本
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewVersion(object sender, EventArgs e)
        {
            var index = dataGridView_archiveFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;
                var selectModel = list[index];
                var frm = new FrmKeepSeeHistoryFile(selectModel.id);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void replaceFile(object sender, EventArgs e)
        {
            var index = dataGridView_archiveFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;
                var selectModel = list[index];

                var resultData = new List<JiSuanFrameModel>();
                if (HttpGet(AppGlobalModel.JiSuanFrame, ref resultData))
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

                        var resultUpdateData = string.Empty;
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
                                jiSuanFrame = resultData.FirstOrDefault(o => (o.minW <= pdfPage.Width && o.maxW >= pdfPage.Width) && (o.minH <= pdfPage.Height && o.maxH >= pdfPage.Height));

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
                        if (HttpUploadFile(AppGlobalModel.KeepAgainFileUpload, openFileDialog.FileName, ref resultUpdateData, paras))
                        {
                            LoadList();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 在右侧文件列表中选定文件 加入列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectFileToList(object sender, EventArgs e)
        {
            var index = dataGridView_archiveFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;

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
        #endregion



        /// <summary>
        /// 左侧树节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView_Archive.SelectedNode = e.Node;
            //让选中项背景色呈现红色
            treeView_Archive.SelectedNode.BackColor = SystemColors.Highlight;
            //前景色为白色
            treeView_Archive.SelectedNode.ForeColor = Color.White;
            //按钮
            btn上传文件.Enabled = AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:upload");
            //菜单
            var selectObject = e.Node.Tag;
            //如果点键的是右键
            if (e.Button == MouseButtons.Right)
            {
                //加载右键菜单
                LoadStripTreeView();
                //绑定菜单
                e.Node.ContextMenuStrip = stripTreeView;
            }

            //右侧列表
            dataGridView_archiveFile.DataSource = null;
            //清空选中行
            dataGridView_archiveFile.ClearSelection();
            label1.Text = $"文件数量：0";
            label2.Text = $"总A1数量：0   A1";

            queryInfo.pageNum = 1;
            //如果点击的是左键
            if (e.Button == MouseButtons.Left && treeViewNodeMouseClick)
            {
                e.Node.Nodes.Clear();
                //LoadKeepProjectDir(queryInfo.parentId);
            }
            //如果点击的是部门
            if (selectObject is SelectKeepDeptModel)
            {
                //部门
                var deptInfo = (SelectKeepDeptModel)selectObject;
                //如果点击的是部门
                if (e.Button == MouseButtons.Left && treeViewNodeMouseClick)
                {
                    //加载档案部门
                    LoadKeepDept(deptInfo.id);
                }
                //查询部门
                queryInfo.parentId = deptInfo.id;
            }
            else
            {
                //项目
                var deptInfo = (GetKeepProjectDirModel)selectObject;
                //如果点击的是项目
                queryInfo.parentId = deptInfo.id;
            }
            //如果点击的是项目
            if (e.Button == MouseButtons.Left && treeViewNodeMouseClick)
            {
                //加载项目
                LoadKeepProjectDir(queryInfo.parentId);
            }
            //加载列表
            LoadList();
            if (e.Node.Nodes.Count != 0)
            {
                // 对所有子节点进行排序  
                SortTreeViewNodes(e.Node.Nodes);
            }
            treeViewNodeMouseClick = true;
        }

        /// <summary>
        /// 折叠节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }

        /// <summary>
        /// 绘制行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_archiveFile_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                               e.RowBounds.Location.Y,
                                               dataGridView_archiveFile.RowHeadersWidth - 4,
                                               e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView_archiveFile.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView_archiveFile.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 表格复选框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_archiveFile_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                if (e.ColumnIndex == 0)
                {
                    var curValue = Convert.ToBoolean(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !curValue;
                }

                var list = ((BindingList<GetKeepProjectDirModel>)dataGridView.DataSource).ToList();
                if (list.Exists(o => o.isCheck))
                {
                    btn删除文件.Enabled = AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:del");
                    btn发起审批.Enabled = AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:apply");

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
                    btn发起审批.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 选中一行时右键出现菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_archiveFile_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    dataGridView_archiveFile.ClearSelection();
                    dataGridView_archiveFile.Rows[e.RowIndex].Selected = true;
                    dataGridView_archiveFile.CurrentCell = dataGridView_archiveFile.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    LoadStripDataGridView();

                    stripDataGridView.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn上传文件_Click(object sender, EventArgs e)
        {
            var selectObject = treeView_Archive.SelectedNode.Tag;
            string parentId;
            //0归档区 1归档项目区
            string type;
            if (selectObject is SelectKeepDeptModel)
            {
                var deptInfo = (SelectKeepDeptModel)selectObject;
                parentId = deptInfo.id;
                type = "0";
            }
            else
            {
                var deptInfo = (GetKeepProjectDirModel)selectObject;
                parentId = deptInfo.id;
                type = "1";
            }
            var frm = new FrmUploadFile(parentId, type);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadList();
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn删除文件_Click(object sender, EventArgs e)
        {
            var index = dataGridView_archiveFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;

                var selectModel = list[index];

                var selectFile = list.Where(o => o.isCheck || o.id == selectModel.id).Select(o => o.id).ToList();

                if (ShowSuccessOKCancelMsg($"是否确定删除文件！") == DialogResult.OK)
                {
                    Splasher.Show(typeof(FrmLoading));
                    var resultData = string.Empty;
                    if (HttpGet(AppGlobalModel.DelKeepProjectFile + $"?id={string.Join(",", selectFile)}", ref resultData))
                    {
                        total -= selectFile.Count;
                        dataGridView_archiveFile.DataSource = null;
                        dataGridView_archiveFile.DataSource = new SortableBindingList<GetKeepProjectDirModel>(list.Where(o => !selectFile.Contains(o.id)).ToList());
                        dataGridView_archiveFile.ClearSelection();

                        btn全选.Text = "全选";
                        btn删除文件.Enabled = false;
                        btn发起审批.Enabled = false;

                        Splasher.Close();
                    }
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
            var list = ((BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource).ToList();

            if (list.Exists(o => !o.isCheck))
            {
                for (var i = 0; i < dataGridView_archiveFile.Rows.Count; i++)
                {
                    dataGridView_archiveFile.Rows[i].Cells[0].Value = true;
                }

                btn全选.Text = "取消";
                btn删除文件.Enabled = AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:del");
                btn发起审批.Enabled = AppGlobalModel.OverallSituationMenu.Exists(o => o == "proarchive:apply");
            }
            else
            {
                for (var i = 0; i < dataGridView_archiveFile.Rows.Count; i++)
                {
                    dataGridView_archiveFile.Rows[i].Cells[0].Value = false;
                }

                btn全选.Text = "全选";
                btn删除文件.Enabled = false;
                btn发起审批.Enabled = false;
            }

        }

        /// <summary>
        /// 发起审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn发起审批_Click(object sender, EventArgs e)
        {
            var index = dataGridView_archiveFile.CurrentRow.Index;
            if (index > -1)
            {
                var list = (BindingList<GetKeepProjectDirModel>)dataGridView_archiveFile.DataSource;

                var selectModel = list[index];

                var selectFile = list.Where(o => o.isCheck || o.id == selectModel.id).Select(o => o.id).ToList();

                #region 获取项目属性信息
                var resultData = new GetProjectAttributeModel();
                if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={selectModel.projectId}", ref resultData))
                {
                    var frm = new FrmInitApproval(resultData, 3, string.Join(",", selectFile), 1, selectFile.Count == 1 ? (string.IsNullOrWhiteSpace(selectModel.pageAll) ? 0 : Convert.ToInt32(selectModel.pageAll)) : 0);
                    frm.ShowDialog();
                }
                #endregion      
            }
        }

        /// <summary>
        /// 树节点选中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (treeView_Archive.SelectedNode != null)
            {
                //将上一个选中的节点背景色还原（原先没有颜色）
                treeView_Archive.SelectedNode.BackColor = Color.Empty;
                //还原前景色
                treeView_Archive.SelectedNode.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// 树节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_Archive_Leave(object sender, EventArgs e)
        {
            if (treeView_Archive.SelectedNode != null)
            {
                //让选中项背景色呈现红色
                treeView_Archive.SelectedNode.BackColor = SystemColors.Highlight;
                //前景色为白色
                treeView_Archive.SelectedNode.ForeColor = Color.White;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn搜索_Click(object sender, EventArgs e)
        {
            var frm = new FrmSelectProject("archive");
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var parentList = frm.selectInfo.parentList.Split(',').ToList();
                parentList.Add(frm.selectInfo.id);

                foreach (TreeNode item in treeView_Archive.Nodes)
                {
                    var nodeInfo = (SelectKeepDeptModel)item.Tag;
                    if (parentList.Contains(nodeInfo.id))
                    {
                        treeView_Archive_NodeMouseClick(treeView_Archive, new TreeNodeMouseClickEventArgs(item, MouseButtons.Left, 0, 0, 0));

                        SimulationTreeViewNodeMouseClick(item, parentList);
                    }
                }
            }
        }

        /// <summary>
        /// 模拟点击
        /// </summary>
        /// <param name="treeNode"></param>
        /// <param name="parentList"></param>
        private void SimulationTreeViewNodeMouseClick(TreeNode treeNode, List<string> parentList)
        {
            //遍历树节点
            foreach (TreeNode item in treeNode.Nodes)
            {
                //获取树节点的标签
                var selectObject = item.Tag;
                //判断标签类型:查询归档目录层级/ 1：Id/ 2:parentId 父id/ 3:name 名称/ 4：createTime 创建时间/ 5：ancestors值/ 6：status 状态/ 7：identifier 标识符
                if (selectObject is SelectKeepDeptModel)
                {
                    //获取树节点的标签
                    var nodeInfo = (SelectKeepDeptModel)selectObject;
                    //判断标签的Id是否在列表中
                    if (parentList.Contains(nodeInfo.id))
                    {
                        //模拟点击
                        treeView_Archive_NodeMouseClick(treeView_Archive, new TreeNodeMouseClickEventArgs(item, MouseButtons.Left, 0, 0, 0));
                        //递归
                        SimulationTreeViewNodeMouseClick(item, parentList);
                    }
                }
                else
                {
                    //获取树节点的标签
                    var nodeInfo = (GetKeepProjectDirModel)selectObject;
                    //判断标签的Id是否在列表中
                    if (parentList.Contains(nodeInfo.id))
                    {
                        //模拟点击
                        treeView_Archive_NodeMouseClick(treeView_Archive, new TreeNodeMouseClickEventArgs(item, MouseButtons.Left, 0, 0, 0));
                        //递归
                        SimulationTreeViewNodeMouseClick(item, parentList);
                    }
                }
            }
        }

        private void btn文件清单_Click(object sender, EventArgs e)
        {
            //var frm = new FrmFileShoppingCart();
            var frm = new FrmArchiveFileShoppingCart();
            frm.ShowDialog();
        }


        /// <summary>
        /// 点鼠标两次的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_archiveFile_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                openFile(sender, e);
            }
        }
    }
    /// <summary>
    /// 查询项目文件
    /// </summary>
    class QueryKeepProjectFile
    {
        /// <summary>
        /// 父级id
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 父级id
        /// </summary>
        public string projectName { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int pageNum { get; set; }
        /// <summary>
        /// 每页条数
        /// </summary>
        public int pageSize { get; set; }
    }
}