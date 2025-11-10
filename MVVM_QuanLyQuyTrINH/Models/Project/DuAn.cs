using MVVM_QuanLyQuyTrINH.Models.Account;
using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models.Project
{
    public partial class DuAn
    {
        public DuAn()
        {
            BaoCaoDuAns = new HashSet<BaoCaoDuAn>();
            CongViecs = new HashSet<CongViec>();
            QuyTrinhs = new HashSet<QuyTrinh>();
            ThongKeCvs = new HashSet<ThongKeCv>();
        }

        public int MaDuAn { get; set; }
        public string TenDuAn { get; set; } = null!;
        public string? MoTa { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }
        public int? MaTruongNhom { get; set; }
        public string? TrangThai { get; set; }

        public virtual NhanVien? MaTruongNhomNavigation { get; set; }
        public virtual ICollection<BaoCaoDuAn> BaoCaoDuAns { get; set; }
        public virtual ICollection<CongViec> CongViecs { get; set; }
        public virtual ICollection<QuyTrinh> QuyTrinhs { get; set; }
        public virtual ICollection<ThongKeCv> ThongKeCvs { get; set; }
    }
}
