namespace DMC
{
    partial class FrmProjectFile
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn文件清单 = new System.Windows.Forms.Button();
            this.btn发起审批 = new System.Windows.Forms.Button();
            this.btn项目归档 = new System.Windows.Forms.Button();
            this.btn下载 = new System.Windows.Forms.Button();
            this.btn全选 = new System.Windows.Forms.Button();
            this.btn删除文件 = new System.Windows.Forms.Button();
            this.btn上传文件 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView_ProjectFileTreeView = new System.Windows.Forms.TreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBox_ProjectSearch = new System.Windows.Forms.ComboBox();
            this.btn_searchProject = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView_objectFile = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_objectFile)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel1.Controls.Add(this.btn文件清单);
            this.panel1.Controls.Add(this.btn发起审批);
            this.panel1.Controls.Add(this.btn项目归档);
            this.panel1.Controls.Add(this.btn下载);
            this.panel1.Controls.Add(this.btn全选);
            this.panel1.Controls.Add(this.btn删除文件);
            this.panel1.Controls.Add(this.btn上传文件);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(923, 45);
            this.panel1.TabIndex = 0;
            // 
            // btn文件清单
            // 
            this.btn文件清单.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn文件清单.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn文件清单.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn文件清单.ForeColor = System.Drawing.Color.White;
            this.btn文件清单.Location = new System.Drawing.Point(570, 0);
            this.btn文件清单.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn文件清单.Name = "btn文件清单";
            this.btn文件清单.Size = new System.Drawing.Size(136, 45);
            this.btn文件清单.TabIndex = 44;
            this.btn文件清单.Text = "选定文件清单";
            this.btn文件清单.UseVisualStyleBackColor = true;
            this.btn文件清单.Visible = false;
            this.btn文件清单.Click += new System.EventHandler(this.btn文件清单_Click);
            // 
            // btn发起审批
            // 
            this.btn发起审批.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn发起审批.Enabled = false;
            this.btn发起审批.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn发起审批.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn发起审批.ForeColor = System.Drawing.Color.White;
            this.btn发起审批.Location = new System.Drawing.Point(475, 0);
            this.btn发起审批.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn发起审批.Name = "btn发起审批";
            this.btn发起审批.Size = new System.Drawing.Size(95, 45);
            this.btn发起审批.TabIndex = 43;
            this.btn发起审批.Text = "发起审批";
            this.btn发起审批.UseVisualStyleBackColor = true;
            this.btn发起审批.Click += new System.EventHandler(this.btn发起审批_Click);
            // 
            // btn项目归档
            // 
            this.btn项目归档.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn项目归档.Enabled = false;
            this.btn项目归档.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn项目归档.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn项目归档.ForeColor = System.Drawing.Color.White;
            this.btn项目归档.Location = new System.Drawing.Point(380, 0);
            this.btn项目归档.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn项目归档.Name = "btn项目归档";
            this.btn项目归档.Size = new System.Drawing.Size(95, 45);
            this.btn项目归档.TabIndex = 42;
            this.btn项目归档.Text = "项目归档";
            this.btn项目归档.UseVisualStyleBackColor = true;
            this.btn项目归档.Click += new System.EventHandler(this.btn项目归档_Click);
            // 
            // btn下载
            // 
            this.btn下载.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn下载.Enabled = false;
            this.btn下载.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn下载.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn下载.ForeColor = System.Drawing.Color.White;
            this.btn下载.Location = new System.Drawing.Point(285, 0);
            this.btn下载.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn下载.Name = "btn下载";
            this.btn下载.Size = new System.Drawing.Size(95, 45);
            this.btn下载.TabIndex = 41;
            this.btn下载.Text = "下载";
            this.btn下载.UseVisualStyleBackColor = true;
            this.btn下载.Click += new System.EventHandler(this.btn下载_Click);
            // 
            // btn全选
            // 
            this.btn全选.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn全选.Enabled = false;
            this.btn全选.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn全选.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn全选.ForeColor = System.Drawing.Color.White;
            this.btn全选.Location = new System.Drawing.Point(190, 0);
            this.btn全选.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn全选.Name = "btn全选";
            this.btn全选.Size = new System.Drawing.Size(95, 45);
            this.btn全选.TabIndex = 40;
            this.btn全选.Text = "全选";
            this.btn全选.UseVisualStyleBackColor = true;
            this.btn全选.Click += new System.EventHandler(this.btn全选_Click);
            // 
            // btn删除文件
            // 
            this.btn删除文件.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn删除文件.Enabled = false;
            this.btn删除文件.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn删除文件.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn删除文件.ForeColor = System.Drawing.Color.White;
            this.btn删除文件.Location = new System.Drawing.Point(95, 0);
            this.btn删除文件.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn删除文件.Name = "btn删除文件";
            this.btn删除文件.Size = new System.Drawing.Size(95, 45);
            this.btn删除文件.TabIndex = 39;
            this.btn删除文件.Text = "删除文件";
            this.btn删除文件.UseVisualStyleBackColor = true;
            this.btn删除文件.Click += new System.EventHandler(this.btn删除文件_Click);
            // 
            // btn上传文件
            // 
            this.btn上传文件.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn上传文件.Enabled = false;
            this.btn上传文件.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn上传文件.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.btn上传文件.ForeColor = System.Drawing.Color.White;
            this.btn上传文件.Location = new System.Drawing.Point(0, 0);
            this.btn上传文件.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn上传文件.Name = "btn上传文件";
            this.btn上传文件.Size = new System.Drawing.Size(95, 45);
            this.btn上传文件.TabIndex = 38;
            this.btn上传文件.Text = "上传文件";
            this.btn上传文件.UseVisualStyleBackColor = true;
            this.btn上传文件.Click += new System.EventHandler(this.btn上传文件_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(735, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 20);
            this.label2.TabIndex = 26;
            this.label2.Text = "总A1数量：0   A1";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(739, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "文件数量：0";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1276, 696);
            this.panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView_ProjectFileTreeView);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.AutoScroll = true;
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView_objectFile);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1276, 696);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView_ProjectFileTreeView
            // 
            this.treeView_ProjectFileTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_ProjectFileTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawAll;
            this.treeView_ProjectFileTreeView.Font = new System.Drawing.Font("微软雅黑", 12.5F);
            this.treeView_ProjectFileTreeView.HideSelection = false;
            this.treeView_ProjectFileTreeView.Indent = 19;
            this.treeView_ProjectFileTreeView.ItemHeight = 35;
            this.treeView_ProjectFileTreeView.Location = new System.Drawing.Point(0, 135);
            this.treeView_ProjectFileTreeView.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.treeView_ProjectFileTreeView.Name = "treeView_ProjectFileTreeView";
            this.treeView_ProjectFileTreeView.Size = new System.Drawing.Size(350, 561);
            this.treeView_ProjectFileTreeView.TabIndex = 0;
            this.treeView_ProjectFileTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_ProjectFile_AfterCheck);
            this.treeView_ProjectFileTreeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_ProjectFile_BeforeCollapse);
            this.treeView_ProjectFileTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.treeView_ProjectFile_DrawNode);
            this.treeView_ProjectFileTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_object_BeforeSelect);
            this.treeView_ProjectFileTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_ProjectFile_AfterSelect);
            this.treeView_ProjectFileTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_ProjectFile_NodeMouseClick);
            this.treeView_ProjectFileTreeView.Leave += new System.EventHandler(this.treeView_object_Leave);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.comboBox_ProjectSearch);
            this.panel3.Controls.Add(this.btn_searchProject);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.MinimumSize = new System.Drawing.Size(300, 35);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(350, 135);
            this.panel3.TabIndex = 0;
            // 
            // comboBox_ProjectSearch
            // 
            this.comboBox_ProjectSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox_ProjectSearch.FormattingEnabled = true;
            this.comboBox_ProjectSearch.Location = new System.Drawing.Point(3, 97);
            this.comboBox_ProjectSearch.Name = "comboBox_ProjectSearch";
            this.comboBox_ProjectSearch.Size = new System.Drawing.Size(292, 33);
            this.comboBox_ProjectSearch.TabIndex = 40;
            this.comboBox_ProjectSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ComboBox_ProjectSearch_KeyDown);
            // 
            // btn_searchProject
            // 
            this.btn_searchProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_searchProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn_searchProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_searchProject.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btn_searchProject.ForeColor = System.Drawing.Color.White;
            this.btn_searchProject.Location = new System.Drawing.Point(297, 97);
            this.btn_searchProject.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn_searchProject.Name = "btn_searchProject";
            this.btn_searchProject.Size = new System.Drawing.Size(48, 33);
            this.btn_searchProject.TabIndex = 39;
            this.btn_searchProject.Text = "查找项目";
            this.btn_searchProject.UseVisualStyleBackColor = false;
            this.btn_searchProject.Click += new System.EventHandler(this.btn_searchProject_Click);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(126, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "项目文件";
            // 
            // dataGridView_objectFile
            // 
            this.dataGridView_objectFile.AllowUserToAddRows = false;
            this.dataGridView_objectFile.AllowUserToDeleteRows = false;
            this.dataGridView_objectFile.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView_objectFile.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_objectFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_objectFile.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_objectFile.ColumnHeadersHeight = 45;
            this.dataGridView_objectFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column2,
            this.Column8,
            this.Column7,
            this.Column9});
            this.dataGridView_objectFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_objectFile.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridView_objectFile.Location = new System.Drawing.Point(0, 45);
            this.dataGridView_objectFile.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            this.dataGridView_objectFile.Name = "dataGridView_objectFile";
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            this.dataGridView_objectFile.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_objectFile.RowTemplate.Height = 40;
            this.dataGridView_objectFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_objectFile.Size = new System.Drawing.Size(923, 651);
            this.dataGridView_objectFile.TabIndex = 12;
            this.dataGridView_objectFile.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellContentClick);
            this.dataGridView_objectFile.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellDoubleClick);
            this.dataGridView_objectFile.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView1_CellMouseUp);
            this.dataGridView_objectFile.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridView_objectFile.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.DataGridView1_RowPostPaint);
            this.dataGridView_objectFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseDown);
            this.dataGridView_objectFile.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseMove);
            this.dataGridView_objectFile.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DataGridView1_MouseUp);
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column1.DataPropertyName = "isCheck";
            this.Column1.FillWeight = 10F;
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "";
            this.Column1.MinimumWidth = 50;
            this.Column1.Name = "Column1";
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Width = 50;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.DataPropertyName = "name";
            this.Column3.FillWeight = 90F;
            this.Column3.HeaderText = "文件名称";
            this.Column3.MinimumWidth = 300;
            this.Column3.Name = "Column3";
            this.Column3.Width = 300;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.DataPropertyName = "frameName";
            this.Column4.FillWeight = 5F;
            this.Column4.HeaderText = "图幅";
            this.Column4.MinimumWidth = 80;
            this.Column4.Name = "Column4";
            this.Column4.Width = 80;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.DataPropertyName = "folded";
            this.Column5.FillWeight = 5F;
            this.Column5.HeaderText = "折A1";
            this.Column5.MinimumWidth = 80;
            this.Column5.Name = "Column5";
            this.Column5.Width = 80;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.DataPropertyName = "realName";
            this.Column6.FillWeight = 5F;
            this.Column6.HeaderText = "上传人";
            this.Column6.MinimumWidth = 80;
            this.Column6.Name = "Column6";
            this.Column6.Width = 80;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.DataPropertyName = "updateTime";
            this.Column2.FillWeight = 5F;
            this.Column2.HeaderText = "更新时间";
            this.Column2.MinimumWidth = 200;
            this.Column2.Name = "Column2";
            this.Column2.Width = 200;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column8.DataPropertyName = "fileTypeName";
            this.Column8.FillWeight = 5F;
            this.Column8.HeaderText = "文件类型";
            this.Column8.MinimumWidth = 100;
            this.Column8.Name = "Column8";
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column7.DataPropertyName = "majorName";
            this.Column7.FillWeight = 5F;
            this.Column7.HeaderText = "专业";
            this.Column7.MinimumWidth = 65;
            this.Column7.Name = "Column7";
            this.Column7.Width = 65;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "processtypeName";
            this.Column9.HeaderText = "流程名称";
            this.Column9.Name = "Column9";
            this.Column9.Width = 113;
            // 
            // FrmProjectFile
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1280, 700);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MinimumSize = new System.Drawing.Size(1280, 700);
            this.Name = "FrmProjectFile";
            this.Text = "项目文件";
            this.Load += new System.EventHandler(this.FrmProjectFile_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_objectFile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;       
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView_ProjectFileTreeView;
        private System.Windows.Forms.DataGridView dataGridView_objectFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn上传文件;
        private System.Windows.Forms.Button btn全选;
        private System.Windows.Forms.Button btn下载;
        private System.Windows.Forms.Button btn项目归档;
        private System.Windows.Forms.Button btn发起审批;
        private System.Windows.Forms.Button btn文件清单;
        private System.Windows.Forms.Button btn删除文件;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_searchProject;
        private System.Windows.Forms.ComboBox comboBox_ProjectSearch;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
    }
}