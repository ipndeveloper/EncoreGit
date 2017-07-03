using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class Billing
    {
        List<BillingProfile> _billingProfiles = new List<BillingProfile>();

        public Billing(BillingProfile profile)
        {
            AddProfile(profile);
        }

        public int ProfileCount
        {
            get { return _billingProfiles.Count; }
        }

        public int AddProfile(BillingProfile profile)
        {
            int index = _billingProfiles.Count;
            _billingProfiles.Add(profile);
            return index;
        }

        public BillingProfile GetProfile(int index)
        {
            return _billingProfiles[index];
        }

        public List<BillingProfile> GetProfiles()
        {
            return _billingProfiles;
        }
    }
}
