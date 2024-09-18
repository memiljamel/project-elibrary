using ELibrary.Data;
using ELibrary.Models;

namespace ELibrary.Repositories
{
    public class PhoneRepository : GenericRepository<Phone>, IPhoneRepository
    {
        public PhoneRepository(ELibraryContext context)
            : base(context) { }
    }
}
