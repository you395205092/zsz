using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
    public class HouseEntity:BaseEntity
    {
        public long CommunityId { get; set; }
        public virtual CommunityEntity Community { get; set; }
        public long RoomTypeId { get; set; }
        public virtual IdNameEntity RoomType { get; set; }
        public string Address { get; set; }
        public int MonthRent { get; set; }
        public long StatusId { get; set; }
        public virtual IdNameEntity Status { get; set; }
        public decimal Area { get; set; }
        public long DecorateStatusId { get; set; }
        public virtual IdNameEntity DecorateStatus { get; set; }
        public int TotalFloorCount { get; set; }
        public int FloorIndex { get; set; }
        public long TypeId { get; set; }
        public virtual IdNameEntity Type { get; set; }
        public string Direction { get; set; }
        public DateTime LookableDateTime { get; set; }
        public DateTime CheckInDateTime { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNum { get; set; }
        public string Description { get; set; }

        public virtual ICollection<AttachmentEntity> Attachments { get; set; } = new List<AttachmentEntity>();
        public virtual ICollection<HousePicEntity> HousePics { get; set; } = new List<HousePicEntity>();
    }
}
