using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.DTO;

namespace ZSZ.AdminWeb.Models
{
    public class AdminUserEditViewModel
    {
        public AdminUserDTO AdminUser { get; set; }
        public CityDTO[] Cities { get; set; }
        public RoleDTO[] Roles { get; set; }

        /// <summary>
        /// 当前用户拥有的角色id
        /// </summary>
        public long[] UserRoleIds { get; set; }
    }
}