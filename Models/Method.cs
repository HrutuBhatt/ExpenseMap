using Microsoft.AspNetCore.Identity;

namespace Expense_Manager.Models
{
    public class Method
    {
        public int MethodId { get; set; }
        public string MethodName { get; set; }
        public string UserId { get; set; }
        public IdentityUser User { get; set; }

    }
}
