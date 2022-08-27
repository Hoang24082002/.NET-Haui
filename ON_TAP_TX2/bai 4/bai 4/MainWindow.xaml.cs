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
using bai_4.Models;
using System.Reflection;
using System.Text.RegularExpressions;


namespace bai_4
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

        private void HienThiDuLieu()
        {
          var query = from sp in db.SanPhams
                      orderby sp.Dongia
                      select new
                      {
                          sp.Masp,
                          sp.Tensp,
                          sp.Maloai,
                          sp.Soluong,
                          sp.Dongia,
                          ThanhTien = sp.Soluong * sp.Dongia
                      };
            dgvsanpham.ItemsSource = query.ToList();
        }
        private void HienThiCb()
        {
            var query = from loai in db.LoaiSanPhams select loai;
            cboloai.ItemsSource = query.ToList();
            cboloai.DisplayMemberPath = "Tenloai";
            cboloai.SelectedValuePath = "Maloai";
            cboloai.SelectedIndex = 0;
        }

        private void btnthem_Click(object sender, RoutedEventArgs e)
        {
            //Lay ra san pham thoa man dieu kien ma san pham truyen vao tu ban phim
            var query = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            //Kiem tra xem san pham da ton tai hay chua?
            if (query != null)
            {
                MessageBox.Show("Ma san pham da ton tai!", "Thong bao");
            }
            else
            {
                SanPham sp = new SanPham();
                sp.Masp = txtma.Text;
                sp.Tensp = txtten.Text;
                LoaiSanPham loai = (LoaiSanPham)cboloai.SelectedItem;
                sp.Maloai = loai.Maloai;
                sp.Soluong = int.Parse(txtsoluong.Text);
                sp.Dongia = int.Parse(txtgia.Text);
                db.SanPhams.Add(sp);
                db.SaveChanges();
                HienThiDuLieu();
                MessageBox.Show("Them san pham thanh cong!", "Thoongg bao");
            }
        }

        private void btnsua_Click(object sender, RoutedEventArgs e)
        {
            //Tim san pham theo ma san pham
            var sp = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            if (sp != null)
            {
                sp.Tensp = txtten.Text;
                LoaiSanPham loai = (LoaiSanPham)cboloai.SelectedItem;
                sp.Maloai = loai.Maloai;
                sp.Soluong = int.Parse(txtsoluong.Text);
                sp.Dongia = int.Parse(txtgia.Text);
                db.SaveChanges();
                MessageBox.Show("Sua san pham thanh cong!", "Thong bao");
                HienThiDuLieu();
            }
            else
            {
                MessageBox.Show("Ma san pham khong ton tai!", "Thong bao");
            }
        }

        private void btnxoa_Click(object sender, RoutedEventArgs e)
        {
            //Tim san pham theo ma san pham
            var sp = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            MessageBoxResult rs = MessageBox.Show("Ban co chac chan muon xoa?", "Thong bao", MessageBoxButton.YesNo);
            
            if (sp != null)
            {
                if (rs == MessageBoxResult.Yes)
                {
                    db.SanPhams.Remove(sp);
                    MessageBox.Show("Xoa san pham thanh cong!");
                    db.SaveChanges();
                    HienThiDuLieu();
                }
            }
            else
            {
                MessageBox.Show("Khong tim thay san pham can xoa!");
            }
           
        }

        private void btntimkiem_Click(object sender, RoutedEventArgs e)
        {
            var sp = db.SanPhams.Where(x => x.Masp.Equals(txtma.Text)).Select(t => new { 
                     t.Masp,
                     t.Tensp,
                     t.Maloai,
                     t.Soluong,
                     t.Dongia,
                     ThanhTien = t.Soluong * t.Dongia
            });
            if (sp != null)
            {
                dgvsanpham.ItemsSource = sp.ToList();
                
            }
        }

        private void btnthongke_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiCb();
            HienThiDuLieu();
        }

        private void dgvsanpham_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            
            if (dgvsanpham.SelectedItem != null)
            {
                try
                {
                    Type t = dgvsanpham.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtma.Text = p[0].GetValue(dgvsanpham.SelectedValue).ToString();
                    txtten.Text = p[1].GetValue(dgvsanpham.SelectedValue).ToString();
                    cboloai.SelectedValue = p[2].GetValue(dgvsanpham.SelectedValue).ToString();
                    txtsoluong.Text = p[3].GetValue(dgvsanpham.SelectedValue).ToString();
                    txtgia.Text = p[4].GetValue(dgvsanpham.SelectedValue).ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Co loi khi chon dong!" + ex.Message + "Thong bao");
                }
            }
        }
    }
}
