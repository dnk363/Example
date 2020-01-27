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

namespace Example.Controllers
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
            string userId = "";

            if (id == null)
            {
                return new JsonResult("Enter the URL in field");
            }

            id.ToLower();
            Regex regex = new Regex(@"^http.*");
            MatchCollection matches = regex.Matches(id);
            if (matches.Count == 0)
            {
                id = "http://" + id; //Add http(s) to long URL if it is not
            }

            ClaimsPrincipal currentUser = User;
            if (currentUser.Identity.Name != null)
            {
                userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            }

            url.ShortURL = GetShortUrl(id); //Getting a short code for a link from a passed long URL

            var b = db.SUrl.FirstOrDefault(p => p.ShortURL == url.ShortURL && (p.UserId == userId || currentUser.Identity.Name == null)); // Checking for a short URL in the database

            // If there is no short URL in the database - add
            if (    b == null
                || (b.UserId == null && currentUser.Identity.Name != null)
                || (b.UserId != null && (currentUser.Identity.Name == null || userId != b.UserId))
                )
            {
                url.LongURL = id;

                if (currentUser.Identity.Name != null)
                {
                    url.UserId = userId;
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

        public ActionResult ShortUrl(URLShort uRLShort)
        {
            throw new NotImplementedException();
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
                    Id = c.Id,
                    ShortURL = Request.Scheme.ToString() + "://" + Request.Host.ToString() + "/r/l/" + c.ShortURL,
                    LongURL = c.LongURL,
                    UserId = c.UserId
                })
                .Where(p => p.UserId == userId)
                .ToList();

            DataViewModel ivm = new DataViewModel { UrlData = urlData };

            return PartialView(ivm);
        }

        public IActionResult Delete(int id)
        {
            ClaimsPrincipal currentUser = User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            string resultMessage = "";
            var b = db.SUrl.FirstOrDefault(p => p.Id == id && p.UserId == userId); 

            if (b == null)
            {
                resultMessage = "Not found in database";
            }
            else
            {
                db.SUrl.Remove(b);
                db.SaveChangesAsync();
                resultMessage = "Delete successfully";
            }

            return new JsonResult(resultMessage);
        }

        public string GetShortUrl(string lUrl)
        {
            var md5 = MD5.Create();
            var hash = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(lUrl))).Trim('=');

            return hash;
        }
    }
}