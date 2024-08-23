using ELibrary.Data;
using ELibrary.Models;

namespace ELibrary.Repositories
{
    public class BookAuthorRepository : GenericRepository<BookAuthor>, IBookAuthorRepository
    {
        public BookAuthorRepository(ELibraryContext context) : base(context)
        {
        }
    }
}