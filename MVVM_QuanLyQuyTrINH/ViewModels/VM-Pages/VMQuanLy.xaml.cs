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
        public QuanLy()
        {
            InitializeComponent();
        }
        private void btnQuanLyDuAn_Click(object sender, RoutedEventArgs e)
        {
            return;
            //MainFrame.Navigate(new QuanLyDuAn());
        }
        private void btnQuanLyNhanVien_Click(object sender, RoutedEventArgs e)
        {
            return;
            //MainFrame.Navigate(new QuanLyNguoiDung());
        }
        private void btnPhanQuyen_Click(object sender, RoutedEventArgs e)
        {
            return;
            //MainFrame.Navigate(new QuanLyPhanQuyen());
        }
        private void btnQuanLyQuyTrinh_Click(object sender, RoutedEventArgs e)
        {
            return;
            //MainFrame.Navigate(new QuanLyQuyTrinh());
        }
      private void btnQuanLyCongViec_Click(object sender, RoutedEventArgs e)
        {
            return;
            //MainFrame.Navigate(new QuanLyCongViec());
        }
    }
}
