using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_QuanLyQuyTrINH.Models.MyModels
{
    public class CList_CongViec
    {
        public int MaCv { get; set; }
        public string TenCv { get; set; }
        public string TrangThai { get; set; }
        public string TenDuAn { get; set; }
        public bool IsSelected { get; set; }
        public bool IsHoanThanh { get; set; }
    }
}
