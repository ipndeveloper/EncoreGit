using System;
using System.ComponentModel;
using System.Linq;

namespace NetSteps.Data.Entities.Commissions
{
    [Serializable]
    public abstract class BaseProfile
    {
        protected DisbursementProfile _parentProfile;
        protected NetSteps.Data.Entities.Constants.DisbursementProfileType _type;

				public BaseProfile(DisbursementProfile profile, NetSteps.Data.Entities.Constants.DisbursementProfileType type)
        {
            _parentProfile = profile;
            _type = type;
        }
    }
}
