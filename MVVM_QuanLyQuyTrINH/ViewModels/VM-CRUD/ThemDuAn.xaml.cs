using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Account;
using MVVM_QuanLyQuyTrINH.Models.Context;
using MVVM_QuanLyQuyTrINH.Models.Project;
using MVVM_QuanLyQuyTrINH.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MVVM_QuanLyQuyTrINH.Views.Windows
{
    /// <summary>
    /// Interaction logic for ThemDuAn.xaml
    /// </summary>
    public partial class ThemDuAn : Window
    {
        private readonly ProcessService _processService = new ProcessService();
        public DuAn DuAnMoi { get; set; }
        public List<QuyTrinh> Steps { get; set; }
        public ThemDuAn()
        {
            InitializeComponent();
            LoadTruongNhom();
            Steps = new List<QuyTrinh>();
            dgSteps.ItemsSource = Steps;
            DuAnMoi = new DuAn();
        }
        private void LoadTruongNhom()
        {
            try
            {
                using var context = new QLQuyTrinhLamViecContext();
                var leaders = context.QuanLies
                 .Include(q => q.MaQlNavigation)
                 .Select(q => new
                 {
                     Id = q.MaQl,
                     Name = q.MaQlNavigation.HoTen
                 })
                 .ToList();
                cbTruongNhom.ItemsSource = leaders;
                cbTruongNhom.DisplayMemberPath = "Name";
                cbTruongNhom.SelectedValuePath = "Id";

            }
            catch (Exception ex)
            {

                MessageBox.Show("Lỗi khi load danh sách trưởng nhóm: " + ex.Message);
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;
            if (cbTruongNhom.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn trưởng nhóm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            int maTruongNhom;
            try
            {
                maTruongNhom = Convert.ToInt32(cbTruongNhom.SelectedValue);
            }
            catch
            {
                MessageBox.Show("Giá trị trưởng nhóm không hợp lệ!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newDuAn = new DuAn
            {
                TenDuAn = txtTenDuAn.Text.Trim(),
                MaTruongNhom = (int)cbTruongNhom.SelectedValue,
                NgayBatDau = dpBatDau.SelectedDate,
                NgayKetThuc = dpKetThuc.SelectedDate,
                MoTa = txtMoTa.Text.Trim(),
                TrangThai = "Mới tạo"
            };

            bool result = _processService.AddDuAn(newDuAn, Steps);

            if (result)
            {
                MessageBox.Show("Thêm dự án thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Đã xảy ra lỗi khi lưu dự án!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void btnAddStep_Click(object sender, RoutedEventArgs e)
        {
            ThemCacBuoc win = new ThemCacBuoc();
            if (win.ShowDialog() == true)
            {
                var createdStep = win.NewStep;

                if (createdStep != null)
                {
                    createdStep.ThuTu = Steps.Count + 1;
                    Steps.Add(createdStep);
                    dgSteps.Items.Refresh();
                }
            }
        }
        private void btnDeleteStep_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is QuyTrinh selectedStep)
            {
                Steps.Remove(selectedStep);
                int index = 1;
                foreach (var step in Steps)
                    step.ThuTu = index++;

                dgSteps.Items.Refresh();
            }
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenDuAn.Text))
            {
                MessageBox.Show("Vui lòng nhập tên dự án!");
                return false;
            }

            if (cbTruongNhom.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trưởng nhóm!");
                return false;
            }

            if (dpBatDau.SelectedDate == null || dpKetThuc.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời gian bắt đầu và kết thúc!");
                return false;
            }

            if (dpKetThuc.SelectedDate < dpBatDau.SelectedDate)
            {
                MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu!");
                return false;
            }

            return true;
        }
        public void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized && this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
        }
        public void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
