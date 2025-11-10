using MVVM_QuanLyQuyTrINH.Models.Project;
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

namespace MVVM_QuanLyQuyTrINH.Views
{
    /// <summary>
    /// Interaction logic for ChiTietQuyTrinh.xaml
    /// </summary>
    public partial class ChiTietQuyTrinh : Window
    {
        //public List<QuyTrinh>? DanhSachBuoc { get; set; }
        public ChiTietQuyTrinh(DuAn duAn, List<QuyTrinh> buocs)
        {
            InitializeComponent();
            DataContext = new
            {
                duAn.TenDuAn,
                duAn.MaDuAn,
                duAn.NgayBatDau,
                duAn.NgayKetThuc,
                duAn.TrangThai,
                DanhSachBuoc = buocs.OrderBy(b => b.ThuTu).ToList()
            };
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
