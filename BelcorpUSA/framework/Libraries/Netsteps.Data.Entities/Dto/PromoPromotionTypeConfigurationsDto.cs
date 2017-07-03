namespace NetSteps.Data.Entities.Dto
{
    public class PromoPromotionTypeConfigurationsDto
    {
        public int PromotionTypeConfigurationID { get; set; }
        public int PromotionTypeID { get; set; }
        public bool Active { get; set; }
        public bool IncludeBAorders { get; set; }
    }
}
