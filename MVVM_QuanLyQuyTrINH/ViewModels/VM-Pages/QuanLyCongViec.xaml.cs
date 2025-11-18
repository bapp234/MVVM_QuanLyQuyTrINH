using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Views.Details;
using MVVM_QuanLyQuyTrINH.Views.Windows;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    public partial class QuanLyCongViec : Page
    {
        private readonly QLQuyTrinhLamViecContext _db = new QLQuyTrinhLamViecContext();

        public QuanLyCongViec()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var dsCongViec = _db.CongViecs
                .Include(cv => cv.MaDuAnNavigation)
                .Include(cv => cv.MaNvphuTrachNavigation).ThenInclude(nv => nv.MaNvNavigation)
                .ToList();

            TaskList.ItemsSource = dsCongViec;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
            FilterData();
        }

        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterData();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            FilterData();
        }

        private void FilterData()
        {
            //var query = _db.CongViecs
            //    .Include(cv => cv.MaDuAnNavigation)
            //    .Include(cv => cv.MaNvphuTrachNavigation.MaNvNavigation)
            //    .AsQueryable();

            //var keyword = SearchTextBox.Text?.Trim().ToLower();
            //if (!string.IsNullOrEmpty(keyword))
            //{
            //    query = query.Where(cv =>
            //        cv.TenCv.ToLower().Contains(keyword) ||
            //        cv.MaCv.ToString().Contains(keyword));
            //}

            //if (cbFilter.SelectedItem is ComboBoxItem selected && selected.Content.ToString() != "Tất cả")
            //{
            //    var trangThai = selected.Content.ToString();
            //    query = query.Where(cv => cv.TrangThai == trangThai);
            //}

            //TaskList.ItemsSource = query.ToList();
            return;
        }

        private void btnThemCongViec_Click(object sender, RoutedEventArgs e)
        {
            ThemCongViec popup = new ThemCongViec();
            bool? result = popup.ShowDialog();

            if (result == true)
            {
                LoadData(); // Reload danh sách dự án
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
        private void ViewTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var congViec = button.DataContext as CongViec;
            if (congViec == null) return;

            // Mở popup chi tiết nhân viên
            ChiTietCongViec popup = new ChiTietCongViec(congViec);
            popup.ShowDialog();
        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var congViec = button.DataContext as CongViec;
            if (congViec == null) return;

            // Tạo cửa sổ sửa công việc, truyền công việc hiện tại
            var suaCongViecWindow = new Windows.SuaCongViec(congViec.MaCv);

            // Mở cửa sổ dưới dạng modal
            suaCongViecWindow.Owner = Application.Current.MainWindow; // Đặt cửa sổ cha
            suaCongViecWindow.ShowDialog();

            // Sau khi sửa xong, load lại danh sách công việc
            LoadData();
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa công việc này không?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                MessageBox.Show("Đã xóa công việc!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
