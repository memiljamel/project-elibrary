using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELibrary.Models
{
    public enum Category
    {
        Fiction,
        [Display(Name = "Non Fiction")]
        NonFiction,
        Biography,
        Automotive,
        Health,
        Technology,
        Science,
        History,
        Art,
        Business,
        [Display(Name = "Self Help")]
        SelfHelp,
        Comics,
        Novel,
        Poetry,
        Encyclopedia
    }
    
    [Table("books")]
    public class Book : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Column("title", Order = 3)]
        public string Title { get; set; }

        [Required]
        [Column("category", Order = 4)]
        public Category Category { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("publisher", Order = 5)]
        public string Publisher { get; set; }

        [Required]
        [Column("qty", Order = 6)]
        public int Quantity { get; set; }
        
        public ICollection<BookAuthor> BooksAuthors { get; set; }
    }
}
