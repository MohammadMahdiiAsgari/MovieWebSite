using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Movie_Web.Areas.Admin.ViewModels
{
    public class ShowFormViewModel
    {
        public int Id { get; set; } // برای ویرایش لازم است

        [Display(Name = "نام نمایش")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(200)]
        public string ShowName { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(500)]
        public string Description { get; set; }

        [Display(Name = "زبان نمایش")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(50)]
        public string Language { get; set; }

        [Display(Name = "ژانر")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        [MaxLength(100)]
        public string Genre { get; set; }

        [Display(Name = "رده سنی")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        public int ContentRating { get; set; }

        [Display(Name = "تاریخ انتشار")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید.")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "مدت زمان هر قسمت ")]
        public int AverageEpisodeDuration { get; set; }

        [Display(Name = "تصویر نمایش")]
        [DataType(DataType.Upload)]
        public IFormFile? CoverImage { get; set; }

        // TODO: اگر خصوصیات دیگری در مدل Show دارید، اینجا اضافه کنید

        // === مدیریت زیرگروه ها ===

        [Display(Name = "زیرگروه‌ها")]
        // این خصوصیت برای دریافت شناسه‌های زیرگروه‌های انتخاب شده از فرم استفاده می‌شود.
        // نام این خصوصیت در View هنگام ساخت کنترل UI برای انتخاب زیرگروه مهم خواهد بود.
        public List<int>? SelectedSubGroupIds { get; set; }

        // این خصوصیت برای پر کردن Dropdown یا Checkbox List زیرگروه ها در View استفاده می شود.
        // شامل لیست تمام زیرگروه‌های موجود است که به فرمت مناسب SelectListItem تبدیل شده اند.
        public List<SelectListItem>? AvailableSubGroups { get; set; }

        // TODO: برای فرم Edit، ممکن است نیاز به راهی برای نمایش زیرگروه های فعلی نمایش داشته باشید
        // روش های مختلفی برای انجام این کار وجود دارد که در زمان اصلاح View Edit بررسی می کنیم.


        // 👇👇👇 خصوصیت جدید برای نمایش نام عکس فعلی در فرم ویرایش 👇👇👇
        public string? CurrentImageName { get; set; } // نام فایل عکس کاور فعلی
    }

}
