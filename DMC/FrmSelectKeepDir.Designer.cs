namespace DMC
{
    partial class FrmSelectKeepDir
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
            this.btn确定 = new System.Windows.Forms.Button();
            this.btn取消 = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel10 = new System.Windows.Forms.Panel();
            this.btn关闭 = new System.Windows.Forms.Button();
            this.label39 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel10.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.btn确定);
            this.panel1.Controls.Add(this.btn取消);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(2, 903);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1020, 55);
            this.panel1.TabIndex = 0;
            // 
            // btn确定
            // 
            this.btn确定.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn确定.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn确定.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn确定.ForeColor = System.Drawing.Color.White;
            this.btn确定.Location = new System.Drawing.Point(547, 3);
            this.btn确定.Name = "btn确定";
            this.btn确定.Size = new System.Drawing.Size(150, 50);
            this.btn确定.TabIndex = 3;
            this.btn确定.Text = "确定";
            this.btn确定.UseVisualStyleBackColor = false;
            this.btn确定.Click += new System.EventHandler(this.btn确定_Click);
            // 
            // btn取消
            // 
            this.btn取消.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn取消.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn取消.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn取消.ForeColor = System.Drawing.Color.White;
            this.btn取消.Location = new System.Drawing.Point(345, 3);
            this.btn取消.Name = "btn取消";
            this.btn取消.Size = new System.Drawing.Size(150, 50);
            this.btn取消.TabIndex = 2;
            this.btn取消.Text = "取消";
            this.btn取消.UseVisualStyleBackColor = false;
            this.btn取消.Click += new System.EventHandler(this.btn取消_Click);
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(5, 49);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(1014, 849);
            this.treeView1.TabIndex = 1;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel10.Controls.Add(this.btn关闭);
            this.panel10.Controls.Add(this.label39);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.Location = new System.Drawing.Point(2, 2);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(1020, 40);
            this.panel10.TabIndex = 8;
            this.panel10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseDown);
            this.panel10.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseMove);
            // 
            // btn关闭
            // 
            this.btn关闭.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.btn关闭.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn关闭.Font = new System.Drawing.Font("微软雅黑", 17F, System.Drawing.FontStyle.Bold);
            this.btn关闭.ForeColor = System.Drawing.Color.Transparent;
            this.btn关闭.Location = new System.Drawing.Point(980, 0);
            this.btn关闭.Name = "btn关闭";
            this.btn关闭.Size = new System.Drawing.Size(40, 40);
            this.btn关闭.TabIndex = 29;
            this.btn关闭.Text = "X";
            this.btn关闭.UseVisualStyleBackColor = false;
            this.btn关闭.Click += new System.EventHandler(this.btn关闭_Click);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.label39.ForeColor = System.Drawing.Color.White;
            this.label39.Location = new System.Drawing.Point(12, 7);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(52, 27);
            this.label39.TabIndex = 2;
            this.label39.Text = "详情";
            // 
            // FrmSelectKeepDir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1024, 960);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 960);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1024, 960);
            this.Name = "FrmSelectKeepDir";
            this.Text = "选择归档目录";
            this.Load += new System.EventHandler(this.FrmSelectKeepDir_Load);
            this.panel1.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btn确定;
        private System.Windows.Forms.Button btn取消;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Button btn关闭;
        private System.Windows.Forms.Label label39;
    }
}