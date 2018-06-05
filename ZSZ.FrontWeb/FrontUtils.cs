using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZSZ.IService;

namespace ZSZ.FrontWeb
{
    public class FrontUtils
    {
        /// <summary>
        /// 获得当前登录用户id，如果没有登录则返回null
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static long? GetUserId(HttpContextBase ctx)
        {
            return (long?)ctx.Session["UserId"];
        }

        /// <summary>
        /// 获得当前城市Id
        /// </summary>
        /// <param name="ctx"></param>
        /// <returns></returns>
        public static long GetCityId(HttpContextBase ctx)
        {
            long? userId =  GetUserId(ctx);
            if(userId==null)//如果没有用户登录
            {
                long? cityId = (long?)ctx.Session["CityId"];
                //如果Session中存着CityId，则以此为当前Session的城市Id
                if (cityId!=null)
                {
                    return cityId.Value;
                }
                else//如果Session中没有存着CityId
                //则以第一个城市为城市Id
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll()[0].Id;
                }
            }
            else//如果有登录
            {
                //如果用户有CityId，则以此为返回值
                var userService = DependencyResolver.Current.GetService<IUserService>(); ;
                long? cityId = userService.GetById(userId.Value).CityId;
                if(cityId==null)//如果没有CityId，则以第一个城市为返回值
                {
                    var citySvc = DependencyResolver.Current.GetService<ICityService>();
                    return citySvc.GetAll()[0].Id;
                }
                else
                {
                    return cityId.Value;
                }
            }
        }
    }
}