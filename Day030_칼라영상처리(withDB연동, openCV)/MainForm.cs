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
using System.Drawing.Imaging;
using MySql.Data.MySqlClient;
using OpenCvSharp;

namespace Day015_01_컬러영상처리_Beta1_
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        //전역변수 선언부
        byte[,,] inImage = null;
        byte[,,] outImage = null;
        int inH, inW, outH, outW;
        string fileName;
        const int RGB = 3, RR = 0, GG = 1, BB = 2; //숫자보다는 기호로 쓰는 게 좋다
        static bool mouseYN = false;  //픽쳐박스 마우스 클릭 Yes or No
        int sx, sy, ex, ey;         //픽쳐박스 마우스 클릭 시작 좌표와 끝 좌표

        Mat inCvImage, outCvImage;

        // /////////////
        // DB 전역변수
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=blob_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        MySqlDataReader reader; // 트럭이 가져올 끈
        static string DBimageID;
        static int i_id, i_width, i_height;
        static string i_fname, i_extname;
        static long i_fsize;
        // /////////////
        // 조작 함수부
        /// 임시 파일용 배열과 임시 파일 개수
        string[] tmpFiles = new string[500]; // 최대 500개
        int tmpIndex = 0;
        void saveTempFile()
        {
            //////////////////////////////////////////
            // 영상처리 효과가 계속 누적되도록 함.
            //////////////////////////////////////////
            // (1) 입력영상을 디스크에 저장 
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
            // (2) 출력영상 --> 입력영상
            inH = outH; inW = outW;
            inImage = new byte[RGB, inH, inW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                        inImage[rgb, i, k] = outImage[rgb, i, k];
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
        // ////////////
        //공통 함수부
        void openImage()
        {
            OpenFileDialog ofd = new OpenFileDialog();  // 객체 생성
            ofd.DefaultExt = "";
            ofd.Filter = "칼라 필터 | *.png; *.jpg; *.bmp; *.tif"; ;
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            fileName = ofd.FileName;
            //파일-->비트맵
            bitmap = new Bitmap(fileName); //bitmap 라이브러리 불러옴

            // 중요! 입력이미지의 높이, 폭 알아내기-비트맵에서 불러옴(H,W바꿔야 함)
            inW = bitmap.Height;
            inH = bitmap.Width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당...면-행-렬
                                               //비트맵-->메모리()
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    Color c = bitmap.GetPixel(i, k);//컬러 변수 c에 비트맵 함수로 픽셀의 값을 불러온다.
                    inImage[RR, i, k] = c.R;        //컬러 변수 c의 Red값을 불러와, RR번째 면에 넣는다.
                    inImage[GG, i, k] = c.G;
                    inImage[BB, i, k] = c.B;
                }

            equal_image();
        }
        void saveImage()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "";
            sfd.Filter = "PNG File(*.png) | *.png";
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            String saveFname = sfd.FileName;
            Bitmap image = new Bitmap(outH, outW); // 빈 비트맵(종이) 준비
            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    Color c;
                    int r, g, b;
                    r = outImage[0, i, k];
                    g = outImage[1, i, k];
                    b = outImage[2, i, k];
                    c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);  // 종이에 콕콕 찍기
                }
            // 상단에 using System.Drawing.Imaging; 추가해야 함
            image.Save(saveFname, ImageFormat.Png); // 종이를 PNG로 저장
            toolStripStatusLabel1.Text = saveFname + "으로 저장됨.";
        }
        void displayImage()
        {
            double MAXSIZE = 800; // 조절 가능
            int oW = outW, oH = outH;
            double hop = 1.0;
            //원본 영상 크기 outW*outH, 디스플레이 영상 크기 oW*oH,
            if (oH > MAXSIZE || oW > MAXSIZE)
            {
                if (outW > outH)
                    hop = (outW / MAXSIZE);
                else
                    hop = (outH / MAXSIZE);

                oW = (int)(outW / hop); oH = (int)(outH / hop);
            }
            // 종이, 게시판, 벽 크기 조절
            paper = new Bitmap(oH, oW);
            pictureBox1.Size = new System.Drawing.Size(oH, oW);
            this.Size = new System.Drawing.Size(outH + 230, outW + 100); // 벽

            Color pen; // 펜 (콕콕 찍을 펜)
            for (int i = 0; i < oH; i++)
                for (int k = 0; k < oW; k++)
                {
                    if (i >= oH - 1 || k >= oW - 1)
                        continue;
                    byte dataR = outImage[0, (int)(i * hop), (int)(k * hop)];  // 색깔 (잉크)
                    byte dataG = outImage[1, (int)(i * hop), (int)(k * hop)];  // 색깔 (잉크)
                    byte dataB = outImage[2, (int)(i * hop), (int)(k * hop)];  // 색깔 (잉크)
                    pen = Color.FromArgb(dataR, dataG, dataB); // 펜에 잉크 묻힘
                    paper.SetPixel(i, k, pen); // 종이에 콕 찍음.
                }
            pictureBox1.Image = paper; // 게시판에 종이 걸기
            toolStripStatusLabel1.Text = "영상크기 : " + outH + " x " + outW;

            toolStripStatusLabel1.Text =
                outH.ToString() + "x" + outW.ToString() + "  " + fileName;

        }
        double getValue()
        {
            subForm01 sub = new subForm01();
            if (sub.ShowDialog() == DialogResult.Cancel)
                return 0;
            double value = (double)(sub.numUp1.Value); //서로 두 폼을 연결할 땐 Public으로 바꿔주기. 
            return value;

            
        }
        Tuple<byte, byte> Getdistance()
        {
            subForm02 sub = new subForm02(); //서브폼 준비
            if (sub.ShowDialog() == DialogResult.Cancel)
                return new Tuple<byte, byte>(0, 0);
            byte moveH = (byte)(sub.moveH_num.Value);
            byte moveW = (byte)(sub.moveW_num.Value);
            return new Tuple<byte, byte>(moveH, moveW);
        }

        // ///////////////
        // DB 함수 정의부
        void DBimageOpen(string i_id, int i_width, int i_height)
        {
            // 파일 이름에 해당하는 이미지를 pixel 테이블에서 가져 온다.
            sql = "SELECT RGB, p_row, p_col, p_value FROM color_pixel WHERE i_id =" + i_id; // 짐 싸기
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.

            // DB이미지를 배열로 바꿔주기
            inH = i_height;
            inW = i_width;
            inImage = new byte[RGB, inH, inW]; // 메모리 할당
            inImage = new byte[RGB, inH, inW]; // 메모리 할당dd
            //
            Color c;
            int r, g, b; byte rgb;
            int row, col;
            byte pixel;
            while (reader.Read())
            {
                    rgb = byte.Parse(reader["RGB"].ToString());
                    row = int.Parse(reader["p_row"].ToString());
                    col = int.Parse(reader["p_col"].ToString());
                    pixel = byte.Parse(reader["p_value"].ToString());
                    inImage[rgb, row, col] = pixel;
            }
            reader.Close();

            equal_image();
        }
        void DBimageSave()
        {
            //inImage의 파일 정보 가져오기
            //저장 시킬 속성(컬럼) 정의하고 값 지정해주기
            string i_saveDBname, i_user;
            i_saveDBname = i_fname + "(rev)";
            i_user = "Hong";
            Random rnd = new Random();
            i_id = rnd.Next(int.MinValue, int.MaxValue);
            i_fsize = outH * outW;
            //INSERT INTO image (i_id, i_fname, i_extname, i_fsize, i_width, i_height, i_user)
            //VALUES(i_id, 'f_name', 'i_extname', i_fsize, i_width, i_height, i_user);
            //INSERT INTO pixel(i_id, p_row, p_col, p_value) VALUES(i_id, p_row, p_col, p_value);
            sql = "INSERT INTO image(i_id, i_fname, i_extname,i_fsize,i_width ,i_height ,i_user) VALUES (";
            sql += i_id + ",'" + i_saveDBname + "','" + i_extname + "'," + i_fsize + "," + outW + "," + outW + ",'" + i_user + "');";
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            cmd.ExecuteNonQuery();

            // pixel 테이블에 수정된 정보 넣기
            int p_row, p_col, p_value;
            for(int rgb=0; rgb<RGB;rgb++)
            for (int i = 0; i < outW; i++)
                for (int k = 0; k < outH; k++)
                {
                    p_row = i; p_col = k;
                    p_value = outImage[rgb, i, k];
                    sql = "INSERT INTO color_pixel(i_id, RGB, p_row, p_col, p_value) VALUES(";
                    sql += i_id + "," + rgb + "," + p_row + "," + p_col + "," + p_value + ")";
                    cmd.CommandText = sql;  // 짐을 트럭에 싣기
                    cmd.ExecuteNonQuery();
                }
            MessageBox.Show("성공적으로 저장되었습니다.");
        }
        void DBfileSave()
        {
            //f_name = "C:\\images\\Pet_PNG(squre)\\Pet_PNG(256x256)\\cat04_256.png"
            string temp1 = fileName.Split('\\')[4];
            string tempName = temp1.Split('.')[0];
            string saveFname = tempName + "_temp";
            Bitmap image = new Bitmap(outH, outW); // 빈 비트맵(종이) 준비
            for (int i = 0; i < outH; i++)
                for (int k = 0; k < outW; k++)
                {
                    Color c;
                    int r, g, b;
                    r = outImage[0, i, k];
                    g = outImage[1, i, k];
                    b = outImage[2, i, k];
                    c = Color.FromArgb(r, g, b);
                    image.SetPixel(i, k, c);  // 종이에 콕콕 찍기
                }
            // 상단에 using System.Drawing.Imaging; 추가해야 함
            // 종이를 임시 파일로 저장 
            
            string f_name = saveFname;
            string f_extname = "png"; 
            long f_size = inH*inW;
            Random rnd = new Random();
            int f_id = rnd.Next(int.MinValue, int.MaxValue);

            string full_name = "C:\\images\\" + saveFname + "." + f_extname;
            //FileStream fs = new FileStream(full_name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            FileStream fs = new FileStream(full_name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            image.Save(fs, ImageFormat.Png); // 종이를 PNG로 저장
           
            sql = "INSERT INTO blob_table(f_id, f_name, f_extname, f_size, f_data ";
            sql += ") VALUES (";
            sql += f_id + ", '" + f_name + "', '" + f_extname + "', " + f_size + ",";
            sql += "@BLOB_DATA2)";
            // 파일을 준비
            //FileStream fs = new FileStream(full_name, FileMode.Open, FileAccess.Read);
            byte[] blob_data = new byte[f_size];
            fs.Read(blob_data, 0, (int)f_size);
            fs.Close();

            cmd.Parameters.Clear();///
            cmd.Parameters.AddWithValue("@BLOB_DATA2", blob_data);
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            cmd.ExecuteNonQuery();
            MessageBox.Show(saveFname + "으로 저장됨.");

        }

        // ///////////////////////////////
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
        void bw_CV()
        {
            // 이진화
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(outCvImage, outCvImage, 127, 255, ThresholdTypes.Otsu);
        }
        void bw2_CV()//적응형 이진화
        {
            Cv2.CvtColor(inCvImage, outCvImage, ColorConversionCodes.BGR2GRAY);
            Cv2.AdaptiveThreshold(outCvImage, outCvImage, 255,
                AdaptiveThresholdTypes.GaussianC, ThresholdTypes.Binary, 25, 5);
            CV2ToOutImage();
        }
        void zoom_in_CV()
        {
            Cv2.PyrUp(inCvImage, outCvImage, new OpenCvSharp.Size(inCvImage.Width * 2, inCvImage.Height * 2));
            CV2ToOutImage();
        }
        void zoom_out_CV()
        {
            Cv2.PyrDown(inCvImage, outCvImage, new OpenCvSharp.Size(inCvImage.Width / 2, inCvImage.Height / 2));
            CV2ToOutImage();
        }

        void resize_CV()
        {
            Cv2.Resize(inCvImage, outCvImage, new OpenCvSharp.Size(500, 250));
            CV2ToOutImage();
        }
        void blur_CV()
        {
            Cv2.Blur(inCvImage, outCvImage, new OpenCvSharp.Size(15, 15));
            CV2ToOutImage();
        }
        void lr_mirror_CV()
        {
            Cv2.Flip(inCvImage, outCvImage, FlipMode.Y);
            CV2ToOutImage();
        }
        void sobel_CV()
        {
            if (outCvImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inCvImage = outCvImage;
            Mat blur = new Mat();  Mat sobel = new Mat();
            Cv2.GaussianBlur(inCvImage, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);
            Cv2.Sobel(blur, sobel, MatType.CV_32F, 1, 0, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
            sobel.ConvertTo(sobel, MatType.CV_8UC1);
            outCvImage = sobel;

            CV2ToOutImage();
        }

        void scharr_CV()
        {
            Mat blur = new Mat();
            Mat scharr = new Mat();
            Cv2.GaussianBlur(inCvImage, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);
            Cv2.Scharr(blur, scharr, MatType.CV_32F, 1, 0, scale: 1, delta: 0, BorderTypes.Default);
            scharr.ConvertTo(scharr, MatType.CV_8UC1);
            outCvImage = scharr;

            CV2ToOutImage();
        }
        void laplacian_CV()
        {
            
            Mat blur = new Mat();
            Mat laplacian = new Mat();
            Cv2.GaussianBlur(inCvImage, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);
            Cv2.Laplacian(blur, laplacian, MatType.CV_32F, ksize: 3, scale: 1, delta: 0, BorderTypes.Default);
            laplacian.ConvertTo(laplacian, MatType.CV_8UC1);
            outCvImage = laplacian;

            CV2ToOutImage();
        }
        void canny_CV()
        {
            Mat blur = new Mat();
            Mat canny = new Mat();
            Cv2.GaussianBlur(inCvImage, blur, new OpenCvSharp.Size(3, 3), 1, 0, BorderTypes.Default);
            Cv2.Canny(blur, canny, 100, 200, 3, true);
            outCvImage = canny;
            CV2ToOutImage();
        }

        void label_CV()
        {

        }
        void camera_CV()
        {
            //VideoCapture 클래스... 카메라 장치 번호를 사용해 카메라와 연결
            VideoCapture video = new VideoCapture(0); //첫 번째로 연결된 카메라
            Mat frame = new Mat();
            //33ms마다 반복, q키 입력하면 중지
            while (Cv2.WaitKey(33) != 'q')
            {
                video.Read(frame);
                outCvImage = frame;
                // CV2ToOutImage()
                outW = outCvImage.Height;
                outH = outCvImage.Width;
                outImage = new byte[RGB, outW, outH];
                // OpenCV이미지Matrix --> 메모리 (로딩)
                for (int i = 0; i < outW; i++)
                    for (int k = 0; k < outH; k++)
                    {
                        var c = outCvImage.At<Vec3b>(i, k);
                        outImage[RR, i, k] = c.Item2;
                        outImage[GG, i, k] = c.Item1;
                        outImage[BB, i, k] = c.Item0;
                    }
                
                //displayImage();
                // 벽, 게시판, 종이 크기 조절
                paper = new Bitmap(outW, outH); // 종이
                pictureBox1.Size = new System.Drawing.Size(outW, outH); // 캔버스
                this.Size = new System.Drawing.Size(outW + 230, outH + 100); // 벽

                Color pen; // 펜(콕콕 찍을 용도)
                for (int i = 0; i < outW; i++)
                    for (int k = 0; k < outH; k++)
                    {
                        byte r = outImage[RR, i, k]; // 잉크(색상값)R
                        byte g = outImage[GG, i, k]; // 잉크(색상값)G
                        byte b = outImage[BB, i, k]; // 잉크(색상값)B
                        pen = Color.FromArgb(r, g, b); // 펜에 잉크 묻히기
                        paper.SetPixel(i, k, pen); // 종이에 콕 찍기
                    }
                pictureBox1.Image = paper; // 게시판에 종이를 붙이기.
                toolStripStatusLabel1.Text =
                    outH.ToString() + "x" + outW.ToString() + "  " + fileName;
            }

            frame.Dispose();
            video.Release();
            Cv2.DestroyAllWindows();
            
        }

        void slice_CV()
        {
            outCvImage = new Mat();
            outCvImage = inCvImage.SubMat(new Rect(10, 200, 160, 400));
            //(y, x, 세로, 가로)(내 코드에선 가세바뀜)
            CV2ToOutImage();
        }

        void distinct_color_CV_orange()
        {
            outCvImage = new Mat();
            //이미지를 HSV로 변환한 후 H, S, V 로 분리
            Mat hsv = new Mat();
            //이미지를 HSV로 변환
            Cv2.CvtColor(inCvImage, hsv, ColorConversionCodes.BGR2HSV);
            //HSV화 된 이미지를 분리
            Mat[] HSV = Cv2.Split(hsv);
            Mat H = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            //InRange...채널의 최대, 최소치 설정. InRange(원본, 최소, 최대, 결과)
            Cv2.InRange(HSV[0], new Scalar(8), new Scalar(20), H);
            //BitwiseAnd...비트연산중 And연산. BitwiseAnd(이미지1, 이미지2, 결과(=이미지1&이미지2), 마스크)
            Cv2.BitwiseAnd(hsv, hsv, outCvImage, H);
            //HSV이미지를 다시 BGR로 변환
            Cv2.CvtColor(outCvImage, outCvImage, ColorConversionCodes.HSV2BGR);
            /////////////////////////////
            CV2ToOutImage();
        }
        void distinct_color_CV_blue()
        {
            outCvImage = new Mat();
            //이미지를 HSV로 변환한 후 H, S, V 로 분리
            Mat hsv = new Mat();
            //이미지를 HSV로 변환
            Cv2.CvtColor(inCvImage, hsv, ColorConversionCodes.BGR2HSV);
            //HSV화 된 이미지를 분리
            Mat[] HSV = Cv2.Split(hsv);
            Mat H = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            //InRange...채널의 최대, 최소치 설정. InRange(원본, 최소, 최대, 결과)
            Cv2.InRange(HSV[0], new Scalar(110), new Scalar(130), H);
            //BitwiseAnd...비트연산중 And연산. BitwiseAnd(이미지1, 이미지2, 결과(=이미지1&이미지2), 마스크)
            Cv2.BitwiseAnd(hsv, hsv, outCvImage, H);
            //HSV이미지를 다시 BGR로 변환
            Cv2.CvtColor(outCvImage, outCvImage, ColorConversionCodes.HSV2BGR);
            /////////////////////////////
            CV2ToOutImage();
        }
        void distinct_color_CV_green()
        {
            if (outCvImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inCvImage = outCvImage;
            outCvImage = new Mat();
            //이미지를 HSV로 변환한 후 H, S, V 로 분리
            Mat hsv = new Mat();
            //이미지를 HSV로 변환
            Cv2.CvtColor(inCvImage, hsv, ColorConversionCodes.BGR2HSV);
            //HSV화 된 이미지를 분리
            Mat[] HSV = Cv2.Split(hsv);
            Mat H = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            //InRange...채널의 최대, 최소치 설정. InRange(원본, 최소, 최대, 결과)
            Cv2.InRange(HSV[0], new Scalar(40), new Scalar(70), H);  //40~70
            //BitwiseAnd...비트연산중 And연산. BitwiseAnd(이미지1, 이미지2, 결과(=이미지1&이미지2), 마스크)
            Cv2.BitwiseAnd(hsv, hsv, outCvImage, H);
            //HSV이미지를 다시 BGR로 변환
            Cv2.CvtColor(outCvImage, outCvImage, ColorConversionCodes.HSV2BGR);
            /////////////////////////////
            CV2ToOutImage();
        }
        void distinct_color_CV_lightgreen()
        {
            if (outCvImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inCvImage = outCvImage;
            outCvImage = new Mat();
            //이미지를 HSV로 변환한 후 H, S, V 로 분리
            Mat hsv = new Mat();
            //이미지를 HSV로 변환
            Cv2.CvtColor(inCvImage, hsv, ColorConversionCodes.BGR2HSV);
            //HSV화 된 이미지를 분리
            Mat[] HSV = Cv2.Split(hsv);
            Mat H = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            //InRange...채널의 최대, 최소치 설정. InRange(원본, 최소, 최대, 결과)
            Cv2.InRange(HSV[0], new Scalar(30), new Scalar(50), H);  //40~70
            //BitwiseAnd...비트연산중 And연산. BitwiseAnd(이미지1, 이미지2, 결과(=이미지1&이미지2), 마스크)
            Cv2.BitwiseAnd(hsv, hsv, outCvImage, H);
            //HSV이미지를 다시 BGR로 변환
            Cv2.CvtColor(outCvImage, outCvImage, ColorConversionCodes.HSV2BGR);
            /////////////////////////////
            CV2ToOutImage();
        }
        void distinct_color_CV_darkgreen()
        {
            if (outCvImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inCvImage = outCvImage;
            outCvImage = new Mat();
            //이미지를 HSV로 변환한 후 H, S, V 로 분리
            Mat hsv = new Mat();
            //이미지를 HSV로 변환
            Cv2.CvtColor(inCvImage, hsv, ColorConversionCodes.BGR2HSV);
            //HSV화 된 이미지를 분리
            Mat[] HSV = Cv2.Split(hsv);
            Mat H = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            //InRange...채널의 최대, 최소치 설정. InRange(원본, 최소, 최대, 결과)
            Cv2.InRange(HSV[0], new Scalar(70), new Scalar(100), H);  //70~100
            //BitwiseAnd...비트연산중 And연산. BitwiseAnd(이미지1, 이미지2, 결과(=이미지1&이미지2), 마스크)
            Cv2.BitwiseAnd(hsv, hsv, outCvImage, H);
            //HSV이미지를 다시 BGR로 변환
            Cv2.CvtColor(outCvImage, outCvImage, ColorConversionCodes.HSV2BGR);
            /////////////////////////////
            CV2ToOutImage();
        }
        void distinct_color_CV_red()
        {
            outCvImage = new Mat();
            //이미지를 HSV로 변환한 후 H, S, V 로 분리
            Mat hsv = new Mat();
            //이미지를 HSV로 변환
            Cv2.CvtColor(inCvImage, hsv, ColorConversionCodes.BGR2HSV);
            //HSV화 된 이미지를 분리
            Mat[] HSV = Cv2.Split(hsv);
            Mat H = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            Mat H2 = new Mat(inCvImage.Size(), MatType.CV_8UC1);
            //InRange...채널의 최대, 최소치 설정. InRange(원본, 최소, 최대, 결과)
            //RED...0~6, 175~180
            Cv2.InRange(HSV[0], new Scalar(0), new Scalar(6), H);
            Cv2.InRange(HSV[0], new Scalar(175), new Scalar(180), H2);
            //BitwiseAnd...비트연산중 And연산. BitwiseAnd(이미지1, 이미지2, 결과(=이미지1&이미지2), 마스크)
            Cv2.BitwiseAnd(hsv, hsv, outCvImage, H);
            Cv2.BitwiseAnd(hsv, hsv, outCvImage, H2);
            //HSV이미지를 다시 BGR로 변환
            Cv2.CvtColor(outCvImage, outCvImage, ColorConversionCodes.HSV2BGR);
            /////////////////////////////
            CV2ToOutImage();
        }

        void morph_CV()
        {

            outCvImage = new Mat();

            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Cross,
                new OpenCvSharp.Size(3, 3));
            Cv2.Dilate(inCvImage, outCvImage, kernel, new OpenCvSharp.Point(-1, -1),
                3, BorderTypes.Reflect101, new Scalar(0));
            /////////////////////////////

            CV2ToOutImage();

        }
        void distinct_circle_CV()
        {
            if (outCvImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inCvImage = outCvImage;

            outCvImage = new Mat();
            Mat image = new Mat();
            outCvImage = inCvImage.Clone();
            //모폴로지
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            //그레이 스케일...인풋이미지가 그레이가 아닐 경우에만 그레이스케일링
            if (inCvImage.Channels() != 1)
                Cv2.CvtColor(inCvImage, image, ColorConversionCodes.BGR2GRAY);
            else
                image = inCvImage;
            //Dilate연산...필터 내부의 가장 밝은 값으로 변환
            Cv2.Dilate(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);
            Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(13, 13), 3, 3, BorderTypes.Reflect101);
            Cv2.Erode(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);
            //Erode+Dilate=>Opening연산: 주로 작은 노이즈를 제거
            //Dilate+Erode=>Closing연산: 큰 객체로 합쳐줌

            //원검출: HoughCircles(그레이스케일, 메소드,분해능, 최소거리, Edge 임계값, 중심 임계값, 최소반지름, 최대반지름)
            CircleSegment[] circles = Cv2.HoughCircles(image, HoughModes.Gradient, 1, 100, 100, 35, 0, 0);

            for (int i = 0; i < circles.Length; i++)
            {
                //원의 중심점 지정
                OpenCvSharp.Point center = new OpenCvSharp.Point(circles[i].Center.X, circles[i].Center.Y);
                //원 테두리 색 지정
                Cv2.Circle(outCvImage, center, (int)circles[i].Radius, Scalar.White, 3);
                //원의 중심점에 점 찍기
                Cv2.Circle(outCvImage, center, 5, Scalar.AntiqueWhite, Cv2.FILLED);
            }
            /////////////////////////////

            CV2ToOutImage();
        }
        void distinct_color_circle_CV()
        {
            //원 검출--> 관심 영역 생성--> 검출 이미지 정확도 텍스트 출력
            // 1) 원 검출
            outCvImage = new Mat();
            Mat image = new Mat();
            outCvImage = inCvImage.Clone();
            //모폴로지
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            //그레이 스케일
            Cv2.CvtColor(inCvImage, image, ColorConversionCodes.BGR2GRAY);
            //Dilate연산...필터 내부의 가장 밝은 값으로 변환
            Cv2.Dilate(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);
            Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(13, 13), 3, 3, BorderTypes.Reflect101);
            Cv2.Erode(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);

            CircleSegment[] circles = Cv2.HoughCircles(image, HoughModes.Gradient, 1, 100, 100, 35, 0, 0);

            for (int i = 0; i < circles.Length; i++)
            {
                //원의 중심점 지정
                OpenCvSharp.Point center = new OpenCvSharp.Point(circles[i].Center.X, circles[i].Center.Y);

                Mat mask = new Mat(inCvImage.Size(), MatType.CV_8UC1);
                //Cv2.circle(mask, (260, 210), 100, (255, 255, 255), -1);
                //outCvImage = inCvImage.SubMat(new Rect(10, 200, 160, 400));//(시작점y, 시작점x, 세로, 가로)(내 코드에선 가세바뀜)
                Cv2.BitwiseOr(inCvImage, inCvImage, outCvImage, mask);

            }
            //2) 관심영역 자르기



            /////////////////////////////

            CV2ToOutImage();
        }
        void distinct_corner_CV()
        {
            outCvImage = new Mat();
            //////////////////////////
            // 진짜 OpenCV용 알고리즘
            Mat gray = new Mat();
            outCvImage = inCvImage.Clone();
            Cv2.CvtColor(inCvImage, gray, ColorConversionCodes.BGR2GRAY);
            Point2f[] corners = Cv2.GoodFeaturesToTrack(gray, 1000, 0.03, 5, null, 3, false, 0);
            for (int i = 0; i < corners.Length; i++)
            {
                OpenCvSharp.Point pt = new OpenCvSharp.Point((int)corners[i].X, (int)corners[i].Y);
                Cv2.Circle(outCvImage, pt, 5, Scalar.Yellow, Cv2.FILLED);
            }
            /////////////////////////////

            CV2ToOutImage();
        }
        
        void traffic_light_CV()
        {
            outCvImage = new Mat();
            Mat testSurface = new Mat();
            Mat image = new Mat();
            testSurface = inCvImage.Clone();
            int cent_x, cent_y;
            //모폴로지
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            //그레이 스케일...인풋이미지가 그레이가 아닐 경우에만 그레이스케일링
            if (inCvImage.Channels() != 1)
                Cv2.CvtColor(inCvImage, image, ColorConversionCodes.BGR2GRAY);
            else
                image = inCvImage;

            Cv2.Dilate(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);
            Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(13, 13), 3, 3, BorderTypes.Reflect101);
            Cv2.Erode(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);
            
            //원검출: HoughCircles(그레이스케일, 메소드,분해능, 최소거리, Edge 임계값, 중심 임계값, 최소반지름, 최대반지름)
            CircleSegment[] circles = Cv2.HoughCircles(image, HoughModes.Gradient, 1, 100, 100, 35, 0, 0);

            //원:190~300 ->D=110
            for (int i = 0; i < circles.Length; i++)
            {
                //원의 중심점 지정
                OpenCvSharp.Point center = new OpenCvSharp.Point(circles[i].Center.X, circles[i].Center.Y);
                //cent_x = (int)circles[i].Center.X;
                //cent_y = (int)circles[i].Center.Y;
            }
            cent_y = (int)circles[0].Center.X;
            cent_x = (int)circles[0].Center.Y;
            int Rad = 110;
            testSurface = inCvImage.SubMat(new Rect(cent_y-55, cent_x-55, Rad, Rad));
            //(y, x, 세로, 가로)(내 코드에선 가세바뀜)
            //Random rndY = new Random();
            //rndY.Next(0, cent_y);
            //testsurface를 배열로 바꿔준다.
            
            byte[,,] testAry = new byte[RGB, Rad, Rad]; // 메모리 할당
            // OpenCV이미지Matrix --> 메모리 (로딩)
            for (int i = 0; i < Rad; i++)
                for (int k = 0; k < Rad; k++)
                {
                    var color = testSurface.At<Vec3b>(i, k);
                    testAry[RR, i, k] = color.Item2;
                    testAry[GG, i, k] = color.Item1;
                    testAry[BB, i, k] = color.Item0;
                }

            Color c; //한 점 색상 모델
            double hh, ss, vv; //색상 채도 밝기
            int rr, gg, bb; //레드 그린 블루
            int count=0;
            int wholePixel = 110 * 110; //hh의 전체 갯수가 뭐지
            for (int i = 0; i < Rad; i++)
                for (int k = 0; k < Rad; k++)
                {
                    rr = testAry[RR, i, k];
                    gg = testAry[GG, i, k];
                    bb = testAry[BB, i, k];
                    //HSV로 변환
                    c = Color.FromArgb(rr, gg, bb);
                    hh = c.GetHue();
                    if (hh != 0)
                        wholePixel++;
                    //초록색 count
                    if ((hh >= 70 && hh <= 100))
                        count++;
                }
            
            double Gportion = (((double)count / (double)wholePixel) * 100.0);
            MessageBox.Show(Gportion.ToString()+" = "+count+" / "+wholePixel.ToString());
            
            /////////////////////////////


            //CV2ToOutImage();
        }
        void traffic_light_testSurface_CV()
        {
            outCvImage = new Mat();
            Mat testSurface = new Mat();
            Mat image = new Mat();
            testSurface = inCvImage.Clone();
            int cent_x, cent_y;
            //모폴로지
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Rect, new OpenCvSharp.Size(3, 3));
            //그레이 스케일...인풋이미지가 그레이가 아닐 경우에만 그레이스케일링
            if (inCvImage.Channels() != 1)
                Cv2.CvtColor(inCvImage, image, ColorConversionCodes.BGR2GRAY);
            else
                image = inCvImage;

            Cv2.Dilate(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);
            Cv2.GaussianBlur(image, image, new OpenCvSharp.Size(13, 13), 3, 3, BorderTypes.Reflect101);
            Cv2.Erode(image, image, kernel, new OpenCvSharp.Point(-1, -1), 3);

            //원검출: HoughCircles(그레이스케일, 메소드,분해능, 최소거리, Edge 임계값, 중심 임계값, 최소반지름, 최대반지름)
            CircleSegment[] circles = Cv2.HoughCircles(image, HoughModes.Gradient, 1, 100, 100, 35, 0, 0);

            //원:190~300 ->D=110
            for (int i = 0; i < circles.Length; i++)
            {
                //원의 중심점 지정
                OpenCvSharp.Point center = new OpenCvSharp.Point(circles[i].Center.X, circles[i].Center.Y);
                //cent_x = (int)circles[i].Center.X;
                //cent_y = (int)circles[i].Center.Y;
            }
            cent_y = (int)circles[0].Center.X;
            cent_x = (int)circles[0].Center.Y;
            testSurface = inCvImage.SubMat(new Rect(cent_y - 55, cent_x - 55, 110, 110));
            //(y, x, 세로, 가로)(내 코드에선 가세바뀜)

            /////////////////////////////
            ///
            //
            outCvImage = testSurface;
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
            //CV2ToOutImage();
        }
        // /////////////
        //영상처리 함수부
        // /////////////
        // 1) 화소점 처리
        void equal_image()
        {
            if (inImage == null)
                return;
            if (outImage != null || outCvImage != null) //이전 실행값이 있으면 null값으로 초기화 시켜줌.
            {
                outImage = null;
                outCvImage = null;
            }
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
        void brightness()
        {
            if (inImage == null)
                return;
            //if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
            //    inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //sbyte value = (sbyte)getValue();
            double value = trackBar1.Value;
            //MessageBox.Show(value.ToString()); ;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if ((inImage[rgb, i, k] + value) > 255)
                            outImage[rgb, i, k] = 255;
                        else if ((inImage[rgb, i, k] + value) < 0)
                            outImage[rgb, i, k] = 0;
                        else
                            outImage[rgb, i, k] = (byte)(inImage[rgb, i, k] + value);
                    }
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void gray_scale()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***

            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    byte rgb = (byte)(0.299 * inImage[RR, i, k] + 0.587 * inImage[GG, i, k] + 0.114 * inImage[BB, i, k]);

                    outImage[RR, i, k] = rgb;
                    outImage[GG, i, k] = rgb;
                    outImage[BB, i, k] = rgb;
                }
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void bw()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if (inImage[rgb, i, k] > 128)
                            outImage[rgb, i, k] = 255;
                        else
                            outImage[rgb, i, k] = 0;
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void bw_avg()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            int sum = 0, avg = 0;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        sum += inImage[rgb, i, k];
                        avg = sum / (inH * inW);
                        if (inImage[rgb, i, k] > avg)
                            outImage[rgb, i, k] = 255;
                        else
                            outImage[rgb, i, k] = 0;
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void gamma()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            double gammaLCD = 2.5;      //LCD의 감마값이 2.5라고 가정
            double gamma = 1.0 / gammaLCD;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        outImage[rgb, i, k] = (byte)(255.0 * Math.Pow(inImage[rgb, i, k] / 255.0, gamma));
                    }
            /////////////////////////////////////////////
            displayImage();

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

            histoForm hform = new histoForm(rHisto, gHisto, bHisto);
            hform.ShowDialog();
        }
        void parabola() //파라볼라
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = (byte)(255 * Math.Pow(((double)inImage[rgb, i, k] / 128.0) - 1.0, 2));
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void solarizing()   //솔러라이징
        {

            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = (byte)(255 - 255 * Math.Pow(((double)inImage[rgb, i, k] / 128.0) - 1.0, 2));
                    }
            /////////////////////////////////////////////
            displayImage();

        }
        void multiple() //선명도 조절
        {
            if (inImage == null)
                return;
            //if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
            //    inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            double c = getValue();
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if (c * inImage[rgb, i, k] > 255)
                            outImage[rgb, i, k] = 255;
                        else if (c * inImage[rgb, i, k] <= 0)
                            outImage[rgb, i, k] = 0;
                        else
                            outImage[rgb, i, k] = (byte)(c * inImage[rgb, i, k]);
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void poster()
        {

            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //명암 단계 0 32 64 96 128 160 192 224 255
            //const byte step = 32;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if (inImage[rgb, i, k] <= 0)
                            outImage[rgb, i, k] = 0;
                        else if (inImage[rgb, i, k] > 0 && inImage[rgb, i, k] <= 32)
                            outImage[rgb, i, k] = 32;
                        else if (inImage[rgb, i, k] > 32 && inImage[rgb, i, k] <= 64)
                            outImage[rgb, i, k] = 64;
                        //else if (inImage[rgb, i, k] > 64 && inImage[rgb, i, k] <= 96)
                        //    outImage[rgb, i, k] = 96;
                        else if (inImage[rgb, i, k] > 64 && inImage[rgb, i, k] <= 128)
                            outImage[rgb, i, k] = 128;
                        //else if (inImage[rgb, i, k] > 128 && inImage[rgb, i, k] <= 160)
                        //    outImage[rgb, i, k] = 160;
                        else if (inImage[rgb, i, k] > 128 && inImage[rgb, i, k] <= 192)
                            outImage[rgb, i, k] = 192;
                        else if (inImage[rgb, i, k] > 192 && inImage[rgb, i, k] <= 224)
                            outImage[rgb, i, k] = 224;
                        else
                            outImage[rgb, i, k] = 255;
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void change_satur()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //RGB-->HSV. 2. 한 점만 바꾼다(1. 전체를 바꾸거나 2. 한 점만 바꾸거나)
            Color c; //한 점 색상 모델
            double hh, ss, vv; //색상 채도 밝기
            int rr, gg, bb; //레드 그린 블루


            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    rr = inImage[RR, i, k];
                    gg = inImage[GG, i, k];
                    bb = inImage[BB, i, k];
                    //HSV로 변환
                    c = Color.FromArgb(rr, gg, bb);
                    hh = c.GetHue();
                    ss = c.GetSaturation();
                    vv = c.GetBrightness();
                    //(핵심!)채도 올리기
                    ss += 0.15;

                    //HSV->RGB변환
                    HsvToRgb(hh, ss, vv, out rr, out gg, out bb);
                    outImage[RR, i, k] = (byte)rr;
                    outImage[GG, i, k] = (byte)gg;
                    outImage[BB, i, k] = (byte)bb;
                }
            /////////////////////////////////////////////
            displayImage();
        }   //채도 조절
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

        void stretch()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            int value = 30;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if ((255 / (255 - 2 * value) * (inImage[rgb, i, k] - value)) > 255)
                            outImage[rgb, i, k] = 255;
                        else if ((255 / (255 - 2 * value) * (inImage[rgb, i, k] - value)) < 0)
                            outImage[rgb, i, k] = 0;
                        else
                            outImage[rgb, i, k] = (byte)(255 / (255 - 2 * value) * (inImage[rgb, i, k] - value));
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void compress()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            int value = 30;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if ((int)((255.0 - 2.0 * value) / 255.0 * (inImage[rgb, i, k]) + value) > 255)
                            outImage[rgb, i, k] = 255;
                        else if ((int)((255.0 - 2.0 * value) / 255.0 * (inImage[rgb, i, k]) + value) < 0)
                            outImage[rgb, i, k] = 0;
                        else
                            outImage[rgb, i, k] = (byte)((255.0 - 2.0 * value) / 255.0 * (inImage[rgb, i, k]) + value);
                    }

            displayImage();
        }
        void reverse()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = (byte)(255-inImage[rgb, i, k]);
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void selectReverse()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if ((i > sx && i < ex) && (k > sy && k < ey))
                            outImage[rgb, i, k] = (byte)(255 - inImage[rgb, i, k]);
                        else
                            outImage[rgb, i, k] = inImage[rgb, i, k];
                        
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void stress()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        if (inImage[rgb, i, k] >= 0 && inImage[rgb, i, k] < 140)                      //기본 경계값:128&192
                            outImage[rgb, i, k] = inImage[rgb, i, k];
                        else if (inImage[rgb, i, k] >= 180 && inImage[rgb, i, k] < 255)
                            outImage[rgb, i, k] = inImage[rgb, i, k];
                        else
                            outImage[rgb, i, k] = 255;
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void histo_equal() // 히스토그램 스트래칭
        {
            if (inImage == null)
                return;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // 수식 :
            // Out = (In - min) / (max - min) * 255.0
            byte min_val = inImage[0, 0, 0], max_val = inImage[0, 0, 0];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    if (min_val > inImage[rgb, i, k])
                        min_val = inImage[rgb, i, k];
                    if (max_val < inImage[rgb, i, k])
                        max_val = inImage[rgb, i, k];
                }
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    // Out = (In - min) / (max - min) * 255.0
                    outImage[rgb, i, k] = (byte)
                        ((double)(inImage[rgb, i, k] - min_val) / (max_val - min_val) * 255.0);
                }
            /////////////////////////////////////////////
            displayImage();
        }
        void end_in()//엔드 인
        {
            if (inImage == null)
                return;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];

            // 수식 :
            // Out = (In - min) / (max - min) * 255.0
            byte min_val = inImage[0, 0, 0], max_val = inImage[0, 0, 0];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    if (min_val > inImage[rgb, i, k])
                        min_val = inImage[rgb, i, k];
                    if (max_val < inImage[rgb, i, k])
                        max_val = inImage[rgb, i, k];
                }
            //min,max를 강제로 변경
            min_val += 30;
            max_val -= 30;
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    if (inImage[rgb, i, k] <= min_val)
                        outImage[rgb, i, k] = 0;
                    else if (inImage[rgb, i, k] >= max_val)
                        outImage[rgb, i, k] = 255;
                    else
                        outImage[rgb, i, k] = (byte)
                            ((double)(inImage[rgb, i, k] - min_val) / (max_val - min_val) * 255.0);
                }
            /////////////////////////////////////////////
            displayImage();
        }
        // ////////
        // 2)기하학 변환
        void zoom_in() //확대
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            double scale = getValue();
            outH = (int)(inH * scale); outW = (int)(inW * scale);
            outImage = new byte[RGB, outH, outW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = (inImage[rgb, (byte)(i / scale), (byte)(k / scale)]);
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void zoom_out() //축소
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            double scale = getValue();
            outH = (int)(inH / scale); outW = (int)(inW / scale);
            outImage = new byte[RGB, outH, outW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = (inImage[rgb, (byte)(i / scale), (byte)(k / scale)]);
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void lrMirror()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = inImage[rgb, outH - 1 - i, k];
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void tbMirror()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i, k] = inImage[rgb, i, outW - 1 - k];
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void translate()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            var distance = Getdistance();
            byte moveH = distance.Item1;
            byte moveW = distance.Item2;
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        outImage[rgb, i + moveH, k + moveW] = inImage[rgb, i, k];
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void rotate()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 회전하고 싶은 각도 입력
            const double PI = 3.1415926;
            double theta;
            int rotate_h, rotate_w;
            theta = getValue();
            double rad = (double)theta * PI / 180;
            double c = Math.Cos(rad); double s = Math.Sin(rad);
            // 회전했을 때 크기 구하기
            int rotH, rotW;
            rotH = (int)(Math.Abs(inW * s) + Math.Abs(inH * c));
            rotW = (int)(Math.Abs(inH * s) + Math.Abs(inW * c));
            // 이미지 중심 구하기 (영상 중심으로 회전시키기 위해)
            int centerH = rotH / 2, centerW = rotW / 2;
            // 회전했을 때의 크기로 rotateimage 메모리 할당받아 중앙에 inputImage를 넣음
            byte[,,] rotateimage = new byte[RGB, rotH, rotW];
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = centerH - (inH / 2), n = 0; i < centerH + (inH / 2); i++, n++)
                    for (int k = centerW - (inW / 2), m = 0; k < centerW + (inW / 2); k++, m++)
                        rotateimage[rgb, i, k] = inImage[rgb, n, m];

            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            // outImage메모리 할당
            outH = rotH; outW = rotW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        rotate_h = (int)((i - centerH) * Math.Cos(rad) - (k - centerW) * Math.Sin(rad) + centerH);
                        rotate_w = (int)((i - centerH) * Math.Sin(rad) + (k - centerW) * Math.Cos(rad) + centerW);
                        if ((rotate_w >= 0 && rotate_w < outW) && (rotate_h >= 0 && rotate_h < outH))
                            outImage[rgb, i, k] = rotateimage[rgb, rotate_h, rotate_w];
                        else
                        {
                            outImage[rgb, i, k] = 0;
                        }
                    }
            /////////////////////////////////////////////
            displayImage();
        }
        // //////////////////
        // 3)영역처리 함수

        double[,,] color_maskProcess(byte[,,] inImage, double[,] mask)  //컬러용 마스크프로세스
        {
            const int MSIZE = 3;
            //임시 입력, 출력 메모리 할당
            double[,,] tmpInput = new double[RGB, inH + 2, inW + 2];
            double[,,] tmpOutput = new double[RGB, outH, outW];
            //임시 입력을 중간값(혹은 평균값)으로 초기화
            int sum = 0, avg = 0;
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        sum += inImage[rgb, i, k];
                        avg = sum / (inH * inW);
                    }
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        tmpInput[rgb, i, k] = (double)avg;
                    }
            //입력을 임시 입력으로 복사
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                        tmpInput[rgb, i + 1, k + 1] = inImage[rgb, i, k];

            // *** 진짜 영상처리 알고리즘을 구현 ***
            double S = 0.0; //누적시킬 매개변수
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < inH; i++)
                {
                    for (int k = 0; k < inW; k++)
                    {
                        for (int m = 0; m < MSIZE; m++)
                            for (int n = 0; n < MSIZE; n++)
                                S += tmpInput[rgb, i + m, k + n] * mask[m, n];
                        tmpOutput[rgb, i, k] = S;
                        S = 0.0; //다음 점을 계산하기 위해 초기화 해준다.
                    }
                }
            }
            //후처리: 마스크의 합이 0이면 127을 더하기
            double sumMask = 0;
            for (int i = 0; i < MSIZE; i++)
            {
                for (int k = 0; k < MSIZE; k++)
                {
                    sumMask += mask[i, k];
                }
            }
            if (sumMask == 0)
            {
                for (int rgb = 0; rgb < RGB; rgb++)
                    for (int i = 0; i < inH; i++)
                    {
                        for (int k = 0; k < inW; k++)
                        {
                            tmpOutput[rgb, i, k] += 127;
                        }
                    }
            }
            return tmpOutput;
        }
        void emboss_image()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //마스크 결정
            // 중요! 마스크 결정
            double[,] mask = {        { -1.0, 0.0, 0.0 },
                                            {  0.0, 0.0, 0.0 },
                                            {  0.0, 0.0, 1.0 }   };
            double[,,] tmpOutput = color_maskProcess(inImage, mask);
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        double d = tmpOutput[rgb, i, k];
                        if (d > 255.0)
                            d = 255.0;
                        else if (d < 0.0)
                            d = 0.0;
                        outImage[rgb, i, k] = (byte)d;

                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void blur()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //마스크 결정
            // 중요! 마스크 결정
            double[,] mask = {     { 1/9.0, 1/9.0, 1/9.0 },
                                  {  1/9.0, 1/9.0, 1/9.0 },
                                  {  1/9.0, 1/9.0, 1/9.0 }     };
            double[,,] tmpOutput = color_maskProcess(inImage, mask);
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        double d = tmpOutput[rgb, i, k];
                        if (d > 255.0)
                            d = 255.0;
                        else if (d < 0.0)
                            d = 0.0;
                        outImage[rgb, i, k] = (byte)d;

                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void sharpen1()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //마스크 결정
            double[,] mask = {      { -1.0, -1.0, -1.0 },
                                        {  -1.0, 9.0, -1.0 },
                                        {  -1.0, -1.0, -1.0 }     };
            double[,,] tmpOutput = color_maskProcess(inImage, mask);
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        double d = tmpOutput[rgb, i, k];
                        if (d > 255.0)
                            d = 255.0;
                        else if (d < 0.0)
                            d = 0.0;
                        outImage[rgb, i, k] = (byte)d;

                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void sharpen2()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //마스크 결정
            double[,] mask = {      { 0.0, -1.0, 0.0 },
                                        {  -1.0, 5.0, -1.0 },
                                        {  0.0, -1.0, 0.0 }     };
            double[,,] tmpOutput = color_maskProcess(inImage, mask);
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        double d = tmpOutput[rgb, i, k];
                        if (d > 255.0)
                            d = 255.0;
                        else if (d < 0.0)
                            d = 0.0;
                        outImage[rgb, i, k] = (byte)d;

                    }
            /////////////////////////////////////////////
            displayImage();
        }
        void gaussian_filter()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //마스크 결정
            // 중요! 마스크 결정
            double[,] mask = {     { 1/16.0, 1/8.0, 1/16.0 },
                                  {  1/8.0, 1/4.0, 1/8.0 },
                                  {  1/16.0, 1/8.0, 1/16.0 }     };
            //임시 입력, 출력 메모리 할당
            double[,,] tmpOutput = color_maskProcess(inImage, mask);
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < outH; i++)
                    for (int k = 0; k < outW; k++)
                    {
                        double d = tmpOutput[rgb, i, k];
                        if (d > 255.0)
                            d = 255.0;
                        else if (d < 0.0)
                            d = 0.0;
                        outImage[rgb, i, k] = (byte)d;

                    }
            /////////////////////////////////////////////
            displayImage();
        }
        // /////////////////////
        //4) 에지(edge,엣지) 검출
        double[,,] double_edge_maskProcess(double[,,] inImage, double[,] mask) //double용 프로세스
        {
            //에지용 마스크 프로세스 메커니즘: 그레이 스케일링->마스크 회선->아웃풋 이미지 변환
            const int MSIZE = 3;
            //임시 입력, 출력 메모리 할당
            double[,,] tmpInput = new double[RGB, inH + 2, inW + 2];
            double[,,] tmpOutput = new double[RGB, outH, outW];

            // *** 진짜 영상처리 알고리즘을 구현 ***
            //입력을 임시 입력으로 복사
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                        tmpInput[rgb, i + 1, k + 1] = inImage[rgb, i, k];

            double S = 0.0; //누적시킬 매개변수
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < inH; i++)
                {
                    for (int k = 0; k < inW; k++)
                    {
                        for (int m = 0; m < MSIZE; m++)
                            for (int n = 0; n < MSIZE; n++)
                                S += tmpInput[rgb, i + m, k + n] * mask[m, n];
                        tmpOutput[rgb, i, k] = S;
                        S = 0.0; //다음 점을 계산하기 위해 초기화 해준다.
                    }
                }
            }
            return tmpOutput;
        }
        byte[,,] edge3D_maskProcess(byte[,,] inImage, double[,] mask)  //에지용 마스크프로세스
        {   //에지용 마스크 프로세스 메커니즘: 그레이 스케일링->마스크 회선->아웃풋 이미지 변환
            const int MSIZE = 3;
            //임시 입력, 출력 메모리 할당
            double[,,] tmpInput = new double[RGB, inH + 2, inW + 2];
            double[,,] tmpOutput = new double[RGB, outH, outW];
            //그레이 스케일로 바꿔주기
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    byte rgb = (byte)(0.299 * inImage[RR, i, k] + 0.587 * inImage[GG, i, k] + 0.114 * inImage[BB, i, k]);

                    tmpInput[RR, i, k] = rgb;
                    tmpInput[GG, i, k] = rgb;
                    tmpInput[BB, i, k] = rgb;
                }
            // *** 진짜 영상처리 알고리즘을 구현 ***
            double S = 0.0; //누적시킬 매개변수
            for (int rgb = 0; rgb < RGB; rgb++)
            {
                for (int i = 0; i < inH; i++)
                {
                    for (int k = 0; k < inW; k++)
                    {
                        for (int m = 0; m < MSIZE; m++)
                            for (int n = 0; n < MSIZE; n++)
                                S += tmpInput[rgb, i + m, k + n] * mask[m, n];
                        tmpOutput[rgb, i, k] = S;
                        S = 0.0; //다음 점을 계산하기 위해 초기화 해준다.
                    }
                }
            }
            
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int k = 0; k < inW; k++)
                    {
                        double d = tmpOutput[rgb, i, k];

                        if (d > 255.0)
                            d = 255.0;
                        else if (d < 0.0)
                            d = 0.0;
                        else
                            outImage[rgb, i, k] = (byte)tmpOutput[rgb, i, k];
                    }
                }
            return outImage;
        }
        void vertical_edge()    //수직 엣지 검출
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //중요! 마스크 결정
            double[,] mask = {    {  0.0, 0.0, 0.0 },
                                  { -1.0, 1.0, 0.0 },
                                  {  0.0, 0.0, 0.0 }     };
            outImage = edge3D_maskProcess(inImage, mask);
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void horizontal_edge()    //수평 엣지 검출
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //중요! 마스크 결정
            double[,] mask = {    {  0.0,-1.0, 0.0 },
                                  {  0.0, 1.0, 0.0 },
                                  {  0.0, 0.0, 0.0 }     };
            outImage = edge3D_maskProcess(inImage, mask);
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void homogen_edge()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            double max;
            const int MSIZE = 3;
            //임시 입력, 출력 메모리 할당
            double[,,] tmpInput = new double[RGB, inH + 2, inW + 2];
            double[,,] tmpOutput = new double[RGB, outH, outW];
            //그레이 스케일로 바꿔주기->임시 입력으로 보냄
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    byte rgb = (byte)(0.299 * inImage[RR, i, k] + 0.587 * inImage[GG, i, k] + 0.114 * inImage[BB, i, k]);

                    tmpInput[RR, i, k] = rgb;
                    tmpInput[GG, i, k] = rgb;
                    tmpInput[BB, i, k] = rgb;
                }
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //유사 연산자 에지 알고리즘
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    max = 0.0; //블록이 이동할 때마다 최대값 초기화
                    for (int m = 0; m < MSIZE; m++)
                        for (int n = 0; n < MSIZE; n++)
                        {
                            if ((double)Math.Abs((tmpInput[rgb, i + 1, k + 1] - tmpInput[rgb, i + m, k + n])) >= max)
                                max = (double)(Math.Abs(tmpInput[rgb, i + 1, k + 1] - tmpInput[rgb, i + m, k + n]));
                        }
                    tmpOutput[rgb, i, k] = max;
                }
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    if (tmpOutput[rgb, i, k] > 255.0)
                        tmpOutput[rgb, i, k] = 255.0;
                    else if (tmpOutput[rgb, i, k] < 0.0)
                        tmpOutput[rgb, i, k] = 0.0;

                }
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    outImage[rgb, i, k] = (byte)tmpOutput[rgb, i, k];
                }
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void roberts_row_edge()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //중요! 마스크 결정
            double[,] mask = {    { -1.0, 0.0, 0.0 },
                                  {  0.0, 1.0, 0.0 },
                                  {  0.0, 0.0, 0.0 }     };
            outImage = edge3D_maskProcess(inImage, mask);
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void roberts_col_edge()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //중요! 마스크 결정
            double[,] mask = {    {  0.0, 0.0, -1.0 },
                                      {  0.0, 1.0,  0.0 },
                                      {  0.0, 0.0,  0.0 }     };
            outImage = edge3D_maskProcess(inImage, mask);
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void roberts_edge()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            //그레이 스케일로 바꿔주기
            double[,,] grayInput = new double[RGB, inH , inW ];
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    byte rgb = (byte)(0.299 * inImage[RR, i, k] + 0.587 * inImage[GG, i, k] + 0.114 * inImage[BB, i, k]);

                    grayInput[RR, i, k] = rgb;
                    grayInput[GG, i, k] = rgb;
                    grayInput[BB, i, k] = rgb;
                }
            // *** 진짜 영상처리 알고리즘을 구현 ***
            double[,,] tmpOutput1 = new double[RGB, outH, outW];
            double[,,] tmpOutput2 = new double[RGB, outH, outW];
            double[,] mask_row = {    { -1.0, 0.0, 0.0 },
                                  {  0.0, 1.0, 0.0 },
                                  {  0.0, 0.0, 0.0 }     };
            tmpOutput1 = double_edge_maskProcess(grayInput, mask_row);
            double[,] mask_col = {    {  0.0, 0.0, -1.0 },
                                      {  0.0, 1.0,  0.0 },
                                      {  0.0, 0.0,  0.0 }     };
            tmpOutput2 = double_edge_maskProcess(tmpOutput1, mask_col);
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                {
                    for (int k = 0; k < inW; k++)
                    {
                        double d = tmpOutput2[rgb, i, k];

                        if (d > 255.0)
                            d = 255.0;
                        else if (d < 0.0)
                            d = 0.0;
                        else
                            outImage[rgb, i, k] = (byte)tmpOutput2[rgb, i, k];
                    }
                }
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }
        void laplacian_edge()
        {
            if (inImage == null)
                return;
            if (outImage != null) //이전 실행값이 있으면 이전값에 덮어 씌운다.
                inImage = outImage;
            // 중요! 출력이미지의 높이, 폭을 결정  --> 알고리즘에 영향
            outH = inH; outW = inW;
            outImage = new byte[RGB, outH, outW];
            // *** 진짜 영상처리 알고리즘을 구현 ***
            //중요! 마스크 결정
            double[,] mask = {    {  0.0,  1.0, 0.0 },
                                  {  1.0, -4.0, 1.0 },
                                  {  0.0,  1.0, 0.0 }     };
            outImage = edge3D_maskProcess(inImage, mask);
            /////////////////////////////////////////////
            displayImage();
            saveTempFile();
        }


        //메뉴 이벤트 처리부
        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outImage = null;    //새로운 파일을 열 때 그 전의 결과이미지는 지운다.
            openImage_CV();

        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            restoreTempFile();

        }

        private void redoMenuItem_Click(object sender, EventArgs e)
        {

        }

        Bitmap paper, bitmap;

        private void 상하반전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbMirror();
        }

        private void 이동ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            translate();
        }

        private void 파라볼라ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parabola();
        }

        private void 회전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rotate();
        }

        private void 저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveImage();
        }

        private void 엠보싱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            emboss_image();
        }

        private void 블러ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            blur();
        }

        private void 샤프닝1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sharpen1();
        }

        private void 샤프닝2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sharpen2();
        }

        private void 수직ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            vertical_edge();
        }

        private void 채도변경ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            change_satur();
        }

        private void 파라볼라ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            parabola();
        }

        private void 솔러라이징ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            solarizing();
        }

        private void 엠보싱ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            emboss_image();
        }

        private void 반전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reverse();
        }

        private void 흑백이미지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bw();
        }

        private void 선명도조절ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            multiple();
        }

        private void 포스터ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            poster();
        }

        private void 감마변환ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gamma();
        }

        private void 스트레치ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stretch();
        }

        private void 압축ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compress();
        }

        private void 강조ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stress();
        }

        private void 엔드인ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            end_in();
        }

        private void 수평ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            horizontal_edge();
        }

        private void 유사연산자에지검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            homogen_edge();
        }

        private void 로버츠ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            roberts_edge();
        }

        private void 수평ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            roberts_col_edge();
        }

        private void 수평수직ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            roberts_edge();
        }

        private void 수직ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            roberts_row_edge();
        }

        private void 라플라시안ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            laplacian_edge();
        }

        private void 선택영역반전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectReverse();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            if (mouseYN)
            {
                toolStripStatusLabel2.Text = "(" + x + ", " + y + ")";

            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)		//왼쪽 버튼 누르면
            {
                mouseYN = true;				//드래그 변수 true
            }
            sx = e.X; sy = e.Y;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mouseYN)
                return;
            else
            {
                ex = e.X; ey = e.Y;


                if (sx > ex)
                {
                    int tmp = sx; sx = ex; ex = tmp;
                }
                if (sy > sx)
                {
                    int tmp = sy; sy = ey; ey = tmp;
                }
            }

            mouseYN = false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
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

        private void 블러ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blur();
        }

        private void dBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBListForm frm = new DBListForm();

            if (frm.ShowDialog() == DialogResult.OK)
            {
                inImage = frm.DBinImage;
                inH = frm.i_height;
                inW = frm.i_width;
            }
            MessageBox.Show("DB 이미지 로딩 완료" + inW + "*" + inH);
            toolStripStatusLabel3.Text = "DB이미지" + DBimageID + "    " + inW + "*" + inH;

            DBimageOpen(DBimageID, inH, inW);
        }

        private void dB에파일저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {   //통째로 저장
            DBFileForm frm = new DBFileForm();
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    inImage = frm.DBinImage;
                    inH = frm.inH;
                    inW = frm.inW;
                }
                equal_image();
        }

        private void 그레이스케일ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            grayScale_CV();
        }

        private void 소벨ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            sobel_CV();
        }

        private void 샤르ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            scharr_CV();
        }

        private void 라플라시안ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            laplacian_CV();
        }

        private void 캐니ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            canny_CV();
        }

        private void 기하학변환ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 크기조절ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            resize_CV();
        }

        private void 색상검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_color_CV_orange();
        }

        private void 초록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_color_CV_green();
        }

        private void 블루ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_color_CV_blue();
        }

        private void 레드ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_color_CV_red();
        }

        private void 원검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_circle_CV();
        }

        private void 모폴로지ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            morph_CV();
        }

        private void 자르기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            slice_CV();
        }

        private void 색상원검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_color_circle_CV();
        }

        private void 코너검출ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_corner_CV(); 
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //textBox1.Text = trackBar1.Value.ToString();
            //this.trackBar1.SetRange(-100, 100);
            //if (mouseYN == false)
            //{
            //    brightness();
            //}
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
            this.trackBar1.SetRange(-100, 100);
            brightness();
        }

        private void 카메라ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera_CV();
            
        }

        private void 신호등ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            traffic_light_CV();
        }

        private void 신호영역자르기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            traffic_light_testSurface_CV();
        }

        private void 연초록ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_color_CV_lightgreen();
        }

        private void 다크그린ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            distinct_color_CV_darkgreen();
        }

        private void dB에파일로저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBfileSave();
        }

        private void dB에저장ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBimageSave();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //<1> 데이터베이스 연결 (교량 건설) + <2> 트럭 준비
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);

            trackBar1.Value = 0;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //<4> 데이터베이스 해제 (교량 철거)
            conn.Close();
        }

        private void 평활화ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            histo_equal();
        }

        private void 필터링ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 가우시안필터ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gaussian_filter();
        }

        private void 좌우반전ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lrMirror();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void ㅍ일ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
        private void 밝기조절ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            brightness();
        }
        private void 그레이스케일ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gray_scale();
        }
        private void 히스토그램그리기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            draw_histogram();
        }
    }
}

