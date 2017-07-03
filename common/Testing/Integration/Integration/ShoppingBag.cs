using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class ShoppingBag
    {
        private List<OrderItem> _list = new List<OrderItem>();

        public ShoppingBag(OrderItem orderItem)
        {
            _list.Add(orderItem);
        }

        public ShoppingBag(List<OrderItem> orderItems)
        {
            foreach (OrderItem orderItem in orderItems)
                _list.Add(orderItem);
        }

        public decimal Tax
        { get; set; }

        public decimal Shipping
        { get; set; }

        public decimal Handling
        { get; set; }

        public decimal Balance
        { get; set; }

        public int AddOrderItem(Product product, ushort quantity)
        {
            int index = _list.Count;
            _list.Add(new OrderItem(product, quantity));
            return index;
        }

        public bool DeleteOrderItem(int index)
        {
            bool removed = index < _list.Count;
            _list.RemoveAt(index);
            return removed;
        }

        public int OrderItemCount
        {
            get { return _list.Count; }
        }

        public OrderItem GetOrderItem(int index)
        {
            return _list[index];
        }

        public List<OrderItem> GetAllOrderItems()
        {
            return _list;
        }
    }
}
