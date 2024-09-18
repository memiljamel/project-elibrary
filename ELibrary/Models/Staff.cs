using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ELibrary.Enums;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Models
{
    [Table("staffs")]
    [Index(nameof(Username), nameof(StaffNumber), IsUnique = true)]
    public class Staff : BaseEntity
    {
        [Required]
        [Column("username", Order = 1)]
        [MaxLength(100)]
        public string Username { get; set; }

        [Required]
        [MaxLength(256)]
        [Column("password", Order = 2)]
        public string Password { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("name", Order = 3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(16)]
        [Column("staff_number", Order = 4)]
        public string StaffNumber { get; set; }

        [Required]
        [Column("role", Order = 5)]
        public AccessLevelEnum AccessLevel { get; set; }
    }
}
