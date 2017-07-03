using System;
using NetSteps.Addresses.UI.Common.Models;
using NetSteps.Data.Entities;

namespace DistributorBackOffice.Models
{
    public class PartySetupModel
    {
        public HostInformation Host { get; set; }

        public int? PartyID { get; set; }
        public string PartyName { get; set; }
        public DateTime PartyDate { get; set; }
        public DateTime PartyTime { get; set; }
        public bool ListOnPWS { get; set; }
        public int? ParentPartyID { get; set; }
        public bool IsBooking { get; set; }

        public bool UseEvites { get; set; }
        public string EvitesEmail { get; set; }
        public string PersonalizedContent { get; set; }

        public bool PartyIsAtHosts { get; set; }
        public bool PartyIsAtConsultants { get; set; }
        public Address PartyAddress { get; set; }

        public PartyShipTo ShipTo { get; set; }
        public int? ShipToAddressID { get; set; }
        public Address ShippingAddress { get; set; }
    }

    public class HostInformation
    {
        public int? AccountID { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public IAddressUIModel AddressNew { get; set; }
    }

    public enum PartyShipTo
    {
        Host,
        Consultant,
        Other
    }

    public enum PartyLocation
    {
        Host,
        Consultant,
        Other
    }
}