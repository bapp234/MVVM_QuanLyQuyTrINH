using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Services;
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
    /// Interaction logic for ThemNhanVien.xaml
    /// </summary>
    public partial class ThemNhanVien : Window
    {
        private readonly UserService userService = new UserService();
        public ThemNhanVien()
        {
            InitializeComponent();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
        private void cbVaiTro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (spThongTinBoSung == null) return;

            var selectedItem = cbVaiTro.SelectedItem as ComboBoxItem;
           
            string tag = selectedItem.Tag.ToString();

            if (tag == "3") 
            {
                spThongTinBoSung.Visibility = Visibility.Visible;
                lblThongTinBoSung.Text = "Mã Phòng Ban (VD: PB01) *";
            }
            else if (tag == "2")
            {
                spThongTinBoSung.Visibility = Visibility.Visible;
                lblThongTinBoSung.Text = "Chức vụ *";

                txtThongTinBoSung.Visibility = Visibility.Collapsed;
                cbCapQuanLy.Visibility = Visibility.Visible;
            }
            else
            {
                spThongTinBoSung.Visibility = Visibility.Collapsed;
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) ||
                 string.IsNullOrWhiteSpace(txtTenDangNhap.Text) ||
                 string.IsNullOrWhiteSpace(txtMatKhau.Password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ các trường bắt buộc (*)", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedRoleItem = cbVaiTro.SelectedItem as ComboBoxItem;
            int roleType = int.Parse(selectedRoleItem.Tag.ToString());
            string roleTypeStr = "NhanVien";
            string info = "";

            if (roleType == 1)
            {
                roleTypeStr = "Admin";
                info = "FullAccess";
            }
            else if (roleType == 2)
            {
                roleTypeStr = "QuanLy";
                info = cbCapQuanLy.Text;
            }
            else
            {
                roleTypeStr = "NhanVien";
                info = txtThongTinBoSung.Text.Trim();
            }
            if (roleType != 1 && string.IsNullOrWhiteSpace(info))
            {
                MessageBox.Show($"Vui lòng nhập/chọn {lblThongTinBoSung.Text}", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newUser = new User
            {
                TenDangNhap = txtTenDangNhap.Text.Trim(),
                
                MatKhau = BCrypt.Net.BCrypt.HashPassword(txtMatKhau.Password),
                HoTen = txtHoTen.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                MaVaiTro = roleType
            };
            bool isAdded = userService.AddUser(newUser, roleTypeStr, info);

            if (isAdded)
            {
                MessageBox.Show("Thêm thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Thêm thất bại! Tên đăng nhập có thể đã tồn tại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
