using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Models.Context;
namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    /// <summary>
    /// Interaction logic for BaoCao.xaml
    /// </summary>
    public partial class BaoCao : Page
    {
        private readonly QLQuyTrinhLamViecContext db = new QLQuyTrinhLamViecContext();

        // Constructor: lấy báo cáo theo MaBaoCao
        public BaoCao()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // Danh sách báo cáo từng dự án (tính toán từ CongViec)
            var baoCaoDuAns = db.DuAns.Select(duAn => new BaoCaoDuAn
            {
                MaDuAn = duAn.MaDuAn,
                TenDuAn = duAn.TenDuAn,
                TongCv = db.CongViecs.Count(c => c.MaDuAn == duAn.MaDuAn),
                CvhoanThanh = db.CongViecs.Count(c => c.MaDuAn == duAn.MaDuAn && c.TrangThai == "Hoàn thành"),
                CvdangLam = db.CongViecs.Count(c => c.MaDuAn == duAn.MaDuAn && c.TrangThai == "Đang làm"),
                CvtreHan = db.CongViecs.Count(c => c.MaDuAn == duAn.MaDuAn && c.TrangThai == "Trễ hạn"),
                TyLeHoanThanh = db.CongViecs.Any(c => c.MaDuAn == duAn.MaDuAn)
                                  ? (double?)db.CongViecs.Count(c => c.MaDuAn == duAn.MaDuAn && c.TrangThai == "Hoàn thành") /
                                    db.CongViecs.Count(c => c.MaDuAn == duAn.MaDuAn)
                                  : 0,
                NgayBaoCao = db.CongViecs.Where(c => c.MaDuAn == duAn.MaDuAn).OrderByDescending(c => c.NgayGiao).Select(c => c.NgayGiao).FirstOrDefault()
            }).ToList();

            // Tổng quan
            var tongSoDuAn = db.DuAns.Count();
            var tongSoNhanVien = db.NhanViens.Count();
            var tongCv = db.CongViecs.Count();
            var tongCvHoanThanh = db.CongViecs.Count(c => c.TrangThai == "Hoàn thành");
            var tongCvDangLam = db.CongViecs.Count(c => c.TrangThai == "Đang làm");
            var tongCvTreHan = db.CongViecs.Count(c => c.TrangThai == "Trễ hạn");

            // Gán DataContext
            this.DataContext = new
            {
                TongSoDuAn = tongSoDuAn,
                TongSoNhanVien = tongSoNhanVien,
                TongCv = tongCv,
                TongCvHoanThanh = tongCvHoanThanh,
                TongCvDangLam = tongCvDangLam,
                TongCvTreHan = tongCvTreHan,
                BaoCaoDuAns = baoCaoDuAns
            };
        }
    }
}
