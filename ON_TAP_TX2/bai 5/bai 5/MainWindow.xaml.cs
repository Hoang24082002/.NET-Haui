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
using bai_5.Models;
using System.Reflection;
using System.Text.RegularExpressions;

namespace bai_5
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
                            ThanhTien = sp.Soluong* sp.Dongia
                        };
            dgvsanpham.ItemsSource = query.ToList();
        }

        private void HienThiDB()
        {
            var query = from loai in db.LoaiSanPhams select loai;
            cboloai.ItemsSource = query.ToList();
            cboloai.DisplayMemberPath = "Tenloai";
            cboloai.SelectedValuePath = "Maloai";
            cboloai.SelectedIndex = 0;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiDB();
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
                    txtdongia.Text = p[4].GetValue(dgvsanpham.SelectedValue).ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Co loi khi chon dong");
                }
            }
        }

        private void btnthem_Click(object sender, RoutedEventArgs e)
        {
            var sp = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            if (sp != null)
            {
                MessageBox.Show("Ma san pham da ton tai!", "Thong bao");
            }
            else
            {
                SanPham x = new SanPham();
                x.Masp = txtma.Text;
                x.Tensp = txtten.Text;
                LoaiSanPham loai = (LoaiSanPham)cboloai.SelectedItem;
                x.Maloai = loai.Maloai;
                x.Soluong = int.Parse(txtsoluong.Text);
                x.Dongia = int.Parse(txtdongia.Text);
                db.SanPhams.Add(x);
                db.SaveChanges();
                MessageBox.Show("Them san pham thanh cong!", "Thong bao");
                HienThiDuLieu();
            }
        }

        private void btnsua_Click(object sender, RoutedEventArgs e)
        {
            var sp = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            if (sp != null)
            {
                sp.Tensp = txtten.Text;
                LoaiSanPham loai = (LoaiSanPham)cboloai.SelectedItem;
                sp.Maloai = loai.Maloai;
                sp.Soluong = int.Parse(txtsoluong.Text);
                sp.Dongia = int.Parse(txtdongia.Text);
                db.SaveChanges();
                MessageBox.Show("Sua san pham thanh cong", "Thong bao");
                HienThiDuLieu();
            }
            else
            {
                MessageBox.Show("Ma san pham khong ton tai!", "Thong bao");
            }
        }

        private void btnxoa_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            if (query != null)
            {
                MessageBoxResult rs = MessageBox.Show("Ban co chac chan muon xoa?", "Thong bao", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    db.SanPhams.Remove(query);
                    db.SaveChanges();
                    MessageBox.Show("Xoa san pham thanh cong!", "thong bao");
                    HienThiDuLieu();
                }
            }
            else
            {
                MessageBox.Show("Ma san pham khong ton tai", "Thong bao");
            }
        }

        private void btntimkiem_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.OrderBy(t => t.Dongia).Take(1).Select(sp => new { 
               sp.Masp,
               sp.Tensp,
               sp.Maloai,
               sp.Soluong,
               sp.Dongia,
               ThanhTien = sp.Soluong * sp.Dongia
            });
            dgvsanpham.ItemsSource = query.ToList();
        }

        private void btnthongke_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
        }
    }
}
