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
        [DataType(DataType.Text)]
        public string Title { get; set; }
        
        [Required]
        [Display(Name = "Authors")]
        public IEnumerable<Guid> AuthorIDs { get; set; }
        
        [Required]
        [EnumDataType(typeof(Category))]
        public Category Category { get; set; }
        
        [Required]
        [StringLength(100)]
        [DataType(DataType.Text)]
        public string Publisher { get; set; }
        
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}