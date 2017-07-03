using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class EFT
    {
        List<EFTProfile> _profiles = new List<EFTProfile>();

        public EFT()
        {
        }

        public EFT(EFTProfile profile)
        {
            AddProfile(profile);
        }

        public int AddProfile(EFTProfile profile)
        {
            int index = _profiles.Count;
            _profiles.Add(profile);
            return index;
        }

        public EFTProfile GetProfile(int index)
        {
            return _profiles[index];
        }
    }
}
