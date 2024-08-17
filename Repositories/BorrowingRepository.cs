using ELibrary.Data;
using ELibrary.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace ELibrary.Repositories
{
    public class BorrowingRepository : GenericRepository<Borrowing>, IBorrowingRepository
    {
        public BorrowingRepository(ELibraryContext context) : base(context)
        {
        }

        public async Task<IPagedList<Borrowing>> GetBorrowingDetails(string search, int pageNumber, int pageSize = 15)
        {
            var query = _context.Borrowings
                .Include(b => b.Member)
                .Include(b => b.Book)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Member.MemberNumber.ToLower().Contains(search.ToLower()) ||
                                         b.Book.Title.ToLower().Contains(search.ToLower()) ||
                                         b.DateBorrow.ToString().ToLower().Contains(search.ToLower()) ||
                                         b.DateReturn.ToString().ToLower().Contains(search.ToLower()));
            }
            
            return await query.OrderByDescending(b => b.CreatedAt)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Borrowing?> GetBorrowingDetailById(Guid? id)
        {
            return await _context.Borrowings
                .Include(b => b.Member)
                .Include(b => b.Book)
                .FirstOrDefaultAsync(b => b.ID == id);
        }
    }
}