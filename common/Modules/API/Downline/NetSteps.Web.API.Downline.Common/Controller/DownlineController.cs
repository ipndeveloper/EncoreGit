using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Downline.Common;
using NetSteps.Web.API.Base.Common;
using NetSteps.Web.API.Downline.Common.Models;
using NetSteps.Encore.Core;

namespace NetSteps.Web.API.Downline.Common
{
	/// <summary>
	/// Searchs Downline
	/// </summary>
    public class DownlineController : BaseController
    {

        #region Declarations

        private IDownlineSearch downlineSearch;

        private ILogResolver logResolver;

        private ITermResolver termResolver;

		private readonly static ICopier<SearchDownlineModel, ISearchDownlineModel> _searchDownlineCopier = Create.New<ICopier<SearchDownlineModel, ISearchDownlineModel>>();

        #endregion

        #region Constructor(s)

		/// <summary>
		/// Create an instance
		/// </summary>
        public DownlineController() : this(Create.New<IDownlineSearch>(), Create.New<ILogResolver>(), Create.New<ITermResolver>())            
        {
            Contract.Ensures(downlineSearch != null);
            Contract.Ensures(termResolver != null);
            Contract.Ensures(logResolver != null);
        }

		/// <summary>
		/// Create an instance
		/// </summary>
		/// <param name="dSearch">Downline Module</param>
		/// <param name="lResolver">Log Resolver</param>
		/// <param name="tResolver">Term Resolver</param>
        public DownlineController(IDownlineSearch dSearch, ILogResolver lResolver, ITermResolver tResolver)
        {
            this.downlineSearch = dSearch ?? Create.New<IDownlineSearch>();
            this.logResolver = lResolver ?? Create.New<ILogResolver>();
            this.termResolver = tResolver ?? Create.New<ITermResolver>();
        }

        #endregion

        #region Methods

        private bool ValidateAccountID(int accountID, ref string message)
        {
            bool isValid = true;

            if (accountID == 0)
            {
                isValid = false;
                string term = termResolver.Term("Downline_Invalid_AccountID", "Invalid AccountID:");
                message = string.IsNullOrEmpty(message)
                    ? string.Format("{0} {1}", term, accountID)
                    : string.Format("{0} {1} {2}", message, term, accountID);
            }

            return isValid;
        }

        private bool ValidateQuery(string query, ref string message)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(query))
            {
                isValid = false;
                string term = termResolver.Term("Downline_Invalid_Query", "Invalid Query:");
                message = string.IsNullOrEmpty(message)
                    ? string.Format("{0} {1}", term, query)
                    : string.Format("{0} {1} {2}", message, term, query);
            }

            return isValid;
        }

		private bool ValidateSponsorID(int sponsorID, SearchDownlineModel model, ref string message)
        {
            bool isValid = true;

            if (sponsorID == 0)
            {
                isValid = false;
                string term = termResolver.Term("Downline_Invalid_SponsorID", "Invalid SponsorID:");
                message = string.Format("{0} {1}", term, sponsorID);
            }
			if (sponsorID != model.SponsorID)
			{
				isValid = false;
				string term = termResolver.Term("Downline_SponsorID_Mismatch", "SponsorID in the route does not match the SponsorID in the model");
				message = string.Format("{0} {1}", term, sponsorID);
			}

            return isValid;
        }              
        /// <summary>
        /// Search Downline for a specific accountID or by name. an accountID or query is required
		/// 
		/// eg. http://yourdomain.com/account/{sponsorID}/downline?accountID=1
        /// </summary>
        /// <param name="sponsorID">the sponsor whose downline to search</param>
        /// <param name="model">Search Downline Model</param>
		/// <returns>ActionResult</returns>
        [HttpGet]
        [ApiAccessKeyFilter]
        public ActionResult Search(int sponsorID, SearchDownlineModel model)
        {
            try
            {
                string message = string.Empty;

                bool isValidSponsorID = ValidateSponsorID(sponsorID, model, ref message);

				bool isValidAccountID = model.AccountID.HasValue ? ValidateAccountID(model.AccountID.Value, ref message) : true;

				bool isValidQuery = model.AccountID.HasValue ? true : ValidateQuery(model.Query, ref message);

				var dto = Create.New<ISearchDownlineModel>();
				_searchDownlineCopier.CopyTo(dto, model, CopyKind.Loose, Container.Current);

				if (isValidSponsorID && isValidAccountID && isValidQuery)
                {
					IDownlineSearchResult result = downlineSearch.Search(dto);

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
