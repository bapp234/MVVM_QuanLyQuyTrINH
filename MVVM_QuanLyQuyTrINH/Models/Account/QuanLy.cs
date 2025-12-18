using MVVM_QuanLyQuyTrINH.Models.Project;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVVM_QuanLyQuyTrINH.Models.Account
{
    public partial class QuanLy
    {
        public int MaQl { get; set; }
        public string? CapQuanLy { get; set; }
        public virtual User MaQlNavigation { get; set; } = null!;
        public virtual ICollection<DuAn> DuAns { get; set; } = new HashSet<DuAn>();
    }
}
