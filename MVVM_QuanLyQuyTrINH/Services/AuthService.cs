using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
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
        public User Login(string username, string password)
        {
            var user = dB_User.Users.SingleOrDefault(u => u.TenDangNhap == username);
            if (user != null /*&& BCrypt.Net.BCrypt.Verify(password, user.MatKhau)*/)
            {
                return user;
            }
            return null;
        }
       
    }
}
