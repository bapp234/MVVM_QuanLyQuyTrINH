using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models.Project
{
    public partial class BaoCaoDuAn
    {
        public int MaBaoCao { get; set; }
        public int MaDuAn { get; set; }
        public string? TenDuAn { get; set; }
        public int? TongCv { get; set; }
        public int? CvhoanThanh { get; set; }
        public int? CvdangLam { get; set; }
        public int? CvtreHan { get; set; }
        public double? TyLeHoanThanh { get; set; }
        public DateTime? NgayBaoCao { get; set; }

        public virtual DuAn MaDuAnNavigation { get; set; } = null!;
    }
}
