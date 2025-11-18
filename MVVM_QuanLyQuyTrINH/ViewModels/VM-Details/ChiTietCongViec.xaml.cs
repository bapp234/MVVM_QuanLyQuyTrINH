using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using System.Linq;
using System.Windows;

namespace MVVM_QuanLyQuyTrINH.Views.Details
{
    public partial class ChiTietCongViec : Window
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();

        public CongViec CongViec { get; set; }

        // Tên nhân viên phụ trách công việc
        public string TenNhanVienPhuTrach { get; set; }

        // Danh sách lịch sử công việc với thông tin nhân viên
        public IQueryable<object> LichSuCongViec { get; set; }

        public ChiTietCongViec(CongViec congViec)
        {
            InitializeComponent();

            // Load chi tiết công việc cùng nhân viên phụ trách và lịch sử công việc + nhân viên cập nhật
            CongViec = _context.CongViecs
                               .Include(c => c.MaNvphuTrachNavigation)
                                   .ThenInclude(nv => nv.MaNvNavigation) // để lấy User
                               .Include(c => c.LichSuCongViecs)
                                   .ThenInclude(ls => ls.MaNvNavigation)
                                       .ThenInclude(nv => nv.MaNvNavigation)
                               .FirstOrDefault(c => c.MaCv == congViec.MaCv);

            if (CongViec == null)
            {
                MessageBox.Show("Không tìm thấy công việc.");
                Close();
                return;
            }

            // Lấy tên nhân viên phụ trách
            TenNhanVienPhuTrach = CongViec.MaNvphuTrachNavigation?.MaNvNavigation?.HoTen
                                   ?? "Chưa có nhân viên phụ trách";

            // Lấy danh sách lịch sử công việc
            LichSuCongViec = CongViec.LichSuCongViecs
                .OrderByDescending(ls => ls.NgayCapNhat)
                .Select(ls => new
                {
                    ls.NgayCapNhat,
                    ls.NoiDung,
                    TenNhanVien = ls.MaNvNavigation?.MaNvNavigation?.HoTen ?? "Không xác định"
                }).AsQueryable();

            // Binding dữ liệu cho XAML
            this.DataContext = this;
            ListViewLichSu.ItemsSource = LichSuCongViec.ToList();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng sửa đang được phát triển.");
        }
    }
}
