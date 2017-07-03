using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Reflection.Emit;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Static copier used with anonymous/closed types.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class Copier<T>
	{			
		/// <summary>
		/// Performs a loose copy from source to target.
		/// </summary>
		/// <typeparam name="S"></typeparam>
		/// <param name="target"></param>
		/// <param name="source"></param>
		/// <param name="container"></param>
		public static void LooseCopyTo<S>(T target, S source, IContainer container)
		{
			LooseCopyHelper<S>.LooseCopyTo(target, source, container);
		}

		internal static void RegisterAnonymousSourceCopierForTarget<A>()
		{
			Container.Root.ForType<ICopier<A, T>>()
				.Register((c, p) => new AnonymousSourceCopier<A,T>())
				.ResolveAnInstancePerRequest()
				.End();
		}
		
		private static class LooseCopyHelper<S>
		{
			static readonly Lazy<Action<T, S, IContainer>> __looseCopy = new Lazy<Action<T, S, IContainer>>(GeneratePerformLooseCopy, LazyThreadSafetyMode.ExecutionAndPublication);			

			internal static void LooseCopyTo(T target, S source, IContainer container)
			{
				Contract.Requires<ArgumentNullException>(target != null);
				Contract.Requires<ArgumentNullException>(source != null);
				Contract.Requires<ArgumentNullException>(container != null);
				
				__looseCopy.Value(target, source, container);
			}

			internal static Action<T, S, IContainer> GeneratePerformLooseCopy()
			{
				var method = new DynamicMethod(String.Format("LooseCopyTo", typeof(T).Name)
					 , MethodAttributes.Public | MethodAttributes.Static
					 , CallingConventions.Standard
					 , null
					 , new Type[] { typeof(T), typeof(S), typeof(IContainer) }
					 , typeof(S)
					 , false
					 );
				var il = method.GetILGenerator();
				var local = il.DeclareLocal(typeof(S));

				il.Nop();

				var props = (from src in typeof(S).GetReadablePropertiesFromHierarchy(BindingFlags.Instance | BindingFlags.Public)
										 join dest in typeof(T).GetWritableProperties(BindingFlags.Instance | BindingFlags.Public)
											 on src.Name equals dest.Name
										 select new
										 {
											 Source = src,
											 Destination = dest
										 }).ToArray();
				foreach (var prop in props)
				{
					if (prop.Destination.PropertyType.IsAssignableFrom(prop.Source.PropertyType))
					{
						//
						// target.<property-name> = src.<property-name>;
						//
						il.LoadArg_0();
						il.LoadArg_1();
						var getter = prop.Source.GetGetMethod();
						if (getter.IsVirtual || getter.DeclaringType.IsInterface) il.CallVirtual(getter);
						else il.Call(getter);

						var setter = prop.Destination.GetSetMethod();
						if (setter.IsVirtual || setter.DeclaringType.IsInterface) il.CallVirtual(setter);
						else il.Call(setter);

						il.Nop();
					}					
				}
				il.Return();

				// Create the delegate
				return (Action<T, S, IContainer>)method.CreateDelegate(typeof(Action<T, S, IContainer>));
			}
		}		
	}
}
