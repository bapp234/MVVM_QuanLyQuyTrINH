using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Views;
using MVVM_QuanLyQuyTrINH.Views.Details;
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
using MVVM_QuanLyQuyTrINH.Views.Windows;
namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    /// <summary>
    /// Interaction logic for QuanLyQuyTrinh.xaml
    /// </summary>
    public partial class QuanLyQuyTrinh : Page
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        private List<DuAn> _allQuyTrinh = new List<DuAn>();
        public QuanLyQuyTrinh()
        {
            InitializeComponent();
            LoadData();
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }
        private void LoadData()
        {
            _allQuyTrinh = _context.DuAns.ToList();
            ProcessList.ItemsSource = _allQuyTrinh;
        }
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            //string keyword = SearchTextBox.Text?.Trim().ToLower() ?? "";
            //string selectedStatus = (cbFilter.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Tất cả";

            //var filtered = _allQuyTrinh.Where(p =>
            //(
            //    string.IsNullOrWhiteSpace(keyword)
            //    || p.TenDuAn.ToLower().Contains(keyword)
            //    || (p.MoTa != null && p.MoTa.ToLower().Contains(keyword))
            //    || p.MaDuAn.ToString().Contains(keyword)
            //)
            //&&
            //(
            //    selectedStatus == "Tất cả"
            //    || p.TrangThai == selectedStatus
            //)).ToList();

            //ProcessList.ItemsSource = filtered;
            return;
        }
        private void ViewProcess_Click(object sender, RoutedEventArgs e)
        {
            // Ngăn chặn sự kiện click lan ra Border
            e.Handled = true;

            // Lấy button
            var button = sender as Button;
            if (button == null) return;

            // Lấy DataContext của thẻ
            var duAn = button.DataContext as DuAn;
            if (duAn == null) return;

            // Lấy danh sách bước quy trình từ DB
            var danhSachBuoc = _context.QuyTrinhs
                                       .Where(q => q.MaDuAn == duAn.MaDuAn)
                                       .ToList();

            // Mở cửa sổ chi tiết
            ChiTietQuyTrinh popup = new ChiTietQuyTrinh(duAn, danhSachBuoc);
            popup.ShowDialog();
        }
        private void EditProcess_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var duAn = button.DataContext as DuAn;
            if (duAn == null) return;

            // Lấy danh sách bước từ database
            var danhSachBuoc = _context.QuyTrinhs
                                       .Where(q => q.MaDuAn == duAn.MaDuAn)
                                       .ToList();

            // Mở cửa sổ sửa dự án
            var suaDuAn = new SuaDuAn(duAn, danhSachBuoc);
            suaDuAn.ShowDialog();

            // Reload lại danh sách sau khi sửa
            LoadData();
        }
        private void DeleteProcess_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // Quay lại trang quản lý
            NavigationService.GoBack();
        }
        private void btnThemDuAn_Click(object sender, RoutedEventArgs e)
        {
            ThemDuAn themDuAnWindow = new ThemDuAn();
            bool? result = themDuAnWindow.ShowDialog();

            if (result == true)
            {
                LoadData(); // Reload danh sách dự án
            }
        }
    }
}
