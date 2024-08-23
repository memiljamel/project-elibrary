using System.ComponentModel.DataAnnotations;
using ELibrary.Models;

namespace ELibrary.ViewModels
{
    public class ProfileViewModel
    {
        [DataType(DataType.Text)]
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }
        
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Display(Name = "Role")]
        public AccessLevel AccessLevel { get; set; }
        
        [DataType(DataType.Text)]
        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Password (optional)")]
        public string? Password { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Password Confirmation (optional)")]
        public string? PasswordConfirmation { get; set; }
    }
}