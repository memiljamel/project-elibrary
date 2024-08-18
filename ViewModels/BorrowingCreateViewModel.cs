using System.ComponentModel.DataAnnotations;

namespace ELibrary.ViewModels
{
    public class BorrowingCreateViewModel
    {
        [Display(Name = "Member Number")]
        public Guid MemberID { get; set; }
        
        [Display(Name = "Title")]
        public Guid BookID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Borrow")]
        public DateOnly DateBorrow { get; set; }
    }
}