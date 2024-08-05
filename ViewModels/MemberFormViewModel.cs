using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class MemberFormViewModel
    {
        [Required]
        [HiddenInput]
        public Guid ID { get; set; }
        
        [Display(Name = "Member Number")]
        [Required]
        [StringLength(16, MinimumLength = 8)]
        public string MemberNumber { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Display(Name = "Phone Numbers")]
        [Required]
        [StringLength(256)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumbers { get; set; }
        
        [Required]
        [StringLength(1024)]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
    }
}