using System.Collections.Generic;

namespace Expense_Manager.Models
{
    public class ExpenseFilterViewModel
    {
        public IEnumerable<Expense> Expenses { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Month> Months { get; set; } // Assuming you have an enum for months

        public int SelectedCategoryId { get; set; }
        public int SelectedMonth { get; set; }
    }
}
