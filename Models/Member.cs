using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Models
{
    [Table("members")]
    [Index(nameof(MemberNumber), IsUnique = true)]
    public class Member : BaseEntity
    {
        [Required]
        [MaxLength(16)]
        [Column("member_number", Order = 1)]
        public string MemberNumber { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1024)]
        [Column("address", Order = 3)]
        public string Address { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("email", Order = 4)]
        public string Email { get; set; }

        public ICollection<Phone> Phones { get; set; }
    }
}
