using ELibrary.Models;
using X.PagedList;

namespace ELibrary.Repositories
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<IPagedList<Author>> GetPagedAuthors(string search, int pageNumber, int pageSize = 15);
        
        Task<Author?> GetAuthorWithBooksAuthorsById(Guid? id);
    }
}