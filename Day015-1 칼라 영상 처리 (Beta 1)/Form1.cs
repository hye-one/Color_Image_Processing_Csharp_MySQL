using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenCvSharp;
namespace Day015_1_칼라_영상_처리__Beta_1_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /// 전역변수부
        byte[,,] inImage = null, outImage = null;
        int inH, inW, outH, outW;
        string fileName;
        Bitmap paper, bitmap;
        Mat inCvImage, outCvImage;


        const int RGB = 3, RR = 0, GG = 1, BB = 2;
        /// 임시 파일용 배열과 임시 파일 개수
        string[] tmpFiles = new string[500]; //  최대 500개
        int tmpIndex = 0;


        /// 메뉴 이벤트 처리부
        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openImage_CV();
        }
        private void 밝게어둡게ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            add_image();
        }
        private void 그레이스케일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grayscale();
        }

        private void 흑백ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mirror_image();
        }

        private void 히스토그램그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw_histogram();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                switch (e.KeyCode)
                {
                    case Keys.Z:
                        restoreTempFile(); break;
                }
            }
        }

        private void 채도변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            change_satur();
        }
        
        private void 그레이스케일ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            grayScale_CV();
        }
        /// 공통 함수부        
        void openImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();  // 객체 생성
            ofd.DefaultExt = "";
            ofd.Filter = "칼라 필터 | *.png; *.jpg; *.bmp; *.tif";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            fileName = ofd.FileName;
            // 파일 --> 비트맵(bitmap)
            bitmap = new Bitmap(fileName);
            // 중요! 입력이미지의 높이, 폭 알아내기
            inW = bitmap.Height;
            inH = bitmap.Width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당
            // 비트맵(bitmap) --> 메모리 (로딩)
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    Color c = bitmap.GetPixel(i, k);
                    inImage[RR, i, k] = c.R;
                    inImage[GG, i, k] = c.G;
                    inImage[BB, i, k] = c.B;
                }
            equal_image();
        }



        void displayImage()
        {
            // 벽, 게시판, 종이 크기 조절
            paper = new Bitmap(outH, outW); // 종이
            pictureBox1.Size = new System.Drawing.Size(outH, outW); // 캔버스
            this.Size = new System.Drawing.Size(outH + 20, outW + 80); // 벽

            Color pen; // 펜(콕콕 찍을 용도)
            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    byte r = outImage[RR,i, k]; // 잉크(색상값)
                    byte g = outImage[GG, i, k]; // 잉크(색상값)
                    byte b = outImage[BB, i, k]; // 잉크(색상값)
                    pen = Color.FromArgb(r, g, b); // 펜에 잉크 묻히기
                    paper.SetPixel(i,k, pen); // 종이에 콕 찍기
                }
            pictureBox1.Image = paper; // 게시판에 종이를 붙이기.
            toolStripStatusLabel1.Text =
                outH.ToString() + "x" + outW.ToString() + "  " + fileName;


        }
        void saveTempFile()
        {
            //////////////////////////////////////////
            // 영상처리 효과가 계속 누적되도록 함.
            //////////////////////////////////////////
            /// (1) 입력영상을 디스크에 저장 
            string saveFname = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".tmp";
            Bitmap image = new Bitmap(inH, inW); // 빈 비트맵(종이) 준비
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    Color c;
                    int r, g, b;
                    r = inImage[0, i, k];
                    g = inImage[1, i, k];
                    b = inImage[2, i, k];
                    c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);  // 종이에 콕콕 찍기
                }
            image.Save(saveFname, System.Drawing.Imaging.ImageFormat.Png);
            tmpFiles[tmpIndex++] = saveFname;
            /// (2) 출력영상 --> 입력영상
            inH = outH; inW = outW;
            inImage = new byte[RGB, inH, inW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                        inImage[rgb, i, k] = outImage[rgb, i, k];
        }

        private void 화소점처리ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

       

        void restoreTempFile()
        {
            if (tmpIndex <= 0)
                return;
            fileName = tmpFiles[--tmpIndex];
            // 파일 --> 비트맵(bitmap)
            bitmap = new Bitmap(fileName);
            // 중요! 입력이미지의 높이, 폭 알아내기
            inW = bitmap.Height;
            inH = bitmap.Width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당
            // 비트맵(bitmap) --> 메모리 (로딩)
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    Color c = bitmap.GetPixel(i, k);
                    inImage[RR, i, k] = c.R;
                    inImage[GG, i, k] = c.G;
                    inImage[BB, i, k] = c.B;
                }
            equal_image();
            // System.IO.File.Delete(fileName); // 임시파일 삭제 
        }

        /// 영상처리 함수부
        void equal_image()
        {
            if (inImage == null)
                return;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++) 
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = inImage[rgb, i, k];
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void add_image()
        {
            if (inImage == null)
                return;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if (inImage[rgb, i, k] + 100 > 255)
                            outImage[rgb, i, k] = 255;
                        else 
                            outImage[rgb, i, k] = (byte)(inImage[rgb, i, k] + 50);
                    }
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }

        void grayscale()
        {
            if (inImage == null)
                return;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    int hap = inImage[RR, i, k] + inImage[GG, i, k]+ inImage[BB, i, k];
                    byte rgb = (byte) (hap / 3.0);
                    outImage[RR, i, k] = rgb;
                    outImage[GG, i, k] = rgb;
                    outImage[BB, i, k] = rgb;
                }
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }

        void mirror_image()
        {
            if (inImage == null)
                return;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, inH-i-1, k] = inImage[rgb, i, k];
                    }
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }

        void draw_histogram()
        {
            long[] rHisto = new long[256];
            long[] gHisto = new long[256];
            long[] bHisto = new long[256];

            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    rHisto[outImage[RR, i, k]]++;
                    gHisto[outImage[GG, i, k]]++;
                    bHisto[outImage[BB, i, k]]++;
                }

            HistoForm hform = new HistoForm(rHisto, gHisto, bHisto);
            hform.ShowDialog();

        }

        void change_satur()
        {
            if (inImage == null)
                return;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            Color c; // 한점 색상 모델
            double hh, ss, vv; // 색상, 채도, 밝기
            int rr, gg, bb; // 레드, 그린 ,블루

            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    rr = inImage[RR, i, k];
                    gg = inImage[GG, i, k];
                    bb = inImage[BB, i, k];
                    // RGB --> HSV(HSB)
                    c = Color.FromArgb(rr, gg, bb);
                    hh = c.GetHue();
                    ss = c.GetSaturation();
                    vv = c.GetBrightness();

                    // (핵심!) 채도 올리기
                    ss -= 0.2;

                    // HSV --> RGB
                    HsvToRgb(hh, ss, vv, out rr, out gg, out bb);

                    outImage[RR, i, k] = (byte)rr;
                    outImage[GG, i, k] = (byte)gg;
                    outImage[BB, i, k] = (byte)bb;
                }
            /////////////////////////////////////////////
            displayImage();
        }

        void HsvToRgb(double h, double S, double V, out int r, out int g, out int b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {
                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;
                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;
                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;
                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;
                    default:
                        R = G = B = V;
                        break;
                }
            }
            r = CheckRange((int)(R * 255.0));
            g = CheckRange((int)(G * 255.0));
            b = CheckRange((int)(B * 255.0));

            int CheckRange(int i)
            {
                if (i < 0) return 0;
                if (i > 255) return 255;
                return i;
            }
        }
        // /////Open CV 전용함수//////////

        void openImage_CV()
        {
            OpenFileDialog ofd = new OpenFileDialog();  // 객체 생성
            ofd.DefaultExt = "";
            ofd.Filter = "칼라 필터 | *.png; *.jpg; *.bmp; *.tif";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            fileName = ofd.FileName;
            // 파일을 openCV용 Matrix로 읽어오기
            inCvImage = Cv2.ImRead(fileName);
            Cv2.Transpose(inCvImage, inCvImage);
            // 중요! 입력이미지의 높이, 폭 알아내기
            inH = inCvImage.Height;
            inW = inCvImage.Width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당
            // OpenCV이미지Matrix --> 메모리 (로딩)
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    var c = inCvImage.At<Vec3b>(i, k);
                    inImage[RR, i, k] = c.Item2;
                    inImage[GG, i, k] = c.Item1;
                    inImage[BB, i, k] = c.Item0;
                }
            equal_image();
        }
        void CV2ToOutImage()
        {
            //
            outH = outCvImage.Height;
            outW = outCvImage.Width;
            outImage = new byte[RGB, outH, outW];
            // OpenCV이미지Matrix --> 메모리 (로딩)
            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    var c = outCvImage.At<Vec3b>(i, k);
                    outImage[RR, i, k] = c.Item2;
                    outImage[GG, i, k] = c.Item1;
                    outImage[BB, i, k] = c.Item0;
                }
            displayImage();
        }
        void grayScale_CV()
        {
            //출력 이미지 크기 결정
            //int oH, oW; //outCV이미지 크기
            //oH = inCvImage.Height;
            //oW = inCvImage.Width;
            //outCvImage = Mat.Ones(new OpenCvSharp.Size(oW, oH), MatType.CV_8UC1);
            outCvImage = new Mat();

            //진짜 OpenCV용 알고리즘
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);

            CV2ToOutImage();
        }
    }
}
