using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ZSZ.CommonMVC
{
    //一定要使用using System.Web.Mvc下的DefaultModelBinder
    public class TrimToDBCModelBinder : DefaultModelBinder
    {
        
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {            
            object value = base.BindModel(controllerContext, bindingContext);
            if(value is string)
            {
                string strValue = (string)value;
                string value2 = ToDBC(strValue).Trim();
                return value2;
            }
            else
            {
                return value;
            }
        }

        /// <summary> 全角转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        private static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }

    }
}
