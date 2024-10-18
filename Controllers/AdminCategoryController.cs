using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Novitas_Blog.Models.Domain_Models;
using Novitas_Blog.Models.View_Models;
using Novitas_Blog.Repositories;

namespace Novitas_Blog.Controllers
{
    [Authorize]
    public class AdminCategoryController : Controller
    {
        private readonly ICategoryRepository _categoryrepository;
        public AdminCategoryController(ICategoryRepository categoryrepository)
        {
            this._categoryrepository = categoryrepository;
        }
        public async Task<IActionResult> List(string? searchQuery)
        {
            ViewBag.searchQuery = searchQuery;
            var categories = await _categoryrepository.GetAllAsync(searchQuery);
            return View(categories);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateCategoryRequest createCategoryRequest)
        {
            var newCategory = new Category
            {
                CategoryName = createCategoryRequest.CategoryName
            };

            var createCategory = await _categoryrepository.AddAsync(newCategory);
            if (createCategory != null)
            {
                return RedirectToAction("List");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var model = await _categoryrepository.GetCategoryByIdAsync(Id);
            if (model != null)
            {
                var category = new UpdateCategoryRequest
                {
                    Id = model.Id,
                    CategoryName = model.CategoryName,
                };
                return View(category);
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryRequest updateCategoryRequest)
        {
            var category = new Category
            {
                Id = updateCategoryRequest.Id,
                CategoryName = updateCategoryRequest.CategoryName,
            };
            var updatedCategory = await _categoryrepository.UpdateAsync(category);

            if (updatedCategory != null)
            {
                return RedirectToAction("List");
            }
            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var category = await _categoryrepository.DeleteAsync(Id);
            if (category != null)
            {
                return RedirectToAction("List");
            }
            return View();
        }
    }
}
