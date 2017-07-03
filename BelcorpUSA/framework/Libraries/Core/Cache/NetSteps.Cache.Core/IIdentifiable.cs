using System.Diagnostics.CodeAnalysis;
namespace NetSteps.Core.Cache
{
	/// <summary>
	/// Interface for types that are uniquely identifiable by identity key type IK
	/// </summary>
	/// <typeparam name="IK">identity type IK</typeparam>
	public interface IIdentifiable<out IK>
	{
		/// <summary>
		/// Gets the instance's unique identity.
		/// </summary>
		/// <returns>the instance's identity key</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		IK GetIdentity();
	}
}
