using DMC.Helper;
using DMC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 项目文件区文件购物车
    /// </summary>
    public partial class FrmFileShoppingCart : BaseForm
    {
        //树形结构上面的右键菜单
        private ContextMenuStrip stripTreeView = new ContextMenuStrip();
        //那规则就是双击节点加载目录使用，点击节点右侧加载列表，使用+-号算点击对应的节点
        private QueryApprovalProjectStructure queryInfo = null;
        private string projectId = null;
        public FrmFileShoppingCart()
        {
            InitializeComponent();

            queryInfo = new QueryApprovalProjectStructure()
            {
                fileType = 0, //文件来源0 项目区 1归档区
                type = 0,    //发起类型 0购物车 1文件夹 2项目 3文件
                fileIds = "",  //流id列表  用  ，分割
                parentId = "0",  //上级ID
                tab = "1"
            };
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

        private void FrmFileShoppingCart_Load(object sender, EventArgs e)
        {
            LoadFileList();
        }

        #region 树结构菜单
        /// <summary>
        /// 树结构右键菜单
        /// </summary>
        private void LoadStripTreeView()
        {
            stripTreeView.Items.Clear();

            var selectObject = (GetKeepProjectDirModel)treeView1.SelectedNode.Tag;

            if (selectObject.type == 5)
            {
                ToolStripItem ts_openFile = new ToolStripMenuItem("打开");
                ts_openFile.Click += new EventHandler(openFile);
                stripTreeView.Items.Add(ts_openFile);
            }

            ToolStripItem ts_delete = new ToolStripMenuItem("删除");
            ts_delete.Click += new EventHandler(delFile);
            stripTreeView.Items.Add(ts_delete);
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFile(object sender, EventArgs e)
        {
            var projectInfo = (GetKeepProjectDirModel)treeView1.SelectedNode.Tag;

            var listUrl = new List<PreviewAreaViewModel>();
            foreach (TreeNode item in treeView1.SelectedNode.Parent.Nodes)
            {
                var itemInfo = (GetKeepProjectDirModel)item.Tag;
                listUrl.Add(new PreviewAreaViewModel() { filePath = itemInfo.filePath, name = itemInfo.name });
            }

            var frm = new FrmPreviewArea(projectInfo.filePath, queryInfo.fileType, listUrl);
            frm.Show();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void delFile(object sender, EventArgs e)
        {
            if (ShowSuccessOKCancelMsg("是否确定删除！") == DialogResult.OK)
            {
                var projectInfo = (GetKeepProjectDirModel)treeView1.SelectedNode.Tag;
                var para = new
                {
                    fileIds = projectInfo.id
                };

                var resultData = string.Empty;
                if (HttpPost(AppGlobalModel.DelFileCart, para, ref resultData))
                {
                    treeView1.SelectedNode.Remove();
                }
            }
        }
        #endregion

        #region 加载文件列表
        private void LoadFileList()
        {
            var resultData = new List<GetKeepProjectDirModel>();
            if (HttpPost(AppGlobalModel.GetApprovalProjectStructure, queryInfo, ref resultData))
            {
                if (resultData != null && resultData.Any())
                {
                    foreach (var item in resultData.OrderBy(o=>o.name))
                    {
                        TreeNode root = new TreeNode();
                        //根目录名称
                        if (item.type == 5)
                        {
                            root.Text = item.name + "          " + $"（图幅：{item.frameName} 折合A1：{item.folded}）";
                        }
                        else
                        {
                            root.Text = item.name;
                        }

                        root.Tag = item;

                        if (queryInfo.parentId == "0")
                        {
                            projectId = item.id;
                            treeView1.Nodes.Add(root);
                        }
                        else
                        {
                            treeView1.SelectedNode.Nodes.Add(root);
                        }
                    }

                    //第一次加载文件汇总
                    if (queryInfo.parentId == "0")
                    {
                        var resultFileAllData = new GetApprovalProjectStructureAllModel();
                        if (HttpPost(AppGlobalModel.GetApprovalProjectStructureAll, queryInfo, ref resultFileAllData))
                        {
                            if (resultFileAllData != null)
                            {
                                label1.Text = $"文件数量：{resultFileAllData.FileAll}";
                                //医药设计院时,不显示折合A1数量;
                                if (GlobalVariables.companyName == "吉林医药设计院有限公司")
                                {
                                    label2.Visible = false;
                                }
                                label2.Text = $"总A1数量：{resultFileAllData.FoldedAll}   A1";
                            }
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                }
            }
            else
            {
                if (queryInfo.parentId == "0")
                {
                    this.Close();
                }
            }
        }

        private bool treeViewNodeMouseClick = true;

        /// <summary>
        /// 节点点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                LoadStripTreeView();

                e.Node.ContextMenuStrip = stripTreeView;
            }
            else
            {
                var selectInfo = (GetKeepProjectDirModel)e.Node.Tag;
                if (e.Node.Nodes.Count <= 0 && selectInfo.type != 5)
                {
                    queryInfo.parentId = selectInfo.id;

                    LoadFileList();

                    if (treeViewNodeMouseClick)
                    {
                        treeView1.SelectedNode.Expand();
                    }
                }
            }

            treeViewNodeMouseClick = true;
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }
        #endregion

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 发起审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn发起审批_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(projectId))
            {
                ShowErrorMsg("没有任何文件，请先添加文件！");
            }
            else
            {
                #region 获取项目属性信息
                var resultData = new GetProjectAttributeModel();
                if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={projectId}", ref resultData))
                {
                    this.Close();
                    var frm = new FrmInitApproval(resultData, 0, "", 0);
                    frm.ShowDialog();
                }
                #endregion
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         /// <summary>
         /// 下载
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        private void button4下载_Click(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));

            var resultData = string.Empty;
            //调用JAVA下载接口
            if (HttpGet(AppGlobalModel.FileCartDownload, ref resultData))
            {
                Splasher.Close();

                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
            Splasher.Close();
        }
    }
    /// <summary>
    /// 查询审批项目结构：1、fileType：文件来源0 项目区 1归档区 /2、type：发起类型 0购物车 1文件夹 2项目 3文件 /3、fileIds：流id列表用，分割 /4、 parentId：上级ID /5、applyId：审批详情中得主键 /6、tab：是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
    /// </summary>
    class QueryApprovalProjectStructure
    {
        /// <summary>
        /// 文件来源0 项目区 1归档区
        /// </summary>
        public int fileType { get; set; }
        /// <summary>
        /// 发起类型 0购物车 1文件夹 2项目 3文件
        /// </summary>
        public int type { get; set; }
        /// <summary>
        /// 流id列表用  ，分割
        /// </summary>
        public string fileIds { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public string parentId { get; set; }
        /// <summary>
        /// 审批详情中得主键
        /// </summary>
        public string applyId { get; set; }
        /// <summary>
        /// 是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
        /// </summary>
        public string tab { get; set; }
    }
}