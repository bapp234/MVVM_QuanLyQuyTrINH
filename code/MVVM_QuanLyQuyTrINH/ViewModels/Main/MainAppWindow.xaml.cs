using MVVM_QuanLyQuyTrINH.Models;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Views;
using MVVM_QuanLyQuyTrINH.Views.Pages;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using MVVM_QuanLyQuyTrINH.Views.Details;

namespace MVVM_QuanLyQuyTrINH.Views
{
    public partial class MainAppWindow : Window
    {
        private readonly User _currentUser;

        public MainAppWindow(User user)
        {
            InitializeComponent();
            _currentUser = user;
            this.DataContext = _currentUser;
            PhanQuyenMenu();
        }

        private void PhanQuyenMenu()
        {
            if (_currentUser.MaVaiTro == 2)
            {
                if (btnQuanLy != null)
                {
                    btnQuanLy.Visibility = Visibility.Collapsed;
                }

            }
        }
        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.TrangChu());
        }


        private void TrangChu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.TrangChu());
        }

        private void QuanLy_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.QuanLy(MainFrame));
        }

        private void QuyTrinh_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.QuyTrinh());
        }

        private void BaoCao_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.BaoCao());
        }
        private void CongViec_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.CongViec());
        }
        private void CaiDat_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.CaiDat(_currentUser));
        }
        private void btnThongTinCaNhan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.ThongTinCaNhan(_currentUser));
        }


        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất không ?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                new LoginWindow().Show();
                this.Close();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
