using ELibrary.Models;
using X.PagedList;

namespace ELibrary.Repositories
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IPagedList<Book>> GetPagedBooksWithAuthors(string? search, int pageNumber, int pageSize = 15);
        
        Task<Book?> GetBookWithAuthorsById(Guid? id);
    }
}