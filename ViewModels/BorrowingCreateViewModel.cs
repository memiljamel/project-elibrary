using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class BorrowingCreateViewModel
    {
        [Required]
        [Display(Name = "Member Number")]
        public Guid MemberID { get; set; }
        
        [Required]
        [Display(Name = "Title")]
        public Guid BookID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date Borrow")]
        public DateOnly DateBorrow { get; set; }
    }
}