using Expense_Manager.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Expense_Manager.Models
{
    public class SQLIncomeRepository : IIncomeRepository
    {
        private readonly ApplicationDbContext _context;
        public SQLIncomeRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        IncomeSource IIncomeRepository.Add(IncomeSource source)
        {
            _context.IncomeSources.Add(source);
            _context.SaveChanges();
            return source;
        }

        IncomeSource IIncomeRepository.Delete(int Id)
        {
            IncomeSource source = _context.IncomeSources.Find(Id);
            if (source != null)
            {
                _context.IncomeSources.Remove(source);
                _context.SaveChanges();
            }
            return source;
        }

        IEnumerable<IncomeSource> IIncomeRepository.GetAllIncomeSources(string UserId)
        {
            return _context.IncomeSources.Where(c => c.UserId == UserId).Include(c => c.User).ToList();
        }
        IncomeSource IIncomeRepository.GetIncomeSource(int Id)
        {
            return _context.IncomeSources.Find(Id);
        }
        IncomeSource IIncomeRepository.Update(IncomeSource incomeSourceChanges)
        {
            var source = _context.IncomeSources.Attach(incomeSourceChanges);
            source.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return incomeSourceChanges;
        }
    }
}
