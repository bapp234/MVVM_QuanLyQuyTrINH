    using BCrypt.Net;
    using Microsoft.EntityFrameworkCore;
    using MVVM_QuanLyQuyTrINH.Models.Account;
    using MVVM_QuanLyQuyTrINH.Models.Context;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
using System.Windows;

    namespace MVVM_QuanLyQuyTrINH.Services
    {
        public class AuthService
        {
            private readonly QLQuyTrinhLamViecContext dB_User;

            public AuthService()
            {
                dB_User = new QLQuyTrinhLamViecContext();
            }
            public async Task<User> Login(string username, string password)
            {
            try
            {
                var user = await dB_User.Users
                       .Include(u=> u.MaVaiTroNavigation)
                       .Include(u=> u.Admin)
                       .Include(u=> u.NhanVien)
                       .SingleOrDefaultAsync(u => u.TenDangNhap == username);

                if (user == null)
                {
                    return null;
                }

                string dbPass = user.MatKhau?.Trim();
                bool isCorrect = BCrypt.Net.BCrypt.Verify(password, dbPass);

                if (isCorrect) return user;
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
       
        }
    }
