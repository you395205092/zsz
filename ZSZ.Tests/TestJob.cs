using Common.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Tests
{
    class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
           
            try
            { 

            Console.WriteLine("任务执行啦"+DateTime.Now);
            SqlConnection conn = new SqlConnection();
            conn.Open();
            Console.WriteLine("执行完成");
            }
            catch(Exception ex)
            {
                Console.WriteLine("定时任务执行出错"+ex);
                //ILog log = LogManager.GetLogger(typeof(TestJob));
                //log.Error("定时任务执行出错",ex);
            }
        }
    }
}
