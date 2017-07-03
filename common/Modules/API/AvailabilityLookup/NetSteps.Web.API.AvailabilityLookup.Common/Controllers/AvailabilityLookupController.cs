using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.AvailabilityLookup.Common;
using NetSteps.Web.API.Base.Common;

namespace NetSteps.Web.API.AvailabilityLookup.Common
{
    /// <summary>
    /// Lookup hostname availability for a PWS
    /// </summary>
    public class AvailabilityLookupController : BaseController
    {

        #region Declarations

        private IAvailabilityLookup availabilityLookup;

        private ILogResolver logResolver;

        private ITermResolver termResolver;
        
        #endregion

        #region Constructor(s)

		/// <summary>
		/// Create an Instance
		/// </summary>
        public AvailabilityLookupController() : this(Create.New<IAvailabilityLookup>(), Create.New<ILogResolver>(), Create.New<ITermResolver>())
        {
            Contract.Ensures(availabilityLookup != null);
            Contract.Ensures(termResolver != null);
            Contract.Ensures(logResolver != null);
        }
		/// <summary>
		/// Create an Instance
		/// </summary>
		/// <param name="availLookup"></param>
		/// <param name="lResolver"></param>
		/// <param name="tResolver"></param>
        public AvailabilityLookupController(IAvailabilityLookup availLookup, ILogResolver lResolver, ITermResolver tResolver)
        {
            this.availabilityLookup = availLookup;
            this.termResolver = tResolver;
            this.logResolver = lResolver;
        }

        #endregion        

        #region Methods

        private bool ValidateHostName(string hostName, ref string message)
        {
            bool isValid = true;            

            if (string.IsNullOrEmpty(hostName))
            {
                isValid = false;
                string term = termResolver.Term("AvailabilityLookup_Invalid_HostName", "Invalid HostName:");
                message = string.Format("{0} {1}", term, hostName);
            }

            return isValid;
        }

        private bool ValidateMarketID(int marketID, ref string message)
        {
            bool isValid = true;

            if (marketID == 0)
            {
                isValid = false;
                string term = termResolver.Term("AvailabilityLookup_Invalid_MarketID", "Invalid MarketID:");
                message = string.IsNullOrEmpty(message)
                    ? string.Format("{0} {1}", term, marketID)
                    : string.Format("{0} {1} {2}", message, term, marketID);
            }

            return isValid;
        }
        
        /// <summary>
        /// Lookup the availability of a hostname for a PWS
		///  
		/// eg. http://yourdomain.com/sites?hostName={hostName} 
        /// </summary>
        /// <param name="marketID">Optional: Market to search in</param>
        /// <param name="hostName">hostname to lookup</param>
		/// <returns>ActionResult</returns>
		/// <seealso cref="ActionResult"/>
        [HttpGet]
		[ApiAccessKeyFilter]
        public ActionResult Lookup(int? marketID, string hostName)
        {
            try
            {
                string message = string.Empty;

                bool isHostNameValid = ValidateHostName(hostName, ref message);

                bool isMarketValid = marketID.HasValue ? ValidateMarketID(marketID.Value, ref message) : true;

                if (isHostNameValid && isMarketValid)
                {
					ILookupResult result = marketID.HasValue ? availabilityLookup.Lookup(marketID.Value, hostName) : availabilityLookup.Lookup(hostName);

                    return this.Result_200_OK(result);
                }

                return this.Result_400_BadRequest(message);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
            }            
        }

        #endregion

    }
}
