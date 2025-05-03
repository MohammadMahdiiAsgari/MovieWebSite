using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie.Application.Security;
using Movie.Application.Services.Interfaces;
using Movie.Data.Context;
using Movie.Domain.DTOs;
using Movie.Domain.Enums;
using Movie.Domain.Models;

namespace Movie_Web.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UsersController : Controller
    {
        #region Field

        private readonly IUserService _userService;

        #endregion

        #region Constructor

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region List

        public async Task<IActionResult> Index(int pageId = 1)
        {
            int take = 5;
            int pageCount = (int)Math.Ceiling((double)_userService.GetUserCount() / take);
            int skip = (pageId - 1) * take;

            ViewBag.PageCount = pageCount; //تعداد صفحات
            ViewBag.PageCurrent = pageId; //شماره صفحه

            var users = _userService.GetAllUsers(take, skip);

            return View(users);
        }

        #endregion

        #region Details

        public async Task<IActionResult> Details(int id)
        {
            var user = _userService.GetUserDetailsById(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        #endregion

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserDTO createUser)
        {
            if (!ModelState.IsValid)
                return View(createUser);

            var result = _userService.CreateUser(createUser);

            switch (result)
            {
                case ResultCreateUser.Success:
                    return RedirectToAction("Index");


                case ResultCreateUser.EmailNotValid:
                    ModelState.AddModelError("Email", "ایمیل وارد شده معتبر نمی‌باشد.");
                    break;

                case ResultCreateUser.UserNameNotValid:
                    ModelState.AddModelError("UserName", "نام کاربری تکراری است.");
                    break;
            }

            return View(createUser);
        }


        #endregion

        #region Edit

        public IActionResult Edit(int id)
        {

            var user = _userService.EditUser(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditUserDTO editUser)
        {
            if (!ModelState.IsValid)
                return View(editUser);

            var Result = _userService.EditUser(editUser);

            switch (Result)
            {
                case ResultEditUser.Success:
                    return RedirectToAction("Index");

                case ResultEditUser.UserNotFound:
                    ModelState.AddModelError("UserName", "کاربری با این مشخصات یافت نشد.");
                    break;

                case ResultEditUser.EmailDuplicated:
                    ModelState.AddModelError("Email", "ایمیل وارد شده تکراری است.");
                    break;

                case ResultEditUser.UserNameDuplicated:
                    ModelState.AddModelError("UserName", "نام کاربری وارد شده تکراری است.");
                    break;
            }

            return View(editUser);
        }


        #endregion

        #region Delete

        public IActionResult Delete(int id)
        {
            var user = _userService.GetUserDetailsById(id);

            if (user == null)
                return NotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var user=_userService.Delete(id);

            return RedirectToAction(nameof(Index));
        }

        #endregion

    }
}
