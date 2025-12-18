using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.MyModels;
using MVVM_QuanLyQuyTrINH.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel; 
using System.Diagnostics;
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
    public partial class GoCongViec : Window
    {
        private readonly int _maNV;
        private readonly UserService _userService = new UserService();
        public ObservableCollection<CList_DuAn> dsDuAn { get; set; }
        public GoCongViec(int maNV)
        {
            InitializeComponent();
            _maNV = maNV;
            dsDuAn = new ObservableCollection<CList_DuAn>();
            DataContext = this;
            _ = LoadData();
        }

        private async Task LoadData()
        {
            try
            {
                var list = await Task.Run(() => _userService.DanhSachCongViecCanXoa(_maNV));
                if (list == null || list.Count == 0) return;
                var groupedList = list
                    .GroupBy(x => x.TenDuAn)
                    .Select(g => new CList_DuAn
                    {
                        TenDuAn = g.Key ?? "Dự án khác",
                        CongVieces = new ObservableCollection<CList_CongViec>(g)
                    })
                    .ToList();
                Dispatcher.Invoke(() =>
                {
                    dsDuAn.Clear();
                    foreach (var item in groupedList)
                    {
                        dsDuAn.Add(item);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load công việc:\n" + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var selectedTaskIds = dsDuAn
                .SelectMany(da => da.CongVieces)
                .Where(cv => cv.IsSelected)
                .Select(cv => cv.MaCv)
                .ToList();

            if (!selectedTaskIds.Any())
            {
                MessageBox.Show("Vui lòng chọn ít nhất một công việc để gỡ phân công!", "Chưa chọn công việc", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn gỡ {selectedTaskIds.Count} công việc khỏi nhân viên này?", "Xác nhận gỡ phân công", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            bool success = _userService.RemovePhanCong(_maNV, selectedTaskIds);

            if (success)
            {
                MessageBox.Show("Gỡ phân công thành công!", "Hoàn tất", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Gỡ phân công thất bại. Vui lòng kiểm tra lại dữ liệu.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}