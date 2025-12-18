using MVVM_QuanLyQuyTrINH.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MVVM_QuanLyQuyTrINH.Services;
using MVVM_QuanLyQuyTrINH.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace MVVM_QuanLyQuyTrINH.Views.Windows
{
    public partial class SuaDuAn : Window
    {
        private readonly ProcessService _processService = new ProcessService();
        public DuAn DuAnDangChon { get; set; }
        public List<QuyTrinh> Steps { get; set; } = new List<QuyTrinh>();
        public List<QuyTrinh> DeletedSteps { get; set; } = new List<QuyTrinh>();
        private bool flagTrangThai = false;
        private readonly List<string> trangThaiBT = new List<string>
        {
            "Đang thực hiện", "Tạm dừng", "Hoàn thành", "Đã hủy"
        };
        public SuaDuAn(DuAn duAn)
        {
            InitializeComponent();
            DuAnDangChon = duAn;
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbTrangThai.ItemsSource = trangThaiBT;
            LoadTruongNhom();
            LoadBuoc();
            LoadDataToForm();
        }
        private void LoadDataToForm()
        {
            if (DuAnDangChon == null) return;
            txtMaDuAn.Text = DuAnDangChon.MaHienThi;
            txtTenDuAn.Text = DuAnDangChon.TenDuAn;
            txtMoTa.Text = DuAnDangChon.MoTa;
            dpBatDau.SelectedDate = DuAnDangChon.NgayBatDau;
            if (DuAnDangChon.MaTruongNhom.HasValue)
                cbTruongNhom.SelectedValue = DuAnDangChon.MaTruongNhom.Value;
            dpKetThuc.SelectedDate = DuAnDangChon.NgayKetThuc;
            CheckThoiGianThuc();
        }

        private void LoadTruongNhom()
        {
            try
            {
                using var context = new QLQuyTrinhLamViecContext();
                var leaders = context.QuanLies
                    .Include(q => q.MaQlNavigation)
                    .Select(q => new { Id = q.MaQl, Name = q.MaQlNavigation.HoTen })
                    .ToList();

                cbTruongNhom.ItemsSource = leaders;
                cbTruongNhom.DisplayMemberPath = "Name";
                cbTruongNhom.SelectedValuePath = "Id";
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load nhân viên: " + ex.Message); }
        }

        private async void LoadBuoc()
        {
            try
            {
                using var context = new QLQuyTrinhLamViecContext();
                var buocs = await context.QuyTrinhs
                    .Where(q => q.MaDuAn == DuAnDangChon.MaDuAn)
                    .OrderBy(q => q.ThuTu)
                    .AsNoTracking()
                    .ToListAsync();

                Steps.Clear();
                Steps.AddRange(buocs);
                dgSteps.ItemsSource = null;
                dgSteps.ItemsSource = Steps;
            }
            catch (Exception ex) { MessageBox.Show("Lỗi load bước: " + ex.Message); }
        }
        private void dpKetThuc_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            CheckThoiGianThuc();
        }
        private void cbTrangThai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (flagTrangThai) return;
            if (dpKetThuc.SelectedDate == null) return;
            DateTime endDate = dpKetThuc.SelectedDate.Value.Date;
            DateTime now = DateTime.Now.Date;
            if (endDate < now)
            {
                flagTrangThai = true;
                MessageBox.Show("Dự án đã quá hạn thời gian thực hiện!\n\n" +
                                "Trạng thái tự động chuyển thành 'Trễ hạn'.\n" +
                                "Vui lòng gia hạn 'Ngày kết thúc' trước khi thay đổi trạng thái.",
                                "Cảnh báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                cbTrangThai.SelectedItem = null; 
                cbTrangThai.Text = "Trễ hạn";   
                flagTrangThai = false;
            }
        }
        private void CheckThoiGianThuc()
        {
            if (dpKetThuc.SelectedDate == null) return;
            flagTrangThai = true;
            DateTime endDate = dpKetThuc.SelectedDate.Value.Date;
            DateTime now = DateTime.Now.Date;
            string currentStatus = DuAnDangChon.TrangThai;
            if (endDate < now && currentStatus != "Hoàn thành" && currentStatus != "Đã hủy")
            {
                cbTrangThai.Text = "Trễ hạn";
            }
            else
            {
                if (currentStatus == "Trễ hạn" || currentStatus == "Mới tạo")
                {
                    cbTrangThai.Text = "Đang thực hiện";
                    cbTrangThai.SelectedItem = "Đang thực hiện";
                }
                else
                {
                    cbTrangThai.Text = currentStatus;
                    cbTrangThai.SelectedItem = currentStatus;
                }
            }
            flagTrangThai= false;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                DuAnDangChon.TenDuAn = txtTenDuAn.Text.Trim();
                DuAnDangChon.MoTa = txtMoTa.Text.Trim();
                DuAnDangChon.NgayBatDau = dpBatDau.SelectedDate;
                DuAnDangChon.NgayKetThuc = dpKetThuc.SelectedDate;
                if (cbTruongNhom.SelectedValue != null)
                    DuAnDangChon.MaTruongNhom = (int)cbTruongNhom.SelectedValue;
                DuAnDangChon.TrangThai = cbTrangThai.Text;
                bool result = _processService.UpdateDuAnFull(DuAnDangChon, Steps, DeletedSteps);
                if (result)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Cập nhật thất bại!");
                }
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        // ... (Các hàm Add/Delete Step, Validate, Huy... giữ nguyên như cũ) ...

        private void btnAddStep_Click(object sender, RoutedEventArgs e)
        {
            var win = new ThemCacBuoc();
            if (win.ShowDialog() == true && win.NewStep != null)
            {
                win.NewStep.ThuTu = Steps.Count + 1;
                Steps.Add(win.NewStep);
                dgSteps.ItemsSource = null;
                dgSteps.ItemsSource = Steps;
            }
        }

        private void btnDeleteStep_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is QuyTrinh step)
            {
                if (MessageBox.Show($"Xóa bước '{step.TenBuoc}'?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    if (step.MaBuoc > 0) DeletedSteps.Add(step);
                    Steps.Remove(step);
                    int i = 1; foreach (var s in Steps) s.ThuTu = i++;
                    dgSteps.ItemsSource = null;
                    dgSteps.ItemsSource = Steps;
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenDuAn.Text)) { MessageBox.Show("Nhập tên dự án!"); return false; }
            if (cbTruongNhom.SelectedValue == null) { MessageBox.Show("Chọn trưởng nhóm!"); return false; }
            if (dpBatDau.SelectedDate == null || dpKetThuc.SelectedDate == null) { MessageBox.Show("Chọn ngày!"); return false; }
            if (dpKetThuc.SelectedDate < dpBatDau.SelectedDate) { MessageBox.Show("Ngày kết thúc sai!"); return false; }
            return true;
        }

        private void btnHuy_Click(object sender, RoutedEventArgs e) => this.Close();
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => this.DragMove();
    }
}