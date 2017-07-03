using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Properties;
using NetSteps.Encore.Core.Stereotype;

namespace NetSteps.Encore.Core.Dto
{
	/// <summary>
	/// Marks an interface or class as a stereotypical DTO and implements the stereotypical DTO behavior for interfaces.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
	public class DTOAttribute : StereotypeAttribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DTOAttribute()
			: base(StereotypeBehaviors.AutoImplementedBehavior)
		{
		}

		/// <summary>
		/// Implements the stereotypical DTO behavior for interfaces of type T.
		/// </summary>
		/// <typeparam name="T">interface type T</typeparam>
		/// <returns>concrete implementation implementing the 
		/// stereotypical DTO behavior</returns>
		public override bool RegisterStereotypeImplementation<T>(IoC.IContainer container)
		{
			Contract.Assert(typeof(T).IsInterface, Resources.Err_StereotypeBehaviorGeneratedForInterfacesOnly);

			Container.Root.ForType<T>()
				.Register(DataTransfer.ConcreteType<T>())
				.ResolveAnInstancePerRequest()
				.End();
			return true;
		}
	}
}
