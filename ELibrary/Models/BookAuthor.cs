using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Models
{
    [Table("books_authors")]
    [PrimaryKey(nameof(BookID), nameof(AuthorID))]
    public class BookAuthor
    {
        [ForeignKey(nameof(Book))]
        [Column("book_id")]
        public Guid BookID { get; set; }

        [ForeignKey(nameof(Author))]
        [Column("author_id")]
        public Guid AuthorID { get; set; }

        public Book Book { get; set; }

        public Author Author { get; set; }
    }
}
