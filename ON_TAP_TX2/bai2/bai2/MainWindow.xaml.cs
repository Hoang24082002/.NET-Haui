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
using bai2.Models;
using System.Text.RegularExpressions;
using System.Reflection;


namespace bai2
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

        private void HienThiCB()
        {
            var query = from loai in db.LoaiSanPhams select loai;
            cboloai.ItemsSource = query.ToList();
            cboloai.DisplayMemberPath = "Tenloai";
            cboloai.SelectedValuePath = "Maloai";
            cboloai.SelectedIndex = 0;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiCB();
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
                } catch(Exception ex)
                {
                    MessageBox.Show("Co loi khi chon dong" + ex.Message + "Thong bao");
                }
            }
        }

        private void btnthem_Click(object sender, RoutedEventArgs e)
        {
            var query = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            if (query != null)
            {
                MessageBox.Show("Ma san pham nay da ton tai", "Thong bao");
                HienThiDuLieu();
            }
            else
            {
                SanPham sp = new SanPham();
                sp.Masp = txtma.Text;
                sp.Tensp = txtten.Text;
                sp.Dongia = int.Parse(txtgia.Text);
                sp.Soluong = int.Parse(txtsoluong.Text);
                LoaiSanPham loai = (LoaiSanPham)cboloai.SelectedItem;
                sp.Maloai = loai.Maloai;
                db.SanPhams.Add(sp);
                db.SaveChanges();
                MessageBox.Show("Them san pham thanh cong", "Thong bao");
                HienThiDuLieu();
            }
        }

        private void btnsua_Click(object sender, RoutedEventArgs e)
        {
            var spsua = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            if (spsua != null)
            {
                spsua.Tensp = txtten.Text;
                LoaiSanPham loai = (LoaiSanPham)cboloai.SelectedItem;
                spsua.Maloai = loai.Maloai;
                spsua.Soluong = int.Parse(txtsoluong.Text);
                spsua.Dongia = int.Parse(txtgia.Text);
                db.SaveChanges();
                MessageBox.Show("Sua thanh cong!", "Thong bao");
                HienThiDuLieu();
            }
            else
            {
                MessageBox.Show("Khong tim thay san pham can sua!", "Thong bao");
            }
        }

        private void btnxoa_Click(object sender, RoutedEventArgs e)
        {
            var spxoa = db.SanPhams.SingleOrDefault(t => t.Masp.Equals(txtma.Text));
            MessageBoxResult rs = MessageBox.Show("Ban co chac chan muon xoa?", "Thong bao", MessageBoxButton.YesNo);

            if (rs == MessageBoxResult.Yes)
            {
                db.SanPhams.Remove(spxoa);
                db.SaveChanges();
                HienThiDuLieu();
            }
            else
            {
                MessageBox.Show("Khong co san pham nay de xoa", "Thong bao");
            }
        }

        private void btntimkiem_Click(object sender, RoutedEventArgs e)
        {
            var sp = db.SanPhams.Where(x => x.Masp.Equals(txtma.Text)).Select(sp => new
            {
                sp.Masp,
                sp.Tensp,
                sp.Maloai,
                sp.Soluong,
                sp.Dongia
            });
            if (sp != null)
            {
                dgvsanpham.ItemsSource = sp.ToList();
            }
            else
            {
                MessageBox.Show("Khong tim thay san pham", "Thong bao");
            }


        }

        private void btnthongke_Click(object sender, RoutedEventArgs e)
        {
            MainWindow myWindow = new MainWindow();
            myWindow.Show();
        }
    }
}
