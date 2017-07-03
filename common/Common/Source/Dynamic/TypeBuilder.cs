using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using NetSteps.Common.Attributes;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Dynamic
{
	public class TypeBuilder
	{
		private static ConcurrentDictionary<string, Type> _generated = new ConcurrentDictionary<string, Type>();
		private static AssemblyBuilder _assemblyBuilder;
		private static ModuleBuilder _module;

		public static Type GenerateType(string name, Dictionary<string, Type> properties, Type baseType = null, IEnumerable<Type> interfaces = null, bool checkProperties = false, bool addTermNameAttribute = false, bool addDisplayAttribute = false)
		{
			// Intern the name so it is safe to use as both a key and a lock...
			var internedName = String.Intern(name);
			var createdOnThisCall = false;
			Type generated = _generated.GetOrAdd(internedName, (n) => 
				{
					createdOnThisCall = true;
					return PerformGenerateType(n, properties, baseType, interfaces, checkProperties, addTermNameAttribute, addDisplayAttribute);
				});
						
			if (!createdOnThisCall && checkProperties)
			{
				var typeProperties = generated.GetPropertiesCached();
				foreach (var property in properties)
				{
					if (!typeProperties.Any(p => p.Name == property.Key && p.PropertyType == property.Value))
						throw new InvalidOperationException(string.Format("The type '{0}' does not contain the property '{1}'", name, property.Key));
				}
			}
			return generated;
		}

		private static Type PerformGenerateType(string name, Dictionary<string, Type> properties, Type baseType, IEnumerable<Type> interfaces, bool checkProperties, bool addTermNameAttribute, bool addDisplayAttribute)
		{
			if (_assemblyBuilder == null)
			{
				AssemblyName assemblyName = new AssemblyName();
				assemblyName.Name = "NetSteps.Dynamic";
				_assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
			}
			if (_module == null)
			{
				_module = _assemblyBuilder.DefineDynamicModule("NetStepsDynamicModule");
			}

			System.Reflection.Emit.TypeBuilder typeBuilder = baseType == null ? _module.DefineType(name, TypeAttributes.Public | TypeAttributes.Class) : _module.DefineType(name, TypeAttributes.Public | TypeAttributes.Class, baseType);
			if (interfaces != null)
			{
				foreach (Type interfaceType in interfaces)
				{
					typeBuilder.AddInterfaceImplementation(interfaceType);
				}
			}

			foreach (var property in properties)
			{
				var propertyName = property.Key;

				FieldBuilder field = typeBuilder.DefineField("_" + propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1), property.Value, FieldAttributes.Private);

				PropertyBuilder prop = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, property.Value, new Type[] { property.Value });

				MethodAttributes getAndSetAttributes = MethodAttributes.Public | MethodAttributes.HideBySig;

				MethodBuilder get = typeBuilder.DefineMethod("get_" + propertyName, getAndSetAttributes, property.Value, Type.EmptyTypes);
				ILGenerator currGetIL = get.GetILGenerator();
				currGetIL.Emit(OpCodes.Ldarg_0);
				currGetIL.Emit(OpCodes.Ldfld, field);
				currGetIL.Emit(OpCodes.Ret);

				MethodBuilder set = typeBuilder.DefineMethod("set_" + propertyName, getAndSetAttributes, null, new Type[] { property.Value });
				ILGenerator currSetIL = set.GetILGenerator();
				currSetIL.Emit(OpCodes.Ldarg_0);
				currSetIL.Emit(OpCodes.Ldarg_1);
				currSetIL.Emit(OpCodes.Stfld, field);
				currSetIL.Emit(OpCodes.Ret);

				prop.SetGetMethod(get);
				prop.SetSetMethod(set);

				if (addTermNameAttribute)
				{
					// Add attributes to the get & set methods - JHE
					Type[] termNameParams = new Type[] { typeof(string), typeof(string) };
					ConstructorInfo termNameCtorInfo = typeof(TermNameAttribute).GetConstructor(termNameParams);
					CustomAttributeBuilder termNameCABuilder = new CustomAttributeBuilder(termNameCtorInfo, new object[] { propertyName, propertyName.PascalToSpaced() });
					prop.SetCustomAttribute(termNameCABuilder);
				}

				if (addDisplayAttribute)
				{
					Type[] displayParams = new Type[] { };
					ConstructorInfo displayCtorInfo = typeof(DisplayAttribute).GetConstructor(displayParams);
					var propertyInfo = typeof(DisplayAttribute).GetPropertyCached("Name");
					CustomAttributeBuilder displayCABuilder = new CustomAttributeBuilder(displayCtorInfo, new object[] { }, new PropertyInfo[] { propertyInfo }, new object[] { propertyName.PascalToSpaced() });
					prop.SetCustomAttribute(displayCABuilder);
				}
			}

			return typeBuilder.CreateType();
		}
	}
}
