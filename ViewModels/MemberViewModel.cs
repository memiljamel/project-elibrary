using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class MemberViewModel
    {
        public Guid ID { get; set; }
        
        [Display(Name = "Member Number")]
        public string MemberNumber { get; set; }
        
        public string Name { get; set; }
        
        public string Email { get; set; }
        
        [Display(Name = "Phone Numbers")]
        public string PhoneNumbers { get; set; }
        
        public string Address { get; set; }
        
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}