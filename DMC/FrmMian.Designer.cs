namespace DMC
{
    partial class FrmMian
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMian));
            this.contextMenuStripMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMain = new System.Windows.Forms.NotifyIcon(this.components);
            this.panelSubSide = new System.Windows.Forms.Panel();
            this.tabControl_mainWindows = new System.Windows.Forms.TabControl();
            this.panel左侧主按键 = new System.Windows.Forms.Panel();
            this.panelShowSelect = new System.Windows.Forms.Panel();
            this.btnSignatureTools = new System.Windows.Forms.Button();
            this.button统计 = new System.Windows.Forms.Button();
            this.panel系统设置 = new System.Windows.Forms.Panel();
            this.butADMIN = new System.Windows.Forms.Button();
            this.button退出 = new System.Windows.Forms.Button();
            this.button切换用户 = new System.Windows.Forms.Button();
            this.button修改密码 = new System.Windows.Forms.Button();
            this.button服务器与端口 = new System.Windows.Forms.Button();
            this.button系统设置 = new System.Windows.Forms.Button();
            this.panel档案库 = new System.Windows.Forms.Panel();
            this.button纬衡档案 = new System.Windows.Forms.Button();
            this.button签章平台档案 = new System.Windows.Forms.Button();
            this.button档案管理 = new System.Windows.Forms.Button();
            this.button消息管理 = new System.Windows.Forms.Button();
            this.panel项目 = new System.Windows.Forms.Panel();
            this.button项目管理 = new System.Windows.Forms.Button();
            this.button项目文件 = new System.Windows.Forms.Button();
            this.button项目 = new System.Windows.Forms.Button();
            this.button流程事务 = new System.Windows.Forms.Button();
            this.MainBtn = new System.Windows.Forms.Button();
            this.panelLogo = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.logoText2 = new System.Windows.Forms.Label();
            this.logoText1 = new System.Windows.Forms.Label();
            this.panelMinMaxClose = new System.Windows.Forms.Panel();
            this.LogoText = new System.Windows.Forms.Label();
            this.buttonMinSide = new System.Windows.Forms.Button();
            this.buttonMaxSide = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_user = new System.Windows.Forms.Label();
            this.label_time = new System.Windows.Forms.Label();
            this.contextMenuStripMain.SuspendLayout();
            this.panelSubSide.SuspendLayout();
            this.panel左侧主按键.SuspendLayout();
            this.panel系统设置.SuspendLayout();
            this.panel档案库.SuspendLayout();
            this.panel项目.SuspendLayout();
            this.panelLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelMinMaxClose.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripMain
            // 
            this.contextMenuStripMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem5});
            this.contextMenuStripMain.Name = "contextMenuStripMain";
            this.contextMenuStripMain.ShowItemToolTips = false;
            this.contextMenuStripMain.Size = new System.Drawing.Size(125, 26);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem5.Text = "退出系统";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // notifyIconMain
            // 
            this.notifyIconMain.ContextMenuStrip = this.contextMenuStripMain;
            this.notifyIconMain.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIconMain.Icon")));
            this.notifyIconMain.Text = "美智数字签章档案管理系统2.0";
            this.notifyIconMain.Visible = true;
            this.notifyIconMain.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIconMain_MouseDoubleClick);
            // 
            // panelSubSide
            // 
            this.panelSubSide.AutoScroll = true;
            this.panelSubSide.AutoSize = true;
            this.panelSubSide.Controls.Add(this.tabControl_mainWindows);
            this.panelSubSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSubSide.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panelSubSide.Location = new System.Drawing.Point(203, 41);
            this.panelSubSide.Margin = new System.Windows.Forms.Padding(0);
            this.panelSubSide.Name = "panelSubSide";
            this.panelSubSide.Size = new System.Drawing.Size(1074, 820);
            this.panelSubSide.TabIndex = 12;
            // 
            // tabControl_mainWindows
            // 
            this.tabControl_mainWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_mainWindows.Font = new System.Drawing.Font("微软雅黑", 13.8F, System.Drawing.FontStyle.Bold);
            this.tabControl_mainWindows.Location = new System.Drawing.Point(0, 0);
            this.tabControl_mainWindows.Name = "tabControl_mainWindows";
            this.tabControl_mainWindows.SelectedIndex = 0;
            this.tabControl_mainWindows.Size = new System.Drawing.Size(1074, 820);
            this.tabControl_mainWindows.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl_mainWindows.TabIndex = 0;
            // 
            // panel左侧主按键
            // 
            this.panel左侧主按键.AutoScroll = true;
            this.panel左侧主按键.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.panel左侧主按键.Controls.Add(this.panelShowSelect);
            this.panel左侧主按键.Controls.Add(this.btnSignatureTools);
            this.panel左侧主按键.Controls.Add(this.button统计);
            this.panel左侧主按键.Controls.Add(this.panel系统设置);
            this.panel左侧主按键.Controls.Add(this.button系统设置);
            this.panel左侧主按键.Controls.Add(this.panel档案库);
            this.panel左侧主按键.Controls.Add(this.button档案管理);
            this.panel左侧主按键.Controls.Add(this.button消息管理);
            this.panel左侧主按键.Controls.Add(this.panel项目);
            this.panel左侧主按键.Controls.Add(this.button项目);
            this.panel左侧主按键.Controls.Add(this.button流程事务);
            this.panel左侧主按键.Controls.Add(this.MainBtn);
            this.panel左侧主按键.Controls.Add(this.panelLogo);
            this.panel左侧主按键.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel左侧主按键.Location = new System.Drawing.Point(3, 41);
            this.panel左侧主按键.Name = "panel左侧主按键";
            this.panel左侧主按键.Size = new System.Drawing.Size(200, 820);
            this.panel左侧主按键.TabIndex = 4;
            // 
            // panelShowSelect
            // 
            this.panelShowSelect.BackColor = System.Drawing.Color.Orange;
            this.panelShowSelect.Location = new System.Drawing.Point(0, 155);
            this.panelShowSelect.Name = "panelShowSelect";
            this.panelShowSelect.Size = new System.Drawing.Size(10, 70);
            this.panelShowSelect.TabIndex = 23;
            // 
            // btnSignatureTools
            // 
            this.btnSignatureTools.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btnSignatureTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnSignatureTools.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSignatureTools.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.btnSignatureTools.ForeColor = System.Drawing.Color.White;
            this.btnSignatureTools.Location = new System.Drawing.Point(0, 1206);
            this.btnSignatureTools.Name = "btnSignatureTools";
            this.btnSignatureTools.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnSignatureTools.Size = new System.Drawing.Size(183, 65);
            this.btnSignatureTools.TabIndex = 22;
            this.btnSignatureTools.Text = "手动签名签章";
            this.btnSignatureTools.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSignatureTools.UseVisualStyleBackColor = false;
            this.btnSignatureTools.Click += new System.EventHandler(this.btnSignatureTools_Click);
            // 
            // button统计
            // 
            this.button统计.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button统计.Dock = System.Windows.Forms.DockStyle.Top;
            this.button统计.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button统计.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.button统计.ForeColor = System.Drawing.Color.White;
            this.button统计.Location = new System.Drawing.Point(0, 1140);
            this.button统计.Name = "button统计";
            this.button统计.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button统计.Size = new System.Drawing.Size(183, 66);
            this.button统计.TabIndex = 19;
            this.button统计.Text = "统计";
            this.button统计.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button统计.UseVisualStyleBackColor = false;
            this.button统计.Click += new System.EventHandler(this.button统计_Click);
            // 
            // panel系统设置
            // 
            this.panel系统设置.Controls.Add(this.butADMIN);
            this.panel系统设置.Controls.Add(this.button退出);
            this.panel系统设置.Controls.Add(this.button切换用户);
            this.panel系统设置.Controls.Add(this.button修改密码);
            this.panel系统设置.Controls.Add(this.button服务器与端口);
            this.panel系统设置.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel系统设置.Location = new System.Drawing.Point(0, 815);
            this.panel系统设置.Name = "panel系统设置";
            this.panel系统设置.Size = new System.Drawing.Size(183, 325);
            this.panel系统设置.TabIndex = 17;
            // 
            // butADMIN
            // 
            this.butADMIN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.butADMIN.Dock = System.Windows.Forms.DockStyle.Top;
            this.butADMIN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butADMIN.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.butADMIN.ForeColor = System.Drawing.Color.White;
            this.butADMIN.Location = new System.Drawing.Point(0, 260);
            this.butADMIN.Name = "butADMIN";
            this.butADMIN.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.butADMIN.Size = new System.Drawing.Size(183, 65);
            this.butADMIN.TabIndex = 5;
            this.butADMIN.Text = "ADMIN";
            this.butADMIN.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.butADMIN.UseVisualStyleBackColor = false;
            this.butADMIN.Click += new System.EventHandler(this.ButADMIN_Click);
            // 
            // button退出
            // 
            this.button退出.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button退出.Dock = System.Windows.Forms.DockStyle.Top;
            this.button退出.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button退出.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button退出.ForeColor = System.Drawing.Color.White;
            this.button退出.Location = new System.Drawing.Point(0, 195);
            this.button退出.Name = "button退出";
            this.button退出.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button退出.Size = new System.Drawing.Size(183, 65);
            this.button退出.TabIndex = 4;
            this.button退出.Text = "退出";
            this.button退出.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button退出.UseVisualStyleBackColor = false;
            this.button退出.Click += new System.EventHandler(this.button退出_Click);
            // 
            // button切换用户
            // 
            this.button切换用户.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button切换用户.Dock = System.Windows.Forms.DockStyle.Top;
            this.button切换用户.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button切换用户.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button切换用户.ForeColor = System.Drawing.Color.White;
            this.button切换用户.Location = new System.Drawing.Point(0, 130);
            this.button切换用户.Name = "button切换用户";
            this.button切换用户.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button切换用户.Size = new System.Drawing.Size(183, 65);
            this.button切换用户.TabIndex = 3;
            this.button切换用户.Text = "切换用户";
            this.button切换用户.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button切换用户.UseVisualStyleBackColor = false;
            this.button切换用户.Click += new System.EventHandler(this.button切换用户_Click);
            // 
            // button修改密码
            // 
            this.button修改密码.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button修改密码.Dock = System.Windows.Forms.DockStyle.Top;
            this.button修改密码.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button修改密码.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button修改密码.ForeColor = System.Drawing.Color.White;
            this.button修改密码.Location = new System.Drawing.Point(0, 65);
            this.button修改密码.Name = "button修改密码";
            this.button修改密码.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button修改密码.Size = new System.Drawing.Size(183, 65);
            this.button修改密码.TabIndex = 2;
            this.button修改密码.Text = "修改密码";
            this.button修改密码.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button修改密码.UseVisualStyleBackColor = false;
            this.button修改密码.Click += new System.EventHandler(this.button修改密码_Click);
            // 
            // button服务器与端口
            // 
            this.button服务器与端口.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button服务器与端口.Dock = System.Windows.Forms.DockStyle.Top;
            this.button服务器与端口.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button服务器与端口.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button服务器与端口.ForeColor = System.Drawing.Color.White;
            this.button服务器与端口.Location = new System.Drawing.Point(0, 0);
            this.button服务器与端口.Name = "button服务器与端口";
            this.button服务器与端口.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button服务器与端口.Size = new System.Drawing.Size(183, 65);
            this.button服务器与端口.TabIndex = 1;
            this.button服务器与端口.Text = "服务器与端口";
            this.button服务器与端口.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button服务器与端口.UseVisualStyleBackColor = false;
            this.button服务器与端口.Click += new System.EventHandler(this.button服务器与端口_Click);
            // 
            // button系统设置
            // 
            this.button系统设置.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button系统设置.Dock = System.Windows.Forms.DockStyle.Top;
            this.button系统设置.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button系统设置.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.button系统设置.ForeColor = System.Drawing.Color.White;
            this.button系统设置.Location = new System.Drawing.Point(0, 749);
            this.button系统设置.Name = "button系统设置";
            this.button系统设置.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button系统设置.Size = new System.Drawing.Size(183, 66);
            this.button系统设置.TabIndex = 16;
            this.button系统设置.Text = "系统设置";
            this.button系统设置.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button系统设置.UseVisualStyleBackColor = false;
            this.button系统设置.Click += new System.EventHandler(this.button系统设置_Click);
            // 
            // panel档案库
            // 
            this.panel档案库.Controls.Add(this.button纬衡档案);
            this.panel档案库.Controls.Add(this.button签章平台档案);
            this.panel档案库.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel档案库.Location = new System.Drawing.Point(0, 619);
            this.panel档案库.Name = "panel档案库";
            this.panel档案库.Size = new System.Drawing.Size(183, 130);
            this.panel档案库.TabIndex = 15;
            // 
            // button纬衡档案
            // 
            this.button纬衡档案.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button纬衡档案.Dock = System.Windows.Forms.DockStyle.Top;
            this.button纬衡档案.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button纬衡档案.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button纬衡档案.ForeColor = System.Drawing.Color.White;
            this.button纬衡档案.Location = new System.Drawing.Point(0, 65);
            this.button纬衡档案.Name = "button纬衡档案";
            this.button纬衡档案.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button纬衡档案.Size = new System.Drawing.Size(183, 65);
            this.button纬衡档案.TabIndex = 2;
            this.button纬衡档案.Text = "纬衡档案";
            this.button纬衡档案.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button纬衡档案.UseVisualStyleBackColor = false;
            this.button纬衡档案.Click += new System.EventHandler(this.button纬衡档案_Click);
            // 
            // button签章平台档案
            // 
            this.button签章平台档案.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button签章平台档案.Dock = System.Windows.Forms.DockStyle.Top;
            this.button签章平台档案.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button签章平台档案.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button签章平台档案.ForeColor = System.Drawing.Color.White;
            this.button签章平台档案.Location = new System.Drawing.Point(0, 0);
            this.button签章平台档案.Name = "button签章平台档案";
            this.button签章平台档案.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button签章平台档案.Size = new System.Drawing.Size(183, 65);
            this.button签章平台档案.TabIndex = 1;
            this.button签章平台档案.Text = "签章平台档案";
            this.button签章平台档案.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button签章平台档案.UseVisualStyleBackColor = false;
            this.button签章平台档案.Click += new System.EventHandler(this.button签章平台档案_Click);
            // 
            // button档案管理
            // 
            this.button档案管理.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button档案管理.Dock = System.Windows.Forms.DockStyle.Top;
            this.button档案管理.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button档案管理.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.button档案管理.ForeColor = System.Drawing.Color.White;
            this.button档案管理.Location = new System.Drawing.Point(0, 553);
            this.button档案管理.Name = "button档案管理";
            this.button档案管理.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button档案管理.Size = new System.Drawing.Size(183, 66);
            this.button档案管理.TabIndex = 14;
            this.button档案管理.Text = "档案库";
            this.button档案管理.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button档案管理.UseVisualStyleBackColor = false;
            this.button档案管理.Click += new System.EventHandler(this.button档案管理_Click);
            // 
            // button消息管理
            // 
            this.button消息管理.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button消息管理.Dock = System.Windows.Forms.DockStyle.Top;
            this.button消息管理.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button消息管理.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.button消息管理.ForeColor = System.Drawing.Color.White;
            this.button消息管理.Location = new System.Drawing.Point(0, 487);
            this.button消息管理.Name = "button消息管理";
            this.button消息管理.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button消息管理.Size = new System.Drawing.Size(183, 66);
            this.button消息管理.TabIndex = 12;
            this.button消息管理.Text = "消息";
            this.button消息管理.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button消息管理.UseVisualStyleBackColor = false;
            this.button消息管理.Click += new System.EventHandler(this.button消息管理_Click);
            // 
            // panel项目
            // 
            this.panel项目.Controls.Add(this.button项目管理);
            this.panel项目.Controls.Add(this.button项目文件);
            this.panel项目.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel项目.Location = new System.Drawing.Point(0, 357);
            this.panel项目.Name = "panel项目";
            this.panel项目.Size = new System.Drawing.Size(183, 130);
            this.panel项目.TabIndex = 11;
            // 
            // button项目管理
            // 
            this.button项目管理.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button项目管理.Dock = System.Windows.Forms.DockStyle.Top;
            this.button项目管理.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button项目管理.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button项目管理.ForeColor = System.Drawing.Color.White;
            this.button项目管理.Location = new System.Drawing.Point(0, 65);
            this.button项目管理.Name = "button项目管理";
            this.button项目管理.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button项目管理.Size = new System.Drawing.Size(183, 65);
            this.button项目管理.TabIndex = 2;
            this.button项目管理.Text = "项目管理";
            this.button项目管理.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button项目管理.UseVisualStyleBackColor = false;
            this.button项目管理.Click += new System.EventHandler(this.button项目管理_Click);
            // 
            // button项目文件
            // 
            this.button项目文件.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button项目文件.Dock = System.Windows.Forms.DockStyle.Top;
            this.button项目文件.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button项目文件.Font = new System.Drawing.Font("微软雅黑 Light", 12F);
            this.button项目文件.ForeColor = System.Drawing.Color.White;
            this.button项目文件.Location = new System.Drawing.Point(0, 0);
            this.button项目文件.Name = "button项目文件";
            this.button项目文件.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.button项目文件.Size = new System.Drawing.Size(183, 65);
            this.button项目文件.TabIndex = 1;
            this.button项目文件.Text = "项目文件";
            this.button项目文件.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button项目文件.UseVisualStyleBackColor = false;
            this.button项目文件.Click += new System.EventHandler(this.button项目文件_Click);
            // 
            // button项目
            // 
            this.button项目.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button项目.Dock = System.Windows.Forms.DockStyle.Top;
            this.button项目.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button项目.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.button项目.ForeColor = System.Drawing.Color.White;
            this.button项目.Location = new System.Drawing.Point(0, 291);
            this.button项目.Name = "button项目";
            this.button项目.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button项目.Size = new System.Drawing.Size(183, 66);
            this.button项目.TabIndex = 10;
            this.button项目.Text = "项目";
            this.button项目.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button项目.UseVisualStyleBackColor = false;
            this.button项目.Click += new System.EventHandler(this.button项目_Click);
            // 
            // button流程事务
            // 
            this.button流程事务.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button流程事务.Dock = System.Windows.Forms.DockStyle.Top;
            this.button流程事务.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button流程事务.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.button流程事务.ForeColor = System.Drawing.Color.White;
            this.button流程事务.Location = new System.Drawing.Point(0, 225);
            this.button流程事务.Name = "button流程事务";
            this.button流程事务.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.button流程事务.Size = new System.Drawing.Size(183, 66);
            this.button流程事务.TabIndex = 8;
            this.button流程事务.Text = "流程事务";
            this.button流程事务.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button流程事务.UseVisualStyleBackColor = false;
            this.button流程事务.Click += new System.EventHandler(this.button流程事务_Click);
            // 
            // MainBtn
            // 
            this.MainBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.MainBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.MainBtn.FlatAppearance.BorderSize = 0;
            this.MainBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainBtn.Font = new System.Drawing.Font("微软雅黑", 12.5F, System.Drawing.FontStyle.Bold);
            this.MainBtn.ForeColor = System.Drawing.Color.White;
            this.MainBtn.Location = new System.Drawing.Point(0, 155);
            this.MainBtn.Name = "MainBtn";
            this.MainBtn.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.MainBtn.Size = new System.Drawing.Size(183, 70);
            this.MainBtn.TabIndex = 1;
            this.MainBtn.Text = "主页";
            this.MainBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.MainBtn.UseVisualStyleBackColor = false;
            this.MainBtn.Click += new System.EventHandler(this.MainBtn_Click);
            // 
            // panelLogo
            // 
            this.panelLogo.BackColor = System.Drawing.Color.White;
            this.panelLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelLogo.Controls.Add(this.pictureBox1);
            this.panelLogo.Controls.Add(this.logoText2);
            this.panelLogo.Controls.Add(this.logoText1);
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLogo.Location = new System.Drawing.Point(0, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(183, 155);
            this.panelLogo.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = null;
            this.pictureBox1.Image = global::DMC.Properties.Resources.BKMZ_LOGO_蓝白底_白字;
            this.pictureBox1.Location = new System.Drawing.Point(32, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 91);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // logoText2
            // 
            this.logoText2.AutoSize = true;
            this.logoText2.BackColor = System.Drawing.Color.White;
            this.logoText2.Font = new System.Drawing.Font("微软雅黑", 8.8F, System.Drawing.FontStyle.Bold);
            this.logoText2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.logoText2.Location = new System.Drawing.Point(29, 125);
            this.logoText2.Name = "logoText2";
            this.logoText2.Size = new System.Drawing.Size(135, 17);
            this.logoText2.TabIndex = 2;
            this.logoText2.Text = "Technology Co., LTD";
            // 
            // logoText1
            // 
            this.logoText1.AutoSize = true;
            this.logoText1.BackColor = System.Drawing.Color.White;
            this.logoText1.Font = new System.Drawing.Font("微软雅黑", 8.8F, System.Drawing.FontStyle.Bold);
            this.logoText1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.logoText1.Location = new System.Drawing.Point(53, 103);
            this.logoText1.Name = "logoText1";
            this.logoText1.Size = new System.Drawing.Size(86, 17);
            this.logoText1.TabIndex = 1;
            this.logoText1.Text = "Beike Meizhi";
            // 
            // panelMinMaxClose
            // 
            this.panelMinMaxClose.AutoSize = true;
            this.panelMinMaxClose.BackColor = System.Drawing.Color.White;
            this.panelMinMaxClose.Controls.Add(this.LogoText);
            this.panelMinMaxClose.Controls.Add(this.buttonMinSide);
            this.panelMinMaxClose.Controls.Add(this.buttonMaxSide);
            this.panelMinMaxClose.Controls.Add(this.buttonClose);
            this.panelMinMaxClose.Cursor = System.Windows.Forms.Cursors.Default;
            this.panelMinMaxClose.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMinMaxClose.Location = new System.Drawing.Point(3, 3);
            this.panelMinMaxClose.MinimumSize = new System.Drawing.Size(1080, 38);
            this.panelMinMaxClose.Name = "panelMinMaxClose";
            this.panelMinMaxClose.Size = new System.Drawing.Size(1274, 38);
            this.panelMinMaxClose.TabIndex = 5;
            this.panelMinMaxClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseDown);
            this.panelMinMaxClose.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseMove);
            // 
            // LogoText
            // 
            this.LogoText.AutoSize = true;
            this.LogoText.BackColor = System.Drawing.Color.White;
            this.LogoText.Font = new System.Drawing.Font("微软雅黑", 14F, System.Drawing.FontStyle.Bold);
            this.LogoText.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.LogoText.Location = new System.Drawing.Point(6, 5);
            this.LogoText.Margin = new System.Windows.Forms.Padding(5);
            this.LogoText.Name = "LogoText";
            this.LogoText.Size = new System.Drawing.Size(202, 26);
            this.LogoText.TabIndex = 3;
            this.LogoText.Text = "美智签章档案管理平台";
            this.LogoText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonMinSide
            // 
            this.buttonMinSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.buttonMinSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonMinSide.Font = new System.Drawing.Font("微软雅黑", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonMinSide.ForeColor = System.Drawing.Color.Transparent;
            this.buttonMinSide.Location = new System.Drawing.Point(1160, 0);
            this.buttonMinSide.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMinSide.Name = "buttonMinSide";
            this.buttonMinSide.Size = new System.Drawing.Size(38, 38);
            this.buttonMinSide.TabIndex = 5;
            this.buttonMinSide.Text = "-";
            this.buttonMinSide.UseVisualStyleBackColor = false;
            this.buttonMinSide.Click += new System.EventHandler(this.buttonMinSide_Click);
            // 
            // buttonMaxSide
            // 
            this.buttonMaxSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.buttonMaxSide.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonMaxSide.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.buttonMaxSide.ForeColor = System.Drawing.Color.Transparent;
            this.buttonMaxSide.Location = new System.Drawing.Point(1198, 0);
            this.buttonMaxSide.Margin = new System.Windows.Forms.Padding(5, 0, 0, 5);
            this.buttonMaxSide.Name = "buttonMaxSide";
            this.buttonMaxSide.Size = new System.Drawing.Size(38, 38);
            this.buttonMaxSide.TabIndex = 4;
            this.buttonMaxSide.Text = "口";
            this.buttonMaxSide.UseVisualStyleBackColor = false;
            this.buttonMaxSide.Click += new System.EventHandler(this.buttonMaxSide_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.buttonClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonClose.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.buttonClose.ForeColor = System.Drawing.Color.Transparent;
            this.buttonClose.Location = new System.Drawing.Point(1236, 0);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(38, 38);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_user);
            this.panel1.Controls.Add(this.label_time);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 861);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1274, 36);
            this.panel1.TabIndex = 3;
            // 
            // label_user
            // 
            this.label_user.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label_user.AutoSize = true;
            this.label_user.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label_user.Location = new System.Drawing.Point(7, 9);
            this.label_user.Name = "label_user";
            this.label_user.Size = new System.Drawing.Size(50, 20);
            this.label_user.TabIndex = 2;
            this.label_user.Text = "label1";
            // 
            // label_time
            // 
            this.label_time.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_time.AutoSize = true;
            this.label_time.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.label_time.Location = new System.Drawing.Point(1050, 9);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(185, 20);
            this.label_time.TabIndex = 1;
            this.label_time.Text = "时间：2021-03-04 16:16:37";
            // 
            // FrmMian
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1280, 900);
            this.Controls.Add(this.panelSubSide);
            this.Controls.Add(this.panel左侧主按键);
            this.Controls.Add(this.panelMinMaxClose);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.MinimumSize = new System.Drawing.Size(1280, 900);
            this.Name = "FrmMian";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "美智数字化签章与档案管理平台2.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMian_FormClosing);
            this.Load += new System.EventHandler(this.FrmMian_Load);
            this.contextMenuStripMain.ResumeLayout(false);
            this.panelSubSide.ResumeLayout(false);
            this.panel左侧主按键.ResumeLayout(false);
            this.panel系统设置.ResumeLayout(false);
            this.panel档案库.ResumeLayout(false);
            this.panel项目.ResumeLayout(false);
            this.panelLogo.ResumeLayout(false);
            this.panelLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelMinMaxClose.ResumeLayout(false);
            this.panelMinMaxClose.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label_time;
        private System.Windows.Forms.Label label_user;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMain;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.NotifyIcon notifyIconMain;
        private System.Windows.Forms.Panel panelMinMaxClose;
        private System.Windows.Forms.Label LogoText;
        private System.Windows.Forms.Button buttonMinSide;
        private System.Windows.Forms.Button buttonMaxSide;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label logoText2;
        private System.Windows.Forms.Label logoText1;
        private System.Windows.Forms.Button MainBtn;
        private System.Windows.Forms.Button button流程事务;
        private System.Windows.Forms.Button button项目;
        private System.Windows.Forms.Panel panel项目;
        private System.Windows.Forms.Button button项目管理;
        private System.Windows.Forms.Button button项目文件;
        private System.Windows.Forms.Button button消息管理;
        private System.Windows.Forms.Button button档案管理;
        private System.Windows.Forms.Panel panel档案库;
        private System.Windows.Forms.Button button纬衡档案;
        private System.Windows.Forms.Button button签章平台档案;
        private System.Windows.Forms.Button button系统设置;
        private System.Windows.Forms.Panel panel系统设置;
        private System.Windows.Forms.Button butADMIN;
        private System.Windows.Forms.Button button退出;
        private System.Windows.Forms.Button button切换用户;
        private System.Windows.Forms.Button button修改密码;
        private System.Windows.Forms.Button button服务器与端口;
        private System.Windows.Forms.Panel panel左侧主按键;
        private System.Windows.Forms.Button btnSignatureTools;
        private System.Windows.Forms.Panel panelShowSelect;
        private System.Windows.Forms.Button button统计;
        private System.Windows.Forms.Panel panelSubSide;
        private System.Windows.Forms.TabControl tabControl_mainWindows;
    }
}

