using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Enumeration of scope behaviors
	/// </summary>
	[Flags]
	public enum ScopeBehavior
	{
		/// <summary>
		/// Default == InstancePerRequest
		/// </summary>
		Default = 0,

		/// <summary>
		/// Indicates an instance is created for each
		/// container request. (Default)
		/// </summary>
		InstancePerRequest = 0,

		/// <summary>
		/// Indicates that specialization is no longer allowed.
		/// Pertains to the current container as well as any subsequent container scopes.
		/// </summary>
		SpecializationDisallowed = 1,

		/// <summary>
		/// Indicates an instance is created for each
		/// lifetime scope.
		/// </summary>
		InstancePerScope = 2,

		/// <summary>
		/// Indicates an instance is only created once
		/// within the shared-scope of a root container.
		/// </summary>
		Singleton = 4,

		/// <summary>
		/// Indicates an instance is a singleton and cannot be
		/// overridden.
		/// </summary>
		FinalSingleton = Singleton | SpecializationDisallowed
	}
}
