using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Views.Details;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVM_QuanLyQuyTrINH.Views.Pages
{
    public partial class CongViec : Page
    {
        // Chưa dùng context lúc này — chỉ hiển thị UI
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();

        // Danh sách công việc tạm thời — dữ liệu sẽ gán sau
        private List<Models.Project.CongViec> _allTasks = new List<Models.Project.CongViec>();

        public CongViec()
        {
            InitializeComponent();

            //LoadData();
        }

        /*
        private void LoadData()
        {
            try
            {
                _allTasks = _context.CongViecs
                                   .Select(cv => cv)
                                   .ToList();

                TaskList.ItemsSource = _allTasks;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu công việc: " + ex.Message);
            }
        }
        */

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(SearchTextBox.Text)
                ? Visibility.Visible
                : Visibility.Hidden;
        }

        // ================== LỌC (TẮT TẠM) ==================
        private void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ApplyFilters();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //ApplyFilters();
        }

        /*
        private void ApplyFilters()
        {
            return;
        }
        */
        private void TaskCard_Click(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Tính năng xem chi tiết sẽ được thêm sau.");
        }

        private void btnThemCongViec_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng thêm công việc sẽ được thêm sau.");
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }
    }
}
