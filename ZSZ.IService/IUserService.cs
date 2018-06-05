using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZSZ.DTO;

namespace ZSZ.IService
{
    public interface IUserService:IServiceSupport
    {
        long AddNew(String phoneNum, String password);
        UserDTO GetById(long id);
        UserDTO GetByPhoneNum(String phoneNum);

        ////检查用户名密码是否正确（很好体现了分层的思想）
        bool CheckLogin(String phoneNum, String password);
        void UpdatePwd(long userId, String newPassword);

        //设置用户userId的城市id
        void SetUserCityId(long userId, long cityId);
        /// <summary>
        /// 记录一次登录失败
        /// </summary>
        /// <param name="id"></param>
        void IncrLoginError(long id);

        /// <summary>
        /// 充值登录失败信息
        /// </summary>
        /// <param name="id"></param>
        void ResetLoginError(long id);

        /// <summary>
        /// 判断用户是否已经被锁定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsLocked(long id);
    }

}
