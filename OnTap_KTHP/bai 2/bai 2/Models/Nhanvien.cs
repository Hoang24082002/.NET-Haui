using System;
using System.Collections.Generic;

#nullable disable

namespace bai_2.Models
{
    public partial class Nhanvien
    {
        public string MaNv { get; set; }
        public string Hoten { get; set; }
        public int? Luong { get; set; }
        public int? Thuong { get; set; }
        public string MaPhong { get; set; }

        public virtual PhongBan MaPhongNavigation { get; set; }
    }
}
