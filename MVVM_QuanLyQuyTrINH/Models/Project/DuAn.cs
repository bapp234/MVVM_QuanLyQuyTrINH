using MVVM_QuanLyQuyTrINH.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public string MaHienThi => $"QT{MaDuAn:D3}";
        [NotMapped]
        public string TenTruongNhom => MaTruongNhomNavigation?.MaNvNavigation?.HoTen ?? "Chưa có trưởng nhóm";
        [NotMapped]
        public int SoLuongCongViec => CongViecs?.Count ?? 0;
        [NotMapped]
        public int SoLuongBuoc => QuyTrinhs?.Count ?? 0;
        [NotMapped]
        public string MauTrangThai
        {
            get
            {
                switch (TrangThai)
                {
                    case "Mới tạo": return "#90A4AE";
                    case "Đang thực hiện": return "#2196F3";
                    case "Hoàn thành": return "#2ECC71";
                    case "Tạm dừng": return "#F1C40F";
                    case "Trễ hạn": return "#E74C3C";
                    //case "Đã hủy": return "#616161";
                    default: return "#90A4AE";
                }
            }
        }
    }
}
