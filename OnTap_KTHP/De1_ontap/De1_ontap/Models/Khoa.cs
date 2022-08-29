using System;
using System.Collections.Generic;

#nullable disable

namespace De1_ontap.Models
{
    public partial class Khoa
    {
        public Khoa()
        {
            BenhNhans = new HashSet<BenhNhan>();
        }

        public int MaKhoa { get; set; }
        public string BenhNhan { get; set; }

        public virtual ICollection<BenhNhan> BenhNhans { get; set; }
    }
}
