using System;
using NetSteps.Objects.Business;
using WebSupergoo.ABCpdf6;

namespace NetSteps.Web.Base
{
    public class PDFEnrollment
    {
        #region---Properties---
        private Account _account;
        public Account Account
        {
            get { return _account; }
            set { _account = value; }
        }

        private AccountAddress _address;
        public AccountAddress Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _path;

        #endregion

        #region---Constructors---

        public PDFEnrollment()
        {
        }
        public PDFEnrollment(Account account, string path)
        {
            this._account = account;
            //this._address = account.Addresses.GetByTypeID(Constants.AddressType.Main); //AccountAddress.eAddressType.MailingAddress);
            this._path = path;
        }

        #endregion

        #region---Methods---

        public byte[] FillPdf()
        {
            Doc doc = new Doc();
            doc.Read(_path);
            doc.Font = doc.AddFont("Helvetica-Bold");
            doc.FontSize = 12;
            doc.Rect.Pin = (int)XRect.Corner.TopLeft;

            doc.Form["AppM"].Value = Account.EnrollmentDate.Month.ToString();
            doc.Form["AppD"].Value = Account.EnrollmentDate.Day.ToString();
            doc.Form["AppY"].Value = Account.EnrollmentDate.Year.ToString();
            doc.Form["AppName"].Value = Account.LastName + " " + Account.FirstName;
            //doc.Form["AppFirstName"].Value = Account.FirstName;
            doc.Form["PartName"].Value = Account.Properties.GetValue<string>("CoAppLastName") + "" + Account.Properties.GetValue<string>("CoAppFirstName");
            //doc.Form["PartFirstName"].Value = Account.Properties.GetValue<string>("CoAppFirstName");
            doc.Form["AppGovID"].Value = Account.Properties.GetValue<string>("GovernmentID");

            Account sponsorGovID = Account.Find(Account.Site, Account.Properties.GetValue<string>("GovernmentID"));
            if (sponsorGovID != null)
            {
                doc.Form["PartGovID"].Value = sponsorGovID.EnrollerID.ToString(); // for now we will show sponsor's ID
            }
            //the following 3 items are not part of enrollment currently
            doc.Form["chkPartHusband"].Value = "";
            doc.Form["chkPartWife"].Value = "";
            doc.Form["chkPartChild"].Value = "";

            doc.Form["AppBM"].Value = Account.Birthday.Month.ToString();
            doc.Form["AppBD"].Value = Account.Birthday.Day.ToString();

            doc.Form["AppBY"].Value = Account.Birthday.Year.ToString();
            if (Account.GenderID == (short)Account.Gender.Male)
            {
                doc.Form["AppSexM"].Value = "X";
            }
            else
            {
                doc.Form["AppSexF"].Value = "X";
            }

            string[] dateParts = Account.Properties.GetValue<DateTime>("CoAppBirthdate").ToString("yy-MM-dd").Split('-');
            if (dateParts[0] != "1900")
            {
                doc.Form["PartBM"].Value = dateParts[1];
                doc.Form["PartBD"].Value = dateParts[2];
                doc.Form["PartBY"].Value = dateParts[0];
            }

            Account sponsorGender = null;
            if (sponsorGender != null)
            {
                if (sponsorGender.GenderID == (short)Account.Gender.Female)
                {
                    doc.Form["PartSexM"].Value = "X";
                }
                else
                {
                    doc.Form["PartSexF"].Value = "X";
                }
            }

            AccountAddress address = Account.Addresses.GetByTypeID(Constants.AddressType.Main);
            doc.Form["HomeAddress"].Value = address.Address1 + "" + address.City + "" + address.State + "" + address.PostalCode;

            AccountAddress mailingAdd = Account.Addresses.GetByTypeID(Constants.AddressType.Shipping);
            doc.Form["MailingAddress"].Value = mailingAdd.Address1 + "" + mailingAdd.City + "" + mailingAdd.State + " " + mailingAdd.PostalCode;

            AccountPhone phone = Account.Phones.GetByTypeID(AccountPhone.ePhoneType.HomeNumber);
            doc.Form["Tel"].Value = phone.PhoneNumber;

            AccountPhone cellphone = Account.Phones.GetByTypeID(AccountPhone.ePhoneType.CellNumber);
            doc.Form["Mobile"].Value = cellphone.PhoneNumber;

            AccountPhone faxPhone = Account.Phones.GetByTypeID(AccountPhone.ePhoneType.FaxNumber);
            doc.Form["Fax"].Value = faxPhone.PhoneNumber;
            doc.Form["Email"].Value = Account.EmailAddress;

            //Currently we don't gather any info for previous registered distributors in our enrollment process
            /*
           doc.Form["AppRegisteredLastName"].Value = "San Juan";
           doc.Form["AppRegisteredFirstName"].Value = "Nick";
           doc.Form["PartRegisteredLastName"].Value = "Joe";
           doc.Form["PartRegisteredFirstName"].Value = "Doe";
           doc.Form["AppPreviousID"].Value = "2222233333";
           doc.Form["CancellationDateMonth"].Value = "10";
           doc.Form["CancellationDateDay"].Value = "12";
           doc.Form["CancellationDateYear"].Value = "1995";
           doc.Form["PartPreviousID"].Value = "2222224444";
           doc.Form["PartCancellationDateMonth"].Value = "10";
           doc.Form["PartCancellationDateDay"].Value = "12";
           doc.Form["PartCancellationDateYear"].Value = "1997";
            */

            Account sponsor = Account.Find(Account.Site, Account.Properties.GetValue<string>("NewaysID"));

            if (sponsor != null)
            {
                doc.Form["SponsorLastName"].Value = sponsor.LastName;
                doc.Form["SponsorFirstName"].Value = sponsor.FirstName;
                AccountAddress sponsorAddress = Account.Addresses.GetByTypeID(Constants.AddressType.Main);
                doc.Form["SponsorAddress"].Value = sponsorAddress.Address1 + "" + sponsorAddress.City + "" + sponsorAddress.State + "" + sponsorAddress.PostalCode;
                AccountPhone sponsorPhone = Account.Phones.GetByTypeID(AccountPhone.ePhoneType.HomeNumber);
                doc.Form["SponsorPhone"].Value = sponsorPhone.PhoneNumber;
                AccountPhone sponsorFaxPhone = Account.Phones.GetByTypeID(AccountPhone.ePhoneType.FaxNumber);
                doc.Form["SponsorFax"].Value = sponsorFaxPhone.PhoneNumber;
            }
            doc.Form.Stamp();
            return doc.GetData();

        }
        #endregion
    }
}
