using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
    public class CommunityEntity : BaseEntity
    {
        public string Name { get; set; }
        public long RegionId { set; get; }

        public virtual RegionEntity Region { set; get; }

        public string Location { get; set; }
        public string Traffic { get; set; }
        public int? BuiltYear { get; set; }
    }
}
