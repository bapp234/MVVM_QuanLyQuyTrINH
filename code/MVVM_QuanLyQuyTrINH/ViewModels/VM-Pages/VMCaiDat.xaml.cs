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
using MVVM_QuanLyQuyTrINH.Models.Account;

namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    /// <summary>
    /// Interaction logic for CaiDat.xaml
    /// </summary>
    public partial class CaiDat : Page
    {
        private User _currentUser;
        public CaiDat(User user)
        {
            InitializeComponent();
            _currentUser = user;
            MainFrame.Navigate(new ThongTinCaNhan(_currentUser));
        }

        private void btnThongTinCaNhan_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ThongTinCaNhan(_currentUser));
        }

        private void btnDoiMatKhau_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DoiMatKhau());
        }
    }
}
