using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NetSteps.Common.Validation
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Helper methods to work with System.ComponentModel.DataAnnotations
    /// Created: 03-05-2010
    /// </summary>
    public class DataAnnotationHelpers
    {
        private static Dictionary<Type, Dictionary<PropertyDescriptor, List<ValidationAttribute>>> _classesWithDataAnnotationAttributes = new Dictionary<Type, Dictionary<PropertyDescriptor, List<ValidationAttribute>>>();

        public static bool DataAnnotationAttributes(object obj)
        {
            if (_classesWithDataAnnotationAttributes.ContainsKey(obj.GetType()))
                return true;

            // TODO: Finish this (add code to log types that were checked and contain no attributes) - JHE

            return GetPropertiesWithDataAnnotationAttributes(obj).Count > 0;
        }

        /// <summary>
        /// http://www.davidmuto.com/Blog.muto/View/validation-with-dataannotations
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static Dictionary<PropertyDescriptor, List<ValidationAttribute>> GetPropertiesWithDataAnnotationAttributes(object instance)
        {
            Type type = instance.GetType();
            if (_classesWithDataAnnotationAttributes.ContainsKey(type))
                return _classesWithDataAnnotationAttributes[type];

            var metadataAttrib = instance.GetType().GetCustomAttributes(typeof(MetadataTypeAttribute), true).OfType<MetadataTypeAttribute>().FirstOrDefault();
            var typeObject = metadataAttrib != null ? metadataAttrib.MetadataClassType : instance.GetType();
            var typeProperties = TypeDescriptor.GetProperties(typeObject).Cast<PropertyDescriptor>();
            var classProperties = TypeDescriptor.GetProperties(instance.GetType()).Cast<PropertyDescriptor>();

            Dictionary<PropertyDescriptor, List<ValidationAttribute>> returnValue = new Dictionary<PropertyDescriptor, List<ValidationAttribute>>();
            var list = from property in typeProperties
                       join modelProp in classProperties on property.Name equals modelProp.Name
                       from attribute in property.Attributes.OfType<ValidationAttribute>()
                       select new { Property = modelProp, Attrib = attribute };

            foreach (var item in list)
            {
                if (!returnValue.ContainsKey(item.Property))
                    returnValue.Add(item.Property, new List<ValidationAttribute>());
                returnValue[item.Property].Add(item.Attrib);
            }

            if (returnValue.Count > 0 && !_classesWithDataAnnotationAttributes.ContainsKey(type))
                _classesWithDataAnnotationAttributes.Add(type, returnValue);

            return returnValue;
        }
    }
}