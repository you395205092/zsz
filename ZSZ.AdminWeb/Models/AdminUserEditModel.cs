using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserEditModel
    {
        public long Id { get; set; }

        [Required]
        [Phone]
        public string PhoneNum { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string Password2 { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public long? CityId { get; set; }
        public long[] RoleIds { get; set; }
    }
}