using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_QuanLyQuyTrINH.Models
{
    [Table("YeuCauDatLaiMatKhau")]
    public class YeuCauDatLaiMatKhau
    {
        [Key]
        public int YeuCauID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        public DateTime NgayYeuCau { get; set; } = DateTime.Now;

        [StringLength(20)]
        public string TrangThai { get; set; } = "Chờ xử lý";

        public virtual User User { get; set; }
    }
}
