using Microsoft.AspNetCore.Identity;

namespace Expense_Manager.Models
{
    public enum Month
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
    public class IncomeSource
    {
        public int IncomeSourceId { get; set; }
        public string SourceName { get; set; }
        public int Amount { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }
        public Month IncomeMonth { get; set; }
    }
}
