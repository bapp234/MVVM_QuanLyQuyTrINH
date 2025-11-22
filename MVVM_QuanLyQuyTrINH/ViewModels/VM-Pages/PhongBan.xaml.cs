using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
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
    /// Interaction logic for PhongBan.xaml
    /// </summary>
    public partial class PhongBan : Page
    {
        public PhongBan()
        {
            InitializeComponent();
      
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            
            if (NavigationService != null && NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
