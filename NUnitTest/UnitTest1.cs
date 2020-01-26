using Example.Controllers;
using Example.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace NUnitTest
{
    public class HomeConytrollerTests
    {
        private ShortContext db;

        string createShortUrl = "https://docs.microsoft.com/ru-ru/visualstudio/test/run-unit-tests-with-test-explorer?view=vs-2019";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateShortUrl()
        {
            HomeController controller = new HomeController(db);

            string result = controller.GetShortUrl(createShortUrl) as string;

            Assert.NotNull(result);
        }
    }
}