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
            public QuanLy(Frame mainframe) 
            {
                InitializeComponent();
                _mainframe = mainframe;
        }
            private void btnQuanLyNhanVien_Click(object sender, RoutedEventArgs e)
            {
                _mainframe.Navigate(new QuanLyNhanVien());
        }
            private void btnPhanQuyen_Click(object sender, RoutedEventArgs e)
            {
                _mainframe.Navigate(new PhanQuyen());
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
