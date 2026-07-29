using DMC.Helper;
using DMC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace DMC
{
    /// <summary>
    /// 消息管理
    /// </summary>
    public partial class FrmMessageManage : BaseForm
    {
        private QueryMessage queryMessage = null;
        /// <summary>
        /// 总条数
        /// </summary>
        private int total = 0;
        public FrmMessageManage()
        {
            InitializeComponent();

            dataGridView1.DoubleBufferedDataGirdView(true);
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.RegistScrollToEndEvent(dataGrid_OnScrollToEnd);
        }

        private void dataGrid_OnScrollToEnd(object sender, EventArgs e)
        {
            if (total != dataGridView1.Rows.Count)
            {
                queryMessage.pageNum = queryMessage.pageNum + 1;
                LoadMessage();
            }
        }

        private void FrmMessageManage_Load(object sender, EventArgs e)
        {
            queryMessage = new QueryMessage()
            {
                isRead = "",    //是否已读
                pageNum = 1,  //页数
                pageSize = 100,  //条数
            };
            btn所有消息.BackColor = Color.Orange;
            LoadMessage();
        }

        /// <summary>
        /// 单元格格式化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns["Column4"].Index == e.ColumnIndex)
            {
                if (e.Value == null)
                {
                    return;
                }
                if (e.Value.ToString().Equals("0"))
                {
                    e.Value = "未读";
                }
                else if (e.Value.ToString().Equals("1"))
                {
                    e.Value = "已读";
                }
                else
                {
                    e.Value = "未知";
                }
            }
        }

        /// <summary>
        /// 序号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                                              e.RowBounds.Location.Y,
                                              dataGridView1.RowHeadersWidth - 4,
                                              e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            dataGridView1.RowHeadersDefaultCellStyle.Font,
            rectangle,
            dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
            TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void button_Click(object sender, EventArgs e)
        {
            var buttonbackcolor = panel1.BackColor;
            btn所有消息.BackColor = buttonbackcolor;
            btn已读.BackColor = buttonbackcolor;
            btn未读.BackColor = buttonbackcolor;
            string type = "";
            Button label = (Button)sender;
            if (label.Text.Equals("已读"))
            {
                type = "1";
            }
            else if (label.Text.Equals("未读"))
            {
                type = "0";
            }

            label.BackColor = Color.Orange;

            queryMessage.isRead = type;    //是否已读
            queryMessage.pageNum = 1;  //页数

            LoadMessage();
        }

        private void LoadMessage()
        {
            var resultData = new List<MyMessageModel>();
            if (HttpPost(AppGlobalModel.MyMessage, queryMessage, ref resultData, ref total))
            {
                if (queryMessage.pageNum != 1)
                {
                    var list = ((BindingList<MyMessageModel>)dataGridView1.DataSource).ToList();
                    list.AddRange(resultData);
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = new SortableBindingList<MyMessageModel>(list);
                }
                else
                {
                    dataGridView1.DataSource = new SortableBindingList<MyMessageModel>(resultData);
                }

                dataGridView1.ClearSelection();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 0)
            {
                DataGridView dataGridView = (DataGridView)sender;
                var messageInfo = ((BindingList<MyMessageModel>)dataGridView.DataSource)[e.RowIndex];
                var frm = new FrmMessageInfo(messageInfo);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    LoadMessage();
                }
            }
        }
    }

    class QueryMessage
    {
        /// <summary>
        /// 是否已读，不传就是查全部，0未读，1已读
        /// </summary>
        public string isRead { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int pageNum { get; set; }
        /// <summary>
        /// 条数
        /// </summary>
        public int pageSize { get; set; }
    }

}
