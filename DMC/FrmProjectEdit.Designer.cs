namespace DMC
{
    partial class FrmProjectEdit
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
            this.panel_项目管理 = new System.Windows.Forms.Panel();
            this.panel_阶段_专业_角色_人员 = new System.Windows.Forms.Panel();
            this.groupBox_阶段 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_阶段 = new System.Windows.Forms.CheckedListBox();
            this.groupBox_专业 = new System.Windows.Forms.GroupBox();
            this.checkedListBox_专业 = new System.Windows.Forms.CheckedListBox();
            this.groupBox_人员角色 = new System.Windows.Forms.GroupBox();
            this.dataGridView_人员角色表 = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label_删除人员 = new System.Windows.Forms.Label();
            this.label_添加人员 = new System.Windows.Forms.Label();
            this.panel_项目基本信息 = new System.Windows.Forms.Panel();
            this.groupBox_项目信息 = new System.Windows.Forms.GroupBox();
            this.dataGridView_项目属性表 = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelMinMaxClose = new System.Windows.Forms.Panel();
            this.LogoText1 = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.panel_项目管理.SuspendLayout();
            this.panel_阶段_专业_角色_人员.SuspendLayout();
            this.groupBox_阶段.SuspendLayout();
            this.groupBox_专业.SuspendLayout();
            this.groupBox_人员角色.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_人员角色表)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel_项目基本信息.SuspendLayout();
            this.groupBox_项目信息.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_项目属性表)).BeginInit();
            this.panelMinMaxClose.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_项目管理
            // 
            this.panel_项目管理.Controls.Add(this.panel_阶段_专业_角色_人员);
            this.panel_项目管理.Controls.Add(this.panel_项目基本信息);
            this.panel_项目管理.Controls.Add(this.panelMinMaxClose);
            this.panel_项目管理.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_项目管理.Location = new System.Drawing.Point(2, 2);
            this.panel_项目管理.Name = "panel_项目管理";
            this.panel_项目管理.Size = new System.Drawing.Size(1276, 891);
            this.panel_项目管理.TabIndex = 2;
            // 
            // panel_阶段_专业_角色_人员
            // 
            this.panel_阶段_专业_角色_人员.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_阶段_专业_角色_人员.Controls.Add(this.groupBox_阶段);
            this.panel_阶段_专业_角色_人员.Controls.Add(this.groupBox_专业);
            this.panel_阶段_专业_角色_人员.Controls.Add(this.groupBox_人员角色);
            this.panel_阶段_专业_角色_人员.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_阶段_专业_角色_人员.Location = new System.Drawing.Point(0, 440);
            this.panel_阶段_专业_角色_人员.Name = "panel_阶段_专业_角色_人员";
            this.panel_阶段_专业_角色_人员.Size = new System.Drawing.Size(1276, 451);
            this.panel_阶段_专业_角色_人员.TabIndex = 4;
            // 
            // groupBox_阶段
            // 
            this.groupBox_阶段.Controls.Add(this.checkedListBox_阶段);
            this.groupBox_阶段.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_阶段.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_阶段.Location = new System.Drawing.Point(0, 0);
            this.groupBox_阶段.Name = "groupBox_阶段";
            this.groupBox_阶段.Size = new System.Drawing.Size(220, 447);
            this.groupBox_阶段.TabIndex = 1;
            this.groupBox_阶段.TabStop = false;
            this.groupBox_阶段.Text = "阶段";
            // 
            // checkedListBox_阶段
            // 
            this.checkedListBox_阶段.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_阶段.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.checkedListBox_阶段.FormattingEnabled = true;
            this.checkedListBox_阶段.Location = new System.Drawing.Point(3, 29);
            this.checkedListBox_阶段.Name = "checkedListBox_阶段";
            this.checkedListBox_阶段.Size = new System.Drawing.Size(214, 415);
            this.checkedListBox_阶段.TabIndex = 1;
            this.checkedListBox_阶段.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_阶段_ItemCheck);
            this.checkedListBox_阶段.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_阶段_SelectedIndexChanged);
            // 
            // groupBox_专业
            // 
            this.groupBox_专业.Controls.Add(this.checkedListBox_专业);
            this.groupBox_专业.Enabled = false;
            this.groupBox_专业.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_专业.Location = new System.Drawing.Point(220, 0);
            this.groupBox_专业.Name = "groupBox_专业";
            this.groupBox_专业.Size = new System.Drawing.Size(170, 446);
            this.groupBox_专业.TabIndex = 2;
            this.groupBox_专业.TabStop = false;
            this.groupBox_专业.Text = "专业";
            // 
            // checkedListBox_专业
            // 
            this.checkedListBox_专业.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_专业.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.checkedListBox_专业.FormattingEnabled = true;
            this.checkedListBox_专业.Location = new System.Drawing.Point(3, 29);
            this.checkedListBox_专业.Name = "checkedListBox_专业";
            this.checkedListBox_专业.Size = new System.Drawing.Size(164, 414);
            this.checkedListBox_专业.TabIndex = 1;
            this.checkedListBox_专业.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_专业_ItemCheck);
            this.checkedListBox_专业.SelectedIndexChanged += new System.EventHandler(this.checkedListBox_专业_SelectedIndexChanged);
            // 
            // groupBox_人员角色
            // 
            this.groupBox_人员角色.Controls.Add(this.dataGridView_人员角色表);
            this.groupBox_人员角色.Controls.Add(this.panel2);
            this.groupBox_人员角色.Enabled = false;
            this.groupBox_人员角色.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox_人员角色.Location = new System.Drawing.Point(393, 0);
            this.groupBox_人员角色.Name = "groupBox_人员角色";
            this.groupBox_人员角色.Size = new System.Drawing.Size(882, 440);
            this.groupBox_人员角色.TabIndex = 3;
            this.groupBox_人员角色.TabStop = false;
            this.groupBox_人员角色.Text = "角色人员";
            // 
            // dataGridView_人员角色表
            // 
            this.dataGridView_人员角色表.AllowUserToAddRows = false;
            this.dataGridView_人员角色表.AllowUserToDeleteRows = false;
            this.dataGridView_人员角色表.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_人员角色表.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView_人员角色表.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_人员角色表.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_人员角色表.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3});
            this.dataGridView_人员角色表.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_人员角色表.Location = new System.Drawing.Point(3, 62);
            this.dataGridView_人员角色表.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView_人员角色表.Name = "dataGridView_人员角色表";
            this.dataGridView_人员角色表.ReadOnly = true;
            this.dataGridView_人员角色表.RowHeadersVisible = false;
            this.dataGridView_人员角色表.RowTemplate.Height = 40;
            this.dataGridView_人员角色表.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_人员角色表.Size = new System.Drawing.Size(876, 375);
            this.dataGridView_人员角色表.TabIndex = 63;
            this.dataGridView_人员角色表.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_角色人员_CellClick);
            this.dataGridView_人员角色表.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_角色人员_CellContentClick);
            this.dataGridView_人员角色表.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_角色人员_CellFormatting);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "UserName";
            this.dataGridViewTextBoxColumn3.FillWeight = 105.5964F;
            this.dataGridViewTextBoxColumn3.HeaderText = "姓名";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 100;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label_删除人员);
            this.panel2.Controls.Add(this.label_添加人员);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 29);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(876, 33);
            this.panel2.TabIndex = 20;
            // 
            // label_删除人员
            // 
            this.label_删除人员.AutoSize = true;
            this.label_删除人员.Enabled = false;
            this.label_删除人员.Location = new System.Drawing.Point(106, 6);
            this.label_删除人员.Name = "label_删除人员";
            this.label_删除人员.Size = new System.Drawing.Size(88, 25);
            this.label_删除人员.TabIndex = 1;
            this.label_删除人员.Text = "删除人员";
            this.label_删除人员.Click += new System.EventHandler(this.label_删除人员_Click);
            // 
            // label_添加人员
            // 
            this.label_添加人员.AutoSize = true;
            this.label_添加人员.Enabled = false;
            this.label_添加人员.Location = new System.Drawing.Point(13, 6);
            this.label_添加人员.Name = "label_添加人员";
            this.label_添加人员.Size = new System.Drawing.Size(88, 25);
            this.label_添加人员.TabIndex = 0;
            this.label_添加人员.Text = "添加人员";
            this.label_添加人员.Click += new System.EventHandler(this.label_添加人员_Click);
            // 
            // panel_项目基本信息
            // 
            this.panel_项目基本信息.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel_项目基本信息.Controls.Add(this.groupBox_项目信息);
            this.panel_项目基本信息.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_项目基本信息.Location = new System.Drawing.Point(0, 40);
            this.panel_项目基本信息.Name = "panel_项目基本信息";
            this.panel_项目基本信息.Size = new System.Drawing.Size(1276, 400);
            this.panel_项目基本信息.TabIndex = 7;
            // 
            // groupBox_项目信息
            // 
            this.groupBox_项目信息.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox_项目信息.Controls.Add(this.dataGridView_项目属性表);
            this.groupBox_项目信息.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_项目信息.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBox_项目信息.Font = new System.Drawing.Font("微软雅黑", 17F);
            this.groupBox_项目信息.Location = new System.Drawing.Point(0, 0);
            this.groupBox_项目信息.Name = "groupBox_项目信息";
            this.groupBox_项目信息.Size = new System.Drawing.Size(1272, 396);
            this.groupBox_项目信息.TabIndex = 0;
            this.groupBox_项目信息.TabStop = false;
            this.groupBox_项目信息.Text = "基本信息";
            // 
            // dataGridView_项目属性表
            // 
            this.dataGridView_项目属性表.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_项目属性表.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView_项目属性表.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_项目属性表.ColumnHeadersHeight = 45;
            this.dataGridView_项目属性表.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4});
            this.dataGridView_项目属性表.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_项目属性表.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView_项目属性表.Location = new System.Drawing.Point(3, 33);
            this.dataGridView_项目属性表.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView_项目属性表.Name = "dataGridView_项目属性表";
            this.dataGridView_项目属性表.RowHeadersWidth = 45;
            this.dataGridView_项目属性表.RowTemplate.Height = 40;
            this.dataGridView_项目属性表.Size = new System.Drawing.Size(1266, 360);
            this.dataGridView_项目属性表.TabIndex = 20;
            this.dataGridView_项目属性表.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_项目属性表_CellClick);
            this.dataGridView_项目属性表.CurrentCellChanged += new System.EventHandler(this.dataGridView_项目属性表_CurrentCellChanged);
            this.dataGridView_项目属性表.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_项目属性表_RowPostPaint);
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.DataPropertyName = "Name";
            this.Column3.HeaderText = "项目属性";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 130;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "Value";
            this.Column4.HeaderText = "";
            this.Column4.Name = "Column4";
            // 
            // panelMinMaxClose
            // 
            this.panelMinMaxClose.AutoSize = true;
            this.panelMinMaxClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panelMinMaxClose.Controls.Add(this.LogoText1);
            this.panelMinMaxClose.Controls.Add(this.buttonClose);
            this.panelMinMaxClose.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelMinMaxClose.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMinMaxClose.Location = new System.Drawing.Point(0, 0);
            this.panelMinMaxClose.MinimumSize = new System.Drawing.Size(1080, 40);
            this.panelMinMaxClose.Name = "panelMinMaxClose";
            this.panelMinMaxClose.Size = new System.Drawing.Size(1276, 40);
            this.panelMinMaxClose.TabIndex = 6;
            this.panelMinMaxClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseDown);
            this.panelMinMaxClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseMove);
            // 
            // LogoText1
            // 
            this.LogoText1.AutoSize = true;
            this.LogoText1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.LogoText1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.LogoText1.ForeColor = System.Drawing.Color.White;
            this.LogoText1.Location = new System.Drawing.Point(10, 6);
            this.LogoText1.Margin = new System.Windows.Forms.Padding(5);
            this.LogoText1.Name = "LogoText1";
            this.LogoText1.Size = new System.Drawing.Size(92, 27);
            this.LogoText1.TabIndex = 3;
            this.LogoText1.Text = "项目管理";
            this.LogoText1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClose.Font = new System.Drawing.Font("微软雅黑", 17F, System.Drawing.FontStyle.Bold);
            this.buttonClose.ForeColor = System.Drawing.Color.Transparent;
            this.buttonClose.Location = new System.Drawing.Point(1236, 0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(40, 40);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel9.Controls.Add(this.button7);
            this.panel9.Controls.Add(this.button6);
            this.panel9.Controls.Add(this.button5);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(2, 893);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(1276, 65);
            this.panel9.TabIndex = 1;
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button7.ForeColor = System.Drawing.Color.White;
            this.button7.Location = new System.Drawing.Point(719, 11);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(150, 50);
            this.button7.TabIndex = 2;
            this.button7.Text = "发布";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button_发布_Click);
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(407, 11);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(150, 50);
            this.button6.TabIndex = 1;
            this.button6.Text = "取消";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button_取消_Click);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button5.ForeColor = System.Drawing.Color.White;
            this.button5.Location = new System.Drawing.Point(563, 11);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(150, 50);
            this.button5.TabIndex = 0;
            this.button5.Text = "保存";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button_保存_Click);
            // 
            // FrmProjectEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1280, 960);
            this.Controls.Add(this.panel_项目管理);
            this.Controls.Add(this.panel9);
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.MinimumSize = new System.Drawing.Size(1280, 960);
            this.Name = "FrmProjectEdit";
            this.Text = "修改项目";
            this.Load += new System.EventHandler(this.FrmProjectAdd_Load);
            this.panel_项目管理.ResumeLayout(false);
            this.panel_项目管理.PerformLayout();
            this.panel_阶段_专业_角色_人员.ResumeLayout(false);
            this.groupBox_阶段.ResumeLayout(false);
            this.groupBox_专业.ResumeLayout(false);
            this.groupBox_人员角色.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_人员角色表)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel_项目基本信息.ResumeLayout(false);
            this.groupBox_项目信息.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_项目属性表)).EndInit();
            this.panelMinMaxClose.ResumeLayout(false);
            this.panelMinMaxClose.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Panel panel_项目管理;
        private System.Windows.Forms.GroupBox groupBox_人员角色;
        private System.Windows.Forms.GroupBox groupBox_阶段;
        private System.Windows.Forms.GroupBox groupBox_专业;
        private System.Windows.Forms.CheckedListBox checkedListBox_专业;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label_删除人员;
        private System.Windows.Forms.Label label_添加人员;
        private System.Windows.Forms.DataGridView dataGridView_人员角色表;
        private System.Windows.Forms.CheckedListBox checkedListBox_阶段;
        private System.Windows.Forms.GroupBox groupBox_项目信息;
        private System.Windows.Forms.DataGridView dataGridView_项目属性表;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.Panel panelMinMaxClose;
        private System.Windows.Forms.Label LogoText1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panel_项目基本信息;
        private System.Windows.Forms.Panel panel_阶段_专业_角色_人员;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    }
}