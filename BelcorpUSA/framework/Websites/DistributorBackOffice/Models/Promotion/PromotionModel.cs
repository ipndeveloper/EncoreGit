using NetSteps.Promotions.UI.Common.Interfaces;

namespace DistributorBackOffice.Models.Promotion
{
    public class PromotionModel
    {
        public IDisplayInfo Info { get; private set; }
        public string ExpirationDateVal { get; private set; }
        
        public PromotionModel(IDisplayInfo info)
        {
            Info = info;
            ExpirationDateVal = string.Empty;
            if (info.ExpiredDate.HasValue)
            {
                ExpirationDateVal = info.ExpiredDate.Value
                    .ToLocalTime()
                    .ToString("d", info.FormatProvider);
            }
        }
    }
}