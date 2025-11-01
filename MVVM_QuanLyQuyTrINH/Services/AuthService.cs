using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var user = await dB_User.Users.AsNoTracking().SingleOrDefaultAsync(u => u.TenDangNhap == username);
            if (user != null /*&& BCrypt.Net.BCrypt.Verify(password, user.MatKhau)*/)
            {
                return user;
            }
            return null;
        }
       
    }
}
