using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.FrontWeb.Models
{
    public class UserRegModel
    {
        [Required]
        [Phone]
        public string PhoneNum { get; set; }

        [Required]
        [StringLength(4,MinimumLength =4)]
        public string SmsCode { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }
    }
}