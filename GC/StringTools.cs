using System;
using System.Linq;
using System.Web;

namespace GC.API
{
    public class StringTools
    {
        public static string GetQueryString(object obj)
        {
            var properties = obj.GetType().GetProperties()
                .Select(x => x.Name + "=" + HttpUtility.UrlEncode(x.GetValue(obj, null).ToString()));

            return string.Join("&", properties).Replace("+", "%20");
        }
    }
}
