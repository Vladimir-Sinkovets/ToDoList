using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Models.ViewModels;

namespace ToDoList.Controllers
{
    public class AccountController : Controller
    {
        private IRepository db; // переименовать

        public AccountController(IRepository db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var allUsers = db.GetAllUsers();
            if (ModelState.IsValid)
            {
                User user = allUsers.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(model.Email); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var allUsers = db.GetAllUsers();
            if (ModelState.IsValid)
            {
                User user = allUsers.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    db.AddUser(new User() { Email = model.Email, Password = model.Password });
                    await Authenticate(model.Email);
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View();
        }
        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
