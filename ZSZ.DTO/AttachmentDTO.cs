using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.DTO
{
    [Serializable]
    public class AttachmentDTO : BaseDTO
    {
        public String Name { get; set; }
        public String IconName { get; set; }
    }

}
