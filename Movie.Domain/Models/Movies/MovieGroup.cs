using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.Movies
{
    public class MovieGroup : BaseEntity
    {
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(200)]
        public string GroupTitle { get; set; }

        public List<MovieSubGroup>? MovieSubGroups { get; set; }

        public List<Movies>? Movies { get; set; }
    }

    public class MovieSubGroup :BaseEntity
    {
        public int GroupId { get; set; }

        [Display(Name = "عنوان زیر گروه")]
        [Required(ErrorMessage = "لطفاً {0} خود را وارد نمایید.")]
        [MaxLength(200)]
        public string SubGroupTitle { get; set; }

        [ForeignKey("GroupId")]
        public MovieGroup? MovieGroup { get; set; }

        public List<Movies>? Movies { get; set; }
    }
}
