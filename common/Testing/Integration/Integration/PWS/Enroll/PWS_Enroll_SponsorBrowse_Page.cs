using System;
using WatiN.Core;
using ListItems = WatiN.Core.ListItem;
using System.Linq;
using System.Threading;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    /// <summary>
    /// Class related to Controls and methods of PWS Sponsor Browse page.
    /// </summary>
    public class PWS_Enroll_SponsorBrowse_Page : PWS_Base_Page
    {
        private TextField _txtID, _txtFirstName, _txtLastName, _txtPostalCode, _street, _city, _zip, _distance;
        private Link _lnkSearchDistributor;
        private RadioButton _searchByConsultant, _searchByLocation;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _searchByConsultant = _content.GetElement<RadioButton>(new Param("btnSearchTypeAccountInfo"));
            _searchByLocation = _content.GetElement<RadioButton>(new Param("btnSearchTypeLocation"));
            _txtID = _content.GetElement<TextField>(new Param("AccountLocator_AccountNumber"));
            _txtFirstName = _content.GetElement<TextField>(new Param("AccountLocator_FirstName"));
            _txtLastName = _content.GetElement<TextField>(new Param("AccountLocator_LastName"));
            _txtPostalCode = _content.GetElement<TextField>(new Param("AccountLocator_PostalCode"));
            _street = _content.GetElement<TextField>(new Param("AccountLocator_Address1"));
            _street.CustomWaitForExist();
            _street.CustomWaitForVisibility(false, 10);
            _city = _content.GetElement<TextField>(new Param("AccountLocator_City"));
            _zip = _content.GetElement<TextField>(new Param("AccountLocator_PostalCode"));
            _distance = _content.GetElement<TextField>(new Param("AccountLocator_MaximumDistance"));
            _lnkSearchDistributor = _content.GetElement<Link>(new Param("Button btnContinue btnLocator", AttributeName.ID.ClassName));
            Thread.Sleep(2000); // Wait for page to tab the search criteria;
        }

        public override bool IsPageRendered()
        {            
            return _lnkSearchDistributor.Exists;
        }

        public PWS_Enroll_SponsorBrowse_Page SearchByLocation(bool searchByLocation)
        {
            _searchByLocation.CustomSelectRadioButton(searchByLocation);
            return this;
        }

        public PWS_Enroll_SponsorBrowse_Page EnterLocation(Address address, string distance = null)
        {
            if (address.Address1 != null)
            {
                _street.CustomSetTextQuicklyHelper(address.Address1);
                _street.CustomRunScript(Util.strChange);
            }
            if (address.City != null)
            {
                _city.CustomSetTextQuicklyHelper(address.City);
                _city.CustomRunScript(Util.strChange);
            }
            _zip.CustomSetTextQuicklyHelper(address.PostalCode);
            _zip.CustomRunScript(Util.strChange);
            if (distance != null)
            {
                _distance.CustomSetTextQuicklyHelper(distance);
                _distance.CustomRunScript(Util.strChange);
            }
            return this;
        }

        public PWS_Enroll_SponsorBrowse_Page EnterID(string id)
        {
            _txtID.CustomSetTextQuicklyHelper(id);
            _txtID.CustomRunScript(Util.strChange);
            return this;
        }

        public PWS_Enroll_SponsorBrowse_Page EnterName(string firstName, string lastName)
        {
            _txtFirstName.CustomSetTextQuicklyHelper(firstName);
            _txtFirstName.CustomRunScript(Util.strChange);
            _txtLastName.CustomSetTextQuicklyHelper(lastName);
            _txtLastName.CustomRunScript(Util.strChange);
            return this;
        }

        public PWS_Enroll_SponsorBrowse_Page EnterName(Distributor distributor)
        {
            return EnterName(distributor.FirstName, distributor.LastName);
        }

        public PWS_Enroll_SponsorBrowse_Page ClickSearchDistributors(int? timeout = null)
        {
            _lnkSearchDistributor.CustomClick(timeout);
            return this;
        }

        public ControlCollection<PWS_Enroll_Sponsor_Control> GetSponsors()
        {
            _content.GetElement<Div>(new Param("Profile", AttributeName.ID.ClassName)).CustomWaitForExist();
            return _content.GetElements<Div>(new Param("Profile", AttributeName.ID.ClassName)).As<PWS_Enroll_Sponsor_Control>();
        }

        public PWS_Enroll_Sponsor_Control GetSponsor(int index)
        {
            return GetSponsors()[index];
        }

        public PWS_Enroll_Sponsor_Control GetSponsor(string urlMatch)
        {
            PWS_Enroll_Sponsor_Control sponsor = null;
            foreach (PWS_Enroll_Sponsor_Control currentSponsor in GetSponsors())
            {
                if (currentSponsor.Website.Contains(urlMatch))
                {
                    sponsor = currentSponsor;
                    break;
                }
            }
            return sponsor;
        }

        //public bool IsSearchResultExists()
        //{
        //    return _lnkSearchDistributor.Exists;
        //}
    }
}
