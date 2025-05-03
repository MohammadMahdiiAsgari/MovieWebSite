using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie.Data.Context;
using Movie.Domain.Models.TVShows;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SeasonsController : Controller
    {
        private readonly MyMovieContext _context;

        public SeasonsController(MyMovieContext context)
        {
            _context = context;
        }

        // GET: Admin/Seasons
        public async Task<IActionResult> Index()
        {
            var myMovieContext = _context.Seasons.Include(s => s.ShowLists);
            return View(await myMovieContext.ToListAsync());
        }

        // GET: Admin/Seasons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seasons = await _context.Seasons
                .Include(s => s.ShowLists)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seasons == null)
            {
                return NotFound();
            }

            return View(seasons);
        }

        // GET: Admin/Seasons/Create
        public IActionResult Create()
        {
            ViewData["ShowId"] = new SelectList(_context.ShowLists, "Id", "Description");
            return View();
        }

        // POST: Admin/Seasons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SeasonNumber,SeasonDescription,ShowId,Id,CreateDate,ModifiedDate,IsDeleted")] Seasons seasons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(seasons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowId"] = new SelectList(_context.ShowLists, "Id", "Description", seasons.ShowId);
            return View(seasons);
        }

        // GET: Admin/Seasons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seasons = await _context.Seasons.FindAsync(id);
            if (seasons == null)
            {
                return NotFound();
            }
            ViewData["ShowId"] = new SelectList(_context.ShowLists, "Id", "Description", seasons.ShowId);
            return View(seasons);
        }

        // POST: Admin/Seasons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SeasonNumber,SeasonDescription,ShowId,Id,CreateDate,ModifiedDate,IsDeleted")] Seasons seasons)
        {
            if (id != seasons.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seasons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeasonsExists(seasons.Id))
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
            ViewData["ShowId"] = new SelectList(_context.ShowLists, "Id", "Description", seasons.ShowId);
            return View(seasons);
        }

        // GET: Admin/Seasons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seasons = await _context.Seasons
                .Include(s => s.ShowLists)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seasons == null)
            {
                return NotFound();
            }

            return View(seasons);
        }

        // POST: Admin/Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seasons = await _context.Seasons.FindAsync(id);
            if (seasons != null)
            {
                _context.Seasons.Remove(seasons);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeasonsExists(int id)
        {
            return _context.Seasons.Any(e => e.Id == id);
        }
    }
}
