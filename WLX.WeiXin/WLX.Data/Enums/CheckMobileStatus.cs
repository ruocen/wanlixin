using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Data.Enums
{
    public enum CheckMobileStatus
    {
        成功发送验证码 = 0,

        这个手机已经绑定 = 1,

        验证间隔时间太短 = 2,

        手机已被其他账户绑定 = 3,

        错误 = 4
    }
}
