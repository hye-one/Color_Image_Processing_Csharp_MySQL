
namespace Day015_01_컬러영상처리_Beta1_
{
    partial class DBFileForm
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
            this.Download = new System.Windows.Forms.Button();
            this.btn_upload = new System.Windows.Forms.Button();
            this.tb_fullfileName = new System.Windows.Forms.TextBox();
            this.btn_fileOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.DBListComboBox = new System.Windows.Forms.ComboBox();
            this.btn_openDB = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btn_image_putIn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Download
            // 
            this.Download.Location = new System.Drawing.Point(446, 299);
            this.Download.Name = "Download";
            this.Download.Size = new System.Drawing.Size(139, 43);
            this.Download.TabIndex = 8;
            this.Download.Text = "다운로드";
            this.Download.UseVisualStyleBackColor = true;
            this.Download.Click += new System.EventHandler(this.Download_Click);
            // 
            // btn_upload
            // 
            this.btn_upload.Location = new System.Drawing.Point(345, 122);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(122, 26);
            this.btn_upload.TabIndex = 7;
            this.btn_upload.Text = "업로드";
            this.btn_upload.UseVisualStyleBackColor = true;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // tb_fullfileName
            // 
            this.tb_fullfileName.Location = new System.Drawing.Point(34, 74);
            this.tb_fullfileName.Name = "tb_fullfileName";
            this.tb_fullfileName.Size = new System.Drawing.Size(622, 25);
            this.tb_fullfileName.TabIndex = 6;
            // 
            // btn_fileOpen
            // 
            this.btn_fileOpen.Location = new System.Drawing.Point(662, 74);
            this.btn_fileOpen.Name = "btn_fileOpen";
            this.btn_fileOpen.Size = new System.Drawing.Size(104, 25);
            this.btn_fileOpen.TabIndex = 5;
            this.btn_fileOpen.Text = "파일선택...";
            this.btn_fileOpen.UseVisualStyleBackColor = true;
            this.btn_fileOpen.Click += new System.EventHandler(this.btn_fileOpen_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // DBListComboBox
            // 
            this.DBListComboBox.FormattingEnabled = true;
            this.DBListComboBox.Location = new System.Drawing.Point(12, 299);
            this.DBListComboBox.Name = "DBListComboBox";
            this.DBListComboBox.Size = new System.Drawing.Size(428, 23);
            this.DBListComboBox.TabIndex = 10;
            this.DBListComboBox.SelectedIndexChanged += new System.EventHandler(this.DBListComboBox_SelectedIndexChanged);
            // 
            // btn_openDB
            // 
            this.btn_openDB.Location = new System.Drawing.Point(12, 260);
            this.btn_openDB.Name = "btn_openDB";
            this.btn_openDB.Size = new System.Drawing.Size(366, 33);
            this.btn_openDB.TabIndex = 11;
            this.btn_openDB.Text = "DB 이미지 목록 추출";
            this.btn_openDB.UseVisualStyleBackColor = true;
            this.btn_openDB.Click += new System.EventHandler(this.btn_openDB_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pictureBox1.Location = new System.Drawing.Point(604, 223);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(138, 119);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // btn_image_putIn
            // 
            this.btn_image_putIn.Location = new System.Drawing.Point(588, 390);
            this.btn_image_putIn.Name = "btn_image_putIn";
            this.btn_image_putIn.Size = new System.Drawing.Size(164, 37);
            this.btn_image_putIn.TabIndex = 14;
            this.btn_image_putIn.Text = "이미지 불러오기";
            this.btn_image_putIn.UseVisualStyleBackColor = true;
            this.btn_image_putIn.Click += new System.EventHandler(this.btn_image_putIn_Click);
            // 
            // DBFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_image_putIn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_openDB);
            this.Controls.Add(this.DBListComboBox);
            this.Controls.Add(this.Download);
            this.Controls.Add(this.btn_upload);
            this.Controls.Add(this.tb_fullfileName);
            this.Controls.Add(this.btn_fileOpen);
            this.Name = "DBFileForm";
            this.Text = "DBFileForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DBFileForm_FormClosed);
            this.Load += new System.EventHandler(this.DBFileForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button Download;
        private System.Windows.Forms.Button btn_upload;
        private System.Windows.Forms.TextBox tb_fullfileName;
        private System.Windows.Forms.Button btn_fileOpen;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox DBListComboBox;
        private System.Windows.Forms.Button btn_openDB;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btn_image_putIn;
    }
}