using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;
using ZSZ.IService;
using ZSZ.Service.Entities;
using System.Data.Entity;
using ZSZ.Common;

namespace ZSZ.Service
{
    public class AdminUserService : IAdminUserService
    {
        public long AddAdminUser(string name, string phoneNum, 
            string password, string email, long? cityId)
        {
            AdminUserEntity user = new AdminUserEntity();
            user.CityId = cityId;
            user.Email = email;
            user.Name = name;
            user.PhoneNum = phoneNum;
            string salt = CommonHelper.CreateVerifyCode(5);//盐
            user.PasswordSalt = salt;
            //Md5(盐+用户密码)
            string pwdHash = CommonHelper.CalcMD5(salt+password);
            user.PasswordHash = pwdHash;
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs 
                    = new BaseService<AdminUserEntity>(ctx);
                bool exists = bs.GetAll().Any(u => u.PhoneNum == phoneNum);
                if(exists)
                {
                    throw new ArgumentException("手机号已经存在"+phoneNum);
                }
                ctx.AdminUsers.Add(user);
                ctx.SaveChanges();
                return user.Id;
            }
        }

        public bool CheckLogin(string phoneNum, string password)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs = new BaseService<AdminUserEntity>(ctx);
                //除了错不可怕，最怕的是有错但是表面“风平浪静”
                var user = bs.GetAll().SingleOrDefault(u=>u.PhoneNum==phoneNum);
                if (user == null)
                {
                    return false;
                }
                string dbHash = user.PasswordHash;
                string userHash = CommonHelper.CalcMD5(user.PasswordSalt+password);
                //比较数据库中的PasswordHash是否和MD5(salt+用户输入密码)一直
                return userHash == dbHash;
            }
        }

        private AdminUserDTO ToDTO(AdminUserEntity user)
        {
            AdminUserDTO dto = new AdminUserDTO();
            dto.CityId = user.CityId;
            if(user.City!=null)
            {
                dto.CityName = user.City.Name;//需要Include提升性能
                //如鹏总部（北京）、如鹏网上海分公司、如鹏广州分公司、如鹏北京分公司
            }
            else
            {
                dto.CityName = "总部";
            }
            
            dto.CreateDateTime = user.CreateDateTime;
            dto.Email = user.Email;
            dto.Id = user.Id;
            dto.LastLoginErrorDateTime = user.LastLoginErrorDateTime;
            dto.LoginErrorTimes = user.LoginErrorTimes;
            dto.Name = user.Name;
            dto.PhoneNum = user.PhoneNum;
            return dto;
        }

        public AdminUserDTO[] GetAll()
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                //using System.Data.Entity;才能在IQueryable中用Include、AsNoTracking
                BaseService<AdminUserEntity> bs 
                    = new BaseService<AdminUserEntity>(ctx);
                return bs.GetAll().Include(u=>u.City)
                    .AsNoTracking().ToList().Select(u => ToDTO(u)).ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cityId">如果为null则获取总部的管理员；否则是获取某个地区的</param>
        /// <returns></returns>
        public AdminUserDTO[] GetAll(long? cityId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs
                    = new BaseService<AdminUserEntity>(ctx);
                //CityId is null;CityId=3
                var all = bs.GetAll().Include(u => u.City)
                    .AsNoTracking().Where(u => u.CityId == cityId);
                return all.ToList().Select(u => ToDTO(u)).ToArray();
            }
        }

        public AdminUserDTO GetById(long id)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs
                    = new BaseService<AdminUserEntity>(ctx);
                //这里不能用bs.GetById(id);因为无法Include、AsNoTracking()等
                var user = bs.GetAll().Include(u => u.City)
                    .AsNoTracking().SingleOrDefault(u=>u.Id==id);
                    //.AsNoTracking().Where(u=>u.Id==id).SingleOrDefault();
                //var user = bs.GetById(id); 用include就不能用GetById
                if (user==null)
                {
                    return null;
                }
                return ToDTO(user);
            }
        }

        public AdminUserDTO GetByPhoneNum(string phoneNum)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs
                    = new BaseService<AdminUserEntity>(ctx);
                var users = bs.GetAll().Include(u => u.City)
                    .AsNoTracking().Where(u => u.PhoneNum == phoneNum);
                int count = users.Count();
                if(count <= 0)
                {
                    return null;
                }
                else if(count==1)
                {
                    return ToDTO(users.Single());
                }
                else
                {
                    throw new ApplicationException("找到多个手机号为"+phoneNum+"的管理员");
                }
            }
        }

        //HasPermission(5,"User.Add")
        public bool HasPermission(long adminUserId, string permissionName)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs
                    = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetAll().Include(u => u.Roles)
                    .AsNoTracking().SingleOrDefault(u=>u.Id==adminUserId);
                //var user = bs.GetById(adminUserId);
                if (user==null)
                {
                    throw new ArgumentException("找不到id="+adminUserId+"的用户");
                }
                //每个Role都有一个Permissions属性
                //Roles.SelectMany(r => r.Permissions)就是遍历Roles的每一个Role
                //然后把每个Role的Permissions放到一个集合中
                //IEnumerable<PermissionEntity>
                return user.Roles.SelectMany(r => r.Permissions)
                    .Any(p=>p.Name==permissionName);
            }
        }

        public void MarkDeleted(long adminUserId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs 
                    = new BaseService<AdminUserEntity>(ctx);
                bs.MarkDeleted(adminUserId);
            }
        }

        public void RecordLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void ResetLoginError(long id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAdminUser(long id, string name, string phoneNum, 
            string password, string email, long? cityId)
        {
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                BaseService<AdminUserEntity> bs
                    = new BaseService<AdminUserEntity>(ctx);
                var user = bs.GetById(id);
                if(user==null)
                {
                    throw new ArgumentException("找不到id="+id+"的管理员");
                }
                user.Name = name;
                user.PhoneNum = phoneNum;
                user.Email = email;
                if(!string.IsNullOrEmpty(password))
                {
                    user.PasswordHash =
                    CommonHelper.CalcMD5(user.PasswordSalt + password);
                }                
                user.CityId = cityId;                
                ctx.SaveChanges();
            }
        }
    }
}
