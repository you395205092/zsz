using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Service.Entities
{
    public class RegionEntity :BaseEntity
    {
        public string Name { get; set; }
        public long CityId { get; set; }

        public virtual CityEntity City { get; set; }
    }
}
