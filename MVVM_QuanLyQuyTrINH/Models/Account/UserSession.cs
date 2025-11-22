using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_QuanLyQuyTrINH.Models.Account
{
    public static class UserSession
    {
        public static User? CurrentUser { get; set; }
    }
}
