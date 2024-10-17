using System.Collections.Generic;

namespace Expense_Manager.Models
{
    public interface IExpenseRepository
    {
        Expense GetExpense(int Id);
        IEnumerable<Expense> GetAllExpenses(string userId);
        Expense Add(Expense method);
        Expense Update(Expense method);
        Expense Delete(int Id);
        IEnumerable<Category> GetCategories(string Id);
        IEnumerable<Method> GetMethods(string Id);
        IEnumerable<ExpenseCategorySummary> GetExpenseSummaryByCategory(string userId);
        IEnumerable<ExpenseCategorySummary> GetExpensesByMonth(string userId, int month);
        IEnumerable<Expense> GetExpensesForYear(string userId, int year);

    }
}
