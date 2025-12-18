using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Services;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                     .Include(cv => cv.PhanCongs)
                         .ThenInclude(pc => pc.MaNvNavigation) 
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

            DataContext = this;
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
                var dsChon = lbNhanVien.SelectedItems.Cast<NhanVien>().ToList();
                var dsPhanCong = _context.PhanCongs.Where(pc => pc.MaCv == CongViec.MaCv);
                _context.PhanCongs.RemoveRange(dsPhanCong);
                foreach (var nv in dsChon)
                {
                    _context.PhanCongs.Add(new PhanCong
                    {
                        MaCv = CongViec.MaCv,
                        MaNv = nv.MaNv
                    });
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
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var dsThamGia = _context.PhanCongs
       .Where(pc => pc.MaCv == CongViec.MaCv)
       .Select(pc => pc.MaNv)
       .ToHashSet();

            foreach (var nv in DanhSachNhanVien)
            {
                if (dsThamGia.Contains(nv.MaNv))
                    lbNhanVien.SelectedItems.Add(nv);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
