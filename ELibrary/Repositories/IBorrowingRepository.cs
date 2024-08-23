using ELibrary.Models;
using X.PagedList;

namespace ELibrary.Repositories
{
    public interface IBorrowingRepository : IGenericRepository<Borrowing>
    {
        Task<IPagedList<Borrowing>> GetBorrowingDetails(string? search, int pageNumber, int pageSize = 15);

        Task<Borrowing?> GetBorrowingDetailById(Guid? id);
    }
}