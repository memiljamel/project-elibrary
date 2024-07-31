using Bogus;
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
        public DbSet<Member> Members { get; set; }
        public DbSet<Phone> Phones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var staffs = new Faker<Staff>()
                .RuleFor(s => s.ID, f => Guid.NewGuid())
                .RuleFor(s => s.Username, f => f.Internet.UserName())
                .RuleFor(s => s.Password, f => f.Internet.Password())
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.EmployeeNumber, f => f.Random.Replace("EMP-#####"))
                .RuleFor(s => s.AccessLevel, f => f.PickRandom<AccessLevel>())
                .RuleFor(s => s.CreatedAt, f => f.Date.Past(1))
                .RuleFor(s => s.UpdatedAt, f => f.Date.Recent(1))
                .Generate(50);
            
            var members = new Faker<Member>()
                .RuleFor(m => m.ID, f => Guid.NewGuid())
                .RuleFor(m => m.MemberNumber, f => f.Random.Replace("MEM-#####"))
                .RuleFor(m => m.Name, f => f.Person.FullName)
                .RuleFor(m => m.Address, f => f.Address.FullAddress())
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.CreatedAt, f => f.Date.Past(1))
                .RuleFor(m => m.UpdatedAt, f => f.Date.Recent(1))
                .Generate(50);
            
            var phones = new List<Phone>();

            foreach (var member in members)
            {
                var memberPhones = new Faker<Phone>()
                    .RuleFor(p => p.ID, f => Guid.NewGuid())
                    .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(p => p.MemberID, f => member.ID)
                    .RuleFor(p => p.CreatedAt, f => f.Date.Past(1))
                    .RuleFor(p => p.UpdatedAt, f => f.Date.Recent(1))
                    .Generate(2);

                phones.AddRange(memberPhones);
            }
            
            modelBuilder.Entity<Staff>().HasData(staffs);
            modelBuilder.Entity<Member>().HasData(members);
            modelBuilder.Entity<Phone>().HasData(phones);
        }
    }
}