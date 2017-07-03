

namespace NetSteps.Testing.Integration
{
    public class OrderItem
    {
        public OrderItem(Product product, ushort quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product
        { get; set; }

        public ushort Quantity
        { get; set; }
    }
}
