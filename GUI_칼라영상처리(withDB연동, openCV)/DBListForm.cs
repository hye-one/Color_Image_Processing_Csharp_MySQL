using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace Day015_01_컬러영상처리_Beta1_
{
    public partial class DBListForm : Form
    {
        public int i_id, i_width, i_height;
        public string i_fname, i_extname;
        public long i_fsize;
        public byte[,,] DBinImage;
        public int inH, inW;
        public DBListForm()
        {
            InitializeComponent();
        }
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=image_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        MySqlDataReader reader; // 트럭이 가져올 끈
        String full_name;
        string fileName;
        const int RGB = 3, RR = 0, GG = 1, BB = 2; //숫자보다는 기호로 쓰는 게 좋다
        Bitmap bitmap;

        // 선택창에서 DB 이미지 선택시 Event
        private void DBListComboBox_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectStr = DBListComboBox.SelectedItem.ToString();

            i_id = int.Parse(selectStr.Split('/')[0]);
            int i_width = int.Parse(selectStr.Split('/')[2].Split('x')[0]);
            int i_height = int.Parse(selectStr.Split('/')[2].Split('x')[1]);

            Bitmap paper = new Bitmap(i_width, i_height);   //종이 준비
            pictureBox1.Size = new Size(i_width, i_height);
            Color c;    //펜 준비

            //SQL문...SELECT p_row, p_col, p_value FROM color_pixel WHERE i_id = -1319994452 & RGB=0;
            sql = "SELECT RGB, p_row, p_col, p_value FROM color_pixel WHERE i_id =" + i_id; // 짐 싸기
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.

            int row, col;
            byte r=0, g=0, b=0;
            byte rgb=0;
            while (reader.Read())
            {
                row = int.Parse(reader["p_row"].ToString());
                col = int.Parse(reader["p_col"].ToString());
                r = g = b = byte.Parse(reader["p_value"].ToString());
                c = Color.FromArgb(r, g, b);
                paper.SetPixel(row, col, c);
            }
            reader.Close();
            pictureBox1.Image = paper;
        }
        
        private void btn_fileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.DefaultExt = "";
            ofd.Filter = "칼라 필터 | *.png; *.jpg; *.bmp; *.tif";
            ofd.ShowDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            String full_name = ofd.FileName;
            tb_fullfileName.Text = full_name;
            //파일-->비트맵
            bitmap = new Bitmap(full_name); //bitmap 라이브러리 불러옴

            // 중요! 입력이미지의 높이, 폭 알아내기-비트맵에서 불러옴(H,W바꿔야 함)
            inW = bitmap.Height;
            inH = bitmap.Width;
            DBinImage = new byte[RGB, inH, inW]; // 메모리 할당...면-행-렬
                                               
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            String full_name = tb_fullfileName.Text.ToString();
            // C:\\images\\Pet_PNG(squre)\\Pet_PNG(64x64)\\cat01_64.png 을 나눔
            string[] tmp = full_name.Split('\\');
            string tmp1 = tmp[tmp.Length - 1]; //cat01_64.png
            string[] tmp2 = tmp1.Split('.');
            i_fname = tmp2[0]; //cat01_64
            i_extname = tmp2[1]; //png
            i_width = bitmap.Height;
            i_height = bitmap.Width;
            i_fsize = i_width*i_height;
            String i_user = "Hong";
            Random rnd = new Random();
            i_id = rnd.Next(int.MinValue, int.MaxValue);
            // 이미지 테이블(부모 테이블)에 Insert
            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            // SQL문...INSERT INTO image(i_id, i_fname, i_extname,i_fsize,i_width ,i_height ,i_user) VALUES ();
            sql = "INSERT INTO image(i_id, i_fname, i_extname, i_fsize, i_width, i_height, i_user) VALUES (";
            sql += i_id + ",'" + i_fname + "','" + i_extname + "'," + i_fsize + "," + i_width + "," + i_height + ",'" + i_user + "');";
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            cmd.ExecuteNonQuery();

            //
            //PNG파일 열어서 color_pixel 테이블에 INSERT
            int p_row, p_col, p_value;
            cmd = new MySqlCommand("", conn);
           
            for (int rgb = 0; rgb < RGB; rgb++)
                for (int i = 0; i < inH; i++)
                    for (int k = 0; k < inW; k++)
                    {
                        p_row = i; p_col = k;
                        Color c = bitmap.GetPixel(i, k);//컬러 변수 c에 비트맵 함수로 픽셀의 값을 불러온다.
                        if (rgb == 0)
                            p_value = c.R;     //컬러 변수 c의 Red값을 불러와, RR번째 면에 넣는다.
                        else if (rgb == 1)
                            p_value = c.G;
                        else
                            p_value = c.B;

                        sql = "INSERT INTO color_pixel(i_id, RGB, p_row, p_col, p_value) VALUES(";
                        sql += i_id + "," + rgb + "," + p_row + "," + p_col + "," + p_value + ")";
                        cmd.CommandText = sql;  // 짐을 트럭에 싣기
                        cmd.ExecuteNonQuery();
                    }
            MessageBox.Show(full_name + " 입력 완료!");
        }

        private void btn_openDB_Click(object sender, EventArgs e)
        {
            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            sql = "SELECT i_id, i_fname, i_extname, i_width, i_height FROM image"; // 짐 싸기
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.

            string[] file_list = { }; //빈 배열 잡아 놓고 추가
            while (reader.Read())
            {
                i_id = (int)reader["i_id"];
                i_fname = (string)reader["i_fname"];
                i_extname = (string)reader["i_extname"];
                i_width = (int)reader["i_width"];
                i_height = (int)reader["i_height"];
                string str = i_id + "/" + i_fname + "." + i_extname;
                str += "/" + i_width + "x" + i_height;
                Array.Resize(ref file_list, file_list.Length + 1); //배열 크기 1개 증가
                file_list[file_list.Length - 1] = str;
            }
            reader.Close();
            DBListComboBox.Items.AddRange(file_list);
        }

        
        private void btn_image_putIn_Click(object sender, EventArgs e)
        {
            //메인폼으로 전달할 변수들: i_id, i_fname, i_extname, i_width, i_height

            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            string selectStr = DBListComboBox.SelectedItem.ToString();
            i_id = int.Parse(selectStr.Split('/')[0]);
            sql = "SELECT i_id, i_fname, i_extname, i_width, i_height FROM image WHERE i_id=" + i_id; // 짐 싸기
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.

            while (reader.Read())// 끈을 톡!하고 당기기
            {
                i_id = (int)reader["i_id"];
                i_fname = (string)reader["i_fname"];
                i_extname = (string)reader["i_extname"];
                i_width = (int)reader["i_width"];
                i_height = (int)reader["i_height"];
            }
            reader.Close();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DBListForm_Load(object sender, EventArgs e)
        {
            //<1> 데이터베이스 연결 (교량 건설) + <2> 트럭 준비
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
        }

        private void DBListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //<4> 데이터베이스 해제 (교량 철거)
            conn.Close();
        }
    }
}
