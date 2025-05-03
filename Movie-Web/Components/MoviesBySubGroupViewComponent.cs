using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Data.Context;
using Movie.Domain.Models.Movies;

namespace Movie_Web.Components
{
     public class MoviesBySubGroupViewModel
    {
        public string SubGroupTitle { get; set; }

        public IEnumerable<Movies> Movies { get; set; }
    }


    public class MoviesBySubGroupViewComponent : ViewComponent
    {
        private readonly MyMovieContext _movieContext;

        public MoviesBySubGroupViewComponent(MyMovieContext movieContext)
        {
            _movieContext = movieContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int subGroupId, string subGroupTitle)
        {
            var movies = await _movieContext.Movies
                .Where(m=>m.SubGroupId == subGroupId && !m.IsDeleted)
                .ToListAsync();

            var viewModel = new MoviesBySubGroupViewModel
            {
                SubGroupTitle=subGroupTitle,
                Movies = movies
            };

            return View(viewModel);
        }
    }
}
