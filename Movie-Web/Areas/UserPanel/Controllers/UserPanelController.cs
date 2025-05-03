using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.Services.Interfaces;
using Movie.Domain.DTOs;
using System.Security.Claims;

namespace Movie_Web.Areas.UserPanel.Controllers
{
    [Authorize]
    [Area("UserPanel")]
    public class UserPanelController : Controller
    {
        private readonly IUserService _userService;

        public UserPanelController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        [Route("Change-Password")]
        public IActionResult ChangePassword()
        {
            return View();
        }
        
        [HttpPost("Change-Password")]
        public IActionResult ChangePassword(ChangePasswordDTO changePassword)
        {
            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            int currentUserId= int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var result=_userService.UserChangePassword(currentUserId, changePassword);

            ViewBag.Restult= result;

            return View();
        }
    }
}
