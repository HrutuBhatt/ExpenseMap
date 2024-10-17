using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Expense_Manager.Controllers
{
    public class IncomeSourceController : Controller
    {
        private readonly IIncomeRepository _incomerepo;
        public IncomeSourceController(IIncomeRepository categoryrepo)
        {
            _incomerepo = categoryrepo;
        }
        public ViewResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _incomerepo.GetAllIncomeSources(userId);
            return View(model);
        }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(IncomeSource model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                IncomeSource source = new IncomeSource
                {
                    SourceName = model.SourceName,
                    Amount = model.Amount,
                    UserId = userId,
                    IncomeMonth = model.IncomeMonth,
                };
                _incomerepo.Add(source);
                return RedirectToAction("details", new { id = source.IncomeSourceId });
            }
            return View();
        }

        public ViewResult Details(int id)
        {
            IncomeSource source = _incomerepo.GetIncomeSource(id);
            if (source == null)
            {
                Response.StatusCode = 404;
                return View("CategoryNotFound", id);
            }
            return View(source);
        }
        [HttpGet]
        public ViewResult Edit(int id)
        {
            IncomeSource source = _incomerepo.GetIncomeSource(id);
            IncomeSource newSource = new IncomeSource
            {
                IncomeSourceId = source.IncomeSourceId,
                SourceName = source.SourceName,
                Amount = source.Amount,
                UserId = source.UserId,
                IncomeMonth = source.IncomeMonth,
            };
            return View(newSource);
        }
        [HttpPost]
        public IActionResult Edit(IncomeSource model)
        {
            if (ModelState.IsValid)
            {
                IncomeSource source = _incomerepo.GetIncomeSource(model.IncomeSourceId);
                source.SourceName = model.SourceName;
                source.Amount = model.Amount;
                //category.UserId = model.UserId;
                source.IncomeMonth = model.IncomeMonth;
                IncomeSource updatedsource = _incomerepo.Update(source);
                return RedirectToAction("index");

            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            IncomeSource source = _incomerepo.GetIncomeSource(id);
            if (source == null)
            {
                return NotFound();
            }
            return View(source);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var source = _incomerepo.GetIncomeSource(id);
            _incomerepo.Delete(source.IncomeSourceId);
            return RedirectToAction("index");
        }
    }
}
