using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.Data.Context;

namespace Movie_Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly MyMovieContext _context;

        public MovieController(MyMovieContext context)
        {
            _context = context;
        }

        [Route("/Movie")]
        public async Task<IActionResult> Movies()
        {
            var movieGroupsWithSubgroups = await _context.MovieGroups
                .Include(g=>g.MovieSubGroups)
                .ToListAsync();

            return View(movieGroupsWithSubgroups);
        }

        [Route("/TVShows")]
        public IActionResult TVShows()
        {
            return View();
        }

        public IActionResult ShowMovie(int id)
        {
            var movie= _context.Movies
                .Include(p=>p.movieGalleries)
                .SingleOrDefault(p=>p.Id == id);

            if(movie == null) 
                return NotFound();

            movie.Visit++;

            _context.SaveChanges();

            return View(movie);
        }
    }
}
