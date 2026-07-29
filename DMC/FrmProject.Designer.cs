namespace DMC
{
    partial class FrmProject
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.treeView_组织架构 = new System.Windows.Forms.TreeView();
            this.panel_项目管理_名头 = new System.Windows.Forms.Panel();
            this.label_项目位置 = new System.Windows.Forms.Label();
            this.label_项目管理 = new System.Windows.Forms.Label();
            this.tabControl_项目 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView_项目列表 = new System.Windows.Forms.DataGridView();
            this.ProjectNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.createTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView_我创建的项目 = new System.Windows.Forms.DataGridView();
            this.MyProjectNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.dataGridView_未发布项目 = new System.Windows.Forms.DataGridView();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button_停用 = new System.Windows.Forms.Button();
            this.button_导入 = new System.Windows.Forms.Button();
            this.button_下载模板 = new System.Windows.Forms.Button();
            this.button_删除 = new System.Windows.Forms.Button();
            this.button_修改 = new System.Windows.Forms.Button();
            this.button_新建 = new System.Windows.Forms.Button();
            this.button_搜索 = new System.Windows.Forms.Button();
            this.textBox_搜索 = new System.Windows.Forms.TextBox();
            this.No_ProjectNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel_项目管理_名头.SuspendLayout();
            this.tabControl_项目.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_项目列表)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_我创建的项目)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_未发布项目)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1276, 896);
            this.panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel_项目管理_名头);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl_项目);
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Size = new System.Drawing.Size(1276, 896);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.treeView_组织架构);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 115);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(350, 781);
            this.panel3.TabIndex = 1;
            // 
            // treeView_组织架构
            // 
            this.treeView_组织架构.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_组织架构.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.treeView_组织架构.ItemHeight = 38;
            this.treeView_组织架构.Location = new System.Drawing.Point(0, 0);
            this.treeView_组织架构.Name = "treeView_组织架构";
            this.treeView_组织架构.Size = new System.Drawing.Size(350, 781);
            this.treeView_组织架构.TabIndex = 1;
            this.treeView_组织架构.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_组织机构_BeforeCollapse);
            this.treeView_组织架构.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_组织机构_BeforeSelect);
            this.treeView_组织架构.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_组织机构_AfterSelect);
            this.treeView_组织架构.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_组织机构_NodeMouseClick);
            this.treeView_组织架构.Leave += new System.EventHandler(this.treeView_组织机构_Leave);
            // 
            // panel_项目管理_名头
            // 
            this.panel_项目管理_名头.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel_项目管理_名头.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_项目管理_名头.Controls.Add(this.label_项目位置);
            this.panel_项目管理_名头.Controls.Add(this.label_项目管理);
            this.panel_项目管理_名头.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_项目管理_名头.Location = new System.Drawing.Point(0, 0);
            this.panel_项目管理_名头.Name = "panel_项目管理_名头";
            this.panel_项目管理_名头.Size = new System.Drawing.Size(350, 115);
            this.panel_项目管理_名头.TabIndex = 0;
            // 
            // label_项目位置
            // 
            this.label_项目位置.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label_项目位置.AutoSize = true;
            this.label_项目位置.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_项目位置.Location = new System.Drawing.Point(12, 83);
            this.label_项目位置.Name = "label_项目位置";
            this.label_项目位置.Size = new System.Drawing.Size(132, 27);
            this.label_项目位置.TabIndex = 1;
            this.label_项目位置.Text = "选择项目位置";
            // 
            // label_项目管理
            // 
            this.label_项目管理.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_项目管理.AutoSize = true;
            this.label_项目管理.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_项目管理.Location = new System.Drawing.Point(126, 36);
            this.label_项目管理.Name = "label_项目管理";
            this.label_项目管理.Size = new System.Drawing.Size(96, 28);
            this.label_项目管理.TabIndex = 0;
            this.label_项目管理.Text = "项目管理";
            // 
            // tabControl_项目
            // 
            this.tabControl_项目.Controls.Add(this.tabPage1);
            this.tabControl_项目.Controls.Add(this.tabPage2);
            this.tabControl_项目.Controls.Add(this.tabPage3);
            this.tabControl_项目.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_项目.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.tabControl_项目.ItemSize = new System.Drawing.Size(88, 40);
            this.tabControl_项目.Location = new System.Drawing.Point(0, 45);
            this.tabControl_项目.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl_项目.Name = "tabControl_项目";
            this.tabControl_项目.SelectedIndex = 0;
            this.tabControl_项目.Size = new System.Drawing.Size(925, 851);
            this.tabControl_项目.TabIndex = 16;
            this.tabControl_项目.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView_项目列表);
            this.tabPage1.Location = new System.Drawing.Point(4, 44);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(917, 803);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "项目列表";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView_项目列表
            // 
            this.dataGridView_项目列表.AllowUserToAddRows = false;
            this.dataGridView_项目列表.AllowUserToDeleteRows = false;
            this.dataGridView_项目列表.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_项目列表.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_项目列表.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView_项目列表.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_项目列表.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProjectNo,
            this.name,
            this.createTime,
            this.Column7});
            this.dataGridView_项目列表.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_项目列表.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_项目列表.Margin = new System.Windows.Forms.Padding(0);
            this.dataGridView_项目列表.Name = "dataGridView_项目列表";
            this.dataGridView_项目列表.ReadOnly = true;
            this.dataGridView_项目列表.RowHeadersWidth = 50;
            this.dataGridView_项目列表.RowTemplate.Height = 38;
            this.dataGridView_项目列表.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_项目列表.Size = new System.Drawing.Size(911, 797);
            this.dataGridView_项目列表.TabIndex = 16;
            this.dataGridView_项目列表.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            this.dataGridView_项目列表.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            this.dataGridView_项目列表.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            // 
            // ProjectNo
            // 
            this.ProjectNo.DataPropertyName = "ProjectNo";
            this.ProjectNo.FillWeight = 20F;
            this.ProjectNo.HeaderText = "项目编号";
            this.ProjectNo.MinimumWidth = 50;
            this.ProjectNo.Name = "ProjectNo";
            this.ProjectNo.ReadOnly = true;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.DataPropertyName = "name";
            this.name.FillWeight = 80F;
            this.name.HeaderText = "项目名称";
            this.name.MinimumWidth = 350;
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // createTime
            // 
            this.createTime.DataPropertyName = "createTime";
            this.createTime.FillWeight = 20F;
            this.createTime.HeaderText = "创建日期";
            this.createTime.MinimumWidth = 180;
            this.createTime.Name = "createTime";
            this.createTime.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "status";
            this.Column7.FillWeight = 20F;
            this.Column7.HeaderText = "状态";
            this.Column7.MinimumWidth = 150;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.dataGridView_我创建的项目);
            this.tabPage2.Location = new System.Drawing.Point(4, 44);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(917, 803);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "我创建的项目";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView_我创建的项目
            // 
            this.dataGridView_我创建的项目.AllowUserToAddRows = false;
            this.dataGridView_我创建的项目.AllowUserToDeleteRows = false;
            this.dataGridView_我创建的项目.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_我创建的项目.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_我创建的项目.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_我创建的项目.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_我创建的项目.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MyProjectNo,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.Column5});
            this.dataGridView_我创建的项目.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_我创建的项目.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_我创建的项目.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView_我创建的项目.Name = "dataGridView_我创建的项目";
            this.dataGridView_我创建的项目.ReadOnly = true;
            this.dataGridView_我创建的项目.RowTemplate.Height = 35;
            this.dataGridView_我创建的项目.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_我创建的项目.Size = new System.Drawing.Size(911, 797);
            this.dataGridView_我创建的项目.TabIndex = 17;
            this.dataGridView_我创建的项目.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            this.dataGridView_我创建的项目.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            this.dataGridView_我创建的项目.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            // 
            // MyProjectNo
            // 
            this.MyProjectNo.DataPropertyName = "ProjectNo";
            this.MyProjectNo.FillWeight = 20F;
            this.MyProjectNo.HeaderText = "项目编号";
            this.MyProjectNo.MinimumWidth = 50;
            this.MyProjectNo.Name = "MyProjectNo";
            this.MyProjectNo.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn1.FillWeight = 80F;
            this.dataGridViewTextBoxColumn1.HeaderText = "项目名称";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 350;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "createTime";
            this.dataGridViewTextBoxColumn2.FillWeight = 20F;
            this.dataGridViewTextBoxColumn2.HeaderText = "创建日期";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 180;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "status";
            this.Column5.FillWeight = 20F;
            this.Column5.HeaderText = "状态";
            this.Column5.MinimumWidth = 150;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.dataGridView_未发布项目);
            this.tabPage3.Location = new System.Drawing.Point(4, 44);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(917, 803);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "未发布的项目";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // dataGridView_未发布项目
            // 
            this.dataGridView_未发布项目.AllowUserToAddRows = false;
            this.dataGridView_未发布项目.AllowUserToDeleteRows = false;
            this.dataGridView_未发布项目.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_未发布项目.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_未发布项目.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView_未发布项目.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_未发布项目.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No_ProjectNo,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.Column6});
            this.dataGridView_未发布项目.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_未发布项目.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_未发布项目.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView_未发布项目.Name = "dataGridView_未发布项目";
            this.dataGridView_未发布项目.ReadOnly = true;
            this.dataGridView_未发布项目.RowTemplate.Height = 23;
            this.dataGridView_未发布项目.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_未发布项目.Size = new System.Drawing.Size(911, 797);
            this.dataGridView_未发布项目.TabIndex = 17;
            this.dataGridView_未发布项目.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            this.dataGridView_未发布项目.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView_CellFormatting);
            this.dataGridView_未发布项目.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel4.Controls.Add(this.button_停用);
            this.panel4.Controls.Add(this.button_导入);
            this.panel4.Controls.Add(this.button_下载模板);
            this.panel4.Controls.Add(this.button_删除);
            this.panel4.Controls.Add(this.button_修改);
            this.panel4.Controls.Add(this.button_新建);
            this.panel4.Controls.Add(this.button_搜索);
            this.panel4.Controls.Add(this.textBox_搜索);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(925, 45);
            this.panel4.TabIndex = 1;
            // 
            // button_停用
            // 
            this.button_停用.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_停用.Enabled = false;
            this.button_停用.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_停用.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button_停用.ForeColor = System.Drawing.Color.White;
            this.button_停用.Location = new System.Drawing.Point(500, 0);
            this.button_停用.Name = "button_停用";
            this.button_停用.Size = new System.Drawing.Size(100, 45);
            this.button_停用.TabIndex = 45;
            this.button_停用.Tag = "promanage:disable";
            this.button_停用.Text = "停用";
            this.button_停用.UseVisualStyleBackColor = true;
            this.button_停用.Click += new System.EventHandler(this.button_停用_Click);
            // 
            // button_导入
            // 
            this.button_导入.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_导入.Enabled = false;
            this.button_导入.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_导入.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button_导入.ForeColor = System.Drawing.Color.White;
            this.button_导入.Location = new System.Drawing.Point(400, 0);
            this.button_导入.Name = "button_导入";
            this.button_导入.Size = new System.Drawing.Size(100, 45);
            this.button_导入.TabIndex = 43;
            this.button_导入.Tag = "promanage:import";
            this.button_导入.Text = "导入";
            this.button_导入.UseVisualStyleBackColor = true;
            this.button_导入.Click += new System.EventHandler(this.button_导入_Click);
            // 
            // button_下载模板
            // 
            this.button_下载模板.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_下载模板.Enabled = false;
            this.button_下载模板.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_下载模板.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button_下载模板.ForeColor = System.Drawing.Color.White;
            this.button_下载模板.Location = new System.Drawing.Point(300, 0);
            this.button_下载模板.Name = "button_下载模板";
            this.button_下载模板.Size = new System.Drawing.Size(100, 45);
            this.button_下载模板.TabIndex = 42;
            this.button_下载模板.Tag = "promanage:down";
            this.button_下载模板.Text = "下载模板";
            this.button_下载模板.UseVisualStyleBackColor = true;
            this.button_下载模板.Click += new System.EventHandler(this.button_下载模板_Click);
            // 
            // button_删除
            // 
            this.button_删除.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_删除.Enabled = false;
            this.button_删除.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_删除.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button_删除.ForeColor = System.Drawing.Color.White;
            this.button_删除.Location = new System.Drawing.Point(200, 0);
            this.button_删除.Name = "button_删除";
            this.button_删除.Size = new System.Drawing.Size(100, 45);
            this.button_删除.TabIndex = 41;
            this.button_删除.Tag = "promanage:del";
            this.button_删除.Text = "删除";
            this.button_删除.UseVisualStyleBackColor = true;
            this.button_删除.Click += new System.EventHandler(this.button_删除_Click);
            // 
            // button_修改
            // 
            this.button_修改.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_修改.Enabled = false;
            this.button_修改.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_修改.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button_修改.ForeColor = System.Drawing.Color.White;
            this.button_修改.Location = new System.Drawing.Point(100, 0);
            this.button_修改.Name = "button_修改";
            this.button_修改.Size = new System.Drawing.Size(100, 45);
            this.button_修改.TabIndex = 40;
            this.button_修改.Tag = "promanage:edit";
            this.button_修改.Text = "修改";
            this.button_修改.UseVisualStyleBackColor = true;
            this.button_修改.Click += new System.EventHandler(this.button_修改_Click);
            // 
            // button_新建
            // 
            this.button_新建.Dock = System.Windows.Forms.DockStyle.Left;
            this.button_新建.Enabled = false;
            this.button_新建.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_新建.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button_新建.ForeColor = System.Drawing.Color.White;
            this.button_新建.Location = new System.Drawing.Point(0, 0);
            this.button_新建.Name = "button_新建";
            this.button_新建.Size = new System.Drawing.Size(100, 45);
            this.button_新建.TabIndex = 39;
            this.button_新建.Tag = "promanage:add";
            this.button_新建.Text = "新建";
            this.button_新建.UseVisualStyleBackColor = true;
            this.button_新建.Click += new System.EventHandler(this.button_新建_Click);
            // 
            // button_搜索
            // 
            this.button_搜索.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_搜索.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.button_搜索.Location = new System.Drawing.Point(615, 8);
            this.button_搜索.Margin = new System.Windows.Forms.Padding(4);
            this.button_搜索.Name = "button_搜索";
            this.button_搜索.Size = new System.Drawing.Size(64, 30);
            this.button_搜索.TabIndex = 25;
            this.button_搜索.Tag = "promanage:list";
            this.button_搜索.Text = "搜索";
            this.button_搜索.UseVisualStyleBackColor = true;
            this.button_搜索.Click += new System.EventHandler(this.button_搜索_Click);
            // 
            // textBox_搜索
            // 
            this.textBox_搜索.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox_搜索.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_搜索.Location = new System.Drawing.Point(696, 8);
            this.textBox_搜索.Margin = new System.Windows.Forms.Padding(4);
            this.textBox_搜索.Name = "textBox_搜索";
            this.textBox_搜索.Size = new System.Drawing.Size(201, 29);
            this.textBox_搜索.TabIndex = 24;
            // 
            // No_ProjectNo
            // 
            this.No_ProjectNo.DataPropertyName = "ProjectNo";
            this.No_ProjectNo.FillWeight = 20F;
            this.No_ProjectNo.HeaderText = "项目编号";
            this.No_ProjectNo.MinimumWidth = 50;
            this.No_ProjectNo.Name = "No_ProjectNo";
            this.No_ProjectNo.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn4.FillWeight = 80F;
            this.dataGridViewTextBoxColumn4.HeaderText = "项目名称";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 350;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "createTime";
            this.dataGridViewTextBoxColumn5.FillWeight = 20F;
            this.dataGridViewTextBoxColumn5.HeaderText = "创建日期";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 180;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "status";
            this.Column6.FillWeight = 20F;
            this.Column6.HeaderText = "状态";
            this.Column6.MinimumWidth = 150;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // FrmProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.Controls.Add(this.panel2);
            this.MinimumSize = new System.Drawing.Size(1280, 900);
            this.Name = "FrmProject";
            this.Text = "项目管理";
            this.Load += new System.EventHandler(this.FrmProject_Load);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel_项目管理_名头.ResumeLayout(false);
            this.panel_项目管理_名头.PerformLayout();
            this.tabControl_项目.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_项目列表)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_我创建的项目)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_未发布项目)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button_搜索;
        private System.Windows.Forms.TextBox textBox_搜索;
        private System.Windows.Forms.TreeView treeView_组织架构;
        private System.Windows.Forms.TabControl tabControl_项目;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView_项目列表;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView dataGridView_我创建的项目;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView dataGridView_未发布项目;
        private System.Windows.Forms.Button button_导入;
        private System.Windows.Forms.Button button_下载模板;
        private System.Windows.Forms.Button button_删除;
        private System.Windows.Forms.Button button_修改;
        private System.Windows.Forms.Button button_新建;
        private System.Windows.Forms.Button button_停用;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel_项目管理_名头;
        private System.Windows.Forms.Label label_项目位置;
        private System.Windows.Forms.Label label_项目管理;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProjectNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn createTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn MyProjectNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn No_ProjectNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}