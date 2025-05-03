using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie.Data.Context;
using Movie.Domain.Models.Movies;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieGroupsController : Controller
    {
        private readonly MyMovieContext _context;

        public MovieGroupsController(MyMovieContext context)
        {
            _context = context;
        }

        // GET: Admin/MovieGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.MovieGroups.ToListAsync());
        }

        // GET: Admin/MovieGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieGroup = await _context.MovieGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieGroup == null)
            {
                return NotFound();
            }

            return View(movieGroup);
        }

        // GET: Admin/MovieGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/MovieGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupTitle,Id,CreateDate,ModifiedDate,IsDeleted")] MovieGroup movieGroup)
        {
            if (ModelState.IsValid)
            {
                movieGroup.CreateDate = DateTime.Now;
                _context.Add(movieGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movieGroup);
        }

        // GET: Admin/MovieGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieGroup = await _context.MovieGroups.FindAsync(id);
            if (movieGroup == null)
            {
                return NotFound();
            }

            ViewBag.SubGroup = _context.MovieSubGroups
                .Where(s => s.GroupId == id.Value)
                .ToList();

            return View(movieGroup);
        }

        // POST: Admin/MovieGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupTitle,Id,CreateDate,ModifiedDate,IsDeleted")] MovieGroup movieGroup)
        {
            if (id != movieGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    movieGroup.ModifiedDate = DateTime.Now;
                    _context.Update(movieGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieGroupExists(movieGroup.Id))
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
            return View(movieGroup);
        }

        // GET: Admin/MovieGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movieGroup = await _context.MovieGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movieGroup == null)
            {
                return NotFound();
            }

            return View(movieGroup);
        }

        // POST: Admin/MovieGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movieGroup = await _context.MovieGroups.FindAsync(id);
            if (movieGroup != null)
            {
                movieGroup.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieGroupExists(int id)
        {
            return _context.MovieGroups.Any(e => e.Id == id);
        }

        [HttpPost]
        public IActionResult CreateSubGroup(string name, int groupId, int subgroupId=0)
        {
            if (!string.IsNullOrEmpty(name))
            {
                MovieSubGroup movieSubGroup = new MovieSubGroup()
                {
                    
                    CreateDate = DateTime.Now,
                    GroupId = groupId,
                    IsDeleted = false,
                    SubGroupTitle = name
                };

                if(subgroupId!=0)
                {
                    movieSubGroup.Id = subgroupId;
                    _context.MovieSubGroups.Update(movieSubGroup);
                }
                else
                {
                    _context.MovieSubGroups.Add(movieSubGroup);
                }
                _context.SaveChanges();
            }

            return RedirectToAction("Edit", new { Id = groupId });
        }

        public IActionResult DeleteSubGroup(int id)
        {
            var subgroup = _context.MovieSubGroups.Find(id);
            subgroup.IsDeleted = !subgroup.IsDeleted;
            _context.SaveChanges();

            return RedirectToAction("Edit", new { Id = subgroup.GroupId });
        }

        public IActionResult EditSubGroup(int id)
        {
            var subgroup = _context.MovieSubGroups.Find(id);
            var group = _context.MovieGroups.Find(subgroup.GroupId);

            ViewBag.SubTitle = subgroup.SubGroupTitle;
            ViewBag.SubGroup = _context.MovieSubGroups
                .Where(s => s.GroupId == subgroup.GroupId)
                .ToList();
            ViewBag.SubGroupId = subgroup.Id;

            return View("Edit", group);
        }
    }
}
