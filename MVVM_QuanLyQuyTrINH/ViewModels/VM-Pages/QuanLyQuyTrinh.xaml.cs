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
using MVVM_QuanLyQuyTrINH.Services;
using Microsoft.EntityFrameworkCore;
using System.Data;
namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    /// <summary>
    /// Interaction logic for QuanLyQuyTrinh.xaml
    /// </summary>
    public partial class QuanLyQuyTrinh : Page
    {
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        private List<DuAn> _allQuyTrinh = new List<DuAn>();
        private readonly ProcessService _processService = new ProcessService();
        public QuanLyQuyTrinh()
        {
            InitializeComponent();
            LoadData();
          
        }
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text)
                            ? Visibility.Visible
                            : Visibility.Collapsed;
            ApplyFilters();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void LoadData()
        {
            try
            {
                _allQuyTrinh = _processService.GetDuAnList();
                if (ProcessList != null)
                {
                    ApplyFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
        private void cbFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }
        private void ApplyFilters()
        {
            if (_allQuyTrinh == null || !_allQuyTrinh.Any()) return;
            var viewList = _allQuyTrinh.AsEnumerable();
            string keyword = SearchTextBox.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(keyword))
            {
                viewList = viewList.Where(p =>
                    (p.TenDuAn != null && p.TenDuAn.ToLower().Contains(keyword)) ||
                    (p.MaHienThi.ToLower().Contains(keyword)) ||
                    (p.MoTa != null && p.MoTa.ToLower().Contains(keyword))
                );
            }
            if (cbFilterStatus.SelectedItem is ComboBoxItem statusItem)
            {
                string status = statusItem.Content.ToString();
                if (status != "Tất cả trạng thái")
                {
                    if (status == "Trễ hạn")
                    {
                        viewList = viewList.Where(da => da.MauTrangThai == "#E74C3C" || da.MauTrangThai == "#D32F2F");
                    }
                    else
                    {
                        viewList = viewList.Where(p => p.TrangThai == status);
                    }
                }
            }
            if (cbSort.SelectedItem is ComboBoxItem sortItem && sortItem.Tag != null)
            {
                string sortType = sortItem.Tag.ToString();
                switch (sortType)
                {
                    case "DateDesc":
                        viewList = viewList.OrderByDescending(p => p.NgayBatDau);
                        break;
                    case "DateAsc":
                        viewList = viewList.OrderBy(p => p.NgayBatDau);
                        break;
                    case "NameAsc":
                        viewList = viewList.OrderBy(p => p.TenDuAn);
                        break;
                    case "NameDesc":
                        viewList = viewList.OrderByDescending(p => p.TenDuAn);
                        break;
                    default:
                        break;
                }
            }

            if (ProcessList != null)
            {
                ProcessList.ItemsSource = viewList.ToList();
            }
        }
        private void ViewProcess_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            var button = sender as Button;
            if (button == null) return;
            var duAn = button.DataContext as DuAn;
            if (duAn == null) return;

            var danhSachBuoc = _context.QuyTrinhs
                                       .Where(q => q.MaDuAn == duAn.MaDuAn)
                                       .ToList();

            ChiTietQuyTrinh popup = new ChiTietQuyTrinh(duAn, danhSachBuoc);
            popup.ShowDialog();
        }
        private void EditProcess_Click(object sender, RoutedEventArgs e)
        {
           if (sender is Button btn && btn.DataContext is DuAn duAn)
            {
                var suaduAn = new SuaDuAn(duAn);
                suaduAn.ShowDialog();
                LoadData();
            }
        }
        private void DeleteProcess_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is DuAn process)
            {
                var confirmDelete = MessageBox.Show($"Bạn có chắc chắn muốn xóa dự án:\n'{process.TenDuAn}'?\n\nLưu ý: Hành động này không thể hoàn tác.","Xác nhận xóa",MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (confirmDelete == MessageBoxResult.Yes)
                {
                    bool isDeleted = _processService.DeleteDuAn(process.MaDuAn);
                    if (isDeleted)
                    {
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại! Có thể dự án này đang được liên kết dữ liệu khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        private void btnAddProcess_Click(object sender, RoutedEventArgs e)
        {
            ThemDuAn themDuAnWindow = new ThemDuAn();
            bool? result = themDuAnWindow.ShowDialog();

            if (result == true)
            {
                LoadData();
            }
        }
    }
}
