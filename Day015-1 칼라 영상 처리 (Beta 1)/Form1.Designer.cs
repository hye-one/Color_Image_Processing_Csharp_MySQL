
namespace Day015_1_칼라_영상_처리__Beta_1_
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.파일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.열기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.저장ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.종료ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.화소점ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.동일이미지ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.밝게어둡게ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.흑백ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.그레이스케일ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.히스토그램그리기ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.채도변경ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openCVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.화소점처리ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.기하학변환ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.화소영역처리ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.머신러닝ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.그레이스케일ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.밝기조절ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.파일ToolStripMenuItem,
            this.화소점ToolStripMenuItem,
            this.openCVToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(789, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 파일ToolStripMenuItem
            // 
            this.파일ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.열기ToolStripMenuItem,
            this.저장ToolStripMenuItem,
            this.종료ToolStripMenuItem});
            this.파일ToolStripMenuItem.Name = "파일ToolStripMenuItem";
            this.파일ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.파일ToolStripMenuItem.Text = "파일";
            // 
            // 열기ToolStripMenuItem
            // 
            this.열기ToolStripMenuItem.Name = "열기ToolStripMenuItem";
            this.열기ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.열기ToolStripMenuItem.Text = "열기";
            this.열기ToolStripMenuItem.Click += new System.EventHandler(this.열기ToolStripMenuItem_Click);
            // 
            // 저장ToolStripMenuItem
            // 
            this.저장ToolStripMenuItem.Name = "저장ToolStripMenuItem";
            this.저장ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.저장ToolStripMenuItem.Text = "저장";
            // 
            // 종료ToolStripMenuItem
            // 
            this.종료ToolStripMenuItem.Name = "종료ToolStripMenuItem";
            this.종료ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.종료ToolStripMenuItem.Text = "종료";
            // 
            // 화소점ToolStripMenuItem
            // 
            this.화소점ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.동일이미지ToolStripMenuItem,
            this.밝게어둡게ToolStripMenuItem,
            this.흑백ToolStripMenuItem,
            this.그레이스케일ToolStripMenuItem,
            this.히스토그램그리기ToolStripMenuItem,
            this.채도변경ToolStripMenuItem});
            this.화소점ToolStripMenuItem.Name = "화소점ToolStripMenuItem";
            this.화소점ToolStripMenuItem.Size = new System.Drawing.Size(68, 24);
            this.화소점ToolStripMenuItem.Text = "화소점";
            // 
            // 동일이미지ToolStripMenuItem
            // 
            this.동일이미지ToolStripMenuItem.Name = "동일이미지ToolStripMenuItem";
            this.동일이미지ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.동일이미지ToolStripMenuItem.Text = "동일 이미지";
            // 
            // 밝게어둡게ToolStripMenuItem
            // 
            this.밝게어둡게ToolStripMenuItem.Name = "밝게어둡게ToolStripMenuItem";
            this.밝게어둡게ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.밝게어둡게ToolStripMenuItem.Text = "밝게/어둡게";
            this.밝게어둡게ToolStripMenuItem.Click += new System.EventHandler(this.밝게어둡게ToolStripMenuItem_Click);
            // 
            // 흑백ToolStripMenuItem
            // 
            this.흑백ToolStripMenuItem.Name = "흑백ToolStripMenuItem";
            this.흑백ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.흑백ToolStripMenuItem.Text = "좌우 미러링";
            this.흑백ToolStripMenuItem.Click += new System.EventHandler(this.흑백ToolStripMenuItem_Click);
            // 
            // 그레이스케일ToolStripMenuItem
            // 
            this.그레이스케일ToolStripMenuItem.Name = "그레이스케일ToolStripMenuItem";
            this.그레이스케일ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.그레이스케일ToolStripMenuItem.Text = "그레이 스케일";
            this.그레이스케일ToolStripMenuItem.Click += new System.EventHandler(this.그레이스케일ToolStripMenuItem_Click);
            // 
            // 히스토그램그리기ToolStripMenuItem
            // 
            this.히스토그램그리기ToolStripMenuItem.Name = "히스토그램그리기ToolStripMenuItem";
            this.히스토그램그리기ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.히스토그램그리기ToolStripMenuItem.Text = "히스토그램 그리기";
            this.히스토그램그리기ToolStripMenuItem.Click += new System.EventHandler(this.히스토그램그리기ToolStripMenuItem_Click);
            // 
            // 채도변경ToolStripMenuItem
            // 
            this.채도변경ToolStripMenuItem.Name = "채도변경ToolStripMenuItem";
            this.채도변경ToolStripMenuItem.Size = new System.Drawing.Size(217, 26);
            this.채도변경ToolStripMenuItem.Text = "채도 변경";
            this.채도변경ToolStripMenuItem.Click += new System.EventHandler(this.채도변경ToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 493);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(789, 26);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(152, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(0, 30);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(114, 62);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openCVToolStripMenuItem
            // 
            this.openCVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.화소점처리ToolStripMenuItem,
            this.기하학변환ToolStripMenuItem,
            this.화소영역처리ToolStripMenuItem,
            this.머신러닝ToolStripMenuItem});
            this.openCVToolStripMenuItem.Name = "openCVToolStripMenuItem";
            this.openCVToolStripMenuItem.Size = new System.Drawing.Size(81, 24);
            this.openCVToolStripMenuItem.Text = "OpenCV";
            // 
            // 화소점처리ToolStripMenuItem
            // 
            this.화소점처리ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.그레이스케일ToolStripMenuItem1,
            this.밝기조절ToolStripMenuItem});
            this.화소점처리ToolStripMenuItem.Name = "화소점처리ToolStripMenuItem";
            this.화소점처리ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.화소점처리ToolStripMenuItem.Text = "화소점처리";
            this.화소점처리ToolStripMenuItem.Click += new System.EventHandler(this.화소점처리ToolStripMenuItem_Click);
            // 
            // 기하학변환ToolStripMenuItem
            // 
            this.기하학변환ToolStripMenuItem.Name = "기하학변환ToolStripMenuItem";
            this.기하학변환ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.기하학변환ToolStripMenuItem.Text = "기하학 변환";
            // 
            // 화소영역처리ToolStripMenuItem
            // 
            this.화소영역처리ToolStripMenuItem.Name = "화소영역처리ToolStripMenuItem";
            this.화소영역처리ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.화소영역처리ToolStripMenuItem.Text = "화소 영역 처리";
            // 
            // 머신러닝ToolStripMenuItem
            // 
            this.머신러닝ToolStripMenuItem.Name = "머신러닝ToolStripMenuItem";
            this.머신러닝ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.머신러닝ToolStripMenuItem.Text = "머신 러닝";
            // 
            // 그레이스케일ToolStripMenuItem1
            // 
            this.그레이스케일ToolStripMenuItem1.Name = "그레이스케일ToolStripMenuItem1";
            this.그레이스케일ToolStripMenuItem1.Size = new System.Drawing.Size(224, 26);
            this.그레이스케일ToolStripMenuItem1.Text = "그레이스케일";
            this.그레이스케일ToolStripMenuItem1.Click += new System.EventHandler(this.그레이스케일ToolStripMenuItem1_Click);
            // 
            // 밝기조절ToolStripMenuItem
            // 
            this.밝기조절ToolStripMenuItem.Name = "밝기조절ToolStripMenuItem";
            this.밝기조절ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.밝기조절ToolStripMenuItem.Text = "밝기 조절";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 519);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 파일ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 열기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 저장ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 종료ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 화소점ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 동일이미지ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 밝게어둡게ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 그레이스케일ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripMenuItem 히스토그램그리기ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 흑백ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 채도변경ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openCVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 화소점처리ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 기하학변환ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 화소영역처리ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 머신러닝ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 그레이스케일ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 밝기조절ToolStripMenuItem;
    }
}

