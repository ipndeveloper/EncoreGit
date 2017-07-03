using NetSteps.Addresses.Common.Models;
using NetSteps.Commissions.Common.Models;
using NetSteps.Data.Entities;

public class DisbursementProfileEFTAccountInfoEditModel
{
    public DisbursementProfileEFTAccountInfoEditModel(IEFTDisbursementProfile disbursementProfile)
    {
        DisbursementProfile = disbursementProfile;
        BankAddress = disbursementProfile.AddressId != 0 ? Address.LoadFull(disbursementProfile.AddressId) : new Address();
    }

    public IEFTDisbursementProfile DisbursementProfile { get; private set; }

    public IAddress BankAddress { get; private set; }
}