using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IIdNameService:IServiceSupport
    {
        //类别名，名字
        long AddNew(String typeName, String name);
        IdNameDTO GetById(long id);

        //获取类别下的IdName（比如所有的民族）
        IdNameDTO[] GetAll(String typeName);
    }

}
