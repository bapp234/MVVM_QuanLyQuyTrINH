using Microsoft.EntityFrameworkCore;
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

namespace MVVM_QuanLyQuyTrINH.Views.Windows
{
    /// <summary>
    /// Interaction logic for ThemDuAn.xaml
    /// </summary>
    public partial class ThemDuAn : Window
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        public ThemDuAn()
        {
            InitializeComponent();
            LoadNhanVien();
        }
        private void LoadNhanVien()
        {
            // Lấy danh sách nhân viên + thông tin user
            var dsNhanVien = _context.NhanViens
                .Include(nv => nv.MaNvNavigation)
                .ToList();

            cbTruongNhom.ItemsSource = dsNhanVien;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Lưu logic ở đây
            MessageBox.Show("Dự án đã được thêm thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
