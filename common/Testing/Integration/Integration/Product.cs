

namespace NetSteps.Testing.Integration
{
    public class Product
    {
        public Product(string sku, string name = null, decimal? price = null, decimal? discountPrice = null)
        {
            SKU = sku;
            Name = name;
            Price = price;
            DiscountPrice = discountPrice;
        }

        public string SKU
        { get; set; }

        public string Name
        { get; set; }

        public decimal? Price
        { get; set; }

        public decimal? DiscountPrice
        { get; set; }
    }
}
