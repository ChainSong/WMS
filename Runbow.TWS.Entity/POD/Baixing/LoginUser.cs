using System;
using Runbow.TWS.Common;

namespace Runbow.TWS.Entity.POD
{
    /// <summary>
    /// webapi接口用户验签
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// 编号
        /// </summary>
        [EntityPropertyExtension("Id", "Id")]
        public long Id { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [EntityPropertyExtension("LoginName", "LoginName")]
        public string LoginName { get; set; }

        /// <summary>
        /// 验签
        /// </summary>
        [EntityPropertyExtension("access_token", "access_token")]
        public string access_token { get; set; }
    }
}
