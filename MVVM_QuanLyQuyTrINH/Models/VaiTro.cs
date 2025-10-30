using System;
using System.Collections.Generic;

namespace MVVM_QuanLyQuyTrINH.Models
{
    public partial class VaiTro
    {
        public VaiTro()
        {
            Users = new HashSet<User>();
        }

        public int MaVaiTro { get; set; }
        public string TenVaiTro { get; set; } = null!;
        public string? MoTa { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
