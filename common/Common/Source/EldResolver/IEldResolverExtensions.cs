using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Common.EldResolver
{
	/// <summary>
	/// Extension methods for working with the <see cref="IEldResolver"/>.
	/// </summary>
	public static class IEldResolverExtensions
	{
		/// <summary>
		/// Adds the Environment Level Domain (ELD) to an absolute URI string.
		/// </summary>
		/// <param name="absoluteUri">A base absolute URI string.</param>
		/// <returns>An ELD-encoded absolute URI string.</returns>
		public static string EldEncode(this string absoluteUri)
		{
			Contract.Requires<ArgumentNullException>(absoluteUri != null);

			return Create.New<IEldResolver>()
				.EldEncode(absoluteUri);
		}

		/// <summary>
		/// Adds the Environment Level Domain (ELD) to an absolute URI string.
		/// </summary>
		/// <param name="eldResolver"></param>
		/// <param name="absoluteUri">A base absolute URI string.</param>
		/// <returns>An ELD-encoded absolute URI string.</returns>
		public static string EldEncode(this IEldResolver eldResolver, string absoluteUri)
		{
			Contract.Requires<ArgumentNullException>(eldResolver != null);
			Contract.Requires<ArgumentNullException>(absoluteUri != null);

			return eldResolver
				.EldEncode(new UriBuilder(absoluteUri))
				.Uri
				.AbsoluteUri;
		}

		/// <summary>
		/// Adds the Environment Level Domain (ELD) to a URI.
		/// </summary>
		/// <param name="uri">A <see cref="Uri"/> containing the base host.</param>
		/// <returns>The <see cref="Uri"/> with the ELD-encoded host.</returns>
		public static Uri EldEncode(this Uri uri)
		{
			Contract.Requires<ArgumentNullException>(uri != null);

			return Create.New<IEldResolver>()
				.EldEncode(uri);
		}

		/// <summary>
		/// Adds the Environment Level Domain (ELD) to a URI.
		/// </summary>
		/// <param name="eldResolver"></param>
		/// <param name="uri">A <see cref="Uri"/> containing the base host.</param>
		/// <returns>The <see cref="Uri"/> with the ELD-encoded host.</returns>
		public static Uri EldEncode(this IEldResolver eldResolver, Uri uri)
		{
			Contract.Requires<ArgumentNullException>(eldResolver != null);
			Contract.Requires<ArgumentNullException>(uri != null);

			return eldResolver
				.EldEncode(new UriBuilder(uri))
				.Uri;
		}

		/// <summary>
		/// Adds the Environment Level Domain (ELD) to a URI.
		/// </summary>
		/// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the base host.</param>
		/// <returns>The <see cref="UriBuilder"/> with the ELD-encoded host.</returns>
		public static UriBuilder EldEncode(this UriBuilder uriBuilder)
		{
			Contract.Requires<ArgumentNullException>(uriBuilder != null);

			return Create.New<IEldResolver>()
				.EldEncode(uriBuilder);
		}

		/// <summary>
		/// Removes the Environment Level Domain (ELD) from an absolute URI string.
		/// </summary>
		/// <param name="absoluteUri">An ELD-encoded absolute URI string.</param>
		/// <returns>A decoded absolute URI string.</returns>
		public static string EldDecode(this string absoluteUri)
		{
			Contract.Requires<ArgumentNullException>(absoluteUri != null);

			return Create.New<IEldResolver>()
				.EldDecode(absoluteUri);
		}

		/// <summary>
		/// Removes the Environment Level Domain (ELD) from an absolute URI string.
		/// </summary>
		/// <param name="eldResolver"></param>
		/// <param name="absoluteUri">An ELD-encoded absolute URI string.</param>
		/// <returns>A decoded absolute URI string.</returns>
		public static string EldDecode(this IEldResolver eldResolver, string absoluteUri)
		{
			Contract.Requires<ArgumentNullException>(eldResolver != null);
			Contract.Requires<ArgumentNullException>(absoluteUri != null);

			return eldResolver
				.EldDecode(new UriBuilder(absoluteUri))
				.Uri
				.AbsoluteUri;
		}

		/// <summary>
		/// Removes the Environment Level Domain (ELD) from a URI.
		/// </summary>
		/// <param name="uri">A <see cref="Uri"/> containing the ELD-encoded host.</param>
		/// <returns>The <see cref="Uri"/> with the decoded host.</returns>
		public static Uri EldDecode(this Uri uri)
		{
			Contract.Requires<ArgumentNullException>(uri != null);

			return Create.New<IEldResolver>()
				.EldDecode(uri);
		}

		/// <summary>
		/// Removes the Environment Level Domain (ELD) from a URI.
		/// </summary>
		/// <param name="eldResolver"></param>
		/// <param name="uri">A <see cref="Uri"/> containing the ELD-encoded host.</param>
		/// <returns>The <see cref="Uri"/> with the decoded host.</returns>
		public static Uri EldDecode(this IEldResolver eldResolver, Uri uri)
		{
			Contract.Requires<ArgumentNullException>(eldResolver != null);
			Contract.Requires<ArgumentNullException>(uri != null);

			return eldResolver
				.EldDecode(new UriBuilder(uri))
				.Uri;
		}

		/// <summary>
		/// Removes the Environment Level Domain (ELD) from a URI.
		/// </summary>
		/// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the ELD-encoded host.</param>
		/// <returns>The <see cref="UriBuilder"/> with the decoded host.</returns>
		public static UriBuilder EldDecode(this UriBuilder uriBuilder)
		{
			Contract.Requires<ArgumentNullException>(uriBuilder != null);

			return Create.New<IEldResolver>()
				.EldDecode(uriBuilder);
		}
	}
}
