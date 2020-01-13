using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Example.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Example.ViewModel;
using System.Text;

namespace URLShortener.Controllers
{
    public class HomeController : Controller
    {
        URLShort url = new URLShort();

        private ShortContext db;

        public HomeController(ShortContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShortUrl(string id)
        {
            if (id == null)
            {
                return new JsonResult("Enter the URL in field");
            }

            id.ToLower();
            Regex regex = new Regex(@"^http.*");
            MatchCollection matches = regex.Matches(id);
            ClaimsPrincipal currentUser = User;

            if (matches.Count == 0)
            {
                id = "http://" + id;//Добавление http(s) к длинной ссылке, если его нет
            }

            url.ShortURL = GetShortUrl(id); // Получение короткого кода для ссылки из переданной длинной ссылки

            var b = db.SUrl.FirstOrDefault(p => p.ShortURL == url.ShortURL); // Проверка наличия короткой ссылки в базе

            // Если в базе нет короткой ссылки - добавляем
            if (b == null)
            {
                url.LongURL = id;

                if (currentUser.Identity.Name != null)
                {
                    url.UserId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                }

                db.SUrl.Add(url);
                db.SaveChangesAsync();
            }
            else
            {
                url.LongURL = b.LongURL;
            }

            string resulturl = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/r/l/" + GetShortUrl(id);

            return new JsonResult(resulturl);
        }

        public IActionResult R()
        {
            return View();
        }

        public IActionResult FoundError()
        {
            return View();
        }

        public IActionResult Chat()
        {
            return View();
        }

        [Authorize]
        public IActionResult Data()
        {
            ClaimsPrincipal currentUser = User;

            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<URLShort> urlData = db.SUrl
                .Select(c => new URLShort
                {
                    ShortURL = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/r/l/" + c.ShortURL,
                    LongURL = c.LongURL,
                    UserId = c.UserId
                })
                .Where(p => p.UserId == userId)
                .ToList();

            DataViewModel ivm = new DataViewModel { UrlData = urlData };


            return PartialView(ivm);
        }

        private string GetShortUrl(string lUrl)
        {
            var md5 = MD5.Create();
            var hash = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(lUrl))).Trim('=');

            return hash;
        }
    }
}