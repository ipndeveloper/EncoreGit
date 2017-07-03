using NetSteps.Validation.Handlers.Common.Services;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Handlers.Services
{
    public class OrderTypeService : IOrderTypeService
    {
        protected readonly IList<IOrderType> OrderTypeDictionary;

        protected readonly Func<IOrderType> PriceTypeConstructor;

        public OrderTypeService(Func<IOrderType> priceTypeConstructor)
        {
            OrderTypeDictionary = new List<IOrderType>();
            PriceTypeConstructor = priceTypeConstructor;

            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 1, Name = "Online Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 2, Name = "Workstation Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID =3, Name = "Party Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 4, Name = "Portal Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 5, Name = "Autoship Template" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 6, Name = "Autoship Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 7, Name = "Override Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 8, Name = "Return Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 9, Name = "Comp Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 10, Name = "Replacement Order" });
            OrderTypeDictionary.Add(new OrderType() { OrderTypeID = 11, Name = "Enrollment Order" });
        }

        public Common.Services.ServiceModels.IOrderType GetOrderType(int orderTypeId)
        {
            return OrderTypeDictionary.SingleOrDefault(x => x.OrderTypeID == orderTypeId);
        }

        public Common.Services.ServiceModels.IOrderType GetOrderType(string orderTypeName)
        {
            return OrderTypeDictionary.SingleOrDefault(x => x.Name.Equals(orderTypeName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Common.Services.ServiceModels.IOrderType> GetAllOrderTypes()
        {
            return OrderTypeDictionary.ToArray();
        }
    }
}
