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
using De2_ontap.Models;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Globalization;

namespace De2_ontap
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

        QuanLySanPhamDBContext db = new QuanLySanPhamDBContext();

        private void HienThiDuLieu()
        {
            var query = from sp in db.SanPhams
                        join NhomHang in db.NhomHangs
                        on sp.MaNhomHang equals NhomHang.MaNhomHang
                        orderby sp.SoLuongBan descending
                        let TienBan = string.Format(new CultureInfo("vi-VN"),"{0:#,##0}",sp.DonGia*sp.SoLuongBan)
                        select new
                        {
                            sp.MaSp,
                            sp.TenSanPham,
                            sp.DonGia,
                            sp.SoLuongBan,
                            NhomHang.TenNhomHang,
                            TienBan,
                        };
            dgvsanpham.ItemsSource = query.ToList();
        }

        private void HienThiCB()
        {
            var query = from nhomhang in db.NhomHangs select nhomhang;
            cbonhomhang.ItemsSource = query.ToList();
            cbonhomhang.DisplayMemberPath = "TenNhomHang";
            cbonhomhang.SelectedValuePath = "MaNhomHang";
            cbonhomhang.SelectedIndex = 0;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiCB();
            HienThiDuLieu();
        }

        //Chon dong trong data grid
        private void dgvsanpham_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvsanpham.SelectedItem != null)
            {
                try
                {
                    Type t = dgvsanpham.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtmasanpham.Text = p[0].GetValue(dgvsanpham.SelectedValue).ToString();
                    txttensanpham.Text = p[1].GetValue(dgvsanpham.SelectedValue).ToString();
                    txtdongia.Text = p[2].GetValue(dgvsanpham.SelectedValue).ToString();
                    txtsoluongban.Text = p[3].GetValue(dgvsanpham.SelectedValue).ToString();
                    /*
                    string manhomhang = db.NhomHangs.Where(t => t.TenNhomHang.Equals(p[4].GetValue(dgvsanpham.SelectedValue).ToString())).Select(hang => new { hang.MaNhomHang }).ToString();
                    cbonhomhang.SelectedValue = manhomhang;
                    */
                    cbonhomhang.Text= p[4].GetValue(dgvsanpham.SelectedValue).ToString();

                }
                catch(Exception ex)
                {
                    MessageBox.Show("Co loi khi chon dong!", "Thong bao");
                }
            }
        }

        private bool CheckDL()
        {
            string tb = "";
            if (txtmasanpham.Text == "" || txttensanpham.Text == "" || txtdongia.Text =="" || txtsoluongban.Text=="")
            {
                tb = "Nhap thieu thong tin!";
            }

            //===Check kieu du lieu la so ===/
            /* !int.TryParse(mabenhnhan, out int MaBn) */
            // !Regex.IsMatch(txtsoluongban.Text, @"\d+")

            int masp;
            if (!int.TryParse(txtmasanpham.Text, out masp)) tb += "\nMa san pham phai la so nguyen";

            //Check so luong ban
            float dongia;
            int soluongban;
            if (!int.TryParse(txtsoluongban.Text, out soluongban))
                tb += "\nSo luong ban phai la so nguyen";
            else
            {
                if (float.Parse(txtsoluongban.Text) < 1) tb += "\nSo luong ban phai >=1";
            }

            //Check don gia
            if (!float.TryParse(txtdongia.Text, out dongia)) tb += "\nDon gia phai la so";
            else
            {
                if (float.Parse(txtdongia.Text) < 0) tb += "\nDon gia phai >=0";
            }

            if (tb != "")
                return false;
            return true;
        }
        private void btnthem_Click(object sender, RoutedEventArgs e)
        {
 
            if (CheckDL())
            {
                var sp = db.SanPhams.SingleOrDefault(sp => sp.MaSp.Equals(int.Parse(txtmasanpham.Text)));
                if (sp != null)
                {
                    MessageBox.Show("Ma san pham da ton tai!");
                }
                else
                {
                    SanPham x = new SanPham();
                    x.MaSp = int.Parse(txtmasanpham.Text);
                    x.TenSanPham = txttensanpham.Text;
                    x.DonGia = float.Parse(txtdongia.Text);
                    x.SoLuongBan = int.Parse(txtsoluongban.Text);
                    NhomHang nhomhang = (NhomHang)cbonhomhang.SelectedItem;
                    x.MaNhomHang = nhomhang.MaNhomHang;
                    db.SanPhams.Add(x);
                    db.SaveChanges();
                    MessageBox.Show("Them san pham thanh cong!", "Thong bao");
                    HienThiDuLieu();
                }

            }
            else
            {
                MessageBox.Show("Du lieu nhap vao khong hop le!", "Thong bao");
            }
        }
 
        private void btntim_Click(object sender, RoutedEventArgs e)
        {
            var query = from sp in db.SanPhams
                        join NhomHang in db.NhomHangs
                        on sp.MaNhomHang equals NhomHang.MaNhomHang
                        orderby sp.SoLuongBan descending
                        let TienBan = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", sp.DonGia * sp.SoLuongBan)
                        where sp.MaNhomHang==9
                        select new
                        {
                            sp.MaSp,
                            sp.TenSanPham,
                            sp.DonGia,
                            sp.SoLuongBan,
                            NhomHang.TenNhomHang,
                            TienBan,
                        };

            var check = db.SanPhams.FirstOrDefault(x => x.MaNhomHang==9);
            if (check != null)
                dgvsanpham.ItemsSource = query.ToList();
            else
            {
                MessageBox.Show("Khong tim thay san pham nao!", "Thong bao");
            }
        }

        private void txtsua_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(sp => sp.MaSp.Equals(int.Parse(txtmasanpham.Text)));
            if (query != null)
            {
               if (CheckDL())
                {
                    query.MaSp = int.Parse(txtmasanpham.Text);
                    query.TenSanPham = txttensanpham.Text;
                    query.DonGia = float.Parse(txtdongia.Text);
                    query.SoLuongBan = int.Parse(txtsoluongban.Text);
                    NhomHang nhomHang = (NhomHang)cbonhomhang.SelectedItem;
                    query.MaNhomHang = nhomHang.MaNhomHang;
                    db.SaveChanges();
                    MessageBox.Show("Sua thanh cong!", "Thong bao");
                    HienThiDuLieu();
                }
                else
                {
                    MessageBox.Show("Du lieu nhap vao sai dinh dang hoac thieu");
                }
            }
            else
            {
                MessageBox.Show("Khong tim thay ma san pham can sua!");
            }
        }

        private void txtxoa_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(sp => sp.MaSp.Equals(int.Parse(txtmasanpham.Text)));
          
                MessageBoxResult rs = MessageBox.Show("Ban co chac chan muon xoa?", "Thong bao", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    if (query != null)
                    {
                        db.SanPhams.Remove(query);
                        db.SaveChanges();
                        MessageBox.Show("Xoa san pham thanh cong!", "Thong bao");
                        HienThiDuLieu();
                    }
                    else MessageBox.Show("Khong tim thay ma san pham!", "Thong bao");
                }
        }
    }
}
