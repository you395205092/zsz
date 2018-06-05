using Autofac.Integration.Mvc;
using Autofac;
using log4net;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZSZ.IService;
using System.Text;
using System.Net.Mail;

namespace ZSZ.AdminWeb.Jobs
{
    public class BossReportJob : IJob
    {
        private static ILog log
            = LogManager.GetLogger(typeof(BossReportJob));

        public void Execute(IJobExecutionContext context)
        {
            log.Debug("准备收集今日新增房源数量");
            try
            {
                string bossEmails;//老板邮箱
                string smtpServer, smtpUserName, smtpPassword, smtpEmail;
                StringBuilder sbMsg = new StringBuilder();
                var container = AutofacDependencyResolver.Current.ApplicationContainer;
                using (container.BeginLifetimeScope())
                {
                    var cityService = container.Resolve<ICityService>();
                    var houseService = container.Resolve<IHouseService>();
                    var settingService = container.Resolve<ISettingService>();
                    bossEmails = settingService.GetValue("老板邮箱");//读取配置中的老板邮箱 
                    smtpServer = settingService.GetValue("SmtpServer");
                    smtpUserName = settingService.GetValue("SmtpUserName");
                    smtpPassword = settingService.GetValue("SmtpPassword");
                    smtpEmail = settingService.GetValue("SmtpEmail");
                    foreach (var city in cityService.GetAll())
                    {
                        long count = houseService.GetTodayNewHouseCount(city.Id);
                        sbMsg.Append(city.Name).Append("新增房源的数量是：")
                            .Append(count).AppendLine();
                    }
                }
                log.Debug("收集新增房源数量完成"+sbMsg);
                //要使用System.Net.Mail下的类，不要用System.Web.Mail下的类
                using (MailMessage mailMessage = new MailMessage())
                using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                {
                    //由于可能有多个老板都想收到这个邮件，因此在配置中可以用分号
                    //分隔开各个老板的邮箱地址
                    foreach(var bossEmail in bossEmails.Split(';'))
                    {
                        mailMessage.To.Add(bossEmail);
                    }
                    mailMessage.Body = sbMsg.ToString();
                    mailMessage.From = new MailAddress(smtpEmail);
                    mailMessage.Subject = "今日新增房源数量报表";
                    smtpClient.Credentials 
                        = new System.Net.NetworkCredential(smtpUserName, smtpPassword);//如果启用了“客户端授权码”，要用授权码代替密码
                    smtpClient.Send(mailMessage);
                }
                log.Debug("给老板发送新增房源数量报表完成");
            }
            catch (Exception ex)
            {
                log.Error("给老板发报表出错", ex);
            }
        }
    }
}