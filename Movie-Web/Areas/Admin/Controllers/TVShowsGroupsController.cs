using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie.Data.Context;
using Movie.Domain.Models.TVShows;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TVShowsGroupsController : Controller
    {
        private readonly MyMovieContext _context;

        public TVShowsGroupsController(MyMovieContext context)
        {
            _context = context;
        }

        // GET: Admin/TVShowsGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.TVShowsGroups.ToListAsync());
        }

        // GET: Admin/TVShowsGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tVShowsGroup = await _context.TVShowsGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tVShowsGroup == null)
            {
                return NotFound();
            }

            return View(tVShowsGroup);
        }

        // GET: Admin/TVShowsGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TVShowsGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupTitle,Id,CreateDate,ModifiedDate,IsDeleted")] TVShowsGroup tVShowsGroup)
        {
            if (ModelState.IsValid)
            {
                tVShowsGroup.CreateDate = DateTime.Now;
                _context.Add(tVShowsGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tVShowsGroup);
        }

        // GET: Admin/TVShowsGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tVShowsGroup = await _context.TVShowsGroups.FindAsync(id);
            if (tVShowsGroup == null)
            {
                return NotFound();
            }

            ViewBag.SubGroups = _context.TVShowsSubGroups.Where(t => t.GroupId == id.Value).ToList();

            return View(tVShowsGroup);
        }

        // POST: Admin/TVShowsGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GroupTitle,Id,CreateDate,ModifiedDate,IsDeleted")] TVShowsGroup tVShowsGroup)
        {
            if (id != tVShowsGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tVShowsGroup.ModifiedDate = DateTime.Now;
                    _context.Update(tVShowsGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TVShowsGroupExists(tVShowsGroup.Id))
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
            return View(tVShowsGroup);
        }

        // GET: Admin/TVShowsGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tVShowsGroup = await _context.TVShowsGroups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tVShowsGroup == null)
            {
                return NotFound();
            }

            return View(tVShowsGroup);
        }

        // POST: Admin/TVShowsGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tVShowsGroup = await _context.TVShowsGroups.FindAsync(id);
            if (tVShowsGroup != null)
            {
                tVShowsGroup.IsDeleted = true;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TVShowsGroupExists(int id)
        {
            return _context.TVShowsGroups.Any(e => e.Id == id);
        }

        public IActionResult CreateSubGroup(string name, int groupId, int subgroupId = 0)
        {
            if (!string.IsNullOrEmpty(name))
            {
                TVShowsSubGroup tVShowsSubGroup = new TVShowsSubGroup()
                {
                    CreateDate = DateTime.Now,
                    GroupId = groupId,
                    IsDeleted = false,
                    SubGroupTitle = name
                };

                if (subgroupId != 0)
                {
                    tVShowsSubGroup.Id = subgroupId;
                    _context.TVShowsSubGroups.Update(tVShowsSubGroup);
                }
                else
                {
                    _context.TVShowsSubGroups.Add(tVShowsSubGroup);
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Edit", new { Id = groupId });
        }

        public IActionResult DeleteSubGroups(int id)
        {
            var subgroup = _context.TVShowsSubGroups.Find(id);
            subgroup.IsDeleted = !subgroup.IsDeleted;
            _context.SaveChanges();

            return RedirectToAction("Edit", new { Id = subgroup.GroupId });
        }

        public IActionResult EditSubGroup(int id)
        {
            var subgroup = _context.TVShowsSubGroups.Find(id);
            var group = _context.TVShowsGroups.Find(subgroup.GroupId);

            ViewBag.SubGroupTitle = subgroup.SubGroupTitle;

            ViewBag.SubGroups = _context.TVShowsSubGroups.
                Where(s => s.GroupId == subgroup.GroupId).
                ToList();

            ViewBag.SubGroupId = subgroup.Id;

            return View("Edit", group);
        }
    }

}
