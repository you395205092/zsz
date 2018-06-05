using Autofac;
using CaptchaGen;
using CodeCarvings.Piczard;
using CodeCarvings.Piczard.Filters.Watermarks;
using log4net;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZSZ.Common;
using ZSZ.CommonMVC;
using ZSZ.Service;

namespace ZSZ.Tests
{
    class Prog
    {
        public static int Add(int i, int j)
        {
            /*
            if(i==0||j==0)
            {
                return 0;
            }*/
            return i + j;
        }

        //Assert：断言。
        public static void AssertEqual(int value,int expectValue)
        {
            if(value!=expectValue)
            {
                throw new Exception("两个值不一致");
            }
        }
        public static void Main (string[] args)
        {
            /*
            AssertEqual(Add(1,1),2);//TestCase
            AssertEqual(Add(1, 0),1);//TestCase
            AssertEqual(Add(0, 0),0);
            AssertEqual(Add(-1,1),0);*/
            /*
            using (ZSZDbContext ctx = new ZSZDbContext())
            {
                ctx.Database.Delete();
                ctx.Database.Create();
            }*/

            NameValueCollection nvc = new NameValueCollection();
            nvc["id"] = "5";
            nvc["age"] = "6";
            nvc["name"] = "如鹏网";
            Console.WriteLine(MVCHelper.ToQueryString(nvc));
            Console.WriteLine(MVCHelper.RemoveQueryString(nvc,"age"));
            Console.WriteLine(MVCHelper.RemoveQueryString(nvc, "aaaa"));
            Console.WriteLine(MVCHelper.UpdateQueryString(nvc,"age","888"));
            Console.WriteLine(MVCHelper.UpdateQueryString(nvc, "height", "1.80"));
            Console.WriteLine("ok");
            Console.ReadKey();
        }

        public static void Main3(string[] args)
        {

            string userName = "rupengtest1";
            string appKey = "fdsafasdf@adfasdfa";
            string templateId = "183";
            string code = "6666";
            string phoneNum = "18918918189";
            /*
                        WebClient wc = new WebClient();
                        string url = "http://sms.rupeng.cn/SendSms.ashx?userName=" +
                            Uri.EscapeDataString(userName) + "&appKey=" + Uri.EscapeDataString(appKey) +
                            "&templateId=" + templateId + "&code=" + Uri.EscapeDataString(code) +
                            "&phoneNum=" + phoneNum;
                        wc.Encoding = Encoding.UTF8;
                        string resp = wc.DownloadString(url);
                        //发出url这样一个http请求（Get请求）返回值为响应报文体
                        Console.WriteLine(resp);
                        */
            RuPengSMSSender sender = new RuPengSMSSender();
            sender.AppKey = appKey;
            sender.UserName = userName;
            var result = sender.SendSMS(templateId, code, phoneNum);
            Console.WriteLine("返回码："+result.code+",消息："+result.msg);
            Console.WriteLine("ok");
            Console.ReadKey();
        }
        // public IDogBll DogBll { get; set; }
        public static void Main2(string[] args)
        {
            /*
            string s = CommonHelper.CreateVerifyCode(4);
            Console.WriteLine(s);*/

            /*
            while(true)
            { 
            using (MailMessage mailMessage = new MailMessage())
            using (SmtpClient smtpClient = new SmtpClient("smtp.163.com"))
            {
                mailMessage.To.Add("about521@163.com");
                mailMessage.To.Add("yzk365@qq.com");
                mailMessage.Body = "各位如鹏的同学大家好，我是你们的老朋友";
                mailMessage.From = new MailAddress("rupengtest01@163.com");
                mailMessage.Subject = "如鹏同学好";
                smtpClient.EnableSsl = true;//如果邮箱需要开启SSL访问
                smtpClient.Credentials = 
                    new System.Net.NetworkCredential("rupengtest01@163.com", "123rupeng");//如果启用了“客户端授权码”，要用授权码代替密码
                smtpClient.Send(mailMessage);
            }
            }
            */
            /*
            ImageProcessingJob job = new ImageProcessingJob();
            job.Filters.Add(new FixedResizeConstraint(200,200));
            job.SaveProcessedImageToFileSystem(@"D:\a\Tulips.jpg", @"D:\a\1.jpg",
                new JpegFormatEncoderParams());
            //job.SaveProcessedImageToStream()*/
            /*
            ImageWatermark imgWatermark = new ImageWatermark(@"D:\a\sauce.png");
            imgWatermark.ContentAlignment = System.Drawing.ContentAlignment.BottomRight;//水印位置
            imgWatermark.Alpha = 100;//透明度，需要水印图片是背景透明的png图片
            ImageProcessingJob jobNormal = new ImageProcessingJob();
            jobNormal.Filters.Add(imgWatermark);//添加水印
            jobNormal.Filters.Add(new FixedResizeConstraint(300, 300));//限制图片的大小，避免生成大图。如果想原图大小处理，就不用加这个Filter
            jobNormal.SaveProcessedImageToFileSystem(@"D:\a\Tulips.jpg", @"D:\a\2.png");
            */

            /*
            using (MemoryStream ms =
                ImageFactory.GenerateImage("a123fdasfa32", 80, 150,
                30, 10))
            using (FileStream fs = File.OpenWrite(@"d:\1.jpg"))
            {
                ms.CopyTo(fs);
            }*/
            //不要忘了，从App.config/Web.config中加载log4net的配置

            log4net.Config.XmlConfigurator.Configure();
            /*
            log4net.Config.XmlConfigurator.Configure();
            ILog log = LogManager.GetLogger(typeof(Prog));
            log.Debug("飞行高度10000米");
            log.Warn("油压不足");
            log.Error("引擎失灵");*/
            //log.DebugFormat("当前数据有问题,{0}", "aaaaaaaa");
            //log.Debug("当前数据有问题,"+ "aaaaaaaa");
            /*
            try
            {
                SqlConnection conn = new SqlConnection();
                conn.Open();
            }
           catch(Exception ex)
            {
                log.Error("连接数据库失败",ex);
            }*/
            /*
            IScheduler sched = new StdSchedulerFactory().GetScheduler();

            { 
                JobDetailImpl jdBossReport = new JobDetailImpl("jdTest", typeof(TestJob));
                var builder = CalendarIntervalScheduleBuilder.Create();
                builder.WithInterval(3, IntervalUnit.Second);
                IMutableTrigger triggerBossReport = builder.Build();
                //IMutableTrigger triggerBossReport =
                //CronScheduleBuilder.DailyAtHourAndMinute(21, 11).Build();//每天23:45执行一次
                triggerBossReport.Key = new TriggerKey("triggerTest");
                sched.ScheduleJob(jdBossReport, triggerBossReport);
            }
            {
                JobDetailImpl jdBossReport = new JobDetailImpl("jdTest", typeof(TwoJob));
                var builder = CalendarIntervalScheduleBuilder.Create();
                builder.WithInterval(8, IntervalUnit.Second);
                IMutableTrigger triggerBossReport = builder.Build();
                //IMutableTrigger triggerBossReport =
                //CronScheduleBuilder.DailyAtHourAndMinute(21, 11).Build();//每天23:45执行一次
                triggerBossReport.Key = new TriggerKey("triggerTest");
                sched.ScheduleJob(jdBossReport, triggerBossReport);
            }


            sched.Start();

    */
            //UserBll bll = new UserBll();
            /*
            IUserBll bll = new UserBll();
            bll.AddNew("aa", "123");*/
            ContainerBuilder builder = new ContainerBuilder();
            //把UserBll注册为IUserBll实现类
            //builder.RegisterType<UserBll>().As<IUserBll>();
            //builder.RegisterType<UserBll>().AsImplementedInterfaces();
            //builder.RegisterType<DogBll>().AsImplementedInterfaces();
            Assembly asm = Assembly.Load("MyBllImpl");
            //builder.RegisterAssemblyTypes(asm).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(asm).AsImplementedInterfaces()
                .PropertiesAutowired().SingleInstance();

            IContainer container = builder.Build();

            //创建IUserBll实现类的对象
            /*
            IUserBll bll = container.Resolve<IUserBll>();//new UserBll 
            Console.WriteLine(bll.GetType());
            bll.AddNew("aa", "23");*/
            /*
            IEnumerable<IUserBll>  blls = container.Resolve<IEnumerable<IUserBll>>();
            foreach(IUserBll userBll in blls)
            {
                Console.WriteLine(userBll.GetType());
                userBll.AddNew("aaa", "33");
            }

            IDogBll dogBll = container.Resolve<IDogBll>();
            dogBll.Bark();
            */
            /*
            ISchool s = container.Resolve<ISchool>();
            s.FangXue();

            // ISchool s2 = new School();
            ISchool s1 = container.Resolve<ISchool>();
            Console.WriteLine(object.ReferenceEquals(s, s1));
            */
            Console.WriteLine("OK");
            Console.ReadKey();

        }
    }
}
