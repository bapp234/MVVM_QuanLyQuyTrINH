using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace MVVM_QuanLyQuyTrINH.Views
{
    public partial class LoginWindow : Window
    {
        private bool isPasswordVisible = false;
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text;
            string password = isPasswordVisible ? txtVisiblePassword.Text : pwdBox.Password;

            if (email == "admin@gmail.com" && password == "123")
            {
                MainAppWindow mainAppWindow = new MainAppWindow();
                mainAppWindow.Show();
            }
            else
            {
                MessageBox.Show("Sai email hoặc mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Vui lòng liên hệ quản trị viên để đặt lại mật khẩu.", "Quên mật khẩu", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                // Ẩn mật khẩu
                pwdBox.Password = txtVisiblePassword.Text;
                txtVisiblePassword.Visibility = Visibility.Collapsed;
                pwdBox.Visibility = Visibility.Visible;
                btnTogglePassword.Content = "👁";
                isPasswordVisible = false;
            }
            else
            {
                // Hiện mật khẩu
                txtVisiblePassword.Text = pwdBox.Password;
                txtVisiblePassword.Visibility = Visibility.Visible;
                pwdBox.Visibility = Visibility.Collapsed;
                btnTogglePassword.Content = "🚫";
                isPasswordVisible = true;
            }
        }
    }


}