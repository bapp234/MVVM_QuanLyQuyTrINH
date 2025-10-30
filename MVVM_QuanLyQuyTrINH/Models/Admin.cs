using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models
{
    public partial class Admin
    {
        public int MaAd { get; set; }
        public string? QuyenAdmin { get; set; }

        public virtual User MaAdNavigation { get; set; } = null!;
    }
}
