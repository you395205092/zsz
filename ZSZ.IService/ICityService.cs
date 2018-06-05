using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface ICityService:IServiceSupport
    {
        /// <summary>
        /// 新增城市
        /// </summary>
        /// <param name="cityName"></param>
        /// <returns>新增城市的id</returns>
        long AddNew(string cityName);

        //根据id获取城市DTO
        CityDTO GetById(long id);

        //获取所有城市
        CityDTO[] GetAll();

    }
}
