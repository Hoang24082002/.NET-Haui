using System;
using System.Collections.Generic;

#nullable disable

namespace De2_ontap.Models
{
    public partial class NhomHang
    {
        public NhomHang()
        {
            SanPhams = new HashSet<SanPham>();
        }

        public int MaNhomHang { get; set; }
        public string TenNhomHang { get; set; }

        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
