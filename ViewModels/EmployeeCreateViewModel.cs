using System.ComponentModel.DataAnnotations;
using ELibrary.Models;

namespace ELibrary.ViewModels
{
    public class EmployeeCreateViewModel
    {
        [Required]
        [StringLength(16, MinimumLength = 8)]
        [DataType(DataType.Text)]
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }
        
        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(AccessLevel))]
        [Display(Name = "Role")]
        public AccessLevel AccessLevel { get; set; }
        
        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Username { get; set; }
        
        [Required]
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirmation { get; set; }
    }
}