namespace DMC
{
    partial class FrmMessagePopup
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
            this.panel_AllMessage = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel_AllMessage
            // 
            this.panel_AllMessage.AutoScroll = true;
            this.panel_AllMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_AllMessage.Location = new System.Drawing.Point(5, 5);
            this.panel_AllMessage.Margin = new System.Windows.Forms.Padding(0);
            this.panel_AllMessage.Name = "panel_AllMessage";
            this.panel_AllMessage.Size = new System.Drawing.Size(490, 490);
            this.panel_AllMessage.TabIndex = 21;
            // 
            // FrmMessagePopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(500, 500);
            this.Controls.Add(this.panel_AllMessage);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 500);
            this.Name = "FrmMessagePopup";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.Text = "系统提示";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMessagePopup_FormClosing);
            this.Load += new System.EventHandler(this.FrmMessagePopup_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel_AllMessage;
    }

}