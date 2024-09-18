using ELibrary.Data;
using ELibrary.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using X.PagedList.EF;

namespace ELibrary.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        public AuthorRepository(ELibraryContext context)
            : base(context) { }

        public async Task<IPagedList<Author>> GetPagedAuthors(
            string? search,
            int pageNumber,
            int pageSize = 15
        )
        {
            var query = _context.Authors.Include(a => a.BooksAuthors).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a =>
                    a.Name.ToLower().Contains(search.ToLower())
                    || a.Email.ToLower().Contains(search.ToLower())
                );
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToPagedListAsync(pageNumber, pageSize);
        }

        public async Task<Author?> GetAuthorWithBooksAuthorsById(Guid? id)
        {
            return await _context
                .Authors.Include(a => a.BooksAuthors)
                .FirstOrDefaultAsync(m => m.ID == id);
        }
    }
}
