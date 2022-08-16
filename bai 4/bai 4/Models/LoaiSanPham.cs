using System;
using System.Collections.Generic;

#nullable disable

namespace bai_4.Models
{
    public partial class LoaiSanPham
    {
        public LoaiSanPham()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public string Maloai { get; set; }
        public string Tenloai { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
