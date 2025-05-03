using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.TVShows
{
    public class Seasons:BaseEntity
    {

        [Display(Name = "شماره فصل")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        public int? SeasonNumber { get; set; }

        [Display(Name = "توضیحات فصل")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string? SeasonDescription { get; set; }

        // کلید خارجی برای ارتباط با نمایش والد
        public int ShowId { get; set; }

        // خصوصیت Navigation برای دسترسی به نمایش والد
        [ForeignKey("ShowId")]
        public ShowLists? ShowLists { get; set; }

        // خصوصیت Navigation برای دسترسی به لیست قسمت‌های این فصل
        public List<Episodes>? Episodes { get; set; }
    }
}
