using WatiN.Core;
using ListItems = WatiN.Core.ListItem;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using NetSteps.Testing.Integration.GMP.Accounts;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    /// <summary>
    /// Class related to Controls and methods of PWS Sponsor page.
    /// </summary>
    public class PWS_Enroll_Sponsor_Page : PWS_Base_Page
    {
        private Link _continueSelectedSponsor;
        private Link _searchForDifferentSponsor;
        private Div _divSponsor;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            // Sponsor
            _continueSelectedSponsor = Document.GetElement<Link>
                (
                new Param("btnSelectSponsor", AttributeName.ID.ClassName, RegexOptions.None)
                .Or(new Param("^Button mt5", AttributeName.ID.ClassName, RegexOptions.None))
                );
            _searchForDifferentSponsor = Document.GetElement<Link>(new Param("btnSwitchSponsor", AttributeName.ID.ClassName, RegexOptions.None).Or(new Param("FL Button MinorButton mt5", AttributeName.ID.ClassName)));
            _divSponsor = Document.GetElement<Div>(new Param("sponsor"));
        }

        public TPage ClickContinueSelectedSponsor<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = this._continueSelectedSponsor.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Enroll_SponsorBrowse_Page ClickSearchForDifferentSponsor(int? timeout = null, bool pageRequired = true)
        {
            timeout = this._searchForDifferentSponsor.CustomClick(timeout);
            return Util.GetPage<PWS_Enroll_SponsorBrowse_Page>(timeout, pageRequired);
        }

         public override bool IsPageRendered()
        {
            return this._continueSelectedSponsor.Exists;
        }

        public bool ValidateSponsor(GMP_Accounts_Account_Control account)
        {
            Span spanSponsor = this._divSponsor.Spans.First<Span>();
            return spanSponsor.CustomGetText().Contains(string.Format("{0} {1}", account.FirstName, account.LastName));
        }
    }
}