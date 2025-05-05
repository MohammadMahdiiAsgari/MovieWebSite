using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie.Data.Context;
using Movie.Domain.Models.TVShows;
using Movie_Web.Areas.Admin.ViewModels;
using static System.Net.Mime.MediaTypeNames;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ShowListsController : Controller
    {
        private readonly MyMovieContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ShowListsController(MyMovieContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment; 
        }

        // GET: Admin/ShowLists
        public async Task<IActionResult> Index()
        {
            // واکشی تمام نمایش ها از دیتابیس
            // 👇👇👇 Include کردن اطلاعات زیرگروه های مرتبط 👇👇👇
            var movieDbContext = _context.ShowLists // DbSet برای مدل ShowLists
                                        .Include("ShowListTVShowsSubGroups.TVShowsSubGroup"); // Include کردن خصوصیت Navigation به لیست مدل واسط در ShowLists
                                        

            return View(await movieDbContext.ToListAsync());
        }

        // GET: Admin/ShowLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showLists = await _context.ShowLists
                .Include("ShowListTVShowsSubGroups.TVShowsSubGroup") // مسیر خصوصیت های Navigation به صورت رشته
                .FirstOrDefaultAsync(m => m.Id == id);
            if (showLists == null)
            {
                return NotFound();
            }

            return View(showLists);
        }

        // GET: Admin/ShowLists/Create
        public async Task<IActionResult> Create()
        {
            // 1. واکشی لیست تمام زیرگروه های موجود از دیتابیس
            var subGroups = await _context.TVShowsSubGroups // نام DbSet برای TVShowsSubGroup در DbContext شما
                                           .Where(s => !s.IsDeleted) // فیلتر زیرگروه های حذف نشده
                                           .OrderBy(s => s.SubGroupTitle) // مرتب سازی بر اساس عنوان
                                           .ToListAsync();

            // 2. ایجاد لیست SelectListItem برای پر کردن Dropdown یا Checkbox List در View
            var subGroupListItems = subGroups.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(), // مقدار گزینه (شناسه زیرگروه)
                Text = s.SubGroupTitle // متن نمایشی گزینه (عنوان زیرگروه)
            }).ToList();

            // 3.ایجاد ViewModel و قرار دادن لیست زیرگروه ها در آن
            var viewModel = new ShowFormViewModel
            {
                AvailableSubGroups = subGroupListItems
                // سایر خصوصیات ViewModel در این مرحله خالی هستند چون فرم جدید است
            };

            // 4. پاس دادن ViewModel به View Create
            return View(viewModel);
        }

        // POST: Admin/ShowLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShowFormViewModel showFormViewModel)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null; // نام فایل یکتا اگر عکسی آپلود شود
                string defaultImageName = "noimage.jpeg";

                if (showFormViewModel.CoverImage != null) // اگر فایلی برای آپلود انتخاب شده است
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ShowImages");
                    // TODO: مطمئن شوید پوشه wwwroot/images/showcovers وجود دارد

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + showFormViewModel.CoverImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await showFormViewModel.CoverImage.CopyToAsync(fileStream);
                    }
                }

                // 2. ایجاد یک موجودیت Show جدید و پر کردن خصوصیات آن از ViewModel
                var newShow = new ShowLists
                {
                    ShowName = showFormViewModel.ShowName,
                    Description = showFormViewModel.Description,
                    Language = showFormViewModel.Language,
                    Genre = showFormViewModel.Genre,
                    ContentRating = showFormViewModel.ContentRating,
                    ReleaseDate = showFormViewModel.ReleaseDate,
                    Duration = showFormViewModel.AverageEpisodeDuration,
                    // 👇👇👇 تعیین نام عکس: اگر uniqueFileName مقدار دارد از آن استفاده کن، در غیر این صورت از defaultImageName استفاده کن 👇👇👇
                    ImageName = uniqueFileName ?? defaultImageName, // استفاده از Null-coalescing operator (??)
                    CreateDate = DateTime.Now,
                    IsDeleted = false
                    // ModifiedDate در زمان ایجاد معمولا null است
                };

                _context.Add(newShow);

                if(showFormViewModel.SelectedSubGroupIds != null && showFormViewModel.SelectedSubGroupIds.Any())
                {
                    foreach (var subGroupId in showFormViewModel.SelectedSubGroupIds)
                    {
                        var showSubGroup = new ShowListTVShowsSubGroup
                        {
                            ShowId = newShow.Id,
                            ShowLists=newShow,
                            TVShowsSubGroupId = subGroupId
                        };
                        _context.Add(showSubGroup);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // 7. اگر مدل نامعتبر بود (همانند قبل، با پر کردن مجدد لیست زیرگروه ها)
            var subGroups = await _context.TVShowsSubGroups
                                           .Where(s => !s.IsDeleted)
                                           .OrderBy(s => s.SubGroupTitle)
                                           .ToListAsync();

            var subGroupListItems = subGroups.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SubGroupTitle
            }).ToList();

            showFormViewModel.AvailableSubGroups = subGroupListItems;

            return View(showFormViewModel);
        }

        // GET: Admin/ShowLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // اگر شناسه ارسال نشده باشد، 404 برمی گردانیم
            }

            // TODO: (اختیاری) استفاده از لایه Application برای واکشی داده در مرحله Refactor
            // در این مرحله، مستقیما از DbContext استفاده می کنیم.

            // 1. واکشی نمایش از دیتابیس بر اساس شناسه
            // 👇👇👇 استفاده از String-based Include برای بارگذاری زیرگروه های مرتبط 👇👇👇
            var showLists = await _context.ShowLists // DbSet برای مدل ShowLists
                                          .Include("ShowListTVShowsSubGroups.TVShowsSubGroup") // مسیر خصوصیت های Navigation به صورت رشته
                                          .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted); // پیدا کردن نمایش بر اساس شناسه و فیلتر حذف شده ها

            if (showLists == null)
            {
                return NotFound(); // اگر نمایشی با این شناسه پیدا نشد یا حذف شده بود، 404 برمی گردانیم
            }

            // 2. واکشی لیست تمام زیرگروه های موجود (همانند اکشن Create GET)
            var subGroups = await _context.TVShowsSubGroups // DbSet برای مدل TVShowsSubGroup
                                           .Where(s => !s.IsDeleted)
                                           .OrderBy(s => s.SubGroupTitle)
                                           .ToListAsync();

            // 3. ایجاد لیست SelectListItem برای پر کردن Dropdown یا Checkbox List در View
            var subGroupListItems = subGroups.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SubGroupTitle,
                // 4. مشخص کردن زیرگروه های انتخاب شده فعلی
                // بررسی می کنیم که آیا شناسه این زیرگروه در لیست زیرگروه های مرتبط با نمایش فعلی وجود دارد
                Selected = showLists.ShowListTVShowsSubGroups != null && showLists.ShowListTVShowsSubGroups.Any(ssts => ssts.TVShowsSubGroupId == s.Id)
            }).ToList();

            // 5. ایجاد ViewModel و پر کردن خصوصیات آن با اطلاعات نمایش واکشی شده
            var viewModel = new ShowFormViewModel
            {
                Id = showLists.Id, // شناسه نمایش (بسیار مهم برای استفاده در فرم Edit POST)
                ShowName = showLists.ShowName, // پر کردن خصوصیات ViewModel از مدل ShowLists
                Description = showLists.Description,
                Language = showLists.Language,
                Genre = showLists.Genre, // TODO: بررسی کنید نام خصوصیت Genres در ViewModel و Genre در مدل ShowLists سازگار است
                ContentRating = showLists.ContentRating,
                ReleaseDate = showLists.ReleaseDate,
                AverageEpisodeDuration = showLists.Duration, // TODO: بررسی کنید نام خصوصیت AverageEpisodeDuration در ViewModel و Duration در مدل ShowLists سازگار است

                // پر کردن لیست زیرگروه های موجود و زیرگروه های انتخاب شده فعلی در ViewModel
                AvailableSubGroups = subGroupListItems,
                // توجه: خصوصیت SelectedSubGroupIds در این مرحله (GET) نیاز به پر شدن صریح ندارد،
                // زیرا SelectListItem ها با خصوصیت Selected وضعیت انتخاب را مشخص می کنند.
                // SelectedSubGroupIds توسط Model Binder در اکشن POST دریافت می شود.

                // 👇👇👇 اضافه کردن نام عکس فعلی به ViewModel برای نمایش در فرم Edit 👇👇👇
                CurrentImageName = showLists.ImageName // فرض می کنیم در ViewModel خصوصیت CurrentImageName دارید
            };

            // 6. پاس دادن ViewModel به View Edit
            return View(viewModel);
        }


        // POST: Admin/ShowLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // (نیازی به پارامتر id در امضای متد POST نیست، زیرا id در ViewModel وجود دارد)

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ShowFormViewModel viewModel) // ViewModel حاوی اطلاعات ویرایش شده
        {
            // 1. بررسی اعتبار مدل
            if (ModelState.IsValid)
            {
                // 2. واکشی موجودیت اصلی از دیتابیس (برای اعمال تغییرات و مدیریت روابط)
                // مهم: دوباره باید روابط را Include کنیم تا EF Core بتواند تغییرات در لیست زیرگروه های مرتبط را ردیابی و مدیریت کند.
                var showToUpdate = await _context.ShowLists // DbSet برای مدل ShowLists
                                  .Include("ShowListTVShowsSubGroups.TVShowsSubGroup") // مسیر خصوصیت های Navigation به صورت رشته
                                         .FirstOrDefaultAsync(m => m.Id == viewModel.Id); // پیدا کردن نمایش بر اساس شناسه ViewModel
                if (showToUpdate == null)
                {
                    return NotFound(); // اگر نمایشی با این شناسه پیدا نشد (نباید اتفاق بیفتد اگر از صفحه Edit آمده باشد)
                }

                // === به‌روزرسانی خصوصیات مدل اصلی ShowLists ===
                showToUpdate.ShowName = viewModel.ShowName; // TODO: بررسی سازگاری نام Title/ShowName
                showToUpdate.Description = viewModel.Description;
                showToUpdate.Language = viewModel.Language;
                showToUpdate.Genre = viewModel.Genre; // TODO: بررسی سازگاری نام Genres/Genre
                showToUpdate.ContentRating = viewModel.ContentRating;
                showToUpdate.ReleaseDate = viewModel.ReleaseDate; // چون در ViewModel nullable است و اعتبارسنجی انجام شده
                showToUpdate.Duration = viewModel.AverageEpisodeDuration; // TODO: بررسی سازگاری نام
                showToUpdate.ModifiedDate = DateTime.Now; // به‌روزرسانی تاریخ ویرایش


                // === مدیریت آپلود عکس جدید و حذف عکس قدیمی ===
                if (viewModel.CoverImage != null) // اگر فایل عکس جدیدی برای آپلود انتخاب شده است
                {
                    // a. تعیین مسیر ذخیره فایل
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ShowImages"); // مسیر دقیق پوشه ذخیره سازی

                    // b. ایجاد نام فایل یکتا
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + viewModel.CoverImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // c. ذخیره فایل جدید در سیستم فایل
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await viewModel.CoverImage.CopyToAsync(fileStream);
                    }

                    // d. حذف عکس قدیمی (اگر وجود دارد و عکس پیش فرض نیست)
                    string defaultImageFileName = "noimage.jpeg"; // نام دقیق فایل عکس پیش فرض شما
                    if (!string.IsNullOrEmpty(showToUpdate.ImageName) && showToUpdate.ImageName != defaultImageFileName)
                    {
                        string oldFilePath = Path.Combine(uploadsFolder, showToUpdate.ImageName);
                        if (System.IO.File.Exists(oldFilePath)) // بررسی می کنیم فایل وجود دارد
                        {
                            System.IO.File.Delete(oldFilePath); // حذف فایل قدیمی
                        }
                    }

                    // e. به‌روزرسانی نام عکس در مدل ShowLists با نام فایل جدید
                    showToUpdate.ImageName = uniqueFileName;
                }
                // توجه: اگر فایل جدیدی آپلود نشود، ImageName در مدل ShowLists همان مقدار قبلی باقی می ماند
                // مگر اینکه بخواهید گزینه ای برای حذف عکس بدون جایگزینی داشته باشید (که در اینجا پیاده سازی نشده است)


                // === مدیریت رابطه Many-to-Many با زیرگروه ها (اضافه و حذف ارتباطات در جدول واسط) ===
                // این پیچیده ترین قسمت است: مقایسه زیرگروه های انتخاب شده جدید با زیرگروه های فعلی نمایش

                // لیست شناسه‌ی زیرگروه های مرتبط فعلی با نمایش
                var currentSubGroupIds = showToUpdate.ShowListTVShowsSubGroups?
                                                     .Select(ssts => ssts.TVShowsSubGroupId)
                                                     .ToList() ?? new List<int>(); // اگر لیست ارتباطات null بود، یک لیست خالی ایجاد می کنیم

                // لیست شناسه‌ی زیرگروه های انتخاب شده توسط کاربر در فرم
                var selectedSubGroupIds = viewModel.SelectedSubGroupIds ?? new List<int>(); // اگر لیست انتخاب شده null بود، یک لیست خالی ایجاد می کنیم

                // زیرگروه هایی که باید اضافه شوند (شناسه هایی در SelectedSubGroupIds که در currentSubGroupIds نیستند)
                var subGroupsToAdd = selectedSubGroupIds.Except(currentSubGroupIds).ToList();

                // زیرگروه هایی که باید حذف شوند (شناسه هایی در currentSubGroupIds که در SelectedSubGroupIds نیستند)
                var subGroupsToRemove = currentSubGroupIds.Except(selectedSubGroupIds).ToList();

                // حذف ارتباطات قدیمی
                if (subGroupsToRemove.Any())
                {
                    // پیدا کردن موجودیت های ShowListTVShowsSubGroup مربوط به زیرگروه هایی که باید حذف شوند
                    var relationshipsToRemove = showToUpdate.ShowListTVShowsSubGroups
                                                             .Where(ssts => subGroupsToRemove.Contains(ssts.TVShowsSubGroupId))
                                                             .ToList();
                    // حذف این موجودیت ها از DbContext (که منجر به حذف از دیتابیس می شود)
                    _context.RemoveRange(relationshipsToRemove);
                }

                // اضافه کردن ارتباطات جدید
                if (subGroupsToAdd.Any())
                {
                    foreach (var subGroupId in subGroupsToAdd)
                    {
                        // ایجاد یک موجودیت جدید در جدول واسط برای هر ارتباط جدید
                        var newShowSubGroup = new ShowListTVShowsSubGroup
                        {
                            ShowId = showToUpdate.Id, // شناسه نمایش مورد ویرایش
                            TVShowsSubGroupId = subGroupId
                            // خصوصیت Navigation ShowLists در اینجا نیاز به تنظیم صریح ندارد اگر ShowId تنظیم شود
                        };
                        _context.Add(newShowSubGroup); // اضافه کردن موجودیت واسط جدید به DbContext
                    }
                }
                // === پایان مدیریت رابطه Many-to-Many ===


                // 6. ذخیره تمام تغییرات در دیتابیس (خصوصیات اصلی، اضافه/حذف ارتباطات، نام عکس)
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) // اگر همزمان شخص دیگری در حال ویرایش این نمایش بود
                {
                    // TODO: پیاده سازی منطق حل تداخل (Concurrency Conflict)
                    // این یک سناریوی پیشرفته تر است که در اینجا به آن نمی پردازیم
                    // برای سادگی فعلا فقط اگر خطا رخ داد، می توانید پیام خطا نمایش دهید
                    ModelState.AddModelError("", "خطا در ذخیره تغییرات. ممکن است نمایش توسط کاربر دیگری ویرایش شده باشد.");
                    // باید ViewModel را برای نمایش مجدد فرم با خطا پر کنید
                    // این شامل واکشی مجدد زیرگروه ها و تنظیم وضعیت انتخاب شده است
                    // این قسمت شبیه به بلوک else در متدهای Create و Edit GET است.
                    // برای سادگی در اینجا، فقط پیام خطا را اضافه می کنیم و فرض می کنیم View Model برای نمایش خطا کافی است.
                    // در سناریوهای واقعی باید ViewModel را کامل پر کرده و View را برگردانید.
                    return View(viewModel); // یک پیاده سازی ساده
                }


                // 7. هدایت کاربر به صفحه لیست نمایش ها پس از موفقیت
                return RedirectToAction(nameof(Index));
            }

            // 8. اگر مدل نامعتبر بود، فرم را دوباره نمایش می دهیم (با پر کردن مجدد لیست زیرگروه ها و عکس فعلی)
            // این قسمت شبیه به بلوک else در متد POST Create و متد GET Edit است.

            var subGroups = await _context.TVShowsSubGroups
                                           .Where(s => !s.IsDeleted)
                                           .OrderBy(s => s.SubGroupTitle)
                                           .ToListAsync();

            var subGroupListItems = subGroups.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.SubGroupTitle,
                // در اینجا باید بر اساس SelectedSubGroupIds در ViewModel، وضعیت انتخاب را تنظیم کنیم
                Selected = viewModel.SelectedSubGroupIds != null && viewModel.SelectedSubGroupIds.Contains(s.Id)
            }).ToList();

            viewModel.AvailableSubGroups = subGroupListItems;
            // نام عکس فعلی در ViewModel از قبل وجود دارد چون از فرم ارسال شده است (در صورت نیاز می توانید آن را دوباره از دیتابیس واکشی کنید)

            return View(viewModel); // بازگرداندن View با ViewModel حاوی داده های وارد شده و پیام های خطا
        }

// TODO: اکشن متدهای Details و Delete نیاز به پیاده سازی دارند.
            

        // GET: Admin/ShowLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var showLists = await _context.ShowLists
                .FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted); // پیدا کردن نمایش بر اساس شناسه و فیلتر حذف نشده ها
            if (showLists == null)
            {
                return NotFound();
            }

            return View(showLists);
        }

        // POST: Admin/ShowLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var showLists = await _context.ShowLists
                .FirstOrDefaultAsync(m => m.Id == id); // پیدا کردن نمایش بر اساس شناسه
            if (showLists == null)
            {
                return NotFound();
            }

            // 3. انجام حذف منطقی (Soft Delete)
            showLists.IsDeleted = true; // تنظیم خصوصیت IsDeleted به true
            showLists.ModifiedDate = DateTime.Now; // به‌روزرسانی تاریخ ویرایش

            // 4. اعلام به DbContext که مدل تغییر کرده است (برای اطمینان، هرچند EF Core معمولا تغییر در موجودیت ردیابی شده را تشخیص می دهد)
            _context.Update(showLists); // یا _context.Entry(showLists).State = EntityState.Modified;

            // 5. ذخیره تغییر در دیتابیس
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO: مدیریت خطا در زمان ذخیره حذف
                Console.WriteLine($"Error during soft delete: {ex.Message}");
                // می توانید به صفحه خطا هدایت کنید یا پیام خطا در صفحه لیست نمایش دهید
                ModelState.AddModelError("", "خطا در حذف نمایش.");
                // اگر می خواهید پیام خطا در صفحه لیست نمایش داده شود، نیاز به Pass کردن خطا به View دارید
                return RedirectToAction(nameof(Index)); // فعلا فقط به صفحه لیست هدایت می کنیم
            }


            // 6. هدایت کاربر به صفحه لیست نمایش ها پس از حذف موفق
            return RedirectToAction(nameof(Index));
        }

        [HttpPost] // این اکشن فقط درخواست های POST را مدیریت می کند
        [ValidateAntiForgeryToken] // برای امنیت CSRF
        public async Task<IActionResult> ToggleDeleteStatus(int id) // id نمایش برای تغییر وضعیت حذف
        {

            // 1. واکشی نمایش از دیتابیس بر اساس شناسه (بدون فیلتر IsDeleted)
            var showLists = await _context.ShowLists // DbSet برای مدل ShowLists
                                          .FirstOrDefaultAsync(m => m.Id == id); // پیدا کردن نمایش بر اساس شناسه

            // 2. بررسی وجود نمایش
            if (showLists == null)
            {
                // اگر نمایشی با این شناسه پیدا نشد
                return NotFound(); // یا Redirect به صفحه لیست با پیام خطا
            }

            // 3. تغییر وضعیت IsDeleted
            showLists.IsDeleted = !showLists.IsDeleted; // برعکس کردن مقدار فعلی IsDeleted
            showLists.ModifiedDate = DateTime.UtcNow; // به‌روزرسانی تاریخ ویرایش

            // 4. اعلام به DbContext که مدل تغییر کرده است
            _context.Update(showLists); // یا _context.Entry(showLists).State = EntityState.Modified;

            // 5. ذخیره تغییر در دیتابیس
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO: مدیریت خطا در زمان ذخیره تغییر وضعیت
                Console.WriteLine($"Error during toggle delete status: {ex.Message}");
                // می توانید لاگ کنید یا پیام خطا در صفحه لیست نمایش دهید.
                ModelState.AddModelError("", "خطا در تغییر وضعیت حذف نمایش.");
                // نیاز است ViewModel را برای نمایش خطا در صفحه مقصد (Index) پر کنید
                return RedirectToAction(nameof(Index)); // فعلا فقط به صفحه لیست هدایت می کنیم
            }


            // 6. هدایت کاربر به صفحه لیست نمایش ها پس از تغییر وضعیت موفق
            // نمایش در صفحه لیست اصلی نمایش داده می شود یا پنهان می شود بسته به وضعیت نهایی IsDeleted و فیلتر Index.
            return RedirectToAction(nameof(Index));
        }

        private bool ShowListsExists(int id)
        {
            return _context.ShowLists.Any(e => e.Id == id);
        }
    }
}
