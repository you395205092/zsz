using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.App_Start;
using ZSZ.AdminWeb.Models;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class PermissionController : Controller
    {
        public IPermissionService PermSvc { get; set; }

        [CheckPermission("Permission.List")]
        public ActionResult List()
        {
            var perms = PermSvc.GetAll();
            return View(perms);
        }

        [CheckPermission("Permission.Delete")]
        public ActionResult GetDelete(long id)
        {
            PermSvc.MarkDeleted(id);
            //return RedirectToAction("List");//删除之后刷新
            return RedirectToAction(nameof(List));
        }

        [CheckPermission("Permission.Delete")]
        public ActionResult Delete2(long id)
        {
            PermSvc.MarkDeleted(id);
            return Json(new AjaxResult { Status="ok"});
        }

        [CheckPermission("Permission.Add")]
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [CheckPermission("Permission.Add")]
        [HttpPost]
        //public ActionResult Add(string name,string description)
        public ActionResult Add(PermissionAddNewModel model)
        {
            PermSvc.AddPermission(model.Name, model.Description);
            //return RedirectToAction(nameof(List));
            //todo:权限项名字不能重复
            return Json(new AjaxResult { Status = "ok" });
        }

        [CheckPermission("Permission.Edit")]
        [HttpGet]
        public ActionResult Edit(long id)
        {
            var perm = PermSvc.GetById(id);
            return View(perm);
        }

        [CheckPermission("Permission.Edit")]
        [HttpPost]
        public ActionResult Edit(PermissionEditModel model)
        {
            PermSvc.UpdatePermission(model.Id, model.Name, model.Description);
            //todo:检查name不能重复
            return Json(new AjaxResult { Status = "ok" });
        }

    }
}