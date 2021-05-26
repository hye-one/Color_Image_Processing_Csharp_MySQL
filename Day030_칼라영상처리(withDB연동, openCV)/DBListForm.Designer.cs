
namespace Day015_01_컬러영상처리_Beta1_
{
    partial class DBListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBListForm));
            this.btn_image_putIn = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tb_fullfileName = new System.Windows.Forms.TextBox();
            this.btn_fileOpen = new System.Windows.Forms.Button();
            this.btn_upload = new System.Windows.Forms.Button();
            this.btn_openDB = new System.Windows.Forms.Button();
            this.DBListComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_image_putIn
            // 
            this.btn_image_putIn.Location = new System.Drawing.Point(559, 371);
            this.btn_image_putIn.Name = "btn_image_putIn";
            this.btn_image_putIn.Size = new System.Drawing.Size(164, 37);
            this.btn_image_putIn.TabIndex = 13;
            this.btn_image_putIn.Text = "이미지 불러오기";
            this.btn_image_putIn.UseVisualStyleBackColor = true;
            this.btn_image_putIn.Click += new System.EventHandler(this.btn_image_putIn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.pictureBox1.Location = new System.Drawing.Point(580, 225);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // tb_fullfileName
            // 
            this.tb_fullfileName.Location = new System.Drawing.Point(60, 51);
            this.tb_fullfileName.Name = "tb_fullfileName";
            this.tb_fullfileName.Size = new System.Drawing.Size(592, 25);
            this.tb_fullfileName.TabIndex = 11;
            // 
            // btn_fileOpen
            // 
            this.btn_fileOpen.Location = new System.Drawing.Point(658, 51);
            this.btn_fileOpen.Name = "btn_fileOpen";
            this.btn_fileOpen.Size = new System.Drawing.Size(113, 25);
            this.btn_fileOpen.TabIndex = 10;
            this.btn_fileOpen.Text = "파일 선택...";
            this.btn_fileOpen.UseVisualStyleBackColor = true;
            this.btn_fileOpen.Click += new System.EventHandler(this.btn_fileOpen_Click);
            // 
            // btn_upload
            // 
            this.btn_upload.Location = new System.Drawing.Point(333, 82);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(161, 28);
            this.btn_upload.TabIndex = 9;
            this.btn_upload.Text = "파일 업로드";
            this.btn_upload.UseVisualStyleBackColor = true;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // btn_openDB
            // 
            this.btn_openDB.Location = new System.Drawing.Point(30, 144);
            this.btn_openDB.Name = "btn_openDB";
            this.btn_openDB.Size = new System.Drawing.Size(366, 33);
            this.btn_openDB.TabIndex = 8;
            this.btn_openDB.Text = "DB 이미지 목록 추출";
            this.btn_openDB.UseVisualStyleBackColor = true;
            this.btn_openDB.Click += new System.EventHandler(this.btn_openDB_Click);
            // 
            // DBListComboBox
            // 
            this.DBListComboBox.FormattingEnabled = true;
            this.DBListComboBox.Location = new System.Drawing.Point(30, 183);
            this.DBListComboBox.Name = "DBListComboBox";
            this.DBListComboBox.Size = new System.Drawing.Size(428, 23);
            this.DBListComboBox.TabIndex = 7;
            this.DBListComboBox.SelectedIndexChanged += new System.EventHandler(this.DBListComboBox_SelectedIndexChanged_1);
            // 
            // DBListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_image_putIn);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tb_fullfileName);
            this.Controls.Add(this.btn_fileOpen);
            this.Controls.Add(this.btn_upload);
            this.Controls.Add(this.btn_openDB);
            this.Controls.Add(this.DBListComboBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DBListForm";
            this.Text = "DB이미지 불러오기";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DBListForm_FormClosed);
            this.Load += new System.EventHandler(this.DBListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_image_putIn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox tb_fullfileName;
        private System.Windows.Forms.Button btn_fileOpen;
        private System.Windows.Forms.Button btn_upload;
        private System.Windows.Forms.Button btn_openDB;
        private System.Windows.Forms.ComboBox DBListComboBox;
    }
}