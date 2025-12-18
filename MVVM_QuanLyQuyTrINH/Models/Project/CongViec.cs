using MVVM_QuanLyQuyTrINH.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

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
        public string? DoUuTien { get; set; }

        public virtual DuAn MaDuAnNavigation { get; set; } = null!;
        public virtual ICollection<LichSuCongViec> LichSuCongViecs { get; set; }
        public virtual ICollection<PhanCong> PhanCongs { get; set; }

        [NotMapped]
        public string maHienThi
        {
            get
            {
                return $"{MaCv:D3}";
            }
        }

        [NotMapped]
        public string tenDuAn => MaDuAnNavigation?.TenDuAn ?? "Chưa có dự án";
        [NotMapped]
        public string trangThaiHienThi
        {
            get
            {
                if (HanHoanThanh.HasValue && HanHoanThanh.Value.Date < DateTime.Now.Date
                    && TrangThai != "Hoàn thành"/* && TrangThai != "Đã hủy"*/)
                {
                    return "Trễ hạn";
                }
                return TrangThai;
            }
        }
        [NotMapped]
        public string mauTrangThai
        {
            get
            {
                    switch (trangThaiHienThi)
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
        [NotMapped]
        public string mauDoUuTien
        {
            get
            {
                string level = DoUuTien?.Trim().ToLower() ?? "";
                switch (level)
                {
                   
                    case "rất cao":
                        return "#D32F2F"; // Đỏ đậm

                    case "cao":
                        return "#F57C00"; // Cam đậm

                    case "trung bình":
                        return "#FBC02D"; // Vàng đậm

                    case "thấp":
                        return "#388E3C"; // Xanh lá đậm

                    default:
                        return "#FF9800"; // Màu mặc định (Cam) nếu không khớp
                }
            }
        }
        [NotMapped]
        public string MauNenDoUuTien
        {
            get
            {
                string level = DoUuTien?.Trim().ToLower() ?? "";

                switch (level)
                {
                    case "rất cao":
                        return "#FFEBEE"; // Hồng nhạt

                    case "cao":
                        return "#FFF3E0"; // Cam nhạt

                    case "trung bình":
                        return "#FFFDE7"; // Vàng nhạt

                    case "thấp":
                        return "#E8F5E9"; // Xanh nhạt

                    default:
                        return "#FFF3E0"; // Mặc định (Cam nhạt)
                }
            }
        }
    }

}
