using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.TVShows
{
    public class TVShowsGroup : BaseEntity
    {
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(300)]
        public string GroupTitle { get; set; }

        public List<TVShowsSubGroup>? TVShowsSubGroups { get; set; }
    }

    public class TVShowsSubGroup:BaseEntity
    {
        public int GroupId { get; set; }

        [Display(Name = "عنوان زیرگروه")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(300)]
        public string SubGroupTitle { get; set; }

        [ForeignKey("GroupId")]
        public TVShowsGroup? TVShowsGroups { get; set; }

        // خصوصیت Navigation برای دسترسی به لیست ارتباطات با نمایش ها
        public List<ShowListTVShowsSubGroup>? ShowListTVShowsSubGroups { get; set; }
    }
}
