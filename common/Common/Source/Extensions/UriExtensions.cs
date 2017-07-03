using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Routing;

namespace NetSteps.Common.Extensions
{
    public static class UriExtensions
    {
        /// <summary>
        /// Sets the scheme of a URI to "https".
        /// </summary>
        /// <param name="absoluteUri">An absolute URI string.</param>
        /// <returns>An absolute URI string with the modified scheme.</returns>
        public static string ToHttps(this string absoluteUri)
        {
            Contract.Requires<ArgumentNullException>(absoluteUri != null);

            return new UriBuilder(absoluteUri)
                .ToHttps()
                .Uri
                .AbsoluteUri;
        }

        /// <summary>
        /// Sets the scheme of a URI to "https".
        /// </summary>
        /// <param name="uri">A <see cref="Uri"/> containing the URI.</param>
        /// <returns>The specified <see cref="Uri"/> with the modified scheme.</returns>
        public static Uri ToHttps(this Uri uri)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentException>(uri.Scheme != null);

            // Check if already secure
            if (uri.Scheme.EqualsIgnoreCase("https"))
            {
                return uri;
            }

            if (!uri.Scheme.EqualsIgnoreCase("http"))
            {
                throw new ArgumentException("Input URI scheme must be http or https.", "uri");
            }

            return new UriBuilder(uri)
                .ToHttps()
                .Uri;
        }

        /// <summary>
        /// Sets the scheme of a URI to "https".
        /// </summary>
        /// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the URI.</param>
        /// <returns>The specified <see cref="UriBuilder"/> with the modified scheme.</returns>
        public static UriBuilder ToHttps(this UriBuilder uriBuilder)
        {
            Contract.Requires<ArgumentNullException>(uriBuilder != null);
            Contract.Requires<ArgumentException>(uriBuilder.Scheme != null);

            // Check if already secure
            if (uriBuilder.Scheme.EqualsIgnoreCase("https"))
            {
                return uriBuilder;
            }

            if (!uriBuilder.Scheme.EqualsIgnoreCase("http"))
            {
                throw new ArgumentException("Input URI scheme must be http or https.", "uriBuilder");
            }

            uriBuilder.Scheme = "https";
            uriBuilder.Port = 443;
            return uriBuilder;
        }

        /// <summary>
        /// Adds values to the query string of an absolute URI string.
        /// </summary>
        /// <param name="absoluteUri">An absolute URI string.</param>
        /// <param name="queryValues">An object that contains the additional query string values. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>An absolute URI string with the modified query string.</returns>
        public static string AppendQueryValues(this string absoluteUri, object queryValues)
        {
            Contract.Requires<ArgumentNullException>(absoluteUri != null);
            Contract.Requires<ArgumentNullException>(queryValues != null);

            return new UriBuilder(absoluteUri)
                .AppendQueryValues(queryValues)
                .Uri
                .AbsoluteUri;
        }

        /// <summary>
        /// Adds values to the query string of a URI.
        /// </summary>
        /// <param name="uri">A <see cref="Uri"/> containing the URI.</param>
        /// <param name="queryValues">An object that contains the additional query string values. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>The specified <see cref="Uri"/> with the modified query string.</returns>
        public static Uri AppendQueryValues(this Uri uri, object queryValues)
        {
            Contract.Requires<ArgumentNullException>(uri != null);
            Contract.Requires<ArgumentNullException>(queryValues != null);

            return new UriBuilder(uri)
                .AppendQueryValues(queryValues)
                .Uri;
        }

        /// <summary>
        /// Adds values to the query string of a URI.
        /// </summary>
        /// <param name="uriBuilder">A <see cref="UriBuilder"/> containing the URI.</param>
        /// <param name="queryValues">An object that contains the additional query string values. The parameters are retrieved through reflection by examining the properties of the object. The object is typically created by using object initializer syntax.</param>
        /// <returns>The specified <see cref="UriBuilder"/> with the modified query string.</returns>
        public static UriBuilder AppendQueryValues(this UriBuilder uriBuilder, object queryValues)
        {
            Contract.Requires<ArgumentNullException>(uriBuilder != null);
            Contract.Requires<ArgumentNullException>(queryValues != null);

            // Parse query string into an HttpValueCollection
            var queryItems = HttpUtility.ParseQueryString(uriBuilder.Query);

            // Parse, filter, & add queryValues
            new RouteValueDictionary(queryValues)
                .Where(x => !queryItems.AllKeys.Contains(x.Key))
                .ToList()
                .ForEach(x =>
                {
                    queryItems[x.Key] = x.Value.ToString();
                });

            // Save new query string (HttpValueCollection automatically URL encodes)
            uriBuilder.Query = queryItems.ToString();

            return uriBuilder;
        }


        /// <summary>
        /// Swap the host part of a Uri
        /// </summary>
        public static UriBuilder SwapHost(this UriBuilder target, UriBuilder source)
        {
            target.Host = source.Uri.Host;

            return target;
        }
    }
}
