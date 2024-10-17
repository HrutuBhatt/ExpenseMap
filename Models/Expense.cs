using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;

namespace Expense_Manager.Models
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int MethodId { get; set; }
        public int CategoryId { get; set; }

        public IdentityUser User { get; set; }
        public Category Category { get; set; }
        public Method Method { get; set; }
    }
}
