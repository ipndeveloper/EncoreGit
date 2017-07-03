using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Conversion.Core
{
    internal static class ClassDataManager
    {
        private static readonly Dictionary<string, ClassData> ClassDataDictionary;

        static ClassDataManager()
        {
            ClassDataDictionary = new Dictionary<string, ClassData>();
        }

        internal static ClassData GetClassData(Type type)
        {
            if (!ClassDataDictionary.ContainsKey(type.FullName))
            {
                var newClass = new ClassData();
                newClass.TypeName = type.FullName;
                foreach (
                    var property in
                        type.GetProperties()
                            .Where(x => IsProperty(x.PropertyType)))
                {
                    newClass.PropertyAccessors.Add(property.Name, new PropertyData()
                    {
                        PropertyType = property.PropertyType,
                        PropertyValue = (obj) => property.GetValue(obj)
                        });
                }

                foreach (
                    var property in
                        type.GetProperties()
                            .Where(
                                x =>
                                x.PropertyType.IsGenericType && typeof (IEnumerable).IsAssignableFrom(x.PropertyType)))
                {
                    newClass.CollectionAccessors.Add(property.Name, new PropertyData()
                    {
                        PropertyType = property.PropertyType,
                        PropertyValue = (obj) => property.GetValue(obj)
                    });
                }
                ClassDataDictionary.Add(type.FullName, newClass);
            }
            return ClassDataDictionary[type.FullName];
        }

        private static bool IsProperty(Type fieldType)
        {
            if (fieldType.IsValueType)
            {
                return true;
            }
            else if (typeof (string).IsAssignableFrom(fieldType))
            {
                return true;
            }
            else if (typeof (byte[]).IsAssignableFrom(fieldType))
            {
                return true;
            }
            else if (typeof(Nullable).IsAssignableFrom(fieldType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
