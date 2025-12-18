using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    public partial class ThongTinCaNhan : Page
    {
        private User _user;

        public ThongTinCaNhan(User user)
        {
            InitializeComponent();

            // 🔹 Tải lại user từ DB, kèm VaiTro
            using (var db = new QLQuyTrinhLamViecContext())
            {
                _user = db.Users
                    .Include(u => u.MaVaiTroNavigation)
                    .FirstOrDefault(u => u.UserId == user.UserId)
                    ?? user; // fallback nếu không tìm thấy
            }

            DataContext = _user;
        }

        private void BtnLuu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new QLQuyTrinhLamViecContext())
                {
                    var userInDb = db.Users.FirstOrDefault(u => u.UserId == _user.UserId);
                    if (userInDb != null)
                    {
                        userInDb.HoTen = _user.HoTen;
                        userInDb.Email = _user.Email;
                        db.SaveChanges();
                        MessageBox.Show("✅ Lưu thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("❌ Không tìm thấy người dùng trong cơ sở dữ liệu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
