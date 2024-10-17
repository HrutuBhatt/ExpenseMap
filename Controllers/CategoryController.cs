using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Manager.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryrepo;
        public CategoryController(ICategoryRepository categoryrepo)
        {
            _categoryrepo = categoryrepo;
        }
        public ViewResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _categoryrepo.GetAllCategories(userId);
            return View(model);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Category category = new Category
                {
                    CategoryName = model.CategoryName,
                    UserId = userId
                };
                _categoryrepo.Add(category);
                return RedirectToAction("details", new { id = category.CategoryId });
            }
            return View();
        }

        public ViewResult Details(int id)
        {
            Category category = _categoryrepo.GetCategory(id);
            if(category == null)
            {
                Response.StatusCode = 404;
                return View("CategoryNotFound", id);
            }
            return View(category);
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Category category= _categoryrepo.GetCategory(id);
            Category newCategory = new Category
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                UserId = category.UserId
            };
            return View(newCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category model) 
        {
            if (ModelState.IsValid) 
            {
                Category category = _categoryrepo.GetCategory(model.CategoryId);
                category.CategoryName = model.CategoryName;
                //category.UserId = model.UserId;
                Category updatedCategory = _categoryrepo.Update(category);
                return RedirectToAction("index");
             
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id) 
        {
            Category category = _categoryrepo.GetCategory(id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryrepo.GetCategory(id);
            _categoryrepo.Delete(category.CategoryId);
            return RedirectToAction("index");   
        }
    }
}
