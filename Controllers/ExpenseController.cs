using Expense_Manager.Data;
using Expense_Manager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;

namespace Expense_Manager.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseRepository _expenserepo;
        private readonly IIncomeRepository _incomerepo;
        private readonly ICategoryRepository _categoryrepo;

        private readonly ApplicationDbContext _context;
        public ExpenseController(IExpenseRepository expenserepo, ApplicationDbContext context, IIncomeRepository incomerepo, ICategoryRepository categoryrepo)
        {
            _context = context;
            _expenserepo = expenserepo;
            _incomerepo = incomerepo;
            _categoryrepo = categoryrepo;
        }
        public ViewResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _expenserepo.GetAllExpenses(userId);
            return View(model);
        }
     
        public IActionResult IndexFilter(int? selectedCategoryId, int? selectedMonth)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fetch all categories for the filter
            var categories = _categoryrepo.GetAllCategories(userId);

            // Fetch all expenses based on selected filters
            var expenses = _expenserepo.GetAllExpenses(userId); // Fetch all expenses first

            if (selectedCategoryId.HasValue && selectedCategoryId.Value > 0)
            {
                expenses = expenses.Where(e => e.CategoryId == selectedCategoryId.Value);
            }

            if (selectedMonth.HasValue)
            {
                expenses = expenses.Where(e => e.Date.Month == selectedMonth.Value);
            }

            var viewModel = new ExpenseFilterViewModel
            {
                Expenses = expenses.ToList(),
                Categories = categories.ToList(),
                Months = Enum.GetValues(typeof(Month)).Cast<Month>().ToList(), // Assuming Month is an enum
                SelectedCategoryId = selectedCategoryId ?? 0,
                SelectedMonth = selectedMonth ?? 0
            };

            return View(viewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //var userCategories = _context.Categories.Where(c => c.UserId == userId).ToList();
            //var userMethods = _context.Methods.Where(m => m.UserId == userId).ToList();
            //ViewBag.Categories = new SelectList(userCategories, "CategoryId", "CategoryName");
            //ViewBag.Methods = new SelectList(userMethods, "MethodId", "MethodName");

            var categories = _expenserepo.GetCategories(userId);
            ViewBag.Categories = new SelectList(categories, "CategoryId", "CategoryName");

            var methods = _expenserepo.GetMethods(userId);
            ViewBag.Methods = new SelectList(methods, "MethodId", "MethodName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Expense model)
        {
            if (ModelState.IsValid)
            {

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Expense expense = new Expense
                {
                    Amount = model.Amount,
                    Date = model.Date,
                    Description = model.Description,
                    CategoryId = model.CategoryId,  
                    MethodId = model.MethodId,     
                    UserId = userId              
                };

                _expenserepo.Add(expense);

                return RedirectToAction("Details", new { id = expense.ExpenseId });
            }

            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Categories = new SelectList(_expenserepo.GetCategories(loggedInUserId), "CategoryId", "CategoryName");
            ViewBag.Methods = new SelectList(_expenserepo.GetMethods(loggedInUserId), "MethodId", "MethodName");

            return View(model);
        }


        public ViewResult Details(int id)
        {
            Expense expense = _expenserepo.GetExpense(id);
            if (expense == null)
            {
                Response.StatusCode = 404;
                return View("CategoryNotFound", id);
            }
            return View(expense);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            Expense expense = _expenserepo.GetExpense(id);

            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (expense == null || expense.UserId != loggedInUserId)
            {
                return NotFound(); 
            }

            ViewBag.Categories = new SelectList(_expenserepo.GetCategories(loggedInUserId), "CategoryId", "CategoryName", expense.CategoryId);
            ViewBag.Methods = new SelectList(_expenserepo.GetMethods(loggedInUserId), "MethodId", "MethodName", expense.MethodId);

            return View(expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense model)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Expense expense = _expenserepo.GetExpense(model.ExpenseId);

                if (expense == null || expense.UserId != userId)
                {
                    return NotFound(); 
                }

                expense.Amount = model.Amount;
                expense.Date = model.Date;
                expense.Description = model.Description;
                expense.CategoryId = model.CategoryId; 
                expense.MethodId = model.MethodId; 

                _expenserepo.Update(expense);

                return RedirectToAction("Details", new { id = expense.ExpenseId });
            }

            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.Categories = new SelectList(_expenserepo.GetCategories(loggedInUserId), "CategoryId", "CategoryName", model.CategoryId);
            ViewBag.Methods = new SelectList(_expenserepo.GetMethods(loggedInUserId), "MethodId", "MethodName", model.MethodId);

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Expense expense = _expenserepo.GetExpense(id);
            if (expense == null)
            {
                return NotFound();
            }
            return View(expense);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var expense = _expenserepo.GetExpense(id);
            _expenserepo.Delete(expense.ExpenseId);
            return RedirectToAction("index");
        }

        public IActionResult Analytics()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _expenserepo.GetExpenseSummaryByCategory(userId);
            return View(model);
        }

        public IActionResult MonthlySummary()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var expenses = _expenserepo.GetAllExpenses(userId); // Retrieve all expenses
            var incomes = _incomerepo.GetAllIncomeSources(userId); // Retrieve all incomes

            var summary = from expense in expenses
                          group expense by expense.Date.ToString("MMMM") into expenseGroup
                          select new MonthlySummaryViewModel
                          {
                              Month = expenseGroup.Key,
                              TotalExpense = expenseGroup.Sum(e => e.Amount),
                              TotalIncome = incomes
                                  .Where(i => i.IncomeMonth.ToString() == expenseGroup.Key) // Filter incomes by month
                                  .Sum(i => i.Amount),
                          };

            foreach (var item in summary)
            {
                item.Savings = item.TotalIncome - item.TotalExpense;
                item.SavingsPercentage = item.TotalIncome != 0 ? (item.Savings / item.TotalIncome) * 100 : 0;
            }

            return View(summary.ToList());
        }

        public IActionResult MonthlyAnalytics(string month)
        {
            int monthNumber = DateTime.ParseExact(month, "MMMM", CultureInfo.InvariantCulture).Month;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var categoryWiseExpenses = _expenserepo.GetExpensesByMonth(userId, monthNumber);
                                                  
            ViewBag.Month = (Month)monthNumber;
            Console.WriteLine(month);
            Console.WriteLine(categoryWiseExpenses);
            return View(categoryWiseExpenses);
        }

        public IActionResult YearlyExpenditure()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Group expenses by month and calculate the total expense for each month
            var monthlyExpenses = _expenserepo.GetExpensesForYear(userId, DateTime.Now.Year)
                                              .GroupBy(e => e.Date.Month)
                                              .Select(group => new YearlyExpenseViewModel
                                              {
                                                  Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(group.Key),
                                                  TotalAmount = group.Sum(e => e.Amount)
                                              })
                                              .OrderBy(m => m.Month) // Order by month
                                              .ToList();         

            return View(monthlyExpenses);
        }

    }
}
