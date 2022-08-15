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
using bai1.Models;
using System.Text.RegularExpressions;
using System.Reflection;
namespace bai1
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

        QLBanHangContext db = new QLBanHangContext();

        //Hàm load dữ liệu lên DataGrid
        private void HienThiDuLieu()
        {
            var query = from SanPham in db.SanPhams
                        orderby SanPham.Dongia
                        select new
                        {
                            SanPham.Masp,
                            SanPham.Tensp,
                            SanPham.Maloai,
                            SanPham.Soluong,
                            SanPham.Dongia,
                            ThanhTien = SanPham.Soluong * SanPham.Dongia
                        };
            sanpham.ItemsSource = query.ToList();
        }

        // Hàm hiển thị dữ liệu lên ComboBox
        private void HienThiCB()
        {
           
            var query = from LoaiSanPham in db.LoaiSanPhams
                        select LoaiSanPham;
            cboloaisanpham.ItemsSource = query.ToList();
            cboloaisanpham.DisplayMemberPath = "Tenloai";
            cboloaisanpham.SelectedValuePath = "Maloai";
            cboloaisanpham.SelectedIndex = 0;
            
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDuLieu();
            HienThiCB();
        }

        private void btnthem_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtmasanpham.Text));
            if (query != null)
            {
                MessageBox.Show("Ma san pham nay da ton tai!", "Thong bao");
                HienThiDuLieu();
            }
            else
            {
                SanPham spMoi = new SanPham();
                spMoi.Masp = txtmasanpham.Text;
                spMoi.Tensp = txttensanpham.Text;
                spMoi.Dongia = int.Parse(txtdongia.Text);
                spMoi.Soluong = int.Parse(txtsoluong.Text);
                LoaiSanPham intemSelected = (LoaiSanPham)cboloaisanpham.SelectedItem;
                spMoi.Maloai = intemSelected.Maloai;
                db.SanPhams.Add(spMoi);
                db.SaveChanges();
                MessageBox.Show("Them san pham thanh cong", "Thong bao");
                HienThiDuLieu();

            }
        }

        private void btnsua_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnxoa_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btntim_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnthongke_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
