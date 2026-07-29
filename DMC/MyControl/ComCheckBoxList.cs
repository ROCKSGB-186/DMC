using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.CheckedListBox;

namespace DMC.MyControl
{
    /// <summary>
    /// 带下拉框的用户控件
    /// </summary>
    public partial class ComCheckBoxList : UserControl
    {
        private TextBox tbSelectedValue;
        private ButtonS btnSelect;//下拉箭头
        private LabelS lbGrip;//此LABEL用于设置可以拖动下拉窗体变化

        private CheckedListBox checkListBox;
        private Label lbSelectAll;//全选
        private Label lbSelectNo;//取消

        private Form frmCheckList;

        private Panel pnlBack;
        private Panel pnlCheck;

        private System.Drawing.Point DragOffset; //用于记录窗体大小变化的位置

        //单击列表项状态更改事件
        public delegate void CheckBoxListItemClick(object sender, ItemCheckEventArgs e);
        public event CheckBoxListItemClick ItemClick;

        public ComCheckBoxList()
        {
            InitializeComponent();
            this.Name = "comBoxCheckBoxList";
            this.Layout += new LayoutEventHandler(ComCheckBoxList_Layout);

            //生成控件
            tbSelectedValue = new TextBox();
            tbSelectedValue.ReadOnly = true;
            tbSelectedValue.Click += new EventHandler(btnSelect_Click);
            tbSelectedValue.BorderStyle = BorderStyle.None;

            //下拉箭头
            this.btnSelect = new ButtonS();
            btnSelect.FlatStyle = FlatStyle.Flat;
            btnSelect.Click += new EventHandler(btnSelect_Click);

            //全选
            this.lbSelectAll = new Label();
            lbSelectAll.BackColor = Color.Transparent;
            lbSelectAll.Text = "全选";
            lbSelectAll.Size = new Size(40, 20);
            lbSelectAll.ForeColor = Color.Blue;
            lbSelectAll.Cursor = Cursors.Hand;
            lbSelectAll.TextAlign = ContentAlignment.MiddleCenter;
            lbSelectAll.Click += new EventHandler(lbSelectAll_Click);

            //取消
            lbSelectNo = new Label();
            lbSelectNo.BackColor = Color.Transparent;
            lbSelectNo.Text = "取消";
            lbSelectNo.Size = new Size(40, 20);
            lbSelectNo.ForeColor = Color.Blue;
            lbSelectNo.Cursor = Cursors.Hand;
            lbSelectNo.TextAlign = ContentAlignment.MiddleCenter;
            lbSelectNo.Click += new EventHandler(lbSelectNo_Click);

            //生成checkboxlist
            this.checkListBox = new CheckedListBox();
            checkListBox.BorderStyle = BorderStyle.None;
            checkListBox.Location = new Point(0, 0);
            checkListBox.CheckOnClick = true;
            checkListBox.ScrollAlwaysVisible = true;
            checkListBox.Font = new Font(new FontFamily("微软雅黑"), 12);
            checkListBox.LostFocus += new EventHandler(checkListBox_LostFocus);
            checkListBox.ItemCheck += new ItemCheckEventHandler(checkListBox_ItemCheck);

            //窗体
            frmCheckList = new Form();
            frmCheckList.FormBorderStyle = FormBorderStyle.None;
            frmCheckList.StartPosition = FormStartPosition.Manual;
            frmCheckList.BackColor = SystemColors.Control;
            frmCheckList.ShowInTaskbar = false;

            //可拖动窗体大小变化的LABEL
            lbGrip = new LabelS();
            lbGrip.Size = new Size(9, 18);
            lbGrip.BackColor = Color.Transparent;
            lbGrip.Cursor = Cursors.SizeNWSE;
            lbGrip.MouseDown += new MouseEventHandler(lbGrip_MouseDown);
            lbGrip.MouseMove += new MouseEventHandler(lbGrip_MouseMove);

            //panel
            pnlBack = new Panel();
            pnlBack.BorderStyle = BorderStyle.Fixed3D;
            pnlBack.BackColor = Color.White;
            pnlBack.AutoScroll = false;

            //
            pnlCheck = new Panel();
            pnlCheck.BorderStyle = BorderStyle.FixedSingle;
            pnlCheck.BackColor = Color.White; ;

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);

            pnlBack.Controls.Add(tbSelectedValue);
            pnlBack.Controls.Add(btnSelect);

            this.Controls.Add(pnlBack);

            pnlCheck.Controls.Add(checkListBox);
            pnlCheck.Controls.Add(lbSelectAll);
            pnlCheck.Controls.Add(lbSelectNo);
            pnlCheck.Controls.Add(lbGrip);
            this.frmCheckList.Controls.Add(pnlCheck);


        }

        private void ReloationGrip()
        {

            lbGrip.Top = this.frmCheckList.Height - lbGrip.Height - 1;
            lbGrip.Left = this.frmCheckList.Width - lbGrip.Width - 1;

            lbSelectAll.Left = 5;
            lbSelectAll.Top = frmCheckList.Height - lbSelectAll.Height;

            lbSelectNo.Left = 50;
            lbSelectNo.Top = frmCheckList.Height - lbSelectNo.Height;


        }

        #region 事件


        //布局
        private void ComCheckBoxList_Layout(object sender, LayoutEventArgs e)
        {
            this.Height = tbSelectedValue.Height + 6;
            this.pnlBack.Size = new Size(this.Width, this.Height - 2);

            //设置按钮的位置
            this.btnSelect.Size = new Size(20, this.Height - 6);
            btnSelect.Location = new Point(this.Width - this.btnSelect.Width - 4, 0);

            this.tbSelectedValue.Location = new Point(2, 2);
            this.tbSelectedValue.Width = this.Width - btnSelect.Width - 4;

            checkListBox.Height = 150;

            //设置窗体
            this.frmCheckList.Size = new Size(this.Width, this.checkListBox.Height);
            this.pnlCheck.Size = frmCheckList.Size;

            this.checkListBox.Width = this.frmCheckList.Width;
            this.checkListBox.Height = this.frmCheckList.Height - lbSelectNo.Height;

            ReloationGrip();


        }
        /// <summary>
        /// 单价下拉框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnSelect_Click(object sender, EventArgs e)
        {
            if (this.frmCheckList.Visible == false)
            {
                Rectangle rec = this.RectangleToScreen(this.ClientRectangle);
                this.frmCheckList.Location = new Point(rec.X, rec.Y + this.pnlBack.Height);
                this.frmCheckList.Show();
                this.frmCheckList.BringToFront();

                ReloationGrip();
            }
            else
                this.frmCheckList.Hide();
        }

        //全选事件
        private void lbSelectAll_Click(object sender, EventArgs e)
        {
            var valueStr = string.Empty;
            for (int i = 0; i < checkListBox.Items.Count; i++)
            {
                checkListBox.SetItemChecked(i, true);
                valueStr += checkListBox.GetItemText(checkListBox.Items[i]) + ",";
            }
            tbSelectedValue.Text = valueStr.TrimEnd(',');
        }
        //取消
        private void lbSelectNo_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkListBox.Items.Count; i++)
            {
                checkListBox.SetItemChecked(i, false);
            }
            tbSelectedValue.Text = "";
        }

        private void checkListBox_LostFocus(object sender, EventArgs e)
        {
            //如果鼠标位置在下拉框按钮的以为地方，则隐藏下拉框
            if (!this.btnSelect.RectangleToScreen(this.btnSelect.ClientRectangle).Contains(Cursor.Position))
            {
                frmCheckList.Hide();
            }
        }

        private void checkListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (ItemClick != null)
            {
                ItemClick(sender, e);
            }

            var valueStr = string.Empty;

            if (e.NewValue == CheckState.Checked)
            {
                for (int i = 0; i < checkListBox.Items.Count; i++)
                {
                    if (this.checkListBox.CheckedItems.Contains(this.checkListBox.Items[i]))
                    {
                        valueStr += checkListBox.GetItemText(checkListBox.Items[i]) + ",";
                    }

                    if (i == e.Index)
                    {
                        valueStr += checkListBox.GetItemText(checkListBox.Items[i]) + ",";
                    }
                }
            }
            else
            {
                for (int i = 0; i < checkListBox.Items.Count; i++)
                {
                    if (this.checkListBox.CheckedItems.Contains(this.checkListBox.Items[i]))
                    {
                        if (i != e.Index)
                        {
                            valueStr += checkListBox.GetItemText(checkListBox.Items[i]) + ",";
                        }
                    }
                }
            }

            tbSelectedValue.Text = valueStr.TrimEnd(',');
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbGrip_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int offsetX = System.Math.Abs(Cursor.Position.X - frmCheckList.RectangleToScreen(this.frmCheckList.ClientRectangle).Right);
                int offsetY = System.Math.Abs(Cursor.Position.Y - frmCheckList.RectangleToScreen(this.frmCheckList.ClientRectangle).Bottom);
                this.DragOffset = new Point(offsetX, offsetY);
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbGrip_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //获取拉伸长度
                int curWidth = Cursor.Position.X - frmCheckList.Location.X;
                int curHeight = Cursor.Position.Y - frmCheckList.Location.Y;
                if (curWidth < this.Width)
                {
                    curWidth = this.Width;
                }

                if (curHeight < checkListBox.Height)
                {
                    curHeight = checkListBox.Height;
                }

                this.frmCheckList.Size = new Size(this.Width, curHeight);
                this.pnlCheck.Size = frmCheckList.Size;
                this.checkListBox.Height = (this.frmCheckList.Height - lbGrip.Height) < 50 ? 50 : this.frmCheckList.Height - lbGrip.Height;

                ReloationGrip();
                SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
                SetStyle(ControlStyles.ResizeRedraw, true);
                SetStyle(ControlStyles.UserPaint, true);
                SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            }
        }

        #endregion

        /// <summary>
        /// 设置数据源
        /// </summary>
        public object DataSource
        {
            set
            {
                this.checkListBox.DataSource = value;
            }
            get
            {
                return checkListBox.DataSource;
            }
        }
        /// <summary>
        /// 设置值
        /// </summary>
        public string ValueMember
        {
            set
            {
                checkListBox.ValueMember = value;
            }
        }
        /// <summary>
        /// 设置显示名称
        /// </summary>
        public string DisplayMember
        {
            set
            {
                checkListBox.DisplayMember = value;
            }
        }

        /// <summary>
        /// 添加项
        /// </summary>
        public int AddItems(object value)
        {
            checkListBox.Items.Add(value);
            return checkListBox.Items.Count;
        }

        /// <summary>
        /// 选项集合
        /// </summary>
        public CheckedListBox.ObjectCollection Items
        {
            get
            {
                return checkListBox.Items;
            }
        }

        /// <summary>
        /// 获取选中项的文本
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string GetItemText(object item)
        {
            return checkListBox.GetItemText(item);
        }

        public void SetItemDefaultValue(int index)
        {
            checkListBox.SetItemChecked(index, true);

            var valueStr = string.Empty;
            for (int i = 0; i < checkListBox.Items.Count; i++)
            {
                if (checkListBox.GetItemChecked(i))
                {
                    valueStr += checkListBox.GetItemText(checkListBox.Items[i]) + ",";
                }                
            }

            tbSelectedValue.Text = valueStr.TrimEnd(',');
        }

        public string GetText()
        {
            return tbSelectedValue.Text;
        }

        //清空数据
        public void ClearData()
        {
            for (int i = 0; i < checkListBox.Items.Count; i++)
            {
                checkListBox.SetItemChecked(i, false);
            }
            tbSelectedValue.Text = "";
        }

        public CheckedItemCollection GetValue()
        {
            return this.checkListBox.CheckedItems;
        }

        //设置全选
        public void SelectAllItems()
        {
            var valueStr = string.Empty;
            for (int i = 0; i < checkListBox.Items.Count; i++)
            {
                checkListBox.SetItemChecked(i, true);
                valueStr += checkListBox.GetItemText(checkListBox.Items[i]) + ",";
            }
            tbSelectedValue.Text = valueStr.TrimEnd(',');
        }
    }

    /// <summary>
    /// 重写LABEL
    /// </summary>
    public class LabelS : Label
    {
        public LabelS()
        {
            //控件绘制的时候减少闪烁
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            System.Windows.Forms.ControlPaint.DrawSizeGrip(e.Graphics, Color.Black, 1, 0, this.Size.Width, this.Size.Height);
        }
    }

    /// <summary>
    /// 重写BUTTON
    /// </summary>
    public class ButtonS : Button
    {
        public ButtonS()
        {
            //防止重绘控件出现闪烁
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }
        ButtonState state;
        //当按钮被按下
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            state = ButtonState.Pushed;
            base.OnMouseDown(mevent);
        }

        //当按钮被释放
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            state = ButtonState.Normal;
            base.OnMouseUp(mevent);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            System.Windows.Forms.ControlPaint.DrawComboButton(pevent.Graphics, 0, 0, this.Width, this.Height, state);
        }
    }
}
