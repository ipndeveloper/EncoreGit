using System;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.Web;
using System.Collections.Generic;

namespace NetSteps.SOD.Common
{
    /// <summary>
    /// Extension methods for transforming API models for use with the API
    /// </summary>
    public static class Extensions
    {
        static char[] xmlReservedChars = { '<', '>', '&', '%' };

        static XElement ToXElement(IAccountInfo acct)
        {
            Contract.Requires<ArgumentNullException>(acct != null);
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(acct.FirstName), "FirstName is required");
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(acct.LastName), "LastName is required");
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(acct.Password), "Password is required");
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(acct.Email), "Email is required");

            return new XElement("distributor",
                new XElement("firstname", SafeEncodeValue(acct.FirstName)),
                new XElement("lastname", SafeEncodeValue(acct.LastName)),
                new XElement("password", SafeEncodeValue(SuccessOnDemandApiUtils.PreparePasswordForApi(acct.Password))),
                new XElement("email", SafeEncodeValue(acct.Email)),
                new XElement("website", SafeEncodeValue(acct.Website)),
                new XElement("phone", SafeEncodeValue(acct.Phone)),
                new XElement("address", SafeEncodeValue(acct.Address)),
                new XElement("city", SafeEncodeValue(acct.City)),
                new XElement("state", SafeEncodeValue(acct.State)),
                new XElement("zip", SafeEncodeValue(acct.Zip)),
                new XElement("country", SafeEncodeValue(acct.Country)),
                new XElement("language", SafeEncodeValue(acct.Language)),
                new XElement("active", SafeEncodeValue(acct.Active)),
                new XElement("type", SafeEncodeValue(acct.Type)),
                new XElement("rank", SafeEncodeValue(acct.Rank)),
                new XElement("ID", SafeEncodeValue(acct.ID))
                );
        }

        /// <summary>
        /// Per the SOD API doc, strings that contain '&' chars must be encoded as CDATA...
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static XText SafeEncodeValue(string value)
        {
            if (value == null) return new XText(String.Empty);
            if (value.IndexOfAny(xmlReservedChars) >= 0) return new XCData(value);
            return new XText(value);
        }

        /// <summary>
        /// Encodes Success on Demand account info for use with the SOD API.
        /// </summary>
        /// <param name="acct">account info</param>
        /// <returns>string representation for the post body</returns>
        public static string EncodeAsPostBody(this IAccountInfo acct)
        {
            Contract.Requires<ArgumentNullException>(acct != null);

            return EncodeAsPostBody(acct, false);
        }
        /// <summary>
        /// Encodes Success on Demand account info for use with the SOD API.
        /// </summary>
        /// <param name="acct">account info</param>
        /// <param name="forCreate">indicates whether the postbody will be used to create and account in SOD</param>
        /// <returns>string representation for the post body</returns>
        public static string EncodeAsPostBody(this IAccountInfo acct, bool forCreate)
        {
            Contract.Requires<ArgumentNullException>(acct != null);

            var xml = ToXElement(acct);
            if (!forCreate)
            {
                if (String.IsNullOrWhiteSpace(acct.DistID)) throw new ArgumentException("DistID is required when updating account info.", "acct");
                xml.AddFirst(new XElement("DistID", acct.DistID));
            }
            return String.Concat("xmldata=", xml.ToString());
        }

        /// <summary>
        /// Encodes login info for use with the SOD API
        /// </summary>
        /// <param name="acct">account info</param>
        /// <returns>string representation for the post body</returns>
        public static string EncodeAsFormBody(this ILoginInfo login)
        {
            Contract.Requires<ArgumentNullException>(login != null);
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(login.Email), "Email is required");
            Contract.Requires<ArgumentException>(!String.IsNullOrWhiteSpace(login.Password), "Password is required");


            return String.Concat("email_address=", HttpUtility.UrlEncode(login.Email)
                , "&password=", HttpUtility.UrlEncode(SuccessOnDemandApiUtils.PreparePasswordForApi(login.Password)));
        }
    }
}
