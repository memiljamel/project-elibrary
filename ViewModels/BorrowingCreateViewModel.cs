using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class BorrowingCreateViewModel
    {
        [Display(Name = "Member Number")]
        [Required]
        public Guid MemberID { get; set; }
        
        [Display(Name = "Title")]
        [Required]
        public Guid BookID { get; set; }

        [Display(Name = "Date Borrow")]
        [Required]
        [DataType(DataType.Date)]
        public DateOnly DateBorrow { get; set; }
    }
}