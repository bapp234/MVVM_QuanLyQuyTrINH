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
using MVVM_QuanLyQuyTrINH.Services;


namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    public partial class QuanLyCongViec : Page
    {
        private readonly WorkService _db_Work = new WorkService();
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        private List<Models.Project.CongViec> dsCongViec = new List<Models.Project.CongViec>();
        public QuanLyCongViec()
        {
            InitializeComponent();
            LoadInitialData();
            LoadData();
        }
        private void LoadInitialData()
        {
            try
            {
                var listDuAn = _context.DuAns.ToList();
                listDuAn.Insert(0, new DuAn { MaDuAn = 0, TenDuAn = "Tất cả dự án" });
                cbFilterProject.ItemsSource = listDuAn;
                cbFilterProject.SelectedIndex = 0;
            }
            catch { }
        }
        private void LoadData()
        {
            try
            {
                dsCongViec = _db_Work.GetCongViecs();
                if (WorkList != null)
                {
                    ApplyFilters(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrWhiteSpace(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;
            ApplyFilters();
        }

        private void cbFilterProject_SelectionChanged(object sender, SelectionChangedEventArgs e) { ApplyFilters(); }
        private void cbFilterStatus_SelectionChanged(object sender, SelectionChangedEventArgs e) { ApplyFilters(); }
        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e) { ApplyFilters(); }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            if (WorkList == null) return;
            var viewList = dsCongViec.AsEnumerable();
            string searchText = SearchTextBox.Text?.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                viewList = viewList.Where(cv =>
             (cv.TenCv != null && cv.TenCv.ToLower().Contains(searchText)) ||
             (cv.MaDuAnNavigation != null && cv.MaDuAnNavigation.TenDuAn != null && cv.MaDuAnNavigation.TenDuAn.ToLower().Contains(searchText)) ||
             (cv.PhanCongs != null && cv.PhanCongs.Any(pc =>
                 pc.MaNvNavigation != null &&
                 pc.MaNvNavigation.MaNvNavigation != null && 
                 pc.MaNvNavigation.MaNvNavigation.HoTen != null &&
                 pc.MaNvNavigation.MaNvNavigation.HoTen.ToLower().Contains(searchText)))
         );
            }
            if (cbFilterProject.SelectedValue != null)
            {
                int selectedProjectId = (int)cbFilterProject.SelectedValue;
                if (selectedProjectId != 0)
                {
                    viewList = viewList.Where(cv => cv.MaDuAn == selectedProjectId);
                }
            }
            if (cbFilterStatus.SelectedItem is ComboBoxItem statusItem)
            {
                string status = statusItem.Content.ToString();
                if (status != "Tất cả trạng thái")
                {
                    if (status == "Trễ hạn")
                    {
                        viewList = viewList.Where(cv => cv.mauTrangThai == "#E74C3C" || cv.mauTrangThai == "#D32F2F");
                    }
                    else
                    {
                        viewList = viewList.Where(cv => cv.TrangThai == status);
                    }
                }
            }
            if (cbSort.SelectedItem is ComboBoxItem sortItem && sortItem.Tag != null)
            {
                string sortType = sortItem.Tag.ToString();
                switch (sortType)
                {
                    case "DateDesc": 
                        viewList = viewList.OrderByDescending(cv => cv.NgayGiao);
                        break;
                    case "DateAsc": 
                        viewList = viewList.OrderBy(cv => cv.NgayGiao);
                        break;
                    case "NameAsc": 
                        viewList = viewList.OrderBy(cv => cv.TenCv);
                        break;
                    case "NameDesc":
                        viewList = viewList.OrderByDescending(cv => cv.TenCv);
                        break;
                    case "Priority":
                        viewList = viewList.OrderBy(cv => GetPriorityScore(cv.DoUuTien));
                        break;
                }
            }
            WorkList.ItemsSource = viewList.ToList();
        }

        private void btnAddWork_Click(object sender, RoutedEventArgs e)
        {
            var popup = new ThemCongViec();
            if (popup.ShowDialog() == true)
            {
                LoadData();
            }
        }
        private int GetPriorityScore(string priority)
        {
            if (string.IsNullOrEmpty(priority)) return 5;
            string p = priority.Trim().ToLower();
            if (p == "khẩn cấp" || p == "rất cao") return 1;
            if (p == "cao") return 2;
            if (p == "trung bình") return 3;
            if (p == "thấp") return 4;
            return 5;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack) NavigationService.GoBack();
        }
        private void ViewWork_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Models.Project.CongViec task)
            {
                var ChitetWindow = new ChiTietCongViec(task);
                ChitetWindow.ShowDialog();  
            }
        }

        private void EditWork_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Models.Project.CongViec task)
            {
                var popup = new SuaCongViec(task.MaCv);

                if (popup.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void DeleteWork_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Models.Project.CongViec task)
            {
                var confirmResult = MessageBox.Show(
             $"Bạn có chắc chắn muốn xóa công việc:\n'{task.TenCv}'?\n\nLưu ý: Hành động này không thể hoàn tác.",
             "Xác nhận xóa",
             MessageBoxButton.YesNo,
             MessageBoxImage.Warning);
                if (confirmResult == MessageBoxResult.Yes)
                {
                    bool isDeleted = _db_Work.Delete(task.MaCv);
                    if (isDeleted)
                    {
                        LoadData();
                        MessageBox.Show("Đã xóa công việc thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Xóa thất bại! Có thể công việc này đang được liên kết dữ liệu khác.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }

        }

    }
}
