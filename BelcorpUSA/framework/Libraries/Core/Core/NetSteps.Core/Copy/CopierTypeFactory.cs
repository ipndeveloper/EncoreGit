using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading;

using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Reflection.Emit;

namespace NetSteps.Encore.Core
{
	internal static class CopierTypeFactory
	{
		static readonly Lazy<EmittedModule> __module = new Lazy<EmittedModule>(() =>
		{ return RuntimeAssemblies.DynamicAssembly.DefineModule("Copiers", null); },
			LazyThreadSafetyMode.ExecutionAndPublication
			);

		static EmittedModule GeneratedModule { get { return __module.Value; } }

		static readonly MethodInfo GenericCreateType = typeof(CopierTypeFactory).GetGenericMethod("ConcreteType", BindingFlags.Static | BindingFlags.NonPublic, 0, 2);

		internal static Type ConcreteType(Type sourceType, Type targetType)
		{
			Contract.Requires<ArgumentNullException>(sourceType != null);
			Contract.Requires<ArgumentNullException>(targetType != null);
			Contract.Ensures(Contract.Result<Type>() != null);

			var method = GenericCreateType.MakeGenericMethod(sourceType, targetType);
			return (Type)method.Invoke(null, null);
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		internal static Type ConcreteType<S, T>()
		{
			Contract.Ensures(Contract.Result<Type>() != null);

			var targetType = typeof(ICopier<S, T>);
			string typeName = RuntimeAssemblies.PrepareTypeName(targetType, "Copier");

			lock (targetType.GetLockForType())
			{
				var module = GeneratedModule;
				Type type = module.Builder.GetType(typeName, false, false);
				if (type == null)
				{
					type = BuildCopierType<S, T>(module, typeName);
				}
				return type;
			}
		}

		[SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "By design.")]
		static Type BuildCopierType<S, T>(EmittedModule module, string typeName)
		{
			Contract.Requires(module != null);
			Contract.Requires(typeName != null);
			Contract.Requires(typeName.Length > 0);
			Contract.Ensures(Contract.Result<Type>() != null);

			var builder = module.DefineClass(typeName, EmittedClass.DefaultTypeAttributes, typeof(Copier<S, T>), null);
			builder.Attributes = TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.BeforeFieldInit;

			ImplementPerformLooseCopy<S, T>(builder);
			ImplementPerformStrictCopy<S, T>(builder);

			builder.Compile();
			return builder.Ref.Target;
		}

		static void ImplementPerformStrictCopy<S, T>(EmittedClass builder)
		{
			var baseMethod = typeof(Copier<S, T>).GetMethod("PerformStrictCopy", BindingFlags.NonPublic | BindingFlags.Instance);
			var method = builder.DefineOverrideMethod(baseMethod);

			method.ContributeInstructions((m, il) =>
			{
				il.Nop();
				var props = from src in typeof(S).GetReadablePropertiesFromHierarchy(BindingFlags.Instance | BindingFlags.Public)
										select new
										{
											Source = src,
											Destination = typeof(T).GetWritablePropertyWithAssignmentCompatablityFromHierarchy(src.Name, BindingFlags.Instance | BindingFlags.Public, src.PropertyType)
										};
				if (props.FirstOrDefault(p => p.Destination == null) != null)
				{
					il.Nop();
					il.LoadValue("Cannot perfrom a strict copy because the source and target do not have all the properties in common.");
					il.New<InvalidOperationException>(typeof(string));
					il.Throw();
				}
				else
				{
					foreach (var p in props)
					{
						//
						// result.<property name> = src.<property name>;
						//
						il.LoadArg_1();
						il.LoadArg_2();
						il.CallVirtual(p.Source.GetGetMethod());
						il.CallVirtual(p.Destination.GetSetMethod());
					}
				}
			});
		}


		static void ImplementPerformLooseCopy<S, T>(EmittedClass builder)
		{
			var baseMethod = typeof(Copier<S, T>).GetMethod("PerformLooseCopy", BindingFlags.NonPublic | BindingFlags.Instance);
			var method = builder.DefineOverrideMethod(baseMethod);

			method.ContributeInstructions((m, il) =>
			{
				il.Nop();

				foreach (var prop in from src in typeof(S).GetReadablePropertiesFromHierarchy(BindingFlags.Instance | BindingFlags.Public)
														 join dest in typeof(T).GetWritablePropertiesFromHierarchy(BindingFlags.Instance | BindingFlags.Public)
														 on src.Name equals dest.Name
														 select new
														 {
															 Source = src,
															 Destination = dest
														 })
				{
					if (prop.Destination.PropertyType.IsAssignableFrom(prop.Source.PropertyType))
					{
						//
						// result.<property-name> = src.<property-name>;
						//
						il.LoadArg_1();
						il.LoadArg_2();
						il.CallVirtual(prop.Source.GetGetMethod());
						il.CallVirtual(prop.Destination.GetSetMethod());
						il.Nop();
					}
					//else if (!prop.Destination.PropertyType.IsValueType
					//  && !prop.Source.PropertyType.IsValueType)
					//{
					//  //
					//  // <source-property-type> temp_<n>;
					//  // if (temp_<n> != null)
					//  // {
					//  //   target.<property-name> = Factory<destination-property-type>
					//  //     .Copier<destination-property-type>
					//  //     .LooseCopy<source-property-type>(temp_<n>);
					//  // }
					//  //
					//  var copierType = typeof(ICopier<,>).MakeGenericType(prop.Source.PropertyType, prop.Destination.PropertyType);
					//  var localCopier = il.DeclareLocal(copierType);
					//  il.LoadArg_3();
					//  il.Call(typeof(IContainerExtensions).GetGenericMethod("New", 1, 1).MakeGenericMethod(copierType));
					//  il.StoreLocal(localCopier);
					//  il.LoadArg_1();
					//  il.LoadLocal(localCopier);
					//  il.LoadArg_3();
					//  il.LoadArg_2();
					//  il.CallVirtual(prop.Source.GetGetMethod());
					//  il.LoadValue(CopyKind.Loose);
					//  il.Call(typeof(ICopierExtensions).GetGenericMethod("Copy", 4, 2).MakeGenericMethod(prop.Source.PropertyType, prop.Destination.PropertyType));
					//  il.CallVirtual(prop.Destination.GetSetMethod());
					//  il.Nop();
					//}
				}
			});
		}
	}

}
