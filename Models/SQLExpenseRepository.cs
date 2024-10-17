using Expense_Manager.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Expense_Manager.Models
{
    public class SQLExpenseRepository : IExpenseRepository
    {
        private readonly ApplicationDbContext _context;
        public SQLExpenseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Expense IExpenseRepository.Add(Expense expense)
        {
            _context.Expenses.Add(expense);
            _context.SaveChanges();
            return expense;
        }

        Expense IExpenseRepository.Delete(int Id)
        {
            Expense expense = _context.Expenses.Find(Id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                _context.SaveChanges();
            }
            return expense;
        }

        IEnumerable<Expense> IExpenseRepository.GetAllExpenses(string userId)
        {
            return _context.Expenses.Where(c => c.UserId == userId) 
                .Include(c => c.User)
                .Include(e => e.Category)   
                .Include(e => e.Method)
                .ToList();
        }
        Expense IExpenseRepository.GetExpense(int Id)
        {
            return _context.Expenses               
                .Include(c => c.User)
                .Include(e => e.Category)
                .Include(e => e.Method)
                .FirstOrDefault(ex => ex.ExpenseId==Id);
        }
        Expense IExpenseRepository.Update(Expense expenseChanges)
        {
            var expense = _context.Expenses.Attach(expenseChanges);
            expense.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return expenseChanges;
        }

        IEnumerable<Category> IExpenseRepository.GetCategories(string Id)
        {
            return _context.Categories.Where(c => c.UserId == Id).ToList();
        }

        IEnumerable<Method> IExpenseRepository.GetMethods(string Id)
        {
            return _context.Methods.Where(c => c.UserId == Id).ToList();
        }

        IEnumerable<ExpenseCategorySummary> IExpenseRepository.GetExpenseSummaryByCategory(string userId)
        {
            return _context.Expenses
                           .Where(e => e.UserId == userId)
                           .GroupBy(e => e.Category.CategoryName)
                           .Select(g => new ExpenseCategorySummary
                           {
                               CategoryName = g.Key,
                               TotalAmount = g.Sum(e => e.Amount)
                           })
                           .ToList();
        }

        IEnumerable<ExpenseCategorySummary> IExpenseRepository.GetExpensesByMonth(string userId, int month)
        {
            return _context.Expenses
                           .Where(e => e.UserId == userId && e.Date.Month == month)
                           .GroupBy(e => e.Category.CategoryName)
                                                   .Select(group => new ExpenseCategorySummary
                                                   {
                                                       CategoryName = group.Key,
                                                       TotalAmount = group.Sum(e => e.Amount)
                                                   }).ToList();
                         
        }

        IEnumerable<Expense> IExpenseRepository.GetExpensesForYear(string userId, int year)
        {
            return _context.Expenses
                           .Where(e => e.UserId == userId && e.Date.Year == year)
                           .ToList();
        }


    }
}
