
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
namespace NetSteps.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static object AddProperty(this object obj, string name, object value)
        {
            var dictionary = obj is IDictionary<string, object> ? (IDictionary<string, object>)obj : obj.ToDictionary();
            dictionary.Add(name, value);
            object newObj = dictionary;
            return newObj;
        }

        //helper
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            IDictionary<string, object> result = new Dictionary<string, object>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
            foreach (PropertyDescriptor property in properties)
            {
                result.Add(property.Name, property.GetValue(obj));
            }
            return result;
        }


    }
}
