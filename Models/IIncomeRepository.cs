using System.Collections.Generic;

namespace Expense_Manager.Models
{
    public interface IIncomeRepository
    {
        IncomeSource GetIncomeSource(int Id);
        IEnumerable<IncomeSource> GetAllIncomeSources(string userId);
        IncomeSource Add(IncomeSource incomeSource);
        IncomeSource Update(IncomeSource incomeSource);
        IncomeSource Delete(int Id);
    }
}
