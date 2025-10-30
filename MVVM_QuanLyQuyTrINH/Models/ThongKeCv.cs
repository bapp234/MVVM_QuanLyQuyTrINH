using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models
{
    public partial class ThongKeCv
    {
        public int MaThongKe { get; set; }
        public int MaNv { get; set; }
        public int MaDuAn { get; set; }
        public int? SoCvdanglam { get; set; }
        public int? SoCvhoanThanh { get; set; }
        public int? SoCvtrehan { get; set; }
        public DateTime? NgayThongKe { get; set; }

        public virtual DuAn MaDuAnNavigation { get; set; } = null!;
        public virtual NhanVien MaNvNavigation { get; set; } = null!;
    }
}
