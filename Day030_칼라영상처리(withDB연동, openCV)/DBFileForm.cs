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
namespace Day015_01_컬러영상처리_Beta1_
{
    public partial class DBFileForm : Form
    {
        public int inH, inW;
        public string f_name, f_extname;
        public byte[,,] DBinImage;
        public DBFileForm()
        {
            InitializeComponent();
        }
        String connStr = "Server=192.168.56.101;Uid=winuser;Pwd=4321;Database=blob_db;Charset=UTF8";
        MySqlConnection conn; // 교량
        MySqlCommand cmd; // 트럭
        String sql = "";  // 물건박스
        MySqlDataReader reader; // 트럭이 가져올 끈
        ulong f_size;
        int f_id;
        const int RGB = 3, RR = 0, GG = 1, BB = 2;
        string fileName;
        
        private void DBFileForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_image_putIn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_fileOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            String full_name = ofd.FileName;
            tb_fullfileName.Text = full_name;
        }

        private void DBFileForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //<4> 데이터베이스 해제 (교량 철거)
            conn.Close();
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            //<1> 데이터베이스 연결 (교량 건설) + <2> 트럭 준비
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
            /*
              CREATE  TABLE  blob_table (
                f_id  INT  NOT NULL PRIMARY KEY, -- UUID, GUID (MySQL, C#)  char(36)
                f_name VARCHAR(50) NOT NULL,    -- 파일명
                f_extname VARCHAR(10) NOT NULL,  -- 확장명
                f_size  BIGINT UNSIGNED NOT NULL,		  -- 파일 크기
                f_data   LONGBLOB
              );
           */
            String full_name = tb_fullfileName.Text.ToString();
            // c:\\images\\pet_raw\\cat256_01.raw --> 아무 파일
            String[] tmp = full_name.Split('\\');
            String tmp1 = tmp[tmp.Length - 1];  // cat256_01.raw
            String[] tmp2 = tmp1.Split('.');
            String f_name = tmp2[0]; // cat256_01
            String f_extname = tmp2[1]; // raw
            long f_size = new FileInfo(full_name).Length;
            Random rnd = new Random();
            int i_id = rnd.Next(int.MinValue, int.MaxValue);
            // 이미지 테이블(부모 테이블)에 Insert
            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            sql = "INSERT INTO blob_table(f_id, f_name, f_extname, f_size, f_data ";
            sql += ") VALUES (";
            sql += i_id + ", '" + f_name + "', '" + f_extname + "', " + f_size + ",";
            sql += "@BLOB_DATA )";
            // 파일을 준비
            FileStream fs = new FileStream(full_name, FileMode.Open, FileAccess.Read);
            byte[] blob_data = new byte[f_size];
            fs.Read(blob_data, 0, (int)f_size);
            fs.Close();

            cmd.Parameters.AddWithValue("@BLOB_DATA", blob_data);
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            cmd.ExecuteNonQuery();

            conn.Close();
            MessageBox.Show(full_name + " 업로드 완료!");
        }

        private void Download_Click(object sender, EventArgs e)
        {
            //<1> 데이터베이스 연결 (교량 건설) + <2> 트럭 준비
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
            //파일 아이디, 파일 이름, 확장명, 파일 데이터 
            //DB파일을 임시폴더에 저장한 후 
            string selectStr = DBListComboBox.SelectedItem.ToString();
            string f_id = selectStr.Split('/')[0];
            sql = "SELECT f_id, f_name, f_extname, f_size, f_data FROM blob_table";
            sql += "  WHERE f_id=" + f_id;
            cmd.CommandText = sql;

            reader = cmd.ExecuteReader();
            reader.Read();
            string f_name = reader["f_name"].ToString();
            string f_extname = reader["f_extname"].ToString();
            int f_size = int.Parse(reader["f_size"].ToString());
            byte[] f_data = new byte[f_size];
            reader.GetBytes(reader.GetOrdinal("f_data"), 0, f_data, 0, f_size);

            //tmp폴더 찾아서 거기에 저장
            string full_name = "C:\\images\\" + f_name + "." + f_extname;

            FileStream fs = new FileStream(full_name, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Write(f_data, 0, (int)f_size);
             //tmp파일이 지워짐
            //inImage에 넣기
            fileName = "c:\\images\\" + f_name + "." + f_extname;
            //파일-->비트맵으로 읽어 오기
            Bitmap bitmap = new Bitmap(File.Open(fileName, FileMode.Open)); //종이 준비
            Color c;                              //펜 준비
            // 중요! 입력이미지의 높이, 폭 알아내기-비트맵에서 불러옴(H,W바꿔야 함)
            inW = bitmap.Height;
            inH = bitmap.Width;
            DBinImage = new byte[RGB, inH, inW]; // 메모리 할당...면-행-렬
                                                 // 비트맵-->메모리()
            for (int i = 0; i < inH; i++)
                for (int k = 0; k < inW; k++)
                {
                    c = bitmap.GetPixel(i, k);//컬러 변수 c에 비트맵 함수로 픽셀의 값을 불러온다.
                    DBinImage[RR, i, k] = c.R;  //컬러 변수 c의 Red값을 불러와, RR번째 면에 넣는다.
                    DBinImage[GG, i, k] = c.G;
                    DBinImage[BB, i, k] = c.B;
                }
            pictureBox1.Size = new Size(inW, inH);
            pictureBox1.Image = bitmap;
            fs.Close();
            MessageBox.Show(full_name + "...다운로드 완료");
            
        }

        private void DBListComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btn_openDB_Click(object sender, EventArgs e)
        {
            //<1> 데이터베이스 연결 (교량 건설) + <2> 트럭 준비
            conn = new MySqlConnection(connStr);
            conn.Open();
            cmd = new MySqlCommand("", conn);
            // <3> 물건을 준비해서, 트럭에 실어서 다리 건너 부어넣기.
            sql = "SELECT f_id, f_name, f_extname, f_size FROM blob_table"; // 짐 싸기
            cmd.CommandText = sql;  // 짐을 트럭에 싣기
            reader = cmd.ExecuteReader(); // 짐을 서버에 부어넣고, 끈으로 묶어서 끈만 가져옴.

            string[] file_list = { }; //빈 배열 잡아 놓고 추가
            while (reader.Read())
            {
                f_id = (int)reader["f_id"];
                f_name = (string)reader["f_name"];
                f_extname = (string)reader["f_extname"];
                f_size = (ulong)reader["f_size"];
                string str = f_id + "/" + f_name + "." + f_extname;
                Array.Resize(ref file_list, file_list.Length + 1); //배열 크기 1개 증가
                file_list[file_list.Length - 1] = str;
            }
            reader.Close();
            DBListComboBox.Items.AddRange(file_list);
            conn.Close();
        }
        
    }
}
