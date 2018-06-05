using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;

namespace ZSZ.Service
{
    public class CityService : ICityService
    {

        public long AddNew(string cityName)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<CityEntity> bs 
                    = new BaseService<CityEntity>(ctx);
                //判断是否存在任何一条数据满足 c.Name == cityName
                //即存在这样一个名字的城市
                //如果只是判断“是否存在”，那么用Any效率比Where().count()效率高
                //Where(c => c.Name == cityName).Count()>0
                bool exists =bs.GetAll().Any(c => c.Name == cityName);
                if (exists)
                {
                    throw new ArgumentException("城市已经存在");
                }
                CityEntity city = new CityEntity();
                city.Name = cityName;
                ctx.Cities.Add(city);
                ctx.SaveChanges();
                return city.Id;
            }
        }

        private CityDTO ToDTO(CityEntity city)
        {
            CityDTO dto = new CityDTO();
            dto.CreateDateTime = city.CreateDateTime;
            dto.Id = city.Id;
            dto.Name = city.Name;
            return dto;
        }

        public CityDTO[] GetAll()
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                return bs.GetAll().AsNoTracking()
                    .ToList().Select(c => ToDTO(c)).ToArray();
            }
        }

        public CityDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<CityEntity> bs = new BaseService<CityEntity>(ctx);
                var city = bs.GetById(id);
                if(city==null)
                {
                    return null;
                }
                return ToDTO(city);
            }
        }
    }
}
