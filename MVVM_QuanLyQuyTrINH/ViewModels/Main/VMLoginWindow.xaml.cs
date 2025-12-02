using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Services;
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

namespace MVVM_QuanLyQuyTrINH.Views;

public partial class LoginWindow : Window
{
    private readonly AuthService authService = new AuthService();
    public LoginWindow()
    {
        InitializeComponent();
        
    }
    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private async void Login_Click(object sender, RoutedEventArgs e)
    {

        string userName = txtUserName.Text.Trim();
        string password = txtVisiblePassword.Visibility==Visibility.Visible?txtVisiblePassword.Text:pwdBox.Password;

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        try 
        {
            Mouse.OverrideCursor = Cursors.Wait;
            var user = await authService.Login(userName,password); 
            if (user != null)
            {
                UserSession.CurrentUser = user;
                MainAppWindow mainAppWindow = new MainAppWindow(user);
                mainAppWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Đã xảy ra lỗi trong quá trình đăng nhập: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            Mouse.OverrideCursor = null;
        }
    }
    private void ForgotPassword_Click(object sender, RoutedEventArgs e)
    {
        ForgotPassword fp = new ForgotPassword();
        fp.Show();
        this.Close();
    }
    private void TogglePassword_Click(object sender, RoutedEventArgs e)
    {
        if (txtVisiblePassword.Visibility == Visibility.Collapsed)
        {
            txtVisiblePassword.Text = pwdBox.Password;
            txtVisiblePassword.Visibility = Visibility.Visible;
            pwdBox.Visibility = Visibility.Collapsed;
        }
        else
        {
            pwdBox.Password = txtVisiblePassword.Text;
            pwdBox.Visibility = Visibility.Visible;
            txtVisiblePassword.Visibility = Visibility.Collapsed;
        }
    }
}