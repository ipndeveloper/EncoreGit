using NetSteps.Addresses.Common.Models;

namespace NetSteps.Data.Entities.Business.HelperObjects.AddressDisplay
{
    public class AddressDisplayModel
    {
        public bool ShowCountry { get; set; }
        public bool ShowPhone { get; set; }
        public string Delimiter { get; set; }
        public bool ShowName { get; set; }
        public Country Country { get; set; }
        public IAddress Address { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public bool ShowProfileName { get; set; }
        public bool ShowShipToEmail { get; set; }
        public string ShipToEmailAddress { get; set; }
    }
}
