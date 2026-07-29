namespace DMC
{
    partial class FrmArchiveManage
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
            this.panel_NodeTree = new System.Windows.Forms.Panel();
            this.treeView_Archive = new System.Windows.Forms.TreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView_archiveFile = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn文件清单 = new System.Windows.Forms.Button();
            this.btn搜索 = new System.Windows.Forms.Button();
            this.btn发起审批 = new System.Windows.Forms.Button();
            this.btn全选 = new System.Windows.Forms.Button();
            this.btn删除文件 = new System.Windows.Forms.Button();
            this.btn上传文件 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel_NodeTree.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_archiveFile)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(2, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(996, 696);
            this.panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel_NodeTree);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Font = new System.Drawing.Font("微软雅黑", 13F);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView_archiveFile);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.splitContainer1.Size = new System.Drawing.Size(996, 696);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 0;
            // 
            // panel_NodeTree
            // 
            this.panel_NodeTree.Controls.Add(this.treeView_Archive);
            this.panel_NodeTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_NodeTree.Location = new System.Drawing.Point(0, 105);
            this.panel_NodeTree.Name = "panel_NodeTree";
            this.panel_NodeTree.Size = new System.Drawing.Size(350, 591);
            this.panel_NodeTree.TabIndex = 2;
            // 
            // treeView_Archive
            // 
            this.treeView_Archive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Archive.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.treeView_Archive.ItemHeight = 38;
            this.treeView_Archive.Location = new System.Drawing.Point(0, 0);
            this.treeView_Archive.Name = "treeView_Archive";
            this.treeView_Archive.Size = new System.Drawing.Size(350, 591);
            this.treeView_Archive.TabIndex = 0;
            this.treeView_Archive.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_Archive_BeforeCollapse);
            this.treeView_Archive.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_Archive_BeforeSelect);
            this.treeView_Archive.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_Archive_NodeMouseClick);
            this.treeView_Archive.Leave += new System.EventHandler(this.treeView_Archive_Leave);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(350, 105);
            this.panel3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(138, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 28);
            this.label3.TabIndex = 0;
            this.label3.Text = "档案库";
            // 
            // dataGridView_archiveFile
            // 
            this.dataGridView_archiveFile.AllowUserToAddRows = false;
            this.dataGridView_archiveFile.AllowUserToDeleteRows = false;
            this.dataGridView_archiveFile.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_archiveFile.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridView_archiveFile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView_archiveFile.ColumnHeadersHeight = 45;
            this.dataGridView_archiveFile.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column8,
            this.Column9,
            this.Column2,
            this.Column10,
            this.Column7});
            this.dataGridView_archiveFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_archiveFile.GridColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataGridView_archiveFile.Location = new System.Drawing.Point(0, 60);
            this.dataGridView_archiveFile.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView_archiveFile.Name = "dataGridView_archiveFile";
            this.dataGridView_archiveFile.ReadOnly = true;
            this.dataGridView_archiveFile.RowTemplate.Height = 38;
            this.dataGridView_archiveFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_archiveFile.Size = new System.Drawing.Size(643, 636);
            this.dataGridView_archiveFile.TabIndex = 12;
            this.dataGridView_archiveFile.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_archiveFile_CellContentClick);
            this.dataGridView_archiveFile.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_archiveFile_CellDoubleClick);
            this.dataGridView_archiveFile.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_archiveFile_CellMouseUp);
            this.dataGridView_archiveFile.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView_DataBindingComplete);
            this.dataGridView_archiveFile.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_archiveFile_RowPostPaint);
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
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column1.Width = 50;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Column3.DataPropertyName = "name";
            this.Column3.FillWeight = 90F;
            this.Column3.HeaderText = "文件名称";
            this.Column3.MinimumWidth = 350;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column4.DataPropertyName = "frameName";
            this.Column4.FillWeight = 10F;
            this.Column4.HeaderText = "图幅";
            this.Column4.MinimumWidth = 65;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column4.Width = 65;
            // 
            // Column5
            // 
            this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column5.DataPropertyName = "folded";
            this.Column5.FillWeight = 10F;
            this.Column5.HeaderText = "折A1";
            this.Column5.MinimumWidth = 65;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column5.Width = 65;
            // 
            // Column6
            // 
            this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column6.DataPropertyName = "uuName";
            this.Column6.FillWeight = 10F;
            this.Column6.HeaderText = "上传人";
            this.Column6.MinimumWidth = 80;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column6.Width = 80;
            // 
            // Column8
            // 
            this.Column8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column8.DataPropertyName = "guName";
            this.Column8.FillWeight = 10F;
            this.Column8.HeaderText = "归档人";
            this.Column8.MinimumWidth = 80;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column8.Width = 80;
            // 
            // Column9
            // 
            this.Column9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column9.DataPropertyName = "resultName";
            this.Column9.FillWeight = 10F;
            this.Column9.HeaderText = "审批人";
            this.Column9.MinimumWidth = 80;
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Column9.Width = 80;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column2.DataPropertyName = "createTime";
            this.Column2.FillWeight = 10F;
            this.Column2.HeaderText = "归档时间";
            this.Column2.MinimumWidth = 240;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 240;
            // 
            // Column10
            // 
            this.Column10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column10.DataPropertyName = "upDataTime";
            this.Column10.FillWeight = 10F;
            this.Column10.HeaderText = "更新时间";
            this.Column10.MinimumWidth = 240;
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 240;
            // 
            // Column7
            // 
            this.Column7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Column7.DataPropertyName = "majorName";
            this.Column7.FillWeight = 10F;
            this.Column7.HeaderText = "专业";
            this.Column7.MinimumWidth = 80;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 80;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel1.Controls.Add(this.btn文件清单);
            this.panel1.Controls.Add(this.btn搜索);
            this.panel1.Controls.Add(this.btn发起审批);
            this.panel1.Controls.Add(this.btn全选);
            this.panel1.Controls.Add(this.btn删除文件);
            this.panel1.Controls.Add(this.btn上传文件);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(643, 60);
            this.panel1.TabIndex = 0;
            // 
            // btn文件清单
            // 
            this.btn文件清单.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn文件清单.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn文件清单.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btn文件清单.ForeColor = System.Drawing.Color.White;
            this.btn文件清单.Location = new System.Drawing.Point(500, 0);
            this.btn文件清单.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btn文件清单.Name = "btn文件清单";
            this.btn文件清单.Size = new System.Drawing.Size(100, 60);
            this.btn文件清单.TabIndex = 45;
            this.btn文件清单.Text = "选定文件清单";
            this.btn文件清单.UseVisualStyleBackColor = true;
            this.btn文件清单.Click += new System.EventHandler(this.btn文件清单_Click);
            // 
            // btn搜索
            // 
            this.btn搜索.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn搜索.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn搜索.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn搜索.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btn搜索.ForeColor = System.Drawing.Color.White;
            this.btn搜索.Location = new System.Drawing.Point(400, 0);
            this.btn搜索.Margin = new System.Windows.Forms.Padding(4);
            this.btn搜索.Name = "btn搜索";
            this.btn搜索.Size = new System.Drawing.Size(100, 60);
            this.btn搜索.TabIndex = 44;
            this.btn搜索.Tag = "promanage:list";
            this.btn搜索.Text = "搜索";
            this.btn搜索.UseVisualStyleBackColor = false;
            this.btn搜索.Click += new System.EventHandler(this.btn搜索_Click);
            // 
            // btn发起审批
            // 
            this.btn发起审批.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn发起审批.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn发起审批.Enabled = false;
            this.btn发起审批.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn发起审批.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btn发起审批.ForeColor = System.Drawing.Color.White;
            this.btn发起审批.Location = new System.Drawing.Point(300, 0);
            this.btn发起审批.Name = "btn发起审批";
            this.btn发起审批.Size = new System.Drawing.Size(100, 60);
            this.btn发起审批.TabIndex = 43;
            this.btn发起审批.Text = "发起审批";
            this.btn发起审批.UseVisualStyleBackColor = false;
            this.btn发起审批.Click += new System.EventHandler(this.btn发起审批_Click);
            // 
            // btn全选
            // 
            this.btn全选.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn全选.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn全选.Enabled = false;
            this.btn全选.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn全选.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btn全选.ForeColor = System.Drawing.Color.White;
            this.btn全选.Location = new System.Drawing.Point(200, 0);
            this.btn全选.Name = "btn全选";
            this.btn全选.Size = new System.Drawing.Size(100, 60);
            this.btn全选.TabIndex = 40;
            this.btn全选.Text = "全选";
            this.btn全选.UseVisualStyleBackColor = false;
            this.btn全选.Click += new System.EventHandler(this.btn全选_Click);
            // 
            // btn删除文件
            // 
            this.btn删除文件.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn删除文件.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn删除文件.Enabled = false;
            this.btn删除文件.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn删除文件.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btn删除文件.ForeColor = System.Drawing.Color.White;
            this.btn删除文件.Location = new System.Drawing.Point(100, 0);
            this.btn删除文件.Name = "btn删除文件";
            this.btn删除文件.Size = new System.Drawing.Size(100, 60);
            this.btn删除文件.TabIndex = 39;
            this.btn删除文件.Text = "删除文件";
            this.btn删除文件.UseVisualStyleBackColor = false;
            this.btn删除文件.Click += new System.EventHandler(this.btn删除文件_Click);
            // 
            // btn上传文件
            // 
            this.btn上传文件.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn上传文件.Dock = System.Windows.Forms.DockStyle.Left;
            this.btn上传文件.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn上传文件.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.btn上传文件.ForeColor = System.Drawing.Color.White;
            this.btn上传文件.Location = new System.Drawing.Point(0, 0);
            this.btn上传文件.Margin = new System.Windows.Forms.Padding(0);
            this.btn上传文件.Name = "btn上传文件";
            this.btn上传文件.Size = new System.Drawing.Size(100, 60);
            this.btn上传文件.TabIndex = 38;
            this.btn上传文件.Text = "上传文件";
            this.btn上传文件.UseVisualStyleBackColor = false;
            this.btn上传文件.Click += new System.EventHandler(this.btn上传文件_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(397, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 20);
            this.label2.TabIndex = 26;
            this.label2.Text = "折A1数量：0   A1";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(396, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 20);
            this.label1.TabIndex = 25;
            this.label1.Text = "文件 数量：0";
            // 
            // FrmArchiveManage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.Name = "FrmArchiveManage";
            this.Text = "归档管理";
            this.Load += new System.EventHandler(this.FrmProjectFile_Load);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel_NodeTree.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_archiveFile)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;       
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView_Archive;
        private System.Windows.Forms.DataGridView dataGridView_archiveFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn上传文件;
        private System.Windows.Forms.Button btn全选;
        private System.Windows.Forms.Button btn发起审批;
        private System.Windows.Forms.Button btn删除文件;
        private System.Windows.Forms.Button btn搜索;
        private System.Windows.Forms.Panel panel_NodeTree;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn文件清单;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}