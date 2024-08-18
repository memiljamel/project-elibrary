using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class AuthorFormViewModel
    {
        [HiddenInput]
        public Guid ID { get; set; }
        
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email (optional)")]
        public string? Email { get; set; }
    }
}