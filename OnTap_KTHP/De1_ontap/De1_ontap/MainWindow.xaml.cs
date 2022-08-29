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
using De1_ontap.Models;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Globalization;

namespace De1_ontap
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

        QuanLyBenhNhanDBContext db = new QuanLyBenhNhanDBContext();
        private void HienThiDuLieu()
        {
            /*let TongTienFormatted = string.Format(new CultureInfo("vi-VN"), "{0:#,##0}", nv.Luong + nv.Thuong)*/
            var query = from bn in db.BenhNhans
                        orderby bn.HoTen
                        let formartVienPhi = string.Format(new CultureInfo("vi-VN"),"{0:#,##0}",bn.SoNgayNamVien*200000)
                        select new { bn.MaBn,
                            bn.HoTen,
                            bn.MaKhoa,
                            bn.SoNgayNamVien, 
                            formartVienPhi,
                        };
            dgvbenhnhan.ItemsSource = query.ToList();
        }

        private void HienThiCB()
        {
            var query = from khoa in db.Khoas select khoa;
            cbokhoa.ItemsSource = query.ToList();
            cbokhoa.DisplayMemberPath = "BenhNhan";
            cbokhoa.SelectedValuePath = "MaKhoa";
            cbokhoa.SelectedIndex = 0;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            HienThiCB();
            HienThiDuLieu();
        }

        private void dgvbenhnhan_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgvbenhnhan.SelectedItem != null)
            {
                try
                {
                    Type t = dgvbenhnhan.SelectedItem.GetType();
                    PropertyInfo[] p = t.GetProperties();
                    txtmabenhnhan.Text = p[0].GetValue(dgvbenhnhan.SelectedValue).ToString();
                    txthoten.Text = p[1].GetValue(dgvbenhnhan.SelectedValue).ToString();
                    cbokhoa.SelectedValue = p[2].GetValue(dgvbenhnhan.SelectedValue).ToString();
                    txtsongaynamvien.Text = p[3].GetValue(dgvbenhnhan.SelectedValue).ToString();
                  
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Co loi khi chon dong!", ex.Message);
                }
            }
        }

        //Ham check ma benh nhan
        private bool checkMaBenhNhan(int ma)
        {
            var query = db.BenhNhans.SingleOrDefault(bn => bn.MaBn.Equals(ma));
            if (query != null)
                return true;
            return false;
        }

        private bool CheckDL()
        {
            string tb = "";
            //Kiem tra dien day du thong tin
            if (txtmabenhnhan.Text=="" || txthoten.Text=="" || txtsongaynamvien.Text =="")
            {
                tb = "Ban can dien day du thong tin";
            }
            //Kiem tra kieu du lieu
            if (!Regex.IsMatch(txtsongaynamvien.Text,@"\d+"))
            { tb += "\n So ngay nam vien phai la so!"; }
            else
            {
                int soNgay = int.Parse(txtsongaynamvien.Text);
                if (soNgay < 1) tb += "\nSo ngay nam vien phai >=1";
            }

            //Kiem tra ten benh nhan kieu String
            if (Regex.IsMatch(txthoten.Text, @"\d+"))
            {
                tb += "\nTen benh nhan phai la chuoi ki tu!";
            }

            //Kiem tra ma benh nhan phai la so
            if (!Regex.IsMatch(txtmabenhnhan.Text, @"\d+"))
            {
                tb += "\nMa benh nhan nhap vao phai la so!";
            }

            if (tb != "")
                return false;

            return true;
        }

        private void btnthem_Click(object sender, RoutedEventArgs e)
        {
            if (!checkMaBenhNhan(int.Parse(txtmabenhnhan.Text)))
            {
                try
                {
                    if (CheckDL())
                    {
                        BenhNhan bn = new BenhNhan();
                        bn.MaBn = int.Parse(txtmabenhnhan.Text);
                        bn.HoTen = txthoten.Text;
                        bn.SoNgayNamVien = int.Parse(txtsongaynamvien.Text);
                        Khoa khoa = (Khoa)cbokhoa.SelectedItem;
                        bn.MaKhoa = khoa.MaKhoa;
                        db.BenhNhans.Add(bn);
                        db.SaveChanges();
                        MessageBox.Show("Them benh nhan thanh cong!", "Thong bao");
                        HienThiDuLieu();
                    }
                    else
                    {
                        MessageBox.Show("Du lieu nhap vao thieu hoac khong dung dinh dang!", "Thong bao");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Co loi khi them!", ex.Message);
                }
            
            }
            else
            {
                MessageBox.Show("Khong tim thay ma benh nhan!");
            }
        }

        private void btnsua_Click(object sender, RoutedEventArgs e)
        {
            if (checkMaBenhNhan(int.Parse(txtmabenhnhan.Text))==true)
            {
              if (CheckDL())
                {
                    var bn = db.BenhNhans.SingleOrDefault(x => x.MaBn.Equals(int.Parse(txtmabenhnhan.Text)));
                    bn.HoTen = txthoten.Text;
                    bn.SoNgayNamVien = int.Parse(txtsongaynamvien.Text);
                    Khoa khoa = (Khoa)cbokhoa.SelectedItem;
                    bn.MaKhoa = khoa.MaKhoa;
                    db.SaveChanges();
                    MessageBox.Show("Sua thanh cong!", "Thong bao");
                    HienThiDuLieu();
                }
                else
                {
                    MessageBox.Show("Ban can nhap du lieu chinh xac!", "Thong bao");
                }
            }
            else
            {
                MessageBox.Show("Khong tim thay ma benh nhan!");
            }
        }

        private void btnxoa_Click(object sender, RoutedEventArgs e)
        {
            if (checkMaBenhNhan(int.Parse(txtmabenhnhan.Text)))
            {
                MessageBoxResult rs = MessageBox.Show("Ban co chac chan muon xoa?", "Thong bao", MessageBoxButton.YesNo);
                if (rs == MessageBoxResult.Yes)
                {
                    var bn = db.BenhNhans.SingleOrDefault(x => x.MaBn.Equals(int.Parse(txtmabenhnhan.Text)));
                    db.BenhNhans.Remove(bn);
                    db.SaveChanges();
                    MessageBox.Show("Xoa thanh cong!", "Thong bao");
                    HienThiDuLieu();
                }  
            }
            else
            {
                MessageBox.Show("Khong tim thay ma benh nhan!", "Thong bao");
            }
        }

        private void btntimkiem_Click(object sender, RoutedEventArgs e)
        {
            //Tim benh nhan co ma khoa = 1
            var query = db.BenhNhans.Where(t => t.MaKhoa == 1).Select(bn => bn);
            dgvbenhnhan.ItemsSource = query.ToList();
        }
    }
}
