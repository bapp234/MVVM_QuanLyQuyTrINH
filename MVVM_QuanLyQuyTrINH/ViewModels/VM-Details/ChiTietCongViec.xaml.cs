using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Services;
using MVVM_QuanLyQuyTrINH.Views.Windows;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Windows;

namespace MVVM_QuanLyQuyTrINH.Views.Details
{
    public partial class ChiTietCongViec : Window
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        private readonly WorkService dbWork=new WorkService();
        public string TenTruongNhom { get; set; }
        public List<string> DanhSachThamGia { get; set; }
        public CongViec CongViec { get; set; }
        public string TenNhanVienPhuTrach { get; set; }
        public IQueryable<object> LichSuCongViec { get; set; }
        private readonly int maCV;
        public ChiTietCongViec(CongViec congViec)
        {
            InitializeComponent();
            CongViec = dbWork.GetChiTietCongViec(congViec.MaCv);

            if (CongViec == null)
            {
                MessageBox.Show("Không tìm thấy công việc.");
                Close();
                return;
            }
            TenTruongNhom = dbWork.GetTenTruongNhom(CongViec);
            DanhSachThamGia = dbWork.GetDanhSachNhanVienThamGia(CongViec);
            LoadLichSuCongViec();
            ApplyDataContext();
        }
        private void LoadLichSuCongViec()
        {
            LichSuCongViec = CongViec.LichSuCongViecs
                .OrderByDescending(ls => ls.NgayCapNhat)
                .Select(ls => new
                {
                    ls.NgayCapNhat,
                    ls.NoiDung,
                    TenNhanVien = ls.MaNvNavigation?.MaNvNavigation?.HoTen ?? "Không xác định"
                })
                .AsQueryable();

            ListViewLichSu.ItemsSource = LichSuCongViec.ToList();
        }
        private void ApplyDataContext()
        {
            this.DataContext = this;
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
