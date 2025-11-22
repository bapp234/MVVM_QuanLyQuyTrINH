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
        public string TenNhanVienPhuTrach { get; set; }
        public IQueryable<object> LichSuCongViec { get; set; }
        public ChiTietCongViec(CongViec congViec)
        {
            InitializeComponent();

            CongViec = _context.CongViecs
                               .Include(c => c.MaNvphuTrachNavigation)
                                   .ThenInclude(nv => nv.MaNvNavigation)
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

            TenNhanVienPhuTrach = CongViec.MaNvphuTrachNavigation?.MaNvNavigation?.HoTen
                                   ?? "Chưa có nhân viên phụ trách";

            LichSuCongViec = CongViec.LichSuCongViecs
                .OrderByDescending(ls => ls.NgayCapNhat)
                .Select(ls => new
                {
                    ls.NgayCapNhat,
                    ls.NoiDung,
                    TenNhanVien = ls.MaNvNavigation?.MaNvNavigation?.HoTen ?? "Không xác định"
                }).AsQueryable();

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
