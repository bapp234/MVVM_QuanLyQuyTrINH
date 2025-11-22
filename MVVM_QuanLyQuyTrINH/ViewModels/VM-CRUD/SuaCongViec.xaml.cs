using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Models.Account;
using System.Linq;
using System.Windows;
using MVVM_QuanLyQuyTrINH.Services;

namespace MVVM_QuanLyQuyTrINH.Views.Windows
{
    public partial class SuaCongViec : Window
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        private readonly WorkService _workService = new WorkService();

        public CongViec CongViec { get; set; }
        public List<NhanVien> DanhSachNhanVien { get; set; }
        public SuaCongViec(int maCv)
        {
            InitializeComponent();
            LoadCongViec(maCv);

        }
        private void LoadCongViec(int maCv)
        {
         CongViec = _context.CongViecs
                .Include(cv => cv.MaDuAnNavigation)
                .Include(cv => cv.MaNvphuTrachNavigation)
                    .ThenInclude(nv => nv.MaNvNavigation)
                .FirstOrDefault(cv => cv.MaCv == maCv);
            if (CongViec == null)
            {
                MessageBox.Show("Không tìm thấy công việc này (Có thể đã bị xóa).");
                this.Close();
                return;
            }
            DanhSachNhanVien = _context.NhanViens
                .Include(nv => nv.MaNvNavigation)
                .ToList();
            this.DataContext = this;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CongViec.TenCv))
                {
                    MessageBox.Show("Tên công việc không được để trống!");
                    return;
                }
                _context.CongViecs.Update(CongViec);
                _context.SaveChanges();
                MessageBox.Show("Cập nhật công việc thành công!");
                this.DialogResult = true;
                this.Close();
            }
            catch (System.Exception ex)
            {

                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
