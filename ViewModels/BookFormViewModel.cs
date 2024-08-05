using System.ComponentModel.DataAnnotations;
using ELibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace ELibrary.ViewModels
{
    public class BookFormViewModel
    {
        [Required]
        [HiddenInput]
        public Guid ID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        
        [Display(Name = "Authors")]
        [Required]
        public IEnumerable<Guid> AuthorIDs { get; set; }
        
        [Required]
        [EnumDataType(typeof(Category))]
        public Category Category { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Publisher { get; set; }
        
        [Required]
        public int Quantity { get; set; }
    }
}