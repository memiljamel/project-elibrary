using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class BorrowingEditViewModel
    {
        [HiddenInput]
        public Guid ID { get; set; }
        
        [Display(Name = "Member Number")]
        public Guid MemberID { get; set; }
        
        public string MemberNumber { get; set; }
        
        [Display(Name = "Title")]
        public Guid BookID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Borrow")]
        public DateOnly DateBorrow { get; set; }
        
        [DataType(DataType.Date)]
        [Display(Name = "Date Return")]
        public DateOnly? DateReturn { get; set; }
    }
}