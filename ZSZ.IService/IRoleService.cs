using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IRoleService:IServiceSupport
    {
        //新增角色
        long AddNew(String roleName);
        void Update(long roleId, String roleName);
        void MarkDeleted(long roleId);
        RoleDTO GetById(long id);
        RoleDTO GetByName(string name);
        RoleDTO[] GetAll();
        //给用户adminuserId增加权限roleIds
        void AddRoleIds(long adminUserId, long[] roleIds);

        //更新权限，先删再加
        void UpdateRoleIds(long adminUserId, long[] roleIds);

        //获取用户的角色
        RoleDTO[] GetByAdminUserId(long adminUserId);
    }

}
