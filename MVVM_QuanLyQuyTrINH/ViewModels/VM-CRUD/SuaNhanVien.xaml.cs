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

namespace MVVM_QuanLyQuyTrINH.Views.CRUD
{
    /// <summary>
    /// Interaction logic for SuaNhanVien.xaml
    /// </summary>
    public partial class SuaNhanVien : Window
    {
        private readonly UserService _userService = new UserService();
        private readonly User _currentUser;
        public SuaNhanVien(User userEdit)
        {
            InitializeComponent();
            _currentUser = userEdit;
            LoadDataToForm();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed) this.DragMove();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void LoadDataToForm()
        {
            if (_currentUser == null) return;
            txtUserId.Text = _currentUser.UserId.ToString();
            txtHoTen.Text = _currentUser.HoTen;
            txtEmail.Text = _currentUser.Email;
            foreach (ComboBoxItem item in cbVaiTro.Items)
            {
                if (item.Tag.ToString() == _currentUser.MaVaiTro.ToString())
                {
                    cbVaiTro.SelectedItem = item;
                    break;
                }
            }
            if (_currentUser.MaVaiTro == 3 && _currentUser.NhanVien != null) 
            {
                txtThongTinBoSung.Text = _currentUser.NhanVien.MaPb;
            }
            else if (_currentUser.MaVaiTro == 2 && _currentUser.QuanLy != null) 
            {
                foreach (ComboBoxItem item in cbCapQuanLy.Items)
                {
                    if (item.Content.ToString() == _currentUser.QuanLy.CapQuanLy)
                    {
                        cbCapQuanLy.SelectedItem = item;
                        break;
                    }
                }
            }
        }
        private void cbVaiTro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (spThongTinBoSung == null) return;

            var selectedItem = cbVaiTro.SelectedItem as ComboBoxItem;
            string tag = selectedItem.Tag.ToString();
            if(tag == "3")
            {
                
                    spThongTinBoSung.Visibility = Visibility.Visible;
                    lblThongTinBoSung.Text = "Mã Phòng Ban (VD: PB01) *";

                    txtThongTinBoSung.Visibility = Visibility.Visible;
                    cbCapQuanLy.Visibility = Visibility.Collapsed;
                }
            else if (tag == "2")
                {
                    spThongTinBoSung.Visibility = Visibility.Visible;
                    lblThongTinBoSung.Text = "Chức vụ quản lý *";

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
            if (string.IsNullOrWhiteSpace(txtHoTen.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Họ tên và Email!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedRoleItem = cbVaiTro.SelectedItem as ComboBoxItem;
            int newRoleId = int.Parse(selectedRoleItem.Tag.ToString());
            string roleTypeStr = "NhanVien";
            if (newRoleId == 1) roleTypeStr = "Admin";
            else if (newRoleId == 2) roleTypeStr = "QuanLy";
            string extraInfo = "";
            if (newRoleId == 3) extraInfo = txtThongTinBoSung.Text.Trim();
            else if (newRoleId == 2) extraInfo = cbCapQuanLy.Text;
            if (newRoleId != 1 && string.IsNullOrWhiteSpace(extraInfo))
            {
                MessageBox.Show($"Vui lòng nhập/chọn {lblThongTinBoSung.Text}", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_userService.IsEmailExists(txtEmail.Text.Trim(), _currentUser.UserId))
            {
                MessageBox.Show("Email này đã được sử dụng bởi tài khoản khác!", "Trùng dữ liệu", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_currentUser.MaVaiTro == 1 && newRoleId != 1)
            {
                if (_userService.IsUserLast(_currentUser.UserId))
                {
                    MessageBox.Show("KHÔNG THỂ THAY ĐỔI!\nĐây là tài khoản Quản Trị Viên duy nhất.\nBạn cần chỉ định một Admin khác trước khi giáng chức tài khoản này.",
                                    "Bảo mật hệ thống", MessageBoxButton.OK, MessageBoxImage.Stop);
                    return;
                }
            }
            string oldPb = _currentUser.NhanVien?.MaPb;
            bool isChangingRoleOrDept = (_currentUser.MaVaiTro == 3 && (newRoleId != 3 || oldPb != extraInfo));

            if (isChangingRoleOrDept)
            {
                int activeTasks = _userService.CountActiveTasks(_currentUser.UserId);

                if (activeTasks > 0)
                {
                    var ask = MessageBox.Show(
                        $"Nhân viên đang có {activeTasks} công việc chưa hoàn thành.\n" +
                        "Bạn có muốn gỡ phân công trước khi đổi vai trò không?",
                        "Xác nhận",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (ask == MessageBoxResult.No)
                        return;

                    var popup = new GoCongViec(_currentUser.UserId)
                    {
                        Owner = this
                    };

                    bool? popupResult = popup.ShowDialog();
                    if (popupResult != true)
                        return; // ❌ không gỡ → chặn đổi role
                }
            }

            _currentUser.HoTen = txtHoTen.Text.Trim();
            _currentUser.Email = txtEmail.Text.Trim();
            _currentUser.MaVaiTro = newRoleId;
            string newPassword = PasswordBoxNew.Password.Trim();
            bool result = _userService.UpdateUser(_currentUser, roleTypeStr, extraInfo, newPassword);

            if (result)
            {
                MessageBox.Show("Cập nhật thông tin thành công!", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Cập nhật thất bại. Vui lòng kiểm tra lại kết nối hoặc dữ liệu.", "Lỗi hệ thống", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
