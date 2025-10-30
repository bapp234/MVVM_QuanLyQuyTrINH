using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models
{
    public partial class LichSuCongViec
    {
        public int MaLichSu { get; set; }
        public int MaCv { get; set; }
        public int MaNv { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public string? NoiDung { get; set; }
        public string? TrangThaiMoi { get; set; }

        public virtual CongViec MaCvNavigation { get; set; } = null!;
        public virtual NhanVien MaNvNavigation { get; set; } = null!;
    }
}
