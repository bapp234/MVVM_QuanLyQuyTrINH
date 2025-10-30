using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models
{
    public partial class PhanCong
    {
        public int MaPhanCong { get; set; }
        public int MaCv { get; set; }
        public int MaNv { get; set; }
        public string? VaiTroTrongCv { get; set; }

        public virtual CongViec MaCvNavigation { get; set; } = null!;
        public virtual NhanVien MaNvNavigation { get; set; } = null!;
    }
}
