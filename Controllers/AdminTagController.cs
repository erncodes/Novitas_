﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Novitas_Blog.Models.Domain_Models;
using Novitas_Blog.Models.View_Models;
using Novitas_Blog.Repositories;

namespace Novitas_Blog.Controllers
{
    [Authorize]
    public class AdminTagController : Controller
    {
        private readonly IBlogArticleRepository _blogArticleRepository;
        private readonly IBlogTagRepository _blogTagRepository;

        public AdminTagController(IBlogArticleRepository blogArticleRepository, IBlogTagRepository blogTagRepository)
        {
            this._blogArticleRepository = blogArticleRepository;
            this._blogTagRepository = blogTagRepository;
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateTagRequest createTagRequest)
        {
            var newTag = new BlogTag
            {
                Name = createTagRequest.Name,
            };

            var createtag = await _blogTagRepository.AddAsync(newTag);
            if (createtag != null)
            {
                return RedirectToAction("List");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var model = await _blogTagRepository.GetTagByIdAsync(Id);
            if (model != null)
            {
                var tag = new UpdateTagRequest
                {
                    Id = model.Id,
                    Name = model.Name,
                    Articles = model.Articles
                };
                return View(tag);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateTagRequest updateTagRequest)
        {
            var tag = new BlogTag
            {
                Id = updateTagRequest.Id,
                Name = updateTagRequest.Name,
                Articles = updateTagRequest.Articles
            };
            var updatedTag = await _blogTagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                return RedirectToAction("List");
            }
            return View(null);
        }
        [HttpGet]
        public async Task<IActionResult> List(string? searchQuery)
        {
            ViewBag.searchQuery = searchQuery;
            var tags = await _blogTagRepository.GetAllAsync(searchQuery);
            return View(tags);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var tag = await _blogTagRepository.DeleteAsync(Id);
            if (tag != null)
            {
                return RedirectToAction("List");
            }
            return View();
        }
    }
}