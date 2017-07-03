using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Reflection
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Method to copy value properties from 1 object to another using reflection and dynamic code.
	/// This is about 10X faster than the reflection only method when more that 1 call for specified
	/// same types is called.
	/// Taken from: http://weblogs.asp.net/gunnarpeipman/archive/2010/02/03/performance-using-dynamic-code-to-copy-property-values-of-two-objects.aspx
	/// Created: 03-01-2010
	/// </summary>
	public class CopyProperties
	{
		#region Members
		private static readonly ConcurrentDictionary<string, PropertyMap[]> _maps = new ConcurrentDictionary<string, PropertyMap[]>();
		private static readonly ConcurrentDictionary<string, Type> _comp = new ConcurrentDictionary<string, Type>();
		#endregion

		#region Helper Class
		public class PropertyMap
		{
			public PropertyInfo SourceProperty { get; set; }
			public PropertyInfo TargetProperty { get; set; }
		}
		#endregion

		#region Helper Methods
		public static IList<PropertyMap> GetMatchingProperties(Type sourceType, Type targetType)
		{
			var sourceProperties = sourceType.GetPropertiesCached();
			var targetProperties = targetType.GetPropertiesCached();

			var properties = (from s in sourceProperties
							  from t in targetProperties
							  where s.Name == t.Name &&
									s.CanRead &&
									t.CanWrite &&
									s.PropertyType.IsPublic &&
									t.PropertyType.IsPublic &&
									s.PropertyType == t.PropertyType &&
									(
									  (s.PropertyType.IsValueType && t.PropertyType.IsValueType) ||
									  (s.PropertyType == typeof(string) && t.PropertyType == typeof(string))
									)
							  select new PropertyMap
							  {
								  SourceProperty = s,
								  TargetProperty = t
							  }).ToList();
			return properties;
		}

		private static void AddPropertyMap<T, TU>()
		{
			var props = GetMatchingProperties(typeof(T), typeof(TU));
			var className = GetClassName(typeof(T), typeof(TU));
			_maps.Add(className, props.ToArray());
		}

		public static void CopyMatchingCachedProperties(object source, object target)
		{
			var className = GetClassName(source.GetType(), target.GetType());
			var propMap = _maps[className];

			for (var i = 0; i < propMap.Length; i++)
			{
				var prop = propMap[i];
				var sourceValue = prop.SourceProperty.GetValue(source, null);
				prop.TargetProperty.SetValue(target, sourceValue, null);
			}
		}

		private static string GetClassName(Type sourceType, Type targetType)
		{
			var className = "Copy_";
			className += sourceType.FullName.Replace(".", "_");
			className += "_";
			className += targetType.FullName.Replace(".", "_");
			return className;
		}

		private static void GenerateCopyClass<T, TU>()
		{
			var sourceType = typeof(T);
			var targetType = typeof(TU);
			var className = GetClassName(sourceType, targetType);

			_comp.GetOrAdd(className, 
                key =>
                {
                    var builder = new StringBuilder();
                    builder.Append("namespace Copy {\r\n");
                    builder.Append("    public class ");
                    builder.Append(className);
                    builder.Append(" {\r\n");
                    builder.Append("        public static void CopyProps(");
                    builder.Append(sourceType.FullName);
                    builder.Append(" source, ");
                    builder.Append(targetType.FullName);
                    builder.Append(" target) {\r\n");

                    var map = GetMatchingProperties(sourceType, targetType);

                    foreach (var item in map)
                    {
                        builder.Append("            target.");
                        builder.Append(item.TargetProperty.Name);
                        builder.Append(" = ");
                        builder.Append("source.");
                        builder.Append(item.SourceProperty.Name);
                        builder.Append(";\r\n");
                    }

                    builder.Append("        }\r\n   }\r\n}");

                    // Write out method body
                    Debug.WriteLine(builder.ToString());

                    var myCodeProvider = new CSharpCodeProvider();
                    //var myCodeCompiler = myCodeProvider.CreateCompiler(); //This is obsolete, so I'm changing it to call the code provider directly - DES
                    var myCompilerParameters = new CompilerParameters();
                    myCompilerParameters.ReferencedAssemblies.Add(sourceType.Assembly.Location);
                    myCompilerParameters.ReferencedAssemblies.Add(targetType.Assembly.Location);
                    myCompilerParameters.ReferencedAssemblies.Add(typeof(System.ComponentModel.INotifyPropertyChanged).Assembly.Location);
                    myCompilerParameters.GenerateInMemory = true;

                    var results = myCodeProvider.CompileAssemblyFromSource(myCompilerParameters, builder.ToString());

                    //var results = myCodeCompiler.CompileAssemblyFromSource(myCompilerParameters, builder.ToString());

                    // Compiler output
                    foreach (var line in results.Output)
                        Debug.WriteLine(line);

                    return results.CompiledAssembly.GetType("Copy." + className); 
                });
		}
		#endregion

		#region Methods
		/// <summary>
		/// Method to copy value properties from 1 object to another using reflection and dynamic code.
		/// This is about 10X faster than the reflection only method when more that 1 call for specified
		/// same types is called. - JHE
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TU"></typeparam>
		/// <param name="source"></param>
		/// <param name="target"></param>
		public static void CopyWithDom<T, TU>(T source, TU target)
		{
			if (source == null || target == null)
				return;

			var className = GetClassName(typeof(T), typeof(TU));
			var flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod;
			var args = new object[] { source, target };

			GenerateCopyClass<T, TU>(); // Added to make sure the code for the types is already generated. - JHE
			_comp[className].InvokeMember("CopyProps", flags, null, null, args);
		}
		#endregion
	}
}
