using MVVM_QuanLyQuyTrINH.Models.Account;
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
        /// Interaction logic for QuanLy.xaml
        /// </summary>
        public partial class QuanLy : Page
        {
            private Frame _mainframe;
        private string _userRole;
        public QuanLy(Frame mainframe)
        {
            InitializeComponent();
            _mainframe = mainframe;

            if (UserSession.CurrentUser != null)
            {
                string roleGoc = UserSession.CurrentUser.MaVaiTroNavigation?.TenVaiTro;

                if (roleGoc == null)
                {
                    MessageBox.Show("LỖI: Không lấy được tên Role từ Database! (MaVaiTroNavigation bị null).\n\nHệ thống sẽ gán tạm là 'NhanVien'.");
                    _userRole = "NhanVien";
                }
                else
                {
                    _userRole = roleGoc;
                }
            }
            else
            {
                MessageBox.Show("LỖI: UserSession bị Null (Chưa đăng nhập hoặc mất phiên).");
                _userRole = "NhanVien";
            }

            PhanQuyenGiaoDien();
        }

        private void PhanQuyenGiaoDien()
        {
            gridMenuButtons.Visibility = Visibility.Visible;
            btnPhongBan.Visibility = Visibility.Visible;
            txtTitleNguoiDung.Text = "QUẢN LÝ NGƯỜI DÙNG";
            txtDescNguoiDung.Text = "Quản lý thông tin người dùng.";

            string roleCheck = _userRole.Trim().ToLower();

            
            switch (roleCheck)
            {
                case "admin":
                case "quantri":
                    break;

                case "quanly":
                case "quản lý": 
                case "manager":
                    btnPhongBan.Visibility = Visibility.Collapsed;
                    txtTitleNguoiDung.Text = "QUẢN LÝ NHÂN VIÊN";
                    txtDescNguoiDung.Text = "Quản lý danh sách nhân viên.";
                    break;

                case "nhanvien":
                case "nhân viên": 
                case "staff":
                case "user":
                    gridMenuButtons.Visibility = Visibility.Collapsed;
                    break;

                default:
                    MessageBox.Show($"CẢNH BÁO: Role '{roleCheck}' không khớp trường hợp nào -> Ẩn giao diện.");
                    gridMenuButtons.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void btnQuanLyNhanVien_Click(object sender, RoutedEventArgs e)
            {
            _mainframe.Navigate(new QuanLyNguoiDung());
        }
            private void btnQuanLyPhongBan_Click(object sender, RoutedEventArgs e)
            {
                _mainframe.Navigate(new PhongBan());
            }
            private void btnQuanLyQuyTrinh_Click(object sender, RoutedEventArgs e)
            {
                    _mainframe.Navigate(new QuanLyQuyTrinh());
            }
            private void btnQuanLyCongViec_Click(object sender, RoutedEventArgs e)
            {
                   _mainframe.Navigate(new QuanLyCongViec());
            }
        }
    }
