using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ZSZ.Service
{
    public class HouseAppointmentService : IHouseAppointmentService
    {
        public long AddNew(long? userId, string name, string phoneNum, long houseId, DateTime visitDate)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                HouseAppointmentEntity houseApp = new HouseAppointmentEntity();
                houseApp.HouseId = houseId;
                houseApp.Name = name;
                houseApp.PhoneNum = phoneNum;
                houseApp.Status = "未处理";
                houseApp.UserId = userId;
                houseApp.VisitDate = visitDate;
                ctx.HouseAppointments.Add(houseApp);
                ctx.SaveChanges();
                return houseApp.Id;
            }
        }

        public bool Follow(long adminUserId, long houseAppointmentId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> bs = 
                    new BaseService<HouseAppointmentEntity>(ctx);
                var app = bs.GetById(houseAppointmentId);
                if(app==null)
                {
                    throw new ArgumentException("不存在的订单id");
                }
                //FollowAdminUserId不为null，说明要么是自己已经抢过，要么是已经早早的
                //被别人抢了
                if (app.FollowAdminUserId!=null)
                {
                    return app.FollowAdminUserId == adminUserId;
                    /*
                    if(app.FollowAdminUserId==adminUserId)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }*/
                }
                //如果/FollowAdminUserId为null，说明有抢的机会
                app.FollowAdminUserId = adminUserId;
                try
                {
                    ctx.SaveChanges();
                    return true;
                }//如果抛出DbUpdateConcurrencyException说明抢单失败（乐观锁）
                catch (DbUpdateConcurrencyException)
                {
                    return false;
                }                
            }
        }

        private HouseAppointmentDTO ToDTO(HouseAppointmentEntity houseApp)
        {
            HouseAppointmentDTO dto = new HouseAppointmentDTO();
            dto.CommunityName = houseApp.House.Community.Name;
            dto.CreateDateTime = houseApp.CreateDateTime;
            dto.FollowAdminUserId = houseApp.FollowAdminUserId;
            if(houseApp.FollowAdminUser!=null)
            {
                dto.FollowAdminUserName = houseApp.FollowAdminUser.Name;
            }
            dto.FollowDateTime = houseApp.FollowDateTime;
            dto.HouseId = houseApp.HouseId;
            dto.Id = houseApp.Id;
            dto.Name = houseApp.Name;
            dto.PhoneNum = houseApp.PhoneNum;
            dto.RegionName = houseApp.House.Community.Region.Name;
            dto.Status = houseApp.Status;
            dto.UserId = houseApp.UserId;
            dto.VisitDate = houseApp.VisitDate;
            dto.HouseAddress = houseApp.House.Address;
            return dto;
        }

        public HouseAppointmentDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> bs 
                    = new BaseService<HouseAppointmentEntity>(ctx);
                var houseApp = bs.GetAll().Include(a => a.House)
                    //Include("House.Community")
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community))
                    .Include(a => a.FollowAdminUser)
                    //Include("House.Community.Region")
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .AsNoTracking().SingleOrDefault(a => a.Id == id);
                if(houseApp==null)
                {
                    return null;
                }
                return ToDTO(houseApp);
            }
        }

        public HouseAppointmentDTO[] GetPagedData(long cityId, string status, 
            int pageSize, int currentIndex)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> bs
                    = new BaseService<HouseAppointmentEntity>(ctx);
                var apps = bs.GetAll().Include(a => a.House)
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community))
                    .Include(a => a.FollowAdminUser)
                    .Include(nameof(HouseAppointmentEntity.House) + "." + nameof(HouseEntity.Community) + "." + nameof(CommunityEntity.Region))
                    .AsNoTracking()
                    .Where(a => a.House.Community.Region.CityId == cityId && a.Status == status)
                    .OrderByDescending(a => a.CreateDateTime)//Skip之前一定要调用OrderBy
                    .Skip(currentIndex).Take(pageSize);
                return apps.ToList().Select(a=>ToDTO(a)).ToArray();
            }
        }

        public long GetTotalCount(long cityId, string status)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<HouseAppointmentEntity> bs
                    = new BaseService<HouseAppointmentEntity>(ctx);
                var count = bs.GetAll()
                    //Where(a => a.House.Community.Region.CityId == cityId && a.Status == status).LongCount()
                    .LongCount(a => a.House.Community.Region.CityId == cityId && a.Status == status);
                return count;
            }
        }
    }
}
