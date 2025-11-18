using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Views;
using MVVM_QuanLyQuyTrINH.Views.Details;
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
using MVVM_QuanLyQuyTrINH.Views.Windows;
using MVVM_QuanLyQuyTrINH.Models.Account;
using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Views.CRUD;
namespace MVVM_QuanLyQuyTrINH.Views.Pages;


    /// <summary>
    /// Interaction logic for QuanLyNhanVien.xaml
    /// </summary>
    public partial class QuanLyNhanVien : Page
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        private List<User> _user;
        public QuanLyNhanVien()
        {
            InitializeComponent();
            LoadData();
        }
        private void LoadData()
        {
            try
            {
                _user = _context.Users
                    .Include(u => u.MaVaiTroNavigation)
                    .Include(u => u.NhanVien)
                    .Where(u => u.NhanVien != null)
                    .ToList();

                EmployeeList.ItemsSource = _user;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            //string keyword = SearchTextBox.Text?.Trim().ToLower() ?? "";
            //string selectedStatus = (cbFilter.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tất cả";

            //var filtered = _allQuyTrinh.Where(p =>
            //(
            //    string.IsNullOrWhiteSpace(keyword)
            //    || p.TenDuAn.ToLower().Contains(keyword)
            //    || (p.MoTa != null && p.MoTa.ToLower().Contains(keyword))
            //    || p.MaDuAn.ToString().Contains(keyword)
            //)
            //&&
            //(
            //    selectedStatus == "Tất cả"
            //    || p.TrangThai == selectedStatus
            //)).ToList();

            //ProcessList.ItemsSource = filtered;
            return;
        }
        private void EmployeeCard_Click(object sender, MouseButtonEventArgs e)
        {
            //var border = sender as Border;
            //if (border == null) return;

            //var duAn = border.DataContext as DuAn;
            //if (duAn == null) return;

            //// Lấy danh sách bước từ database
            //List<Models.Project.QuyTrinh> danhSachBuoc = _context.QuyTrinhs
            //                                        .Where(q => q.MaDuAn == duAn.MaDuAn)
            //                                        .ToList();

            //ChiTietQuyTrinh popup = new ChiTietQuyTrinh(duAn, danhSachBuoc);
            //popup.ShowDialog();
            return;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại trang quản lý
            NavigationService.GoBack();
        }
        private void ViewEmployee_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var nhanVien = button.DataContext as User;
            if (nhanVien == null) return;

            // Mở popup chi tiết nhân viên
            ChiTietNhanVien popup = new ChiTietNhanVien(nhanVien);
            popup.ShowDialog();
        }
        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            ThemNhanVien popup = new ThemNhanVien();
            bool? result = popup.ShowDialog();

            if (result == true)
            {
                LoadData(); // Reload danh sách dự án
            }
        }
        private void EditEmployee_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var nhanVien = button.DataContext as User;
            if (nhanVien == null) return;

            // Mở popup chi tiết nhân viên
            SuaNhanVien popup = new SuaNhanVien(nhanVien);
            popup.ShowDialog();
        }
        private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bạn có muốn xóa nhân viên này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        }
    }
