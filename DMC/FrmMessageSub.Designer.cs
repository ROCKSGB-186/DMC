namespace DMC
{
    partial class FrmMessageSub
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
            this.button_已读 = new System.Windows.Forms.Button();
            this.richTextBox_Message = new System.Windows.Forms.RichTextBox();
            this.label标题 = new System.Windows.Forms.Label();
            this.label_流程标题 = new System.Windows.Forms.Label();
            this.label_项目名称 = new System.Windows.Forms.Label();
            this.label项目名称 = new System.Windows.Forms.Label();
            this.label_发起人 = new System.Windows.Forms.Label();
            this.label发起人 = new System.Windows.Forms.Label();
            this.button_link = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_已读
            // 
            this.button_已读.BackColor = System.Drawing.Color.White;
            this.button_已读.BackgroundImage = global::DMC.Properties.Resources.立体按键;
            this.button_已读.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_已读.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_已读.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.button_已读.ForeColor = System.Drawing.Color.White;
            this.button_已读.Location = new System.Drawing.Point(381, 39);
            this.button_已读.Margin = new System.Windows.Forms.Padding(5);
            this.button_已读.Name = "button_已读";
            this.button_已读.Size = new System.Drawing.Size(94, 28);
            this.button_已读.TabIndex = 2;
            this.button_已读.Text = "已读";
            this.button_已读.UseVisualStyleBackColor = false;
            this.button_已读.Click += new System.EventHandler(this.button_已读_Click);
            // 
            // richTextBox_Message
            // 
            this.richTextBox_Message.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_Message.Location = new System.Drawing.Point(8, 72);
            this.richTextBox_Message.Margin = new System.Windows.Forms.Padding(5);
            this.richTextBox_Message.Name = "richTextBox_Message";
            this.richTextBox_Message.ReadOnly = true;
            this.richTextBox_Message.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox_Message.Size = new System.Drawing.Size(464, 86);
            this.richTextBox_Message.TabIndex = 0;
            this.richTextBox_Message.Text = "";
            // 
            // label标题
            // 
            this.label标题.AutoSize = true;
            this.label标题.Location = new System.Drawing.Point(9, 18);
            this.label标题.Name = "label标题";
            this.label标题.Size = new System.Drawing.Size(78, 21);
            this.label标题.TabIndex = 17;
            this.label标题.Text = "流程标题:";
            // 
            // label_流程标题
            // 
            this.label_流程标题.AutoSize = true;
            this.label_流程标题.Location = new System.Drawing.Point(93, 18);
            this.label_流程标题.Name = "label_流程标题";
            this.label_流程标题.Size = new System.Drawing.Size(14, 21);
            this.label_流程标题.TabIndex = 18;
            this.label_流程标题.Text = ".";
            // 
            // label_项目名称
            // 
            this.label_项目名称.AutoSize = true;
            this.label_项目名称.Location = new System.Drawing.Point(93, 44);
            this.label_项目名称.Name = "label_项目名称";
            this.label_项目名称.Size = new System.Drawing.Size(14, 21);
            this.label_项目名称.TabIndex = 20;
            this.label_项目名称.Text = ".";
            // 
            // label项目名称
            // 
            this.label项目名称.AutoSize = true;
            this.label项目名称.Location = new System.Drawing.Point(9, 44);
            this.label项目名称.Name = "label项目名称";
            this.label项目名称.Size = new System.Drawing.Size(78, 21);
            this.label项目名称.TabIndex = 19;
            this.label项目名称.Text = "项目名称:";
            // 
            // label_发起人
            // 
            this.label_发起人.AutoSize = true;
            this.label_发起人.Location = new System.Drawing.Point(289, 18);
            this.label_发起人.Name = "label_发起人";
            this.label_发起人.Size = new System.Drawing.Size(14, 21);
            this.label_发起人.TabIndex = 22;
            this.label_发起人.Text = ".";
            // 
            // label发起人
            // 
            this.label发起人.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label发起人.AutoSize = true;
            this.label发起人.Location = new System.Drawing.Point(221, 18);
            this.label发起人.Name = "label发起人";
            this.label发起人.Size = new System.Drawing.Size(62, 21);
            this.label发起人.TabIndex = 21;
            this.label发起人.Text = "发起人:";
            // 
            // button_link
            // 
            this.button_link.BackColor = System.Drawing.Color.White;
            this.button_link.BackgroundImage = global::DMC.Properties.Resources.立体按键;
            this.button_link.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_link.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_link.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Bold);
            this.button_link.ForeColor = System.Drawing.Color.White;
            this.button_link.Location = new System.Drawing.Point(381, 9);
            this.button_link.Margin = new System.Windows.Forms.Padding(5);
            this.button_link.Name = "button_link";
            this.button_link.Size = new System.Drawing.Size(94, 28);
            this.button_link.TabIndex = 23;
            this.button_link.Text = "链接至流程";
            this.button_link.UseVisualStyleBackColor = false;
            this.button_link.Click += new System.EventHandler(this.button_link_Click);
            // 
            // FrmMessageSub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 165);
            this.Controls.Add(this.button_已读);
            this.Controls.Add(this.button_link);
            this.Controls.Add(this.richTextBox_Message);
            this.Controls.Add(this.label_发起人);
            this.Controls.Add(this.label发起人);
            this.Controls.Add(this.label项目名称);
            this.Controls.Add(this.label_流程标题);
            this.Controls.Add(this.label标题);
            this.Controls.Add(this.label_项目名称);
            this.Margin = new System.Windows.Forms.Padding(8, 9, 8, 9);
            this.Name = "FrmMessageSub";
            this.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Text = "FrmMessageSub";
            this.Load += new System.EventHandler(this.FrmMessageSub_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button_已读;
        private System.Windows.Forms.RichTextBox richTextBox_Message;
        private System.Windows.Forms.Label label_流程标题;
        private System.Windows.Forms.Label label标题;
        private System.Windows.Forms.Label label_发起人;
        private System.Windows.Forms.Label label发起人;
        private System.Windows.Forms.Label label_项目名称;
        private System.Windows.Forms.Label label项目名称;
        private System.Windows.Forms.Button button_link;
    }
}