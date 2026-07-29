namespace DMC
{
    partial class FrmProTran
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView_流程详情表 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button_Refresh = new System.Windows.Forms.Button();
            this.btn已审批 = new System.Windows.Forms.Button();
            this.btn待审批 = new System.Windows.Forms.Button();
            this.btn我发起的 = new System.Windows.Forms.Button();
            this.btn所有流程 = new System.Windows.Forms.Button();
            this.dateTimePicker_End = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.dateTimePicker_Start = new System.Windows.Forms.DateTimePicker();
            this.button_搜索 = new System.Windows.Forms.Button();
            this.textBox_搜索关键字 = new System.Windows.Forms.TextBox();
            this.comboBox_下拉选择 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_流程详情表)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView_流程详情表
            // 
            this.dataGridView_流程详情表.AllowUserToAddRows = false;
            this.dataGridView_流程详情表.AllowUserToDeleteRows = false;
            this.dataGridView_流程详情表.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_流程详情表.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 13.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_流程详情表.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_流程详情表.ColumnHeadersHeight = 45;
            this.dataGridView_流程详情表.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView_流程详情表.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column9,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column7,
            this.Column6,
            this.Column8});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 12F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_流程详情表.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_流程详情表.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_流程详情表.Location = new System.Drawing.Point(0, 55);
            this.dataGridView_流程详情表.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView_流程详情表.Name = "dataGridView_流程详情表";
            this.dataGridView_流程详情表.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 13.5F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_流程详情表.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView_流程详情表.RowHeadersWidth = 55;
            this.dataGridView_流程详情表.RowTemplate.Height = 40;
            this.dataGridView_流程详情表.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_流程详情表.Size = new System.Drawing.Size(1080, 825);
            this.dataGridView_流程详情表.TabIndex = 13;
            this.dataGridView_流程详情表.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView_流程详情表.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            this.dataGridView_流程详情表.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "applyXh";
            this.Column1.FillWeight = 10F;
            this.Column1.HeaderText = "流程序号";
            this.Column1.MinimumWidth = 115;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 115;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "NAME";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.Highlight;
            this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column2.FillWeight = 20F;
            this.Column2.HeaderText = "流程标题";
            this.Column2.MinimumWidth = 150;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 150;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "processtypeId";
            this.Column9.FillWeight = 20F;
            this.Column9.HeaderText = "流程类型";
            this.Column9.MinimumWidth = 150;
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 150;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.DataPropertyName = "proName";
            this.Column3.FillWeight = 80F;
            this.Column3.HeaderText = "项目名称";
            this.Column3.MinimumWidth = 300;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "userName";
            this.Column4.FillWeight = 20F;
            this.Column4.HeaderText = "提交用户";
            this.Column4.MinimumWidth = 100;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "createTime";
            this.Column5.FillWeight = 20F;
            this.Column5.HeaderText = "提交时间";
            this.Column5.MinimumWidth = 200;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 200;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "result";
            this.Column7.FillWeight = 20F;
            this.Column7.HeaderText = "审批状态";
            this.Column7.MinimumWidth = 100;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "lastTime";
            this.Column6.FillWeight = 20F;
            this.Column6.HeaderText = "最后审批时间";
            this.Column6.MinimumWidth = 200;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 200;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "remark";
            this.Column8.FillWeight = 20F;
            this.Column8.HeaderText = "备注";
            this.Column8.MinimumWidth = 180;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 180;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel3.Controls.Add(this.button_Refresh);
            this.panel3.Controls.Add(this.btn已审批);
            this.panel3.Controls.Add(this.btn待审批);
            this.panel3.Controls.Add(this.btn我发起的);
            this.panel3.Controls.Add(this.btn所有流程);
            this.panel3.Controls.Add(this.dateTimePicker_End);
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.dateTimePicker_Start);
            this.panel3.Controls.Add(this.button_搜索);
            this.panel3.Controls.Add(this.textBox_搜索关键字);
            this.panel3.Controls.Add(this.comboBox_下拉选择);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1080, 55);
            this.panel3.TabIndex = 12;
            // 
            // button_Refresh
            // 
            this.button_Refresh.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_Refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_Refresh.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.button_Refresh.ForeColor = System.Drawing.Color.White;
            this.button_Refresh.Location = new System.Drawing.Point(380, 0);
            this.button_Refresh.Name = "button_Refresh";
            this.button_Refresh.Size = new System.Drawing.Size(90, 55);
            this.button_Refresh.TabIndex = 42;
            this.button_Refresh.Text = "刷新";
            this.button_Refresh.UseVisualStyleBackColor = true;
            this.button_Refresh.Click += new System.EventHandler(this.button_Refresh_Click);
            // 
            // btn已审批
            // 
            this.btn已审批.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn已审批.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn已审批.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn已审批.ForeColor = System.Drawing.Color.White;
            this.btn已审批.Location = new System.Drawing.Point(290, 0);
            this.btn已审批.Name = "btn已审批";
            this.btn已审批.Size = new System.Drawing.Size(90, 55);
            this.btn已审批.TabIndex = 41;
            this.btn已审批.Text = "已审批";
            this.btn已审批.UseVisualStyleBackColor = true;
            this.btn已审批.Click += new System.EventHandler(this.button_Click);
            // 
            // btn待审批
            // 
            this.btn待审批.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn待审批.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn待审批.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn待审批.ForeColor = System.Drawing.Color.White;
            this.btn待审批.Location = new System.Drawing.Point(200, 0);
            this.btn待审批.Name = "btn待审批";
            this.btn待审批.Size = new System.Drawing.Size(90, 55);
            this.btn待审批.TabIndex = 40;
            this.btn待审批.Text = "待审批";
            this.btn待审批.UseVisualStyleBackColor = true;
            this.btn待审批.Click += new System.EventHandler(this.button_Click);
            // 
            // btn我发起的
            // 
            this.btn我发起的.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn我发起的.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn我发起的.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn我发起的.ForeColor = System.Drawing.Color.White;
            this.btn我发起的.Location = new System.Drawing.Point(100, 0);
            this.btn我发起的.Name = "btn我发起的";
            this.btn我发起的.Size = new System.Drawing.Size(100, 55);
            this.btn我发起的.TabIndex = 39;
            this.btn我发起的.Text = "我发起的";
            this.btn我发起的.UseVisualStyleBackColor = true;
            this.btn我发起的.Click += new System.EventHandler(this.button_Click);
            // 
            // btn所有流程
            // 
            this.btn所有流程.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn所有流程.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn所有流程.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn所有流程.ForeColor = System.Drawing.Color.White;
            this.btn所有流程.Location = new System.Drawing.Point(0, 0);
            this.btn所有流程.Name = "btn所有流程";
            this.btn所有流程.Size = new System.Drawing.Size(100, 55);
            this.btn所有流程.TabIndex = 38;
            this.btn所有流程.Text = "所有流程";
            this.btn所有流程.UseVisualStyleBackColor = true;
            this.btn所有流程.Click += new System.EventHandler(this.button_Click);
            // 
            // dateTimePicker_End
            // 
            this.dateTimePicker_End.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dateTimePicker_End.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker_End.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker_End.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_End.Location = new System.Drawing.Point(966, 21);
            this.dateTimePicker_End.Name = "dateTimePicker_End";
            this.dateTimePicker_End.Size = new System.Drawing.Size(106, 29);
            this.dateTimePicker_End.TabIndex = 20;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(940, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 21);
            this.label8.TabIndex = 21;
            this.label8.Text = "至";
            // 
            // dateTimePicker_Start
            // 
            this.dateTimePicker_Start.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.dateTimePicker_Start.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker_Start.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.dateTimePicker_Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker_Start.Location = new System.Drawing.Point(835, 20);
            this.dateTimePicker_Start.Name = "dateTimePicker_Start";
            this.dateTimePicker_Start.Size = new System.Drawing.Size(106, 29);
            this.dateTimePicker_Start.TabIndex = 19;
            this.dateTimePicker_Start.Value = new System.DateTime(2024, 9, 25, 0, 0, 0, 0);
            // 
            // button_搜索
            // 
            this.button_搜索.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_搜索.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_搜索.Location = new System.Drawing.Point(475, 20);
            this.button_搜索.Margin = new System.Windows.Forms.Padding(4);
            this.button_搜索.Name = "button_搜索";
            this.button_搜索.Size = new System.Drawing.Size(60, 29);
            this.button_搜索.TabIndex = 9;
            this.button_搜索.Text = "搜索";
            this.button_搜索.UseVisualStyleBackColor = true;
            this.button_搜索.Click += new System.EventHandler(this.button8_搜索);
            // 
            // textBox_搜索关键字
            // 
            this.textBox_搜索关键字.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox_搜索关键字.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_搜索关键字.Location = new System.Drawing.Point(539, 20);
            this.textBox_搜索关键字.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_搜索关键字.Name = "textBox_搜索关键字";
            this.textBox_搜索关键字.Size = new System.Drawing.Size(145, 29);
            this.textBox_搜索关键字.TabIndex = 8;
            // 
            // comboBox_下拉选择
            // 
            this.comboBox_下拉选择.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.comboBox_下拉选择.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_下拉选择.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_下拉选择.FormattingEnabled = true;
            this.comboBox_下拉选择.Items.AddRange(new object[] {
            "项目名称",
            "时间",
            "用户"});
            this.comboBox_下拉选择.Location = new System.Drawing.Point(689, 20);
            this.comboBox_下拉选择.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox_下拉选择.Name = "comboBox_下拉选择";
            this.comboBox_下拉选择.Size = new System.Drawing.Size(139, 29);
            this.comboBox_下拉选择.TabIndex = 7;
            this.comboBox_下拉选择.SelectedIndexChanged += new System.EventHandler(this.comboBox_select_SelectedIndexChanged);
            // 
            // FrmProTran
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1080, 880);
            this.Controls.Add(this.dataGridView_流程详情表);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.MinimumSize = new System.Drawing.Size(1080, 880);
            this.Name = "FrmProTran";
            this.Padding = new System.Windows.Forms.Padding(0);
            this.Text = "事务流程";
            this.Load += new System.EventHandler(this.FrmProTran_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_流程详情表)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button_搜索;
        private System.Windows.Forms.TextBox textBox_搜索关键字;
        private System.Windows.Forms.ComboBox comboBox_下拉选择;
        private System.Windows.Forms.DataGridView dataGridView_流程详情表;
        private System.Windows.Forms.DateTimePicker dateTimePicker_End;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dateTimePicker_Start;
        private System.Windows.Forms.Button btn已审批;
        private System.Windows.Forms.Button btn待审批;
        private System.Windows.Forms.Button btn我发起的;
        private System.Windows.Forms.Button btn所有流程;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.Button button_Refresh;
    }
}