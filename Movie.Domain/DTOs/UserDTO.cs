using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsAdmin { get; set; }

        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; } = false;
    }

    public class CreateUserDTO
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "ادمین باشد؟")]
        public bool IsAdmin { get; set; }

        [Display(Name = "کلمه عبور")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class EditUserDTO
    {
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "ایمیل")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "ادمین باشد؟")]
        public bool IsAdmin { get; set; }

        [Display(Name = "کلمه عبور جدید")]
        [MaxLength(100)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Display(Name ="حذف شود؟")]
        public bool IsDeleted { get; set; }

    }

}
