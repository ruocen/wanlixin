using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WLX.Utils
{
    public class JsonHandler
    {
        public static JsonMessage CreateMessage(int pstatus, string pinfo, string purl)
        {
            JsonMessage json = new JsonMessage()
            {
                status = pstatus,
                info = pinfo,
                url = purl
            };
            return json;
        }
        public static JsonMessage CreateMessage(int pstatus, string pinfo)
        {
            JsonMessage json = new JsonMessage()
            {
                status = pstatus,
                info = pinfo
            };
            return json;
        }
        public class JsonMessage
        {
            public int status { get; set; }
            public string info { get; set; }
            public string url { get; set; }
        }
    }
}
