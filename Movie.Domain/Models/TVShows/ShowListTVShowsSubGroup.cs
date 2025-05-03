using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.TVShows
{
    public class ShowListTVShowsSubGroup
    {
        // کلید خارجی به مدل Show
        [Key]
        public int ShowId { get; set; }

        public ShowLists ShowLists { get; set; } // خصوصیت Navigation به Show

        // کلید خارجی به مدل TVShowsSubGroup
        [Key]
        public int TVShowsSubGroupId { get; set; }

        public TVShowsSubGroup TVShowsSubGroup { get; set; } // خصوصیت Navigation به TVShowsSubGroup
    }
}
