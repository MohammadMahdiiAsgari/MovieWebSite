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
        public async Task<IActionResult> Index(int showId) // دریافت شناسه نمایش والد
        {
            // 1. واکشی نمایش والد (ShowLists) برای نمایش اطلاعات آن در صفحه لیست فصل ها (اختیاری)
            var parentShow = await _context.ShowLists
                                           .FirstOrDefaultAsync(s => s.Id == showId && !s.IsDeleted);

            if (parentShow == null)
            {
                // اگر نمایش والد پیدا نشد یا حذف شده بود
                return NotFound(); // یا Redirect به صفحه لیست نمایش ها با پیام خطا
            }

            // 2. واکشی لیست فصل های مربوط به این نمایش والد
            // نیازی به Include کردن روابط در این مرحله نیست مگر اینکه در لیست فصل ها اطلاعات مرتبط (مثل تعداد قسمت ها) را نمایش دهید.
            var seasons = await _context.Seasons // DbSet برای مدل Season
                                       .Where(s => s.ShowId == showId && !s.IsDeleted) // فیلتر بر اساس شناسه نمایش والد (ShowId) و وضعیت حذف شده
                                        .OrderBy(s => s.SeasonNumber) // مرتب سازی فصل ها بر اساس شماره فصل
                                        .ToListAsync();

            // 3. ارسال لیست فصل ها و اطلاعات نمایش والد به View
            // می توانید یک ViewModel سفارشی برای این صفحه بسازید که شامل اطلاعات نمایش والد و لیست فصل ها باشد.
            // برای سادگی فعلا فقط لیست فصل ها را ارسال می کنیم و نام نمایش والد را از ViewData/ViewBag پاس می دهیم.
            ViewBag.ParentShowName = parentShow.ShowName; // پاس دادن نام نمایش والد به View
            ViewBag.ParentShowId = showId; // پاس دادن شناسه نمایش والد به View

            return View(seasons); // ارسال لیست فصل ها به View Season/Index.cshtml
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
