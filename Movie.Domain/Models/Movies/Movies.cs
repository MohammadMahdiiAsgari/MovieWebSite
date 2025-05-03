using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.Movies
{
    public class Movies:BaseEntity
    {
        [Display(Name = "نام فیلم")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(80)]
        public string Title { get; set; }

        [Display(Name = "گروه فیلم")]
        public int GroupId { get; set; }

        [Display(Name = "زیر گروه")]
        public int SubGroupId { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(350)]
        public string Description { get; set; }

        [Display(Name = "زبان فیلم")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(30)]
        public string Language { get; set; }

        [Display(Name = "ژانر")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(30)]
        public string Genres { get; set; }

        [Display(Name = "مدت زمان فیلم")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        public int Duration { get; set; }

        [Display(Name = "رده سنی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        public int ContentRating { get; set; }

        [Display(Name = "تاریخ اکران")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        public int PubliceDate { get; set; }

        [Display(Name = "کیفیت")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(10)]
        public String Quality { get; set; }

        [Display(Name = "تصویر")]
        public string? ImageName { get; set; }

        [Display(Name = "تگ‌ها")]
        public string Tags { get; set; }

        [Display(Name = "بازدید")]
        public int Visit { get; set; }

        public List<MovieGallery>? movieGalleries { get; set; }

        [ForeignKey("GroupId")]
        public MovieGroup? MovieGroup { get; set; }

        [ForeignKey("SubGroupId")]
        public MovieSubGroup? movieSubGroup { get; set; }
    }
}
