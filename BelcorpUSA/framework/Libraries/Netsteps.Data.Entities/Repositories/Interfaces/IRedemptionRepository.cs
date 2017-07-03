
namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IRedemptionRepository
    {
        Redemption Load(string redemptionNumber);
    }
}
