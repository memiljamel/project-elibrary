using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ELibrary.ViewModels
{
    public class BorrowingEditViewModel
    {
        [Required]
        [HiddenInput]
        public Guid ID { get; set; }
        
        [Required]
        [Display(Name = "Member Number")]
        public Guid MemberID { get; set; }
        
        [ValidateNever]
        [DataType(DataType.Text)]
        public string MemberNumber { get; set; }
        
        [Required]
        [Display(Name = "Title")]
        public Guid BookID { get; set; }

        [ValidateNever]
        [DataType(DataType.Date)]
        [Display(Name = "Date Borrow")]
        public DateOnly DateBorrow { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Date Return")]
        public DateOnly? DateReturn { get; set; }
    }
}