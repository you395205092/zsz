using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IRegionService:IServiceSupport
    {
        RegionDTO GetById(long id);

        //获取城市下的区域
        RegionDTO[] GetAll(long cityId);
    }

}
