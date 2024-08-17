using ELibrary.Models;
using X.PagedList;

namespace ELibrary.Repositories
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<IPagedList<Member>> GetPagedMembersWithPhones(string search, int pageNumber, int pageSize = 15);

        Task<Member?> GetMemberWithPhonesById(Guid? id);
    }
}