using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZSZ.AdminWeb.Models
{
    public class HouseAddModel
    {        

        [Required]
        public long CommunityId { get; set; }

        [Required]
        public long RoomTypeId { get; set; }

        [Required]
        public string address { get; set; }

        [Required]
        public int monthRent { get; set; }

        [Required]
        public long StatusId { get; set; }

        [Required]
        public decimal area { get; set; }

        [Required]
        public long DecorateStatusId { get; set; }

        [Required]
        public int floorIndex { get; set; }

        [Required]
        public int totalFloor { get; set; }

        [Required]
        public string direction { get; set; }

        [Required]
        public DateTime lookableDateTime { get; set; }

        [Required]
        public DateTime checkInDateTime { get; set; }

        [Required]
        public string ownerName { get; set; }

        [Required]
        public string ownerPhoneNum { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public long[] attachmentIds { get; set; }

        [Required]
        public long TypeId { get; set; }
    }
}