using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using bai_2.Models;
using System.Text.RegularExpressions;
using System.Reflection;

namespace bai_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //Tạo đối tượng trỏ tới Model
        QLNhanvienContext db = new QLNhanvienContext();

        //Hàm load dữ liệu trên DataGrid
        private void HienThiDuLieu()
        {
            var query = from nv in db.Nhanviens
                        orderby nv.Hoten
                        select new
                        {
                            nv.MaPhong,
                            nv.MaNv,
                            nv.Hoten,
                            nv.Luong,
                            nv.Thuong,
                            TongTien=nv.Luong+nv.Thuong
                        };
            dgvnhanvien.ItemsSource = query.ToList();
        }

        //Hàm load dữ liệu lên ComboBox
        private void HienThiCB()
        {
            var query = from phong in db.PhongBans select phong;
            cbomaphong.ItemsSource = query.ToList();
            cbomaphong.DisplayMemberPath = "MaPhong";
            cbomaphong.SelectedValuePath = "MaPhong";
            cbomaphong.SelectedIndex = 0;
        }

        //Load dữ liệu lên Window
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDuLieu();
            HienThiCB();
        }

        private void DataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvnhanvien.SelectedItem != null)
            {
                try
                {
                    Type t = dgvnhanvien.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtmanhanvien.Text = p[1].GetValue(dgvnhanvien.SelectedValue).ToString();
                    txthoten.Text = p[2].GetValue(dgvnhanvien.SelectedValue).ToString();
                    txtluong.Text = p[3].GetValue(dgvnhanvien.SelectedValue).ToString();
                    txtthuong.Text = p[4].GetValue(dgvnhanvien.SelectedValue).ToString();
                    cbomaphong.SelectedValue = p[0].GetValue(dgvnhanvien.SelectedValue).ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Có lỗi khi chọn dòng" + ex.Message);
                }
            }

        }

        private void btnthem_Click(object sender, RoutedEventArgs e)
        {
            //Tìm nhân viên cần thêm theo mã
            var query = db.Nhanviens.SingleOrDefault(t => t.MaNv.Equals(txtmanhanvien.Text));
       
            if (query != null)
            {
                MessageBox.Show("Mã nhân viên đã tồn tại!", "Thông báo");
                HienThiDuLieu();
            }
            else
            {
                Nhanvien nv = new Nhanvien();
                nv.MaNv = txtmanhanvien.Text;
                nv.Hoten = txthoten.Text;
                nv.Luong = int.Parse(txtluong.Text);
                nv.Thuong = int.Parse(txtthuong.Text);
                PhongBan phong = (PhongBan)cbomaphong.SelectedItem;
                nv.MaPhong = phong.MaPhong;
                db.Nhanviens.Add(nv);
                db.SaveChanges();
                MessageBox.Show("Thêm thành công", "Thông báo");
                HienThiDuLieu();
            }
        }

        private void btnsua_Click(object sender, RoutedEventArgs e)
        {
            //Tìm nhân viên cần sửa theo mã
            var nv = db.Nhanviens.SingleOrDefault(t => t.MaNv.Equals(txtmanhanvien.Text));
            if (nv != null)
            {
                nv.Hoten = txthoten.Text;
                nv.Luong = int.Parse(txtluong.Text);
                nv.Thuong = int.Parse(txtthuong.Text);
                PhongBan phong = (PhongBan)cbomaphong.SelectedItem;
                nv.MaPhong = phong.MaPhong;
                db.SaveChanges();
                MessageBox.Show("Sửa thành công!","Thông báo");
                HienThiDuLieu();
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên cần sửa!","Thông báo");
            }
        }

        private void btnxoa_Click(object sender, RoutedEventArgs e)
        {
            var nv = db.Nhanviens.SingleOrDefault(t => t.MaNv.Equals(txtmanhanvien.Text));
            if (nv != null)
            {
                MessageBoxResult rs = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Thông báo", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    db.Nhanviens.Remove(nv);
                    db.SaveChanges();
                    HienThiDuLieu();
                }
            }
            else
            {
                MessageBox.Show("Không có nhân viên này để xóa!", "Thông báo");
            }
        }

        private void btntimkiem_Click(object sender, RoutedEventArgs e)
        {
            /* ========= Tìm theo mã nhân viên ============
            var check = db.Nhanviens.SingleOrDefault(t => t.MaNv.Equals(txtmanhanvien.Text));
            if (check!=null)
            {
                var query = db.Nhanviens.Where(t => t.MaNv.Equals(txtmanhanvien.Text)).Select(nv => new {
                    nv.MaPhong,
                    nv.MaNv,
                    nv.Hoten,
                    nv.Luong,
                    nv.Thuong,
                    TongTien = nv.Luong + nv.Thuong
                });
                dgvnhanvien.ItemsSource = query.ToList();
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên", "Thông báo");
            }
            */

            //Tìm theo lương
            var check = db.Nhanviens.FirstOrDefault(t => t.Luong.Equals(int.Parse(txtluong.Text)));
            if (check != null)
            {
                var query = db.Nhanviens.Where(t => t.Luong.Equals(int.Parse(txtluong.Text))).Select(nv => new {
                    nv.MaPhong,
                    nv.MaNv,
                    nv.Hoten,
                    nv.Luong,
                    nv.Thuong,
                    TongTien = nv.Luong + nv.Thuong
                });
                dgvnhanvien.ItemsSource = query.ToList();
            }
            else
            {
                MessageBox.Show("Không tìm thấy nhân viên", "Thông báo");
            }
        }
    }
}
