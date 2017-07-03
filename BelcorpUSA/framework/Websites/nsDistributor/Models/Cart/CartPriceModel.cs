using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace nsDistributor.Models.Cart
{
    public class CartPriceModel
    {
        private InventoryBaseRepository inventory;

        public NetSteps.Data.Entities.OrderItem OrderItem { get; set; }
        public Constants.AccountType AccountType { get; set; }
        public int CurrencyID { get; set; }
        public Product Product { get; set; }


        public CartPriceModel(NetSteps.Data.Entities.OrderItem orderItem, Constants.AccountType accountType, int currencyID, InventoryBaseRepository Inventory)
        {
            OrderItem = orderItem;
            AccountType = accountType;
            CurrencyID = currencyID;
            inventory = Inventory;

            Product = inventory.GetProduct(orderItem.ProductID ?? 0);
        }


    }
}