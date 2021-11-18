using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Models.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Remote("IsNameInUse", "Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Remote("IsEmailInUse", "Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string RePassword { get; set; }
    }
}
