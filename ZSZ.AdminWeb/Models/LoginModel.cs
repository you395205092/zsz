using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class LoginModel
    {
        [Required]
        [Phone]
        public string PhoneNum { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string VerifyCode { get; set; }
    }
}