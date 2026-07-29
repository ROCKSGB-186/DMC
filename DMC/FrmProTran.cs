using DMC.Helper;
using DMC.Models;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 签名签章审批,出版审批,归档审批
    /// </summary>
    public partial class FrmProTran : BaseForm
    {

        /// <summary>
        /// 查询流程相关内容类型：1流程类型：processtypeid；2审批状态type；3页数pageNum；4幅面pageSize；5项目名称proName；6发起人userName；7开始时间startTime；8结束时间endTime 
        /// </summary>
        private QueryApply queryApply = new QueryApply();
        /// <summary>
        /// 设定每次查询服务器的条数
        /// </summary>
        private int queryApplyPageSize = 30;
        /// <summary>
        /// 总条数   
        /// </summary>
        private int totalRows;
        /// <summary>
        /// 记录点击那个子页面；
        /// </summary>
        private string subType = "";
        /// <summary>
        /// 初始化一个读线下文件的变量；
        /// </summary>
        private List<ApplyListModel> applyListTempFile = new List<ApplyListModel>();
        /// <summary>
        /// 流程窗口初始化
        /// </summary>
        public FrmProTran()
        {
            InitializeComponent();
            comboBox_下拉选择.SelectedIndex = 0;
            dataGridView_流程详情表.DoubleBufferedDataGirdView(true);
            dataGridView_流程详情表.AutoGenerateColumns = false;
            dataGridView_流程详情表.RegistScrollToEndEvent(dataGrid_OnScrollToEnd);
            var dateTime = dateTimePicker_End.Value;
            var oldDateTime = dateTime.AddDays(-15);
            dateTimePicker_Start.Value = oldDateTime;
        }
        /// <summary>
        /// 鼠标滚轮到最下面时发起的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGrid_OnScrollToEnd(object sender, EventArgs e)
        {
            //判断流程总数是不是与表内的行数相同 
            if (totalRows != dataGridView_流程详情表.Rows.Count)
            {
                //供给查询审批流程参数的第几面
                queryApply.pageNum = queryApply.pageNum + 1;
                OfflineLoadApplyList();
            }
        }
        /// <summary>
        /// 默认加载窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmProTran_Load(object sender, EventArgs e)
        {
            btn待审批.BackColor = Color.Orange;
            #region 加载所有事务流程
            queryApply = new QueryApply()
            {
                type = null,
                //供给查询审批流程参数的第1面
                pageNum = 1,
                //供给查询审批流程参数的第10条
                pageSize = queryApplyPageSize,
            };
            //读取服务器内的审批流程
            SystemTempData.ReadApplyListHttpDatas(queryApply, dateTimePicker_Start.Value.Date, dateTimePicker_End.Value.Date);
            //读取服务器内的审批流程
            //SystemTempData.ReadApplyListHttpDatas(queryApply);
            #endregion
            // 查询流程相关内容类型：1流程类型：processtypeid；2审批状态type（0我发起的 1待审批 2已审批，不传就是查询所有）；3页数pageNum；4：pageSize 返回条数；5项目名称proName；6发起人userName；7开始时间startTime；8结束时间endTime 
            queryApply.type = "1";
            subType = "1";
            OfflineLoadApplyList();
        }
        /// <summary>
        /// 服务器查询流程数据方法
        /// </summary>
        private void applyHttpDatas()
        {
            #region 加载所有事务流程
            queryApply = new QueryApply()
            {
                type = null,
                //供给查询审批流程参数的第1面
                pageNum = 1,
                //供给查询审批流程参数的第10条
                pageSize = queryApplyPageSize,
            };
            //读取服务器内的审批流程
            SystemTempData.ReadApplyListHttpDatas(queryApply, dateTimePicker_Start.Value.Date, dateTimePicker_End.Value.Date);
            //读取服务器内的审批流程
            //SystemTempData.ReadApplyListHttpDatas(queryApply);
            #endregion
            // 查询流程相关内容类型：1流程类型：processtypeid；2审批状态type（0我发起的 1待审批 2已审批，不传就是查询所有）；3页数pageNum；4：pageSize 返回条数；5项目名称proName；6发起人userName；7开始时间startTime；8结束时间endTime 
            queryApply.type = "1";
            subType = "";
            OfflineLoadApplyList();
        }

        /// <summary>
        /// 加载列表
        /// </summary>
        private void OfflineLoadApplyList()
        {
            //applyListTempFile = SystemTempData.allApplyListTemp;
            //调用读取本地文件方法赋值这个变量；
            SystemTempData.LoadApplyListFromJson(ref applyListTempFile);
            ///审批时间范围内的临时变量
            var resultResult = new List<ApplyListModel>();
            if (subType == "0")//我发起的
            {
                //循环所有流程
                foreach (var applyListItem in applyListTempFile)
                    //判断流程发起人是否等于当前用户&&时间在指定范围内
                    if (applyListItem.userName == GlobalVariables.userName && Convert.ToDateTime(applyListItem.createTime).Date >= dateTimePicker_Start.Value.Date && Convert.ToDateTime(applyListItem.createTime).Date <= dateTimePicker_End.Value.Date)
                    {
                        //收集符合条件的流程
                        resultResult.Add(applyListItem);
                    }
            }
            else if (subType == "1")//待审批
            {
                foreach (var applyListItem in applyListTempFile)
                    if (applyListItem.result == 0)
                    {
                        resultResult.Add(applyListItem);
                    }
            }
            else if (subType == "2")//已审批
            {
                foreach (var applyListItem in applyListTempFile)
                    if (applyListItem.lastTime != null && Convert.ToDateTime(applyListItem.createTime).Date >= dateTimePicker_Start.Value.Date && Convert.ToDateTime(applyListItem.createTime).Date <= dateTimePicker_End.Value.Date)
                    {
                        resultResult.Add(applyListItem);
                    }
            }
            else if (subType == "")//所有流程
            {
                foreach (var applyListItem in applyListTempFile)
                    if (Convert.ToDateTime(applyListItem.createTime).Date >= dateTimePicker_Start.Value.Date && Convert.ToDateTime(applyListItem.createTime).Date <= dateTimePicker_End.Value.Date)
                    {
                        resultResult.Add(applyListItem);
                    }
            }
            dataGridView_流程详情表.Rows.Clear();
            dataGridView_流程详情表.DataSource = new SortableBindingList<ApplyListModel>(resultResult);
            totalRows = dataGridView_流程详情表.Rows.Count;
            //if (resultResult.Count != 100)
            //{
            //    totalRows = dataGridView_流程详情表.Rows.Count;
            //}
            Splasher.Close();
        }

        /// <summary>
        /// 所有流程、我发起的、待审批、已审批点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, EventArgs e)
        {
            var buttonbackcolor = panel3.BackColor;
            btn所有流程.BackColor = buttonbackcolor;
            btn我发起的.BackColor = buttonbackcolor;
            btn待审批.BackColor = buttonbackcolor;
            btn已审批.BackColor = buttonbackcolor;
            subType = "";
            Button label = (Button)sender;
            if (label.Text.Equals("我发起的"))
            {
                subType = "0";
            }
            else if (label.Text.Equals("待审批"))
            {
                subType = "1";
            }
            else if (label.Text.Equals("已审批"))
            {
                subType = "2";
            }
            //设置点击后的按键颜色
            label.BackColor = Color.Orange;
            //查询流程类型；
            queryApply.type = subType;
            //查询流程页面；
            queryApply.pageNum = 1;
            //加载流程列表；
            //
            //RefreshPage();
            OfflineLoadApplyList();
        }
        /// <summary>
        /// 表格序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                              e.RowBounds.Location.Y,
                                              dataGridView_流程详情表.RowHeadersWidth - 8,
                                              e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView_流程详情表.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView_流程详情表.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        /// <summary>
        /// 单元格格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView_流程详情表.Columns["Column7"].Index == e.ColumnIndex)
            {
                if (e.Value == null)
                {
                    return;
                }
                if (e.Value.ToString().Equals("0"))
                {
                    e.Value = "进行中";
                }
                else if (e.Value.ToString().Equals("1"))
                {
                    e.Value = "已通过";
                }
                else if (e.Value.ToString().Equals("-1") || e.Value.ToString().Equals("-2"))
                {
                    e.Value = "未通过";
                }
                else
                {
                    e.Value = "未知";
                }
            }
        }
        /// <summary>
        /// 搜索按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_搜索(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));
            //加载本地流程审批列表文件"搜索是找当前日期之前的，与刷新不同，刷新是找最新日期的"
            SystemTempData.LoadApplyListFromJson(ref applyListTempFile);
            var selectValue = comboBox_下拉选择.SelectedItem.ToString();

            if (selectValue == "项目名称")
            {
                //项目筛选
                var projectDataTest = applyListTempFile?.Where(o => o.proName.Contains(textBox_搜索关键字.Text.Trim())).ToList();

                dataGridView_流程详情表.DataSource = new SortableBindingList<ApplyListModel>(projectDataTest);
            }
            else if (selectValue == "用户")
            {
                //项目筛选
                var projectDataTest = applyListTempFile?.Where(o => o.userName.Contains(textBox_搜索关键字.Text.Trim())).ToList();
                dataGridView_流程详情表.DataSource = new SortableBindingList<ApplyListModel>(projectDataTest);
            }
            else if (selectValue == "时间")
            {
                applyHttpDatas();
            }
            Splasher.Close();
        }
        /// <summary>
        /// 下拉框选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectValue = comboBox_下拉选择.SelectedItem.ToString();

            if (selectValue == "时间")
            {
                textBox_搜索关键字.Visible = false;
                dateTimePicker_Start.Visible = true;
                label8.Visible = true;
                dateTimePicker_End.Visible = true;
                //button8.Location = new Point(1110, 5);
                button_搜索.Visible = true;
            }
            else
            {
                dateTimePicker_Start.Visible = true;
                label8.Visible = true;
                dateTimePicker_End.Visible = true;
                textBox_搜索关键字.Visible = true;
                //button8.Location = new Point(1079, 5);
                button_搜索.Visible = true;
            }
        }
        /// <summary>
        /// 表格点击按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // 判断点击的单元格是第几行第几列
            if (e.RowIndex > -1 && e.ColumnIndex == 1)
            {
                DataGridView dataGridView = (DataGridView)sender;//获取当前行的数据
                var apply = ((BindingList<ApplyListModel>)dataGridView.DataSource)[e.RowIndex];//获取当前行的数据源
                var frm = new FrmApprovalInfo(apply.id);//打开流程详情
                frm.Show();
            }
        }
        /// <summary>
        /// 刷新按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Refresh_Click(object sender, EventArgs e)
        {
            Splasher.Show(typeof(FrmLoading));
            RefreshPage();
            Splasher.Close();
        }
        /// <summary>
        /// 刷新按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void RefreshPage()
        {
            queryApply = new QueryApply()
            {
                ///流程类型
                type = null,
                ///供给查询审批流程参数的第1面
                pageNum = 1,
                ///供给查询审批流程参数的第100条
                pageSize = queryApplyPageSize,
                ///开始时间
                //startTime = Convert.ToDateTime(dateTimePicker_Start.Value).ToString(),
                ///结束时间
                //endTime = Convert.ToDateTime(dateTimePicker_End.Value).ToString(),
            };
            SystemTempData.CreateEmptyApplyListJsonFile();
            var endDateTime = dateTimePicker_End.Value.Date;
            //var startDateTime = dateTime.AddDays(-7);
            var startDateTime= dateTimePicker_Start.Value.Date;
            //读取服务器内的审批流程
            SystemTempData.ReadApplyListHttpDatas(queryApply, startDateTime, endDateTime);

            OfflineLoadApplyList();
        }
    }
}
