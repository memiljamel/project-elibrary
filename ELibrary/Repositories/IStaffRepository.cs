using ELibrary.Models;
using X.PagedList;

namespace ELibrary.Repositories
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task<IPagedList<Staff>> GetPagedStaffs(string? search, int pageNumber, int pageSize = 15);

        Task<Staff?> GetStaffByUsername(string? username);

        bool IsStaffNumberUnique(string staffNumber, Guid? id);

        bool IsUsernameUnique(string username, Guid? id);
    }
}
