using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELibrary.Models
{
    [Table("phones")]
    public class Phone : BaseEntity
    {
        [Required]
        [MaxLength(15)]
        [Column("phone_number", Order = 1)]
        public string PhoneNumber { get; set; }

        [ForeignKey(nameof(Member))]
        [Column("member_id", Order = 2)]
        public Guid MemberID { get; set; }

        public Member Member { get; set; }
    }
}
