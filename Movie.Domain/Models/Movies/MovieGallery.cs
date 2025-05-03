using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movie.Domain.Models.Movies
{
    public class MovieGallery:BaseEntity
    {
        public int MovieId { get; set; }

        public string ImageName { get; set; }

        [ForeignKey("MovieId")]
        public Movies? movie { get; set; }
    }
}
