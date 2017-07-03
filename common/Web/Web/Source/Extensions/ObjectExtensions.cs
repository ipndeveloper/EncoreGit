using System;
using System.Web.Script.Serialization;

namespace NetSteps.Web.Extensions
{
    public static class ObjectExtensions
    {
        private static JavaScriptSerializer jsSerializer;
        public static string ToJSON(this object obj)
        {
            if (jsSerializer == null)
                jsSerializer = new JavaScriptSerializer()
                {
                    MaxJsonLength = int.MaxValue
                };
            return jsSerializer.Serialize(obj);
        }

        public static string ToJSON<T>(this T obj, Func<T, object> select)
        {
            return select(obj).ToJSON();
        }
    }
}
