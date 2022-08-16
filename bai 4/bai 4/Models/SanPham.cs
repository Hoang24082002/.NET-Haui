using System;
using System.Collections.Generic;

#nullable disable

namespace bai_4.Models
{
    public partial class SanPham
    {
        public string Masp { get; set; }
        public string Tensp { get; set; }
        public string Maloai { get; set; }
        public int? Soluong { get; set; }
        public decimal? Dongia { get; set; }

        public virtual LoaiSanPham MaloaiNavigation { get; set; }
    }
}
