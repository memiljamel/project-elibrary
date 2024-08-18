using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class MemberFormViewModel
    {
        [HiddenInput]
        public Guid ID { get; set; }
        
        [DataType(DataType.Text)]
        [Display(Name = "Member Number")]
        public string MemberNumber { get; set; }
        
        [DataType(DataType.Text)]
        public string Name { get; set; }
        
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Numbers")]
        public string PhoneNumbers { get; set; }
        
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
    }
}