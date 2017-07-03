using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Context used during the framework's mutation
	/// logic to communicate the container and detect cycles
	/// and multiple references in the graph.
	/// </summary>
	public interface IMutationContext : ICopyContext
	{		
	}

	/// <summary>
	/// Default mutation context implementation
	/// </summary>
	public sealed class MutationContext : CopyContext, IMutationContext
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="container">an IoC container (for scope)</param>
		public MutationContext(IContainer container)
			: base(container)
		{
			Contract.Requires<ArgumentNullException>(container != null);
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="container">an IoC container (for scope)</param>
		/// <param name="tracking">lifespan tracking for copies made within the context.</param>
		public MutationContext(IContainer container, LifespanTracking tracking)
			: base(container, tracking)
		{
			Contract.Requires<ArgumentNullException>(container != null);
		}		
	}
}
