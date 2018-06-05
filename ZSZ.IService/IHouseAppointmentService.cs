using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IHouseAppointmentService : IServiceSupport
    {
        //新增一个预约：userId用户id（可以为null，表示匿名用户）；name姓名、phoneNum手机号、houseId房间id、visiteDate预约看房时间
        long AddNew(long? userId, String name, String phoneNum, long houseId, DateTime visitDate);

        bool Follow(long adminUserId, long houseAppointmentId); //使用乐观锁解决并发的问题（这里先不实现，先抛个异常，后面再做）

        //根据id获取预约
        HouseAppointmentDTO GetById(long id);

        //得到cityId这个城市中状态为status的预约订单数
        long GetTotalCount(long cityId, String status);

        //分页获取数据
        //limit后面两个数不能用计算表达式，只能用固定的值，因此只能通过参数传递，计算在java中完成。
        HouseAppointmentDTO[] GetPagedData(long cityId, String status, int pageSize, int currentIndex);

    }

}
