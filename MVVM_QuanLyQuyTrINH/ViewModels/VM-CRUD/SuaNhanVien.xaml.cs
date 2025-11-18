using MVVM_QuanLyQuyTrINH.Models.Context;
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
using System.Windows.Shapes;
using MVVM_QuanLyQuyTrINH.Models.Account;

namespace MVVM_QuanLyQuyTrINH.Views.CRUD
{
    /// <summary>
    /// Interaction logic for SuaNhanVien.xaml
    /// </summary>
    public partial class SuaNhanVien : Window
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        public SuaNhanVien(User nv)
        {
            InitializeComponent();
            this.DataContext = nv;
            //using (var _context = new QLQuyTrinhLamViecContext())
            //{
            //    if (nv.NhanVien != null)
            //    {
            //        // Lấy danh sách công việc mà nhân viên này phụ trách
            //        var congViecs = _context.CongViecs
            //                                .Where(c => c.MaNvphuTrach == nv.NhanVien.MaNv)
            //                                .Select(c => new { c.TenCv, c.TrangThai })
            //                                .ToList();

            //        ListViewCongViecs.ItemsSource = congViecs;
            //    }
            //}
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Lưu logic ở đây
            MessageBox.Show("Nhân viên đã được sửa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
    }
}
