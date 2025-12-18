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
using System.Windows.Shapes;

namespace MVVM_QuanLyQuyTrINH.Views.Details
{
    /// <summary>
    /// Interaction logic for ChiTietNhanVien.xaml
    /// </summary>
    public partial class ChiTietNhanVien : Window
    {
        public ChiTietNhanVien(User nv)
        {
            InitializeComponent();
            this.DataContext = nv;
            using (var _context = new QLQuyTrinhLamViecContext())
            {
                if (nv.NhanVien != null)
                {
                    var congViecs = _context.PhanCongs
                                            .Where(c => c.MaNv == nv.NhanVien.MaNv)
                                            .Select(c => new { c.MaCvNavigation.TenCv, c.MaCvNavigation.TrangThai })
                                            .ToList();

                    ListViewCongViecs.ItemsSource = congViecs;
                }
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
