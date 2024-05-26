﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Novitas_Blog.Models.Domain_Models;
using Novitas_Blog.Models.View_Models;
using Novitas_Blog.Repositories;

namespace Novitas_Blog.Controllers
{
    [Authorize]
    public class AdminArticleController : Controller
    {
        private readonly IBlogArticleRepository _blogArticleRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly ICategoryRepository _categoryRepository;

        public AdminArticleController(IBlogArticleRepository blogArticleRepository, IBlogTagRepository blogTagRepository,
                                        ICategoryRepository categoryRepository)
        {
            this._blogArticleRepository = blogArticleRepository;
            this._blogTagRepository = blogTagRepository;
            this._categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var tags = await _blogTagRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync(null);
            var model = new CreateArticleRequest
            {
                Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                Category = categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.Id.ToString() })
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateArticleRequest createArticleRequest)
        {
            if (ModelState.IsValid)
            {
                var article = new BlogArticle
                {
                    Title = createArticleRequest.Title,
                    Description = createArticleRequest.Description,
                    Content = createArticleRequest.Content,
                    Blog_Url = createArticleRequest.Blog_Url,
                    Featured_Image_Url = createArticleRequest.Featured_Image_Url,
                    Author = createArticleRequest.Author,
                    Is_Visible = createArticleRequest.Is_Visible,
                    Is_Featured = createArticleRequest.Is_Featured,
                };

                var category = new Category();
                var categoryId = await _categoryRepository.GetCategoryByIdAsync(Guid.Parse(createArticleRequest.Selected_Category));
                if (categoryId != null)
                {
                    category = categoryId;
                }
                article.Category = category;

                var selectedTags = new List<BlogTag?>();
                foreach (var selected_tag in createArticleRequest.Selected_Tags)
                {
                    var selectedTagId = Guid.Parse(selected_tag);
                    var existingTag = await _blogTagRepository.GetTagByIdAsync(selectedTagId);

                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }
                article.Blog_Tags = selectedTags;

                await _blogArticleRepository.AddAsync(article);
                return RedirectToAction("Add");
            }
            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var articleToUpdate = await _blogArticleRepository.GetBlogByIdAsync(Id);
            var tags = await _blogTagRepository.GetAllAsync();
            var categories = await _categoryRepository.GetAllAsync(null);
            if (articleToUpdate != null)
            {
                var model = new UpdateArticleRequest
                {
                    Id = articleToUpdate.Id,
                    Title = articleToUpdate.Title,
                    Description = articleToUpdate.Description,
                    Content = articleToUpdate.Content,
                    Blog_Url = articleToUpdate.Blog_Url,
                    Featured_Image_Url = articleToUpdate.Featured_Image_Url,
                    Author = articleToUpdate.Author,
                    Published_Date = articleToUpdate.Published_Date,
                    Is_Featured = articleToUpdate.Is_Featured,
                    Is_Visible = articleToUpdate.Is_Visible,
                    Tags = tags.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                    Selected_Tags = articleToUpdate.Blog_Tags.Select(x => x.Id.ToString()).ToArray(),
                    Category = categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.Id.ToString()}),
                };
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateArticleRequest updateArticleRequest)
        {

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
            var categoryAsId = await _categoryRepository.GetCategoryByIdAsync(Guid.Parse(updateArticleRequest.Selected_Category));
         
            article.Category = categoryAsId;
            var selectedTags = new List<BlogTag?>();

            foreach (var selected_tag in updateArticleRequest.Selected_Tags)
            {
                if (Guid.TryParse(selected_tag, out var tag))
                {
                    var selectedTagId = Guid.Parse(selected_tag);
                    var existingTag = await _blogTagRepository.GetTagByIdAsync(selectedTagId);

                    if (existingTag != null)
                    {
                        selectedTags.Add(existingTag);
                    }
                }
            }
            article.Blog_Tags = selectedTags;

            var updatedArticle = await _blogArticleRepository.UpdateAsync(article);

            if (updatedArticle != null)
            {
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit");

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var article = await _blogArticleRepository.DeleteAsync(Id);
            if (article != null)
            {
                return RedirectToAction("List");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery)
        {
            ViewBag.searchQuery = searchQuery;
            var articles = await _blogArticleRepository.GetAllAsync(searchQuery);
            return View(articles);
        }
    }
}