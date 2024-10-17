using System.Collections.Generic;

namespace Expense_Manager.Models
{
    public interface IMethodRepository
    {
        Method GetMethod(int Id);
        IEnumerable<Method> GetAllMethods(string userId);
        Method Add(Method method);
        Method Update(Method method);
        Method Delete(int Id);
    }
}
