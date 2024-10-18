using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Novitas_Blog.Models.Domain_Models;
using Novitas_Blog.Models.View_Models;
using Novitas_Blog.Repositories;

namespace Novitas_Blog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IBlogArticleRepository _blogArticleRepository;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ICommentRepository _commentRepository;
        public ArticleController(IBlogArticleRepository blogArticleRepository,
            SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            ICommentRepository commentRepository)
        {
            this._blogArticleRepository = blogArticleRepository;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._commentRepository = commentRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string urlHandle)
        {
            var mainArticle = await _blogArticleRepository.GetBlogByHandleAsync(urlHandle);
            if (mainArticle != null)
            {
                var allArticles = await _blogArticleRepository.GetAllAsync(null);
                var relatedArticle = allArticles.Where(x => x.Category == mainArticle.Category && x.Id != mainArticle.Id).Take(4);
                GetTimeArticle(mainArticle);

                string shorternedDescription = "";
                var lastSpaceInDescription = 0;

                foreach (var article in relatedArticle)
                {
                    if (article.Description.Length >= 150)
                    {
                        shorternedDescription = article.Description.Substring(0, 150);
                        lastSpaceInDescription = shorternedDescription.LastIndexOf(" ");
                        shorternedDescription = article.Description.Substring(0, lastSpaceInDescription);
                        shorternedDescription = shorternedDescription.Insert(lastSpaceInDescription, " . . .");
                        article.Description = shorternedDescription;
                    }


                };
                var comments = await _commentRepository.GetCommentsAsync(mainArticle.Id);
                var updatedCommentsViews = new List<CommentView>();
                foreach (var comment in comments)
                {
                    var commentView = new CommentView
                    {
                        Descripton = comment.Descripton,
                        DateAdded = comment.DateAdded,
                        UserName = (await _userManager.FindByIdAsync(comment.UserId.ToString())).UserName
                    };
                    updatedCommentsViews.Add(commentView);
                }
                if (allArticles != null)
                {
                    var model = new ArticleDetailsViewModel
                    {
                        MainArticle = mainArticle,
                        RelatedArticles = relatedArticle,
                        Comments = updatedCommentsViews
                    };
                    return View(model);
                }
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ArticleDetailsViewModel articleDetailsViewModel)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var comment = new Comment
                {
                    PostId = articleDetailsViewModel.MainArticle.Id,
                    Descripton = articleDetailsViewModel.Comment,
                    DateAdded = DateTime.Now,
                    UserId = Guid.Parse(_userManager.GetUserId(User)),
                };

                var commentUpload = await _commentRepository.AddAsync(comment);
                if (commentUpload != null)
                {
                    return RedirectToAction("Index", "Article", new { urlHandle = articleDetailsViewModel.UrlHandle });
                }
            }

            return View();
        }
        public async Task<IActionResult> Search(string? searchQuery)
        {
            ViewBag.searchQuery = searchQuery;
            var articles = await _blogArticleRepository.GetAllByCategoryAsync(searchQuery);
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    DateTime Now = DateTime.Now;
                    DateTime publishedDate = article.Published_Date;
                    TimeSpan timeElapsed = Now - publishedDate;
                    int totalMinutes = (int)timeElapsed.TotalMinutes;
                    string messageTime;

                    switch (totalMinutes)
                    {
                        case <= 15:
                            messageTime = "Just Now";
                            break;
                        case <= 59:
                            messageTime = "Updated " + totalMinutes + "m ago";
                            break;
                        case <= 1440:
                            messageTime = "Updated " + (totalMinutes / 60) + "h ago";
                            break;
                        case <= 10080:
                            messageTime = "Updated " + (totalMinutes / 1440) + "d ago";
                            break;
                        default:
                            messageTime = publishedDate.ToShortDateString();
                            break;
                    }
                    article.TimeString = messageTime;
                };
                return View(articles);
            }
            return View();
        }
        public string GetTimeArticle(BlogArticle article)
        {
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
                    messageTime = "Updated " + totalMinutes + "m ago";
                    break;
                case <= 1440:
                    messageTime = "Updated " + (totalMinutes / 60) + "h ago";
                    break;
                case <= 10080:
                    messageTime = "Updated " + (totalMinutes / 1440) + "d ago";
                    break;
                default:
                    messageTime = publishedDate.ToShortDateString();
                    break;
            }
            return article.TimeString = messageTime;
        }


    }
}
