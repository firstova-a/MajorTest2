using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.WebPages;
using Castle.Core.Internal;
using Laboratory9.Entities;
using Laboratory9.Models;
using Microsoft.AspNetCore.Http;

namespace Laboratory9.Controllers
{
    public class HomeController : Controller
    {
        public LinkContext context = new LinkContext();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            LoginModel empty = new LoginModel();
            return View(empty);
        }
        [HttpPost]
        public async Task<IActionResult> Register(string login, string pswd)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                LoginModel model = new LoginModel()
                {
                    Password = pswd,
                    ErrorMessage = "Имя пользователя не указано!"
                };
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(pswd))
            {
                LoginModel model = new LoginModel()
                {
                    Login = login,
                    ErrorMessage = "Пароль не корректный!"
                };
                return View(model);
            }
            UserInfo usr = context.Users.FirstOrDefault(u => u.Username == login);
            if (usr != null)
            {
                LoginModel model = new LoginModel()
                {
                    Password = pswd,
                    ErrorMessage = "Данное имя пользователя уже используется"
                };
                return View(model);
            }
            else
            {
                UserInfo user = new UserInfo()
                {
                    Username = login,
                    PasswordHash = Crypto.HashPassword(pswd)
                };
                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                return Redirect("/Home/Index");
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginModel empty = new LoginModel();
            return View(empty);
        }

        private async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
                };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }


        [HttpPost]
        public async Task<IActionResult> Login(string login, string pswd)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                LoginModel model = new LoginModel()
                {
                    Password = pswd,
                    ErrorMessage = "Имя пользователя не указано!"
                };
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(pswd))
            {
                LoginModel model = new LoginModel()
                {
                    Login = login,
                    ErrorMessage = "Пароль не корректный!"
                };
                return View(model);
            }
            UserInfo usr = context.Users.FirstOrDefault(u => u.Username == login);
            if (usr == null)
            {
                LoginModel model = new LoginModel()
                {
                    ErrorMessage = "Пользователя не существует!"
                };
                return View(model);
            }
            else if (!Crypto.VerifyHashedPassword(usr.PasswordHash,pswd))
            {
                LoginModel model = new LoginModel()
                {
                    ErrorMessage = "Неверный пароль!"
                };
                return View(model);
            }
            else
            {
                await Authenticate(login);
                return Redirect("/Home/UserPage");
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Home/Index");

        }

       [Authorize]
        public async Task<IActionResult> DeleteNews(int IdNews, string Sender)
        {
            News NewsDelete = context.News.Find(IdNews);
            if (NewsDelete.Author.Username == Sender)
            {
                context.News.Remove(NewsDelete);
                await context.SaveChangesAsync();
            }
            return Redirect("/Home/UserPage");
        }
     



        [Authorize]
        public async Task<IActionResult> AllLinks()
        {
            return View(context.News.ToList().Reverse<News>());
        }


       [Authorize]
        public async Task<IActionResult> UserPage()
        {
            return View(context.News.ToList().Reverse<News>());
        }



        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserPage(string title, string text, string button)
        {
            UserInfo user = context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if ((button == "Добавить") && (!title.IsNullOrEmpty()) && (!text.IsNullOrEmpty()))
            {
                News news = new News()
                    {
                    Title = title,
                    Text = text,
                    Time = DateTime.Now,
                    Author = user

                };
                    await context.News.AddAsync(news);
                    await context.SaveChangesAsync();
                }
            

            

           return View(context.News.ToList());
        }
    }
}
