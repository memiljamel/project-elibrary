using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ELibrary.Models
{
    public enum AccessLevel
    {
        Administrator,
        Staff
    }
    
    [Table("employees")]
    [Index(nameof(Username), nameof(EmployeeNumber), IsUnique = true)]
    public class Employee : BaseEntity
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
        [Column("employee_number", Order = 4)]
        public string EmployeeNumber { get; set; }

        [Required]
        [Column("role", Order = 5)]
        public AccessLevel AccessLevel { get; set; }
    }
}
