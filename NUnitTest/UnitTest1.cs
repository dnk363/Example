using Example.Controllers;
using Example.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;

namespace NUnitTest
{
    public class HomeControllerTests
    {
        private ShortContext db;

        /*public HomeControllerTests(ShortContext context)
        {
            db = context;
        }*/

        string createShortUrl = "https://docs.microsoft.com/ru-ru/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateShortUrl()
        {
            HomeController controller = new HomeController(db);

            string createShort = controller.GetShortUrl(createShortUrl) as string;

            Regex regex = new Regex(@"\w{22}");
            MatchCollection matches = regex.Matches(createShort);

            Assert.NotNull(createShort);
            Assert.AreEqual(matches.Count, 1);
        }
        
        [Test]
        public void ChatView()
        {
            HomeController controller = new HomeController(db);

            ViewResult result = controller.Chat() as ViewResult;

            Assert.NotNull(result);
        }

        [Test]
        public void IndexView()
        {
            HomeController controller = new HomeController(db);

            ViewResult indexView = controller.Index() as ViewResult;

            Assert.NotNull(indexView);
        }
    }
}