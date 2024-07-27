﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ELibrary.Models
{
   public class StaffEditViewModel
    {
        [Required]
        public Guid ID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Display(Name = "Employee Number")]
        [Required]
        [StringLength(16)]
        public string EmployeeNumber { get; set; }
        
        [Display(Name = "Role")]
        [Required]
        public AccessLevel AccessLevel { get; set; }
        
        [ValidateNever]
        public string Username { get; set; }
        
        [StringLength(256)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        [Display(Name = "Password Confirmation")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? PasswordConfirmation { get; set; }
    }
}