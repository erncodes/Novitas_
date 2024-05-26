using Microsoft.AspNetCore.Mvc;
using Novitas_Blog.Models;
using Novitas_Blog.Models.View_Models;
using Novitas_Blog.Repositories;
using System.Diagnostics;

namespace Novitas_Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogArticleRepository _blogArticleRepository;

        public HomeController(ILogger<HomeController> logger, IBlogArticleRepository blogArticleRepository)
        {
            _logger = logger;
            this._blogArticleRepository = blogArticleRepository;
        }

        public async Task<IActionResult> Index()
        {
            var allArticles = await _blogArticleRepository.GetAllAsync(null);

            //Manipulating Published_Date & Description For Display Purposes
            foreach (var article in allArticles)
            {
                string shorter = "";
                var lastSpaceInDescription = 0;
                if (article.Description.Length >= 150)
                {
                    shorter = article.Description.Substring(0, 150);
                    lastSpaceInDescription = shorter.LastIndexOf(" ");
                    shorter = article.Description.Substring(0, lastSpaceInDescription);
                    shorter = shorter.Insert(lastSpaceInDescription, " . . .");
                    article.Description = shorter;
                }

                DateTime Now = DateTime.Now;
                DateTime publishedDate = article.Published_Date;
                TimeSpan timeElapsed = Now - publishedDate;
                int totalMinutes = (int)timeElapsed.TotalMinutes;
                int totalHours
                    = (int)timeElapsed.TotalMinutes;
                string messageTime = "";

                switch (totalMinutes)
                {
                    case <= 15:
                        messageTime = "Just Now";
                        break;
                    case <= 59:
                        messageTime = "Updated " + totalMinutes +"m ago";
                        break;
                    case <= 1440:
                        messageTime = "Updated " + totalMinutes/60 + "h ago";
                        break;
                    case <= 10080:
                        messageTime = "Updated " + totalMinutes / 1440 + "d ago";
                        break;
                    default:
                        messageTime = publishedDate.ToShortDateString();
                        break;
                }
                article.TimeString = messageTime;

            };
            var generalArticles = (allArticles.Where(x => x.Is_Featured == false)).Take(4);
            var allFeatured = (allArticles.Where(x => x.Is_Featured).OrderByDescending(x => x.Published_Date)).Skip(1).Take(4);
            var latestFeatured = (allArticles.Where(x => x.Is_Featured).OrderByDescending(x => x.Published_Date)).First();

            var model = new HomeViewModel
            {
                General_Articles = generalArticles,
                Featured_Articles = allFeatured,
                Latest_Featured = latestFeatured
            };
            return View(model);
        }
        [HttpGet]

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
