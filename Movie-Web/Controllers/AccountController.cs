using Movie.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Movie.Application.Services.Interfaces;
using Movie.Domain.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;


namespace Movie_Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            this._userService = userService;
        }

        #region Register User

        [Route("sign-up")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("sign-up")]
        public IActionResult Register(RegisterDTO register)
        {
            if (!ModelState.IsValid)
                return View(register);

            var result = _userService.RegisterUser(register);

            switch (result)
            {
                case ResultRegisterUser.UserNameNotValid:
                    {
                        ModelState.AddModelError("UserName", "نام کاربری تکراری است.");
                        return View(register);
                    }

                case ResultRegisterUser.EmailNotValid:
                    {
                        ModelState.AddModelError("Email", "ایمیل وارد شده معتبر نمی‌باشد.");
                        return View(register);
                    }

                case ResultRegisterUser.Success:
                    {
                        return View("SuccessRegister", register);
                    }
            }

            return View();
        }

        #endregion


        #region Login

        [Route("login")]
        public IActionResult Login(string ReturnUrl="/")
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDTO login, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = _userService.UserLogin(login);
            if (user == null)
            {
                ModelState.AddModelError("Email", "ایمیل یا رمز عبور خود را اشتباه وارد کردید");
                return View(login);
            }

            if (user.IsDeleted)
            {
                ModelState.AddModelError("Email", "حساب کاربری شما غیرفعال شده است. لطفاً با پشتیبانی تماس بگیرید.");
                return View(login);
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("IsAdmin",user.IsAdmin.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(claimsIdentity);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                IsPersistent = login.RememberME
            };
            HttpContext.SignInAsync(principal, properties);

                return Redirect(ReturnUrl ?? "/");
        }

        #endregion


        #region Logout

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/login");
        }

        #endregion


    }
}
