using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.AdminWeb.Models;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.AdminWeb.Controllers
{
    public class MainController : Controller
    {
        public IAdminUserService userService { get; set; }

        public ActionResult Index()
        {
            long? userId = AdminHelper.GetUserId(HttpContext);
            if(userId==null)
            {
                return Redirect("~/Main/Login");
            }
            var user = userService.GetById((long)userId);
            return View(user);
        }

        public ActionResult Logout()
        {
            Session.Abandon();//销毁Session
            return Redirect("~/Main/Login");
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new AjaxResult { Status="error",
                    ErrorMsg =MVCHelper.GetValidMsg(ModelState)});
            }

            //TODO：有漏洞。和验证码有关。

            //if (model.VerifyCode != Session["verifyCode"])
            //string s="abc";string s1="abc";
            //if (model.VerifyCode != (string)Session["verifyCode"])
            if (model.VerifyCode != (string)TempData["verifyCode"])
            {
                return Json(new AjaxResult { Status="error",ErrorMsg="验证码错误"});
            }
            bool result = userService.CheckLogin(model.PhoneNum, model.Password);
            if(result)
            {
                //Session中保存当前登录用户Id
                Session["LoginUserId"] 
                    = userService.GetByPhoneNum(model.PhoneNum).Id;
                //给后面检查“当前Session登录的这个用户有没有***的权限”
                return Json(new AjaxResult { Status="ok"});
            }
            else
            {
                return Json(new AjaxResult { Status = "error",ErrorMsg="用户名或者密码错误" });
            }
        }

        public ActionResult CreateVerifyCode()
        {
            string verifyCode = CommonHelper.CreateVerifyCode(4);
            TempData["verifyCode"] = verifyCode;
            //Session["verifyCode"] = verifyCode;
            /*
            using (MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20, 6))
            {
                return File(ms, "image/jpeg");
            }*/
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 20, 6);
            return File(ms, "image/jpeg");
        }
    }
}