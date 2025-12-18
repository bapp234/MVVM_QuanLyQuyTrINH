using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_QuanLyQuyTrINH.Models.MyModels
{
    public class CList_DuAn
    {
        public int MaDuAn { get; set; }
        public string TenDuAn { get; set; }
        public ObservableCollection<CList_CongViec> CongVieces { get; set; }
    }
}
