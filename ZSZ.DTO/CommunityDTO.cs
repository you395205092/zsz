using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    public class CommunityDTO : BaseDTO
    {
        public String Name { get; set; }
        public long RegionId { get; set; }
        public String Location { get; set; }
        public String Traffic { get; set; }
        public int? BuiltYear { get; set; }
    }

}
