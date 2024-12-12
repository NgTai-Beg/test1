using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test1
{
    public partial class Form1 : Form
    {
        private decimal giaVe = 0;
        private int soLuongNguoiLon = 0;
        private int soLuongSV = 0;
        private int soLuongTreEm = 0;
        private decimal giaVeNguoiLon = 100000;  // Giá vé người lớn
        private decimal giaVeSV = 75000;         // Giá vé sinh viên
        private decimal giaVeTreEm = 50000;     // Giá vé trẻ em
        private decimal tongTien = 0;
        public static string GetAsciiString(int[] numbers)
        {
            StringBuilder sb = new StringBuilder();

            foreach (int number in numbers)
            {
                sb.Append((char)number);
            }

            return sb.ToString();
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            khoitaosoghe(11, 15);
            khoitaoday(11, 1);
        }

        private void khoitaoday(int v1, int v2)
        {
            int x, y = 16, kc = 30, d = 65;
            for (int i = 0; i < v1; i++)
            {
                x = 11;
                for (int j = 0; j < v2; j++)
                {
                    Label lblDay = new Label();
                    lblDay.Location = new System.Drawing.Point(x, y);
                    lblDay.Size = new System.Drawing.Size(30, 23);
                    lblDay.Text = ((char)d).ToString();
                    lblDay.BackColor = Color.Black;
                    lblDay.TextAlign = ContentAlignment.MiddleCenter;
                    lblDay.ForeColor = Color.White;
                    panel1.Controls.Add(lblDay);
                    x += kc;
                }
                y += kc;
                d++;
            }

        }

        private void khoitaosoghe(int v1, int v2)
        {
            int x, y = 16, kc = 30, d;
            for (int i = 0; i < v1; i++)
            {
                x = 11;
                d = 1;
                for (int j = 0; j < v2; j++)
                {
                    Button btnGhe = new Button();
                    btnGhe.Location = new System.Drawing.Point(x, y);
                    btnGhe.Size = new System.Drawing.Size(30, 23);
                    btnGhe.Text = d++.ToString();
                    btnGhe.BackColor = Color.White;
                    btnGhe.TextAlign = ContentAlignment.MiddleCenter;
                    panel2.Controls.Add(btnGhe);
                    btnGhe.Click += BtnGhe_Click;
                    x += kc;
                }
                y += kc;
            }
        }
        private Dictionary<int, List<int>> gheDaChon = new Dictionary<int, List<int>>();
        private bool KiemTraGheBiBoGiua()
        {
            foreach (var kvp in gheDaChon)
            {
                List<int> gheDaChonTrongHang = kvp.Value;

                // Sắp xếp danh sách các ghế đã chọn theo vị trí (X)
                gheDaChonTrongHang.Sort();

                // Nếu ghế số 2 được chọn, kiểm tra ghế số 1
                if (gheDaChonTrongHang.Contains(41)) // 41 là tọa độ X của ghế số 2
                {
                    if (!gheDaChonTrongHang.Contains(11)) // 11 là tọa độ X của ghế số 1
                    {
                        return true; // Ghế số 2 được chọn nhưng ghế số 1 bị bỏ trống
                    }
                }

                // Kiểm tra ghế bị bỏ giữa (chỉ khi ghế đầu tiên đã được chọn)
                if (gheDaChonTrongHang.Count > 1)
                {
                    int firstSeat = gheDaChonTrongHang.First();
                    int lastSeat = gheDaChonTrongHang.Last();

                    for (int x = firstSeat; x <= lastSeat; x += 30) // 30 là khoảng cách pixel giữa các ghế
                    {
                        if (!gheDaChonTrongHang.Contains(x))
                        {
                            return true; // Phát hiện ghế bị bỏ giữa
                        }
                    }
                }
            }
            return false; // Không có ghế bị bỏ giữa hoặc ghế số 1 bị bỏ trống k
        }
        private bool KiemTraGheCot1DaChon(int yPosition)
        {
            // Duyệt qua các ghế đã chọn trong panel2 để tìm ghế ở cột 1 cùng hàng
            foreach (Control control in panel2.Controls)
            {
                if (control is Button btn && btn.Location.Y == yPosition)
                {
                    // Kiểm tra nếu ghế ở cột 1 (vị trí x = 11) đã được chọn (màu đỏ)
                    if (btn.Location.X == 11 && btn.BackColor == Color.Red)
                    {
                        return true;  // Nếu ghế ở cột 1 đã chọn
                    }
                }
            }
            return false;  // Không có ghế ở cột 1 đã chọn
        }
        private void CapNhatGheDaChon(Button b)
        {
            int yPosition = b.Location.Y;
            int xPosition = b.Location.X;

            if (!gheDaChon.ContainsKey(yPosition))
            {
                gheDaChon[yPosition] = new List<int>();
            }

            if (b.BackColor == Color.Red) // Ghế đã chọn
            {
                if (!gheDaChon[yPosition].Contains(xPosition))
                {
                    gheDaChon[yPosition].Add(xPosition);
                }
            }
            else // Ghế bỏ chọn
            {
                if (gheDaChon[yPosition].Contains(xPosition))
                {
                    gheDaChon[yPosition].Remove(xPosition);
                }

                // Xóa hàng nếu không còn ghế nào trong hàng được chọn
                if (gheDaChon[yPosition].Count == 0)
                {
                    gheDaChon.Remove(yPosition);
                }
            }
        }
        private bool KiemTraGheCungHang(int yPosition)
        {
            if (gheDaChon.ContainsKey(yPosition))
            {
                // Kiểm tra nếu có ghế ở cột 1 (X = 11) trong danh sách các ghế đã chọn trong cùng hàng
                if (gheDaChon[yPosition].Contains(11))
                {
                    return true;  // Ghế ở cột 1 đã được chọn
                }
            }
            return false;
        }
        private bool HasEmptySeatBetween(Button selectedSeat)
        {
            if (gheDaChon.ContainsKey(selectedSeat.Location.Y))
            {
                List<int> selectedSeatsInRow = gheDaChon[selectedSeat.Location.Y];

                int leftSeat = Math.Min(selectedSeat.Location.X, selectedSeatsInRow[0]);
                int rightSeat = Math.Max(selectedSeat.Location.X, selectedSeatsInRow[0]);

                // Kiểm tra có ghế trống giữa không
                for (int x = leftSeat + 30; x < rightSeat; x += 30) // Giả sử kích thước ghế là 30px
                {
                    if (IsSeatEmpty(x, selectedSeat.Location.Y))
                    {
                        return true;  // Có ghế trống giữa
                    }
                }
            }
            return false;
        }
        private bool IsSeatEmpty(int x, int y)
        {
            foreach (Control control in panel2.Controls)
            {
                if (control is Button btn && btn.Location.X == x && btn.Location.Y == y && btn.BackColor == Color.White)
                {
                    return true;  // Ghế trống
                }
            }
            return false; // Không có ghế trống
        }
        private void BtnGhe_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;

            // Kiểm tra nếu chưa chọn loại vé
            if (!radNguoiLon.Checked && !radSV.Checked && !radTreEm.Checked)
            {
                MessageBox.Show("Vui lòng chọn loại vé trước khi chọn ghế!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xử lý nếu ghế đã đặt (màu xám)
            if (b.BackColor == Color.Gray)
            {
                DialogResult result = MessageBox.Show("Ghế này đã được đặt. Bạn có muốn chọn lại không?", "Thông Báo", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    b.BackColor = Color.White; // Trả ghế về trạng thái có thể chọn
                    return;
                }
                else
                {
                    return; // Nếu không, không làm gì
                }
            }

            // Xử lý ghế trống
            if (b.BackColor == Color.White) // Ghế trống
            {
                b.BackColor = Color.Red; // Đánh dấu ghế đã chọn
                CapNhatGheDaChon(b); // Cập nhật danh sách ghế đã chọn

                // Tăng số lượng ghế tùy theo loại vé đã chọn
                if (radNguoiLon.Checked)
                {
                    soLuongNguoiLon++;
                }
                else if (radSV.Checked)
                {
                    soLuongSV++;
                }
                else if (radTreEm.Checked)
                {
                    soLuongTreEm++;
                }

                TinhThanhTien(); // Tính lại tổng tiền
            }
            else if (b.BackColor == Color.Red) // Nếu ghế đã chọn, bỏ chọn
            {
                b.BackColor = Color.White; // Đánh dấu ghế không được chọn nữa
                CapNhatGheDaChon(b); // Cập nhật danh sách ghế đã chọn

                // Giảm số lượng ghế tùy theo loại vé đã chọn
                if (radNguoiLon.Checked)
                {
                    soLuongNguoiLon--;
                }
                else if (radSV.Checked)
                {
                    soLuongSV--;
                }
                else if (radTreEm.Checked)
                {
                    soLuongTreEm--;
                }

                TinhThanhTien(); // Tính lại tổng tiền
            }
        }
        private void TinhGiaVe()
        {
            decimal giaVe = 0;

            // Kiểm tra loại vé đã chọn và gán giá tiền tương ứng
            if (radNguoiLon.Checked)
            {
                giaVe = 100000;
            }
            else if (radSV.Checked)
            {
                giaVe = 75000;
            }
            else if (radTreEm.Checked)
            {
                giaVe = 50000;
            }

            // Cập nhật giá vé vào TextBox "Giá Tiền"
            txtGiaVe.Text = giaVe.ToString();  // Format thành tiền tệ

            // Tính số lượng ghế đã chọn
            int soLuongGhe = 0;
            foreach (Control control in panel2.Controls)
            {
                if (control is Button btn && btn.BackColor == Color.Red) // Kiểm tra ghế đã được chọn
                {
                    soLuongGhe++;
                }
            }

        }
        private void TinhThanhTien()
        {

            decimal tongTien = 0;

            // Tính tiền cho từng loại vé đã chọn
            tongTien += soLuongNguoiLon * giaVeNguoiLon;
            tongTien += soLuongSV * giaVeSV;
            tongTien += soLuongTreEm * giaVeTreEm;

            // Cập nhật lại tổng tiền vào TextBox
            txtThanhTien.Text = tongTien.ToString("N0");

        }


        private void radNguoiLon_CheckedChanged_1(object sender, EventArgs e)
        {

            TinhGiaVe();
            TinhThanhTien();
        }

        private void radSV_CheckedChanged_1(object sender, EventArgs e)
        {

            TinhGiaVe();
            TinhThanhTien();
        }

        private void radTreEm_CheckedChanged_1(object sender, EventArgs e)
        {

            TinhGiaVe();
            TinhThanhTien();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            // Kiểm tra nếu có ghế bị bỏ giữa hoặc ghế số 1 bị bỏ trống khi ghế số 2 được chọn
            if (KiemTraGheBiBoGiua())
            {
                MessageBox.Show("Không thể thanh toán vì có ghế bị bỏ giữa hoặc ghế đầu tiên trong hàng bị bỏ trống khi ghế số 2 được chọn!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có muốn thanh toán?", "Thông Báo", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("Thanh toán thành công!", "Thông Báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Đổi màu ghế đã chọn thành màu xám
                foreach (Control control in panel2.Controls)
                {
                    if (control is Button btn && btn.BackColor == Color.Red)
                    {
                        btn.BackColor = Color.Gray; // Đổi màu ghế đã chọn thành màu xám
                    }
                }

                // Reset lại dữ liệu
                gheDaChon.Clear(); // Xóa danh sách ghế đã chọn
                soLuongNguoiLon = 0;
                soLuongSV = 0;
                soLuongTreEm = 0;
                tongTien = 0;
                txtThanhTien.Clear(); // Xóa hiển thị tổng tiền
            }
        }
        }
}
