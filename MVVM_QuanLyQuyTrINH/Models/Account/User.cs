using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using BCrypt.Net;

namespace MVVM_QuanLyQuyTrINH.Models.Account
{
    public partial class User
    {
        public int UserId { get; set; }
        public string TenDangNhap { get; set; } = null!;
        public string MatKhau { get; set; } = null!;
        public string? HoTen { get; set; }
        public string? Email { get; set; }
        public int MaVaiTro { get; set; }

        public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;
        public virtual Admin? Admin { get; set; }
        public virtual NhanVien? NhanVien { get; set; }
        public virtual QuanLy? QuanLy { get; set; }

        public void SetPassword(string password)
        {
            MatKhau = BCrypt.Net.BCrypt.HashPassword(password);
        }
        [NotMapped]
        public string RoleName
        {
            get
            {
                return MaVaiTroNavigation?.TenVaiTro ?? "Unknown";
            }
        }

        [NotMapped]
        public string idToString
        {
            get
            {
                string fsID = "NV";
                if(MaVaiTro==1) fsID="AD";
                else if(MaVaiTro==2) fsID = "QL";
                return $"{fsID}{UserId.ToString("D4")}";
            }
        }
    }
}
