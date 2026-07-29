using DMC.Helper;
using DMC.Models;
using Mysqlx;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ScrollEventArgs = System.Windows.Forms.ScrollEventArgs;

namespace DMC
{
    /// <summary>
    /// 项目归档
    /// </summary>
    public partial class FrmProjectArchive : BaseForm
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        private string projectId = null;

        /// <summary>
        /// 文件夹架构List：1、ParentId：父ID /2、PrimaryKey：主键 /3、Name：名 /4、Type：类型1文件夹2文件 /5、fileUpload：上传文件参数
        /// </summary>
        private List<DirectoryStructureModel> directoryStructureList = null;

        /// <summary>
        /// 文件列表查询条件:1、fileType：文件来源0 项目区 1归档区 /2、type：发起类型 0购物车 1文件夹 2项目 3文件 /3、fileIds：流id列表用，分割 /4、 parentId：上级ID /5、applyId：审批详情中得主键 /6、tab：是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
        /// </summary>
        private QueryApprovalProjectStructure queryInfos = null;

        /// <summary>
        /// 本地记录
        /// </summary>
        private List<GetKeepTechnicalNameListModel> LocalFileList = null;

        /// <summary>
        /// 添加窗体字段来保存上传的文件ID
        /// </summary>
        private string UploadedSupplementaryFileId = string.Empty;

        /// <summary>
        /// 防止重复处理
        /// </summary>
        private bool isProcessing = false;

        /// <summary>
        /// 项目归档
        /// </summary>
        /// <param name="objId">要归档的项目Id</param>
        public FrmProjectArchive(string objId)
        {
            InitializeComponent();
            //不自动排列
            dataGridView1_项目属性信息.AutoGenerateColumns = false;
            dataGridView4_专业人员.AutoGenerateColumns = false;
            dataGridView项目技术资料归档表.AutoGenerateColumns = false;
            //归档的项目Id
            projectId = objId;

            queryInfos = new QueryApprovalProjectStructure()
            {
                fileType = 0, //文件来源0 项目区 1归档区
                type = 2,    //发起类型 0购物车 1文件夹 2项目 3文件
                fileIds = projectId,  //项目Id
                parentId = "0",  //上级ID
                tab = "1"  // 是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
            };
        }

        #region 窗口拉伸

        const int Guying_HTLEFT = 10;
        const int Guying_HTRIGHT = 11;
        const int Guying_HTTOP = 12;
        const int Guying_HTTOPLEFT = 13;
        const int Guying_HTTOPRIGHT = 14;
        const int Guying_HTBOTTOM = 15;
        const int Guying_HTBOTTOMLEFT = 0x10;
        const int Guying_HTBOTTOMRIGHT = 17;
        /// <summary>
        /// 窗口拉申方法
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0084:
                    base.WndProc(ref m);
                    Point vPoint = new Point((int)m.LParam & 0xFFFF,
                        (int)m.LParam >> 16 & 0xFFFF);
                    vPoint = PointToClient(vPoint);
                    if (vPoint.X <= 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPLEFT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMLEFT;
                        else m.Result = (IntPtr)Guying_HTLEFT;
                    else if (vPoint.X >= ClientSize.Width - 5)
                        if (vPoint.Y <= 5)
                            m.Result = (IntPtr)Guying_HTTOPRIGHT;
                        else if (vPoint.Y >= ClientSize.Height - 5)
                            m.Result = (IntPtr)Guying_HTBOTTOMRIGHT;
                        else m.Result = (IntPtr)Guying_HTRIGHT;
                    else if (vPoint.Y <= 5)
                        m.Result = (IntPtr)Guying_HTTOP;
                    else if (vPoint.Y >= ClientSize.Height - 5)
                        m.Result = (IntPtr)Guying_HTBOTTOM;
                    break;
                case 0x0201:                //鼠标左键按下的消息 
                    m.Msg = 0x00A1;         //更改消息为非客户区按下鼠标 
                    m.LParam = IntPtr.Zero; //默认值 
                    m.WParam = new IntPtr(2);//鼠标放在标题栏内 
                    base.WndProc(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

        #region 简化方法 窗体移动,直接变化Left、Top
        private Point originLocation;
        /// <summary>
        /// 窗口移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// 对窗口点下鼠标键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originLocation = e.Location;
            }
        }
        #endregion

        #region 窗口相关

        /// <summary>
        /// 最大化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMaxSide_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.MaximizedBounds = Screen.PrimaryScreen.WorkingArea;
                this.WindowState = FormWindowState.Maximized;
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            // 弹出消息框，标题为“提示”，内容为“是否保存？”，按钮为“是/否”
            DialogResult result = MessageBox.Show("是否保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // 用户点击了“是”
                // 执行保存操作
                保存审图版图纸附件记录(); // 假设这是你的保存方法
                保存项目技术资料记录();
            }
            this.Close();
        }

        /// <summary>
        /// 最小化窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMinSide_Click(object sender, EventArgs e)
        {
            //MinimizeForm();
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion

        #region 修改窗体加载和关闭事件

        /// <summary>
        /// 项目归档加载（修改版本 - 项目隔离）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProjectArchive_Load(object sender, EventArgs e)
        {
            try
            {
                // 初始化右键菜单
                初始化TreeView右键菜单();
                // 原有的加载逻辑...
                if (GlobalVariables.companyName == "华商国际工程有限公司")
                {
                    tabPage4审图版图纸及意见.Text = "审图版图纸及意见";
                }
                else
                {
                    tabPage4审图版图纸及意见.Text = "项目其它附件（可选上传）";
                }

                var resultDataModel = new GetProjectAttributeModel();

                #region 获取项目属性信息
                if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={projectId}", ref resultDataModel))
                {
                    textBox2.Text = resultDataModel.identifier;
                    textBox4.Text = resultDataModel.name;
                    textBox5.Text = resultDataModel.unit;

                    dataGridView1_项目属性信息.DataSource = resultDataModel.customList;
                    dataGridView1_项目属性信息.ClearSelection();

                    // 专业变量
                    var resultDataList = new List<GetProjectUserModel>();
                    if (HttpGet(AppGlobalModel.GetProjectUser + $"?projectId={projectId}", ref resultDataList))
                    {
                        if (resultDataList != null && resultDataList.Any())
                        {
                            #region 先添加角色的列
                            var roleList = resultDataList.First().roleList;

                            foreach (var item in roleList)
                            {
                                var col = new DataGridViewTextBoxColumn();
                                col.CellTemplate = new DataGridViewTextBoxCell();
                                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                col.Name = item.roleName;
                                col.HeaderText = item.roleName;
                                col.DataPropertyName = "roleList";

                                dataGridView4_专业人员.Columns.Add(col);
                            }

                            dataGridView4_专业人员.DataSource = resultDataList;
                            dataGridView4_专业人员.ClearSelection();
                            #endregion
                        }
                    }
                    else
                    {
                        this.Close();
                        return;
                    }
                }
                else
                {
                    this.Close();
                    return;
                }
                #endregion

                #region 加载技术资料（使用项目专属方法）

                var resulMySqlData = new List<GetKeepTechnicalNameListModel>();
                加载技术资料基础数据(ref resulMySqlData);

                初始化筛选分配技术资料列表(resulMySqlData);

                加载技术资料缓存记录();//加载本地记录:当前项目技术资料列表  这个变量会有内容
                // 应用本地记录到分类列表
                应用本地记录到分类列表();
                // 更新各分类的DataGridView显示
                更新资料表分类显示();

                #endregion

                // 新增：加载之前保存的附件记录
                加载审图附件记录();

                // 确保添加窗体关闭事件处理
                this.FormClosing += FrmProjectArchive_FormClosing;

                LogHelper.WriteLocalLog(this, $"项目归档页面加载完成，项目ID: {projectId}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载页面时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalLog(this, $"FrmProjectArchive_Load 出错: {ex.Message}");
                this.Close();
            }
        }

        /// <summary>
        /// 窗体关闭事件 - 保存附件记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProjectArchive_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("是否保存？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 用户点击了“是”
                    // 执行保存操作
                    保存审图版图纸附件记录(); // 假设这是你的保存方法
                    保存项目技术资料记录();
                }
                if (!isProcessing) // 防止重复执行
                {
                    if (UploadedSupplementaryFileId != "")
                        // 执行删除操作
                        if (RollbackDeleteSupplementaryFile(UploadedSupplementaryFileId))
                        {
                            //MessageBox.Show("补充文件已成功删除。", "删除成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            UploadedSupplementaryFileId = string.Empty; // 清空已删除的文件ID
                        }
                        else
                        {
                            // 阻止窗体关闭，让用户手动处理
                            //e.Cancel = true;
                            isProcessing = false; // 重置标志
                            return;
                        }
                }

                LogHelper.WriteLocalLog(this, "窗体关闭时已保存附件记录");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"保存附件记录时出错: {ex.Message}");
            }
        }

        #endregion

        #region DataGridView 操作

        int VerticalScrollIndex = 0;
        int HorizontalOffset = 0;

        /// <summary>
        /// 专业人员列表格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView4_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                var roleName = dataGridView4_专业人员.Columns[e.ColumnIndex].Name;
                var roleList = (List<RoleListItem>)e.Value;
                var roleInfo = roleList.FirstOrDefault(o => o.roleName == roleName);
                if (roleInfo != null)
                {
                    e.Value = string.Join(",", roleInfo.userList);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 表格序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                             e.RowBounds.Location.Y,
                                             dataGridView1_项目属性信息.RowHeadersWidth - 4,
                                             e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView1_项目属性信息.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView1_项目属性信息.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);


            if (dataGridView项目技术资料归档表.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView项目技术资料归档表.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].must == 0)
                    {
                        dataGridView项目技术资料归档表.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView项目技术资料归档表.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView项目技术资料归档表.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView项目技术资料归档表.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView项目技术资料归档表.Rows[i].Cells[2].Value = "";
                        dataGridView项目技术资料归档表.Rows[i].Cells[3].Value = "";
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView项目技术资料归档表.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView项目技术资料归档表.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView项目技术资料归档表.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_石油.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_石油.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].must == 0)
                    {
                        dataGridView_石油.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_石油.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_石油.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_石油.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_石油.Rows[i].Cells[2].Value = "";
                        dataGridView_石油.Rows[i].Cells[3].Value = "";
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_石油.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_石油.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_石油.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_农产品.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_农产品.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].must == 0)
                    {
                        dataGridView_农产品.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_农产品.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_农产品.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_农产品.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_农产品.Rows[i].Cells[2].Value = "";
                        dataGridView_农产品.Rows[i].Cells[3].Value = "";
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_农产品.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_农产品.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_农产品.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_食品.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_食品.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].must == 0)
                    {
                        dataGridView_食品.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_食品.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_食品.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_食品.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_食品.Rows[i].Cells[2].Value = "";
                        dataGridView_食品.Rows[i].Cells[3].Value = "";
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_食品.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_食品.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_食品.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_冷链物流.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_冷链物流.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].must == 0)
                    {
                        dataGridView_冷链物流.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_冷链物流.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_冷链物流.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_冷链物流.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_冷链物流.Rows[i].Cells[2].Value = "";
                        dataGridView_冷链物流.Rows[i].Cells[3].Value = "";
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_冷链物流.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_冷链物流.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_冷链物流.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_制冷.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_制冷.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].must == 0)
                    {
                        dataGridView_制冷.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_制冷.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_制冷.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_制冷.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_制冷.Rows[i].Cells[2].Value = "";
                        dataGridView_制冷.Rows[i].Cells[3].Value = "";
                    }

                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_制冷.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_制冷.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_制冷.Rows[i].Cells[4].Value = "";
                    }
                }
            }

        }

        /// <summary>
        /// 技术资料列表内容点击事件（修改版本 - 项目专属）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 点击button按钮事件
                if (dataGridView项目技术资料归档表.Columns[e.ColumnIndex].Name == "upload" && e.RowIndex >= 0)
                {
                    // 确保技术资料存储目录存在
                    if (!Directory.Exists(项目技术资料存储路径))
                    {
                        Directory.CreateDirectory(项目技术资料存储路径);
                    }
                    // 获取当前选中的行数据
                    DataGridView dataGridView = (DataGridView)sender;
                    // 获取当前行数据
                    var list = (List<GetKeepTechnicalNameListModel>)dataGridView.DataSource;
                    // 获取当前行数据
                    var dataInfo = list[e.RowIndex];
                    // 打开文件选择器
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = AppGlobalModel.InitialDirectory;// 初始目录
                    openFileDialog.Filter = "所有文件(*.*)|*.*";
                    openFileDialog.Multiselect = true; // 可以选择多个选项

                    if (openFileDialog.ShowDialog() == DialogResult.OK)// 点击确定
                    {
                        #region 保存打开的文件目录
                        AppGlobalModel.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);// 保存打开的文件目录
                        ConfigHelper.SaveConfigInfo("InitialDirectory", AppGlobalModel.InitialDirectory);// 保存打开的文件目录
                        #endregion

                        string[] strNames = openFileDialog.FileNames;
                        bool 有新文件添加 = false;

                        for (int i = 0; i < strNames.Length; i++)
                        {
                            try
                            {
                                // 检查是否已经存在该文件的记录
                                var 现有记录 = 当前项目技术资料列表.FirstOrDefault(o =>
                                    o.技术资料Id == dataInfo.id && o.原始文件路径 == strNames[i]);

                                if (现有记录 == null)
                                {
                                    // 获取文件信息
                                    FileInfo fileInfo = new FileInfo(strNames[i]);

                                    // 创建项目专属的存储目录
                                    string 项目资料目录 = Path.Combine(项目技术资料存储路径, projectId, dataInfo.id);
                                    if (!Directory.Exists(项目资料目录))
                                    {
                                        Directory.CreateDirectory(项目资料目录);
                                    }

                                    string 目标路径 = Path.Combine(项目资料目录, fileInfo.Name);

                                    // 复制文件
                                    File.Copy(strNames[i], 目标路径, true);

                                    // 创建本地记录
                                    var 本地记录 = new 本地技术资料记录
                                    {
                                        技术资料Id = dataInfo.id,
                                        原始文件路径 = strNames[i],
                                        本地存储路径 = 目标路径,
                                        文件名 = fileInfo.Name,
                                        上传时间 = DateTime.Now,
                                        文件大小 = fileInfo.Length
                                    };

                                    // 添加到当前项目的技术资料列表
                                    当前项目技术资料列表.Add(本地记录);
                                    有新文件添加 = true;

                                    // 更新数据显示
                                    if (string.IsNullOrWhiteSpace(dataInfo.localFile))
                                    {
                                        dataInfo.localFile = 目标路径;
                                    }
                                    else
                                    {
                                        list.Add(new GetKeepTechnicalNameListModel()
                                        {
                                            id = dataInfo.id,
                                            localFile = 目标路径,
                                            localFilePath = dataInfo.localFilePath,
                                            must = dataInfo.must,
                                            sort = dataInfo.sort,
                                            name = dataInfo.name
                                        });
                                    }
                                }
                            }
                            catch (Exception fileEx)
                            {
                                MessageBox.Show($"处理文件 {Path.GetFileName(strNames[i])} 时出错: {fileEx.Message}",
                                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }

                        if (有新文件添加)
                        {
                            // 保存项目技术资料记录
                            保存项目技术资料记录();

                            // 更新界面显示
                            dataGridView项目技术资料归档表.DataSource = null;
                            dataGridView项目技术资料归档表.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();

                            MessageBox.Show("文件上传成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else if (dataGridView项目技术资料归档表.Columns[e.ColumnIndex].Name == "download" && e.RowIndex >= 0)
                {
                    DataGridView dataGridView = (DataGridView)sender;
                    var dataInfo = ((List<GetKeepTechnicalNameListModel>)dataGridView.DataSource)[e.RowIndex];

                    var frm = new FrmDownloadFile(dataInfo.filePath);
                    frm.ShowDialog();
                }
                else if (dataGridView项目技术资料归档表.Columns[e.ColumnIndex].Name == "delFile" && e.RowIndex >= 0)
                {
                    if (ShowSuccessOKCancelMsg($"是否确定删除文件！") == DialogResult.OK)
                    {
                        DataGridView dataGridView = (DataGridView)sender;
                        var list = (List<GetKeepTechnicalNameListModel>)dataGridView.DataSource;
                        var dataInfo = ((List<GetKeepTechnicalNameListModel>)dataGridView.DataSource)[e.RowIndex];

                        try
                        {
                            // 查找并删除对应的本地记录
                            var 要删除的记录 = 当前项目技术资料列表
                                .Where(o => o.技术资料Id == dataInfo.id &&
                                       (!string.IsNullOrEmpty(dataInfo.localFile) ?
                                        o.本地存储路径 == dataInfo.localFile :
                                        o.技术资料Id == dataInfo.id))
                                .ToList();

                            foreach (var 记录 in 要删除的记录)
                            {
                                // 删除本地文件
                                if (File.Exists(记录.本地存储路径))
                                {
                                    File.Delete(记录.本地存储路径);
                                }

                                // 从记录列表中移除
                                当前项目技术资料列表.Remove(记录);
                            }

                            // 更新数据显示
                            if (string.IsNullOrWhiteSpace(dataInfo.name))
                            {
                                list.Remove(dataInfo);
                            }
                            else
                            {
                                var fistData = list.FirstOrDefault(o => o.id == dataInfo.id && string.IsNullOrWhiteSpace(o.name));

                                if (fistData != null)
                                {
                                    dataInfo.localFile = fistData.localFile;
                                    list.Remove(fistData);
                                }
                                else
                                {
                                    dataInfo.localFile = "";
                                }
                            }

                            // 保存更新后的记录
                            保存项目技术资料记录();

                            // 刷新界面
                            dataGridView项目技术资料归档表.DataSource = null;
                            dataGridView项目技术资料归档表.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                        }
                        catch (Exception delEx)
                        {
                            MessageBox.Show($"删除文件时出错: {delEx.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                dataGridView项目技术资料归档表.FirstDisplayedScrollingRowIndex = VerticalScrollIndex;
                dataGridView项目技术资料归档表.HorizontalScrollingOffset = HorizontalOffset;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理技术资料操作时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 技术资料上传的本地文件点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                var dataInfo = ((List<GetKeepTechnicalNameListModel>)dataGridView.DataSource)[e.RowIndex];
                if (!string.IsNullOrWhiteSpace(dataInfo.localFile))
                {
                    System.Diagnostics.Process.Start(dataInfo.localFile);
                }
            }
        }

        /// <summary>
        /// 格式化技术资料表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView2_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView项目技术资料归档表.Columns["Column1"].Index == e.ColumnIndex)
            {
                if (e.Value != null)
                {
                    e.Value = Path.GetFileName(e.Value.ToString());
                }
            }
        }

        /// <summary>
        /// 滚动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView2_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                VerticalScrollIndex = e.NewValue;
            }
            else if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                HorizontalOffset = e.NewValue;
            }
        }

        #endregion


        #region 加载施工图文件列表

        /// <summary>
        /// 在类的字段部分添加
        /// </summary>
        private ContextMenuStrip treeView右键菜单;

        /// <summary>
        /// 删除节点菜单项
        /// </summary>
        private ToolStripMenuItem 删除节点菜单项;

        /// <summary>
        /// 树节点单击事件是否为真
        /// </summary>
        private bool treeViewNodeMouseClick = true;

        /// <summary> 
        /// 获取指定文件夹下所有子目录及文件函数 
        /// </summary> 
        /// <param name="theDir">指定目录</param> 
        /// <param name="nLevel">默认起始值,调用时,一般为0</param> 
        /// <returns></returns> 
        public void ListTreeShow(DirectoryInfo theDir, string nLevel, TreeNode node)//递归目录 文件 
        {
            var dirModel = new DirectoryStructureModel();
            dirModel.ParentId = nLevel;
            dirModel.PrimaryKey = Guid.NewGuid().ToString();
            dirModel.Name = theDir.Name.ToString();
            dirModel.Type = 1;
            directoryStructureList.Add(dirModel);

            TreeNode root = new TreeNode();
            //根目录名称
            root.Text = dirModel.Name;
            root.Tag = dirModel;

            if (string.IsNullOrWhiteSpace(nLevel) && node == null)
            {
                treeView审图版图纸树.Nodes.Add(root);
            }
            else
            {
                node.Nodes.Add(root);
            }

            FileInfo[] fileInfo = theDir.GetFiles(); //目录下的文件 
            foreach (FileInfo fInfo in fileInfo)
            {
                var fileModel = new DirectoryStructureModel();
                fileModel.ParentId = dirModel.PrimaryKey;
                fileModel.Name = fInfo.FullName;
                fileModel.Type = 2;
                directoryStructureList.Add(fileModel);

                TreeNode rootFile = new TreeNode();
                //根目录名称
                rootFile.Text = fileModel.Name;
                rootFile.Tag = fileModel;
                root.Nodes.Add(rootFile);
            }

            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录 
            foreach (DirectoryInfo dirinfo in subDirectories)
            {
                ListTreeShow(dirinfo, dirModel.PrimaryKey, root);
            }
        }

        /// <summary>
        /// 加载施工图文件列表
        /// </summary>
        private void LoadFileList()
        {
            // 从全局变量获取项目信息
            //var selectProjectInfo = FrmProjectFile.treeNodeSelectProjectInto;
            var resultData = new List<GetKeepProjectDirModel>();

            if (HttpPost(AppGlobalModel.GetApprovalProjectStructure, queryInfos, ref resultData))
            {
                foreach (var item in resultData)
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

                    if (queryInfos.parentId == "0")
                    {
                        treeView项目文件树.Nodes.Add(root);
                    }
                    else
                    {
                        treeView项目文件树.SelectedNode.Nodes.Add(root);
                    }
                }

                //第一次加载要加载文件汇总
                if (queryInfos.parentId == "0")
                {
                    var resultFileAllData = new GetApprovalProjectStructureAllModel();
                    if (HttpPost(AppGlobalModel.GetApprovalProjectStructureAll, queryInfos, ref resultFileAllData))
                    {
                        label15.Text = $"文件数量：{resultFileAllData.FileAll}";
                        label2.Text = $"总A1数量：{resultFileAllData.FoldedAll}   A1";
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                if (queryInfos.parentId == "0")
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 节点展开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView2_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }

        /// <summary>
        /// 文件列表节点单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView施工图树_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView项目文件树.SelectedNode = e.Node;
            var selectInfo = (GetKeepProjectDirModel)e.Node.Tag;
            if (e.Node.Nodes.Count <= 0 && selectInfo.type != 5)
            {
                queryInfos.parentId = selectInfo.id;
                LoadFileList();

                if (treeViewNodeMouseClick)
                {
                    treeView审图版图纸树.SelectedNode.Expand();
                }
            }
            else
            {
                if (selectInfo.type == 5)
                {
                    var listUrl = new List<PreviewAreaViewModel>();
                    foreach (TreeNode item in e.Node.Parent.Nodes)
                    {
                        var itemInfo = (GetKeepProjectDirModel)item.Tag;
                        if (itemInfo.type == 5)
                        {
                            listUrl.Add(new PreviewAreaViewModel() { filePath = itemInfo.filePath, name = itemInfo.name });
                        }
                    }

                    var frm = new FrmPreviewArea(selectInfo.filePath, queryInfos.fileType, listUrl);
                    frm.Show();
                }
            }

            treeViewNodeMouseClick = true;
        }

        #endregion

        #region 审图版图纸

        /// <summary>
        /// 初始化TreeVie右键菜单
        /// </summary>
        private void 初始化TreeView右键菜单()
        {
            try
            {
                // 创建右键菜单
                treeView右键菜单 = new ContextMenuStrip();

                // 创建删除菜单项
                删除节点菜单项 = new ToolStripMenuItem("删除");
                删除节点菜单项.Click += 删除节点菜单项_Click;
                删除节点菜单项.Image = SystemIcons.Error.ToBitmap(); // 可选：添加删除图标

                // 添加菜单项到右键菜单
                treeView右键菜单.Items.Add(删除节点菜单项);

                // 为TreeView控件绑定右键菜单
                if (treeView项目文件树 != null)
                {
                    treeView项目文件树.ContextMenuStrip = treeView右键菜单;
                    treeView项目文件树.NodeMouseClick += TreeView项目文件树_NodeMouseClick;
                }

                if (treeView审图版图纸树 != null)
                {
                    treeView审图版图纸树.ContextMenuStrip = treeView右键菜单;
                    treeView审图版图纸树.NodeMouseClick += TreeView审图版图纸树_NodeMouseClick;
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, $"初始化TreeView右键菜单时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// TreeView节点鼠标点击事件（用于显示右键菜单）
        /// </summary>
        private void TreeView项目文件树_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                // 选中被点击的节点
                treeView项目文件树.SelectedNode = e.Node;

                // 如果是右键点击，显示菜单
                if (e.Button == MouseButtons.Right)
                {
                    // 可以根据需要设置不同的菜单项可见性
                    删除节点菜单项.Visible = true;
                    treeView右键菜单.Show(treeView项目文件树, e.Location);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, $"TreeView项目文件树节点点击时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 审图版图纸TreeView节点鼠标点击事件
        /// </summary>
        private void TreeView审图版图纸树_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                // 选中被点击的节点
                treeView审图版图纸树.SelectedNode = e.Node;

                // 如果是右键点击，显示菜单
                if (e.Button == MouseButtons.Right)
                {
                    // 可以根据需要设置不同的菜单项可见性
                    删除节点菜单项.Visible = true;
                    treeView右键菜单.Show(treeView审图版图纸树, e.Location);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, $"TreeView审图版图纸树节点点击时出错: {ex.Message}");
            }
        }

        #region 删除节点菜单项点击事件

        /// <summary>
        /// 删除节点菜单项点击事件
        /// </summary>
        private void 删除节点菜单项_Click(object sender, EventArgs e)
        {
            try
            {
                // 确定是哪个TreeView的节点被选中
                TreeNode selectedNode = null;
                TreeView sourceTreeView = null;

                if (treeView项目文件树 != null && treeView项目文件树.SelectedNode != null)
                {
                    selectedNode = treeView项目文件树.SelectedNode;
                    sourceTreeView = treeView项目文件树;
                }
                else if (treeView审图版图纸树 != null && treeView审图版图纸树.SelectedNode != null)
                {
                    selectedNode = treeView审图版图纸树.SelectedNode;
                    sourceTreeView = treeView审图版图纸树;
                }

                if (selectedNode == null)
                {
                    MessageBox.Show("请先选择要删除的节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 获取节点信息
                var nodeTag = selectedNode.Tag;
                string nodeName = selectedNode.Text;

                // 确认删除
                DialogResult result = MessageBox.Show(
                    $"确定要删除节点 '{nodeName}' 及其所有子节点吗？\n此操作不可恢复！",
                    "确认删除",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    return;
                }

                // 根据不同的TreeView执行不同的删除逻辑
                if (sourceTreeView == treeView项目文件树)
                {
                    删除项目文件树节点(selectedNode);
                }
                else if (sourceTreeView == treeView审图版图纸树)
                {
                    删除审图版图纸树节点(selectedNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除节点时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalErrorLog(this, $"删除节点时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除项目文件树节点
        /// </summary>
        private void 删除项目文件树节点(TreeNode nodeToDelete)
        {
            try
            {
                var nodeInfo = nodeToDelete.Tag as GetKeepProjectDirModel;
                if (nodeInfo == null)
                {
                    MessageBox.Show("无法删除该节点：节点信息不完整！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 如果是文件（type == 5），可以删除
                if (nodeInfo.type == 5)
                {
                    // 删除文件节点
                    DialogResult result = MessageBox.Show(
                        $"确定要删除文件 '{nodeInfo.name}' 吗？",
                        "确认删除文件",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // 从UI中移除节点
                        nodeToDelete.Remove();

                        // 如果需要从服务器删除，可以在这里添加代码
                        // 例如调用API删除服务器上的文件

                        LogHelper.WriteLocalLog(this, $"已删除项目文件树节点: {nodeInfo.name}");
                        MessageBox.Show("文件节点删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // 删除文件夹节点及其所有子节点
                    int 子节点数量 = GetChildNodeCount(nodeToDelete);
                    DialogResult result = MessageBox.Show(
                        $"确定要删除文件夹 '{nodeInfo.name}' 及其包含的 {子节点数量} 个子项吗？",
                        "确认删除文件夹",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // 从UI中移除节点
                        nodeToDelete.Remove();

                        // 如果需要从服务器删除，可以在这里添加代码

                        LogHelper.WriteLocalLog(this, $"已删除项目文件树文件夹节点: {nodeInfo.name} 及其子项");
                        MessageBox.Show("文件夹节点删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除项目文件树节点时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalErrorLog(this, $"删除项目文件树节点时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 删除审图版图纸树节点
        /// </summary>
        private void 删除审图版图纸树节点(TreeNode nodeToDelete)
        {
            try
            {
                var nodeInfo = nodeToDelete.Tag as DirectoryStructureModel;
                if (nodeInfo == null)
                {
                    MessageBox.Show("无法删除该节点：节点信息不完整！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 删除节点及其所有子节点
                int 子节点数量 = GetChildNodeCount(nodeToDelete);
                string 节点类型 = nodeInfo.Type == 1 ? "文件夹" : "文件";

                DialogResult result = MessageBox.Show(
                    $"确定要删除{节点类型} '{nodeInfo.Name}' 及其包含的 {子节点数量} 个子项吗？",
                    $"确认删除{节点类型}",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // 从附件记录列表中移除相关记录
                        删除附件记录(nodeInfo);

                        // 从UI中移除节点
                        nodeToDelete.Remove();

                        // 保存更新后的附件记录
                        保存审图版图纸附件记录();

                        LogHelper.WriteLocalLog(this, $"已删除审图版图纸树节点: {nodeInfo.Name} 及其子项");
                        MessageBox.Show($"{节点类型}删除成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception deleteEx)
                    {
                        MessageBox.Show($"删除节点时出错: {deleteEx.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogHelper.WriteLocalErrorLog(this, $"删除审图版图纸树节点时出错: {deleteEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除审图版图纸树节点时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalErrorLog(this, $"删除审图版图纸树节点时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 递归删除附件记录
        /// </summary>
        private void 删除附件记录(DirectoryStructureModel nodeInfo)
        {
            try
            {
                // 删除当前节点记录
                var 要删除的记录 = 审图版图纸附件列表.Where(r => r.PrimaryKey == nodeInfo.PrimaryKey).ToList();
                foreach (var record in 要删除的记录)
                {
                    审图版图纸附件列表.Remove(record);
                    LogHelper.WriteLocalLog(this, $"已从记录中删除节点: {record.Name}");
                }

                // 如果是文件夹，还需要删除其下的所有文件和子文件夹
                if (nodeInfo.Type == 1) // 文件夹
                {
                    // 删除所有子节点记录
                    var 子节点记录 = 审图版图纸附件列表.Where(r => r.ParentId == nodeInfo.PrimaryKey).ToList();
                    foreach (var childRecord in 子节点记录)
                    {
                        // 递归删除子节点
                        删除附件记录(childRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, $"删除附件记录时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取节点及其子节点的总数
        /// </summary>
        private int GetChildNodeCount(TreeNode node)
        {
            int count = 0;
            foreach (TreeNode childNode in node.Nodes)
            {
                count++; // 计算当前子节点
                count += GetChildNodeCount(childNode); // 递归计算子节点的子节点
            }
            return count;
        }

        #endregion

        #endregion

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 时间选择器格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            var dateTimePicker = (DateTimePicker)sender;
            //这里可以个更改自己的需要的格式，例：yyyy年MM月dd日
            dateTimePicker.CustomFormat = "yyyy-MM-dd";
        }


        #region 多分类筛选

        /// <summary>
        /// 各个分类的数据列表
        /// </summary>
        private List<GetKeepTechnicalNameListModel> 建筑资料列表 = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 石油资料列表 = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 农产品资料列表 = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 食品资料列表 = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 冷链物流资料列表 = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 制冷资料列表 = new List<GetKeepTechnicalNameListModel>();



        /// <summary>
        /// 筛选技术资料并分配到不同分类（灵活版 - 支持多分类）
        /// </summary>
        private void 初始化筛选分配技术资料列表(List<GetKeepTechnicalNameListModel> sourceData)
        {
            try
            {
                // 清空各分类列表
                建筑资料列表.Clear();
                石油资料列表.Clear();
                农产品资料列表.Clear();
                食品资料列表.Clear();
                冷链物流资料列表.Clear();
                制冷资料列表.Clear();

                // 定义各分类的关键字（可以重叠）
                var 分类关键字 = new Dictionary<string, List<string>>
                {
                    {
                        "建筑",
                        new List<string> {"设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                            "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单","地形图",
                            "红线图","市政管网图","用地勘界图","岩土工程勘察报告","会议纪要",
                            "会议签到表","设计方案确认函","施工图启动函","建筑工程施工图设计文件审查意见及回复","建筑工程施工图设计文件审查合格书",
                            "图纸会审","洽商记录","设计函","工作联系单","建筑工程竣工验收备案表",
                            "建筑工程竣工验收报告","顾客满意度调查表","建设单位意见","施工单位意见","建筑节能计算书",
                            "结构计算书","水计算书","暖通计算书","电计算书","制冷工艺计算书",
                            "设计服务及设计更改文件汇总表","中标通知书","设计变更通知书","规划设计条件通知单","初步设计阶段的批复文件",
                            "工程名称变更函","发改委立项批复","规划设计方案批准书","规划意见书","建筑项目选址意见书",
                            "建设用地规划许可证","建设工程规划许可证","企业投资项目备案证明","单项工程登记备案申请表","外勘察设计单位勘察设计项目备案表",
                            "公共建筑节能设计审查备案登记表","防空地下室规划建设要点","绿色建筑设计标识申报自评价报告","绿色建筑施工图设计审查备案表","建设项目环境影响报告表",
                            "环境影响评价报告","社会稳定风险分析报告","地质灾害危险性评价报告书","建筑工程竣工验收消防备案凭证","建筑工程消防验收意见书",
                             "建设工程规划核实合格书","其他"}
                    },
                    {
                        "石油",
                        new List<string> { "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                            "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单","地形图",
                            "红线图","岩土工程勘察报告","会议纪要",
                            "会议签到表","设计方案确认函","施工图启动函",
                            "图纸会审","设计变更","洽商记录","设计函","工作联系单",
                            "建筑工程竣工验收报告","顾客满意度调查表","建设单位意见","施工单位意见",
                            "建筑节能计算书","结构计算书","水计算书","暖通计算书","电计算书","制冷工艺计算书","石油工艺计算书",
                            "设计服务及设计更改文件汇总表","中标通知书","设计变更通知书","初步设计阶段的批复文件",
                            "工程名称变更函","建筑项目选址意见书","建设用地规划许可证",
                            "安全条件论证报告","安全设施设计专篇","环境影响评价报告","地质灾害危险性评价报告书","建筑工程消防验收意见书",
                             "其他"}
                    },
                    {
                        "农产品",
                        new List<string> { "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                            "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单","地形图",
                            "红线图","岩土工程勘察报告","会议纪要",
                            "会议签到表","设计方案确认函","施工图启动函","建筑工程施工图设计文件审查意见及回复",
                            "建筑工程施工图设计文件审查合格书","图纸会审","设计变更","设计函","工作联系单",
                            "建筑工程竣工验收备案表","建筑工程竣工验收报告","顾客满意度调查表","建设单位意见","施工单位意见",
                            "建筑节能计算书","结构计算书","水计算书","暖通计算书","电计算书",
                            "设计服务及设计更改文件汇总表","中标通知书","设计变更通知书",
                            "设计变更通知书","规划设计条件通知单","初步设计阶段的批复文件","工程名称变更函","发改委立项批复","规划设计方案批准书",
                            "规划意见书","建筑项目选址意见书","建设用地规划许可证","建设工程规划许可证","企业投资项目备案证明",
                            "单项工程登记备案申请表","公共建筑节能设计审查备案登记表","防空地下室规划建设要点",
                            "绿色建筑设计标识申报自评价报告","绿色建筑施工图设计审查备案表","建设项目环境影响报告表","安全卫生评价报告","安全现状评价报告",
                            "安全条件论证报告","安全设施设计专篇","环境影响评价报告","社会稳定风险分析报告","地质灾害危险性评价报告书",
                            "建筑工程竣工验收消防备案凭证","建筑工程消防验收意见书","建设工程规划核实合格书",
                             "其他", }
                    },
                    {
                        "食品",
                        new List<string> { "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                             "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单",
                             "会议纪要",
                             "设计方案确认函","施工图启动函",
                             "图纸会审","洽商记录","设计函","工作联系单",
                             "顾客满意度调查表",
                             "设计服务及设计更改文件汇总表","设计变更通知书","其他", }
                    },{
                        "冷链物流",
                        new List<string> { "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                            "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单","地形图",
                            "红线图","岩土工程勘察报告","会议纪要",
                            "会议签到表","设计方案确认函","施工图启动函","建筑工程施工图设计文件审查意见及回复",
                            "建筑工程施工图设计文件审查合格书","图纸会审","设计变更","设计函","工作联系单",
                            "建筑工程竣工验收备案表","建筑工程竣工验收报告","顾客满意度调查表","建设单位意见","施工单位意见",
                            "建筑节能计算书","结构计算书","水计算书","暖通计算书","电计算书",
                            "设计服务及设计更改文件汇总表","中标通知书","设计变更通知书",
                            "设计变更通知书","规划设计条件通知单","初步设计阶段的批复文件","工程名称变更函","发改委立项批复","规划设计方案批准书",
                            "规划意见书","建筑项目选址意见书","建设用地规划许可证","建设工程规划许可证","企业投资项目备案证明",
                            "单项工程登记备案申请表","公共建筑节能设计审查备案登记表","防空地下室规划建设要点",
                            "绿色建筑设计标识申报自评价报告","绿色建筑施工图设计审查备案表","建设项目环境影响报告表","安全卫生评价报告","安全现状评价报告",
                            "安全条件论证报告","安全设施设计专篇","环境影响评价报告","社会稳定风险分析报告","地质灾害危险性评价报告书",
                            "建筑工程竣工验收消防备案凭证","建筑工程消防验收意见书","建设工程规划核实合格书",
                             "其他", }
                    },{
                        "制冷",
                        new List<string> { "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                            "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单","地形图",
                            "红线图","岩土工程勘察报告","会议纪要",
                            "会议签到表","设计方案确认函","施工图启动函","建筑工程施工图设计文件审查意见及回复",
                            "建筑工程施工图设计文件审查合格书","图纸会审","设计变更","设计函","工作联系单",
                            "建筑工程竣工验收备案表","建筑工程竣工验收报告","顾客满意度调查表","建设单位意见","施工单位意见",
                            "建筑节能计算书","结构计算书","水计算书","暖通计算书","电计算书",
                            "设计服务及设计更改文件汇总表","中标通知书","设计变更通知书",
                            "设计变更通知书","规划设计条件通知单","初步设计阶段的批复文件","工程名称变更函","发改委立项批复","规划设计方案批准书",
                            "规划意见书","建筑项目选址意见书","建设用地规划许可证","建设工程规划许可证","企业投资项目备案证明",
                            "单项工程登记备案申请表","公共建筑节能设计审查备案登记表","防空地下室规划建设要点",
                            "绿色建筑设计标识申报自评价报告","绿色建筑施工图设计审查备案表","建设项目环境影响报告表","安全卫生评价报告","安全现状评价报告",
                            "安全条件论证报告","安全设施设计专篇","环境影响评价报告","社会稳定风险分析报告","地质灾害危险性评价报告书",
                            "建筑工程竣工验收消防备案凭证","建筑工程消防验收意见书","建设工程规划核实合格书",
                             "其他", }
                    },
                };

                // 为每个数据项进行分类
                foreach (var item in sourceData)
                {
                    string itemName = item.name ?? "";
                    bool 已分配到任何分类 = false;

                    // 检查每个分类
                    foreach (var category in 分类关键字.Keys)
                    {
                        bool 匹配当前分类 = 分类关键字[category].Any(keyword =>
                            itemName.Contains(keyword));

                        if (匹配当前分类)
                        {
                            // 根据分类添加到对应列表
                            switch (category)
                            {
                                case "建筑":
                                    建筑资料列表.Add(CloneTechnicalItem(item));
                                    已分配到任何分类 = true;
                                    break;
                                case "石油":
                                    石油资料列表.Add(CloneTechnicalItem(item));
                                    已分配到任何分类 = true;
                                    break;
                                case "农产品":
                                    农产品资料列表.Add(CloneTechnicalItem(item));
                                    已分配到任何分类 = true;
                                    break;
                                case "食品":
                                    食品资料列表.Add(CloneTechnicalItem(item));
                                    已分配到任何分类 = true;
                                    break;
                                case "冷链物流":
                                    冷链物流资料列表.Add(CloneTechnicalItem(item));
                                    已分配到任何分类 = true;
                                    break;
                                case "制冷":
                                    制冷资料列表.Add(CloneTechnicalItem(item));
                                    已分配到任何分类 = true;
                                    break;
                            }
                        }
                    }

                    // 如果没有匹配任何分类，则添加到默认的建筑分类
                    if (!已分配到任何分类)
                    {
                        建筑资料列表.Add(CloneTechnicalItem(item));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"筛选技术资料时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 应用本地记录到各分类列表
        /// </summary>
        private void 应用本地记录到分类列表()
        {
            try
            {
                // 应用建筑分类记录
                应用本地记录到分类列表(建筑资料列表, "建筑");

                // 应用石油分类记录
                应用本地记录到分类列表(石油资料列表, "石油");

                // 应用农产品分类记录
                应用本地记录到分类列表(农产品资料列表, "农产品");

                // 应用食品分类记录
                应用本地记录到分类列表(食品资料列表, "食品");

                // 应用冷链物流分类记录
                应用本地记录到分类列表(冷链物流资料列表, "冷链物流");

                // 应用制冷分类记录
                应用本地记录到分类列表(制冷资料列表, "制冷");

                LogHelper.WriteLocalLog(this, $"应用本地记录到分类列表完成。");

            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, $"应用本地记录到分类列表时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 应用指定分类的记录到列表
        /// </summary>
        private void 应用本地记录到分类列表(List<GetKeepTechnicalNameListModel> list, string 分类名称)
        {
            try
            {
                LocalFileList = new List<GetKeepTechnicalNameListModel>();
                // 查找该分类的本地记录
                var 分类记录 = 当前项目技术资料列表.Where(r => r.分类 == 分类名称).ToList();
                //LogHelper.WriteLocalLog(this, $"分类 {分类名称} 有 {分类记录.Count} 个记录");

                foreach (var record in 分类记录)
                {
                    LogHelper.WriteLocalLog(this, $"处理记录: ID={record.技术资料Id}, 文件={record.文件名}, 路径={record.本地存储路径}");

                    // 查找对应的资料项
                    var 对应资料 = list.FirstOrDefault(item => item.id == record.技术资料Id);
                    if (对应资料 != null)
                    {
                        LogHelper.WriteLocalLog(this, $"找到对应资料项: {对应资料.name}");
                        LocalFileList.Add(new GetKeepTechnicalNameListModel()//新加入同样名头的一行，也就是一个名头有多个文件
                        {
                            id = record.技术资料Id,
                            localFile = record.本地存储路径,
                            localFilePath = record.本地存储路径,
                            must = 对应资料.must,
                            sort = 对应资料.sort,
                            name = 对应资料.name
                        });
                        // 更新数据显示
                        if (string.IsNullOrWhiteSpace(对应资料.localFile))
                        {
                            对应资料.localFile = record.文件名;
                            LogHelper.WriteLocalLog(this, $"更新文件路径: {record.文件名}");
                        }
                        else
                        {
                            list.Add(new GetKeepTechnicalNameListModel()//新加入同样名头的一行，也就是一个名头有多个文件
                            {
                                id = record.技术资料Id,
                                localFile = record.文件名,
                                localFilePath = record.本地存储路径,
                                must = 对应资料.must,
                                sort = 对应资料.sort,
                                name = 对应资料.name
                            });

                            LogHelper.WriteLocalLog(this, $"新加文件路径: {record.文件名}");
                        }
                    }
                    else
                    {
                        LogHelper.WriteLocalLog(this, $"未找到对应资料项，ID: {record.技术资料Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"应用{分类名称}分类记录时出错: {ex.Message}");
                LogHelper.WriteLocalErrorLog(this, ex, $"应用{分类名称}分类记录时出错:");
            }
        }

        /// <summary>
        /// 克隆技术资料项（深度复制）
        /// </summary>
        private GetKeepTechnicalNameListModel CloneTechnicalItem(GetKeepTechnicalNameListModel source)
        {
            if (source == null)
                return null;

            return new GetKeepTechnicalNameListModel
            {
                id = source.id,
                name = source.name,
                localFile = source.localFile,
                localFilePath = source.localFilePath,
                filePath = source.filePath,
                must = source.must,
                sort = source.sort,
                fileUpload = source.fileUpload,
                rowNo = source.rowNo,
                status = source.status,
                major = source.major,

            };
        }

        /// <summary>
        /// 更新各分类建筑资料列表\石油资料列表等表的显示
        /// </summary>
        private void 更新资料表分类显示()
        {
            try
            {
                LogHelper.WriteLocalLog(this, "开始更新分类显示");

                // 更新建筑分类显示
                if (dataGridView项目技术资料归档表 != null)
                {
                    LogHelper.WriteLocalLog(this, $"建筑分类数据项数: {建筑资料列表.Count}");
                    dataGridView项目技术资料归档表.AutoGenerateColumns = false;
                    dataGridView项目技术资料归档表.DataSource = null;
                    dataGridView项目技术资料归档表.DataSource = 建筑资料列表.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                    dataGridView项目技术资料归档表.ClearSelection();
                    LogHelper.WriteLocalLog(this, "建筑分类显示更新完成");
                }

                // 更新石油分类显示
                if (dataGridView_石油 != null)
                {
                    LogHelper.WriteLocalLog(this, $"石油分类数据项数: {石油资料列表.Count}");
                    dataGridView_石油.AutoGenerateColumns = false;
                    dataGridView_石油.DataSource = null;
                    dataGridView_石油.DataSource = 石油资料列表.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                    dataGridView_石油.ClearSelection();
                    LogHelper.WriteLocalLog(this, "石油分类显示更新完成");
                }

                // 更新农产品分类显示
                if (dataGridView_农产品 != null)
                {
                    LogHelper.WriteLocalLog(this, $"农产品分类数据项数: {农产品资料列表.Count}");
                    dataGridView_农产品.AutoGenerateColumns = false;
                    dataGridView_农产品.DataSource = null;
                    dataGridView_农产品.DataSource = 农产品资料列表.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                    dataGridView_农产品.ClearSelection();
                    LogHelper.WriteLocalLog(this, "农产品分类显示更新完成");
                }

                // 更新食品分类显示
                if (dataGridView_食品 != null)
                {
                    LogHelper.WriteLocalLog(this, $"食品分类数据项数: {食品资料列表.Count}");
                    dataGridView_食品.AutoGenerateColumns = false;
                    dataGridView_食品.DataSource = null;
                    dataGridView_食品.DataSource = 食品资料列表.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                    dataGridView_食品.ClearSelection();
                    LogHelper.WriteLocalLog(this, "食品分类显示更新完成");
                }

                // 更新冷链物流分类显示
                if (dataGridView_冷链物流 != null)
                {
                    LogHelper.WriteLocalLog(this, $"冷链物流分类数据项数: {冷链物流资料列表.Count}");
                    dataGridView_冷链物流.AutoGenerateColumns = false;
                    dataGridView_冷链物流.DataSource = null;
                    dataGridView_冷链物流.DataSource = 冷链物流资料列表.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                    dataGridView_冷链物流.ClearSelection();
                    LogHelper.WriteLocalLog(this, "冷链物流分类显示更新完成");
                }

                // 更新制冷分类显示
                if (dataGridView_制冷 != null)
                {
                    LogHelper.WriteLocalLog(this, $"制冷分类数据项数: {制冷资料列表.Count}");
                    dataGridView_制冷.AutoGenerateColumns = false;
                    dataGridView_制冷.DataSource = null;
                    dataGridView_制冷.DataSource = 制冷资料列表.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                    dataGridView_制冷.ClearSelection();
                    LogHelper.WriteLocalLog(this, "制冷分类显示更新完成");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新分类显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalErrorLog(this, ex, "更新分类显示时出错:");
            }
        }

        /// <summary>
        /// 更新各分类的显示
        /// </summary>
        private void 更新分类显示(List<GetKeepTechnicalNameListModel> list, TabControl tabControl, string selectTabItem)
        {
            try
            {
                if (selectTabItem != null)
                {
                    selectTabItem = selectTabItem.Replace(" ", "");//去除空格
                    switch (selectTabItem)
                    {
                        case "建筑":
                            建筑资料列表 = list;
                            dataGridView项目技术资料归档表.DataSource = null;
                            dataGridView项目技术资料归档表.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                            //dataGridView项目技术资料归档表.ClearSelection();
                            break;
                        case "石油":
                            石油资料列表 = list;
                            dataGridView_石油.DataSource = null;
                            dataGridView_石油.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                            //dataGridView_石油.ClearSelection();
                            break;
                        case "农产品":
                            农产品资料列表 = list;
                            dataGridView_农产品.DataSource = null;
                            dataGridView_农产品.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                            //dataGridView_农产品.ClearSelection();
                            break;
                        case "食品":
                            食品资料列表 = list;
                            dataGridView_食品.DataSource = null;
                            dataGridView_食品.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                            //dataGridView_食品.ClearSelection();
                            break;
                        case "冷链物流":
                            冷链物流资料列表 = list;
                            dataGridView_冷链物流.DataSource = null;
                            dataGridView_冷链物流.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                            //dataGridView_冷链物流.ClearSelection();
                            break;
                        case "制冷":
                            制冷资料列表 = list;
                            dataGridView_制冷.DataSource = null;
                            dataGridView_制冷.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                            //dataGridView_制冷.ClearSelection();
                            break;
                    }
                }
                else
                {
                    dataGridView项目技术资料归档表.DataSource = null;
                    dataGridView项目技术资料归档表.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                    //dataGridView项目技术资料归档表.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"更新分类显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 统一处理技术资料操作（多分类支持版）
        /// </summary>
        private void 处理技术资料操作(object sender, DataGridViewCellEventArgs e, List<GetKeepTechnicalNameListModel> 当前资料列表, string 当前分类名称)
        {
            try
            {
                // 获取当前选中的行数据 delFile_农产品
                DataGridView dataGridView = (DataGridView)sender;
                var uploadList = new List<string>()
                {
                   "upload","upload_建筑", "upload_石油", "Upload_农产品", "Upload_食品", "Upload_冷链物流", "Upload_制冷"

                };
                var delFileList = new List<string>()
                {
                   "delFile", "delFile_建筑", "delFile_石油", "delFile_农产品", "delFile_食品", "delFile_冷链物流", "delFile_制冷"
                };
                var donwnloadList = new List<string>()
                {
                    "download","download_建筑", "download_石油", "download_农产品", "download_食品", "download_冷链物流", "download_制冷"
                };
                // 点击button按钮事件
                if (uploadList.Contains(dataGridView.Columns[e.ColumnIndex].Name) && e.RowIndex >= 0)
                {
                    // 确保技术资料存储目录存在
                    if (!Directory.Exists(项目技术资料存储路径))
                    {
                        Directory.CreateDirectory(项目技术资料存储路径);
                    }

                    // 获取当前dataGridView.DataSource表数据
                    var list = (List<GetKeepTechnicalNameListModel>)dataGridView.DataSource;
                    // 获取当前行数据
                    var dataInfo = list[e.RowIndex];
                    // 打开文件选择器
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = AppGlobalModel.InitialDirectory;// 初始目录
                    openFileDialog.Filter = "所有文件(*.*)|*.*";
                    openFileDialog.Multiselect = true; // 可以选择多个选项

                    if (openFileDialog.ShowDialog() == DialogResult.OK)// 点击确定
                    {
                        string[] strNames = openFileDialog.FileNames;
                        bool 有新文件添加 = false;
                        for (int i = 0; i < strNames.Length; i++)
                        {
                            try
                            {
                                // 检查是否已经存在该文件的记录（在同一分类中）
                                var 现有记录 = 当前项目技术资料列表.FirstOrDefault(o =>
                                    o.技术资料Id == dataInfo.id &&
                                    o.原始文件路径 == strNames[i] &&
                                    o.分类 == 当前分类名称);

                                if (现有记录 == null)
                                {
                                    // 获取文件信息
                                    FileInfo fileInfo = new FileInfo(strNames[i]);

                                    // 创建项目专属的存储目录
                                    string 项目资料目录 = Path.Combine(项目技术资料存储路径, projectId, dataInfo.id);
                                    if (!Directory.Exists(项目资料目录))
                                    {
                                        Directory.CreateDirectory(项目资料目录);
                                    }
                                    // 获取文件名中"."的位置序号
                                    int dotIndex = dataInfo.name.IndexOf('.');
                                    // 结果：如果存在"."，则截取"."之前的字符，否则返回"00"
                                    string resultNo = dotIndex != -1 ? dataInfo.name.Substring(0, dotIndex + 1) : "00";
                                    // 目标路径
                                    string 目标路径 = Path.Combine(项目资料目录, $"{resultNo}" + $"{i}" + "." + fileInfo.Name);

                                    // 复制文件
                                    File.Copy(strNames[i], 目标路径, true);

                                    // 创建本地记录
                                    var 本地记录 = new 本地技术资料记录
                                    {
                                        技术资料Id = dataInfo.id,
                                        原始文件路径 = strNames[i],
                                        本地存储路径 = 目标路径,
                                        文件名 = $"{resultNo}" + $"{i}" + "." + fileInfo.Name,
                                        上传时间 = DateTime.Now,
                                        文件大小 = fileInfo.Length,
                                        分类 = 当前分类名称 // 记录分类信息
                                    };
                                    LocalFileList.Add(new GetKeepTechnicalNameListModel()//新加入同样名头的一行，也就是一个名头有多个文件
                                    {
                                        id = dataInfo.id,
                                        localFile = 目标路径,
                                        localFilePath = 目标路径,
                                        must = dataInfo.must,
                                        sort = dataInfo.sort,
                                        name = dataInfo.name
                                    });
                                    // 添加到当前项目的技术资料列表
                                    当前项目技术资料列表.Add(本地记录);
                                    有新文件添加 = true;

                                    // 更新数据显示
                                    if (string.IsNullOrWhiteSpace(dataInfo.localFile))
                                    {
                                        dataInfo.localFile = 目标路径;
                                    }
                                    else
                                    {
                                        list.Add(new GetKeepTechnicalNameListModel()//新加入同样名头的一行，也就是一个名头有多个文件
                                        {
                                            id = dataInfo.id,
                                            localFile = 目标路径,
                                            localFilePath = 目标路径,
                                            must = dataInfo.must,
                                            sort = dataInfo.sort,
                                            name = dataInfo.name
                                        });
                                    }
                                }
                            }
                            catch (Exception fileEx)
                            {
                                MessageBox.Show($"处理文件 {Path.GetFileName(strNames[i])} 时出错: {fileEx.Message}",
                                    "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                LogHelper.WriteLocalErrorLog(this, $"处理文件 {Path.GetFileName(strNames[i])} 时出错: {fileEx.Message}",
                                    "错误");
                            }
                        }

                        if (有新文件添加)
                        {
                            // 保存项目技术资料记录
                            保存项目技术资料记录();
                            // 获取当前选中的TabItem名
                            var selectTabItem = this.tabControl_技术资料.SelectedTab.Text;
                            更新分类显示(list, tabControl_技术资料, selectTabItem);
                            MessageBox.Show("文件上传成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogHelper.WriteLocalLog(this, "上传文件成功！");
                        }
                    }
                }

                // 下载按钮
                else if (donwnloadList.Contains(dataGridView.Columns[e.ColumnIndex].Name) && e.RowIndex >= 0)
                {
                    dataGridView = (DataGridView)sender;
                    var dataInfo = ((List<GetKeepTechnicalNameListModel>)dataGridView.DataSource)[e.RowIndex];

                    var frm = new FrmDownloadFile(dataInfo.filePath, null);

                    frm.ShowDialog();
                    //if (frm.ShowDialog() == DialogResult.OK)
                    //{
                    //    // 获取文件名
                    //    string fileName = Path.GetFileName(dataInfo.filePath);
                    //    // 获取文件存储路径
                    //    string filePath = Path.Combine(项目技术资料存储路径, projectId, dataInfo.id, fileName);
                    //    // 创建文件
                    //    File.Create(filePath).Dispose();
                    //}
                }
                // 删除按钮 - 完整优化版
                else if (delFileList.Contains(dataGridView.Columns[e.ColumnIndex].Name) && e.RowIndex >= 0)
                {
                    if (MessageBox.Show("是否确定删除文件！", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        dataGridView = (DataGridView)sender;
                        var list = (List<GetKeepTechnicalNameListModel>)dataGridView.DataSource;
                        var dataInfo = ((List<GetKeepTechnicalNameListModel>)dataGridView.DataSource)[e.RowIndex];
                        dataInfo.rowNo = e.RowIndex;

                        try
                        {
                            // 查找并删除对应的本地记录
                            var 要删除的记录 = 当前项目技术资料列表
                                .Where(o => o.技术资料Id == dataInfo.id &&
                                       o.分类 == 当前分类名称 &&
                                       (!string.IsNullOrEmpty(dataInfo.localFilePath) ?
                                        o.本地存储路径 == dataInfo.localFilePath :
                                        o.技术资料Id == dataInfo.id))
                                .ToList();

                            foreach (var 记录 in 要删除的记录)
                            {
                                if (File.Exists(记录.本地存储路径))
                                {
                                    File.Delete(记录.本地存储路径);
                                }
                                当前项目技术资料列表.Remove(记录);
                                同步LocalFileList();

                                LocalFileList.Remove(dataInfo);
                            }

                            // 更新DataGridView显示
                            var 相同名称的记录列表 = list.Where(o => o.name == dataInfo.name).ToList();

                            if (相同名称的记录列表.Count > 1)
                            {
                                // 有多行相同name的记录，删除当前行
                                list.RemoveAt(e.RowIndex);
                            }
                            else
                            {
                                // 只有一行相同name的记录，只清除数据
                                dataInfo.localFile = "";
                                dataInfo.localFilePath = "";
                            }

                            // 保存更新后的记录
                            保存项目技术资料记录();

                            // 更新分类显示
                            更新分类显示(list, this.tabControl_技术资料, this.tabControl_技术资料.SelectedTab.Text);
                        }
                        catch (Exception delEx)
                        {
                            MessageBox.Show($"删除文件时出错: {delEx.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LogHelper.WriteLocalErrorLog(this, $"删除文件时出错: {delEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理技术资料操作时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalErrorLog(this, $"处理技术资料操作时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 同步 LocalFileList 与 当前项目技术资料列表
        /// </summary>
        private void 同步LocalFileList()
        {
            try
            {
                LocalFileList.Clear();

                // 从当前项目技术资料列表创建 LocalFileList
                foreach (var record in 当前项目技术资料列表)
                {
                    var existingItem = LocalFileList.FirstOrDefault(x => x.id == record.技术资料Id && x.localFile == record.本地存储路径);

                    if (existingItem == null)
                    {
                        LocalFileList.Add(new GetKeepTechnicalNameListModel()
                        {
                            id = record.技术资料Id,
                            localFile = record.本地存储路径,
                            localFilePath = record.本地存储路径,
                            must = 0, // 根据需要设置
                            sort = 0, // 根据需要设置
                            name = "" // 根据需要设置
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalErrorLog(this, $"同步LocalFileList时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 建筑分类DataGridView事件处理
        /// </summary>
        private void DataGridView_建筑_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            处理技术资料操作(sender, e, 建筑资料列表, "建筑");
        }

        /// <summary>
        /// 石油分类DataGridView事件处理
        /// </summary>
        private void DataGridView_石油_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            处理技术资料操作(sender, e, 石油资料列表, "石油");
        }

        /// <summary>
        /// 农产品分类DataGridView事件处理
        /// </summary>
        private void DataGridView_农产品_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            处理技术资料操作(sender, e, 农产品资料列表, "农产品");
        }

        /// <summary>
        /// 食品分类DataGridView事件处理
        /// </summary>
        private void DataGridView_食品_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            处理技术资料操作(sender, e, 食品资料列表, "食品");
        }

        /// <summary>
        /// 冷链物流分类DataGridView事件处理
        /// </summary>
        private void DataGridView_冷链物流_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            处理技术资料操作(sender, e, 冷链物流资料列表, "冷链物流");
        }

        /// <summary>
        /// 食品分类DataGridView事件处理
        /// </summary>
        private void DataGridView_制冷_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            处理技术资料操作(sender, e, 制冷资料列表, "制冷");
        }

        #endregion

        #region 文件离线上传

        #region 必要字段
        // 在类的字段部分添加以下字段

        /// <summary>
        /// 当前项目技术资料本地存储路径
        /// </summary>
        private string 项目技术资料存储路径 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DMC", "ProjectArchive", "TechnicalData");

        /// <summary>
        /// 当前项目技术资料记录文件路径
        /// </summary>
        private string 当前项目技术资料记录文件 => Path.Combine(项目技术资料存储路径, $"{projectId}_technical.json");

        /// <summary>
        /// 当前项目ID的附件记录文件路径
        /// </summary>
        private string 当前项目附件记录文件 => Path.Combine(附件记录存储路径, $"{projectId}_attachments.json");

        /// <summary>
        /// 当前项目的技术资料记录
        /// </summary>
        private List<本地技术资料记录> 当前项目技术资料列表 = new List<本地技术资料记录>();

        /// <summary>
        /// 上传附件记录的临时存储路径
        /// </summary>
        private string 附件记录存储路径 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "DMC", "ProjectArchive", "Attachments");

        /// <summary>
        /// 已上传附件的记录列表
        /// </summary>
        private List<UploadedAttachmentModel> 已上传审图附件列表 = new List<UploadedAttachmentModel>();

        /// <summary>
        /// 审图版图纸附件记录（文件夹结构）
        /// </summary>
        private List<DirectoryStructureModel> 审图版图纸附件列表 = new List<DirectoryStructureModel>();

        /// <summary>
        /// 本地技术资料记录模型
        /// </summary>
        public class 本地技术资料记录
        {
            public string 技术资料Id { get; set; }
            public string 原始文件路径 { get; set; }
            public string 本地存储路径 { get; set; }
            public string 文件名 { get; set; }
            public DateTime 上传时间 { get; set; }
            public long 文件大小 { get; set; }
            public string 分类 { get; set; } // 添加分类信息
        }

        /// <summary>
        /// 附件上传记录模型
        /// </summary>
        public class UploadedAttachmentModel
        {
            /// <summary>
            /// 文件原始路径
            /// </summary>
            public string OriginalPath { get; set; }

            /// <summary>
            /// 文件名
            /// </summary>
            public string FileName { get; set; }

            /// <summary>
            /// 临时存储路径
            /// </summary>
            public string TempPath { get; set; }

            /// <summary>
            /// 文件大小
            /// </summary>
            public long FileSize { get; set; }

            /// <summary>
            /// 上传时间
            /// </summary>
            public DateTime UploadTime { get; set; }

            /// <summary>
            /// 文件类型
            /// </summary>
            public string FileType { get; set; }

            /// <summary>
            /// 文件分类（用于区分不同用途的附件）
            /// </summary>
            public string Category { get; set; }
        }
        #endregion

        #region 文件离线上传方法

        /// <summary>
        /// 保存审图版图纸与审图意见附件上传记录到本地文件
        /// </summary>
        private void 保存审图版图纸附件记录()
        {
            try
            {
                // 确保存储目录存在
                if (!Directory.Exists(附件记录存储路径))
                {
                    Directory.CreateDirectory(附件记录存储路径);
                }
                // 过滤掉无效的数据
                var validAttachments = 审图版图纸附件列表.Where(item =>
                    !string.IsNullOrEmpty(item.PrimaryKey) &&
                    !string.IsNullOrEmpty(item.Name)).ToList();
                // 创建记录对象
                var 审图附件记录 = new 审图附件记录模型
                {
                    ProjectId = projectId,
                    上传时间 = DateTime.Now,
                    审图附件列表 = 已上传审图附件列表,
                    审图图纸列表 = validAttachments // 保存过滤后的有效数据
                };
                // 序列化并保存到文件
                string json = JsonConvert.SerializeObject(审图附件记录, Formatting.Indented);
                File.WriteAllText(当前项目附件记录文件, json);
                LogHelper.WriteLocalLog(this, $"附件记录已保存到: {当前项目附件记录文件}，共 {validAttachments.Count} 项");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存附件记录时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalErrorLog(this, ex, "保存附件记录时出错:");
            }
        }

        /// <summary>
        /// 从本地文件加载附件记录
        /// </summary>
        private void 加载审图附件记录()
        {
            try
            {
                // 检查记录文件是否存在
                if (File.Exists(当前项目附件记录文件))
                {
                    // 读取文件内容
                    string json = File.ReadAllText(当前项目附件记录文件);
                    // 反序列化
                    var 审图附件记录 = JsonConvert.DeserializeObject<审图附件记录模型>(json);
                    if (审图附件记录 != null && 审图附件记录.ProjectId == projectId)
                    {
                        // 恢复附件列表
                        已上传审图附件列表 = 审图附件记录.审图附件列表 ?? new List<UploadedAttachmentModel>();
                        审图版图纸附件列表 = 审图附件记录.审图图纸列表 ?? new List<DirectoryStructureModel>();

                        // 恢复审图版图纸附件显示
                        恢复审图版图纸显示();
                        LogHelper.WriteLocalLog(this, $"成功加载 {已上传审图附件列表.Count} 个附件记录");
                    }
                }
                else
                {
                    LogHelper.WriteLocalLog(this, "未找到附件记录文件，使用空列表");
                    已上传审图附件列表 = new List<UploadedAttachmentModel>();
                    审图版图纸附件列表 = new List<DirectoryStructureModel>();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"加载附件记录时出错: {ex.Message}");
                // 出错时使用空列表
                已上传审图附件列表 = new List<UploadedAttachmentModel>();
                审图版图纸附件列表 = new List<DirectoryStructureModel>();
            }
        }

        /// <summary>
        /// 附件记录模型（用于序列化）
        /// </summary>
        private class 审图附件记录模型
        {
            public string ProjectId { get; set; }
            public DateTime 上传时间 { get; set; }
            public List<UploadedAttachmentModel> 审图附件列表 { get; set; }
            public List<DirectoryStructureModel> 审图图纸列表 { get; set; }
        }

        #endregion

        #region 显示已上传附件


        /// <summary>
        /// 恢复审图版图纸显示
        /// </summary>
        private void 恢复审图版图纸显示()
        {
            try
            {
                if (审图版图纸附件列表 != null && 审图版图纸附件列表.Any())
                {
                    treeView审图版图纸树.Nodes.Clear();

                    // 重新构建树形结构
                    重建附件树形结构(审图版图纸附件列表);

                    treeView审图版图纸树.ExpandAll();

                    LogHelper.WriteLocalLog(this, $"恢复了 {审图版图纸附件列表.Count(f => f.Type == 2)} 个文件和 {审图版图纸附件列表.Count(f => f.Type == 1)} 个文件夹");
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"恢复审图版图纸显示时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 重建附件树形结构
        /// </summary>
        private void 重建附件树形结构(List<DirectoryStructureModel> fileList)
        {
            try
            {
                if (fileList == null || !fileList.Any())
                    return;

                // 创建节点字典
                var nodeDictionary = new Dictionary<string, TreeNode>();

                // 先创建所有节点
                foreach (var fileItem in fileList)
                {
                    TreeNode node = new TreeNode();
                    // 根据类型设置显示文本
                    if (fileItem.Type == 1) // 文件夹
                    {
                        node.Text = fileItem.Name ?? "未命名文件夹";
                    }
                    else // 文件
                    {
                        node.Text = Path.GetFileName(fileItem.Name) ?? fileItem.Name ?? "未命名文件";
                    }
                    node.Tag = fileItem;

                    // 使用PrimaryKey作为字典键
                    if (!string.IsNullOrEmpty(fileItem.PrimaryKey))
                    {
                        nodeDictionary[fileItem.PrimaryKey] = node;
                    }
                }

                // 建立父子关系
                foreach (var fileItem in fileList)
                {
                    if (string.IsNullOrEmpty(fileItem.ParentId) || fileItem.ParentId == "0" || fileItem.ParentId == "")
                    {
                        // 根节点（没有父级ID或父级ID为空）
                        if (!string.IsNullOrEmpty(fileItem.PrimaryKey) && nodeDictionary.ContainsKey(fileItem.PrimaryKey))
                        {
                            treeView审图版图纸树.Nodes.Add(nodeDictionary[fileItem.PrimaryKey]);
                        }
                    }
                    else
                    {
                        // 子节点
                        if (!string.IsNullOrEmpty(fileItem.PrimaryKey) &&
                            nodeDictionary.ContainsKey(fileItem.PrimaryKey) &&
                            nodeDictionary.ContainsKey(fileItem.ParentId))
                        {
                            // 将当前节点添加到父节点下
                            nodeDictionary[fileItem.ParentId].Nodes.Add(nodeDictionary[fileItem.PrimaryKey]);
                        }
                        else if (!string.IsNullOrEmpty(fileItem.PrimaryKey) && nodeDictionary.ContainsKey(fileItem.PrimaryKey))
                        {
                            // 如果找不到父节点，则作为根节点添加
                            treeView审图版图纸树.Nodes.Add(nodeDictionary[fileItem.PrimaryKey]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"重建附件树形结构时出错: {ex.Message}");
            }
        }

        #endregion

        #region 审图版图纸上传附件（修改版本）

        /// <summary>
        /// 审图版图纸上传附件（修改版本）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn上传附件_Click(object sender, EventArgs e)
        {
            try
            {
                // 选择文件夹
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择包含附件文件的文件夹";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    // 清空现有树形结构
                    treeView审图版图纸树.Nodes.Clear();

                    // 如果之前有记录，询问是否要清除
                    if (审图版图纸附件列表.Any())
                    {
                        var result = MessageBox.Show("是否要替换现有的附件？点击'是'将清除现有附件并添加新附件，点击'否'将在现有附件基础上添加。",
                            "确认操作", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                        if (result == DialogResult.Cancel)
                        {
                            return; // 取消操作
                        }
                        else if (result == DialogResult.Yes)
                        {
                            // 清除现有记录
                            审图版图纸附件列表.Clear();
                            treeView审图版图纸树.Nodes.Clear();
                        }
                        // 如果选择否，则在现有基础上添加
                    }

                    // 处理选中的文件夹
                    var selectedDirectory = new DirectoryInfo(dialog.SelectedPath);
                    ListTreeShowWithRecord(selectedDirectory, "", null);

                    // 展开所有节点
                    treeView审图版图纸树.ExpandAll();

                    // 保存记录
                    保存审图版图纸附件记录();

                    MessageBox.Show($"成功添加 {审图版图纸附件列表.Count(f => f.Type == 2)} 个文件和 {审图版图纸附件列表.Count(f => f.Type == 1)} 个文件夹",
                        "上传成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"上传附件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalLog(this, $"上传附件时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取指定文件夹下所有子目录及文件函数（带记录保存）
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">父级ID</param>
        /// <param name="node">树节点</param>
        public void ListTreeShowWithRecord(DirectoryInfo theDir, string nLevel, TreeNode node)
        {
            try
            {
                // 创建文件夹记录
                var dirModel = new DirectoryStructureModel();
                dirModel.ParentId = nLevel;
                dirModel.PrimaryKey = Guid.NewGuid().ToString();
                dirModel.Name = theDir.Name;
                dirModel.Type = 1; // 文件夹类型

                // 添加到记录列表
                审图版图纸附件列表.Add(dirModel);

                // 创建树节点
                TreeNode rootNode = new TreeNode();
                rootNode.Text = dirModel.Name;
                rootNode.Tag = dirModel;

                if (string.IsNullOrWhiteSpace(nLevel) || nLevel == "0")
                {
                    treeView审图版图纸树.Nodes.Add(rootNode);
                }
                else
                {
                    node?.Nodes.Add(rootNode);
                }

                // 处理目录下的文件
                FileInfo[] fileInfo = theDir.GetFiles();
                foreach (FileInfo fInfo in fileInfo)
                {
                    try
                    {
                        var fileModel = new DirectoryStructureModel();
                        fileModel.ParentId = dirModel.PrimaryKey; // 父级是当前文件夹的ID
                        fileModel.Name = fInfo.FullName;
                        fileModel.Type = 2; // 文件类型
                        fileModel.PrimaryKey = Guid.NewGuid().ToString(); // 为文件也生成唯一ID

                        // 添加到记录列表
                        审图版图纸附件列表.Add(fileModel);

                        // 创建文件树节点
                        TreeNode fileNode = new TreeNode();
                        fileNode.Text = fInfo.Name; // 显示文件名而不是完整路径
                        fileNode.Tag = fileModel;
                        rootNode.Nodes.Add(fileNode);
                    }
                    catch (Exception fileEx)
                    {
                        LogHelper.WriteLocalLog(this, $"处理文件 {fInfo.FullName} 时出错: {fileEx.Message}");
                    }
                }

                // 递归处理子目录
                DirectoryInfo[] subDirectories = theDir.GetDirectories();
                foreach (DirectoryInfo dirinfo in subDirectories)
                {
                    ListTreeShowWithRecord(dirinfo, dirModel.PrimaryKey, rootNode);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"处理目录 {theDir.FullName} 时出错: {ex.Message}");
            }
        }

        #endregion


        #region 修改技术资料加载方法，使用项目专属的记录

        /// <summary>
        /// 加载技术资料基础信息
        /// </summary>
        /// <param name="getKeepTechnicalNameLists">返回数据库中的DataGridView的基础数据</param>
        private void 加载技术资料基础数据(ref List<GetKeepTechnicalNameListModel> getKeepTechnicalNameLists)
        {
            try
            {
                var resultTechnicalData = new List<GetKeepTechnicalNameListModel>();// 设定一个项目下的技术资料列表的变量
                if (HttpGet(AppGlobalModel.GetKeepTechnicalNameList + $"?proId={projectId}", ref resultTechnicalData))// 服务器上获取项目下的技术资料列表
                {
                    var technicalInfoValue = ConfigurationManager.AppSettings["TechnicalInfo"];// 从配置文件中获取项目下的技术资料列表

                    if (!string.IsNullOrWhiteSpace(technicalInfoValue))
                    {
                        var loadTechnicalInfoList = JsonConvert.DeserializeObject<List<GetKeepTechnicalNameListModel>>(technicalInfoValue);

                        foreach (var item in loadTechnicalInfoList)
                        {
                            if (resultTechnicalData.Exists(o => o.id == item.id))
                            {
                                var dataInfo = resultTechnicalData.FirstOrDefault(o => o.id == item.id);
                                if (string.IsNullOrWhiteSpace(dataInfo.localFile))
                                {
                                    dataInfo.localFile = item.localFile;
                                }
                                else
                                {
                                    resultTechnicalData.Add(new GetKeepTechnicalNameListModel()
                                    {
                                        id = item.id,
                                        localFile = item.localFile,
                                        must = dataInfo.must,
                                        sort = dataInfo.sort,
                                        name = dataInfo.name
                                    });
                                }
                            }
                        }
                    }

                    // 使用灵活版筛选并分配到不同分类（支持多分类）
                    // 初始化筛选分配技术资料列表(resultTechnicalData);

                    // 应用本地记录到技术资料列表（关键步骤）
                    //应用本地技术资料记录(resultTechnicalData);
                }
                getKeepTechnicalNameLists = resultTechnicalData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载技术资料时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                getKeepTechnicalNameLists = null;
            }
        }


        /// <summary>
        /// 加载当前项目之前保存的技术资料记录
        /// </summary>
        private void 加载技术资料缓存记录()
        {
            try
            {
                #region 加载项目缓存在本地的技术资料记录文件
                // 确保存储目录存在
                if (!Directory.Exists(项目技术资料存储路径))
                {
                    Directory.CreateDirectory(项目技术资料存储路径);
                }

                // 检查当前项目的技术资料记录文件是否存在
                if (File.Exists(当前项目技术资料记录文件))
                {
                    string json = File.ReadAllText(当前项目技术资料记录文件);
                    var 记录 = JsonConvert.DeserializeObject<技术资料记录模型>(json);

                    if (记录 != null && 记录.ProjectId == projectId)
                    {
                        当前项目技术资料列表 = 记录.技术资料列表 ?? new List<本地技术资料记录>();
                        LogHelper.WriteLocalLog(this, $"成功加载项目 {projectId} 的 {当前项目技术资料列表.Count} 个技术资料记录");
                        LogHelper.WriteLocalLog(this, $"成功加载项目 {projectId} 的 {当前项目技术资料列表.Count} 个技术资料记录");
                    }
                    else
                    {
                        当前项目技术资料列表 = new List<本地技术资料记录>();
                        LogHelper.WriteLocalLog(this, "技术资料记录项目ID不匹配，使用空列表");
                    }
                }
                else
                {
                    当前项目技术资料列表 = new List<本地技术资料记录>();
                    LogHelper.WriteLocalLog(this, "未找到技术资料记录文件，使用空列表");
                }
                //更新资料表分类显示();
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"加载项目技术资料记录时出错: {ex.Message}");
                当前项目技术资料列表 = new List<本地技术资料记录>();
                LogHelper.WriteLocalErrorLog(this, ex, "加载项目技术资料记录时出错:");
            }
        }


        /// <summary>
        /// 保存当前项目的技术资料记录
        /// </summary>
        private void 保存项目技术资料记录()
        {
            try
            {
                // 确保存储目录存在
                if (!Directory.Exists(项目技术资料存储路径))
                {
                    Directory.CreateDirectory(项目技术资料存储路径);
                }
                // 创建记录对象
                var 技术资料记录 = new 技术资料记录模型
                {
                    ProjectId = projectId,
                    SaveTime = DateTime.Now,
                    技术资料列表 = 当前项目技术资料列表
                };
                // 序列化并保存
                string json = JsonConvert.SerializeObject(技术资料记录, Formatting.Indented);
                File.WriteAllText(当前项目技术资料记录文件, json);
                LogHelper.WriteLocalLog(this, $"项目 {projectId} 的技术资料记录已保存");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"保存项目技术资料记录时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 技术资料记录模型
        /// </summary>
        private class 技术资料记录模型
        {
            public string ProjectId { get; set; }
            public DateTime SaveTime { get; set; }
            public string major { get; set; }
            public string localFilePath { get; set; }
            public List<本地技术资料记录> 技术资料列表 { get; set; }
        }

        #endregion



        #region 清理

        /// <summary>
        /// 清理项目相关的临时文件
        /// </summary>
        private void 清理项目临时文件()
        {
            try
            {
                // 清理项目技术资料
                string 项目资料目录 = Path.Combine(项目技术资料存储路径, projectId);
                if (Directory.Exists(项目资料目录))
                {
                    Directory.Delete(项目资料目录, true);
                }

                // 清理项目附件记录文件
                if (File.Exists(当前项目技术资料记录文件))
                {
                    File.Delete(当前项目技术资料记录文件);
                }

                // 清空内存中的记录
                当前项目技术资料列表.Clear();

                LogHelper.WriteLocalLog(this, $"项目 {projectId} 的临时文件已清理");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"清理项目临时文件时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理所有上传文件记录与本地临时文件
        /// </summary>
        private void 清理上传文件()
        {
            try
            {
                // 显示确认对话框
                DialogResult result = MessageBox.Show(
                    "确定要清理所有上传的文件记录和本地临时文件吗？\n此操作将删除所有已上传的本地文件，且无法恢复。",
                    "确认清理",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);

                if (result == DialogResult.No)
                {
                    return; // 用户取消操作
                }

                // 1. 清理技术资料记录
                清理技术资料记录();

                // 2. 清理附件记录
                清理附件记录();

                // 3. 清理项目技术资料临时文件
                清理项目技术资料临时文件();

                // 4. 清理附件临时文件
                清理附件临时文件();

                // 5. 更新界面显示
                更新所有数据显示();

                MessageBox.Show("文件清理完成！", "清理成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LogHelper.WriteLocalLog(this, "文件清理操作完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"清理文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalLog(this, $"清理上传文件时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理技术资料记录
        /// </summary>
        private void 清理技术资料记录()
        {
            try
            {
                // 清空内存中的技术资料列表
                建筑资料列表.Clear();
                石油资料列表.Clear();
                农产品资料列表.Clear();
                食品资料列表.Clear();

                // 清空当前项目技术资料列表
                当前项目技术资料列表.Clear();

                // 删除技术资料记录文件
                if (File.Exists(当前项目技术资料记录文件))
                {
                    File.Delete(当前项目技术资料记录文件);
                }

                // 清空配置中的技术资料信息
                ConfigHelper.SaveConfigInfo("TechnicalInfo", "");

                LogHelper.WriteLocalLog(this, "技术资料记录已清理");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"清理技术资料记录时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理附件记录
        /// </summary>
        private void 清理附件记录()
        {
            try
            {
                // 清空内存中的附件列表
                已上传审图附件列表.Clear();
                审图版图纸附件列表.Clear();

                // 删除附件记录文件
                if (File.Exists(当前项目附件记录文件))
                {
                    File.Delete(当前项目附件记录文件);
                }

                LogHelper.WriteLocalLog(this, "附件记录已清理");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"清理附件记录时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理项目技术资料临时文件
        /// </summary>
        private void 清理项目技术资料临时文件()
        {
            try
            {
                // 删除项目技术资料目录
                string 项目资料目录 = Path.Combine(项目技术资料存储路径, projectId);
                if (Directory.Exists(项目资料目录))
                {
                    Directory.Delete(项目资料目录, true);
                }

                // 删除技术资料根目录下的相关文件（如果为空则删除）
                if (Directory.Exists(AppGlobalModel.TechnicalInfoUrl))
                {
                    try
                    {
                        // 只删除当前项目的目录
                        string 当前项目技术目录 = Path.Combine(AppGlobalModel.TechnicalInfoUrl, projectId);
                        if (Directory.Exists(当前项目技术目录))
                        {
                            Directory.Delete(当前项目技术目录, true);
                        }
                    }
                    catch (Exception dirEx)
                    {
                        LogHelper.WriteLocalLog(this, $"删除技术资料目录时出错: {dirEx.Message}");
                    }
                }

                LogHelper.WriteLocalLog(this, "项目技术资料临时文件已清理");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"清理项目技术资料临时文件时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 清理附件临时文件
        /// </summary>
        private void 清理附件临时文件()
        {
            try
            {
                // 删除附件临时文件目录
                string 附件临时目录 = Path.Combine(附件记录存储路径, "TempFiles");
                if (Directory.Exists(附件临时目录))
                {
                    Directory.Delete(附件临时目录, true);
                }

                // 删除审图版图纸临时文件
                string 审图图纸临时目录 = Path.Combine(附件记录存储路径, projectId);
                if (Directory.Exists(审图图纸临时目录))
                {
                    Directory.Delete(审图图纸临时目录, true);
                }

                LogHelper.WriteLocalLog(this, "附件临时文件已清理");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"清理附件临时文件时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新所有数据的界面显示
        /// </summary>
        private void 更新所有数据显示()
        {
            try
            {
                //加载技术资料基础数据();
                //加载项目技术资料记录();

                // 清空附件树形显示
                if (treeView审图版图纸树 != null)
                {
                    treeView审图版图纸树.Nodes.Clear();
                }
                LogHelper.WriteLocalLog(this, "所有数据显示已更新");
            }
            catch (Exception ex)
            {
                LogHelper.WriteLocalLog(this, $"更新数据显示时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 回滚删除补充文件
        /// </summary>
        /// <param name="fileId">要删除的文件ID（可能是临时ID或文件名）</param>
        /// <returns>是否删除成功</returns>
        private bool RollbackDeleteSupplementaryFile(string fileId)
        {
            try
            {
                // 首先验证fileId是否是真实的数据库ID
                string realFileId = fileId;

                realFileId = SQLiteDataBase.GetRealFileIdFromDatabase(fileId);
                if (string.IsNullOrEmpty(realFileId))
                {
                    Console.WriteLine($"无法找到文件的真实ID: {fileId}");
                    return false;
                }

                // 使用真实的文件ID进行删除
                var para = new
                {
                    projectFileId = realFileId,//获取是不是管理员的删除权限；
                    isDel = true
                };
                var resultData = new object(); // 根据您的API返回类型调整

                // 使用当前窗体的HttpPost方法（继承自BaseForm）
                bool result = this.HttpPost(AppGlobalModel.DelProjectFile, para, ref resultData);

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除补充文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 发起项目归档（修改版本 - 清理临时文件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button发起归档_Click(object sender, EventArgs e)
        {
            try
            {
                保存项目技术资料记录();
                // 获取数据
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView项目技术资料归档表.DataSource);
                // 从全局变量获取项目信息
                var selectProjectInfo = FrmProjectFile.treeNodeSelectProjectInto;
                // 创建参数对象
                var paraInfo = new AddKeepProjectAttributeModel()
                {
                    oneStartTime = dateTimePicker1.Text.ToString(),
                    oneEndTime = dateTimePicker2.Text.ToString(),
                    twoStartTime = dateTimePicker4.Text.ToString(),
                    twoEndTime = dateTimePicker3.Text.ToString(),
                    threeStartTime = dateTimePicker6.Text.ToString(),
                    threeEndTime = dateTimePicker5.Text.ToString(),
                    fourStartTime = dateTimePicker8.Text.ToString(),
                    fourEndTime = dateTimePicker7.Text.ToString(),
                    projectId = projectId,
                    other = "",
                    remarks = textBox28.Text.Trim()
                };
                // 查询结果为0时，执行补充文件上传逻辑
                try
                {
                    // 检查项目信息是否有效
                    if (selectProjectInfo != null && selectProjectInfo.Any() && !string.IsNullOrEmpty(selectProjectInfo[0].id))
                    {
                        // 上传补充文件（增强异常记录）
                        string uploadedFileId = null;
                        try
                        {
                            uploadedFileId = FrmUploadFile.UploadSupplementaryFileForProject(selectProjectInfo[0].id);
                        }
                        catch (Exception exUpload)
                        {
                            // 记录完整异常用于定位：包含 InnerException 和 StackTrace
                            LogHelper.WriteLocalErrorLog(this, exUpload, "上传补充文件失败（详细）");
                            MessageBox.Show($"上传补充文件时发生错误: {exUpload.Message}\n\n详细信息已记录到本地日志，请将日志提供给运维或开发人员。",
                                "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // 继续让上层流程按需处理（不抛出以免阻塞清理）
                        }
                        if (!string.IsNullOrEmpty(uploadedFileId))
                        {
                            // 将上传的文件ID保存到窗体变量中，以便后续删除
                            UploadedSupplementaryFileId = uploadedFileId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"上传补充文件时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // 创建FrmArchiveProgress对象
                FrmArchiveProgress frm = new FrmArchiveProgress(LocalFileList, 审图版图纸附件列表, paraInfo);
                /// 显示进度窗口
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    #region 获取项目属性信息
                    // 获取项目属性信息
                    var resultProjectData = new GetProjectAttributeModel();
                    // 使用当前窗体的HttpGet方法（继承自BaseForm）
                    if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={projectId}", ref resultProjectData))
                    {
                        // 创建FrmInitApproval对象
                        var frmApproval = new FrmInitApproval(resultProjectData, 2, resultProjectData.id, 0, 0, frm.resultDataModel);
                        // 清空保存的技术资料记录
                        保存项目技术资料记录();
                        this.Hide();
                        if (frmApproval.ShowDialog() == DialogResult.OK)
                        {
                            this.Close();
                            // 清空附件记录
                            清理项目临时文件();
                        }
                        else
                        {
                            this.Show();
                        }
                    }
                    #endregion
                }              
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发起归档时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalLog(this, $"发起项目归档时出错: {ex.Message}");
            }
        }

        #endregion


        #endregion

        private void btn_保存_Click(object sender, EventArgs e)
        {
            //保存审图版图纸与审图意见附件记录
            保存审图版图纸附件记录();
            //保存项目技术资料记录
            保存项目技术资料记录();
        }

        private void button清理上传文件_Click(object sender, EventArgs e)
        {
            try
            {
                // 执行清理操作
                清理上传文件();
                var resulMySqlData = new List<GetKeepTechnicalNameListModel>();
                加载技术资料基础数据(ref resulMySqlData);

                初始化筛选分配技术资料列表(resulMySqlData);

                LocalFileList.Clear();

                加载技术资料缓存记录();//加载本地记录:当前项目技术资料列表  这个变量会有内容

                应用本地记录到分类列表();
                // 更新各分类的DataGridView显示
                更新资料表分类显示();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"执行清理操作时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogHelper.WriteLocalLog(this, $"清理上传文件按钮点击事件出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 按键的状态
        /// </summary>
        private bool btnStatus = false;

        private void tabControl_主Tab_Selected(object sender, TabControlEventArgs e)
        {
            if (tabControl_主Tab.SelectedTab.Text == "施工图")
            {
                if (!btnStatus)
                {
                    btnStatus = true;
                    // 加载施工图文件列表
                    Splasher.Show(typeof(FrmLoading));
                    LoadFileList();
                    Splasher.Close();
                }
            }

        }

    }
}
