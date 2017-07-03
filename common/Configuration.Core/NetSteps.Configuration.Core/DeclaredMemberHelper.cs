using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;

namespace NetSteps.Configuration
{
	internal static class DeclaredMemberHelper
	{
		private static readonly Type ConfigurationPropertyTypeReference = typeof(ConfigurationProperty);

		internal static void InitializeProperties(this ConfigurationElement configurationElement, ConfigurationPropertyCollection properties)
		{
			foreach (MemberInfo member in configurationElement.GetType().GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.Static))
			{
				if (member.MemberType == MemberTypes.Field)
				{
					FieldInfo field = (FieldInfo)member;
					if (field.FieldType == ConfigurationPropertyTypeReference) properties.Add((ConfigurationProperty)field.GetValue(configurationElement));
				}
				if (member.MemberType == MemberTypes.Property)
				{
					PropertyInfo property = (PropertyInfo)member;
					if (property.PropertyType == ConfigurationPropertyTypeReference) properties.Add((ConfigurationProperty)property.GetValue(configurationElement, null));
				}
			}
		}
	}
}
