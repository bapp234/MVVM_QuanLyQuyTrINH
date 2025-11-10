using MVVM_QuanLyQuyTrINH.Models.Project;
using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models.Account
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            CongViecs = new HashSet<CongViec>();
            DuAns = new HashSet<DuAn>();
            LichSuCongViecs = new HashSet<LichSuCongViec>();
            PhanCongs = new HashSet<PhanCong>();
            ThongBaos = new HashSet<ThongBao>();
            ThongKeCvs = new HashSet<ThongKeCv>();
        }

        public int MaNv { get; set; }
        public string? MaPb { get; set; }

        public virtual User MaNvNavigation { get; set; } = null!;
        public virtual ICollection<CongViec> CongViecs { get; set; }
        public virtual ICollection<DuAn> DuAns { get; set; }
        public virtual ICollection<LichSuCongViec> LichSuCongViecs { get; set; }
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
        public virtual ICollection<ThongBao> ThongBaos { get; set; }
        public virtual ICollection<ThongKeCv> ThongKeCvs { get; set; }
    }
}
