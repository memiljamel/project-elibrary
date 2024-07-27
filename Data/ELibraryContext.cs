using ELibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Data
{
    public class ELibraryContext : DbContext
    {
        public ELibraryContext(DbContextOptions<ELibraryContext> options)
            : base(options)
        {
        }

        public DbSet<Staff> Staffs { get; set; }
    }
}