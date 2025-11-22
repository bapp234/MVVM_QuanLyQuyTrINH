using Microsoft.EntityFrameworkCore;
using MVVM_QuanLyQuyTrINH.Models.Context;
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
using MVVM_QuanLyQuyTrINH.Services;

namespace MVVM_QuanLyQuyTrINH.Views.Windows
{
    /// <summary>
    /// Interaction logic for ThemCongViec.xaml
    /// </summary>
    public partial class ThemCongViec : Window
    {
        private readonly WorkService _db_Work = new WorkService();
        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        public ThemCongViec()
        {
            InitializeComponent();
            loadComboBoxData();
            cbDuAn.SelectionChanged += cbDuAn_SelectionChanged;
        }
        private void loadComboBoxData()
        {
            try
            {
                dpNgayGiao.SelectedDate = DateTime.Now;
                dpHanHoanThanh.SelectedDate = DateTime.Now.AddDays(3);
                cbTrangThai.SelectedIndex = 0;
                cbDoUuTien.SelectedIndex = 1;
                var listDuAn = _context.DuAns.ToList();
                cbDuAn.ItemsSource = listDuAn;
                var listNhanVien = _context.NhanViens
                    .Include(nv => nv.MaNvNavigation)
                    .Select(nv => new
                    {
                        MaNv = nv.MaNv,
                        HoTen = nv.MaNvNavigation.HoTen
                    })
                    .ToList();
                cbNhanVienPhuTrach.ItemsSource = listNhanVien;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }
        private void cbDuAn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDuAn.SelectedValue != null)
            {
                int maDuAn = (int)cbDuAn.SelectedValue;

                var listBuoc = _context.QuyTrinhs
                    .Where(qt => qt.MaDuAn == maDuAn)
                    .OrderBy(qt => qt.ThuTu)
                    .ToList();
                cbBuocLam.ItemsSource = listBuoc;
                cbBuocLam.IsEnabled = true;
            }
            else
            {
                cbBuocLam.ItemsSource = null;
                cbBuocLam.IsEnabled = false;
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenCv.Text))
            {
                MessageBox.Show("Vui lòng nhập tên công việc!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtTenCv.Focus();
                return;
            }
            if (cbDuAn.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn Dự án!", "Thiếu thông tin", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var newJob = new Models.Project.CongViec
                {
                    TenCv = txtTenCv.Text.Trim(),
                    MoTa = txtMoTa.Text?.Trim(),

                    NgayGiao = dpNgayGiao.SelectedDate,
                    HanHoanThanh = dpHanHoanThanh.SelectedDate,
                    TrangThai = (cbTrangThai.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    DoUuTien = (cbDoUuTien.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    MaDuAn = (int)cbDuAn.SelectedValue,
                    MaNvphuTrach = cbNhanVienPhuTrach.SelectedValue as int?,
                };
                if (_db_Work.Add(newJob))
                {
                    MessageBox.Show("Thêm công việc thành công!", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Thêm thất bại! Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
