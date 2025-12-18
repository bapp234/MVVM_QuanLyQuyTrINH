using MVVM_QuanLyQuyTrINH.Models.Account;
using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models.Project
{
    public partial class ThongBao
    {
        public int MaTb { get; set; }
        public int MaNv { get; set; }
        public string? TieuDe { get; set; }
        public string? NoiDung { get; set; }
        public DateTime? NgayGui { get; set; }
        public string? TrangThai { get; set; }

        public virtual NhanVien MaNvNavigation { get; set; } = null!;
    }
}
