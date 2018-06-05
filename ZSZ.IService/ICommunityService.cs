using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface ICommunityService : IServiceSupport
    {
        //获取区域regionId下的所有小区
        CommunityDTO[] GetByRegionId(long regionId);
    }

}
