using Expense_Manager.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Expense_Manager.Models
{
    public class SQLMethodRepository : IMethodRepository
    {
        private readonly ApplicationDbContext _context;
        public SQLMethodRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        Method IMethodRepository.Add(Method method)
        {
            _context.Methods.Add(method);
            _context.SaveChanges();
            return method;
        }

        Method IMethodRepository.Delete(int Id)
        {
            Method method = _context.Methods.Find(Id);
            if (method != null)
            {
                _context.Methods.Remove(method);
                _context.SaveChanges();
            }
            return method;
        }

        IEnumerable<Method> IMethodRepository.GetAllMethods(string userId)
        {
            return _context.Methods.Where(c => c.UserId == userId).Include(c => c.User).ToList();
        }
        Method IMethodRepository.GetMethod(int Id)
        {
            return _context.Methods.Find(Id);
        }
        Method IMethodRepository.Update(Method methodChanges)
        {
            var method = _context.Methods.Attach(methodChanges);
            method.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return methodChanges;
        }
    }
}
