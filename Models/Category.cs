using Microsoft.AspNetCore.Identity;

namespace Expense_Manager.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
