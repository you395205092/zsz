using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ZSZ.Common
{
    public class RuPengSMSSender
    {
        public string UserName { get; set; }
        public String AppKey { get; set; }
        public RuPengSMSResult SendSMS(string templateId,string code,string phoneNum)
        {
            WebClient wc = new WebClient();
            string url = "http://sms.rupeng.cn/SendSms.ashx?userName=" +
                Uri.EscapeDataString(UserName) + "&appKey=" + Uri.EscapeDataString(AppKey) +
                "&templateId=" + Uri.EscapeDataString(templateId)
                + "&code=" + Uri.EscapeDataString(code) +
                "&phoneNum=" + Uri.EscapeDataString(phoneNum);
            wc.Encoding = Encoding.UTF8;
            string resp = wc.DownloadString(url);
            JavaScriptSerializer jss = new JavaScriptSerializer();
            RuPengSMSResult result = jss.Deserialize<RuPengSMSResult>(resp);
            return result;
        }
    }
}
