using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ApplicationServices;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class RoleController : Controller
    {
        public IRoleService roleService { get; set; }
        public IPermissionService permService { get; set; }

        [CheckPermission("Role.List")]
        public ActionResult List()
        {
            var roles = roleService.GetAll();
            return View(roles);
        }

        [CheckPermission("Role.Delete")]
        public ActionResult Delete(long id)
        {
            roleService.MarkDeleted(id);
            return Json(new AjaxResult { Status="ok"});
        }

        [CheckPermission("Role.Delete")]
        public ActionResult BatchDelete(long[] selectdIds)
        {
            foreach(long id in selectdIds)
            {
                roleService.MarkDeleted(id);
            }
            return Json(new AjaxResult { Status = "ok" });
        }

        [CheckPermission("Role.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            var perms = permService.GetAll();//所有可用的权限项
            return View(perms);
        }

        [CheckPermission("Role.Add")]
        [HttpPost]
        public ActionResult Add(RoleAddModel model)
        {
            //检查Model验证是否通过
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status="error",
                    ErrorMsg =MVCHelper.GetValidMsg(ModelState)});
            }
            //TransactionScope
            long roleId = roleService.AddNew(model.Name);
            permService.AddPermIds(roleId, model.PermissionIds);
            return Json(new AjaxResult { Status="ok"});
        }

        [CheckPermission("Role.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var role = roleService.GetById(id);
            var rolePerms = permService.GetByRoleId(id);//id这个角色拥有的权限项
            var allPerms = permService.GetAll();//全部的权限
            /*
            ViewBag.role = role;
            ViewBag.rolePerms = rolePerms;*/
            RoleEditGetModel model = new RoleEditGetModel();
            model.AllPerms = allPerms;
            model.Role = role;
            model.RolePerms = rolePerms;

            return View(model);
        }

        [CheckPermission("Role.Edit")]
        [HttpPost]
        public ActionResult Edit(RoleEditModel model)
        {
            roleService.Update(model.Id, model.Name);
            permService.UpdatePermIds(model.Id, model.PermissionIds);
            return Json(new AjaxResult { Status="ok"});
        }
    }
}