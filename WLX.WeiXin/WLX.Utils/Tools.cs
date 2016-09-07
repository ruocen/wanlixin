using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WLX.Utils
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class Tools
    {
        public static string LogNet = "";
        public static string ExpNet = "";//关于系统日记,

        #region 事件记录函数
        /// <summary>记录事件(记事本)
        /// 
        /// </summary>
        /// <param name="str"></param>
        public static void MessBox(string str)
        {
            MessBox(str, "");
        }


        /// <summary>记录事件
        /// 该函数按天记录事件.用于一些轻量级的日记系统.
        /// 可以记录系统异常,程序异常.如果Paths为空的话,默认会在应用程序的根目录建立一个log的文件夹,并在文件夹中写入日记.
        /// </summary>
        /// <param name="str">事件描述</param>
        /// <param name="path">保存路径,允许为空"" (不建议)</param>
        public static void MessBox(string str, string paths)
        {
            try
            {
                string filename = DateTime.Now.ToString("yyyy-MM-dd", DateTimeFormatInfo.InvariantInfo) + ".txt";
                string directory = AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
                if (paths != "")
                {
                    directory = AppDomain.CurrentDomain.BaseDirectory + paths;
                }

                string path = directory + filename;
                StreamWriter sr;
                //是否存在文件夹,不存在则创建
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                //追加日志
                if (File.Exists(path))
                {
                    sr = File.AppendText(path);
                }
                //创建日志
                else
                {
                    sr = File.CreateText(path);
                }
                string logMessage = string.Format("时间:{0};描述:{1}", DateTime.Now, str);
                sr.WriteLine(logMessage);//by wangyun 日志添加时间
                //sr.WriteLine(str);
                sr.Write("\r\n");
                sr.Close();
            }
            catch
            { }

        }



        #endregion


        /// <summary>得到用户浏览器类型
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetBrowse()
        {
            return System.Web.HttpContext.Current.Request.Browser.Type;
        }

        /// <summary> 获取浏览器端操作系统名称
        ///
        /// </summary>
        /// <returns></returns>
        public static string GetOSName()
        {
            string osVersion = System.Web.HttpContext.Current.Request.Browser.Platform;
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;

            if (userAgent.Contains("NT 6.3"))
            {
                osVersion = "Windows8.1";
            }
            else if (userAgent.Contains("NT 6.2"))
            {
                osVersion = "Windows8";
            }
            else if (userAgent.Contains("NT 6.1"))
            {
                osVersion = "Windows7";
            }
            else if (userAgent.Contains("NT 6.0"))
            {
                osVersion = "WindowsVista";
            }
            else if (userAgent.Contains("NT 5.2"))
            {
                osVersion = "WindowsServer2003";
            }
            else if (userAgent.Contains("NT 5.1"))
            {
                osVersion = "WindowsXP";
            }
            else if (userAgent.Contains("NT 5"))
            {
                osVersion = "Windows2000";
            }
            else if (userAgent.Contains("NT 4"))
            {
                osVersion = "WindowsNT4.0";
            }
            else if (userAgent.Contains("Me"))
            {
                osVersion = "WindowsMe";
            }
            else if (userAgent.Contains("98"))
            {
                osVersion = "Windows98";
            }
            else if (userAgent.Contains("95"))
            {
                osVersion = "Windows95";
            }
            else if (userAgent.Contains("Mac"))
            {
                osVersion = "Mac";
            }
            else if (userAgent.Contains("Unix"))
            {
                osVersion = "UNIX";
            }
            else if (userAgent.Contains("Linux"))
            {
                osVersion = "Linux";
            }
            else if (userAgent.Contains("SunOS"))
            {
                osVersion = "SunOS";
            }
            return osVersion;
        }


        #region 辅助方法

        /// <summary>校验码是否正确
        /// 
        /// </summary>
        /// <param name="parm">参数</param>
        /// <param name="DataKey">传过来的KEY</param>
        /// <param name="_Key">私有密钥</param>
        /// <returns></returns>
        public bool IsKey(List<string> parm, string DataKey, string _Key)
        {
            if (Encryption(parm, _Key) == DataKey)
            {
                return true;
            }
            return false;
        }


        /// <summary>计算密文
        /// 
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public static string Encryption(List<string> parm, string _Key)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in parm)
            {
                sb.Append(item + ",");

            }

            // Tools.MessBox(string.Format("明文:{0},密文:{1}", sb.ToString() + _Key, CreateMD5Key(sb.ToString() + _Key)));
            return CreateMD5Key(sb.ToString() + _Key);

        }

        public static string CreateMD5Key(string Password)
        {
            byte[] pwd = Encoding.Default.GetBytes(Password);
            string md5Pwd = String.Empty;
            MD5 md5 = new MD5CryptoServiceProvider();
            md5Pwd = BitConverter.ToString(md5.ComputeHash(pwd));
            md5Pwd = md5Pwd.Replace("-", "");
            return md5Pwd;
        }
        #endregion

        #region 随机数
        /// <summary>生成随机数</summary>
        /// <param name="num">产生的位数</param>
        /// <param name="state">产生随机数的类型（1：数字；2：大写，3：小写，4：数字+大写，5：数字+小写，6：小写+大写,7:中文,8:数字+大写+小写）</param>
        /// <returns></returns>
        public static string RandomInStrLower(int num, int state)
        {
            string Int = "1234567890";
            string Str = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string str = Str.ToLower();
            switch (state)
            {
                case 1://数字
                    return RandomStr(Int, num);
                case 2://大写
                    return RandomStr(Str, num);
                case 3://小写
                    return RandomStr(str, num);
                case 4://数字+大写
                    return RandomStr(Int + Str, num);
                case 5://数字+小写
                    return RandomStr(Int + str, num);
                case 6://小写+大写
                    return RandomStr(Str + str, num);
                case 7:
                    return CreateRegionCode(num);
                case 8: return RandomStr(Int + Str + str, num);

                default:
                    return RandomStr(Int + Str + str, num);
            }
        }
        private static long tick = DateTime.Now.Ticks;
        /// <summary>根据当前时间刻度产生随机数</summary>
        /// <param name="str">需要产生字符串原始数据</param>
        /// <param name="num">产生的个数</param>
        /// <returns></returns>
        public static string RandomStr(string str, int num)
        {
            if (1 > num) { return ""; }
            string newstr = "";

            Random rnd = new Random((int)tick++);
            for (int i = 0; i < num; i++)
            {

                newstr += str[rnd.Next(0, str.Length)];
            }
            return newstr.ToString();
        }
        /// <summary>生成随机数</summary>
        /// <param name="num">产生的位数</param>
        /// <param name="state">产生随机数的类型（1：数字；2：大写，3：小写，4：数字+大写，5：数字+小写，6：小写+大写,7:中文,8:数字+大写+小写）</param>
        /// <returns></returns>
        public static string CreateRandom(int num, int state)
        {
            return RandomInStrLower(num, state);
        }

        /// <summary>生成验证码字符串
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CreateChekCodeString(int strlength)
        {
            char[] allCharArray ={'0','1','2','3','4','5','6','7','8','9','A','B',
                'C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q',
            'R','S','T','U','V','W','X','Y','Z'};
            //定义验证码字符串
            string randomCode = "";
            Random rand = new Random();
            //生成4位验证码字符串
            for (int i = 0; i < strlength; i++)
                randomCode += allCharArray[rand.Next(allCharArray.Length)];
            return randomCode;
        }
        /// <summary>生成中文随机文字
        /// 
        /// </summary>
        /// <param name="strlength">长度多少</param>
        /// <returns></returns>
        public static string CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random();

            //定义一个object数组用来
            object[] bytes = new object[strlength];

            /**/
            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中
         每个汉字有四个区位码组成
         区位码第1位和区位码第2位作为字节数组第一个元素
         区位码第3位和区位码第4位作为字节数组第二个元素
        */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的种子避免产生重复值


                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中
                bytes.SetValue(str_r, i);

            }

            //      return bytes;
            StringBuilder sb = new StringBuilder();
            Encoding gb = Encoding.GetEncoding("gb2312");
            for (int i = 0; i < strlength; i++)
            {
                sb.Append(gb.GetString((byte[])Convert.ChangeType(bytes[i], typeof(byte[]))));
            }

            return sb.ToString();

        }

        #endregion
    }
}
