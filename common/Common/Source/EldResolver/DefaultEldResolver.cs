using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Common.EldResolver
{
	/// <summary>
	/// The default ELD resolver.
	/// </summary>
	public class DefaultEldResolver : IEldResolver
	{
		private readonly string _eld;

        /// <summary>
        /// Constructs a <see cref="DefaultEldResolver"/> using the specified Environment Level Domain (ELD).
        /// </summary>
        /// <param name="eld">Environment Level Domain (i.e. ".client.prod.netsteps.local").</param>
        public DefaultEldResolver(string eld)
        {
            Contract.Requires<ArgumentNullException>(eld != null);

            // No ELD means we're done.
            if (eld.Length == 0)
            {
                _eld = string.Empty;
                return;
            }

			if (!eld.StartsWith("."))
			{
				throw new ArgumentException("Environment Level Domain (ELD) must start with a \".\".", "eld");
			}

			if (eld.EndsWith("."))
			{
				throw new ArgumentException("Environment Level Domain (ELD) must not end with a \".\".", "eld");
			}

			if (eld.Contains(" "))
			{
                throw new ArgumentException("Environment Level Domain (ELD) must not contain any spaces.", "eld");
            }

            _eld = eld;
        }

        /// <summary>
		/// Adds the Environment Level Domain (ELD) to a URI.
		/// </summary>
		/// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the base host.</param>
		/// <returns>The <see cref="UriBuilder"/> with the ELD-encoded host.</returns>
		public UriBuilder EldEncode(UriBuilder uriBuilder)
		{
            // No ELD means we're done.
            if (_eld.Length == 0)
            {
                return uriBuilder;
            }

            // Avoid double-encoding.
            if (uriBuilder.Host.Contains(_eld))
            {
                return uriBuilder;
            }

            uriBuilder.Host += _eld;

            return uriBuilder;
		}

		/// <summary>
		/// Removes the Environment Level Domain (ELD) from a URI.
		/// </summary>
		/// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the ELD-encoded host.</param>
		/// <returns>The <see cref="UriBuilder"/> with the decoded host.</returns>
		public UriBuilder EldDecode(UriBuilder uriBuilder)
		{
            // No ELD means we're done.
            if (_eld.Length == 0)
            {
                return uriBuilder;
            }

            uriBuilder.Host = uriBuilder.Host.Replace(_eld, string.Empty);

			return uriBuilder;
		}
	}
}
