using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Interface for classes that can resolve references to models of type M
	/// </summary>
	/// <typeparam name="M">model type M</typeparam>
	/// <typeparam name="IK">identity key type IK</typeparam>
	public interface IIdentifiableModelResolver<out M, out IK> : IModelResolver<M>
	{
		/// <summary>
		/// Gets the identity key this resolver can resolve.
		/// </summary>
		IK IdentityKey { get; }
	}
}
