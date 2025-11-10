using System;
using System.Collections.Generic;
using BCrypt.Net;
using MVVM_QuanLyQuyTrINH.Models.Project;


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
    }
}
