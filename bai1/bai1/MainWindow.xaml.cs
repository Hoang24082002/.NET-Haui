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

        //Thêm dữ liệu
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
                LoaiSanPham loaiduocchon = (LoaiSanPham)cboloaisanpham.SelectedItem;
                spMoi.Maloai = loaiduocchon.Maloai;
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

        //Chọn dòng trong DataGrid
        private void sanpham_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (sanpham.SelectedItem != null)
            {
                try
                {
                    Type t = sanpham.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtmasanpham.Text = p[0].GetValue(sanpham.SelectedValue).ToString();
                    txttensanpham.Text = p[1].GetValue(sanpham.SelectedValue).ToString();
                    cboloaisanpham.SelectedValue = p[2].GetValue(sanpham.SelectedValue).ToString();
                    txtsoluong.Text = p[3].GetValue(sanpham.SelectedValue).ToString();
                    txtdongia.Text = p[4].GetValue(sanpham.SelectedValue).ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Co loi khi chon hang" + ex.Message, "Thong bao");
                }
            }
        }
    }
}
