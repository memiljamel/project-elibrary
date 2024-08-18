using ELibrary.Models;
using X.PagedList;

namespace ELibrary.Repositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IPagedList<Employee>> GetPagedEmployees(string search, int pageNumber, int pageSize = 15);

        Task<Employee?> GetEmployeeByUsername(string? username);

        bool IsEmployeeNumberUnique(string employeeNumber, Guid? id);

        bool IsUsernameUnique(string username, Guid? id);
    }
}