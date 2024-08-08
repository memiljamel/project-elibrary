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
        
        [Display(Name = "Member Number")]
        [Required]
        public Guid MemberID { get; set; }
        
        [ValidateNever]
        public string MemberNumber { get; set; }
        
        [Display(Name = "Title")]
        [Required]
        public Guid BookID { get; set; }

        [Display(Name = "Date Borrow")]
        [ValidateNever]
        [DataType(DataType.Date)]
        public DateOnly DateBorrow { get; set; }
        
        [Display(Name = "Date Return")]
        [DataType(DataType.Date)]
        public DateOnly? DateReturn { get; set; }
    }
}