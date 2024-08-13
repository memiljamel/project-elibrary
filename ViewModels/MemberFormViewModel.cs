using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class MemberFormViewModel
    {
        [Required]
        [HiddenInput]
        public Guid ID { get; set; }
        
        [Required]
        [StringLength(16, MinimumLength = 8)]
        [DataType(DataType.Text)]
        [Display(Name = "Member Number")]
        public string MemberNumber { get; set; }
        
        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required]
        [StringLength(256)]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Numbers")]
        public string PhoneNumbers { get; set; }
        
        [Required]
        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
    }
}