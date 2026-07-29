using DMC.Helper;
using DMC.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace DMC
{
    //label16
    /// <summary>
    /// 审批详情
    /// </summary>
    public partial class FrmApprovalInfo : BaseForm
    {
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
        /// <summary>
        /// applyId：审批详情中主键ID： ApplyListModel类型中的ID   
        /// </summary>
        private string applyId = null;
        /// <summary>
        /// 审批详情:1:applyXh 序号/2：appName 流程类型名/3：userDpt 用户部门/4：remark 备注/5：userId 发起人Id/6:userName 用户名/ 7:nodeList 节点List/ 8:NAME 流程标题/9：processtype_id 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /10：result 审批状态： 0进行中 1已通过 -1未通过/11：createTime 提交时间/12：resultTime 最后审批时间/13 :fileids 要是按文件夹发起的就传文件夹id，按项目就传项目id，文件就传文件id，购物车就不用传/14：days 流程天数/15 ：id 主键/16 ：proId 项目id/17：proName 项目名称/ 18：annex_id 打印份数/ 19:fileType 文件来源0 项目区 1归档区/20: guiId 归档使用/21:downUser 下载人主键/22:money 出版订单金额/ 23 :FoldedAll 折A1/ 24: FileAll 文件总数
        /// </summary>
        private ApplyInfoModel applyInfo = null;
        /// <summary>
        /// 查询审批项目结构：1、fileType：文件来源0 项目区 1归档区 /2、type：发起类型 0购物车 1文件夹 2项目 3文件 /3、fileIds：流id列表用，分割 /4、 parentId：上级ID /5、applyId：审批详情中得主键 /6、tab：是否获得未归档的文件 0未归档 1全部 默认传1，就出版和下载都传 0  剩下的都传1
        /// </summary>
        private QueryApprovalProjectStructure queryInfo = null;
        
        // 添加分类数据列表
        private List<GetKeepProjectTempTechnicalModel> 石油资料列表 = new List<GetKeepProjectTempTechnicalModel>();
        private List<GetKeepProjectTempTechnicalModel> 制冷资料列表 = new List<GetKeepProjectTempTechnicalModel>();
        private List<GetKeepProjectTempTechnicalModel> 农产品资料列表 = new List<GetKeepProjectTempTechnicalModel>();
        private List<GetKeepProjectTempTechnicalModel> 冷链物流资料列表 = new List<GetKeepProjectTempTechnicalModel>();
        private List<GetKeepProjectTempTechnicalModel> 食品资料列表 = new List<GetKeepProjectTempTechnicalModel>();
        /// <summary>
        /// 流程审批窗口初始化（）
        /// </summary>
        /// <param name="objId">审批详情中得主键Id</param>
        public FrmApprovalInfo(string objId)
        {
            InitializeComponent();

            dataGridView_项目属性表.AutoGenerateColumns = false;
            dataGridView_技术资料归档表.AutoGenerateColumns = false;
            dataGridView_专业人员表.AutoGenerateColumns = false;
            //objId 是 ApplyListModel 类型变量中的 id 
            applyId = objId;
            InitializeClassificationControls();
        }
        /// <summary>
        /// 在 InitializeComponent 方法或窗体加载时添加
        /// </summary>
        private void InitializeClassificationControls()
        {
            // 为所有分类表格绑定事件
            if (dataGridView_石油 != null)
            {
                dataGridView_石油.RowPostPaint += dataGridView_分类表格_RowPostPaint;
                dataGridView_石油.CellClick += dataGridView_分类表格_CellClick;
            }

            if (dataGridView_制冷 != null)
            {
                dataGridView_制冷.RowPostPaint += dataGridView_分类表格_RowPostPaint;
                dataGridView_制冷.CellClick += dataGridView_分类表格_CellClick;
            }

            if (dataGridView_农产品 != null)
            {
                dataGridView_农产品.RowPostPaint += dataGridView_分类表格_RowPostPaint;
                dataGridView_农产品.CellClick += dataGridView_分类表格_CellClick;
            }

            if (dataGridView_冷链物流 != null)
            {
                dataGridView_冷链物流.RowPostPaint += dataGridView_分类表格_RowPostPaint;
                dataGridView_冷链物流.CellClick += dataGridView_分类表格_CellClick;
            }

            if (dataGridView_食品 != null)
            {
                dataGridView_食品.RowPostPaint += dataGridView_分类表格_RowPostPaint;
                dataGridView_食品.CellClick += dataGridView_分类表格_CellClick;
            }
        }
        
        // <summary>
        /// 专业人员列表格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView专业人员表_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                var roleName = dataGridView_专业人员表.Columns[e.ColumnIndex].Name;
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
        /// 审批页面load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmApprovalInfo_Load(object sender, EventArgs e)
        {

            if (GlobalVariables.companyName == "华商国际工程有限公司")
            {
                tabPage_审图意见.Text = "审图版图纸及意见";
            }
            //else if (GlobalVariables.companyName == "辽宁方大工程设计有限公司")
            //{
            //    tabPage6.Text = "项目其它附件（可选上传）";
            //}
            //else if (GlobalVariables.companyName == "辽宁省建筑设计研究院有限责任公司")
            //{
            //    tabPage6.Text = "成果图（归档）";
            //}
            else
            {
                tabPage_审图意见.Text = "项目其它附件（可选上传）";
            }

            Splasher.Show(typeof(FrmLoading));
            //获取审批详情的接口参数
            var param = new
            {
                id = applyId
            };
            //用于储存获取审批详情的接口返回数据的变量
            var resultData = new ApplyInfoModel();
            //获取审批详情的接口调用,并将返回数据存入resultData变量中
            if (HttpPost(AppGlobalModel.ApplyInfo, param, ref resultData))
            {
                applyInfo = resultData;
                //如果流程Id不等于3(归档),就移除归档页面
                if (applyInfo.processtype_id != "3")// 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /processtype_id
                {
                    tabControl_项目信息表.TabPages.Remove(tabPage_项目信息);
                    tabControl_项目信息表.TabPages.Remove(tabPage_技术资料);
                    tabControl_项目信息表.TabPages.Remove(tabPage_审图意见);
                }

                label_流程序号.Text = applyInfo.applyXh;
                textBox_流程类型.Text = applyInfo.appName;
                textBox_流程标题.Text = applyInfo.NAME;
                textBox_项目名称.Text = applyInfo.proName;
                textBox_提交用户.Text = applyInfo.userName;
                textBox_用户部门.Text = applyInfo.userDept;
                textBox_流程说明.Text = applyInfo.remark;
                textBox_流程天数.Text = applyInfo.days;
                textBox_审批时间.Text = applyInfo.resultTime;
                textBox_提交时间.Text = applyInfo.createTime;
                label_流程状态.Text = (applyInfo.result == 0 ? "进行中" : (applyInfo.result == 1 ? "已通过" : "未通过"));

                //出版
                if (applyInfo.processtype_id == "1")// 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /processtype_id
                {
                    //打开份数显示
                    textBox_出版份数.Visible = true;
                    label_出版份数.Visible = true;
                    textBox_出版份数.Text = applyInfo.annex_id.ToString();
                    //按键状态显示
                    button_拒绝.Enabled = true;
                    button_通过.Enabled = false;
                    button_完成.Visible = true;
                    linkLabel_导出流程.Visible = true;
                    panel1.Visible = true;
                }
                //下载
                if (applyInfo.processtype_id == "2")// 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /processtype_id
                {
                    if (applyInfo.result == 1)// 审批状态： 0进行中 1已通过 -1未通过 2我审批过了
                    {
                        button5施工图下载.Enabled = true;
                        button5施工图下载.Visible = true;
                    }

                }

                //拒绝之后就没有操作了
                if (applyInfo.result == -1 || applyInfo.result == -2)
                {
                    button_拒绝.Enabled = false;
                    button_通过.Enabled = false;
                    button_完成.Visible = false;
                    button_取消.Enabled = true;
                    //打开份数显示
                    textBox_出版份数.Visible = false;
                    label_出版份数.Enabled = true;
                    textBox_出版份数.Text = applyInfo.annex_id.ToString();
                }
                else if (applyInfo.result == 0)// 审批状态： 0进行中 1已通过 -1未通过 2我审批过了
                {
                    //出版
                    if (applyInfo.processtype_id != "1")// 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /processtype_id
                    {
                        if (applyInfo.nodeList.Exists(o => o.sum == 2))
                        {
                            textBox_出版份数.Enabled = true;
                            label_出版份数.Enabled = true;
                            textBox_出版份数.Text = applyInfo.annex_id.ToString();
                            panel1.Visible = true;
                        }
                        else
                        {
                            button_拒绝.Enabled = true;
                            button_通过.Enabled = false;
                            button_取消.Enabled = true;
                        }
                    }
                }
                else
                {
                    // 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /processtype_id
                    if (new string[] { "0", "2", "5", "6" }.Contains(applyInfo.processtype_id) && applyInfo.downUser == AppGlobalModel.UseInfo.id)
                    {
                        button_拒绝.Enabled = false;
                        button_通过.Enabled = false;
                        button_下载.Enabled = true;
                        panel1.Visible = true;
                    }
                    else if (applyInfo.processtype_id == "1")// 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /processtype_id
                    {
                        button_完成.Enabled = false;
                        button_拒绝.Enabled = false;
                        textBox_订单金额.Text = applyInfo.money;
                        textBox_订单金额.Visible = true;
                        label_订单金额.Visible = true;

                    }
                    else
                    {
                        //打开按键
                        button_拒绝.Enabled = false;
                        button_通过.Enabled = false;
                        button_取消.Enabled = true;
                    }
                }
                //再次发起流程
                if (applyInfo.result == 0)
                {
                    linkLabel_流程再发起.Visible = false;
                }
                else
                {
                    linkLabel_流程再发起.Visible = true;
                }

                #region 加载流程进度

                applyInfo.nodeList.Insert(0, new NodeListItem() { nodeName = "开始", applyUser = applyInfo.userName, resultTime = applyInfo.createTime, resultRemark = applyInfo.remark });

                var index = 0;
                foreach (var item in applyInfo.nodeList)
                {
                    #region 操作按钮 button3通过键 panel10 button4下载键 panel10
                    //var panel10 = new Panel();
                    var panel_操作按键区 = new Panel();
                    if (applyInfo.result != -1 && item.sum == 2)// 审批状态： 0进行中 1已通过 -1未通过 2我审批过了
                    {
                        // 
                        // button3通过键
                        // 
                        var button_通过 = new Button();
                        button_通过.Location = new Point(28, 15);
                        button_通过.Size = new Size(85, 35);
                        button_通过.Text = "通过";
                        button_通过.Tag = item;
                        button_通过.Click += Button3完成_Click;
                        button_通过.UseVisualStyleBackColor = true;


                        //流程审批页面的 出版 按键
                        if (applyInfo.processtype_id == "1")// 流程类型： 0签名 1出版 2下载 3归档 4其他 5签章 6签名签章 /processtype_id
                        {
                            button_通过.Text = "完成";
                            button_通过.Location = new Point(75, 15);
                            button_通过.Size = new Size(60, 35);

                            // 
                            // button4下载键
                            // 
                            var button_下载 = new Button();
                            button_下载.Location = new Point(5, 15);
                            button_下载.Size = new Size(60, 35);
                            button_下载.Text = "下载";
                            button_下载.Tag = item;
                            button_下载.Click += Button4下载_Click;
                            button_下载.UseVisualStyleBackColor = true;

                            panel_操作按键区.Controls.Add(button_下载);
                        }

                        // 
                        // panel10
                        // 
                        panel_操作按键区.Controls.Add(button_通过);

                    }

                    panel_操作按键区.BorderStyle = BorderStyle.FixedSingle;
                    panel_操作按键区.Dock = DockStyle.Right;
                    panel_操作按键区.Location = new Point(586, 0);
                    panel_操作按键区.Size = new Size(140, 65);
                    #endregion

                    #region 审批意见/备注 label14 panel9                   
                    // label14                 
                    //var label14 = new Label();
                    var label_备注 = new Label();
                    label_备注.AutoSize = true;
                    label_备注.Location = new Point(0, 0);
                    label_备注.Text = item.resultRemark;  
                    // 备注panel
                    //var panel9 = new Panel();
                    var panel_备注 = new Panel();
                    panel_备注.Controls.Add(label_备注);
                    panel_备注.BorderStyle = BorderStyle.FixedSingle;
                    panel_备注.Dock = DockStyle.Fill;
                    panel_备注.AutoScroll = true;
                    panel_备注.Location = new Point(414, 0);
                    //panel9.Size = new Size(240, 65);
                    panel_备注.AutoSize = true;
                    var textBox = new TextBox();
                    textBox.Dock = DockStyle.Fill;
                    textBox.Multiline = true;
                    textBox.ScrollBars = ScrollBars.Vertical;
                    textBox.ReadOnly = true;
                    textBox.Location = new Point(0, 0);
                    textBox.Text = item.resultRemark;
                    textBox.Size= panel_备注.Size;
                    #endregion

                    #region 提交/审批时间 label16 panel11
                    // 
                    // label16:提交时间
                    // 
                    //var label16 = new Label();
                    var label_提交时间 = new Label();
                    label_提交时间.Dock = DockStyle.Fill;
                    label_提交时间.Location = new Point(0, 0);
                    label_提交时间.Size = new Size(185, 41);
                    label_提交时间.Text = item.resultTime;
                    label_提交时间.TextAlign = ContentAlignment.MiddleCenter;
                    // 
                    // panel11
                    // 
                    var panel_提交时间 = new Panel();
                    panel_提交时间.Controls.Add(label_提交时间);
                    panel_提交时间.BorderStyle = BorderStyle.FixedSingle;
                    panel_提交时间.Dock = DockStyle.Left;
                    panel_提交时间.Location = new Point(260, 0);
                    panel_提交时间.Size = new Size(185, 65);
                    #endregion

                    #region 审批状态 label17 panel12
                    // 
                    // label17流程状态
                    // 
                    //var label17 = new Label();
                    var label_流程状态 = new Label();
                    label_流程状态.Dock = DockStyle.Fill;
                    label_流程状态.Location = new Point(0, 0);
                    label_流程状态.Size = new Size(125, 41);
                    label_流程状态.Text = item.nodeName == "开始" ? "已提交" : (item.result == 0 ? "进行中" : (item.result == 1 ? "已通过" : "未通过"));
                    label_流程状态.TextAlign = ContentAlignment.MiddleCenter;
                    // 
                    // panel12
                    // 
                    //var panel12 = new Panel();
                    var panel_流程状态 = new Panel();
                    panel_流程状态.Controls.Add(label_流程状态);
                    panel_流程状态.BorderStyle = BorderStyle.FixedSingle;
                    panel_流程状态.Dock = DockStyle.Left;
                    panel_流程状态.Location = new Point(150, 0);
                    panel_流程状态.Size = new Size(125, 65);
                    #endregion

                    #region 节点/用户  label18 label19 panel13                  
                    // 
                    // label18节点名称
                    // 
                    //var label18 = new Label();
                    var label_节点名称 = new Label();
                    label_节点名称.Dock = DockStyle.Top;
                    label_节点名称.Location = new Point(0, 0);
                    label_节点名称.Size = new Size(300, 26);
                    label_节点名称.Text = item.nodeName;
                    label_节点名称.Font = new Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular);
                    label_节点名称.TextAlign = ContentAlignment.MiddleLeft;

                    //label36
                    // 
                    // label19发起人\审批人
                    // 
                    var label_发起人审批人 = new Label();
                    //label_发起人审批人.Dock = DockStyle.Fill;
                    label_发起人审批人.AutoSize = true;
                    label_发起人审批人.Location = new Point(0, 26);

                    //var num = label_节点名称.Text.Length/2;
                    var num = 2;
                    var userStr = "";
                    for (var i = 0; i < num; i++)
                    {
                        userStr += "   ";
                    }
                    label_发起人审批人.Font = new Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular);
                    label_发起人审批人.Text = $"{userStr}└ {item.applyUser}";
                    label_发起人审批人.TextAlign = ContentAlignment.MiddleLeft;
                    // 
                    // panel13
                    // 
                    //var panel13 = new Panel();
                    var panel_节点用户 = new Panel();
                    panel_节点用户.Controls.Add(label_发起人审批人);
                    panel_节点用户.Controls.Add(label_节点名称);
                    panel_节点用户.BorderStyle = BorderStyle.FixedSingle;
                    panel_节点用户.Dock = DockStyle.Left;
                    panel_节点用户.AutoScroll = true;
                    panel_节点用户.Location = new Point(0, 0);
                    panel_节点用户.Size = new Size(300, 65);
                    #endregion

                    var panel_流程进度 = new Panel();
                    panel_流程进度.Controls.Add(panel_备注);
                    panel_流程进度.Controls.Add(panel_操作按键区);
                    panel_流程进度.Controls.Add(panel_提交时间);
                    panel_流程进度.Controls.Add(panel_流程状态);
                    panel_流程进度.Controls.Add(panel_节点用户);
                    panel_流程进度.Dock = DockStyle.Top;
                    panel_流程进度.Location = new Point(3, (index * 65) + 44);
                    panel_流程进度.Size = new Size(786, 65);
                    tabPage_流程进度表.Controls.Add(panel_流程进度);
                    panel_流程进度.BringToFront();
                    index++;
                }
                #endregion

                #region 归档单独处理
                if (applyInfo.processtype_id == "3")
                {
                    #region 获得归档项目临时字段
                    var resultKeepProTempAttr = new GetKeepProjectTempAttributeModel();
                    if (HttpGet(AppGlobalModel.GetKeepProjectTempAttribute + $"?id={applyInfo.guiId}", ref resultKeepProTempAttr))
                    {
                        if (resultKeepProTempAttr != null)
                        {
                            textBox_归档_可行研究开始时间.Text = resultKeepProTempAttr.oneStartTime;
                            textBox_归档_可行研究结束时间.Text = resultKeepProTempAttr.oneEndTime;
                            textBox_归档_方案开始时间.Text = resultKeepProTempAttr.twoStartTime;
                            textBox_归档_方案结束时间.Text = resultKeepProTempAttr.twoEndTime;
                            textBox_归档_初步设计开始时间.Text = resultKeepProTempAttr.threeStartTime;
                            textBox_归档_初步设计结束时间.Text = resultKeepProTempAttr.threeEndTime;
                            textBox_归档_施工图开始时间.Text = resultKeepProTempAttr.fourStartTime;
                            textBox_归档_施工图结束时间.Text = resultKeepProTempAttr.fourEndTime;
                            textBox_备注.Text = resultKeepProTempAttr.remarks;

                            #region 获取项目属性信息
                            var resultProAttr = new GetProjectAttributeModel();
                            if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={resultKeepProTempAttr.projectId}", ref resultProAttr))
                            {
                                textBox_归档工程编号.Text = resultProAttr.identifier;
                                textBox_归档工程名称.Text = resultProAttr.name;
                                textBox_归档建设单位.Text = resultProAttr.unit;
                                dataGridView_项目属性表.DataSource = resultProAttr.customList;
                                dataGridView_项目属性表.ClearSelection();

                                var resultDataList = new List<GetProjectUserModel>();
                                if (HttpGet(AppGlobalModel.GetProjectUser + $"?projectId={resultKeepProTempAttr.projectId}", ref resultDataList))
                                {
                                    #region 先添加列
                                    var roleList = resultDataList.First().roleList;

                                    foreach (var item in roleList)
                                    {
                                        var col = new DataGridViewTextBoxColumn();
                                        //要插入列的类型
                                        col.CellTemplate = new DataGridViewTextBoxCell();
                                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                        col.Name = item.roleName;
                                        col.HeaderText = item.roleName;
                                        col.DataPropertyName = "roleList";
                                        dataGridView_专业人员表.Columns.Add(col);
                                    }
                                    dataGridView_专业人员表.DataSource = resultDataList;
                                    dataGridView_专业人员表.ClearSelection();
                                    #endregion
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
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                    #endregion

                    #region 获取技术资料
                    var resultTechnical = new List<GetKeepProjectTempTechnicalModel>();
                    if (HttpGet(AppGlobalModel.GetKeepProjectTempTechnical + $"?tempAttributeId={applyInfo.guiId}", ref resultTechnical))
                    {
                        //resultTechnical = resultTechnical.OrderBy(o => o.id).ToList();//排序
                        resultTechnical = resultTechnical.OrderBy(o => o.name).ToList(); // 按 name 字段排序

                        string nameStr = "";
                        foreach (var item in resultTechnical)
                        {
                            if (nameStr == item.technicalName)
                            {
                                item.technicalName = "";
                            }
                            else
                            {
                                nameStr = item.technicalName;
                            }
                        }

                        dataGridView_技术资料归档表.DataSource = resultTechnical;
                        dataGridView_技术资料归档表.ClearSelection();
                    }
                    else
                    {
                        this.Close();
                    }
                    #endregion

                    #region 审图版图纸
                    var resultKeepProTempDir = new List<GetKeepProjectTempTechnicalModel>();//因为审图版图纸和技术资料的model一样，所以直接用GetKeepProjectTempTechnicalModel这个model来接收数据
                    if (HttpGet(AppGlobalModel.GetKeepProjectTempDir + $"?parentId=0&tempAttributeId={applyInfo.guiId}&type=0", ref resultKeepProTempDir)) //type=0代表获取审图版图纸的根目录，技术资料的根目录是type=1
                    {
                        foreach (var item in resultKeepProTempDir) //循环根目录数据，添加到treeView_意见控件中
                        {
                            TreeNode root = new TreeNode(); //创建树节点
                            //根目录名称
                            root.Text = item.name; //item.name是接口返回数据中的文件夹名称
                            root.Tag = item; //将接口返回的整个对象数据存入节点的Tag属性中，方便后续点击节点时获取该节点对应的数据
                            treeView_意见.Nodes.Add(root); //将根节点添加到treeView_意见控件中
                        }
                        //展开树节点
                        var resultFileAllData = new GetKeepProjectTempNumModel();
                        if (HttpPost(AppGlobalModel.GetKeepProjectTempNum, $"tempAttributeId={applyInfo.guiId}", ref resultFileAllData)) //获取审图版图纸的文件总数和折合A1数量的接口，参数是tempAttributeId，也就是applyInfo.guiId
                        {
                            if (resultFileAllData != null)
                            {

                                //合计文件数量:
                                label_文件总数.Text = $"文件数量：{resultFileAllData.fileNum}";

                                //判断是医药院就不显示折合A1数量
                                if (GlobalVariables.companyName == "吉林医药设计院有限公司")
                                {
                                    label_折A1数.Visible = false;
                                }
                                //合计A1数量:
                                label_折A1数.Text = $"总A1数量：{resultFileAllData.foldedNum}   A1";
                            }
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                Close();
            }

            #region 加载文件列表
            //queryInfo = new QueryApprovalProjectStructure()
            //{
            //    fileType = applyInfo.fileType, //文件来源0 项目区 1归档区
            //    type = 3,    //发起类型 0购物车 1文件夹 2项目 3文件
            //    fileIds = applyInfo.fileids,  //流id列表  用  ，分割
            //    parentId = "0", //上级ID
            //    applyId = applyId,
            //    tab = "1"
            //};
            //LoadFileList();

            //文件来源是项目区的有下载按钮
            if (applyInfo.fileType == 0)
            {
                button5施工图下载.Visible = true;
            }
            #endregion
        }

        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1拒绝_Click(object sender, EventArgs e)
        {
            // 拒绝流程的时候，找到当前审批节点的上一个审批节点，传给审批结果页面，让审批结果页面知道是哪个节点被拒绝了
            var nodeInfo = applyInfo.nodeList.FirstOrDefault(o => o.sum == 2);
            // 通过审批结果接口传递参数，打开审批结果页面
            var frm = new FrmApprovalResult(applyInfo.id, nodeInfo.id, applyInfo.userId, (applyInfo.processtype_id == "1" ? -2 : -1));
            frm.TopMost = true;//设置填写拒绝结果页面置顶显示
            if (frm.ShowDialog() == DialogResult.OK)
            {
                //ShowSuccessMsg("审批通过！");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            var frmProTram = new FrmProTran();//创建流程办理页面对象
            frmProTram.RefreshPage();//刷新流程办理页面
        }

        /// <summary>
        /// 通过
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void button2通过_Click(object sender, EventArgs e)
        //{
        //    // 确认提示（仅针对特定公司）
        //    if (GlobalVariables.companyName == "吉林医药设计院有限公司")
        //    {
        //        var confirm = MessageBox.Show("您确定要通过本流程吗？", "确认", MessageBoxButtons.OKCancel);
        //        if (confirm != DialogResult.OK) return;
        //    }
        //    // 拿到当前需要处理的节点（按原始顺序快照）
        //    foreach (var item in applyInfo.nodeList.Where(o => o.sum == 2))
        //    {
        //        var param = new
        //        {
        //            applyNodeId = item.id,//节点id
        //            result = 1, //1通过 -1不通过(出版的时候 -1下载 1完成)
        //            sendType = 0,
        //            userIds = ""
        //        };

        //        var resultData = string.Empty;
        //        if (HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData))
        //        {
        //            if (applyInfo.processtype_id == "3")//判断是不是归档流程
        //            {
        //                if (resultData == "-1")//-1没通过
        //                {
        //                    var frm = new FrmSelectKeepDir();//创建选择项目归档层级对像
        //                    if (frm.ShowDialog() == DialogResult.OK)
        //                    {
        //                        var archiveDirId = frm.archiveDirId;//档案文件夹Id
        //                        var param1 = new
        //                        {
        //                            applyNodeId = item.id,//节点id
        //                            sendType = 0,
        //                            result = 1,//1通过 -1不通过(出版的时候 -1下载 1完成)
        //                            title = archiveDirId
        //                        };
        //                        resultData = string.Empty;
        //                        if (HttpPost(AppGlobalModel.ApprovalResult, param1, ref resultData))
        //                        {
        //                            ShowSuccessMsg("审批通过！");
        //                            this.DialogResult = DialogResult.OK;
        //                            this.Close();
        //                        }
        //                        else
        //                        {
        //                            return;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        ShowErrorMsg("您没有选择归档目录，将为您退出流程！");
        //                        return;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    ShowSuccessMsg("审批通过！");
        //    this.DialogResult = DialogResult.OK;
        //    this.Close();
        //    var frmProTram = new FrmProTran();
        //    frmProTram.RefreshPage();
        //}


        //private void button2通过_Click(object sender, EventArgs e)
        //{
        //    // 确认提示（仅针对特定公司）
        //    if (GlobalVariables.companyName == "吉林医药设计院有限公司")
        //    {
        //        var confirm = MessageBox.Show("您确定要通过本流程吗？", "确认", MessageBoxButtons.OKCancel);
        //        if (confirm != DialogResult.OK) return;
        //    }

        //    // 拿到当前需要处理的节点（按原始顺序快照）
        //    var nodesToApprove = applyInfo.nodeList.Where(o => o.sum == 2).Select(n => n.id).ToList();
        //    if (!nodesToApprove.Any())
        //    {
        //        ShowErrorMsg("没有可审批的节点。");
        //        return;
        //    }

        //    const int maxRefreshAttempts = 8;
        //    const int refreshDelayMs = 800;

        //    foreach (var nodeId in nodesToApprove)
        //    {
        //        // 1) 提交审批结果
        //        var param = new
        //        {
        //            applyNodeId = nodeId,
        //            result = 1,
        //            sendType = 0,
        //            userIds = ""
        //        };

        //        var resultData = string.Empty;
        //        if (!HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData))
        //        {
        //            ShowErrorMsg("提交审批结果失败，已中止。");
        //            return;
        //        }

        //        // 处理归档流程需要额外选择目录的情况（服务端返回 "-1" 时）
        //        if (applyInfo.processtype_id == "3" && resultData == "-1")
        //        {
        //            var frm = new FrmSelectKeepDir();
        //            if (frm.ShowDialog() != DialogResult.OK)
        //            {
        //                ShowErrorMsg("您没有选择归档目录，将为您退出流程！");
        //                return;
        //            }

        //            var archiveDirId = frm.archiveDirId;
        //            var param1 = new
        //            {
        //                applyNodeId = nodeId,
        //                sendType = 0,
        //                result = 1,
        //                title = archiveDirId
        //            };
        //            resultData = string.Empty;
        //            if (!HttpPost(AppGlobalModel.ApprovalResult, param1, ref resultData))
        //            {
        //                ShowErrorMsg("提交归档目录失败，已中止。");
        //                return;
        //            }
        //        }

        //        // 2) 提交成功后，轮询刷新审批详情，直到服务器推进（或达到重试上限）
        //        bool advanced = false;
        //        for (int attempt = 0; attempt < maxRefreshAttempts; attempt++)
        //        {
        //            Thread.Sleep(refreshDelayMs);

        //            // 重新获取 applyInfo（服务器的最新流程状态）
        //            var paramGet = new { id = applyId };
        //            var freshApplyInfo = new ApplyInfoModel();
        //            if (!HttpPost(AppGlobalModel.ApplyInfo, paramGet, ref freshApplyInfo))
        //            {
        //                // 获取失败，继续重试几次
        //                continue;
        //            }

        //            // 更新本地 applyInfo 引用，便于后续步骤使用最新状态
        //            applyInfo = freshApplyInfo;

        //            // 检查当前 nodeId 的状态（找出同 id 的节点并查看 result）
        //            var nodeState = applyInfo.nodeList.FirstOrDefault(n => n.id == nodeId);
        //            if (nodeState == null)
        //            {
        //                // 若服务端返回的节点列表里找不到该节点，说明流程已推进（或节点被移除）
        //                advanced = true;
        //                break;
        //            }

        //            // 如果节点已被标记为已通过（result == 1），说明服务端已处理
        //            if (nodeState.result == 1)
        //            {
        //                advanced = true;
        //                break;
        //            }

        //            // 否则继续等待/重试
        //        }

        //        if (!advanced)
        //        {
        //            // 如果超时仍未推进，可提示用户并继续或中止
        //            var retry = MessageBox.Show("提交后服务器未及时推进流程，是否继续尝试下一个节点？", "提示", MessageBoxButtons.YesNo);
        //            if (retry == DialogResult.No)
        //            {
        //                return;
        //            }
        //        }
        //    }

        //    // 所有节点请求已完成，通知并刷新上层页面
        //    ShowSuccessMsg("审批提交完成，已请求服务器推进流程。");
        //    this.DialogResult = DialogResult.OK;
        //    this.Close();

        //    var frmProTram = new FrmProTran();
        //    frmProTram.RefreshPage();
        //}

        private void button2通过_Click(object sender, EventArgs e)
        {
            // 确认提示（仅针对特定公司）
            if (GlobalVariables.companyName == "吉林医药设计院有限公司")
            {
                // 使用临时 TopMost 窗体作为 owner，确保消息框在所有窗口上方
                DialogResult dr;
                using (Form top = new Form
                       {
                           TopMost = true,
                           StartPosition = FormStartPosition.Manual,
                           Size = new Size(0, 0),
                           Location = new Point(500, 500) 
                       })
                {
                    top.Show();
                    dr = MessageBox.Show(top, "您确定要通过本流程吗？", "确认", MessageBoxButtons.OKCancel);
                }
                if (dr != DialogResult.OK)
                {
                    return;// 返回处理
                }
            }

            // 拿到当前需要处理的节点（按原始顺序快照）
            var nodesToApprove = applyInfo.nodeList.Where(o => o.sum == 2).Select(n => n.id).ToList();
            if (!nodesToApprove.Any())
            {
                ShowErrorMsg("没有可审批的节点。");
                return;
            }

            const int maxRefreshAttempts = 8;
            const int refreshDelayMs = 800;

            foreach (var nodeId in nodesToApprove)
            {
                // 1) 提交审批结果
                var param = new
                {
                    applyNodeId = nodeId,
                    result = 1,
                    sendType = 0,
                    userIds = ""
                };

                var resultData = string.Empty;
                if (!HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData))
                {
                    ShowErrorMsg("提交审批结果失败，已中止。");
                    return;
                }

                // 处理归档流程需要额外选择目录的情况（服务端返回 "-1" 时）
                if (applyInfo.processtype_id == "3" && resultData == "-1")
                {
                    var frm = new FrmSelectKeepDir();
                    if (frm.ShowDialog() != DialogResult.OK)
                    {
                        ShowErrorMsg("您没有选择归档目录，将为您退出流程！");
                        return;
                    }

                    var archiveDirId = frm.archiveDirId;
                    var param1 = new
                    {
                        applyNodeId = nodeId,
                        sendType = 0,
                        result = 1,
                        title = archiveDirId
                    };
                    resultData = string.Empty;
                    if (!HttpPost(AppGlobalModel.ApprovalResult, param1, ref resultData))
                    {
                        ShowErrorMsg("提交归档目录失败，已中止。");
                        return;
                    }
                }

                // 2) 提交成功后，轮询刷新审批详情，直到服务器推进（或达到重试上限）
                bool advanced = false;
                for (int attempt = 0; attempt < maxRefreshAttempts; attempt++)
                {
                    Thread.Sleep(refreshDelayMs);

                    // 重新获取 applyInfo（服务器的最新流程状态）
                    var paramGet = new { id = applyId };
                    var freshApplyInfo = new ApplyInfoModel();
                    if (!HttpPost(AppGlobalModel.ApplyInfo, paramGet, ref freshApplyInfo))
                    {
                        // 获取失败，继续重试几次
                        continue;
                    }

                    // 更新本地 applyInfo 引用，便于后续步骤使用最新状态
                    applyInfo = freshApplyInfo;

                    // 检查当前 nodeId 的状态（找出同 id 的节点并查看 result）
                    var nodeState = applyInfo.nodeList.FirstOrDefault(n => n.id == nodeId);
                    if (nodeState == null)
                    {
                        // 若服务端返回的节点列表里找不到该节点，说明流程已推进（或节点被移除）
                        advanced = true;
                        break;
                    }

                    // 如果节点已被标记为已通过（result == 1），说明服务端已处理
                    if (nodeState.result == 1)
                    {
                        advanced = true;
                        break;
                    }

                    // 否则继续等待/重试
                }

                if (!advanced)
                {
                    // 如果超时仍未推进，可提示用户并继续或中止
                    var retry = MessageBox.Show("提交后服务器未及时推进流程，是否继续尝试下一个节点？", "提示", MessageBoxButtons.YesNo);
                    if (retry == DialogResult.No)
                    {
                        return;
                    }
                }
            }

            // 所有节点请求已完成，通知并刷新上层页面
            ShowSuccessMsg("流程审批完成。");
            this.DialogResult = DialogResult.OK;
            this.Close();

            var frmProTram = new FrmProTran();
            frmProTram.RefreshPage();
        }

        /// <summary>
        /// 出版流程使用完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3完成_Click_1(object sender, EventArgs e)
        {
            ShowSuccessMsg("审批完成！");
            #region 不调用金额页面，默认金额为1；结束流程；
            var para = new
            {
                applyId = applyId,
                money = 1
            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.ApprovalChubanPass, para, ref resultData))
            {
                DialogResult = DialogResult.OK;
            }
            #endregion
            #region 调输入金额页面
            //var frm = new FrmSettlementAmount(applyId);
            //if (frm.ShowDialog() == DialogResult.OK)
            //{
            //    ShowSuccessMsg("审批完成！");
            //    this.Close();
            //}
            #endregion
            this.Close();
            var frmProTram = new FrmProTran();
            frmProTram.RefreshPage();
        }

        //private void Button3完成_Click(object sender, EventArgs e)
        //{
        //    var button = (Button)sender;
        //    var nodeInfo = (NodeListItem)button.Tag;
        //    var applyNodeInfo = applyInfo.nodeList.FirstOrDefault(o => o.id == nodeInfo.id);

        //    var param = new
        //    {
        //        applyNodeId = nodeInfo.id,//节点id  tabPage_流程进度表
        //        sendType = 0,
        //        result = 1 //1通过 -1不通过(出版的时候 -1下载 1完成)
        //    };

        //    var resultData = string.Empty;
        //    if (HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData))
        //    {
        //        //判断是否是归档流程
        //        if (applyInfo.processtype_id == "3")
        //        {
        //            if (resultData == "-1")
        //            {
        //                var frm = new FrmSelectKeepDir();
        //                if (frm.ShowDialog() == DialogResult.OK)
        //                {
        //                    var archiveDirId = frm.archiveDirId;
        //                    var param1 = new
        //                    {
        //                        applyNodeId = nodeInfo.id,//节点id
        //                        sendType = 0,
        //                        result = 1,//1通过 -1不通过(出版的时候 -1下载 1完成)
        //                        title = archiveDirId
        //                    };
        //                    resultData = string.Empty;
        //                    if (HttpPost(AppGlobalModel.ApprovalResult, param1, ref resultData))
        //                    {
        //                        ShowSuccessMsg("审批通过！");
        //                        this.DialogResult = DialogResult.OK;
        //                        this.Close();
        //                    }
        //                    else
        //                    {
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    ShowErrorMsg("您没有选择归档目录，将为您退出流程！");
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                button.Enabled = false;
        //            }
        //        }
        //        else
        //        {
        //            button.Enabled = false;

        //            if (applyInfo.processtype_id != "1")
        //            {
        //                applyNodeInfo.sum = 0;
        //                if (applyInfo.nodeList.Exists(o => o.sum == 2))
        //                {
        //                    panel1.Visible = true;
        //                }
        //                else
        //                {
        //                    panel1.Enabled = false;

        //                    //panel1.Visible = false;
        //                }
        //            }

        //            ShowSuccessMsg($"审批{button.Text}！");
        //        }
        //    }
        //    bool nodeState = false;
        //    var param2 = new
        //    {
        //        id = applyId
        //    };

        //    if (HttpPost(AppGlobalModel.ApplyInfo, param2, ref applyInfo))
        //    {
        //        for (int i = 0; i < applyInfo.nodeList.Count; i++)
        //        {
        //            if (applyInfo.nodeList[i].resultRemark == null)
        //            {
        //                nodeState = false;
        //                break;
        //            }
        //            else
        //            {
        //                nodeState |= true;
        //                //break;
        //            }
        //        }
        //        if (nodeState)
        //        {
        //            #region 不调用金额页面，默认金额为1；结束流程；
        //            var para = new
        //            {
        //                applyId = applyId,
        //                money = 1
        //            };

        //            var resultData1 = string.Empty;
        //            if (HttpPost(AppGlobalModel.ApprovalChubanPass, para, ref resultData1))
        //            {
        //                DialogResult = DialogResult.OK;
        //            }
        //            #endregion
        //            #region 调输入金额页面
        //            //var frm = new FrmSettlementAmount(applyId);
        //            //if (frm.ShowDialog() == DialogResult.OK)
        //            //{
        //            //    ShowSuccessMsg("审批完成！");
        //            //    this.Close();
        //            //}
        //            #endregion
        //            this.DialogResult = DialogResult.OK;
        //            this.Close();
        //        }
        //    }

        //    var frmProTram = new FrmProTran();
        //    frmProTram.RefreshPage();
        //}

        // 在类字段区添加（与其他 private 字段并列）

        /// <summary>
        /// 标记是否正在处理节点操作（通过/完成），用于防止重复点击和重入
        /// </summary>
        private volatile bool isProcessingNodeAction = false;
        
        private void Button3完成_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var nodeInfo = (NodeListItem)button.Tag;
            var applyNodeInfo = applyInfo.nodeList.FirstOrDefault(o => o.id == nodeInfo.id);

            // 防止重复点击/重入
            if (isProcessingNodeAction) return;
            isProcessingNodeAction = true;
            button.Enabled = false;

            try
            {
                var param = new
                {
                    applyNodeId = nodeInfo.id, //节点id
                    sendType = 0,
                    result = 1 //1通过 -1不通过(出版的时候 -1下载 1完成)
                };

                var resultData = string.Empty;
                if (!HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData))
                {
                    ShowErrorMsg("提交审批结果失败。");
                    button.Enabled = true;
                    return;
                }

                // 归档流程需要选择归档目录（服务端返回 -1 表示需要）
                if (applyInfo.processtype_id == "3" && resultData == "-1")
                {
                    var frm = new FrmSelectKeepDir();
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        var archiveDirId = frm.archiveDirId;
                        var param1 = new
                        {
                            applyNodeId = nodeInfo.id,
                            sendType = 0,
                            result = 1,
                            title = archiveDirId
                        };
                        resultData = string.Empty;
                        if (!HttpPost(AppGlobalModel.ApprovalResult, param1, ref resultData))
                        {
                            ShowErrorMsg("提交归档目录失败。");
                            button.Enabled = true;
                            return;
                        }
                        ShowSuccessMsg("审批通过！");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        return;
                    }
                    else
                    {
                        ShowErrorMsg("您没有选择归档目录，将为您退出流程！");
                        return;
                    }
                }

                // 提交成功，禁用按钮并尝试刷新服务器状态，等待服务器推进该节点
                button.Enabled = false;

                // 本地尝试更新状态（如果本地有节点对象，先设置以优化体验）
                if (applyNodeInfo != null)
                {
                    applyNodeInfo.sum = 0;
                }

                // 轮询刷新，等待服务器把节点标记为已通过或移除节点
                const int maxAttempts = 8;
                const int delayMs = 800;
                bool advanced = false;

                for (int attempt = 0; attempt < maxAttempts; attempt++)
                {
                    System.Threading.Thread.Sleep(delayMs);

                    var param2 = new { id = applyId };
                    var fresh = new ApplyInfoModel();
                    if (!HttpPost(AppGlobalModel.ApplyInfo, param2, ref fresh))
                    {
                        // 获取失败，继续重试
                        continue;
                    }

                    applyInfo = fresh;

                    var nodeState = applyInfo.nodeList.FirstOrDefault(n => n.id == nodeInfo.id);
                    if (nodeState == null || nodeState.result == 1)
                    {
                        advanced = true;
                        break;
                    }
                }

                if (!advanced)
                {
                    // 使用临时 TopMost 窗体作为 owner，确保消息框在所有窗口上方
                    DialogResult dr;
                    using (Form top = new Form
                           {
                               TopMost = true,
                               StartPosition = FormStartPosition.Manual,
                               Size = new Size(0, 0),
                               Location = new Point(-2000, -2000) // 隐藏到屏幕外
                           })
                    {
                        top.Show();
                        dr = MessageBox.Show("提交后服务器未及时推进流程，是否继续？", "提示", MessageBoxButtons.YesNo);
                    }
                    if (dr == DialogResult.No)
                    {
                        return;
                    }
                }

                // 更新界面中 panel1/按钮状态
                if (applyInfo.processtype_id != "1")
                {
                    if (applyInfo.nodeList.Exists(o => o.sum == 2))
                    {
                        panel1.Visible = true;
                        panel1.Enabled = true;
                    }
                    else
                    {
                        panel1.Enabled = false;
                    }
                }

                ShowSuccessMsg($"审批{button.Text}！");

                // 如果所有节点都有审批意见（或已处理），则处理出版完成逻辑（保留原有行为）
                bool allNodesHaveRemark = applyInfo.nodeList.All(n => !string.IsNullOrEmpty(n.resultRemark));
                if (allNodesHaveRemark)
                {
                    var para = new
                    {
                        applyId = applyId,
                        money = 1
                    };

                    var resultData1 = string.Empty;
                    if (HttpPost(AppGlobalModel.ApprovalChubanPass, para, ref resultData1))
                    {
                        DialogResult = DialogResult.OK;
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMsg("审批处理发生异常：" + ex.Message);
            }
            finally
            {
                isProcessingNodeAction = false;
                // 强制刷新上层页面（原逻辑）
                var frmProTram = new FrmProTran();
                frmProTram.RefreshPage();
            }
        }
        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4下载_Click_1(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.DownloadApplyFile + $"?applyId={applyInfo.id}", ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
            Splasher.Close();
        }

        private void Button4下载_Click(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));

            var button = (Button)sender;
            var nodeInfo = (NodeListItem)button.Tag;
            var applyNodeInfo = applyInfo.nodeList.FirstOrDefault(o => o.id == nodeInfo.id);
            var param = new
            {
                applyNodeId = nodeInfo.id,//节点id
                sendType = 0,
                result = -1 //1通过 -1不通过(出版的时候 -1下载 1完成)
            };

            var resultData = string.Empty;
            if (HttpPost(AppGlobalModel.ApprovalResult, param, ref resultData))
            {
                if (HttpGet(AppGlobalModel.DownloadApplyFile + $"?applyId={applyInfo.id}", ref resultData))
                {
                    var frm = new FrmDownloadFile(resultData);
                    frm.ShowDialog();
                }
            }
            Splasher.Close();
        }

        /// <summary>
        /// 施工图下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5施工图下载_Click(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));
            var resultData = string.Empty;
            if (HttpGet(AppGlobalModel.DownloadApplyFileTwo + $"?applyId={applyInfo.id}", ref resultData))
            {
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
            Splasher.Close();
        }

        /// <summary>
        /// 导出出版流程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel导出出版流程_Click(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));
            var resultData = string.Empty;
            var para = new
            {
                id = applyInfo.id
            };
            if (HttpPost(AppGlobalModel.ExportApplyInfo, para, ref resultData))
            {
                Splasher.Close();
                var frm = new FrmDownloadFile(resultData);
                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 表格序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView技术资料归档表_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                             e.RowBounds.Location.Y,
                                             dataGridView_项目属性表.RowHeadersWidth - 4,
                                             e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView_项目属性表.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView_项目属性表.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);

            if (dataGridView_技术资料归档表.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepProjectTempTechnicalModel>)dataGridView_技术资料归档表.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //if (dataList[i].must == 0)
                    //{
                    //    dataGridView2.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    //}

                    if (dataList[i].read == 1)
                    {
                        dataGridView_技术资料归档表.Rows[i].Cells[1].Style.ForeColor = Color.Red;// = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
                    }
                }
            }
        }

        /// <summary>
        /// 技术资料查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView技术资料归档表_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                var list = (List<GetKeepProjectTempTechnicalModel>)dataGridView.DataSource;
                var dataInfo = list[e.RowIndex];
                var listUrl = list.Select(o => new PreviewAreaViewModel() { filePath = o.filePath, name = o.name }).ToList();

                var resultModel = string.Empty;
                if (HttpPost(AppGlobalModel.SetKeepProjectTempRead, $"id={dataInfo.id}", ref resultModel))
                {
                    dataGridView_技术资料归档表.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.Red;

                    var frm = new FrmPreviewArea(dataInfo.filePath, applyInfo.fileType, listUrl);
                    frm.Show();
                }
            }
        }

       /// <summary>
/// 分类筛选技术资料
/// </summary>
/// <param name="sourceData">原始数据</param>
        private void 分类筛选技术资料(List<GetKeepProjectTempTechnicalModel> sourceData)
{
    try
    {
        // 定义各分类的关键字
        var 分类关键字 = new Dictionary<string, List<string>>
        {
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
            },
            {
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
            },
        };

        // 清空各分类列表
        石油资料列表.Clear();
        制冷资料列表.Clear();
        农产品资料列表.Clear();
        冷链物流资料列表.Clear();
        食品资料列表.Clear();

        // 为每个数据项进行分类
        foreach (var item in sourceData)
        {
            string itemName = item.technicalName ?? item.name ?? "";
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
                        case "石油":
                            石油资料列表.Add(CloneTechnicalItem(item));
                            已分配到任何分类 = true;
                            break;
                        case "制冷":
                            制冷资料列表.Add(CloneTechnicalItem(item));
                            已分配到任何分类 = true;
                            break;
                        case "农产品":
                            农产品资料列表.Add(CloneTechnicalItem(item));
                            已分配到任何分类 = true;
                            break;
                        case "冷链物流":
                            冷链物流资料列表.Add(CloneTechnicalItem(item));
                            已分配到任何分类 = true;
                            break;
                        case "食品":
                            食品资料列表.Add(CloneTechnicalItem(item));
                            已分配到任何分类 = true;
                            break;
                    }
                }
            }
        }

        // 将未分类的项目保留在主列表中，但不在其他分类中显示
        // 只保留没有被分类的项目
        var 未分类项目 = sourceData.Where(item => 
        {
            string itemName = item.technicalName ?? item.name ?? "";
            return !分类关键字.Values.Any(categoryKeywords => 
                categoryKeywords.Any(keyword => itemName.Contains(keyword)));
        }).ToList();

        // 将未分类项目添加到主技术资料列表
        主技术资料列表.Clear();
        foreach (var item in 未分类项目)
        {
            主技术资料列表.Add(CloneTechnicalItem(item));
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show($"筛选技术资料时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

        /// <summary>
        /// 添加主技术资料列表字段
        /// </summary>
        private List<GetKeepProjectTempTechnicalModel> 主技术资料列表 = new List<GetKeepProjectTempTechnicalModel>();

        /// <summary>
        /// 克隆技术资料项（深度复制）
        /// </summary>
        private GetKeepProjectTempTechnicalModel CloneTechnicalItem(GetKeepProjectTempTechnicalModel source)
        {
            if (source == null)
                return null;

            return new GetKeepProjectTempTechnicalModel
            {
                id = source.id,
                technicalName = source.technicalName,
                name = source.name,
                filePath = source.filePath,
                read = source.read,
                must = source.must
            };
        }

        /// <summary>
        /// 设置分类数据显示
        /// </summary>
        private void 设置分类数据显示()
        {
            try
            {
                // 设置石油分类显示
                if (dataGridView_石油 != null)
                {
                    dataGridView_石油.AutoGenerateColumns = false;
                    dataGridView_石油.DataSource = null;
                    dataGridView_石油.DataSource = 石油资料列表.OrderBy(o => o.name).ToList();
                    dataGridView_石油.ClearSelection();
                }

                // 设置制冷分类显示
                if (dataGridView_制冷 != null)
                {
                    dataGridView_制冷.AutoGenerateColumns = false;
                    dataGridView_制冷.DataSource = null;
                    dataGridView_制冷.DataSource = 制冷资料列表.OrderBy(o => o.name).ToList();
                    dataGridView_制冷.ClearSelection();
                }

                // 设置农产品分类显示
                if (dataGridView_农产品 != null)
                {
                    dataGridView_农产品.AutoGenerateColumns = false;
                    dataGridView_农产品.DataSource = null;
                    dataGridView_农产品.DataSource = 农产品资料列表.OrderBy(o => o.name).ToList();
                    dataGridView_农产品.ClearSelection();
                }

                // 设置冷链物流分类显示
                if (dataGridView_冷链物流 != null)
                {
                    dataGridView_冷链物流.AutoGenerateColumns = false;
                    dataGridView_冷链物流.DataSource = null;
                    dataGridView_冷链物流.DataSource = 冷链物流资料列表.OrderBy(o => o.name).ToList();
                    dataGridView_冷链物流.ClearSelection();
                }

                // 设置食品分类显示
                if (dataGridView_食品 != null)
                {
                    dataGridView_食品.AutoGenerateColumns = false;
                    dataGridView_食品.DataSource = null;
                    dataGridView_食品.DataSource = 食品资料列表.OrderBy(o => o.name).ToList();
                    dataGridView_食品.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"设置分类数据显示时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 表格序号（为所有分类表格）
        /// </summary>
        private void dataGridView_分类表格_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                             e.RowBounds.Location.Y,
                                             dataGridView.RowHeadersWidth - 4,
                                             e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);

            if (dataGridView.Rows.Count > 0)
            {
                var dataList = ((List<GetKeepProjectTempTechnicalModel>)dataGridView.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    if (dataList[i].read == 1)
                    {
                        dataGridView.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                    }
                }
            }
        }

        /// <summary>
        /// 技术资料查看（为所有分类表格）
        /// </summary>
        private void dataGridView_分类表格_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DataGridView dataGridView = (DataGridView)sender;
                var list = (List<GetKeepProjectTempTechnicalModel>)dataGridView.DataSource;
                var dataInfo = list[e.RowIndex];
                var listUrl = list.Select(o => new PreviewAreaViewModel() { filePath = o.filePath, name = o.name }).ToList();

                var resultModel = string.Empty;
                if (HttpPost(AppGlobalModel.SetKeepProjectTempRead, $"id={dataInfo.id}", ref resultModel))
                {
                    dataGridView.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.Red;

                    var frm = new FrmPreviewArea(dataInfo.filePath, applyInfo.fileType, listUrl);
                    frm.Show();
                }
            }
        }



        #region 加载文件列表        
        /// <summary>
        /// 加载文件
        /// </summary>
        private void LoadFileList()
        {
            var resultData = new List<GetKeepProjectDirModel>();
            if (HttpPost(AppGlobalModel.GetApprovalProjectStructureTwo, queryInfo, ref resultData))
            {
                foreach (var item in resultData.OrderBy(o => o.name, new StringRankComparer()))
                {
                    TreeNode root = new TreeNode();
                    //根目录名称
                    if (item.type == 5)
                    {
                        if (GlobalVariables.companyName == "吉林医药设计院有限公司")
                        {
                            root.Text = item.name + "          ";
                        }
                        else
                        {
                            root.Text = item.name + "          " + $"（图幅：{item.frameName} 折合A1：{item.folded}）";
                        }
                    }
                    else
                    {
                        root.Text = item.name;
                    }

                    root.Tag = item;

                    if (queryInfo.parentId == "0")
                    {
                        treeView_图纸文件表.Nodes.Add(root);
                    }
                    else
                    {
                        treeView_图纸文件表.SelectedNode.Nodes.Add(root);
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
                            label_图纸总图.Text = $"文件数量：{resultFileAllData.FileAll}";
                            //判断是医药院就不显示折合A1数量
                            if (GlobalVariables.companyName == "吉林医药设计院有限公司")
                            {
                                label_总A1数.Visible = false;
                            }
                            label_总A1数.Text = $"总A1数量：{resultFileAllData.FoldedAll}   A1";
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
        /// 文件列表单击节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView图纸文件表_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView_图纸文件表.SelectedNode = e.Node;
            var selectInfo = (GetKeepProjectDirModel)e.Node.Tag;
            if (e.Node.Nodes.Count <= 0 && selectInfo.type != 5)
            {
                queryInfo.parentId = selectInfo.id;

                LoadFileList();

                if (treeViewNodeMouseClick)
                {
                    treeView_图纸文件表.SelectedNode.Expand();
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

                    var frm = new FrmPreviewArea(selectInfo.filePath, applyInfo.fileType, listUrl);
                    frm.Show();
                }
            }

            treeViewNodeMouseClick = true;
        }

        private void treeView图纸文件表_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeViewNodeMouseClick = false;
        }
        #endregion

        #region 审图版图纸

        private bool treeView意见NodeMouseClick = true;

        /// <summary>
        /// 鼠标点文件树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView意见_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView_意见.SelectedNode = e.Node;
            var selectInfo = (GetKeepProjectTempTechnicalModel)e.Node.Tag;
            if (e.Node.Nodes.Count <= 0 && selectInfo.type == 0)
            {
                var resultKeepProTempDir = new List<GetKeepProjectTempTechnicalModel>();
                if (HttpGet(AppGlobalModel.GetKeepProjectTempDir + $"?parentId={selectInfo.id}&tempAttributeId={applyInfo.guiId}&type=0", ref resultKeepProTempDir))
                {
                    foreach (var item in resultKeepProTempDir)
                    {
                        TreeNode root = new TreeNode();
                        //根目录名称
                        root.Text = item.name;
                        root.Tag = item;
                        e.Node.Nodes.Add(root);
                    }
                }

                var resultKeepProTempDrawing = new List<GetKeepProjectTempTechnicalModel>();
                if (HttpGet(AppGlobalModel.GetKeepProjectTempDrawing + $"?parentId={selectInfo.id}&tempAttributeId={applyInfo.guiId}", ref resultKeepProTempDrawing))
                {
                    foreach (var item in resultKeepProTempDrawing)
                    {
                        TreeNode root = new TreeNode();

                        if (item.read == 1)
                        {
                            root.ForeColor = Color.Red;
                        }

                        //根目录名称
                        root.Text = item.name;
                        root.Tag = item;
                        e.Node.Nodes.Add(root);
                    }
                }

                if (treeView意见NodeMouseClick)
                {
                    treeView_意见.SelectedNode.Expand();
                }
            }

            if (selectInfo.type == 1)
            {
                var listUrl = new List<PreviewAreaViewModel>();
                foreach (TreeNode item in e.Node.Parent.Nodes)
                {
                    var itemInfo = (GetKeepProjectTempTechnicalModel)item.Tag;
                    if (itemInfo.type == 1)
                    {
                        listUrl.Add(new PreviewAreaViewModel() { filePath = itemInfo.filePath, name = itemInfo.name });
                    }
                }

                var resultModel = string.Empty;
                if (HttpPost(AppGlobalModel.SetKeepProjectTempRead, $"id={selectInfo.id}", ref resultModel))
                {
                    e.Node.ForeColor = Color.Red;
                    var frm = new FrmPreviewArea(selectInfo.filePath, applyInfo.fileType, listUrl);
                    frm.Show();
                }
            }
        }

        /// <summary>
        /// 意见
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView意见_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            treeView意见NodeMouseClick = false;
        }
        #endregion

        private void button取消_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 再次发起审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel流程再发起_Click(object sender, EventArgs e)
        {
            var proInfo = new GetProjectAttributeModel();
            proInfo.id = applyInfo.proId;
            proInfo.name = applyInfo.proName;

            var resultData = new GetKeepProjectDirModel();

            if (!applyInfo.fileids.Contains(","))
            {
                if (!HttpPost(AppGlobalModel.GetApprovalProjectFileInfo, new
                {
                    fileType = applyInfo.fileType, //文件来源0 项目区 1归档区              
                    fileId = applyInfo.fileids,  //流id列表  用  ，分割
                }, ref resultData))
                {
                    return;
                }
            }

            var frm = new FrmInitApproval(proInfo, 3, applyInfo.fileids, applyInfo.fileType, (!applyInfo.fileids.Contains(",") ? (string.IsNullOrWhiteSpace(resultData.pageAll) ? 0 : Convert.ToInt32(resultData.pageAll)) : 0));
            frm.ShowDialog();
        }

        // 添加到类字段区（例如与其他 private 字段并列）
        private bool is施工图Loaded = false;

        /// <summary>
        /// 在点击施工图时，加载审批文件列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadProjectFileS(object sender, TabControlEventArgs e)
        {
            // 只处理“施工图”页（index == 2）
            if (e.TabPageIndex != 2) return;

            // 如果已经加载过，直接返回，防止重复加载
            if (is施工图Loaded) return;

            // 如果 applyInfo 还未初始化，跳过（或根据需要先初始化）
            if (applyInfo == null) return;

            try
            {
                is施工图Loaded = true; // 设为已加载（失败时在 catch 中重置）
                Splasher.Show(typeof(FrmLoading)); // 显示加载动画

                queryInfo = new QueryApprovalProjectStructure() //查询审批文件列表参数
                {
                    fileType = applyInfo.fileType, //文件来源0 项目区 1归档区
                    type = 3,    //发起类型 0购物车 1文件夹 2项目 3文件
                    fileIds = applyInfo.fileids,  //流id列表  用  ，分割
                    parentId = "0", //上级ID
                    applyId = applyId,
                    tab = "1"
                };

                LoadFileList(); //加载文件列表
            }
            catch (Exception ex)
            {
                // 出现异常时允许重试，并提示错误
                is施工图Loaded = false;
                ShowErrorMsg("加载施工图列表失败：" + ex.Message);
            }
            finally
            {
                Splasher.Close(); // 关闭加载动画
            }
        }

        /// <summary>
        /// 加载技术资料列表（在点击技术资料时加载）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadInformation(object sender, EventArgs e)
        {
            var resultTechnical = new List<GetKeepProjectTempTechnicalModel>();
            if (HttpGet(AppGlobalModel.GetKeepProjectTempTechnical + $"?tempAttributeId={applyInfo.guiId}", ref resultTechnical))
            {
                // 对数据进行分类
                分类筛选技术资料(resultTechnical);

                // 为每个分类排序并设置数据源
                设置分类数据显示();
            }
            else
            {
                this.Close();
            }
        }

       
    }
}