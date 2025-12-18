using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    /// <summary>
    /// Interaction logic for TrangChu.xaml
    /// </summary>
    public partial class TrangChu : Page
    {
        private readonly QLQuyTrinhLamViecContext db = new QLQuyTrinhLamViecContext();

        public ObservableCollection<ThongBao> DanhSachThongBao { get; set; }
        public ObservableCollection<LichSuCongViec> DanhSachLichSu { get; set; }
        public ObservableCollection<QuyTrinh> DanhSachQuyTrinhDangLam { get; set; }
        public ObservableCollection<DuAn> DanhSachDuAnMoiNhat { get; set; }
        public TrangChu()
        {
            InitializeComponent();
            this.DataContext = this;

            LoadThongBao();
            LoadLichSuCongViec();
            LoadQuyTrinhDangLam();
            LoadDuAnMoiNhat();
        }
        private void LoadThongBao()
        {
            DanhSachThongBao = new ObservableCollection<ThongBao>(
                db.ThongBaos
                  .OrderByDescending(tb => tb.NgayGui)
                  .Take(5)
                  .ToList()
            );
        }

        private void LoadLichSuCongViec()
        {
            DanhSachLichSu = new ObservableCollection<LichSuCongViec>(
                db.LichSuCongViecs
                  .OrderByDescending(ls => ls.NgayCapNhat)
                  .Take(10)
                  .ToList()
            );
        }

        private void LoadQuyTrinhDangLam()
        {
            return;
        }

        private void LoadDuAnMoiNhat()
        {
            DanhSachDuAnMoiNhat = new ObservableCollection<DuAn>(
                    db.DuAns
                    .Include(d => d.MaTruongNhomNavigation)
                        .ThenInclude(ql => ql.MaQlNavigation) 
                    .Include(d => d.QuyTrinhs)
                    .Include(d => d.CongViecs) 
                    .OrderByDescending(d => d.NgayBatDau)
                    .Take(5)
                    .ToList()
                );
        }
    }
}
