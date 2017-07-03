
namespace NetSteps.Data.Entities
{
    public partial class Redemption
    {
        public string CouponUrl { get; set; }
        public static Redemption Load(string redemptionNumber)
        {
            return Repository.Load(redemptionNumber);
        }
    }
}
