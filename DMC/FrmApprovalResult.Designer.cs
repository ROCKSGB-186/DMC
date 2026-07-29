namespace DMC
{
    partial class FrmApprovalResult
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button取消 = new System.Windows.Forms.Button();
            this.button确定 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonInitiatorUser = new System.Windows.Forms.RadioButton();
            this.radioButtonSelectUser = new System.Windows.Forms.RadioButton();
            this.checkedListBoxSelectUser = new System.Windows.Forms.CheckedListBox();
            this.checkBoxSelectUser = new System.Windows.Forms.CheckBox();
            this.comCheckBoxList1 = new DMC.MyControl.ComCheckBoxList();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.textBox1.Location = new System.Drawing.Point(55, 306);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(686, 176);
            this.textBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(198, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "请填写您的审批意见:";
            // 
            // button取消
            // 
            this.button取消.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button取消.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button取消.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button取消.ForeColor = System.Drawing.Color.White;
            this.button取消.Location = new System.Drawing.Point(222, 519);
            this.button取消.Name = "button取消";
            this.button取消.Size = new System.Drawing.Size(150, 50);
            this.button取消.TabIndex = 5;
            this.button取消.Text = "取消";
            this.button取消.UseVisualStyleBackColor = false;
            this.button取消.Click += new System.EventHandler(this.button取消_Click);
            // 
            // button确定
            // 
            this.button确定.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.button确定.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button确定.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button确定.ForeColor = System.Drawing.Color.White;
            this.button确定.Location = new System.Drawing.Point(421, 519);
            this.button确定.Name = "button确定";
            this.button确定.Size = new System.Drawing.Size(150, 50);
            this.button确定.TabIndex = 4;
            this.button确定.Text = "确定";
            this.button确定.UseVisualStyleBackColor = false;
            this.button确定.Click += new System.EventHandler(this.button确定_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(796, 40);
            this.panel1.TabIndex = 6;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseMove);
            // 
            // radioButtonInitiatorUser
            // 
            this.radioButtonInitiatorUser.AutoSize = true;
            this.radioButtonInitiatorUser.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.radioButtonInitiatorUser.Location = new System.Drawing.Point(606, 78);
            this.radioButtonInitiatorUser.Name = "radioButtonInitiatorUser";
            this.radioButtonInitiatorUser.Size = new System.Drawing.Size(135, 28);
            this.radioButtonInitiatorUser.TabIndex = 8;
            this.radioButtonInitiatorUser.Text = "流程 (发起者)";
            this.radioButtonInitiatorUser.UseVisualStyleBackColor = true;
            this.radioButtonInitiatorUser.MouseCaptureChanged += new System.EventHandler(this.InitiatorUser);
            // 
            // radioButtonSelectUser
            // 
            this.radioButtonSelectUser.AutoSize = true;
            this.radioButtonSelectUser.Checked = true;
            this.radioButtonSelectUser.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.radioButtonSelectUser.Location = new System.Drawing.Point(55, 78);
            this.radioButtonSelectUser.Name = "radioButtonSelectUser";
            this.radioButtonSelectUser.Size = new System.Drawing.Size(190, 28);
            this.radioButtonSelectUser.TabIndex = 9;
            this.radioButtonSelectUser.TabStop = true;
            this.radioButtonSelectUser.Text = "选择审批意见接收人";
            this.radioButtonSelectUser.UseVisualStyleBackColor = true;
            this.radioButtonSelectUser.MouseCaptureChanged += new System.EventHandler(this.SelcetUser);
            // 
            // checkedListBoxSelectUser
            // 
            this.checkedListBoxSelectUser.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.checkedListBoxSelectUser.FormattingEnabled = true;
            this.checkedListBoxSelectUser.HorizontalScrollbar = true;
            this.checkedListBoxSelectUser.Location = new System.Drawing.Point(55, 112);
            this.checkedListBoxSelectUser.Name = "checkedListBoxSelectUser";
            this.checkedListBoxSelectUser.Size = new System.Drawing.Size(686, 104);
            this.checkedListBoxSelectUser.TabIndex = 10;
            // 
            // checkBoxSelectUser
            // 
            this.checkBoxSelectUser.AutoSize = true;
            this.checkBoxSelectUser.Checked = true;
            this.checkBoxSelectUser.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSelectUser.Location = new System.Drawing.Point(271, 81);
            this.checkBoxSelectUser.Name = "checkBoxSelectUser";
            this.checkBoxSelectUser.Size = new System.Drawing.Size(93, 25);
            this.checkBoxSelectUser.TabIndex = 11;
            this.checkBoxSelectUser.Text = "全部选择";
            this.checkBoxSelectUser.UseVisualStyleBackColor = true;
            this.checkBoxSelectUser.CheckedChanged += new System.EventHandler(this.checkBoxSelectUser_CheckedChanged);
            // 
            // comCheckBoxList1
            // 
            this.comCheckBoxList1.DataSource = null;
            this.comCheckBoxList1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comCheckBoxList1.Location = new System.Drawing.Point(55, 224);
            this.comCheckBoxList1.Margin = new System.Windows.Forms.Padding(5);
            this.comCheckBoxList1.Name = "comCheckBoxList1";
            this.comCheckBoxList1.Size = new System.Drawing.Size(686, 32);
            this.comCheckBoxList1.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(51, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 24);
            this.label2.TabIndex = 22;
            this.label2.Text = "*填写审批意见:";
            // 
            // FrmApprovalResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comCheckBoxList1);
            this.Controls.Add(this.checkBoxSelectUser);
            this.Controls.Add(this.checkedListBoxSelectUser);
            this.Controls.Add(this.radioButtonSelectUser);
            this.Controls.Add(this.radioButtonInitiatorUser);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button取消);
            this.Controls.Add(this.button确定);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmApprovalResult";
            this.Text = "流程审批结果提交";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button取消;
        private System.Windows.Forms.Button button确定;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonInitiatorUser;
        private System.Windows.Forms.RadioButton radioButtonSelectUser;
        private System.Windows.Forms.CheckedListBox checkedListBoxSelectUser;
        private System.Windows.Forms.CheckBox checkBoxSelectUser;
        private MyControl.ComCheckBoxList comCheckBoxList1;
        private System.Windows.Forms.Label label2;
    }
}