using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELibrary.Models
{
    [Table("borrowings")]
    public class Borrowing : BaseEntity
    {
        [Required]
        [Column("date_borrow", Order = 1)]
        public DateOnly DateBorrow { get; set; }

        [Column("date_return", Order = 2)]
        public DateOnly? DateReturn { get; set; }

        [ForeignKey(nameof(Member))]
        [Column("member_id", Order = 3)]
        public Guid MemberID { get; set; }

        [ForeignKey(nameof(Book))]
        [Column("book_id", Order = 4)]
        public Guid BookID { get; set; }

        public Member Member { get; set; }

        public Book Book { get; set; }
    }
}
