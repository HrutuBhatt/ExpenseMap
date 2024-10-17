using Expense_Manager.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Expense_Manager.Models
{
    public class SQLCategoryRepository: ICategoryRepository
    {

        private readonly ApplicationDbContext _context;
        public SQLCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Category ICategoryRepository.Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }

        Category ICategoryRepository.Delete(int Id)
        {
            Category source = _context.Categories.Find(Id);
            if (source != null)
            {
                _context.Categories.Remove(source);
                _context.SaveChanges();
            }
            return source;
        }

        IEnumerable<Category> ICategoryRepository.GetAllCategories(string userId)
        {
            return _context.Categories.Where(c => c.UserId == userId).Include(c => c.User).ToList();
        }
        Category ICategoryRepository.GetCategory(int Id)
        {
            return _context.Categories.Find(Id);
        }
        Category ICategoryRepository.Update(Category categoryChanges)
        {
            var category = _context.Categories.Attach(categoryChanges);
            category.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return categoryChanges;
        }
    }
}
