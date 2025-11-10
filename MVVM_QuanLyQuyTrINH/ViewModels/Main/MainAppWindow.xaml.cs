using MVVM_QuanLyQuyTrINH.Models;
using MVVM_QuanLyQuyTrINH.Views.Pages;
using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace MVVM_QuanLyQuyTrINH.Views
{
    public partial class MainAppWindow : Window
    {
        public MainAppWindow()
        {
            InitializeComponent();
        }

        // Khi cửa sổ mở, tự động load Trang chủ
        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.TrangChu());
        }

        // Nút chuyển trang
        private void TrangChu_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.TrangChu());
        }

        private void QuanLy_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.QuanLy());
        }

        private void QuyTrinh_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.QuyTrinh());
        }

        private void BaoCao_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.BaoCao());
        }

        private void CaiDat_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Views.Pages.CaiDat());
        }

        // Nút đăng xuất
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
