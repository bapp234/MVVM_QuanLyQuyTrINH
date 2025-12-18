using MVVM_QuanLyQuyTrINH.Models.Project;
using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models.Account
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            LichSuCongViecs = new HashSet<LichSuCongViec>();
            PhanCongs = new HashSet<PhanCong>();
            ThongBaos = new HashSet<ThongBao>();
            ThongKeCvs = new HashSet<ThongKeCv>();
        }

        public int MaNv { get; set; }
        public string? MaPb { get; set; }

        public virtual User MaNvNavigation { get; set; } = null!;
        public virtual ICollection<LichSuCongViec> LichSuCongViecs { get; set; }
        public virtual ICollection<PhanCong> PhanCongs { get; set; }
        public virtual ICollection<ThongBao> ThongBaos { get; set; }
        public virtual ICollection<ThongKeCv> ThongKeCvs { get; set; }

    }
}
