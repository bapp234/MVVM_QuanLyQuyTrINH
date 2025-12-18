using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Models.Account;
using System.Linq;
using System.Windows;

namespace MVVM_QuanLyQuyTrINH.Views.Windows
{
    public partial class SuaCongViec : Window
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();

        public CongViec CongViec { get; set; }
        public IQueryable<NhanVien> DanhSachNhanVien { get; set; }

        public SuaCongViec(int maCv)
        {
            InitializeComponent();

            // Load công việc cùng nhân viên phụ trách
            CongViec = _context.CongViecs
                               .Include(c => c.MaNvphuTrachNavigation)
                               .ThenInclude(nv => nv.MaNvNavigation) // để lấy User.HoTen
                               .FirstOrDefault(c => c.MaCv == maCv);

            if (CongViec == null)
            {
                MessageBox.Show("Không tìm thấy công việc.");
                Close();
                return;
            }

            // Load danh sách nhân viên để chọn
            DanhSachNhanVien = _context.NhanViens
                                      .Include(nv => nv.MaNvNavigation) // User
                                      .AsQueryable();

            this.DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _context.CongViecs.Update(CongViec);
                _context.SaveChanges();
                MessageBox.Show("Cập nhật công việc thành công!");
                Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
