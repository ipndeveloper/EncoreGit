using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Common.EldResolver
{
	/// <summary>
	/// The environment level domain resolver interface.
	/// </summary>
	[ContractClass(typeof(Contracts.EldResolverContracts))]
	public interface IEldResolver
	{
		/// <summary>
		/// Adds the Environment Level Domain (ELD) to a URI.
		/// </summary>
		/// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the base host.</param>
		/// <returns>The <see cref="UriBuilder"/> with the ELD-encoded host.</returns>
		UriBuilder EldEncode(UriBuilder uriBuilder);

		/// <summary>
		/// Removes the Environment Level Domain (ELD) from a URI.
		/// </summary>
		/// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the ELD-encoded host.</param>
		/// <returns>The <see cref="UriBuilder"/> with the decoded host.</returns>
		UriBuilder EldDecode(UriBuilder uriBuilder);
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IEldResolver))]
		internal abstract class EldResolverContracts : IEldResolver
		{
			UriBuilder IEldResolver.EldEncode(UriBuilder uriBuilder)
			{
				Contract.Requires<ArgumentNullException>(uriBuilder != null);
				Contract.Requires<ArgumentException>(uriBuilder.Host != null);
				throw new NotImplementedException();
			}

			UriBuilder IEldResolver.EldDecode(UriBuilder uriBuilder)
			{
				Contract.Requires<ArgumentNullException>(uriBuilder != null);
				Contract.Requires<ArgumentException>(uriBuilder.Host != null);
				throw new NotImplementedException();
			}
		}
	}
}
