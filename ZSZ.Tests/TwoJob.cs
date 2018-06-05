using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZSZ.Tests
{
    class TwoJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("我是22222定时执行");
        }
    }
}
