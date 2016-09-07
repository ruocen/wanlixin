using System.Text.RegularExpressions;

namespace WLX.Utils.WebUtils
{
   public static class ValidationHelper
    {
       private const string number = @"^\d*$";

       /// <summary>
       /// 描述：验证数字（不包含小数点）
       /// </summary>
       /// <param name="input">输入字符串</param>
       /// <returns>成功：true;失败：false</returns>
       public static bool IsNumber(string input)
       {
           return Regex.IsMatch(input, number);
       }
    }
}
