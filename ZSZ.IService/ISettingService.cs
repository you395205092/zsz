using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface ISettingService:IServiceSupport
    {
        //设置配置项name的值为value
        void SetValue(String name, String value);//SetValue("SmtpServer","smtp.qq.com")

        //获取配置项name的值
        String GetValue(String name);//GetValue("SmtpServer")

        void SetIntValue(string name, int value);//SetIntValue("秒数",5);

        int? GetIntValue(string name);

        void SetBoolValue(string name, bool value);

        bool? GetBoolValue(string name);

        SettingDTO[] GetAll();
    }

}
