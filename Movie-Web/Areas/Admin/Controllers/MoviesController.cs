using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie.Data.Context;
using Movie.Domain.Models.Movies;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MoviesController : Controller
    {
        private readonly MyMovieContext _context;

        public MoviesController(MyMovieContext context)
        {
            _context = context;
        }

        // GET: Admin/Movies
        public async Task<IActionResult> Index()
        {
            var myMovieContext = _context.Movies.Include(m => m.MovieGroup).Include(m => m.movieSubGroup);
            return View(await myMovieContext.ToListAsync());
        }

        // GET: Admin/Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieGroup)
                .Include(m => m.movieSubGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Admin/Movies/Create
        public IActionResult Create()
        {
            var groups = _context.MovieGroups;
            ViewData["GroupId"] = new SelectList(groups, "Id", "GroupTitle");
            ViewData["SubGroupId"] = new SelectList(_context.MovieSubGroups
                .Where(s => s.GroupId == groups.First().Id), "Id", "SubGroupTitle");
            return View();
        }

        // POST: Admin/Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,GroupId,SubGroupId,Description,Language,Genres,Duration,ContentRating,PubliceDate,Quality,ImageName,Tags,IsDeleted")] Movies movie,
          IFormFile ImgUpload,
          IFormFile[] ImgGalleries)
        {
            if (ModelState.IsValid)
            {
                movie.ImageName = "noimage.jpeg";
                if (ImgUpload != null)
                {
                    movie.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUpload.FileName);

                    string savepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MovieImages", movie.ImageName);

                    using (var stream = new FileStream(savepath, FileMode.Create))
                    {
                        ImgUpload.CopyTo(stream);
                    }
                }

                movie.Visit = 0;
                movie.CreateDate = DateTime.Now;
                _context.Add(movie);
                _context.SaveChanges();

                if (ImgGalleries != null && ImgGalleries.Any())
                {
                    foreach (var img in ImgGalleries) 
                    {
                        string imageName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                        string savepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MovieImages", imageName);

                        using (var stream = new FileStream(savepath, FileMode.Create))
                        {
                            img.CopyTo(stream);
                        }
                        MovieGallery gallery = new MovieGallery()
                        {
                            CreateDate = DateTime.Now,
                            ImageName = imageName,
                            MovieId=movie.Id,
                            IsDeleted=false,
                        };

                        _context.movieGalleries.Add(gallery);
                    }
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.MovieGroups, "Id", "GroupTitle", movie.GroupId);
            ViewData["SubGroupId"] = new SelectList(_context.MovieSubGroups, "Id", "SubGroupTitle", movie.SubGroupId);
            return View(movie);
        }

        // GET: Admin/Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            ViewData["GroupId"] = new SelectList(_context.MovieGroups, "Id", "GroupTitle", movie.GroupId);
            ViewData["SubGroupId"] = new SelectList(_context.MovieSubGroups, "Id", "SubGroupTitle", movie.SubGroupId);
            ViewBag.Galleries=_context.movieGalleries
                .Where(g=>g.MovieId == id.Value).ToList();
            return View(movie);
        }

        public IActionResult GetSubGroup(int groupId)
        {
            ViewBag.SubGroupId = new SelectList(_context.MovieSubGroups.Where(g => g.GroupId == groupId), "Id", "SubGroupTitle");
            return PartialView();
        }

        // POST: Admin/Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,GroupId,SubGroupId,Description,Language,Genres,Duration,ContentRating,PubliceDate,Quality,ImageName,Tags,Visit,Id,CreateDate,ModifiedDate,IsDeleted")] Movies movie,
            IFormFile? ImgUpload,
            IFormFile[] ImgGalleries)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImgUpload != null)
                    {
                        if (movie.ImageName != "noImage.jpeg")
                        {
                            string deletePath = Guid.NewGuid().ToString() + Path.GetExtension(movie.ImageName);

                            if (System.IO.File.Exists(deletePath))
                                System.IO.File.Delete(deletePath);
                        }

                        movie.ImageName = Guid.NewGuid().ToString() + Path.GetExtension(ImgUpload.FileName);

                        string savepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MovieImages", movie.ImageName);

                        using (var stream = new FileStream(savepath, FileMode.Create))
                        {
                            ImgUpload.CopyTo(stream);
                        }
                    }
                    movie.ModifiedDate = DateTime.Now;
                    _context.Update(movie);
                    await _context.SaveChangesAsync();

                    if (ImgGalleries != null && ImgGalleries.Any())
                    {
                        foreach (var img in ImgGalleries)
                        {
                            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);

                            string savepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/MovieImages", imageName);

                            using (var stream = new FileStream(savepath, FileMode.Create))
                            {
                                img.CopyTo(stream);
                            }
                            MovieGallery gallery = new MovieGallery()
                            {
                                CreateDate = DateTime.Now,
                                ImageName = imageName,
                                MovieId = movie.Id,
                                IsDeleted = false,
                            };

                            _context.movieGalleries.Add(gallery);
                        }
                        _context.SaveChanges();
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupId"] = new SelectList(_context.MovieGroups, "Id", "GroupTitle", movie.GroupId);
            ViewData["SubGroupId"] = new SelectList(_context.MovieSubGroups, "Id", "SubGroupTitle", movie.SubGroupId);
            return View(movie);
        }

        // GET: Admin/Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieGroup)
                .Include(m => m.movieSubGroup)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Admin/Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                movie.IsDeleted =! movie.IsDeleted;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }

        public IActionResult DeleteGallery(int id)
        {
            var gallery = _context.movieGalleries.Find(id);
            _context.Remove(gallery);
            string deletepath= Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/MovieImages",gallery.ImageName);
            if (System.IO.File.Exists(deletepath))
                System.IO.File.Delete(deletepath);

            _context.SaveChanges();
            return Redirect("/Admin/Movies/Edit/" + gallery.MovieId + "#gallery");
        }

    }
}
