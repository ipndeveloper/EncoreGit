using System;
using System.Data;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common;

namespace NetSteps.WebService.Mobile.Models
{
    public class ContactModel
    {
        public string address;
        public string birthdate;
        public string commpref;
        public string email;
        public string firstName;
        public string homephone;
        public string lastName;
        public string mobilephone;
        public string thumb;
        public string type;
        public string workphone;
        public string onlinetotal;
        public bool? rsvp;
        public bool customer;
        public string generation;
        public string level;
        public string pv;
        public string gv;
        public string accountnumber;
        public string accountid;
        public string sortorder;
        public string lastpurchasedate;
        public string title;
        public bool showCommissions;
        public string sponsor;

        public static implicit operator ContactModel(DataRow data)
        {
            var table = data.Table;
            var firstNameColumn = table.Columns["FirstName"];
            var lastNameColumn = table.Columns["LastName"];
            var emailAddressColumn = table.Columns["EmailAddress"];
            var birthDateColumn = table.Columns["BirthDay"];
            var genderColumn = table.Columns["GenderId"];
            var address1Column = table.Columns["Address1"];
            var address2Column = table.Columns["Address2"];
            var cityColumn = table.Columns["City"];
            var stateColumn = table.Columns["State"];
            var postalCodeColumn = table.Columns["PostalCode"];
            var mainPhoneColumn = table.Columns["MainPhone"];
            var homePhoneColumn = table.Columns["HomePhone"];
            var cellPhoneColumn = table.Columns["CellPhone"];
            var workPhoneColumn = table.Columns["WorkPhone"];
            var categoryColumn = table.Columns["Category"];
            var profileHtmlColumn = table.Columns["html"];
            var accountNumberColumn = table.Columns["AccountNumber"];
            var accountIDColumn = table.Columns["AccountID"];
            var lastPurchaseColumn = table.Columns["LastPurchaseDate"];

            var firstName = (firstNameColumn != null && data[firstNameColumn] != DBNull.Value) ? data[firstNameColumn].ToString() : "";
            var lastName = (lastNameColumn != null && data[lastNameColumn] != DBNull.Value) ? data[lastNameColumn].ToString() : "";
            var email = (emailAddressColumn != null && data[emailAddressColumn] != DBNull.Value) ? data[emailAddressColumn].ToString() : "";

            var birthdate = (birthDateColumn != null && data[birthDateColumn] != DBNull.Value) ? DateTime.Parse(data[birthDateColumn].ToString()).ToString("MM/dd/yyyy") : "";
            if (!string.IsNullOrEmpty(birthdate) && birthdate == "01/01/1900")
                birthdate = string.Empty;

            var gender = (genderColumn != null && data[genderColumn] != DBNull.Value) ? ((NetSteps.Data.Entities.Constants.Gender)((short)data[genderColumn])).ToString() : "";
            if (!gender.IsNullOrEmpty())
                gender = gender.Substring(0, 1);
            if (gender == "N")
                gender = string.Empty;

            var address1 = data[address1Column] != DBNull.Value ? data[address1Column].ToString() : "";
            var address2 = data[address2Column] != DBNull.Value ? data[address2Column].ToString() : "";
            var city = (cityColumn != null && data[cityColumn] != DBNull.Value) ? data[cityColumn].ToString() : "";
            var state = (stateColumn != null && data[stateColumn] != DBNull.Value) ? data[stateColumn].ToString() : "";
            var postalCode = (postalCodeColumn != null && data[postalCodeColumn] != DBNull.Value) ? data[postalCodeColumn].ToString() : "";

            var address = string.Join(" ", address1, address2);
            address = string.Join("\r\n", address, city);
            address = string.Join(",\r\n", address, state);
            address = string.Join(" ", address, postalCode).Trim();
            if (address.Equals(","))
                address = string.Empty;

            var homephone = (homePhoneColumn != null && data[homePhoneColumn] != DBNull.Value) ? data[homePhoneColumn].ToString() : "";
            var mobilephone = (cellPhoneColumn != null && data[cellPhoneColumn] != DBNull.Value) ? data[cellPhoneColumn].ToString() : "";
            var workphone = (workPhoneColumn != null && data[workPhoneColumn] != DBNull.Value) ? data[workPhoneColumn].ToString() : (mainPhoneColumn != null && data[mainPhoneColumn] != DBNull.Value) ? data[mainPhoneColumn].ToString() : "";

            var model = new ContactModel
            {
                showCommissions = false,
                address = address,
                birthdate = birthdate,
                email = email,
                firstName = firstName,
                homephone = homephone,
                lastName = lastName,
                mobilephone = mobilephone,
                workphone = workphone,
            };

            if (accountNumberColumn != null && data[accountNumberColumn] != DBNull.Value)
                model.accountnumber = data[accountNumberColumn].ToString();

            if (accountIDColumn != null && data[accountIDColumn] != DBNull.Value)
                model.accountid = data[accountIDColumn].ToString();

            if (lastPurchaseColumn != null && data[lastPurchaseColumn] != DBNull.Value)
                model.lastpurchasedate = data[lastPurchaseColumn].ToString();

            if (profileHtmlColumn != null && data[profileHtmlColumn] != DBNull.Value)
            {
                try
                {
                    var html = data[profileHtmlColumn].ToString();
                    model.thumb = html.ReplaceXMLFileUploadPathToken();
                }
                catch (Exception)
                {
                    model.thumb = string.Empty;
                }
            }
            else
            {
                switch (gender)
                {
                    case "M":
                        model.thumb = "lib/resources/themes/images/encore/no_photo_male.png";
                        break;
                    case "F":
                        model.thumb = "lib/resources/themes/images/encore/no_photo.png";
                        break;
                    default:
                        model.thumb = "lib/resources/themes/images/encore/no_photo_gen.png";
                        break;
                }
            }

            return model;
        }

        public void SetCommissionsProperties(dynamic downlineNode, ref Downline downline)
        {
            this.showCommissions = true;
            this.pv = (downlineNode.PV > 0 ? downlineNode.PV : 0m).ToString("0.00");
            this.gv = (downlineNode.GV > 0 ? downlineNode.GV : 0m).ToString("0.00");
            var level = string.Empty;
            for (int i = 0; i < downlineNode.Level; i++)
                level += ".";
            this.level = level + downlineNode.Level.ToString();
            this.title = ((downlineNode as dynamic).CurrentTitle as int? != null) ? Create.New<ICommissionsService>().GetTitle(((downlineNode as dynamic).CurrentTitle as int?).ToInt()).TermName : Translation.GetTerm("N/A");

            int? sponsorID = downlineNode.SponsorID as int?;
            if (sponsorID.HasValue && downline.Lookup.ContainsKey(sponsorID.Value))
            {
                var sponsorNode = downline.Lookup[sponsorID.Value];
                this.sponsor = sponsorNode.FirstName.ToString() + " " + sponsorNode.LastName.ToString();
            }
        }
    }
}