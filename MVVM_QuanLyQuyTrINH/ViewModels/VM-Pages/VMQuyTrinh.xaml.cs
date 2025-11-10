using MVVM_QuanLyQuyTrINH.Models.Context;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    /// <summary>
    /// Interaction logic for QuyTrinh.xaml
    /// </summary>
    public partial class QuyTrinh : Page
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        private List<DuAn> _allQuyTrinh = new List<DuAn>();
        public QuyTrinh()
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
        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var border = sender as Border;
            if (border == null) return;

            var duAn = border.DataContext as DuAn;
            if (duAn == null) return;

            // Lấy danh sách bước từ database
            List<Models.Project.QuyTrinh> danhSachBuoc = _context.QuyTrinhs
                                                    .Where(q => q.MaDuAn == duAn.MaDuAn)
                                                    .ToList();

            ChiTietQuyTrinh popup = new ChiTietQuyTrinh(duAn, danhSachBuoc);
            popup.ShowDialog();
        }
    }
}
