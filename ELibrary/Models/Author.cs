using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELibrary.Models
{
    [Table("authors")]
    public class Author : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Column("name", Order = 1)]
        public string Name { get; set; }

        [MaxLength(100)]
        [Column("email", Order = 2)]
        public string? Email { get; set; }

        public ICollection<BookAuthor> BooksAuthors { get; set; }
    }
}
