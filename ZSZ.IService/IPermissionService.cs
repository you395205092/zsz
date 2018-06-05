using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IPermissionService:IServiceSupport
    {
        long AddPermission(string permName, string description);
        void UpdatePermission(long id, string permName,
            string description);
        void MarkDeleted(long id);

        PermissionDTO GetById(long id);
        PermissionDTO[] GetAll();
        PermissionDTO GetByName(String name);//GetByName("User.Add")

        //获取角色的权限
        PermissionDTO[] GetByRoleId(long roleId);

        //给角色roleId增加权限项id permIds
        void AddPermIds(long roleId, long[] permIds);

        //更新角色role的权限项：先删除再添加
        void UpdatePermIds(long roleId, long[] permIds);
    }

}
