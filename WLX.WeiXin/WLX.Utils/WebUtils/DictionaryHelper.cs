using System.Collections.Generic;
using System.Collections.Specialized;

namespace WLX.Utils.WebUtils
{
    public class DictionaryHelper
    {

        public static Dictionary<string, string> NameValueCollection2StrDict(NameValueCollection collection)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            foreach (var item in collection.AllKeys)
            {
                dic[item] = collection[item];
            }

            return dic;
        }
    }
}
