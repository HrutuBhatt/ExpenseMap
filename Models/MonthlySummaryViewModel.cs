namespace Expense_Manager.Models
{
    public class MonthlySummaryViewModel
    {
        public string Month { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal Savings { get; set; }
        public decimal SavingsPercentage { get; set; }
    }
}
