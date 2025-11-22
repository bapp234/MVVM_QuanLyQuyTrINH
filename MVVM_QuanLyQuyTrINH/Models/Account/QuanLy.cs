using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models.Account
{
    public partial class QuanLy
    {
        public int MaQl { get; set; }
        public string? CapQuanLy { get; set; }

        public virtual User MaQlNavigation { get; set; } = null!;
    }
}
