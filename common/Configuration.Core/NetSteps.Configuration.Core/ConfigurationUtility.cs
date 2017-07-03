using System;
using System.Configuration;
using System.Reflection;
using System.Text;

namespace NetSteps.Configuration
{
	public static class ConfigurationUtility
	{
		public static T GetSection<T>()
			where T : ConfigurationSection, new()
		{
			return GetSection(typeof(T)) as T;
		}

		public static ConfigurationElement GetSection(Type type)
		{
			ConfigurationElement result = null;
			string[] segments = type.FullName.Split('.');
			if(segments != null && segments.Length > 0)
			{
				StringBuilder path = new StringBuilder();
				for(int index = 0; index < segments.Length; index++)
				{
					if(index < segments.Length - 1)
					{
						string segment = segments[index];
						if (Char.IsUpper(segment[0]))
						{
							segment = String.Concat(Char.ToLower(segment[0]), segment.Substring(1));
						}
						if(!String.Equals(segment, "Configuration", StringComparison.InvariantCultureIgnoreCase))
						{
							path.Append('/').Append(segment);
						}
					}
					else
					{
						FieldInfo member = type.GetField("SectionName", BindingFlags.Public | BindingFlags.Static);
						if (member != null)
						{
							path.Append('/').Append(member.GetValue(null));
						}
						else
						{
							path.Append('/');
							string name = type.Name;
							if(name.EndsWith("ConfigSection"))
							{
								name = name.Remove(name.Length - 12);
							}
							else if (name.EndsWith("ConfigurationSection"))
							{
								name = name.Remove(name.Length - 20);
							}
							if(Char.IsUpper(name[0]))
							{
								name = String.Concat(Char.ToLower(name[0]), name.Substring(1));
							}
							path.Append(name);
						}
					}
				}
				//Log.Information("Resolving configuration for type '{0}' using '{1}'", type.Name, path.ToString());
				result = ConfigurationManager.GetSection(path.ToString().TrimStart('/')) as ConfigurationElement;
				if(result == null)
				{
					//Log.Information("No declared configuration could be found for type '{0}' using '{1}' attempting to use default configuration", type.Name, path.ToString());
					result = Activator.CreateInstance(type) as ConfigurationElement;
				}
			}
			return result;
		}
	}
}
