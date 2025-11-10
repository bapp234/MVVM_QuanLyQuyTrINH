using MVVM_QuanLyQuyTrINH.Models.Account;
using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models.Project
{
    public partial class CongViec
    {
        public CongViec()
        {
            LichSuCongViecs = new HashSet<LichSuCongViec>();
            PhanCongs = new HashSet<PhanCong>();
        }

        public int MaCv { get; set; }
        public string TenCv { get; set; } = null!;
        public string? MoTa { get; set; }
        public DateTime? NgayGiao { get; set; }
        public DateTime? HanHoanThanh { get; set; }
        public string? TrangThai { get; set; }
        public int MaDuAn { get; set; }
        public int? MaNvphuTrach { get; set; }
        public string? DoUuTien { get; set; }

        public virtual DuAn MaDuAnNavigation { get; set; } = null!;
        public virtual NhanVien? MaNvphuTrachNavigation { get; set; }
        public virtual ICollection<LichSuCongViec> LichSuCongViecs { get; set; }
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
    }
}
