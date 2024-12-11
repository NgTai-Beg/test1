using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        string connectionString = "Server=.\\SQLEXPRESS;Database=rapphim;Trusted_Connection=True;";

        public int CancelClicked { get; internal set; }

        private void Seller_Load(object sender, EventArgs e)
        {
            // Câu truy vấn SQL để lấy dữ liệu từ cột
            string query = "SELECT TenPhim FROM Phim";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu vào ComboBox
                comboBox1.DisplayMember = "TenPhim"; // Cột hiển thị
                comboBox1.ValueMember = "id";
                comboBox1.DataSource = dataTable;
            }

            query = "SElECT TenMH FROM LoaiManHinh";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Đổ dữ liệu vào ComboBox
                comboBox2.DisplayMember = "TenMH"; // Cột hiển thị
                comboBox2.ValueMember = "id";
                comboBox2.DataSource = dataTable;
            }
        }
            private void button1_Click(object sender, EventArgs e)
        {
            string tenP = comboBox1.Text;
            string tenMH = comboBox2.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Câu truy vấn
                    String query = "SELECT l.id,p.TenPhim,h.TenMH,l.ThoiGianChieu,p.ThoiLuong,c.SoChoNgoi,TenPhong FROM LichChieu l,LoaiManHinh h,Phim p,DinhDangPhim d,PhongChieu c where l.idDinhDang = d.id and d.idPhim = p.id and d.idLoaiManHinh = h.id and l.idPhong = c.id and p.TenPhim = @tenphim and h.TenMH = @tenMH";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@tenphim", tenP);
                        cmd.Parameters.AddWithValue("@tenMH", tenMH);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Gán dữ liệu vào DataGridView
                        dtgLichChieuP.DataSource = dataTable;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 bv = new Form1();
            bv.Show();
        }
    }
}
