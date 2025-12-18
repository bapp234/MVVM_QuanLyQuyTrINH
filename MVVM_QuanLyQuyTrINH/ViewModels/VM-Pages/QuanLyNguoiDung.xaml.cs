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
using MVVM_QuanLyQuyTrINH.Services;
namespace MVVM_QuanLyQuyTrINH.Views.Pages;


/// <summary>
/// Interaction logic for QuanLyNhanVien.xaml
/// </summary>
public partial class QuanLyNguoiDung : Page
{
    private readonly UserService _userService = new UserService();
    private List<User> _user;
    public QuanLyNguoiDung()
    {
        InitializeComponent();
        LoadData();
    }
    private void LoadData()
    {
        try
        {
            _user = _userService.GetAllUsers();

            EmployeeList.ItemsSource = _user;
        }
        catch (System.Exception ex)
        {

            MessageBox.Show("Không có dữ liệu: " + ex.Message);
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
        if (NavigationService.CanGoBack) NavigationService.GoBack();
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
            LoadData();
        }
    }
    private void EditEmployee_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button == null) return;

        var nhanVien = button.DataContext as User;
        if (nhanVien == null) return;

        SuaNhanVien popup = new SuaNhanVien(nhanVien);
        bool? result = popup.ShowDialog();
        if (true)
        {
            LoadData();
        }
    }
    private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        var user = button.DataContext as User;
        if (user == null) return;
        if (UserSession.CurrentUser != null && user.UserId == UserSession.CurrentUser.UserId)
        {
            MessageBox.Show("Bạn không thể xóa tài khoản đang được sử dụng để đăng nhập!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        var activeTasks = _userService.GetActiveTaskNames(user.UserId);

        if (activeTasks.Count > 0)
        {
            string message = $"Nhân viên '{user.HoTen}' đang phụ trách {activeTasks.Count} công việc chưa xong.\n" +
                             "Bạn cần gỡ hết các phân công này trước khi xóa.\n\n" +
                             "Bạn có muốn mở cửa sổ 'Gỡ Phân Công' ngay bây giờ không?";

            var confirmOpenTool = MessageBox.Show(message, "Vướng công việc", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (confirmOpenTool == MessageBoxResult.Yes)
            {
                GoCongViec taskWindow = new GoCongViec(user.UserId);
                bool? dialogResult = taskWindow.ShowDialog();
                if (dialogResult == true)
                {
                    var remainingTasks = _userService.GetActiveTaskNames(user.UserId);
                    if (remainingTasks.Count > 0)
                    {
                        MessageBox.Show("Vẫn còn công việc chưa được gỡ. Không thể xóa nhân viên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
        var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa tài khoản '{user.HoTen}' không?\nHành động này không thể hoàn tác.",
                                     "Xác nhận xóa",
                                     MessageBoxButton.YesNo,
                                     MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            bool isDeleted = _userService.DeleteUser(user.UserId);

            if (isDeleted)
            {
                LoadData();
            }
            else
            {
                MessageBox.Show("Có lỗi xảy ra, không thể xóa nhân viên này!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
