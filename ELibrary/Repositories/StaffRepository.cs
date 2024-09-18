using ELibrary.Data;
using ELibrary.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace ELibrary.Repositories
{
    public class StaffRepository : GenericRepository<Staff>, IStaffRepository
    {
        public StaffRepository(ELibraryContext context)
            : base(context) { }

        public async Task<IPagedList<Staff>> GetPagedStaffs(
            string? search,
            int pageNumber,
            int pageSize = 15
        )
        {
            var query = _context.Staffs.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s =>
                    s.StaffNumber.ToLower().Contains(search.ToLower())
                    || s.Name.ToLower().Contains(search.ToLower())
                    || s.Username.ToLower().Contains(search.ToLower())
                );
            }

            return await query
                .OrderByDescending(s => s.CreatedAt)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Staff?> GetStaffByUsername(string? username)
        {
            return await _context.Staffs.FirstOrDefaultAsync(s => s.Username == username);
        }

        public bool IsStaffNumberUnique(string staffNumber, Guid? id)
        {
            return !_context.Staffs.Any(s => s.StaffNumber == staffNumber && s.ID != id);
        }

        public bool IsUsernameUnique(string username, Guid? id)
        {
            return !_context.Staffs.Any(s => s.Username == username && s.ID != id);
        }
    }
}
