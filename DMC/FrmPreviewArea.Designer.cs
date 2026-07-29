namespace DMC
{
    partial class FrmPreviewArea
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
            this.label文件名 = new System.Windows.Forms.Label();
            this.buttonMinSide = new System.Windows.Forms.Button();
            this.buttonMaxSide = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button_右 = new System.Windows.Forms.Button();
            this.button_左 = new System.Windows.Forms.Button();
            this.pdfViewer2 = new PdfiumViewer.PdfViewer();
            this.button_Height = new System.Windows.Forms.Button();
            this.button_Width = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.panel1.Controls.Add(this.label文件名);
            this.panel1.Controls.Add(this.buttonMinSide);
            this.panel1.Controls.Add(this.buttonMaxSide);
            this.panel1.Controls.Add(this.buttonClose);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1020, 38);
            this.panel1.TabIndex = 1;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.WindowMove_MouseMove);
            // 
            // label文件名
            // 
            this.label文件名.AutoSize = true;
            this.label文件名.ForeColor = System.Drawing.SystemColors.Control;
            this.label文件名.Location = new System.Drawing.Point(15, 11);
            this.label文件名.Name = "label文件名";
            this.label文件名.Size = new System.Drawing.Size(0, 28);
            this.label文件名.TabIndex = 8;
            // 
            // buttonMinSide
            // 
            this.buttonMinSide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMinSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.buttonMinSide.Font = new System.Drawing.Font("微软雅黑", 20F, System.Drawing.FontStyle.Bold);
            this.buttonMinSide.ForeColor = System.Drawing.Color.Transparent;
            this.buttonMinSide.Location = new System.Drawing.Point(888, 2);
            this.buttonMinSide.Margin = new System.Windows.Forms.Padding(0);
            this.buttonMinSide.Name = "buttonMinSide";
            this.buttonMinSide.Size = new System.Drawing.Size(40, 36);
            this.buttonMinSide.TabIndex = 7;
            this.buttonMinSide.Text = "-";
            this.buttonMinSide.UseVisualStyleBackColor = false;
            this.buttonMinSide.Click += new System.EventHandler(this.buttonMinSide_Click);
            // 
            // buttonMaxSide
            // 
            this.buttonMaxSide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMaxSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.buttonMaxSide.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.buttonMaxSide.ForeColor = System.Drawing.Color.Transparent;
            this.buttonMaxSide.Location = new System.Drawing.Point(932, 2);
            this.buttonMaxSide.Margin = new System.Windows.Forms.Padding(5, 0, 0, 5);
            this.buttonMaxSide.Name = "buttonMaxSide";
            this.buttonMaxSide.Size = new System.Drawing.Size(40, 36);
            this.buttonMaxSide.TabIndex = 6;
            this.buttonMaxSide.Text = "口";
            this.buttonMaxSide.UseVisualStyleBackColor = false;
            this.buttonMaxSide.Click += new System.EventHandler(this.buttonMaxSide_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(71)))), ((int)(((byte)(160)))));
            this.buttonClose.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold);
            this.buttonClose.ForeColor = System.Drawing.Color.Transparent;
            this.buttonClose.Location = new System.Drawing.Point(976, 2);
            this.buttonClose.Margin = new System.Windows.Forms.Padding(4);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(40, 36);
            this.buttonClose.TabIndex = 4;
            this.buttonClose.Text = "X";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button2.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button2.Location = new System.Drawing.Point(559, 2);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 36);
            this.button2.TabIndex = 1;
            this.button2.Text = "下一个文件";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button1.Font = new System.Drawing.Font("微软雅黑", 13F);
            this.button1.Location = new System.Drawing.Point(322, 2);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "上一个文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_右
            // 
            this.button_右.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_右.Location = new System.Drawing.Point(240, 1);
            this.button_右.Margin = new System.Windows.Forms.Padding(4);
            this.button_右.Name = "button_右";
            this.button_右.Size = new System.Drawing.Size(55, 25);
            this.button_右.TabIndex = 10;
            this.button_右.Text = "右转";
            this.button_右.UseVisualStyleBackColor = true;
            this.button_右.Click += new System.EventHandler(this.button_右_Click);
            // 
            // button_左
            // 
            this.button_左.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_左.Location = new System.Drawing.Point(178, 1);
            this.button_左.Margin = new System.Windows.Forms.Padding(4);
            this.button_左.Name = "button_左";
            this.button_左.Size = new System.Drawing.Size(55, 25);
            this.button_左.TabIndex = 9;
            this.button_左.Text = "左转";
            this.button_左.UseVisualStyleBackColor = true;
            this.button_左.Click += new System.EventHandler(this.button_左_Click);
            // 
            // pdfViewer2
            // 
            this.pdfViewer2.AutoScroll = true;
            this.pdfViewer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pdfViewer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer2.Location = new System.Drawing.Point(2, 67);
            this.pdfViewer2.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.pdfViewer2.Name = "pdfViewer2";
            this.pdfViewer2.Size = new System.Drawing.Size(1020, 891);
            this.pdfViewer2.TabIndex = 2;
            // 
            // button_Height
            // 
            this.button_Height.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Height.Location = new System.Drawing.Point(90, 1);
            this.button_Height.Margin = new System.Windows.Forms.Padding(4);
            this.button_Height.Name = "button_Height";
            this.button_Height.Size = new System.Drawing.Size(80, 25);
            this.button_Height.TabIndex = 12;
            this.button_Height.Text = "适应高度";
            this.button_Height.UseVisualStyleBackColor = true;
            this.button_Height.Click += new System.EventHandler(this.button_Height_Click);
            // 
            // button_Width
            // 
            this.button_Width.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button_Width.Location = new System.Drawing.Point(3, 1);
            this.button_Width.Margin = new System.Windows.Forms.Padding(4);
            this.button_Width.Name = "button_Width";
            this.button_Width.Size = new System.Drawing.Size(80, 25);
            this.button_Width.TabIndex = 11;
            this.button_Width.Text = "适应宽度";
            this.button_Width.UseVisualStyleBackColor = true;
            this.button_Width.Click += new System.EventHandler(this.button_Width_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button_Height);
            this.panel2.Controls.Add(this.button_右);
            this.panel2.Controls.Add(this.button_Width);
            this.panel2.Controls.Add(this.button_左);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1020, 27);
            this.panel2.TabIndex = 3;
            // 
            // FrmPreviewArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 960);
            this.Controls.Add(this.pdfViewer2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.MinimumSize = new System.Drawing.Size(1024, 960);
            this.Name = "FrmPreviewArea";
            this.Text = "预览文件";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPreviewArea_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonMinSide;
        private System.Windows.Forms.Button buttonMaxSide;
        private System.Windows.Forms.Label label文件名;
        private PdfiumViewer.PdfViewer pdfViewer2;
        private System.Windows.Forms.Button button_左;
        private System.Windows.Forms.Button button_右;
        private System.Windows.Forms.Button button_Height;
        private System.Windows.Forms.Button button_Width;
        private System.Windows.Forms.Panel panel2;
    }
}