using System.Collections.Generic;

namespace Expense_Manager.Models
{
    public interface ICategoryRepository
    {
        Category GetCategory(int Id);
        IEnumerable<Category> GetAllCategories(string userId);
        Category Add(Category category);
        Category Update(Category category);
        Category Delete(int Id);
    }
}
