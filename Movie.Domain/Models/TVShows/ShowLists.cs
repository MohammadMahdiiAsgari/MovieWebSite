using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.TVShows
{
    public class ShowLists:BaseEntity
    {

        [Display(Name = "تصویر نمایش")]
        public string? ImageName { get; set; }

        [Display(Name = " نام نمایش")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string ShowName { get; set; }

        [Display(Name = "توضیحات نمایش")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "ژانر")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Display(Name = "زبان نمایش")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(30)]
        public string Language { get; set; }

        [Display(Name = "رده سنی")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        public int ContentRating { get; set; }

        [Display(Name = "تاریخ اکران")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "مدت زمان")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        public int Duration { get; set; }

        // خصوصیت Navigation برای دسترسی به لیست فصل‌های این نمایش
        public List<Seasons>? Seasons { get; set; }

        // خصوصیت Navigation برای دسترسی به لیست ارتباطات با زیرگروه ها
        public List<ShowListTVShowsSubGroup>? ShowListTVShowsSubGroups { get; set; }
    }
}
