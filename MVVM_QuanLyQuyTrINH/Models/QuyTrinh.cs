using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models
{
    public partial class QuyTrinh
    {
        public int MaBuoc { get; set; }
        public int MaDuAn { get; set; }
        public string? TenBuoc { get; set; }
        public int? ThuTu { get; set; }
        public string? Mota { get; set; }
        public string? TrangThai { get; set; }

        public virtual DuAn MaDuAnNavigation { get; set; } = null!;
    }
}
