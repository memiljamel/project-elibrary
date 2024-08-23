using System.ComponentModel.DataAnnotations;
using ELibrary.Models;

namespace ELibrary.ViewModels
{
    public class EmployeeViewModel
    {
        public Guid ID { get; set; }
        
        [Display(Name = "Employee Number")]
        public string EmployeeNumber { get; set; }
        
        public string Name { get; set; }
        
        [Display(Name = "Role")]
        public AccessLevel AccessLevel { get; set; }
        
        public string Username { get; set; }
        
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}