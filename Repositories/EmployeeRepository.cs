using ELibrary.Data;
using ELibrary.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace ELibrary.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(ELibraryContext context) : base(context)
        {
        }

        public async Task<IPagedList<Employee>> GetPagedEmployees(string search, int pageNumber, int pageSize = 15)
        {
            var query = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.EmployeeNumber.ToLower().Contains(search.ToLower()) ||
                                         e.Name.ToLower().Contains(search.ToLower()) ||
                                         e.Username.ToLower().Contains(search.ToLower()));
            }

            return await query.OrderByDescending(e => e.CreatedAt)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Employee?> GetEmployeeByUsername(string username)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.Username == username);
        }
    }
}