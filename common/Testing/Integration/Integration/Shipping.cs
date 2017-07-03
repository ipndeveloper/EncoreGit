using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class Shipping
    {
        private List<ShippingProfile> _profiles = new List<ShippingProfile>();

        public Shipping(ShippingProfile profile)
        {
            AddProfile(profile);
        }

        public int ProfileCount
        {
            get { return _profiles.Count; }
        }

        public int AddProfile(ShippingProfile profile)
        {
            int index = _profiles.Count;
            _profiles.Add(profile);
            return index;
        }

        public ShippingProfile GetProfile(int index)
        {
            return _profiles[index];
        }
    }
}
