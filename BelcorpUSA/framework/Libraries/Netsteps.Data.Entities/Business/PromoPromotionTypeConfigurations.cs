namespace NetSteps.Data.Entities.Business
{
    public class PromoPromotionTypeConfigurations
    {
        public int PromotionTypeConfigurationID { get; set; }
        public int PromotionTypeID { get; set; }
        public bool Active { get; set; }
        public bool IncludeBAorders { get; set; }
    }
}
