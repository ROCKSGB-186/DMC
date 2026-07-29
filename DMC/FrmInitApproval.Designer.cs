namespace DMC
{
    partial class FrmInitApproval
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn发起 = new System.Windows.Forms.Button();
            this.btn取消 = new System.Windows.Forms.Button();
            this.tabControl_发起流程 = new System.Windows.Forms.TabControl();
            this.tabPage_基本信息 = new System.Windows.Forms.TabPage();
            this.comCheckBoxList1 = new DMC.MyControl.ComCheckBoxList();
            this.label_多面码提示 = new System.Windows.Forms.Label();
            this.label_多页码选选择 = new System.Windows.Forms.Label();
            this.textBox_出版份数 = new System.Windows.Forms.TextBox();
            this.label_出版份数 = new System.Windows.Forms.Label();
            this.textBox_用户部门 = new System.Windows.Forms.TextBox();
            this.textBox_流程天数 = new System.Windows.Forms.TextBox();
            this.textBox_流程说明 = new System.Windows.Forms.TextBox();
            this.textBox_提交用户 = new System.Windows.Forms.TextBox();
            this.textBox_项目名称 = new System.Windows.Forms.TextBox();
            this.textBox_流程标题 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label_流程说明 = new System.Windows.Forms.Label();
            this.label_用户部门 = new System.Windows.Forms.Label();
            this.label_提交用户 = new System.Windows.Forms.Label();
            this.label_项目名称 = new System.Windows.Forms.Label();
            this.label_流程标题 = new System.Windows.Forms.Label();
            this.comboBox_流程类型 = new System.Windows.Forms.ComboBox();
            this.label_流程类型 = new System.Windows.Forms.Label();
            this.tabPage_流程配置 = new System.Windows.Forms.TabPage();
            this.tabPage施工图 = new System.Windows.Forms.TabPage();
            this.treeView_施工图 = new System.Windows.Forms.TreeView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.tabControl_发起流程.SuspendLayout();
            this.tabPage_基本信息.SuspendLayout();
            this.tabPage施工图.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btn发起);
            this.panel1.Controls.Add(this.btn取消);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 518);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1020, 80);
            this.panel1.TabIndex = 0;
            // 
            // btn发起
            // 
            this.btn发起.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn发起.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn发起.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn发起.ForeColor = System.Drawing.Color.White;
            this.btn发起.Location = new System.Drawing.Point(571, 11);
            this.btn发起.Name = "btn发起";
            this.btn发起.Size = new System.Drawing.Size(150, 50);
            this.btn发起.TabIndex = 1;
            this.btn发起.Text = "发起";
            this.btn发起.UseVisualStyleBackColor = false;
            this.btn发起.Click += new System.EventHandler(this.btn发起_Click);
            // 
            // btn取消
            // 
            this.btn取消.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn取消.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn取消.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn取消.ForeColor = System.Drawing.Color.White;
            this.btn取消.Location = new System.Drawing.Point(321, 11);
            this.btn取消.Name = "btn取消";
            this.btn取消.Size = new System.Drawing.Size(150, 50);
            this.btn取消.TabIndex = 0;
            this.btn取消.Text = "取消";
            this.btn取消.UseVisualStyleBackColor = false;
            this.btn取消.Click += new System.EventHandler(this.btn取消_Click);
            // 
            // tabControl_发起流程
            // 
            this.tabControl_发起流程.Controls.Add(this.tabPage_基本信息);
            this.tabControl_发起流程.Controls.Add(this.tabPage_流程配置);
            this.tabControl_发起流程.Controls.Add(this.tabPage施工图);
            this.tabControl_发起流程.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_发起流程.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.tabControl_发起流程.ItemSize = new System.Drawing.Size(75, 45);
            this.tabControl_发起流程.Location = new System.Drawing.Point(0, 0);
            this.tabControl_发起流程.Name = "tabControl_发起流程";
            this.tabControl_发起流程.SelectedIndex = 0;
            this.tabControl_发起流程.Size = new System.Drawing.Size(1020, 476);
            this.tabControl_发起流程.TabIndex = 1;
            // 
            // tabPage_基本信息
            // 
            this.tabPage_基本信息.Controls.Add(this.comCheckBoxList1);
            this.tabPage_基本信息.Controls.Add(this.label_多面码提示);
            this.tabPage_基本信息.Controls.Add(this.label_多页码选选择);
            this.tabPage_基本信息.Controls.Add(this.textBox_出版份数);
            this.tabPage_基本信息.Controls.Add(this.label_出版份数);
            this.tabPage_基本信息.Controls.Add(this.textBox_用户部门);
            this.tabPage_基本信息.Controls.Add(this.textBox_流程天数);
            this.tabPage_基本信息.Controls.Add(this.textBox_流程说明);
            this.tabPage_基本信息.Controls.Add(this.textBox_提交用户);
            this.tabPage_基本信息.Controls.Add(this.textBox_项目名称);
            this.tabPage_基本信息.Controls.Add(this.textBox_流程标题);
            this.tabPage_基本信息.Controls.Add(this.label7);
            this.tabPage_基本信息.Controls.Add(this.label_流程说明);
            this.tabPage_基本信息.Controls.Add(this.label_用户部门);
            this.tabPage_基本信息.Controls.Add(this.label_提交用户);
            this.tabPage_基本信息.Controls.Add(this.label_项目名称);
            this.tabPage_基本信息.Controls.Add(this.label_流程标题);
            this.tabPage_基本信息.Controls.Add(this.comboBox_流程类型);
            this.tabPage_基本信息.Controls.Add(this.label_流程类型);
            this.tabPage_基本信息.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabPage_基本信息.Location = new System.Drawing.Point(4, 49);
            this.tabPage_基本信息.Name = "tabPage_基本信息";
            this.tabPage_基本信息.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_基本信息.Size = new System.Drawing.Size(1012, 423);
            this.tabPage_基本信息.TabIndex = 0;
            this.tabPage_基本信息.Text = "基本信息";
            this.tabPage_基本信息.UseVisualStyleBackColor = true;
            // 
            // comCheckBoxList1
            // 
            this.comCheckBoxList1.DataSource = null;
            this.comCheckBoxList1.Font = new System.Drawing.Font("微软雅黑", 11F);
            this.comCheckBoxList1.Location = new System.Drawing.Point(543, 208);
            this.comCheckBoxList1.Margin = new System.Windows.Forms.Padding(5);
            this.comCheckBoxList1.Name = "comCheckBoxList1";
            this.comCheckBoxList1.Size = new System.Drawing.Size(367, 26);
            this.comCheckBoxList1.TabIndex = 20;
            // 
            // label_多面码提示
            // 
            this.label_多面码提示.AutoSize = true;
            this.label_多面码提示.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.label_多面码提示.ForeColor = System.Drawing.Color.Red;
            this.label_多面码提示.Location = new System.Drawing.Point(599, 170);
            this.label_多面码提示.Name = "label_多面码提示";
            this.label_多面码提示.Size = new System.Drawing.Size(266, 21);
            this.label_多面码提示.TabIndex = 19;
            this.label_多面码提示.Text = "页码可修改，最后数据以文本框为准";
            this.label_多面码提示.Visible = false;
            // 
            // label_多页码选选择
            // 
            this.label_多页码选选择.AutoSize = true;
            this.label_多页码选选择.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label_多页码选选择.Location = new System.Drawing.Point(421, 209);
            this.label_多页码选选择.Name = "label_多页码选选择";
            this.label_多页码选选择.Size = new System.Drawing.Size(126, 25);
            this.label_多页码选选择.TabIndex = 17;
            this.label_多页码选选择.Text = "多页码选择：";
            this.label_多页码选选择.Visible = false;
            // 
            // textBox_出版份数
            // 
            this.textBox_出版份数.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox_出版份数.ForeColor = System.Drawing.Color.Red;
            this.textBox_出版份数.Location = new System.Drawing.Point(543, 250);
            this.textBox_出版份数.Name = "textBox_出版份数";
            this.textBox_出版份数.Size = new System.Drawing.Size(367, 39);
            this.textBox_出版份数.TabIndex = 14;
            this.textBox_出版份数.Visible = false;
            this.textBox_出版份数.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox7出版份数_KeyPress);
            // 
            // label_出版份数
            // 
            this.label_出版份数.AutoSize = true;
            this.label_出版份数.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_出版份数.ForeColor = System.Drawing.Color.Red;
            this.label_出版份数.Location = new System.Drawing.Point(430, 255);
            this.label_出版份数.Name = "label_出版份数";
            this.label_出版份数.Size = new System.Drawing.Size(117, 28);
            this.label_出版份数.TabIndex = 15;
            this.label_出版份数.Text = "出版份数：";
            this.label_出版份数.Visible = false;
            // 
            // textBox_用户部门
            // 
            this.textBox_用户部门.Location = new System.Drawing.Point(210, 162);
            this.textBox_用户部门.Name = "textBox_用户部门";
            this.textBox_用户部门.ReadOnly = true;
            this.textBox_用户部门.Size = new System.Drawing.Size(337, 34);
            this.textBox_用户部门.TabIndex = 8;
            // 
            // textBox_流程天数
            // 
            this.textBox_流程天数.Location = new System.Drawing.Point(210, 255);
            this.textBox_流程天数.Name = "textBox_流程天数";
            this.textBox_流程天数.Size = new System.Drawing.Size(200, 34);
            this.textBox_流程天数.TabIndex = 12;
            this.textBox_流程天数.Text = "1";
            this.textBox_流程天数.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox7出版份数_KeyPress);
            // 
            // textBox_流程说明
            // 
            this.textBox_流程说明.Location = new System.Drawing.Point(210, 303);
            this.textBox_流程说明.Multiline = true;
            this.textBox_流程说明.Name = "textBox_流程说明";
            this.textBox_流程说明.Size = new System.Drawing.Size(700, 100);
            this.textBox_流程说明.TabIndex = 10;
            // 
            // textBox_提交用户
            // 
            this.textBox_提交用户.Location = new System.Drawing.Point(210, 208);
            this.textBox_提交用户.Name = "textBox_提交用户";
            this.textBox_提交用户.ReadOnly = true;
            this.textBox_提交用户.Size = new System.Drawing.Size(200, 34);
            this.textBox_提交用户.TabIndex = 6;
            // 
            // textBox_项目名称
            // 
            this.textBox_项目名称.Location = new System.Drawing.Point(210, 117);
            this.textBox_项目名称.Name = "textBox_项目名称";
            this.textBox_项目名称.ReadOnly = true;
            this.textBox_项目名称.Size = new System.Drawing.Size(700, 34);
            this.textBox_项目名称.TabIndex = 4;
            // 
            // textBox_流程标题
            // 
            this.textBox_流程标题.Location = new System.Drawing.Point(210, 73);
            this.textBox_流程标题.Name = "textBox_流程标题";
            this.textBox_流程标题.Size = new System.Drawing.Size(700, 34);
            this.textBox_流程标题.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label7.Location = new System.Drawing.Point(106, 260);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 25);
            this.label7.TabIndex = 13;
            this.label7.Text = "流程天数：";
            // 
            // label_流程说明
            // 
            this.label_流程说明.AutoSize = true;
            this.label_流程说明.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label_流程说明.Location = new System.Drawing.Point(106, 307);
            this.label_流程说明.Name = "label_流程说明";
            this.label_流程说明.Size = new System.Drawing.Size(107, 25);
            this.label_流程说明.TabIndex = 11;
            this.label_流程说明.Text = "流程说明：";
            // 
            // label_用户部门
            // 
            this.label_用户部门.AutoSize = true;
            this.label_用户部门.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label_用户部门.Location = new System.Drawing.Point(106, 167);
            this.label_用户部门.Name = "label_用户部门";
            this.label_用户部门.Size = new System.Drawing.Size(107, 25);
            this.label_用户部门.TabIndex = 9;
            this.label_用户部门.Text = "用户部门：";
            // 
            // label_提交用户
            // 
            this.label_提交用户.AutoSize = true;
            this.label_提交用户.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label_提交用户.Location = new System.Drawing.Point(106, 213);
            this.label_提交用户.Name = "label_提交用户";
            this.label_提交用户.Size = new System.Drawing.Size(107, 25);
            this.label_提交用户.TabIndex = 7;
            this.label_提交用户.Text = "提交用户：";
            // 
            // label_项目名称
            // 
            this.label_项目名称.AutoSize = true;
            this.label_项目名称.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label_项目名称.Location = new System.Drawing.Point(106, 122);
            this.label_项目名称.Name = "label_项目名称";
            this.label_项目名称.Size = new System.Drawing.Size(107, 25);
            this.label_项目名称.TabIndex = 5;
            this.label_项目名称.Text = "项目名称：";
            // 
            // label_流程标题
            // 
            this.label_流程标题.AutoSize = true;
            this.label_流程标题.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label_流程标题.Location = new System.Drawing.Point(106, 78);
            this.label_流程标题.Name = "label_流程标题";
            this.label_流程标题.Size = new System.Drawing.Size(107, 25);
            this.label_流程标题.TabIndex = 3;
            this.label_流程标题.Text = "流程标题：";
            // 
            // comboBox_流程类型
            // 
            this.comboBox_流程类型.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_流程类型.FormattingEnabled = true;
            this.comboBox_流程类型.Location = new System.Drawing.Point(210, 28);
            this.comboBox_流程类型.Name = "comboBox_流程类型";
            this.comboBox_流程类型.Size = new System.Drawing.Size(700, 35);
            this.comboBox_流程类型.TabIndex = 1;
            this.comboBox_流程类型.SelectedIndexChanged += new System.EventHandler(this.comboBox_流程类型_SelectedIndexChanged);
            // 
            // label_流程类型
            // 
            this.label_流程类型.AutoSize = true;
            this.label_流程类型.Font = new System.Drawing.Font("微软雅黑", 14F);
            this.label_流程类型.Location = new System.Drawing.Point(106, 33);
            this.label_流程类型.Name = "label_流程类型";
            this.label_流程类型.Size = new System.Drawing.Size(107, 25);
            this.label_流程类型.TabIndex = 0;
            this.label_流程类型.Text = "流程类型：";
            // 
            // tabPage_流程配置
            // 
            this.tabPage_流程配置.AutoScroll = true;
            this.tabPage_流程配置.Location = new System.Drawing.Point(4, 49);
            this.tabPage_流程配置.Name = "tabPage_流程配置";
            this.tabPage_流程配置.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_流程配置.Size = new System.Drawing.Size(1012, 423);
            this.tabPage_流程配置.TabIndex = 1;
            this.tabPage_流程配置.Text = "流程配置";
            this.tabPage_流程配置.UseVisualStyleBackColor = true;
            // 
            // tabPage施工图
            // 
            this.tabPage施工图.Controls.Add(this.treeView_施工图);
            this.tabPage施工图.Controls.Add(this.panel2);
            this.tabPage施工图.Location = new System.Drawing.Point(4, 49);
            this.tabPage施工图.Name = "tabPage施工图";
            this.tabPage施工图.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage施工图.Size = new System.Drawing.Size(1012, 423);
            this.tabPage施工图.TabIndex = 2;
            this.tabPage施工图.Text = "施工图";
            this.tabPage施工图.UseVisualStyleBackColor = true;
            // 
            // treeView_施工图
            // 
            this.treeView_施工图.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_施工图.Location = new System.Drawing.Point(3, 3);
            this.treeView_施工图.Name = "treeView_施工图";
            this.treeView_施工图.Size = new System.Drawing.Size(1006, 368);
            this.treeView_施工图.TabIndex = 3;
            this.treeView_施工图.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeCollapse);
            this.treeView_施工图.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(3, 371);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1006, 49);
            this.panel2.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(546, 15);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(165, 25);
            this.label9.TabIndex = 28;
            this.label9.Text = "总A1数量：0   A1";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(254, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(118, 25);
            this.label10.TabIndex = 27;
            this.label10.Text = "文件数量：0";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel3.Controls.Add(this.label13);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(2, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1020, 40);
            this.panel3.TabIndex = 17;
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseDown);
            this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseMove);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(8, 6);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(92, 27);
            this.label13.TabIndex = 14;
            this.label13.Text = "流程发起";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tabControl_发起流程);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(2, 42);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1020, 476);
            this.panel4.TabIndex = 18;
            // 
            // FrmInitApproval
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1024, 600);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 1024);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Name = "FrmInitApproval";
            this.Text = "发起审批";
            this.Load += new System.EventHandler(this.FrmInitApproval_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl_发起流程.ResumeLayout(false);
            this.tabPage_基本信息.ResumeLayout(false);
            this.tabPage_基本信息.PerformLayout();
            this.tabPage施工图.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl_发起流程;
        private System.Windows.Forms.TabPage tabPage_基本信息;
        private System.Windows.Forms.Button btn发起;
        private System.Windows.Forms.Button btn取消;
        private System.Windows.Forms.TabPage tabPage施工图;
        private System.Windows.Forms.TextBox textBox_流程标题;
        private System.Windows.Forms.ComboBox comboBox_流程类型;
        private System.Windows.Forms.Label label_流程类型;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_流程天数;
        private System.Windows.Forms.Label label_流程说明;
        private System.Windows.Forms.TextBox textBox_流程说明;
        private System.Windows.Forms.Label label_用户部门;
        private System.Windows.Forms.TextBox textBox_用户部门;
        private System.Windows.Forms.Label label_提交用户;
        private System.Windows.Forms.TextBox textBox_提交用户;
        private System.Windows.Forms.Label label_项目名称;
        private System.Windows.Forms.TextBox textBox_项目名称;
        private System.Windows.Forms.Label label_流程标题;
        private System.Windows.Forms.TabPage tabPage_流程配置;
        private System.Windows.Forms.TextBox textBox_出版份数;
        private System.Windows.Forms.Label label_出版份数;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TreeView treeView_施工图;
        private System.Windows.Forms.Label label_多页码选选择;
        private System.Windows.Forms.Label label_多面码提示;
        private MyControl.ComCheckBoxList comCheckBoxList1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel4;
    }
}