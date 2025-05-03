using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models
{
    public class BaseEntity
    {
        [Key]
        [Display(Name ="آیدی")]
        public int Id { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "تاریخ ویرایش")]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "حذف شده؟")]
        public bool IsDeleted { get; set; } = false;
    }
}
