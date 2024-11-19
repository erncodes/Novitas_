using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Novitas_Blog.Models.Domain_Models;
using Novitas_Blog.Models.View_Models;
using Novitas_Blog.Repositories;

namespace Novitas_Blog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminArticleController : Controller
    {
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBlogArticleRepository _blogArticleRepository;
        public AdminArticleController(IBlogArticleRepository blogArticleRepository, IBlogTagRepository blogTagRepository,
                                        ICategoryRepository categoryRepository)
        {
            this._blogArticleRepository = blogArticleRepository;
            this._categoryRepository = categoryRepository;
            this._blogTagRepository = blogTagRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            //Get all blogs from storage.
            var tags = await _blogTagRepository.GetAllAsync();
            //Get all categories, pass null arguement for none sorted categories.
            var categories = await _categoryRepository.GetAllAsync(null);
            //Create model to pass existing tags and categories into the view.
            var model = new CreateArticleRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Category = categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.Id.ToString() })
            };
            //Check if the model contains data then pass it to view, or else pass return an empty view.
            return model != null ? View(model) : (IActionResult)View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateArticleRequest createArticleRequest)
        {
            //Check to verify if all required properties are filled before handling any logic.
            if (ModelState.IsValid)
            {
                //Create a new article from the view model passed to post method of the add method.
                var article = new BlogArticle
                {
                    Title = createArticleRequest.Title,
                    Content = createArticleRequest.Content,
                    Blog_Url = createArticleRequest.Blog_Url,
                    Description = createArticleRequest.Description,
                    Featured_Image_Url = createArticleRequest.Featured_Image_Url,
                    Is_Featured = createArticleRequest.Is_Featured,
                    Is_Visible = createArticleRequest.Is_Visible,
                    Author = createArticleRequest.Author,
                };
                //Create a clone category for the 'selected category' from existing categories.
                var category = new Category();
                //Check for the category using category id passed from view.
                var categoryId = await _categoryRepository.GetCategoryByIdAsync(Guid.Parse(createArticleRequest.Selected_Category));
                //If category is found assign it to the newly created clone.
                if (categoryId != null)
                {
                    category = categoryId;
                }
                //Assign the category to the article.
                article.Category = category;
                //Create a list to store tags incase multiple tags are selected.
                var selectedTags = new List<BlogTag?>();
                //Add each selected tag to the newly created list.
                foreach (var selected_tag in createArticleRequest.Selected_Tags)
                {
                    //Check for existing tag with same the id(s) selected in view.
                    var selectedTagId = Guid.Parse(selected_tag);
                    var existingTag = await _blogTagRepository.GetTagByIdAsync(selectedTagId);
                    //If found add to list.
                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }
                //Assign tags to article.
                article.Blog_Tags = selectedTags;
                //Add tags to database using BlogarticleRepository.
                var addResult = await _blogArticleRepository.AddAsync(article);
                //Check to see if the add operation succeeded
                //-If succeeded a newly created blog will be returned otherwise null,indicating failure.
                if (addResult != null)
                {
                    //Navigate to list view on success.
                    return RedirectToAction("List");
                }
                //Return current view if an error occured.
                return View();
            }
            //return view if model is not valid.
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            //Get all categories, pass null arguement for none sorted categories.
            var categories = await _categoryRepository.GetAllAsync(null);
            //Get all blogs from storage.
            var tags = await _blogTagRepository.GetAllAsync();
            //Check for existing article using the passed Id Guid.
            var articleToUpdate = await _blogArticleRepository.GetBlogByIdAsync(Id);
            //If the article to update is found, create a view model to capture the article data.
            if (articleToUpdate != null)
            {
                var model = new UpdateArticleRequest
                {
                    Id = articleToUpdate.Id,
                    Title = articleToUpdate.Title,
                    Content = articleToUpdate.Content,
                    Blog_Url = articleToUpdate.Blog_Url,
                    Description = articleToUpdate.Description,
                    Featured_Image_Url = articleToUpdate.Featured_Image_Url,
                    Published_Date = articleToUpdate.Published_Date,
                    Is_Featured = articleToUpdate.Is_Featured,
                    Is_Visible = articleToUpdate.Is_Visible,
                    Author = articleToUpdate.Author,
                    Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                    Selected_Tags = articleToUpdate.Blog_Tags.Select(x => x.Id.ToString()).ToArray(),
                    Category = categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.Id.ToString() }),
                };
                //Return the view with the created model.
                return View(model);
            }
            //Return an empty view if no article found with matching Id.
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateArticleRequest updateArticleRequest)
        {
            //Check to see if the model passed is valid.
            if (ModelState.IsValid)
            {
                //Create a clone to replace the existing article in storage.
                var article = new BlogArticle
                {
                    Id = updateArticleRequest.Id,
                    Title = updateArticleRequest.Title,
                    Description = updateArticleRequest.Description,
                    Content = updateArticleRequest.Content,
                    Blog_Url = updateArticleRequest.Blog_Url,
                    Featured_Image_Url = updateArticleRequest.Featured_Image_Url,
                    Author = updateArticleRequest.Author,
                    Published_Date = updateArticleRequest.Published_Date,
                    Is_Featured = updateArticleRequest.Is_Featured,
                    Is_Visible = updateArticleRequest.Is_Visible,
                };
                //Check for existing category using the passed id of selected category in view.
                var categoryAsId = await _categoryRepository.GetCategoryByIdAsync(Guid.Parse(updateArticleRequest.Selected_Category));
                //Assign the category to the updated article.
                article.Category = categoryAsId;
                //Create a list to store tags incase multiple tags are selected.
                var selectedTags = new List<BlogTag?>();
                //Add each selected tag to the newly created list.
                foreach (var selected_tag in updateArticleRequest.Selected_Tags)
                {
                    //Check for existing tag with same the id(s) selected in view.
                    if (Guid.TryParse(selected_tag, out var tag))
                    {
                        var selectedTagId = Guid.Parse(selected_tag);
                        var existingTag = await _blogTagRepository.GetTagByIdAsync(selectedTagId);
                        //If tag found add to list.
                        if (existingTag != null)
                        {
                            selectedTags.Add(existingTag);
                        }
                    }
                }
                article.Blog_Tags = selectedTags;
                var updatedArticle = await _blogArticleRepository.UpdateAsync(article);
                //On success return to list view, otherwise edit view.
                return updatedArticle != null ? RedirectToAction("List") : (IActionResult)RedirectToAction("Edit");
            }
            //Return view with the passed model if the passed model is not valid.
            return View(updateArticleRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var article = await _blogArticleRepository.DeleteAsync(Id);
            return article != null ? RedirectToAction("List") : View();
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery)
        {
            //Get all articles from storage
            var allArticles = await _blogArticleRepository.GetAllExistingBlogs();
            //If any article(s) found retrieve articles, else return empty view.
            if (allArticles != null)
            {
                //If articles satisfying search query condition are found, pass the to view.
                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    var articles = await _blogArticleRepository.GetAllAsync(searchQuery);
                    return articles != null ? View(articles) : (IActionResult)View();
                }
                //In the case no search query is passed return all existing articles
                return View(allArticles);
            }
            //Capture the search query and store it in ViewBag for use in the view.
            ViewBag.searchQuery = searchQuery;
            return View();
        }
    }
}
