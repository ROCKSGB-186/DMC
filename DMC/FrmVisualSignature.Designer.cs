namespace DMC
{
    partial class FrmVisualSignature
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
            this.pdfViewer2 = new PdfiumViewer.PdfViewer();
            this.pictureBox_signature = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_signature)).BeginInit();
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
            // pdfViewer2
            // 
            this.pdfViewer2.AutoScroll = true;
            this.pdfViewer2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pdfViewer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdfViewer2.Location = new System.Drawing.Point(2, 74);
            this.pdfViewer2.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.pdfViewer2.Name = "pdfViewer2";
            this.pdfViewer2.ShowBookmarks = false;
            this.pdfViewer2.Size = new System.Drawing.Size(1020, 884);
            this.pdfViewer2.TabIndex = 2;
            // 
            // pictureBox_signature
            // 
            this.pictureBox_signature.Location = new System.Drawing.Point(439, 126);
            this.pictureBox_signature.Name = "pictureBox_signature";
            this.pictureBox_signature.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_signature.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox_signature.TabIndex = 3;
            this.pictureBox_signature.TabStop = false;
            this.pictureBox_signature.Visible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.comboBox1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(2, 40);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1020, 34);
            this.panel2.TabIndex = 4;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(82, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(500, 29);
            this.comboBox1.TabIndex = 55;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(16, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 21);
            this.label2.TabIndex = 54;
            this.label2.Text = "选择章：";
            // 
            // FrmVisualSignature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 28F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 960);
            this.Controls.Add(this.pictureBox_signature);
            this.Controls.Add(this.pdfViewer2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(10, 12, 10, 12);
            this.MinimumSize = new System.Drawing.Size(1024, 960);
            this.Name = "FrmVisualSignature";
            this.Text = "预览文件";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPreviewArea_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_signature)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonMinSide;
        private System.Windows.Forms.Button buttonMaxSide;
        private System.Windows.Forms.Label label文件名;
        private PdfiumViewer.PdfViewer pdfViewer2;
        private System.Windows.Forms.PictureBox pictureBox_signature;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
    }
}