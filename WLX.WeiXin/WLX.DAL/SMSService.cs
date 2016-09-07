using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using WLX.DAL.Models;
using WLX.Data;
using WLX.Data.DataBase;
using WLX.Data.Enums;
using WLX.Data.Models;
using WLX.Utils;
using WLX.Utils.WebUtils;

namespace WLX.DAL
{
    public class SMSService
    {

        CommDAL dal = new CommDAL();

        /// <summary>
        /// 获取手机验证吗，并发送短信通知。此方法有判断重复的手机号
        /// </summary>
        /// <param name="chanceInfo"></param>
        /// <returns></returns>
        public static CheckMobileStatus SendMobileCheckCode(UserCheckMobile checkInfo)
        {
            CheckMobileStatus result = CheckMobileStatus.成功发送验证码;

            bool haveChance = false;
            int interval = 2;//间隔几分钟

            Customer existCheckMobileUserInfo = UserService.GetUserInfoByCheckdMobile(checkInfo.Mobile);

            if (existCheckMobileUserInfo == null)
            {
                //CheckMobileService checkMobileService = new CheckMobileService();
                UserCheckMobile lastCheckInfo = CheckMobileService.GetLastCheckMobileInfo(checkInfo.Mobile);

                if (lastCheckInfo == null)
                {
                    //不存在相关记录，则可以发送
                    haveChance = true;
                }
                else
                {
                    //当前时间已经超过间隔时间
                    if (lastCheckInfo.UpdateTime.AddMinutes(interval) < DateTime.Now)
                    {
                        haveChance = true;
                    }
                    else
                    {
                        haveChance = false;
                        result = CheckMobileStatus.验证间隔时间太短;
                    }
                }

                //符合发放条件
                if (haveChance)
                {
                    CheckMobileService check = new CheckMobileService();
                    if (lastCheckInfo != null)
                    {
                        lastCheckInfo.Mobile = checkInfo.Mobile;
                        lastCheckInfo.CheckCode = checkInfo.CheckCode;
                        lastCheckInfo.GetCodeTimes = lastCheckInfo.GetCodeTimes + 1;
                        lastCheckInfo.UpdateTime = DateTime.Now;

                        check.UpdateCheckMobileInfo(lastCheckInfo);
                    }
                    else
                    {
                        check.InsertCheckMobileInfo(checkInfo);
                    }
                    //变更模板获取 和 短信接口引用,每天只能发送3条验证手机短信。防止恶意刷短信
                    //string msgContent = string.Format(SMSFormat.SMS_CHECKMOBILE, checkInfo.CheckCode);
                    bool send = SMS_SendOnly(checkInfo.Mobile, checkInfo.CheckCode,string.Empty, checkInfo.UserID, 3);

                    if (send)
                    {
                        result = CheckMobileStatus.成功发送验证码;
                    }
                    else
                    {
                        result = CheckMobileStatus.错误;
                    }
                }
            }
            else
            {
                if (existCheckMobileUserInfo.ID == checkInfo.UserID)
                {
                    result = CheckMobileStatus.这个手机已经绑定;
                }
                else
                {
                    result = CheckMobileStatus.手机已被其他账户绑定;
                }
            }

            return result;
        }
        ///   <summary>   
        ///   单发短信每天有限制条数
        ///   </summary>   
        ///   <param   name="MobileNo">手机号码</param>   
        ///   <param   name="SMScontent">发送内容</param>  
        ///   <param   name="SMStype">发送类型(1.注册确认;2.下单;3发货;4.收款;5.交易成功;6.取消订单;7.其他)</param> 
        ///   <param   name="SendTime">发送时间,空值为立即发送</param> 
        ///   <param   name="Userid">用户ID。如果没有指定为-1</param> 
        ///   <param   name="everyDayLimitNum">同一个手机号码每天限制发送条数</param> 
        private static bool SMS_SendOnly(string MobileNo, string SMScontent,string SendTime, string Userid, int everyDayLimitNum)
        {
            try
            {

                int count = GetSMSSendCountByDay(MobileNo);
                if (count >= everyDayLimitNum)
                {
                    //同一个手机号码达到短信发送数量限制
                    return false;
                }
                else return SMS_Send(MobileNo, SMScontent, SendTime, Userid);
            }
            catch
            {
                return false;
            }
        }
        ///   <summary>   
        ///   单发短信 
        ///   </summary>   
        ///   <param   name="MobileNo">手机号码</param>   
        ///   <param   name="SMScontent">发送内容</param>  
        ///   <param   name="SMStype">发送类型(1.注册确认;2.下单;3发货;4.收款;5.交易成功;6.取消订单;7.其他)</param> 
        ///   <param   name="SendTime">发送时间,空值为立即发送</param> 
        ///   <param   name="Userid">用户ID。如果没有指定为-1</param> 
        public static bool SMS_Send(string MobileNo, string SMScontent,string SendTime, string Userid)
        {
            try
            {
                if (string.IsNullOrEmpty(MobileNo))
                {
                    return true;
                }

                //验证码短信发送接口的http地址
                string URL_SEND_YZM = "https://api.dingdongcloud.com/v1/sms/sendyzm";
                //发送内容
                string content = "【万里信】尊敬的用户，你的验证码是：" + SMScontent + "，请在1分钟内输入。请勿告诉其他人。";

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("apikey={0}", "670003cc7c4b5c96a1143c0b1efedcec");//apikey
                sb.AppendFormat("&mobile={0}", HttpUtility.UrlEncode(MobileNo, Encoding.UTF8));//接收手机号集合
                sb.AppendFormat("&content={0}", HttpUtility.UrlEncode(content, Encoding.UTF8));//发送消息内容

                string result = HttpUtils.GetWebRequest(URL_SEND_YZM, sb.ToString(), "POST", Encoding.UTF8);

                SaveLog(Userid, SMScontent, MobileNo, result, SendTime);

                if (result.Contains("成功"))
                    return true;
                else
                    return false;
                
            }
            catch
            {
                return false;
            }
        }

        // 新增短信发送日志
        private static void SaveLog(string userId, string content, string mobileNo, string result, string sendTime)
        {
            try
            {
                using (EntityContext db = new EntityContext())
                {
                    SMSLog SMSloginfo = new SMSLog();
                    SMSloginfo.ID = CommonUtil.GetNewID();
                    SMSloginfo.UserId = userId;
                    SMSloginfo.Message = content;
                    SMSloginfo.SMSTYPE = 1;
                    SMSloginfo.MobilePhone = mobileNo;
                    SMSloginfo.ReturnStr = result;
                    if (!string.IsNullOrEmpty(sendTime)) SMSloginfo.SendTime = DateTime.Parse(sendTime.ToString());
                    else SMSloginfo.SendTime = DateTime.Now;
                    db.SMSLogs.Add(SMSloginfo);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Tools.MessBox(ex.ToString());
            }
        }

        private static int GetSMSSendCountByDay(string mobileNo)
        {
            SqlHelper hel = new SqlHelper();
            string sql = string.Format("select * from SMSLog s where MobilePhone=@MobilePhone And Convert(Varchar(10),SendTime,120)=@SendTime");
            SqlParameter[] prams = new SqlParameter[2];
            prams[0] = new SqlParameter("@MobilePhone", mobileNo);
            prams[1] = new SqlParameter("@SendTime", DateTime.Now.ToString("yyyy-MM-dd"));
            return hel.ExecuteNonQuery(sql, prams);
        }





    }
}
