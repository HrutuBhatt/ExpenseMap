using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Manager.Controllers
{
    public class MethodController : Controller
    {
        private readonly IMethodRepository _methodrepo;
        public MethodController(IMethodRepository methodrepo)
        {
            _methodrepo = methodrepo;
        }
        public ViewResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _methodrepo.GetAllMethods(userId);
            return View(model);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Method model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Method method = new Method
                {
                    MethodName = model.MethodName,
                    UserId = userId
                };
                _methodrepo.Add(method);
                return RedirectToAction("details", new { id = method.MethodId });
            }
            return View();
        }

        public ViewResult Details(int id)
        {
            Method method = _methodrepo.GetMethod(id);
            if (method == null)
            {
                Response.StatusCode = 404;
                return View("CategoryNotFound", id);
            }
            return View(method);
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Method method = _methodrepo.GetMethod(id);
            Method newMethod = new Method
            {
                MethodId = method.MethodId,
                MethodName = method.MethodName,
                UserId = method.UserId
            };
            return View(newMethod);
        }
        [HttpPost]
        public IActionResult Edit(Method model)
        {
            if (ModelState.IsValid)
            {
                Method method = _methodrepo.GetMethod(model.MethodId);
                method.MethodName = model.MethodName;
              
                Method updatedCategory = _methodrepo.Update(method);
                return RedirectToAction("index");

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            Method method = _methodrepo.GetMethod(id);
            if (method == null)
            {
                return NotFound();
            }
            return View(method);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var method = _methodrepo.GetMethod(id);
            _methodrepo.Delete(method.MethodId);
            return RedirectToAction("index");
        }
    }
}
