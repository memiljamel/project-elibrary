using ELibrary.Data;
using ELibrary.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace ELibrary.Repositories
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        public BookRepository(ELibraryContext context)
            : base(context) { }

        public async Task<IPagedList<Book>> GetPagedBooksWithAuthors(
            string? search,
            int pageNumber,
            int pageSize = 15
        )
        {
            var query = _context
                .Books.Include(b => b.BooksAuthors)
                .ThenInclude(ba => ba.Author)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b =>
                    b.Title.ToLower().Contains(search.ToLower())
                    || b.BooksAuthors.Any(ba => ba.Author.Name.ToLower().Contains(search.ToLower()))
                    || b.Publisher.ToLower().Contains(search.ToLower())
                    || b.Quantity.ToString().ToLower().Contains(search.ToLower())
                );
            }

            return await query
                .OrderByDescending(b => b.CreatedAt)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Book?> GetBookWithAuthorsById(Guid? id)
        {
            return await _context
                .Books.Include(b => b.BooksAuthors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefaultAsync(b => b.ID == id);
        }
    }
}
