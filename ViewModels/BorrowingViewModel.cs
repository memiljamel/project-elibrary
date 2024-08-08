using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class BorrowingViewModel
    {
        public Guid ID { get; set; }
        
        [Display(Name = "Member Number")]
        public string MemberNumber { get; set; }
        
        [Display(Name = "Title")]
        public string BookTitle { get; set; }
        
        [Display(Name = "Date Borrow")]
        public DateOnly DateBorrow { get; set; }
        
        [Display(Name = "Date Return")]
        [DisplayFormat(NullDisplayText = "Not Returned")]
        public DateOnly? DateReturn { get; set; }
        
        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }
        
        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; }
    }
}