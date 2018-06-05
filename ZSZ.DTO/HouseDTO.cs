using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    [Serializable]
    public class HouseDTO : BaseDTO
    {
        public long CityId { get; set; }
        public String CityName { get; set; }
        public long RegionId { get; set; }
        public String RegionName { get; set; }
        public long CommunityId { get; set; }
        public String CommunityName { get; set; }
        public String CommunityLocation { get; set; }
        public String CommunityTraffic { get; set; }
        public int? CommunityBuiltYear { get; set; }

        public long RoomTypeId { get; set; }
        public String RoomTypeName { get; set; }
        public String Address { get; set; }
        public int MonthRent { get; set; }
        public long StatusId { get; set; }
        public String StatusName { get; set; }
        public decimal Area { get; set; }
        public long DecorateStatusId { get; set; }
        public String DecorateStatusName { get; set; }
        public int TotalFloorCount { get; set; }
        public int FloorIndex { get; set; }
        public long TypeId { get; set; }
        public String TypeName { get; set; }
        public String Direction { get; set; }
        public DateTime LookableDateTime { get; set; }
        public DateTime CheckInDateTime { get; set; }

        public String OwnerName { get; set; }
        public String OwnerPhoneNum { get; set; }
        public String Description { get; set; }
        public long[] AttachmentIds { get; set; }
        public String FirstThumbUrl { get; set; }
    }

}
