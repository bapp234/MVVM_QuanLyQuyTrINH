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
using System.Windows.Shapes;

namespace MVVM_QuanLyQuyTrINH.Views.Windows
{
    /// <summary>
    /// Interaction logic for ThemCacBuoc.xaml
    /// </summary>
    public partial class ThemCacBuoc : Window
    {
        public QuyTrinh NewStep { get; private set; }

        private readonly QLQuyTrinhLamViecContext _context = new QLQuyTrinhLamViecContext();
        public ThemCacBuoc()
        {
            InitializeComponent();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTenBuoc.Text))
            {
                MessageBox.Show("Vui lòng nhập tên bước xử lý!");
                return false;
            }

            if (dpNgayBatDau.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời gian bắt đầu và kết thúc!");
                return false;
            }

            if (dpNgayKetThuc.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn thời gian bắt đầu và kết thúc!");
                return false;
            }
            if (dpNgayKetThuc.SelectedDate < dpNgayBatDau.SelectedDate)
            {
                MessageBox.Show("Ngày kết thúc không thể nhỏ hơn ngày bắt đầu!");
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput())
                return;
            NewStep = new QuyTrinh
            {
                TenBuoc = txtTenBuoc.Text.Trim(),
                Mota = txtMoTa.Text.Trim(),
                NgayBatDau = dpNgayBatDau.SelectedDate,
                NgayKetThuc = dpNgayKetThuc.SelectedDate
            };

            DialogResult = true;
            Close();
        }
    }
}
