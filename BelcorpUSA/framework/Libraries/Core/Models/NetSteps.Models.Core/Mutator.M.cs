using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Reflection.Emit;
using NetSteps.Models.Core.SPI;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Contains utility methods for mutating implentation type N of model type M.
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="N">implementation type N</typeparam>
	public static class Mutator<M, N>
		where N : M
	{
		/// <summary>
		/// Mutates the target model using the mutation descriptor given.
		/// </summary>
		/// <typeparam name="MD">mutation descriptor type MD</typeparam>
		/// <param name="context">the mutation context</param>
		/// <param name="target">the target model</param>
		/// <param name="mutationDescriptor">a mutation descriptor</param>
		public static void Mutate<MD>(IMutationContext context, N target, MD mutationDescriptor)
		{
			MutationHelper<MD>.Mutate(target, mutationDescriptor, context);
		}

		private static class MutationHelper<MD>
		{
			static readonly Lazy<Action<N, MD, IMutationContext>> __mutate = new Lazy<Action<N, MD, IMutationContext>>(GenerateMutator, LazyThreadSafetyMode.ExecutionAndPublication);

			internal static void Mutate(N target, MD source, IMutationContext context)
			{
				Contract.Requires<ArgumentNullException>(target != null);
				Contract.Requires<ArgumentNullException>(source != null);
				Contract.Requires<ArgumentNullException>(context != null);

				__mutate.Value(target, source, context);
			}

			internal static Action<N, MD, IMutationContext> GenerateMutator()
			{
				var method = new DynamicMethod(String.Format("Mutate_", typeof(M).Name)
					 , MethodAttributes.Public | MethodAttributes.Static
					 , CallingConventions.Standard
					 , null
					 , new Type[] { typeof(N), typeof(MD), typeof(IMutationContext) }
					 , typeof(MD)
					 , false
					 );
				var il = method.GetILGenerator();

				il.Nop();

				var props = (from src in typeof(MD).GetReadableProperties(BindingFlags.Instance | BindingFlags.Public)
										 join dest in typeof(N).GetWritableProperties(BindingFlags.Instance | BindingFlags.Public)
											 on src.Name equals dest.Name
										 select new
										 {
											 Source = src,
											 Destination = dest
										 }).ToArray();
				foreach (var prop in props.Where((p) => p.Destination.PropertyType.IsAssignableFrom(p.Source.PropertyType)))
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

				var notAssignable = props.Where((p) => !p.Destination.PropertyType.IsAssignableFrom(p.Source.PropertyType));
				if (notAssignable.Count() > 0)
				{
					var hasSpi = il.DeclareLocal(typeof(bool));
					var conditional = il.DeclareLocal(typeof(bool));
					il.LoadArg_0();
					il.IsInstance(typeof(IModelSPI<M>));
					il.LoadNull();
					il.CompareGreaterThan_Unsigned();
					il.StoreLocal_0();
					il.LoadLocal_0();
					il.LoadValue(false);
					il.CompareEqual();
					il.StoreLocal_1();
					il.LoadLocal_1();
					var returnLabel = il.DefineLabel();
					il.BranchIfTrue(returnLabel);
					foreach (var prop in notAssignable)
					{
						il.Nop();
						il.LoadArg_0();
						il.LoadArg_0();
						il.CastClass(typeof(IModelSPI<M>));
						il.LoadValue(prop.Source.Name);
						il.LoadArg_2();
						il.LoadArg_1();
						var getter = prop.Source.GetGetMethod();
						if (getter.IsVirtual || getter.DeclaringType.IsInterface) il.CallVirtual(getter);
						else il.Call(getter);
						il.CallVirtual(typeof(IModelSPI<M>).GetGenericMethod("CascadeReferentMutation", 3, 2)
							.MakeGenericMethod(prop.Destination.PropertyType, prop.Source.PropertyType)
							);

						var setter = prop.Destination.GetSetMethod();
						if (setter.IsVirtual || setter.DeclaringType.IsInterface) il.CallVirtual(setter);
						else il.Call(setter);

						il.Nop();
					}
					il.Nop();
					il.MarkLabel(returnLabel);
				}
				il.Return();

				// Create the delegate
				return (Action<N, MD, IMutationContext>)method.CreateDelegate(typeof(Action<N, MD, IMutationContext>));
			}
		}
	}

}
