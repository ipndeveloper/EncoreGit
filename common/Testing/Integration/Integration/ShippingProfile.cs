

namespace NetSteps.Testing.Integration
{
    public class ShippingProfile
    {
        public ShippingProfile()
        {
        }

        public ShippingProfile(Address address)
        {
            Address = address;
        }

        public ShippingProfile(string profileName, Address address)
            : this(address)
        {
            ProfileName = profileName;
        }

        public string ProfileName
        { get; set; }

        public Address Address
        { get; set; }
    }
}
