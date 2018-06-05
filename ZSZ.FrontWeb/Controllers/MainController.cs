using CaptchaGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.FrontWeb.Models;
using ZSZ.IService;

namespace ZSZ.FrontWeb.Controllers
{
    public class MainController : Controller
    {

        public ICityService cityService { get; set; }
        public ISettingService settingService { get; set; }
        public IUserService userService { get; set; }

        public ActionResult Index()
        {
            long cityId = FrontUtils.GetCityId(HttpContext);
            //当前城市的名字
            string cityName =  cityService.GetById(cityId).Name;
            ViewBag.cityName = cityName;
            var cities = cityService.GetAll();
            return View(cities);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult CreateVerifyCode()
        {
            string verifyCode = CommonHelper.CreateVerifyCode(4);
            //验证码保存到TempData中最安全
            TempData["verifyCode"] = verifyCode;
            MemoryStream ms = ImageFactory.GenerateImage(verifyCode, 60, 100, 15, 6);
            return File(ms, "image/jpeg");
        }

        public ActionResult SendSmsVerifyCode(string phoneNum, string verifyCode)
        {
            string serverVerifyCode = (string)TempData["verifyCode"];//取服务器中保存的图形验证码
            if (serverVerifyCode != verifyCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "图形验证码填写错误" });
            }
            //配置信息从T_Settings 表读取
            string appKey = settingService.GetValue("如鹏短信平台AppKey");
            string userName = settingService.GetValue("如鹏短信平台UserName");
            string tempId = settingService.GetValue("如鹏短信平台注册短信模板Id");

            //短信验证码一般都是数字
            string smsCode = new Random().Next(1000, 9999).ToString();
            TempData["smsCode"] = smsCode;//给ActionResult Register(UserRegModel model)用

            RuPengSMSSender smsSender = new RuPengSMSSender();
            smsSender.AppKey = appKey;
            smsSender.UserName = userName;
            var sendResult = smsSender.SendSMS(tempId, smsCode, phoneNum);
            if (sendResult.code == 0)
            {
                //把发送验证码的手机号放到TempData，在注册的时候再次检查一下注册的是不是这个手机号
                //防止网站漏洞
                TempData["RegPhoneNum"] = phoneNum;

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

        [HttpPost]
        public ActionResult Register(UserRegModel model)
        {
            if (ModelState.IsValid == false)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = MVCHelper.GetValidMsg(ModelState)
                });
            }

            //检查一下注册时候的手机号是不是被改掉了。防止漏洞
            string serverPhoneNum = (string)TempData["RegPhoneNum"];
            if(serverPhoneNum!=model.PhoneNum)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "注册的手机号和刚才验证短信验证码的手机号不一致"
                });
            }

            //比较输入的短信验证码和服务器端保存的正确的验证码是否一致
            string serverSmsCode = (string)TempData["smsCode"];
            if (model.SmsCode != serverSmsCode)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "短信验证码错误" });
            }
            //漏洞
            if(userService.GetByPhoneNum(model.PhoneNum)!=null)
            {
                return Json(new AjaxResult { Status = "error", ErrorMsg = "此手机号已经被注册" });
            }
            userService.AddNew(model.PhoneNum, model.Password);
            return Json(new AjaxResult { Status="ok"});
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 切换当前用户的城市Id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public ActionResult SwitchCityId(long cityId)
        {
            long? userId = FrontUtils.GetUserId(HttpContext);
            if(userId==null)//无人登录
            {
                Session["CityId"] = cityId;
            }
            else
            {
                userService.SetUserCityId((long)userId, cityId);
            }
            return Json(new AjaxResult { Status="ok"});
        }

        [HttpPost]
        public ActionResult Login(UserLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = MVCHelper.GetValidMsg(ModelState)
                });
            }
            var user = userService.GetByPhoneNum(model.PhoneNum);
            if(user!=null)
            {
                if(userService.IsLocked(user.Id))
                {
                    //TimeSpan代表时间段，日期相减就代表相差的时间段
                    TimeSpan? leftTimeSpan = 
                        TimeSpan.FromMinutes(30)-(DateTime.Now-user.LastLoginErrorDateTime);
                    return Json(new AjaxResult
                    {
                        Status = "error",
                        ErrorMsg = "账号已被锁定，请" 
                            + (int)leftTimeSpan.Value.TotalMinutes + "分钟后再试"
                    });
                }
            }            

            bool isOK = userService.CheckLogin(model.PhoneNum, model.Password);
            
            if(isOK)
            {
                //一旦登录成功，就重置所有登录错误信息，避免影响下一次登录
                userService.ResetLoginError(user.Id);

                //把当前登录用户信息存入Session 
                Session["UserId"] = user.Id;
                Session["CityId"] = user.CityId;

                return Json(new AjaxResult
                {
                    Status = "ok"
                });
            }
            else
            {
                if(user!=null)//存在这个手机号
                {
                    userService.IncrLoginError(user.Id);
                }                
                return Json(new AjaxResult
                {
                    Status = "error",
                    ErrorMsg = "用户名或者密码错误"
                });
            }
        }
    }
}