using DMC.Helper;
using DMC.Models;
using DMC.MyControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 发起审批
    /// </summary>
    public partial class FrmInitApproval : BaseForm
    {
        //项目属性
        private GetProjectAttributeModel projectAttribute = null;
        //流程信息
        private ApprovalInfoModel approvalInfo = null;
        //type发起类型 0购物车 1文件夹 2项目 3文件
        private int type = 0;
        //文件来源0 项目区 1归档区
        private int fileType = 0;
        //文件id列表
        private string fileIds = null;
        //页数
        private int pageAll = 0;
        //选择的签章
        private List<SeallistOfZhuzhiModel> selectSealList = null;
        //文件列表查询条件
        private QueryApprovalProjectStructure queryInfo = null;
        //项目归档使用
        private string guiId = "";

        /// <summary>
        /// 发起流程中的构造函数/"objInfo"项目属性信息/"objType"发起类型 0购物车 1文件夹 2项目 3文件/"objFileIds">文件id列表/"objFileType"文件来源0 项目区 1归档区/"objPage"/页数/"objTempAttributeId">项目归档使用
        /// </summary>
        /// <param name="objInfo">项目属性信息</param>
        /// <param name="objType">发起类型 0购物车 1文件夹 2项目 3文件</param>
        /// <param name="objFileIds">文件id列表  用  ，分割</param>
        /// <param name="objFileType">文件来源0 项目区 1归档区</param>
        /// <param name="objPage">页数</param>
        /// <param name="objTempAttributeId">项目归档使用</param>
        public FrmInitApproval(GetProjectAttributeModel objInfo, int objType, string objFileIds, int objFileType, int objPage = 0, string objTempAttributeId = null)
        {
            InitializeComponent();

            projectAttribute = objInfo;
            type = objType;
            fileIds = objFileIds;
            fileType = objFileType;
            pageAll = objPage;
            guiId = objTempAttributeId;
            // 初始化专业用户映射关系加载状态标志为false，确保在需要使用专业用户映射关系时能够正确加载数据
            majorUserMapLoaded = false;
            queryInfo = new QueryApprovalProjectStructure()
            {
                fileType = fileType, //文件来源0 项目区 1归档区
                type = type,    //发起类型 0购物车 1文件夹 2项目 3文件
                fileIds = objFileIds,  //流id列表  用  ，分割
                parentId = "0",  //上级ID
                tab = "1"
            };
        }

        #region 拉申窗口方法

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
        /// 加载 流程类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmInitApproval_Load(object sender, EventArgs e)
        {
            //加载流程类型
            #region 加载流程类型
            //流程类型 0签名 5签章 1出版 2下载 3归档 4其他 6签名签章processtype_id
            var resultApprovalData = new List<ApprovalListModel>();
            // 判断是不是成功获取到流程列表,成功则返回列表数据resultApprovalData,失败则关闭窗口
            if (HttpGet(AppGlobalModel.ApprovalList, ref resultApprovalData))
            {
                //判断项目是否归档
                int resultData = 1;
                //判断项目是否归档,0已归档 1未归档成功则返回项目归档状态resultData,失败则关闭窗口
                if (HttpGet(AppGlobalModel.GetProjectIsKeep + $"?projectId={projectAttribute.id}", ref resultData))
                {
                    // 在列表中添加项目
                    resultApprovalData.Insert(0, new ApprovalListModel() { id = "0", name = "请选择" });
                    // 判断项目是否归档如果,是项目归档发起，且guiId为空，则过滤掉归档流程；如果guiId不为空，则只显示归档流程
                    if (string.IsNullOrWhiteSpace(guiId))
                    {
                        // 项目未归档
                        if (resultData == 1)
                        {
                            // 过滤掉归档流程
                            resultApprovalData = resultApprovalData.Where(o => o.processtypeId != "3").ToList();
                        }
                    }
                    else
                    {
                        // 项目已归档
                        resultApprovalData = resultApprovalData.Where(o => o.processtypeId == "3").ToList();
                    }
                    // 设置数据源
                    comboBox_流程类型.DataSource = resultApprovalData;
                    comboBox_流程类型.DisplayMember = "name";// 显示字段
                    comboBox_流程类型.ValueMember = "id";// 值字段
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
            #endregion
            //项目编号
            textBox_项目名称.Text = projectAttribute.identifier + '-'+ projectAttribute.name;
            textBox_提交用户.Text = AppGlobalModel.UseInfo.realName;// 创建人
            textBox_用户部门.Text = AppGlobalModel.UseInfo.deptName;// 部门名称
        }

        /// <summary>
        /// 流程类型选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_流程类型_SelectedIndexChanged(object sender, EventArgs e)
        {
            //流程列表选择的项；
            var selectModel = (ApprovalListModel)comboBox_流程类型.SelectedItem;

            //出版份数
            label_出版份数.Visible = false;
            textBox_出版份数.Visible = false;
            //代签页码
            label_多页码选选择.Visible = false;
            label_多面码提示.Visible = false;
            comCheckBoxList1.Visible = false;
            //清空配置流程
            tabPage_流程配置.Controls.Clear();
            //清空选择的签章
            selectSealList = new List<SeallistOfZhuzhiModel>();

            if (selectModel.id != "0")
            {
                //获取流程信息
                if (HttpGet(AppGlobalModel.ApprovalInfo + $"?id={selectModel.id}&proId={projectAttribute.id}", ref approvalInfo))// 判断是否成功获取到流程信息,成功则返回流程信息approvalInfo,失败则显示错误信息并返回
                {
                    //流程类型 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章processtypeid
                    //0签名
                    if (selectModel.processtypeId == "0")
                    {
                        LoadSignProcess(tabPage_流程配置);
                    }
                    //1出版 
                    else if (selectModel.processtypeId == "1")
                    {

                        #region 出版
                        //出版份数
                        textBox_出版份数.Visible = true;
                        label_出版份数.Visible = true;
                        textBox_流程标题.Text = comboBox_流程类型.Text;
                        Panel panel;
                        TextBox textBox;
                        CheckBox checkBox;

                        var index = 0;
                        foreach (var item in approvalInfo.qzApprovalNodeList.OrderByDescending(o => o.sort))
                        {
                            panel = new Panel();
                            textBox = new TextBox();
                            checkBox = new CheckBox();

                            textBox.Location = new Point(24, 30);
                            textBox.Multiline = true;
                            textBox.Size = new Size(734, 84);
                            textBox.Name = $"textBox_{item.name}";
                            textBox.Enabled = false;

                            checkBox.AutoSize = true;
                            checkBox.Location = new Point(24, 3);
                            checkBox.Size = new Size(110, 25);
                            checkBox.Name = $"checkBox_{item.name}";
                            checkBox.Text = $"{item.name}";
                            checkBox.UseVisualStyleBackColor = true;
                            checkBox.Tag = item;
                            checkBox.CheckState = CheckState.Checked;
                            //checkBox.CheckedChanged += CheckBox_CheckedChanged;

                            panel.Controls.Add(textBox);
                            panel.Controls.Add(checkBox);
                            panel.Dock = DockStyle.Top;
                            panel.Location = new Point(3, (index * 125) + 3);
                            panel.Size = new Size(786, 125);

                            tabPage_流程配置.Controls.Add(panel);

                            index++;
                        }
                        #endregion
                    }
                    //2下载
                    else if (selectModel.processtypeId == "2")
                    {
                        textBox_流程标题.Text = comboBox_流程类型.Text;
                        LoadOtherProcess();
                    }
                    //3归档
                    else if (selectModel.processtypeId == "3")
                    {
                        textBox_流程标题.Text = comboBox_流程类型.Text;
                        LoadOtherProcess();
                    }
                    //4其他
                    else if (selectModel.processtypeId == "4")
                    {
                        textBox_流程标题.Text = comboBox_流程类型.Text;
                        LoadOtherProcess();
                    }
                    //5签章
                    else if (selectModel.processtypeId == "5")
                    {
                        textBox_流程标题.Text = comboBox_流程类型.Text;
                        LoadSignatureProcess(tabPage_流程配置);
                    }
                    //6签名签章
                    else if (selectModel.processtypeId == "6")
                    {
                        textBox_流程标题.Text = comboBox_流程类型.Text;
                        #region 签名签章
                        //初始化签章TabControl
                        var tabControl = new TabControl();
                        //初始化签名子页面TabPage
                        var tabPage_sign = new TabPage();
                        //初始化选章子页面TabPage
                        var tabPage_signature = new TabPage();

                        tabPage_sign.AutoScroll = true;
                        tabPage_sign.Location = new Point(4, 30);
                        tabPage_sign.Padding = new Padding(3);
                        tabPage_sign.Size = new Size(778, 320);
                        tabPage_sign.UseVisualStyleBackColor = true;
                        tabPage_sign.Text = "签名";


                        tabPage_signature.AutoScroll = true;
                        tabPage_signature.Location = new Point(4, 30);
                        tabPage_signature.Padding = new Padding(3);
                        tabPage_signature.Size = new Size(778, 320);
                        tabPage_signature.UseVisualStyleBackColor = true;
                        tabPage_signature.Text = "签章";

                        tabControl.Controls.Add(tabPage_sign);
                        tabControl.Controls.Add(tabPage_signature);
                        tabControl.Dock = DockStyle.Fill;
                        tabControl.Location = new Point(3, 3);
                        tabControl.SelectedIndex = 0;
                        tabControl.Size = new Size(786, 354);

                        //加载签名子页面
                        LoadSignProcess(tabPage_sign);
                        //加载签章子页面
                        LoadSignatureProcess(tabPage_signature);
                        //在流程配置页面内加载TabControl
                        tabPage_流程配置.Controls.Add(tabControl);
                        #endregion
                    }
                    else
                    {
                        ShowErrorMsg("请选择正确流程类型！");
                    }

                    //如果是签名签章流程 //流程类型 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章processtypeid
                    if (selectModel.processtypeId == "0" || selectModel.processtypeId == "5" || selectModel.processtypeId == "6")
                    {
                        //文件发起，并且只有一个文件
                        if (type == 3 && !fileIds.Contains(",") && pageAll > 0)
                        {
                            label_多页码选选择.Visible = true;
                            label_多面码提示.Visible = true;
                            comCheckBoxList1.Visible = true;

                            comCheckBoxList1.Items.Clear();

                            for (var i = 1; i <= pageAll; i++)
                            {
                                comCheckBoxList1.Items.Add(i);
                                //comCheckBoxList1.SetItemDefaultValue(i);
                            }

                            comCheckBoxList1.SetItemDefaultValue(0);
                        }
                    }
                }
            }
            //如果是下载、出版流程
            if (selectModel.processtypeId != "1" && selectModel.processtypeId != "2")
            {
                ResetFileList("1");
            }
            else
            {
                ResetFileList("0");
            }
        }

        #region 签名
        /// <summary>
        /// 初始化签名流程页面
        /// </summary>
        /// <param name="control">要加载签名流程的tabPage控件</param>
        private void LoadSignProcess(Control control)
        {
            ///流程列表选择的项；
            var selectModel = (ApprovalListModel)comboBox_流程类型.SelectedItem;
            var index = 0;//索引
            // 加载专业用户映射关系数据，确保在需要使用专业用户映射关系时能够正确加载数据；
            EnsureMajorUserMap();
            //流程列表
            foreach (var item in approvalInfo.qzApprovalNodeList.OrderByDescending(o => o.sort))
            {
                //签名节点类型 0发起人 1签章节点 2审批节点 3抄送节点
                if (item.nodeType == 1 || item.nodeType == 3)
                {
                    if(selectModel.name == "专业会签")
                    {
                        // 从节点名称中解析专业名称，假设节点名称的格式为“XX会签人”、“XX签字人”、“XX审核人”等，其中“XX”部分即为专业名称；如果节点名称不符合上述格式，则返回整个节点名称作为专业名称
                        var majorName = ParseMajorNameFromNodeName(item.name);
                        // 根据节点名称中的专业名称，从专业用户映射关系中获取对应的用户列表，并自动勾选下拉框中的用户项；如果没有匹配的用户，则不进行自动勾选
                        var hitKey = majorUserMap.Keys.FirstOrDefault(k => k.Contains(majorName));
                        // 如果专业用户映射关系中不包含该专业名称，则直接返回，不进行自动勾选；如果包含但没有匹配的用户，则不进行自动勾选
                        if (hitKey == null || !majorUserMap.ContainsKey(hitKey)) continue;
                    }   

                    var panel = new Panel();//节点面板流程配置面板
                    var leftPanel = new Panel();//节点面板左侧流程配置面板
                    //节点名称如果签章列表为空，则初始化一个空列表，避免后续操作空指针异常
                    if (item.sealList == null)
                    {
                        item.sealList = new List<SealListItem>();//签章列表节点列表
                    }
                    //节点名称,签章列表控件
                    var comboBox = new ComCheckBoxList();
                    comboBox.Dock = DockStyle.Fill;//填充停靠方式填充
                    comboBox.Font = new Font("微软雅黑", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    comboBox.Location = new Point(178, 0);//节点名称控件位置
                    comboBox.Size = new Size(485, 33);//节点名称控件大小
                    comboBox.Name = $"comboBox_{item.name}";//组件名称
                    comboBox.Tag = item;//组件标签组件标签存储节点信息
                    comboBox.DataSource = item.sealList;//绑定数据源节点名称控件数据源绑定签章列表
                    comboBox.DisplayMember = "sealname";//节点名称控件数据源绑定签章列表显示名称
                    comboBox.ValueMember = "id";//节点名称控件数据源绑定签章列表值名称
                    comboBox.Enabled = false;//默认禁用节点名称控件

                    var checkBox = new CheckBox();//创建复选框节点选择控件
                    checkBox.AutoSize = false;//默认禁用复选框控件复选框自动大小设置为false
                    checkBox.Dock = DockStyle.Right;//设置复选框控件停靠位置复选框停靠方式右边停靠
                    checkBox.Location = new Point(0, 0);//设置复选框控件位置
                    checkBox.Size = new Size(170, 25);
                    checkBox.Name = $"checkBox_{item.name}";
                    checkBox.Text = $"{item.name}";
                    checkBox.UseVisualStyleBackColor = true;//使用系统默认样式
                    checkBox.Tag = item;//添加标签复选框组件标签存储节点信息
                    checkBox.CheckedChanged += new System.EventHandler(checkBoxSignProcess_CheckedChanged);//添加复选框控件事件

                    leftPanel.Controls.Add(checkBox);//添加复选框控件
                    leftPanel.Dock = DockStyle.Left;//设置复选框控件位置设置左侧面板停靠方式左边停靠
                    leftPanel.Location = new Point(0, 0);
                    leftPanel.Size = new Size(178, 34);
                    panel.Controls.Add(comboBox);//添加下拉框控件添加节点名称控件
                    panel.Controls.Add(leftPanel);//添加复选框控件添加左侧面板添加节点名称控件
                    panel.Dock = DockStyle.Top;
                    panel.Padding = new Padding(0, 10, 80, 10);
                    panel.Location = new Point(3, (index * 53) + 3);
                    panel.Size = new Size(663, 53);
                    control.Controls.Add(panel);//添加节点控件面板到流程配置页面

                    //节点类型，是3 不签得 默认全选////流程类型 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章processtype_id
                    if (item.nodeType == 3)
                    {
                        checkBox.Checked = true;//默认全选默认选中节点类型是3的节点
                        comboBox.Enabled = true;//默认启用启用节点名称控件
                        comboBox.SelectAllItems();//默认全选节点名称控件
                    }
                    index++;
                }
                else
                {
                    if (selectModel.processtypeId == "0")
                    {
                        ShowErrorMsg("节点类型不匹配，请联系后台管理员！");
                    }                   
                }
            }
        }
        /// <summary>
        /// 初始化签名流程页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxSignProcess_CheckedChanged(object sender, EventArgs e)
        {
            //获取选中的项
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == null) return;

            //获取签名列表 控件
            var controls = Controls.Find("comboBox_" + checkBox.Text, true);
            if (controls == null || controls.Length == 0) return;

            ComCheckBoxList comboBox = controls[0] as ComCheckBoxList;
            if (comboBox == null) return;

            //根据选中状态控制启用
            comboBox.Enabled = checkBox.Checked;
            if (!checkBox.Checked)
            {
                comboBox.ClearData();
                return;
            }

            // 1) 原有签名流程（章）默认勾选逻辑
            if (approvalInfo != null && approvalInfo.processtypeId == "0")
            {
                var nodeInfo = comboBox.Tag as QzApprovalNodeListItem;
                if (nodeInfo != null && nodeInfo.nodeType == 1 && nodeInfo.defaultSealList != null && nodeInfo.defaultSealList.Any())
                {
                    for (var i = 0; i < comboBox.Items.Count; i++)
                    {
                        var item = comboBox.Items[i] as SealListItem;
                        if (item != null && nodeInfo.defaultSealList.Exists(o => o.id == item.id))
                        {
                            comboBox.SetItemDefaultValue(i);
                        }
                    }
                }
            }
            // 设置专业用户映射关系加载状态标志为false，确保在需要使用专业用户映射关系时能够正确加载数据
            //majorUserMapLoaded = false;
            // 2) 你的新增需求：按“节点名中的专业”自动勾选人员
            // 仅当下拉项是 UserListItem 时执行
            var majorName = ParseMajorNameFromNodeName(checkBox.Text);
            if (!string.IsNullOrWhiteSpace(majorName))
            {
                SelectUsersByMajor(comboBox, majorName);
            }
        }
        //private void checkBoxSignProcess_CheckedChanged(object sender, EventArgs e)
        //{
        //    //获取选中的项
        //    CheckBox checkBox = sender as CheckBox;
        //    //获取签名列表 控件
        //    ComCheckBoxList comboBox = (ComCheckBoxList)Controls.Find($"comboBox_" + checkBox.Text, true)[0];
        //    //获取签名列表根据选中状态控制签名列表的启用和禁用
        //    comboBox.Enabled = checkBox.Checked;
        //    if (!checkBox.Checked)//取消选中
        //    {
        //        comboBox.ClearData();//清空签名列表
        //    }
        //    //签名流程，如果选中了签名节点，则设置签名列表默认选中签名
        //    if(approvalInfo.processtypeId == "0" && checkBox.Checked)
        //    {
        //        //签名节点
        //        var nodeInfo = (QzApprovalNodeListItem)comboBox.Tag;
        //        if (nodeInfo != null && nodeInfo.nodeType == 1)//签名节点类型，是1签得默认全选
        //        {
        //            if(nodeInfo.defaultSealList!=null && nodeInfo.defaultSealList.Any())
        //            {
        //                SealListItem item;
        //                for (var i = 0; i < comboBox.Items.Count; i++)
        //                {
        //                    item = (SealListItem)comboBox.Items[i];
        //                    if(nodeInfo.defaultSealList.Exists(o => o.id == item.id))
        //                    {
        //                        comboBox.SetItemDefaultValue(i);
        //                    }
        //                }
        //            }                    
        //        }
        //    }
        //}
        /// <summary>
        /// 专业用户映射关系维护逻辑：定义一个字典变量majorUserMap，键为专业名称，值为对应的用户列表；
        /// 在需要使用专业用户映射关系时，调用EnsureMajorUserMap方法加载数据，
        /// 该方法会根据项目属性中的项目ID，调用接口获取项目的分阶段和分专业信息，并构建专业名称到用户列表的映射关系；
        /// 在SelectUsersByMajor方法中，根据节点名称中的专业名称，从majorUserMap中获取对应的用户列表，并自动勾选下拉框中的用户项；
        /// 通过这种方式实现了根据节点名称中的专业自动勾选对应专业的用户，提高了用户体验和操作效率
        /// </summary>
        private Dictionary<string, List<GetProjectLevelUserModel>> majorUserMap = new Dictionary<string, List<GetProjectLevelUserModel>>();
        /// <summary>
        /// 专业用户映射关系加载状态标志：定义一个布尔变量majorUserMapLoaded，初始值为false，用于标识专业用户映射关系是否已经加载过；
        /// 在EnsureMajorUserMap方法中，首先检查该标志，如果已经加载过则直接返回，避免重复加载；如果没有加载过，则执行加载逻辑，并将该标志设置为true，
        /// 确保专业用户映射关系只会被加载一次，提高性能和效率
        /// </summary>
        private bool majorUserMapLoaded = false;
        /// <summary>
        /// 专业用户映射关系加载逻辑：根据项目属性中的项目ID，调用接口获取项目的分阶段和分专业信息，
        /// 并构建专业名称到用户列表的映射关系；该方法会在需要使用专业用户映射关系时被调用，并且只会加载一次，
        /// 后续使用时直接从内存中获取，避免重复调用接口提高性能
        /// </summary>
        private void EnsureMajorUserMap()
        {
            // 如果已经加载过专业用户映射关系，则直接返回，避免重复加载
            if (majorUserMapLoaded) return;
            // 设置专业用户映射关系加载状态标志为true，确保后续调用时不会重复加载
            majorUserMapLoaded = true;
            // 检查项目属性中的项目ID是否有效，如果无效则直接返回，避免调用接口获取数据时发生错误
            if (projectAttribute == null || string.IsNullOrWhiteSpace(projectAttribute.id))
            {
                return;
            }
            // 调用接口获取项目的分阶段信息，构建专业用户映射关系；如果接口调用失败则直接返回，避免后续操作发生错误
            var stageList = new List<ProjectResultModel>();
            if (!HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={projectAttribute.id}", ref stageList))
            {
                return;
            }
            // 遍历分阶段列表，找到类型为1的阶段（即分专业），然后调用接口获取该阶段下的专业信息；如果接口调用失败则继续处理下一个阶段，避免后续操作发生错误
            foreach (var stage in stageList.Where(o => o.type == 1))
            {
                // 根据阶段ID调用接口获取该阶段下的专业信息，构建专业用户映射关系；如果接口调用失败则继续处理下一个阶段，避免后续操作发生错误
                var majorList = new List<ProjectResultModel>();
                // 根据阶段ID调用接口获取该阶段下的专业信息，构建专业用户映射关系；如果接口调用失败则继续处理下一个阶段，避免后续操作发生错误
                if (!HttpGet(AppGlobalModel.GetProjectLevelDetails + $"?parentId={stage.id}", ref majorList))
                {
                    continue;
                }
                // 遍历专业列表，找到类型为2的专业（即具体专业），然后调用接口获取该专业下的用户信息；如果接口调用失败则继续处理下一个专业，避免后续操作发生错误
                foreach (var major in majorList.Where(o => o.type == 2))
                {
                    // 专业名称如果为空或全是空白字符，则跳过该专业，避免后续操作发生错误
                    if (string.IsNullOrWhiteSpace(major.name)) continue;
                    // 根据专业ID调用接口获取该专业下的用户信息，构建专业用户映射关系；如果接口调用失败则继续处理下一个专业，避免后续操作发生错误
                    var userList = new List<GetProjectLevelUserModel>();
                    // 根据专业ID调用接口获取该专业下的用户信息，构建专业用户映射关系；如果接口调用失败则继续处理下一个专业，避免后续操作发生错误
                    if (!HttpGet(AppGlobalModel.GetProjectLevelUser + $"?projectLevelId={major.id}", ref userList))
                    {
                        continue;
                    }
                    // 将专业名称作为键，用户列表作为值，添加到专业用户映射关系中；如果已经存在该专业名称的键，则将用户列表合并到已有的值中，避免覆盖之前的数据
                    var key = major.name.Trim();
                    // 将专业名称作为键，用户列表作为值，添加到专业用户映射关系中；如果已经存在该专业名称的键，则将用户列表合并到已有的值中，避免覆盖之前的数据
                    if (!majorUserMap.ContainsKey(key))
                    {
                        // 将专业名称作为键，用户列表作为值，添加到专业用户映射关系中；如果已经存在该专业名称的键，则将用户列表合并到已有的值中，避免覆盖之前的数据
                        majorUserMap[key] = new List<GetProjectLevelUserModel>();
                    }
                    // 将专业名称作为键，用户列表作为值，添加到专业用户映射关系中；如果已经存在该专业名称的键，则将用户列表合并到已有的值中，避免覆盖之前的数据
                    foreach (var user in userList)
                    {
                        // 将专业名称作为键，用户列表作为值，添加到专业用户映射关系中；如果已经存在该专业名称的键，则将用户列表合并到已有的值中，避免覆盖之前的数据
                        if (!majorUserMap[key].Any(o => o.userId == user.userId))
                        {
                            // 将专业名称作为键，用户列表作为值，添加到专业用户映射关系中；如果已经存在该专业名称的键，则将用户列表合并到已有的值中，避免覆盖之前的数据
                            majorUserMap[key].Add(user);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 专业名称解析逻辑：从节点名称中解析专业名称，假设节点名称的格式为“XX会签人”、“XX签字人”、“XX审核人”等，其中“XX”部分即为专业名称；如果节点名称不符合上述格式，则返回整个节点名称作为专业名称
        /// </summary>
        /// <param name="nodeName">节点名称</param>
        /// <returns>解析出的专业名称   </returns>
        private string ParseMajorNameFromNodeName(string nodeName)
        {
            if (string.IsNullOrWhiteSpace(nodeName)) return string.Empty;

            var text = nodeName.Trim();
            var suffixList = new[] { "会签人", "会签" };

            foreach (var suffix in suffixList)
            {
                if (text.EndsWith(suffix))
                {
                    return text.Substring(0, text.Length - suffix.Length).Trim();
                }
            }

            return text;
        }
        /// <summary>
        /// 选择用户的逻辑：根据节点名称中的专业，自动勾选对应专业的用户；如果没有匹配的专业或用户，则不进行自动勾选
        /// </summary>
        /// <param name="comboBox">用于显示用户列表的复选框控件</param>
        /// <param name="majorName">节点名称中的专业名称</param>
        private void SelectUsersByMajor(ComCheckBoxList comboBox, string majorName)
        {
            //仅当下拉项是 UserListItem 时执行自动勾选逻辑
            if (comboBox == null || string.IsNullOrWhiteSpace(majorName)) return;
            //
            EnsureMajorUserMap();
            // 如果专业用户映射关系中不包含该专业名称，则直接返回，不进行自动勾选
            //if (!majorUserMap.ContainsKey(majorName)) return;
            var hitKey = majorUserMap.Keys.FirstOrDefault(k => k.Contains(majorName));
            if (string.IsNullOrWhiteSpace(hitKey)) return;
            // 根据节点名称中的专业名称，从专业用户映射关系中获取对应的用户列表，并自动勾选下拉框中的用户项；如果没有匹配的用户，则不进行自动勾选
            var majorUsers = majorUserMap[hitKey];
            //var majorUsers = majorUserMap[majorName];
            if (majorUsers == null || !majorUsers.Any()) return;

            comboBox.ClearData();
            bool findUser = false;
            for (var i = 0; i < comboBox.Items.Count; i++)
            {
                //var userItem = comboBox.Items[i] as UserListItem;
                var userItem = comboBox.Items[i];
                if (userItem == null) continue;
                
                foreach (var item in userItem.GetType().GetProperties())
                {
                    //if (item.Name == "userId")
                    //{
                    //    var userId = item.GetValue(userItem)?.ToString();
                    //    if (majorUsers.Any(o => o.userId == userId))
                    //    {
                    //        comboBox.SetItemDefaultValue(i);
                    //        break;
                    //    }
                    //}
                    //else 
                        if (item.Name == "sealname")
                    {
                        var realName = item.GetValue(userItem)?.ToString();
                        if (majorUsers.Any(o => o.realName == realName))
                        {
                            comboBox.SetItemDefaultValue(i);
                            findUser = true;
                            break;
                        }
                    }
                    if (findUser) break;
                }
                if (findUser) break;
            }
        }
        #endregion

        #region 签章或签名
        /// <summary>
        /// 设计初始化签章流程选章页面
        /// </summary>
        /// <param name="control"></param>
        private void LoadSignatureProcess(Control control)
        {
            SplitContainer splitContainer = new SplitContainer();
            TreeView treeView = new TreeView();
            CheckedListBox checkedListBox = new CheckedListBox();
            var panel = new Panel();
            panel.Dock = DockStyle.Fill;

            var rightPanel = new Panel();
            rightPanel.Dock = DockStyle.Right;
            var listView = new ListView();
            listView.Columns.Add("已选用章（或人名）");
            listView.Columns[0].Width = 195;
            listView.Dock = DockStyle.Fill;
            listView.Font = new Font("宋体", 10.5F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            listView.FullRowSelect = true;
            listView.HideSelection = false;
            listView.UseCompatibleStateImageBehavior = false;
            listView.Name = "listView_selectList";
            listView.View = View.Details;
            rightPanel.Controls.Add(listView);

            treeView.Dock = DockStyle.Fill;
            treeView.Location = new Point(0, 0);
            treeView.Name = "treeView_dept";
            treeView.NodeMouseClick += TreeView_NodeMouseClick;

            checkedListBox.Dock = DockStyle.Fill;
            checkedListBox.CheckOnClick = true;
            checkedListBox.FormattingEnabled = true;
            checkedListBox.Location = new Point(0, 0);
            checkedListBox.Name = "checkedListBox_list";
            checkedListBox.ItemCheck += CheckedListBox_ItemCheck; ;

            panel.Controls.Add(checkedListBox);
            splitContainer.Panel1.Controls.Add(treeView);
            splitContainer.Panel2.Controls.Add(panel);
            splitContainer.Panel2.Controls.Add(rightPanel);

            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(3, 3);
            splitContainer.Size = new Size(948, 524);
            splitContainer.SplitterDistance = 332;

            control.Controls.Add(splitContainer);

            #region 加载组织架构
            //加载组织架构
            if (AppGlobalModel.DeptList != null && AppGlobalModel.DeptList.Any())
            {
                foreach (var item in AppGlobalModel.DeptList.Where(o => o.parentId == "0"))
                {
                    TreeNode root = new TreeNode();
                    //根目录名称
                    root.Text = item.deptName;
                    root.Tag = item;
                    treeView.Nodes.Add(root);
                }
            }
            #endregion
        }
        #endregion

        #region 归档，下载，其他
        private void LoadOtherProcess()
        {
            //流程类型 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章processtype_id
            var index = 0;

            var selectModel = (ApprovalListModel)comboBox_流程类型.SelectedItem;
            if (selectModel.processtypeId == "4")
            {
                foreach (var item in approvalInfo.qzApprovalNodeList.OrderByDescending(o => o.sort))
                {
                    if (item.nodeType == 1 || item.nodeType == 3)
                    {
                        var panel = new Panel();
                        var leftPanel = new Panel();

                        if (item.sealList == null)
                        {
                            item.sealList = new List<SealListItem>();
                        }

                        var comboBox = new ComCheckBoxList();
                        comboBox.Dock = DockStyle.Fill;
                        comboBox.Font = new Font("微软雅黑", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                        comboBox.Location = new Point(178, 0);
                        comboBox.Size = new Size(485, 33);
                        comboBox.Name = $"comboBox_{item.name}";
                        comboBox.Tag = item;
                        comboBox.DataSource = item.userList;
                        comboBox.DisplayMember = "realName";
                        comboBox.ValueMember = "id";
                        comboBox.Enabled = false;

                        var checkBox = new CheckBox();
                        checkBox.AutoSize = false;
                        checkBox.Dock = DockStyle.Right;
                        checkBox.Location = new Point(0, 0);
                        checkBox.Size = new Size(170, 25);
                        checkBox.Name = $"checkBox_{item.name}";
                        checkBox.Text = $"{item.name}";
                        checkBox.UseVisualStyleBackColor = true;
                        checkBox.Tag = item;
                        checkBox.CheckedChanged += new System.EventHandler(checkBoxSignProcess_CheckedChanged);

                        leftPanel.Controls.Add(checkBox);
                        leftPanel.Dock = DockStyle.Left;
                        leftPanel.Location = new Point(0, 0);
                        leftPanel.Size = new Size(178, 34);

                        panel.Controls.Add(comboBox);
                        panel.Controls.Add(leftPanel);
                        panel.Dock = DockStyle.Top;
                        panel.Padding = new Padding(0, 10, 80, 10);
                        panel.Location = new Point(3, (index * 53) + 3);
                        panel.Size = new Size(663, 53);

                        tabPage_流程配置.Controls.Add(panel);

                        //节点类型，是3不签得默认全选
                        if (item.nodeType == 3)
                        {
                            checkBox.Checked = true;
                            comboBox.Enabled = true;
                            comboBox.SelectAllItems();
                        }

                        index++;
                    }
                    else
                    {
                        ShowErrorMsg("节点类型不匹配，请联系后台管理员！");
                    }
                }
            }
            else
            {
                foreach (var item in approvalInfo.qzApprovalNodeList.OrderByDescending(o => o.sort))
                {
                    var panel = new Panel();
                    var leftPanel = new Panel();

                    var textBox = new TextBox();
                    textBox.Dock = DockStyle.Fill;
                    textBox.Font = new Font("微软雅黑", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    textBox.Location = new Point(178, 0);
                    textBox.Size = new Size(485, 33);
                    textBox.ReadOnly = true;
                    textBox.Text = String.Join("、", item.userList?.Select(o => o.realName));

                    var label = new Label();
                    label.Dock = DockStyle.Fill;
                    label.Location = new Point(0, 0);
                    label.Size = new Size(178, 34);
                    label.Text = $"{item.name}：";
                    label.TextAlign = ContentAlignment.MiddleRight;

                    leftPanel.Controls.Add(label);
                    leftPanel.Dock = DockStyle.Left;
                    leftPanel.Location = new Point(0, 0);
                    leftPanel.Size = new Size(178, 34);

                    panel.Controls.Add(textBox);
                    panel.Controls.Add(leftPanel);
                    panel.Dock = DockStyle.Top;
                    panel.Padding = new Padding(0, 10, 80, 10);
                    panel.Location = new Point(3, (index * 53) + 3);
                    panel.Size = new Size(663, 53);

                    tabPage_流程配置.Controls.Add(panel);

                    index++;
                }
            }
        }
        #endregion

        /// <summary>
        /// 签章选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var checkedListBox = (CheckedListBox)sender;
            var selectedItem = (SeallistOfZhuzhiModel)checkedListBox.SelectedItem;
            //判断是否存在
            var isModel = selectSealList.FirstOrDefault(o => o.id == selectedItem.id);

            if (e.NewValue == CheckState.Checked)
            {
                if (isModel == null)
                {
                    selectSealList.Add((SeallistOfZhuzhiModel)checkedListBox.SelectedItem);
                }
            }
            else
            {
                if (isModel != null)
                {
                    selectSealList.Remove(isModel);
                }
            }

            ListView listView = (ListView)Controls.Find($"listView_selectList", true)[0];

            listView.Items.Clear();
            foreach (var item in selectSealList)
            {
                ListViewItem lineLeft = new ListViewItem(item.sealname);
                listView.Items.Add(lineLeft);
            }
        }

        /// <summary>
        /// 加载组织结构
        /// </summary>
        /// <param name="treeNode"></param>
        private void LoadTreeView(TreeNode treeNode, DeptInfoResultModel data)
        {
            TreeNode root = new TreeNode();
            //根目录名称
            root.Text = data.deptName;
            root.Tag = data;
            treeNode.Nodes.Add(root);
        }

        /// <summary>
        /// 签章，组织结构点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var treeView = (TreeView)sender;
            CheckedListBox checkedListBox_list = (CheckedListBox)Controls.Find($"checkedListBox_list", true)[0];
            if (e.Button == MouseButtons.Left)
            {
                treeView.SelectedNode = e.Node;//选中这个节点
                var deptInfo = (DeptInfoResultModel)treeView.SelectedNode.Tag;

                if (!string.IsNullOrWhiteSpace(deptInfo.uuid))
                {
                    #region 加载章列表
                    var resultData = new List<SeallistOfZhuzhiModel>();
                    if (HttpGet(AppGlobalModel.SeallistOfZhuzhi + "?uuid=" + deptInfo.uuid, ref resultData))
                    {
                        checkedListBox_list.DataSource = resultData;
                        checkedListBox_list.DisplayMember = "sealname";
                        checkedListBox_list.ValueMember = "id";

                        if (selectSealList != null && selectSealList.Any())
                        {
                            for (int i = 0; i < checkedListBox_list.Items.Count; i++)
                            {
                                var item = (SeallistOfZhuzhiModel)checkedListBox_list.Items[i];
                                if (selectSealList.Exists(o => o.id == item.id))
                                {
                                    checkedListBox_list.SetItemChecked(i, true);
                                }
                                else
                                {
                                    checkedListBox_list.SetItemChecked(i, false);
                                }
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    checkedListBox_list.DataSource = null;
                }

                #region 加载子级组织架构
                var deptInfoList = AppGlobalModel.DeptList.Where(o => o.parentId == deptInfo.deptId);
                if (deptInfoList == null || !deptInfoList.Any())
                {
                    if (!HttpGet(AppGlobalModel.GetDeptList + "?parentId=" + deptInfo.deptId, ref deptInfoList))
                    {
                        return;
                    }
                }

                if (deptInfoList != null && deptInfoList.Any())
                {
                    treeView.SelectedNode.Nodes.Clear();
                    foreach (var item in deptInfoList)
                    {
                        LoadTreeView(treeView.SelectedNode, item);
                    }

                    treeView.SelectedNode.Expand();
                }
                #endregion
            }
        }

        /// <summary>
        /// 出版流程配置节点选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            var checkBox = (CheckBox)sender;
            var applynodeInfo = (QzApprovalNodeListItem)checkBox.Tag;
            Control control = Controls.Find($"textBox_{applynodeInfo.name}", true)[0];
            if (checkBox.Checked)
            {
                control.Enabled = true;
            }
            else
            {
                control.Text = "";
                control.Enabled = false;
            }
        }

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
        /// 发起
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn发起_Click(object sender, EventArgs e)
        {          
            if (approvalInfo == null)
            {
                ShowErrorMsg("请选择流程类型！");
                return;
            }
            if (string.IsNullOrWhiteSpace(textBox_流程标题.Text.Trim()))
            {
                ShowErrorMsg("请输入流程标题！");
                return;
            }

            //0签名、5签章、6签名签章
            if (approvalInfo.processtypeId == "0" || approvalInfo.processtypeId == "5" || approvalInfo.processtypeId == "6")
            {
                #region 发起签章流程(签名、签章、签名签章)
                var msgStr = "";
                var applynodeInfos = new List<ApplynodeInfo>();
                //0签名、6签名签章
                if (approvalInfo.processtypeId == "0" || approvalInfo.processtypeId == "6")
                {
                    foreach (var item in approvalInfo.qzApprovalNodeList.Where(o=>o.nodeType == 1 || o.nodeType == 3)) // 遍历流程节点列表，找到节点类型是1签得或3不签得的节点，然后根据节点名称找到对应的复选框控件，判断如果该复选框被选中了，则根据节点名称找到对应的下拉框控件中的人员列表，如果没有选择人员，则提示用户配置流程节点，并返回；如果选择了人员，则遍历下拉框控件中选择的人员列表，获取每个人员的id和姓名，并添加到applynodeInfos列表中；同时判断如果某个人员的签名或签章快过期了，则将该人员的姓名添加到msgStr字符串中，最后提示用户哪些人员的签名或签章快过期了
                    {
                        CheckBox checkBox= new CheckBox();
                        try //这个try catch主要是为了防止因为流程节点配置错误导致找不到对应的复选框控件，从而引发异常，如果发生异常则提示用户检查流程节点配置是否正确，并将该复选框设置为未选中状态，避免后续操作发生错误
                        {
                             checkBox = (CheckBox)Controls.Find($"checkBox_{item.name}", true)[0]; // 根据节点名称找到对应的复选框控件
                        }
                        catch (Exception ex)
                        {
                             checkBox.Checked = false;
                        }
                        
                        if (checkBox.Checked)
                        {
                            ComCheckBoxList comboBox = (ComCheckBoxList)Controls.Find($"comboBox_{item.name}", true)[0]; // 根据节点名称找到对应的下拉框控件中的人员列表

                            if (comboBox.GetValue() == null || comboBox.GetValue().Count == 0) // 判断如果没有选择人员，则提示用户配置流程节点，并返回
                            {
                                ShowErrorMsg($"请配置流程{item.name}！");
                                return;
                            }
                            // 遍历下拉框控件中选择的人员列表，获取每个人员的id和姓名，并添加到applynodeInfos列表中；同时判断如果某个人员的签名或签章快过期了，则将该人员的姓名添加到msgStr字符串中，最后提示用户哪些人员的签名或签章快过期了
                            foreach (SealListItem nodeItem in comboBox.GetValue())
                            {
                                if (!string.IsNullOrWhiteSpace(nodeItem.endtime))// 判断如果人员的签名或签章有过期时间，则计算距离过期还有多少天，如果快过期了（比如7天内），则将该人员的姓名添加到msgStr字符串中，最后提示用户哪些人员的签名或签章快过期了
                                {
                                    TimeSpan sp = Convert.ToDateTime(nodeItem.endtime).Subtract(DateTime.Now); // 计算距离过期还有多少天
                                    var day = sp.TotalDays;

                                    if (day < 7)
                                    {
                                        msgStr += $"{nodeItem.sealname}、";
                                    }
                                }

                                applynodeInfos.Add(new ApplynodeInfo()
                                {
                                    node_id = item.id,
                                    seal_id = nodeItem.id
                                });
                            }
                        }
                    }

                    if (!applynodeInfos.Any())
                    {
                        ShowErrorMsg("请配置签名流程节点！");
                        return;
                    }
                }
                //5签章、6签名签章
                if (approvalInfo.processtypeId == "5" || approvalInfo.processtypeId == "6")
                {
                    if (GlobalVariables.companyName == "吉林医药设计院有限公司")
                    {
                        if(MessageBox.Show("请确认发起流程文件已与签章申请核对完成(避免项目号、项目名称、单体名称、出图日期、目录等问题)！！！", "重要提示：", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        {
                            ShowErrorMsg($"请与签章指令核对后再次发起签章流程！");
                            return;
                        }
                    }

                    if (selectSealList == null || !selectSealList.Any())
                    {
                        ShowErrorMsg($"请选择签章！");
                        return;
                    }

                    foreach (var item in selectSealList)
                    {
                        if (!string.IsNullOrWhiteSpace(item.endtime))
                        {
                            TimeSpan sp = Convert.ToDateTime(item.endtime).Subtract(DateTime.Now);
                            var day = sp.TotalDays;

                            if (day < 7)
                            {
                                msgStr += $"{item.sealname}、";
                            }
                        }
                    }
                    applynodeInfos.AddRange(selectSealList.Select(o => new ApplynodeInfo() { seal_id = o.id }).ToList());
                }

                if (!string.IsNullOrWhiteSpace(msgStr))
                {
                    msgStr = msgStr.TrimEnd('、');
                    MessageBox.Show(this, msgStr + "等章快过期了，请及时联系管理员处理！", "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                var param = new
                {
                    applynodes = applynodeInfos,    //节点列表
                    approval_id = approvalInfo.id,  //流程id
                    fileids = fileIds,  //要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传
                    name = textBox_流程标题.Text.Trim(),    //流程标题
                    remark = textBox_流程说明.Text.Trim(), //备注
                    days = textBox_流程天数.Text.Trim(), //流程天数
                    type = type,   //发起类型 0购物车 1文件夹 2项目 3文件
                    fileType = fileType,   //文件来源0 项目区 1归档区
                    pro_id = projectAttribute.id,   //项目id
                    pages = (string.IsNullOrWhiteSpace(comCheckBoxList1.GetText()) ? "" : comCheckBoxList1.GetText())//盖章页数(逗号分割 0代表全部) 只有一个项目文件的时候才穿
                };

                var resultData = string.Empty;
                var postData = $"param={JsonConvert.SerializeObject(param)}";
                if (HttpPost(AppGlobalModel.StartApprovalQianzhang, postData, ref resultData))
                {
                    ShowSuccessMsg("发起成功！");
                    DialogResult = DialogResult.OK;
                }
                #endregion
            }
            //1出版 
            else if (approvalInfo.processtypeId == "1")
            {

                #region 出版
                var applynodes = new List<ApplynodeInfo>();
                ApplynodeInfo applynode;
                foreach (var item in approvalInfo.qzApprovalNodeList)
                {
                    Control textBox = Controls.Find($"textBox_{item.name}", true)[0];
                    CheckBox checkBox = (CheckBox)Controls.Find($"checkBox_{item.name}", true)[0];

                    if (checkBox.Checked)
                    {
                        applynode = new ApplynodeInfo();
                        applynode.node_id = item.id;
                        applynode.node_name = item.name;
                        applynode.userList = item.userList;
                        applynode.remark = textBox.Text.Trim();
                        applynodes.Add(applynode);
                    }
                }

                if (!applynodes.Any())
                {
                    ShowErrorMsg("请配置流程节点！");
                    return;
                }

                var confirmInfo = new ConfirmInfoModel
                {
                    applynodeList = applynodes,
                    annex_id = textBox_出版份数.Text.Trim(),   //打印份数
                    projectName = projectAttribute.name,
                    realName = AppGlobalModel.UseInfo.realName
                };

                var frm = new FrmPublishConfirm(confirmInfo);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var param = new
                    {
                        applynodes = applynodes,    //节点列表
                        approval_id = approvalInfo.id,  //流程id
                        fileids = fileIds,  //要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传
                        name = textBox_流程标题.Text.Trim(),    //流程标题
                        remark = textBox_流程说明.Text.Trim(), //备注
                        days = textBox_流程天数.Text.Trim(), //流程天数
                        type = type,   //发起类型 0购物车 1文件夹 2项目 3文件
                        fileType = fileType,   //文件来源0 项目区 1归档区
                        annex_id = textBox_出版份数.Text.Trim(),   //打印份数
                        pro_id = projectAttribute.id    //项目id
                    };

                    var resultData = string.Empty;
                    var postData = $"param={JsonConvert.SerializeObject(param)}";
                    if (HttpPost(AppGlobalModel.StartApprovalChuban, postData, ref resultData))
                    {
                        ShowSuccessMsg("发起成功！");
                        DialogResult = DialogResult.OK;
                    }
                }
                #endregion
            }
            //2下载//4其他
            else if (approvalInfo.processtypeId == "2" || approvalInfo.processtypeId == "4")
            {
                #region 下载或其他
                if (approvalInfo.qzApprovalNodeList == null || !approvalInfo.qzApprovalNodeList.Any())
                {
                    ShowErrorMsg("请后台管理员配置流程节点！");
                    return;
                }

                var applynodeInfos = new List<OtherApplynodeInfo>();
                if (approvalInfo.processtypeId == "4")
                {
                    foreach (var item in approvalInfo.qzApprovalNodeList)
                    {
                        CheckBox checkBox = (CheckBox)Controls.Find($"checkBox_{item.name}", true)[0];

                        if (checkBox.Checked)
                        {
                            ComCheckBoxList comboBox = (ComCheckBoxList)Controls.Find($"comboBox_{item.name}", true)[0];

                            if (comboBox.GetValue() == null || comboBox.GetValue().Count == 0)
                            {
                                ShowErrorMsg($"请配置流程{item.name}！");
                                return;
                            }

                            var userList = new List<OtherApplyNodeUserList>();
                            foreach (UserListItem nodeItem in comboBox.GetValue())
                            {
                                userList.Add(new OtherApplyNodeUserList() { id = nodeItem.id });
                            }

                            applynodeInfos.Add(new OtherApplynodeInfo()
                            {
                                node_id = item.id,
                                userList = userList
                            });
                        }
                    }
                }
                else
                {
                    applynodeInfos = approvalInfo.qzApprovalNodeList.Select(o => new OtherApplynodeInfo { node_id = o.id, userList = o.userList.Select(u => new OtherApplyNodeUserList { id = u.id }).ToList() }).ToList();
                }

                if (!applynodeInfos.Any())
                {
                    ShowErrorMsg("请配置流程节点！");
                    return;
                }

                var param = new
                {
                    applynodes = applynodeInfos,    //节点列表
                    approval_id = approvalInfo.id,  //流程id
                    fileids = fileIds,  //要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传
                    name = textBox_流程标题.Text.Trim(),    //流程标题
                    remark = textBox_流程说明.Text.Trim(), //备注
                    days = textBox_流程天数.Text.Trim(), //流程天数
                    type = type,   //发起类型 0购物车 1文件夹 2项目 3文件
                    fileType = fileType,   //文件来源0 项目区 1归档区
                    pro_id = projectAttribute.id    //项目id
                };

                var resultData = string.Empty;
                var postData = $"param={JsonConvert.SerializeObject(param)}";
                if (HttpPost(AppGlobalModel.StartApprovalQita, postData, ref resultData))
                {
                    ShowSuccessMsg("发起成功！");
                    DialogResult = DialogResult.OK;
                }
                #endregion
            }
            //3归档
            else if (approvalInfo.processtypeId == "3")
            {
                #region 归档
                if (approvalInfo.qzApprovalNodeList == null || !approvalInfo.qzApprovalNodeList.Any())
                {
                    ShowErrorMsg("请后台管理员配置流程节点！");
                    return;
                }

                var param = new
                {
                    applynodes = approvalInfo.qzApprovalNodeList.Select(o => new { node_id = o.id, userList = o.userList.Select(u => new { id = u.id }) }),    //节点列表
                    approval_id = approvalInfo.id,  //流程id
                    fileids = fileIds,  //要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传
                    name = textBox_流程标题.Text.Trim(),    //流程标题
                    remark = textBox_流程说明.Text.Trim(), //备注
                    days = textBox_流程天数.Text.Trim(), //流程天数
                    type = type,   //发起类型 0购物车 1文件夹 2项目 3文件
                    guiId = guiId,//归档id
                    fileType = fileType,   //文件来源0 项目区 1归档区
                    pro_id = projectAttribute.id    //项目id
                };

                var resultData = string.Empty;
                var postData = $"param={JsonConvert.SerializeObject(param)}";               

                if (HttpPost(AppGlobalModel.StartApprovalGuidang, postData, ref resultData))
                {
                    ShowSuccessMsg("发起成功！");
                    //清空临时文件                
                    DialogResult = DialogResult.OK;
                    
                }
                #endregion
            }
            else
            {
                ShowErrorMsg("此流程类型不合法！");
            }
            
        }


        /// <summary>
        /// 出版份数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox7出版份数_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键 
            if ((e.KeyChar == 0x2D) && (((TextBox)sender).Text.Length == 0)) return;   //处理负数 
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符 
                }
            }
        }

        #region 重置文件列表
        private void ResetFileList(string tab)
        {
            queryInfo.parentId = "0";
            queryInfo.tab = tab;
            treeView_施工图.Nodes.Clear();
            //加载文件列表
            LoadFileList();
        }
        #endregion

        #region 加载文件列表
        /// <summary>
        /// 加载文件列表
        /// </summary>
        private void LoadFileList()
        {
            var resultData = new List<GetKeepProjectDirModel>();
            if (HttpPost(AppGlobalModel.GetApprovalProjectStructure,queryInfo, ref resultData))
            {
                foreach (var item in resultData.OrderBy(o => o.name, new StringRankComparer()))
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
                        treeView_施工图.Nodes.Add(root);
                    }
                    else
                    {
                        treeView_施工图.SelectedNode.Nodes.Add(root);
                    }
                }

                //第一次加载要加载文件汇总
                if (queryInfo.parentId == "0")
                {
                    var resultFileAllData = new GetApprovalProjectStructureAllModel();
                    if (HttpPost(AppGlobalModel.GetApprovalProjectStructureAll, queryInfo, ref resultFileAllData))
                    {
                        if (resultFileAllData != null)
                        {
                            label10.Text = $"文件数量：{resultFileAllData.FileAll}";
                            //医药设计院时,不显示折合A1数量;
                            if ( GlobalVariables.companyName == "吉林医药设计院有限公司")
                            {
                                label9.Visible = false;
                            }else 
                            {
                                label9.Text = $"总A1数量：{resultFileAllData.FoldedAll}   A1";
                            }
                           
                        }
                    }
                    else
                    {
                        this.Close();
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
        /// 文件列表单击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView_施工图.SelectedNode = e.Node;
            var selectInfo = (GetKeepProjectDirModel)e.Node.Tag;
            if (e.Node.Nodes.Count <= 0 && selectInfo.type != 5)
            {
                queryInfo.parentId = selectInfo.id;
                LoadFileList();

                if (treeViewNodeMouseClick)
                {
                    treeView_施工图.SelectedNode.Expand();
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

                    var frm = new FrmPreviewArea(selectInfo.filePath, queryInfo.fileType, listUrl);
                    frm.Show();
                }
            }

            treeViewNodeMouseClick = true;
        }

        private void treeView1_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }
        #endregion

    }
    /// <summary>
    /// 流程节点信息：节点id、章id、节点名称、用户列表、备注
    /// </summary>
    public class ApplynodeInfo
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public string node_id { get; set; }
        /// <summary>
        /// 章id
        /// </summary>
        public string seal_id { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>
        public string node_name { get; set; }
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<UserListItem> userList { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
    }
    /// <summary>
    /// 其它流程节点信息:节点id和用户列表
    /// </summary>
    class OtherApplynodeInfo
    {
        /// <summary>
        /// 节点id
        /// </summary>
        public string node_id { get; set; }
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<OtherApplyNodeUserList> userList { get; set; }
    }
    /// <summary>
    /// 其它流程节点用户列表信息：用户id
    /// </summary>
    class OtherApplyNodeUserList
    {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }
       // label9
    }
    
}
