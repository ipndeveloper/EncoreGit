namespace NetSteps.Data.Entities.Dto
{
    public class ProductPriceTypeDto
    {
        public int ProductPriceTypeID { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public bool Editable { get; set; }
        public bool Mandatory { get; set; }
    }
}
