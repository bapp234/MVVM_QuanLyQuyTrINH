using MVVM_QuanLyQuyTrINH.Models.Project;
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
    /// Interaction logic for SuaDuAn.xaml
    /// </summary>
    public partial class SuaDuAn : Window
    {
        public DuAn CurrentDuAn { get; set; }
        public ObservableCollection<QuyTrinh> DanhSachBuoc { get; set; } = new ObservableCollection<QuyTrinh>();

        // Options cho ComboBox trạng thái
        public List<string> TrangThaiOptions { get; set; } = new List<string> { "Chưa thực hiện", "Đang thực hiện", "Hoàn thành" };

        public SuaDuAn(DuAn duAn, List<QuyTrinh> buocs)
        {
            InitializeComponent();
            CurrentDuAn = duAn;
            if (buocs != null)
            {
                foreach (var b in buocs)
                {
                    DanhSachBuoc.Add(b);
                }
            }
            DataContext = this;
        }
    }
}
