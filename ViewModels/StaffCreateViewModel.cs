using System.ComponentModel.DataAnnotations;
using ELibrary.Models;

namespace ELibrary.ViewModels
{
    public class StaffCreateViewModel
    {
        [Display(Name = "Employee Number")]
        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string EmployeeNumber { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Display(Name = "Role")]
        [Required]
        [EnumDataType(typeof(AccessLevel))]
        public AccessLevel AccessLevel { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string PasswordConfirmation { get; set; }
    }
}