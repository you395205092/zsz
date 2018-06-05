using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class UserController : Controller
    {
        public ISettingService settingService { get; set; }
        public IUserService userService { get; set; }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string phoneNum,
            string verifyCode)
        {
            string serverVerifyCode = (string)TempData["verifyCode"];
            if (serverVerifyCode != verifyCode)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "验证码错误"
                });
            }
            var user = userService.GetByPhoneNum(phoneNum);
            if (user == null)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "没有这个手机号"
                });
            }
            string appKey = settingService.GetValue("如鹏短信平台AppKey");
            string userName = settingService.GetValue("如鹏短信平台UserName");
            string tempId = settingService.GetValue("如鹏短信平台找回密码短信模板Id");
            string smsCode = new Random().Next(1000, 9999).ToString();

            RuPengSMSSender smsSender = new RuPengSMSSender();
            smsSender.AppKey = appKey;
            smsSender.UserName = userName;
            var sendResult = smsSender.SendSMS(tempId, smsCode, phoneNum);
            if (sendResult.code == 0)
            {
                TempData["ForgotPasswordPhoneNum"] = phoneNum;
                TempData["SmsCode"] = smsCode;
                return Json(new AjaxResult { Status = "ok" });
            }
            else
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = sendResult.msg
                });
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword2()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword2(string smsCode)
        {
            string serverSmsCode = (string)TempData["SmsCode"];
            if (smsCode != serverSmsCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "短信验证码错误" });
            }
            else
            {
                //告诉第3步“短信验证码验证通过”，防止恶意用户跳过ForgotPassword2直接重置密码
                TempData["ForgotPassword2_OK"] = true;
                return Json(new AjaxResult
                {
                    Status = "ok"
                });
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword3()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword3(string password)
        {
            //防止恶意用户跳过ForgotPassword2直接重置密码
            bool? is2_OK = (bool?)TempData["ForgotPassword2_OK"];
            if(is2_OK!=true)
            {
                return Json(new AjaxResult { Status = "error",ErrorMsg="您没有通过短信验证码的验证" });
            }

            //需要重置密码的手机号
            string phoneNum =  (string)TempData["ForgotPasswordPhoneNum"];
            var user = userService.GetByPhoneNum(phoneNum);
            userService.UpdatePwd(user.Id, password);
            return Json(new AjaxResult { Status="ok"});
        }

        [HttpGet]
        public ActionResult ForgotPassword4()
        {
            return View();
        }
    }
}