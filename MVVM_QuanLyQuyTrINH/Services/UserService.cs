using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM_QuanLyQuyTrINH.Models.MyModels;
using System.Windows;

namespace MVVM_QuanLyQuyTrINH.Services
{
    public class UserService
    {
        private readonly QLQuyTrinhLamViecContext db_context;
        
        public UserService()
        {
            db_context = new QLQuyTrinhLamViecContext();
        }
        public List<User> GetAllUsers()
        {
            return db_context.Users.Include(u => u.MaVaiTroNavigation)
                                   .Include(u => u.NhanVien)
                                   .ToList();
        }
        
        public bool AddUser(User user, string roleType, string Info)
        {
            using (var transaction = db_context.Database.BeginTransaction())
            {
                try
                {
                    if (db_context.Users.Any(u => u.TenDangNhap == user.TenDangNhap))
                        {
                        return false;
                    }
                    db_context.Users.Add(user);
                    db_context.SaveChanges();
                    switch (roleType)
                    {
                        case "Admin":
                            var admin = new Admin
                            {
                                MaAd = user.UserId,
                                QuyenAdmin = "Mọi quyền"
                            };
                            db_context.Admins.Add(admin);
                            break;
                        case "QuanLy":
                            var quanLy = new QuanLy
                            {
                                MaQl = user.UserId,
                                CapQuanLy = Info
                            };
                            db_context.QuanLies.Add(quanLy);
                            break;
                        default:
                            var nhanVien = new NhanVien
                            {
                                MaNv = user.UserId,
                                MaPb = Info
                            };
                            db_context.NhanViens.Add(nhanVien);
                            break;
                    }
                    db_context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.InnerException?.Message);
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public bool DeleteUser(int userId)
        {
            using (var transaction = db_context.Database.BeginTransaction())
            {
                try
                {
                    var user = db_context.Users
                        .Include(u => u.Admin)
                        .Include(u => u.QuanLy)
                        .Include(u => u.NhanVien)
                        .FirstOrDefault(u => u.UserId == userId);

                    if (user == null) return false;
                    if (user.NhanVien != null)
                    {
                        bool dangLamViec = db_context.PhanCongs
                        .Include(pc => pc.MaCvNavigation)
                        .Any(pc => pc.MaNv == userId &&
                               pc.MaCvNavigation.TrangThai != "Hoàn thành" &&
                               pc.MaCvNavigation.TrangThai != "Đã hủy");
                        if (dangLamViec) { 
                            throw new Exception($"Nhân viên {user.HoTen} đang phụ trách công việc chưa hoàn thành. Vui lòng chuyển giao công việc hoặc gỡ phân công trước khi xóa.");
                        }
                        var phanCongs = db_context.PhanCongs.Where(pc => pc.MaNv == userId).ToList();
                        if (phanCongs.Any()) db_context.PhanCongs.RemoveRange(phanCongs);

                        db_context.NhanViens.Remove(user.NhanVien);
                    }
                    if (user.QuanLy != null)
                    {
                        bool dangQuanLyDuAn = db_context.DuAns.Any(da => da.MaTruongNhom == userId);
                        if (dangQuanLyDuAn)
                        {
                            throw new Exception("Không thể xóa user này vì đang là Trưởng nhóm của một dự án. Hãy chuyển giao dự án trước khi xóa.");
                        }

                        db_context.QuanLies.Remove(user.QuanLy);
                    }
                    if (user.Admin != null)
                    {
                        db_context.Admins.Remove(user.Admin);
                    }
                    var yeuCauMK = db_context.YeuCauDatLaiMatKhau.Where(y => y.UserID == userId).ToList();
                    if (yeuCauMK.Any()) db_context.YeuCauDatLaiMatKhau.RemoveRange(yeuCauMK);
                    db_context.Users.Remove(user);
                    db_context.SaveChanges();
                    int maxID = 0;
                    if (db_context.Users.Any())
                    {
                        maxID = db_context.Users.Max(u => u.UserId);
                    }
                    string sqlResetIdentity = $"DBCC CHECKIDENT ('[User]', RESEED, {maxID})";
                    db_context.Database.ExecuteSqlRaw(sqlResetIdentity);

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    string errorMsg = ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMsg += "\nChi tiết: " + ex.InnerException.Message;
                    }
                    MessageBox.Show(errorMsg, "Lỗi xóa người dùng", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        public bool IsEmailExists(string email, int id)
        {
            return db_context.Users.Any(u => u.Email == email && u.UserId != id);
        }

        public bool IsUserLast(int id)
        {
            int count = db_context.Users.Count();
            bool isAdmin = db_context.Admins.Any(a => a.MaAd == id);
            if (isAdmin && count <= 1)
            {
                return true;
            }
            return false;
        }

        public bool UpdateUser(User userToUpdate, string newRoleType, string extraInfo, string newPassword = null)
        {
            using (var transaction = db_context.Database.BeginTransaction())
            {
                try
                {
                    var dbUser = db_context.Users
                                         .Include(u => u.NhanVien)
                                         .Include(u => u.QuanLy)
                                         .Include(u => u.Admin)
                                         .FirstOrDefault(u => u.UserId == userToUpdate.UserId);

                    if (dbUser == null) return false;

                    dbUser.HoTen = userToUpdate.HoTen;
                    dbUser.Email = userToUpdate.Email;
                    dbUser.MaVaiTro = userToUpdate.MaVaiTro; 

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        dbUser.MatKhau = BCrypt.Net.BCrypt.HashPassword(newPassword);
                    }

                    if (newRoleType != "NhanVien" && dbUser.NhanVien != null) db_context.NhanViens.Remove(dbUser.NhanVien);
                    if (newRoleType != "QuanLy" && dbUser.QuanLy != null) db_context.QuanLies.Remove(dbUser.QuanLy);
                    if (newRoleType != "Admin" && dbUser.Admin != null) db_context.Admins.Remove(dbUser.Admin);

                    db_context.SaveChanges();

                    switch (newRoleType)
                    {
                        case "Admin":
                            if (dbUser.Admin == null)
                                db_context.Admins.Add(new Admin { MaAd = dbUser.UserId, QuyenAdmin = "FullAccess" });
                            break;

                        case "QuanLy":
                            if (dbUser.QuanLy == null)
                                db_context.QuanLies.Add(new QuanLy { MaQl = dbUser.UserId, CapQuanLy = extraInfo });
                            else
                                dbUser.QuanLy.CapQuanLy = extraInfo; 
                            break;

                        default: 
                            if (dbUser.NhanVien == null)
                                db_context.NhanViens.Add(new NhanVien { MaNv = dbUser.UserId, MaPb = extraInfo });
                            else
                                dbUser.NhanVien.MaPb = extraInfo; 
                            break;
                    }

                    db_context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(
                        ex.Message + "\n\n" + ex.InnerException?.Message,
                        "Lỗi chi tiết",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return false;
                }
            }
        }
        public List<string> GetActiveTaskNames(int userId)
        {
            return db_context.PhanCongs
                .Where(pc => pc.MaNv == userId &&
                             pc.MaCvNavigation.TrangThai != "Hoàn thành" &&
                             pc.MaCvNavigation.TrangThai != "Đã hủy")
                .Select(pc => pc.MaCvNavigation.TenCv) 
                .ToList();
        }
        public List<CList_CongViec> DanhSachCongViecCanXoa(int maNv)
        {
            var list = db_context.PhanCongs
                .Where(pc => pc.MaNv == maNv)
                .Include(pc => pc.MaCvNavigation)
                    .ThenInclude(cv => cv.MaDuAnNavigation)
                .Where(pc => pc.MaCvNavigation.TrangThai != "Hoàn thành" && pc.MaCvNavigation.TrangThai != "Đã hủy")
                .Select(pc => new CList_CongViec
                {
                    MaCv = pc.MaCv,
                    TenCv = pc.MaCvNavigation.TenCv,
                    TenDuAn = pc.MaCvNavigation.MaDuAnNavigation != null
                              ? pc.MaCvNavigation.MaDuAnNavigation.TenDuAn
                              : "Công việc chưa phân dự án",

                    IsHoanThanh = pc.MaCvNavigation.TrangThai == "Hoàn thành",
                    IsSelected = false
                })
                .ToList();

            return list;
        }
        public bool RemovePhanCong(int maNv, List<int> maCongViecList)
        {
            using (var transaction = db_context.Database.BeginTransaction())
            {
                try
                {
                    var phanCongsToDelete = db_context.PhanCongs
                        .Where(pc => pc.MaNv == maNv && maCongViecList.Contains(pc.MaCv))
                        .ToList();

                    if (phanCongsToDelete.Any())
                    {
                        db_context.PhanCongs.RemoveRange(phanCongsToDelete);
                        db_context.SaveChanges();
                    }

                    transaction.Commit();
                    return true;
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public int CountActiveTasks(int userId)
        {
            var nhanVien = db_context.NhanViens
                            .Include(nv => nv.PhanCongs)             
                                .ThenInclude(pc => pc.MaCvNavigation) 
                            .FirstOrDefault(nv => nv.MaNv == userId);

            if (nhanVien == null || nhanVien.PhanCongs == null) return 0;
            return nhanVien.PhanCongs
                .Select(pc => pc.MaCvNavigation)
                .Count(cv => cv != null && cv.TrangThai != "Hoàn thành");
        }
    }
}
