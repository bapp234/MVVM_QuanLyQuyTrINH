using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    /// <summary>
    /// Interaction logic for PhanQuyen.xaml
    /// </summary>
    public partial class PhanQuyen : Page
    {
        public List<VaiTro> VaiTroList { get; set; }
        public PhanQuyen()
        {
            InitializeComponent();
            LoadData();
            this.DataContext = this;
      
        }
        private void LoadData()
        {
            using (var db = new QLQuyTrinhLamViecContext())
            {
                // Load danh sách vai trò
                VaiTroList = db.VaiTros
                .GroupBy(v => new { v.MaVaiTro, v.TenVaiTro })
                .Select(g => g.First())
                .ToList();

                var dsNhanVien = db.Users
                                   .Include(u => u.MaVaiTroNavigation)
                                   .ToList();

                PermissionList.ItemsSource = dsNhanVien;
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        private void btnCapQuyen_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Chức năng cấp quyền đang được phát triển.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
