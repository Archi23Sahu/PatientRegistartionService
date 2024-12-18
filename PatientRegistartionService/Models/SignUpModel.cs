﻿using System.ComponentModel.DataAnnotations;

namespace PatientRegistartionService.Models
{
    public class SignUpModel
    {       

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
