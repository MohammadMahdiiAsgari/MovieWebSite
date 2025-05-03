using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.TVShows
{
    public class Episodes:BaseEntity
    {
        [Display(Name = "تصویر قسمت")]
        public string? ImageNameForEpisode { get; set; }

        [Display(Name = "نام یا شماره قسمت")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string? EpisodeName { get; set; }

        [Display(Name = "توضیحات قسمت")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string? EpisodeDescription { get; set; }

        [Display(Name = "شماره قسمت")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        public int? EpisodeNumber { get; set; } // اضافه کردن شماره قسمت

        [Display(Name = "رده سنی قسمت")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        public int? EpisodeContentRating { get; set; }

        [Display(Name = "تاریخ انتشار قسمت")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        public DateTime? ReleaseDate { get; set; }

        [Display(Name = "مدت زمان قسمت")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        public int? EpisodeDuration { get; set; }

        // کلید خارجی برای ارتباط با فصل والد
        public int SeasonId { get; set; }

        // خصوصیت Navigation برای دسترسی به فصل والد
        [ForeignKey("SeasonId")]
        public Seasons? Season { get; set; }

    }
}
