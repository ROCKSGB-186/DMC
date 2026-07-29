using DMC;
using DMC.Models;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMC
{
    internal class bakClass
    {

        /*
         
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
            // MinimizeForm();
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

        /// <summary>
        /// 在类的字段部分添加
        /// </summary>
        private List<GetKeepTechnicalNameListModel> 共用表_DataList = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 建筑_DataList = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 石油_DataList = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 农产品_DataList = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 制冷_DataList = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 冷链物流_DataList = new List<GetKeepTechnicalNameListModel>();
        private List<GetKeepTechnicalNameListModel> 食品_DataList = new List<GetKeepTechnicalNameListModel>();


        /// <summary>
        /// 克隆技术资料数据的方法
        /// </summary>
        /// <param name="source">原数据</param>
        /// <returns></returns>
        private GetKeepTechnicalNameListModel CloneTechnicalData(GetKeepTechnicalNameListModel source)
        {
            if (source == null)
                return null;

            return new GetKeepTechnicalNameListModel
            {
                id = source.id ?? string.Empty,
                name = source.name ?? string.Empty,
                localFile = source.localFile ?? string.Empty,
                must = source.must,
                sort = source.sort
            };
        }

       
        /// <summary>
        /// 允许多个分类匹配的筛选方法
        /// </summary>
        /// <param name="sourceData"></param>
        private void FilterTechnicalDataAllowMultipleCategories(List<GetKeepTechnicalNameListModel> sourceData)
        {
            try
            {
                // 清空现有数据
                建筑_DataList.Clear();
                石油_DataList.Clear();
                农产品_DataList.Clear();
                制冷_DataList.Clear();
                冷链物流_DataList.Clear();
                食品_DataList.Clear();

                // 定义每个分类的明确关键字列表
                var 分类关键字配置 = new Dictionary<string, List<string>>
        {
            {
                "建筑",
                new List<string>
                {
                    "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
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
                     "建设工程规划核实合格书","其他","","","",
                }
            },
            {
                "石油",
                new List<string>
                {
                   "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                    "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单","地形图",
                    "红线图","岩土工程勘察报告","会议纪要",
                    "会议签到表","设计方案确认函","施工图启动函",
                    "图纸会审","设计变更","洽商记录","设计函","工作联系单",
                    "建筑工程竣工验收报告","顾客满意度调查表","建设单位意见","施工单位意见",
                    "建筑节能计算书","结构计算书","水计算书","暖通计算书","电计算书","制冷工艺计算书","石油工艺计算书",
                    "设计服务及设计更改文件汇总表","中标通知书","设计变更通知书","初步设计阶段的批复文件",
                    "工程名称变更函","建筑项目选址意见书","建设用地规划许可证",
                    "安全条件论证报告","安全设施设计专篇","环境影响评价报告","地质灾害危险性评价报告书","建筑工程消防验收意见书",
                     "其他"
                }
            },
            {
                "农产品",
                new List<string>
                {
                   "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
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
                     "其他",
                }
            },
            {
                "制冷",
                new List<string>
                {
                    
                }
            },
            {
                "冷链物流",
                new List<string>
                {
                    
                }
            },
            {
                "食品",
                new List<string>
                {
                   "设计项目接口文件汇总表", "设计委托书","破格任命书","设计项目任务书","设计项目计划表",
                    "设计评审记录表","设计文件校审记录表","专业互提条件记录表","工程设计文件发送单",
                    "会议纪要",
                    "设计方案确认函","施工图启动函",
                    "图纸会审","洽商记录","设计函","工作联系单",
                    "顾客满意度调查表",
                    "设计服务及设计更改文件汇总表","设计变更通知书","其他",
                }
            }
        };

                // 遍历所有数据项
                foreach (var item in sourceData)
                {
                    string itemName = item.name ?? string.Empty;
                    bool itemAdded = false;

                    // 检查每个分类
                    foreach (var category in 分类关键字配置.Keys)
                    {
                        bool matchesCategory = 分类关键字配置[category]
                            .Any(keyword => itemName.Contains(keyword));

                        if (matchesCategory)
                        {
                            // 根据分类名称添加到对应列表
                            switch (category)
                            {
                                case "建筑":
                                    建筑_DataList.Add(CloneTechnicalData(item));
                                    itemAdded = true;
                                    break;
                                case "石油":
                                    石油_DataList.Add(CloneTechnicalData(item));
                                    itemAdded = true;
                                    break;
                                case "农产品":
                                    农产品_DataList.Add(CloneTechnicalData(item));
                                    itemAdded = true;
                                    break;
                                case "制冷":
                                    制冷_DataList.Add(CloneTechnicalData(item));
                                    itemAdded = true;
                                    break;
                                case "冷链物流":
                                    冷链物流_DataList.Add(CloneTechnicalData(item));
                                    itemAdded = true;
                                    break;
                                case "食品":
                                    食品_DataList.Add(CloneTechnicalData(item));
                                    itemAdded = true;
                                    break;
                            }
                        }
                    }

                    // 如果没有任何分类匹配，可以选择丢弃该项目
                }

                // 更新数据源
                UpdateDataGridSources();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"筛选技术资料数据时出错: {ex.Message}");
            }
        }

     
        // 更新各个DataGridView的数据源
        private void UpdateDataGridSources()
        {
            try
            {
                // 更新共用表DataGridView GetKeepTechnicalNameListModel
                if (dataGridView_建筑 != null)
                {
                    dataGridView_建筑.DataSource = null;
                    dataGridView_建筑.DataSource = 建筑_DataList.OrderBy(o => o.sort).ToList();
                    dataGridView_建筑.ClearSelection();
                }

                // 更新建筑DataGridView（如果存在）
                // 注意：您需要确保这些DataGridView控件存在

                if (dataGridView_石油 != null)
                {
                    dataGridView_石油.DataSource = null;
                    dataGridView_石油.DataSource = 石油_DataList.OrderBy(o => o.sort).ToList();
                    dataGridView_石油.ClearSelection();
                }
                if (dataGridView_制冷 != null)
                {
                    dataGridView_制冷.DataSource = null;
                    dataGridView_制冷.DataSource = 制冷_DataList.OrderBy(o => o.sort).ToList();
                    dataGridView_制冷.ClearSelection();
                }
                if (dataGridView_农产品 != null)
                {
                    dataGridView_农产品.DataSource = null;
                    dataGridView_农产品.DataSource = 农产品_DataList.OrderBy(o => o.sort).ToList();
                    dataGridView_农产品.ClearSelection();
                }
                if (dataGridView_冷链 != null)
                {
                    dataGridView_冷链.DataSource = null;
                    dataGridView_冷链.DataSource = 冷链物流_DataList.OrderBy(o => o.sort).ToList();
                    dataGridView_冷链.ClearSelection();
                }
                if (dataGridView_食品 != null)
                {
                    dataGridView_食品.DataSource = null;
                    dataGridView_食品.DataSource = 食品_DataList.OrderBy(o => o.sort).ToList();
                    dataGridView_食品.ClearSelection();
                }

                // 类似地更新其他DataGridView...

            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新DataGridView数据源时出错: {ex.Message}");
            }
        }

        /// <summary>
        /// 项目归档加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProjectArchive_Load(object sender, EventArgs e)
        {

            if (GlobalVariables.companyName == "华商国际工程有限公司")
            {
                tabPage4.Text = "审图版图纸及意见";
            }
            //else if (GlobalVariables.companyName == "辽宁方大工程设计有限公司")
            //{
            //    tabPage4.Text = "项目其它附件（可选上传）";
            //}
            //else if (GlobalVariables.companyName == "辽宁省建筑设计研究院有限责任公司")
            //{
            //    tabPage4.Text = "成果图（归档）";
            //}
            else
            {
                tabPage4.Text = "项目其它附件（可选上传）";
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
                ///专业变量
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
                            //要插入列的类型
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
                }
            }
            else
            {
                this.Close();
            }
            #endregion

            #region 加载技术资料   原方法

            ////初始化从服务器中获取技术资料归档的数据表的变量
            //var resultTechnicalData = new List<GetKeepTechnicalNameListModel>();
            ////从服务器中获取技术资料归档的数据表，是否成功，成功，则返回数据表resultTechnicalData中
            //if (HttpGet(AppGlobalModel.GetKeepTechnicalNameList + $"?proId={projectId}", ref resultTechnicalData))
            //{
            //    var technicalInfoValue = ConfigurationManager.AppSettings["TechnicalInfo"];//获取配置文件中的配置参数

            //    if (!string.IsNullOrWhiteSpace(technicalInfoValue))//配置文件不为空
            //    {
            //        //获取配置文件中的配置参数
            //        var loadTechnicalInfoList = JsonConvert.DeserializeObject<List<GetKeepTechnicalNameListModel>>(technicalInfoValue);

            //        foreach (var item in loadTechnicalInfoList)
            //        {
            //            if (resultTechnicalData.Exists(o => o.id == item.id))
            //            {
            //                var dataInfo = resultTechnicalData.FirstOrDefault(o => o.id == item.id);
            //                if (string.IsNullOrWhiteSpace(dataInfo.localFile))
            //                {
            //                    dataInfo.localFile = item.localFile;
            //                }
            //                else
            //                {
            //                    resultTechnicalData.Add(new GetKeepTechnicalNameListModel() { id = item.id, localFile = item.localFile, must = dataInfo.must, sort = dataInfo.sort });
            //                }
            //            }
            //        }
            //    }

            //    dataGridView_共用表.DataSource = resultTechnicalData.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
            //    dataGridView_共用表.ClearSelection();
            //}
            //else
            //{
            //    this.Close();
            //}
            #endregion

            #region 加载技术资料 新方法加入赛选

            // 初始化从服务器中获取技术资料归档的数据表的变量
            var resultTechnicalData = new List<GetKeepTechnicalNameListModel>();
            // 从服务器中获取技术资料归档的数据表，是否成功，成功，则返回数据表resultTechnicalData中
            if (HttpGet(AppGlobalModel.GetKeepTechnicalNameList + $"?proId={projectId}", ref resultTechnicalData))
            {
                var technicalInfoValue = ConfigurationManager.AppSettings["TechnicalInfo"];// 获取配置文件中的配置参数

                if (!string.IsNullOrWhiteSpace(technicalInfoValue))// 配置文件不为空
                {
                    // 获取配置文件中的配置参数
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
                                    sort = dataInfo.sort
                                });
                            }
                        }
                    }
                }
                // 调用筛选方法，将数据分配到不同的列表中
                FilterTechnicalDataAllowMultipleCategories(resultTechnicalData);

                // 原有的数据显示逻辑（如果需要保留）
                //dataGridView_共用表.DataSource = resultTechnicalData.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                //dataGridView_共用表.ClearSelection();
            }
            else
            {
                this.Close();
            }
            #endregion

            //加载文件列表
            //LoadFileList();
        }

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
        /// 数据行绘制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //绘制行号
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                             e.RowBounds.Location.Y,
                                             dataGridView1_项目属性信息.RowHeadersWidth - 4,
                                             e.RowBounds.Height);
            //绘制行号
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView1_项目属性信息.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView1_项目属性信息.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);


            //if (dataGridView_共用表.Rows.Count > 0)
            //{
            //    //获取资料归档的数据
            //    var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_共用表.DataSource);

            //    for (var i = 0; i < dataList.Count; i++)
            //    {
            //        //如果资料归档的数据为必填，则将字体颜色改为红色
            //        if (dataList[i].must == 0)
            //        {
            //            dataGridView_共用表.Rows[i].Cells[0].Style.ForeColor = Color.Red;
            //        }
            //        // 如果资料归档的数据为空，则将按钮改为文本框
            //        if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_共用表.Rows[i].Cells[2] is DataGridViewButtonCell)
            //        {
            //            dataGridView_共用表.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
            //            dataGridView_共用表.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
            //            dataGridView_共用表.Rows[i].Cells[2].Value = "";
            //            dataGridView_共用表.Rows[i].Cells[3].Value = "";
            //        }
            //        //临时技术资料不存在，创建
            //        if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_共用表.Rows[i].Cells[4] is DataGridViewButtonCell)
            //        {
            //            dataGridView_共用表.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
            //            dataGridView_共用表.Rows[i].Cells[4].Value = "";
            //        }
            //    }
            //}
            dataGridViewFormatting();
        }
        /// <summary>
        /// 数据行格式化
        /// </summary>

        private void dataGridViewFormatting()
        {

            if (dataGridView_建筑.Rows.Count > 0)
            {
                //获取资料归档的数据
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_建筑.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //如果资料归档的数据为必填，则将字体颜色改为红色
                    if (dataList[i].must == 0)
                    {
                        dataGridView_建筑.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }
                    // 如果资料归档的数据为空，则将按钮改为文本框
                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_建筑.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_建筑.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_建筑.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_建筑.Rows[i].Cells[2].Value = "";
                        dataGridView_建筑.Rows[i].Cells[3].Value = "";
                    }
                    //临时技术资料不存在，创建
                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_建筑.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_建筑.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_建筑.Rows[i].Cells[4].Value = "";
                    }
                }
            }

            if (dataGridView_石油.Rows.Count > 0)
            {
                //获取资料归档的数据
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_石油.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //如果资料归档的数据为必填，则将字体颜色改为红色
                    if (dataList[i].must == 0)
                    {
                        dataGridView_石油.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }
                    // 如果资料归档的数据为空，则将按钮改为文本框
                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_石油.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_石油.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_石油.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_石油.Rows[i].Cells[2].Value = "";
                        dataGridView_石油.Rows[i].Cells[3].Value = "";
                    }
                    //临时技术资料不存在，创建
                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_石油.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_石油.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_石油.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_制冷.Rows.Count > 0)
            {
                //获取资料归档的数据
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_制冷.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //如果资料归档的数据为必填，则将字体颜色改为红色
                    if (dataList[i].must == 0)
                    {
                        dataGridView_制冷.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }
                    // 如果资料归档的数据为空，则将按钮改为文本框
                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_制冷.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_制冷.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_制冷.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_制冷.Rows[i].Cells[2].Value = "";
                        dataGridView_制冷.Rows[i].Cells[3].Value = "";
                    }
                    //临时技术资料不存在，创建
                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_制冷.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_制冷.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_制冷.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_农产品.Rows.Count > 0)
            {
                //获取资料归档的数据
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_农产品.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //如果资料归档的数据为必填，则将字体颜色改为红色
                    if (dataList[i].must == 0)
                    {
                        dataGridView_农产品.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }
                    // 如果资料归档的数据为空，则将按钮改为文本框
                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_农产品.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_农产品.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_农产品.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_农产品.Rows[i].Cells[2].Value = "";
                        dataGridView_农产品.Rows[i].Cells[3].Value = "";
                    }
                    //临时技术资料不存在，创建
                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_农产品.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_农产品.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_农产品.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_冷链.Rows.Count > 0)
            {
                //获取资料归档的数据
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_冷链.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //如果资料归档的数据为必填，则将字体颜色改为红色
                    if (dataList[i].must == 0)
                    {
                        dataGridView_冷链.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }
                    // 如果资料归档的数据为空，则将按钮改为文本框
                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_冷链.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_冷链.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_冷链.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_冷链.Rows[i].Cells[2].Value = "";
                        dataGridView_冷链.Rows[i].Cells[3].Value = "";
                    }
                    //临时技术资料不存在，创建
                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_冷链.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_冷链.Rows[i].Cells[4] = new DataGridViewTextBoxCell();
                        dataGridView_冷链.Rows[i].Cells[4].Value = "";
                    }
                }
            }
            if (dataGridView_食品.Rows.Count > 0)
            {
                //获取资料归档的数据
                var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_食品.DataSource);

                for (var i = 0; i < dataList.Count; i++)
                {
                    //如果资料归档的数据为必填，则将字体颜色改为红色
                    if (dataList[i].must == 0)
                    {
                        dataGridView_食品.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                    }
                    // 如果资料归档的数据为空，则将按钮改为文本框
                    if (string.IsNullOrWhiteSpace(dataList[i].name) && dataGridView_食品.Rows[i].Cells[2] is DataGridViewButtonCell)
                    {
                        dataGridView_食品.Rows[i].Cells[2] = new DataGridViewTextBoxCell();
                        dataGridView_食品.Rows[i].Cells[3] = new DataGridViewTextBoxCell();
                        dataGridView_食品.Rows[i].Cells[2].Value = "";
                        dataGridView_食品.Rows[i].Cells[3].Value = "";
                    }
                    //临时技术资料不存在，创建
                    if (string.IsNullOrWhiteSpace(dataList[i].localFile) && dataGridView_食品.Rows[i].Cells[4] is DataGridViewButtonCell)
                    {
                        dataGridView_食品.Rows[i].Cells[4].Value = "";
                    }
                }
            }

        }

        /// <summary>
        /// 技术资料列表内容点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //点击button按钮事件
            if (dataGridView_建筑.Columns[e.ColumnIndex].Name == "upload" && e.RowIndex >= 0)
            {
                //临时技术资料不存在，创建
                if (!Directory.Exists(AppGlobalModel.TechnicalInfoUrl))
                {
                    Directory.CreateDirectory(AppGlobalModel.TechnicalInfoUrl);
                }

                DataGridView dataGridView = (DataGridView)sender;
                var list = (List<GetKeepTechnicalNameListModel>)dataGridView.DataSource;
                var dataInfo = list[e.RowIndex];
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = AppGlobalModel.InitialDirectory;
                openFileDialog.Filter = "所有文件(*.*)|*.*";
                openFileDialog.Multiselect = true;//可以选择多个选项
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    #region 保存打开的文件目录
                    AppGlobalModel.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                    ConfigHelper.SaveConfigInfo("InitialDirectory", AppGlobalModel.InitialDirectory);
                    #endregion

                    #region 获取本地技术资料
                    var technicalInfoValue = ConfigurationManager.AppSettings["TechnicalInfo"];
                    var loadTechnicalInfoList = new List<GetKeepTechnicalNameListModel>();
                    if (!string.IsNullOrWhiteSpace(technicalInfoValue))
                    {
                        loadTechnicalInfoList = JsonConvert.DeserializeObject<List<GetKeepTechnicalNameListModel>>(technicalInfoValue);
                    }
                    #endregion

                    string[] strNames = openFileDialog.FileNames;
                    for (int i = 0; i < strNames.Length; i++)
                    {
                        if (!list.Exists(o => o.name == dataInfo.name && o.localFile == strNames[i]))
                        {
                            if (string.IsNullOrWhiteSpace(dataInfo.localFile))
                            {
                                dataInfo.localFile = strNames[i];
                            }
                            else
                            {
                                list.Add(new GetKeepTechnicalNameListModel() { id = dataInfo.id, localFile = strNames[i], must = dataInfo.must, sort = dataInfo.sort });
                            }

                            if (!Directory.Exists(AppGlobalModel.TechnicalInfoUrl + "\\" + dataInfo.id))
                            {
                                Directory.CreateDirectory(AppGlobalModel.TechnicalInfoUrl + "\\" + dataInfo.id);   //目标目录下不存在此文件夹即创建子文件夹
                            }

                            File.Copy(strNames[i], AppGlobalModel.TechnicalInfoUrl + "\\" + dataInfo.id + "\\" + Path.GetFileName(strNames[i]), true);

                            loadTechnicalInfoList.Add(new GetKeepTechnicalNameListModel() { id = dataInfo.id, localFile = AppGlobalModel.TechnicalInfoUrl + "\\" + dataInfo.id + "\\" + Path.GetFileName(strNames[i]) });
                        }
                    }

                    if (loadTechnicalInfoList != null && loadTechnicalInfoList.Any())
                    {
                        //保存的技术资料
                        ConfigHelper.SaveConfigInfo("TechnicalInfo", JsonConvert.SerializeObject(loadTechnicalInfoList));
                    }

                    dataGridView_建筑.DataSource = null;
                    //刷新数据源
                    dataGridView_建筑.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                }
            }
            else if (dataGridView_建筑.Columns[e.ColumnIndex].Name == "download" && e.RowIndex >= 0)
            {
                DataGridView dataGridView = (DataGridView)sender;
                var dataInfo = ((List<GetKeepTechnicalNameListModel>)dataGridView.DataSource)[e.RowIndex];

                var frm = new FrmDownloadFile(dataInfo.filePath);
                frm.ShowDialog();
            }
            else if (dataGridView_建筑.Columns[e.ColumnIndex].Name == "delFile" && e.RowIndex >= 0)
            {
                if (ShowSuccessOKCancelMsg($"是否确定删除文件！") == DialogResult.OK)
                {
                    DataGridView dataGridView = (DataGridView)sender;
                    var list = (List<GetKeepTechnicalNameListModel>)dataGridView.DataSource;
                    var dataInfo = ((List<GetKeepTechnicalNameListModel>)dataGridView.DataSource)[e.RowIndex];

                    #region 删除本地技术资料                   
                    var technicalInfoValue = ConfigurationManager.AppSettings["TechnicalInfo"];
                    var loadTechnicalInfoList = new List<GetKeepTechnicalNameListModel>();
                    if (!string.IsNullOrWhiteSpace(technicalInfoValue))
                    {
                        loadTechnicalInfoList = JsonConvert.DeserializeObject<List<GetKeepTechnicalNameListModel>>(technicalInfoValue);

                        var fistData = loadTechnicalInfoList.FirstOrDefault(o => o.id == dataInfo.id && o.localFile == AppGlobalModel.TechnicalInfoUrl + "\\" + dataInfo.id + "\\" + Path.GetFileName(dataInfo.localFile));

                        loadTechnicalInfoList.Remove(fistData);

                        File.Delete(AppGlobalModel.TechnicalInfoUrl + "\\" + dataInfo.id + "\\" + Path.GetFileName(dataInfo.localFile));

                        //保存的技术资料
                        ConfigHelper.SaveConfigInfo("TechnicalInfo", JsonConvert.SerializeObject(loadTechnicalInfoList));
                    }
                    #endregion

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

                    dataGridView_建筑.DataSource = null;
                    dataGridView_建筑.DataSource = list.OrderBy(o => o.sort).ThenByDescending(o => o.name).ToList();
                }
            }
            //滚动条索引
            dataGridView_建筑.FirstDisplayedScrollingRowIndex = VerticalScrollIndex;
            //滚动条位置
            dataGridView_建筑.HorizontalScrollingOffset = HorizontalOffset;
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
            if (dataGridView_建筑.Columns["Column1"].Index == e.ColumnIndex)
            {
                if (e.Value != null)
                {
                    e.Value = Path.GetFileName(e.Value.ToString());
                }
            }
            //if (dataGridView_建筑.Columns["Column1"].Index == e.ColumnIndex)
            //{
            //    if (e.Value != null)
            //    {
            //        e.Value = Path.GetFileName(e.Value.ToString());
            //    }
            //}
            //if (dataGridView_石油.Columns["Column1"].Index == e.ColumnIndex)
            //{
            //    if (e.Value != null)
            //    {
            //        e.Value = Path.GetFileName(e.Value.ToString());
            //    }
            //}
            //if (dataGridView_农产品.Columns["Column1"].Index == e.ColumnIndex)
            //{
            //    if (e.Value != null)
            //    {
            //        e.Value = Path.GetFileName(e.Value.ToString());
            //    }
            //}
            //if (dataGridView_制冷.Columns["Column1"].Index == e.ColumnIndex)
            //{
            //    if (e.Value != null)
            //    {
            //        e.Value = Path.GetFileName(e.Value.ToString());
            //    }
            //}
            //if (dataGridView_冷链.Columns["Column1"].Index == e.ColumnIndex)
            //{
            //    if (e.Value != null)
            //    {
            //        e.Value = Path.GetFileName(e.Value.ToString());
            //    }
            //}
            //if (dataGridView_食品.Columns["Column1"].Index == e.ColumnIndex)
            //{
            //    if (e.Value != null)
            //    {
            //        e.Value = Path.GetFileName(e.Value.ToString());
            //    }
            //}

        }

        int VerticalScrollIndex = 0;
        int HorizontalOffset = 0;
        /// <summary>
        /// 滚动条滚动事件
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

        /// <summary>
        /// 审图版图纸上传附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn上传附件_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件夹";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.Nodes.Clear();
                directoryStructureList = new List<DirectoryStructureModel>();

                var thisOne = new DirectoryInfo(dialog.SelectedPath);

                ListTreeShow(thisOne, "", null);

                treeView1.ExpandAll();
            }
        }

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
                treeView1.Nodes.Add(root);
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
        /// 发起项目归档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            var dataList = ((List<GetKeepTechnicalNameListModel>)dataGridView_建筑.DataSource);

            /*
            if (dataList.Exists(o => o.must==0 && string.IsNullOrWhiteSpace(o.localFile)))
            {
                ShowErrorMsg("请上传技术资料！");
                return;
            }

            if (directoryStructureList==null || !directoryStructureList.Any())
            {
                ShowErrorMsg("请上传审图版图纸！");
                return;
            }
           

        var paraInfo = new AddKeepProjectAttributeModel()
        {
            oneStartTime = dateTimePicker1.Text.ToString(),//可行性研究开始时间
            oneEndTime = dateTimePicker2.Text.ToString(),//可行性研究结束时间
            twoStartTime = dateTimePicker4.Text.ToString(),//前期工作开始时间
            twoEndTime = dateTimePicker3.Text.ToString(),//前期工作结束时间
            threeStartTime = dateTimePicker6.Text.ToString(),//初步设计开始时间
            threeEndTime = dateTimePicker5.Text.ToString(),//初步设计结束时间
            fourStartTime = dateTimePicker8.Text.ToString(),//施工图开始时间
            fourEndTime = dateTimePicker7.Text.ToString(),//施工图结束时间
            projectId = projectId,//项目id
            other = "",//其他
            remarks = textBox28.Text.Trim()//备注
        };

        FrmArchiveProgress frm = new FrmArchiveProgress(dataList, directoryStructureList, paraInfo);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                #region 获取项目属性信息
                var resultProjectData = new GetProjectAttributeModel();
                if (HttpGet(AppGlobalModel.GetProjectAttribute + $"?projectId={projectId}", ref resultProjectData))
                {
                    var frmApproval = new FrmInitApproval(resultProjectData, 2, resultProjectData.id, 0, 0, frm.resultDataModel);

        //清空保存的技术资料
        ConfigHelper.SaveConfigInfo("TechnicalInfo", "");

                    //删除技术资料文件
                    if (Directory.Exists(AppGlobalModel.TechnicalInfoUrl))
                    {
                        Directory.Delete(AppGlobalModel.TechnicalInfoUrl, true);
                    }

                    this.Hide();
        frmApproval.ShowDialog();
        }
        #endregion
    }
}
//dataGridView4
#region 加载文件列表
/// <summary>
/// 加载文件列表
/// </summary>
private void LoadFileList()
{
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
                treeView2.Nodes.Add(root);
            }
            else
            {
                treeView2.SelectedNode.Nodes.Add(root);
            }
        }

        //第一次加载要加载文件汇总
        if (queryInfos.parentId == "0")
        {
            var resultFileAllData = new GetApprovalProjectStructureAllModel();
            if (HttpPost(AppGlobalModel.GetApprovalProjectStructureAll, queryInfos, ref resultFileAllData))
            {
                if (resultFileAllData != null)
                {
                    label15.Text = $"文件数量：{resultFileAllData.FileAll}";
                    label2.Text = $"总A1数量：{resultFileAllData.FoldedAll}   A1";
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
        if (queryInfos.parentId == "0")
        {
            this.Close();
        }
    }
}

private bool treeViewNodeMouseClick = true;
/// <summary>
/// 文件列表节点单击事件
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void treeView2_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
{
    treeView2.SelectedNode = e.Node;
    var selectInfo = (GetKeepProjectDirModel)e.Node.Tag;
    if (e.Node.Nodes.Count <= 0 && selectInfo.type != 5)
    {
        queryInfos.parentId = selectInfo.id;
        LoadFileList();

        if (treeViewNodeMouseClick)
        {
            treeView1.SelectedNode.Expand();
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

private void treeView2_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
{
    treeViewNodeMouseClick = false;
}
#endregion

/// <summary>
/// 取消
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void button1_Click(object sender, EventArgs e)
{
    this.Close();
}

private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
{
    var dateTimePicker = (DateTimePicker)sender;
    //这里可以个更改自己的需要的格式，例：yyyy年MM月dd日
    dateTimePicker.CustomFormat = "yyyy-MM-dd";
}

private void btn_保存资料_Click(object sender, EventArgs e)
{

}

private void 施工图_Click(object sender, EventArgs e)
{
    if (treeViewNodeMouseClick)
    {
        //加载文件列表
        LoadFileList();
    }

}
*/






    }
}
