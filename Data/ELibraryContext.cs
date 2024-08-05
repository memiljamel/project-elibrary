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

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BooksAuthors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var employees = new Faker<Employee>()
                .RuleFor(s => s.ID, f => Guid.NewGuid())
                .RuleFor(s => s.Username, f => f.Internet.UserName())
                .RuleFor(s => s.Password, f => f.Internet.Password())
                .RuleFor(s => s.Name, f => f.Name.FullName())
                .RuleFor(s => s.EmployeeNumber, f => f.Random.Replace("EMP-#####"))
                .RuleFor(s => s.AccessLevel, f => f.PickRandom<AccessLevel>())
                .RuleFor(s => s.CreatedAt, f => f.Date.Past(1))
                .RuleFor(s => s.UpdatedAt, f => f.Date.Recent(1))
                .Generate(25);
            
            var members = new Faker<Member>()
                .RuleFor(m => m.ID, f => Guid.NewGuid())
                .RuleFor(m => m.MemberNumber, f => f.Random.Replace("MEM-#####"))
                .RuleFor(m => m.Name, f => f.Person.FullName)
                .RuleFor(m => m.Address, f => f.Address.FullAddress())
                .RuleFor(m => m.Email, f => f.Internet.Email())
                .RuleFor(m => m.CreatedAt, f => f.Date.Past(1))
                .RuleFor(m => m.UpdatedAt, f => f.Date.Recent(1))
                .Generate(25);
            
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
            
            var books = new Faker<Book>()
                .RuleFor(b => b.ID, f => Guid.NewGuid())
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(3))
                .RuleFor(b => b.Category, f => f.PickRandom<Category>())
                .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
                .RuleFor(b => b.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(b => b.CreatedAt, f => f.Date.Past(1))
                .RuleFor(b => b.UpdatedAt, f => f.Date.Recent(1))
                .Generate(25);
            
            var authors = new Faker<Author>()
                .RuleFor(a => a.ID, f => Guid.NewGuid())
                .RuleFor(a => a.Name, f => f.Name.FullName())
                .RuleFor(a => a.Email, f => f.Internet.Email())
                .RuleFor(a => a.CreatedAt, f => f.Date.Past(1))
                .RuleFor(a => a.UpdatedAt, f => f.Date.Recent(1))
                .Generate(50);
            
            var booksAuthors = new List<BookAuthor>();

            foreach (var book in books)
            {
                var bookAuthors = new Faker<BookAuthor>()
                    .RuleFor(ba => ba.BookID, f => book.ID)
                    .RuleFor(ba => ba.AuthorID, f => f.PickRandom(authors).ID)
                    .Generate(2);

                booksAuthors.AddRange(bookAuthors);
            }
            
            modelBuilder.Entity<Employee>().HasData(employees);
            modelBuilder.Entity<Member>().HasData(members);
            modelBuilder.Entity<Phone>().HasData(phones);
            modelBuilder.Entity<Book>().HasData(books);
            modelBuilder.Entity<Author>().HasData(authors);
            modelBuilder.Entity<BookAuthor>().HasData(booksAuthors);
        }
    }
}