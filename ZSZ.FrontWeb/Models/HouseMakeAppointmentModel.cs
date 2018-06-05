using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.FrontWeb.Models
{
    public class HouseMakeAppointmentModel
    {
        public long HouseId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Phone]
        public string PhoneNum { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }
    }
}