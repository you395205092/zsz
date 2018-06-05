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
    public class SettingService : ISettingService
    {
        public SettingDTO[] GetAll()
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                /*
                return bs.GetAll().Select(s => new SettingDTO
                {
                    Id = s.Id,
                    CreateDateTime = s.CreateDateTime,
                    Name = s.Name,
                    Value = s.Value
                }).ToArray();*/
                List<SettingDTO> list = new List<SettingDTO>();
                foreach(var setting in bs.GetAll())
                {
                    SettingDTO dto = new SettingDTO();
                    dto.CreateDateTime = setting.CreateDateTime;
                    dto.Id = setting.Id;
                    dto.Name = setting.Name;
                    dto.Value = setting.Value;
                    list.Add(dto);
                }
                return list.ToArray();
            }
        }

        public bool? GetBoolValue(string name)
        {
            string value = GetValue(name);
            if(value==null)
            {
                return null;
            }
            else
            {
                return Convert.ToBoolean(value);
            }
        }

        public int? GetIntValue(string name)
        {
            string value = GetValue(name);
            if (value == null)
            {
                return null;
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }

        public string GetValue(string name)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                var setting = bs.GetAll().AsNoTracking()
                    .SingleOrDefault(s => s.Name == name);
                if (setting == null)//没有
                {
                    return null;
                }
                else
                {
                    return setting.Value;
                }
            }
        }

        public void SetBoolValue(string name, bool value)
        {
            SetValue(name, value.ToString());
        }

        public void SetIntValue(string name, int value)
        {
            SetValue(name, value.ToString());
        }

        public void SetValue(string name, string value)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<SettingEntity> bs = new BaseService<SettingEntity>(ctx);
                var setting = bs.GetAll().SingleOrDefault(s => s.Name == name);
                if(setting==null)//没有，则新增
                {
                    ctx.Settings.Add(new SettingEntity { Name=name,Value=value});
                }
                else
                {
                    setting.Value = value;
                }
                ctx.SaveChanges();
            }
        }
    }
}
