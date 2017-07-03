using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using NetSteps.Encore.Core.Reflection.Emit;
using NetSteps.Encore.Core.Reflection;


namespace NetSteps.Encore.Core.IoC.Constructors
{
	/// <summary>
	/// Adapter for constructors defined on concrete type C of type T
	/// </summary>
	public partial class ConstructorAdapter<T, C>
		where C: T
	{
		/// <summary>
		/// Compiles a constructor adapter for the given constructor.
		/// </summary>
		/// <param name="ordinal">the ordinal position of the constructor among constructors defined on type T</param>
		/// <param name="ci">constructor info</param>
		/// <returns>the compiled constructor adapter type</returns>
		public static new Type GetConstructorAdapterByOrdinal(int ordinal, ConstructorInfo ci)
		{
			Contract.Ensures(Contract.Result<Type>() != null);

			var targetType = typeof(T);
			var concreteType = typeof(C);
			string typeName = RuntimeAssemblies.PrepareTypeName(targetType, String.Concat(concreteType.Name, "Ctor#", ordinal));

			var module = ConstructorAdapter.Module;
			lock (module)
			{
				Type type = module.Builder.GetType(typeName, false, false);
				if (type == null)
				{
					type = ConstructorAdapter<T>.BuildConstructorAdapter(module, typeName, ci);
				}
				return type;
			}
		}
	}
}
