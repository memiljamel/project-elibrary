using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class AuthorFormViewModel
    {
        [Required]
        [HiddenInput]
        public Guid ID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [EmailAddress]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}