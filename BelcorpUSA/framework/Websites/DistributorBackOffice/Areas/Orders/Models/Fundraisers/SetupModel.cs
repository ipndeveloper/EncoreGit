using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DistributorBackOffice.Models;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Web.Mvc.Controls.Models.Shared;
using Entities = NetSteps.Data.Entities;

namespace DistributorBackOffice.Areas.Orders.Models.Fundraisers
{
    public class SetupModel
    {
        public SetupModel()
        {
            StartDate = DateTime.Now;
            StartTime = DateTime.Now;

            // Setup Addresses
            HostAddress = SetupAddress();
            ShippingAddress = SetupAddress();
            PartyAddress = SetupAddress();
            
            IsFundraiserAtHosts = true;
            ShipTo = PartyShipTo.Consultant;
            Party = new Entities.Party { UseEvites = true };
        }

        public Entities.Party Party { get; set; }

        [NSRequired, NSDisplayName("FirstName", "First Name")]
        public string FirstName { get; set; }

        [NSRequired, NSDisplayName("LastName", "Last Name")]
        public string LastName { get; set; }

        [NSEmail, NSRequired, NSDisplayName("Email", "Email")]
        public string Email { get; set; }

        [Required]
        public int AccountId { get; set; }

        public PhoneNumber PhoneNumber { get; set; }

        public BasicAddressModel HostAddress { get; set; }

        [Required, NSDisplayName("FundraiserStartDate", "Date"), DataType(DataType.Date), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime StartDate { get; set; }

        [Required, NSDisplayName("FundraiserStartTime", "Time"), DataType(DataType.Time), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:t}")]
        public DateTime StartTime { get; set; }

        public string EvitesOrganizersEmail { get; set; }

        public int? HostAddressLocationId { get; set; }

        public BasicAddressModel PartyAddress { get; set; }

        public int? ShippingAddressId { get; set; }
        public BasicAddressModel ShippingAddress { get; set; }

        public EmailTemplate HostInviteEmailTemplate { get; set; }

        public OrderCustomer Host { get; set; }

        public NetSteps.Data.Entities.Account HostAccount { get; set; }

        public PartyShipTo ShipTo { get; set; }

        public bool IsFundraiserAtHosts { get; set; }

        BasicAddressModel SetupAddress()
        {
            return new BasicAddressModel() { CountryID = (int)Constants.Country.UnitedStates };
        }
    }

    public class PhoneNumber
    {
        public PhoneNumber()
        {

        }

        public PhoneNumber(string phoneNumber)
        {
            string cleanPhone = phoneNumber.PhoneFormat(NetSteps.Common.Extensions.StringExtensions.PhoneFormats.OnlyNumbers);

            if (phoneNumber.Length >= 3)
            {
                this.AreaCode = cleanPhone.Substring(0, 3);
            }

            if (phoneNumber.Length >= 6)
            {
                this.FirstThree = cleanPhone.Substring(3, 3);
            }

            if (phoneNumber.Length >= 7)
            {
                this.LastFour = cleanPhone.Substring(6);
            }
        }

        [NSRequired, NSStringLength(3, MinimumLength = 3), NSDisplayName("PhoneNumber", "Phone Number")]
        public string AreaCode { get; set; }

        [NSRequired, NSStringLength(3), NSDisplayName("PhoneNumber", "Phone Number")]
        public string FirstThree { get; set; }

        [NSRequired, NSStringLength(4), NSDisplayName("PhoneNumber", "Phone Number")]
        public string LastFour { get; set; }

        public override string ToString()
        {

            return string.Format("({0}) {1}-{2}", AreaCode, FirstThree, LastFour);
        }
    }
}