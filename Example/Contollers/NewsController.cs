using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Example.Models;
using Example.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Example.Contollers
{
    public class NewsController : Controller
    {
        private NewsContext db;

        public NewsController(NewsContext newsContext)
        {
            db = newsContext;
        }

        public IActionResult Index()
        {
            List<News> newsData = db.NewsData
                .Select(c => new News
                {
                    Id = c.Id,
                    NewsHeading = c.NewsHeading,
                    NewsText = c.NewsText,
                    NewsDate = c.NewsDate
                })
                .ToList();

            NewsViewModel ivm = new NewsViewModel { NewsData = newsData };

            return PartialView(ivm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(News model)
        {
            if (ModelState.IsValid)
            {
                News news = new News {NewsHeading = model.NewsHeading, NewsText = model.NewsText, NewsDate = DateTime.Now };

                db.NewsData.Add(news);
                db.SaveChangesAsync();
                ViewData["Message"] = "News added";
            }

            return View();
        }
    }
}