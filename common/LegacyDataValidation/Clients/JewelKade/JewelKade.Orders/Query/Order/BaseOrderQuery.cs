using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;

namespace JewelKade.Orders.Query.Order
{
    public abstract class BaseOrderQuery : IRecordQuery
    {
        public BaseOrderQuery()
        {
            if (MaximumBufferRecords == 0)
            {
                MaximumBufferRecords = 50;
            }
        }
        public int MaximumBufferRecords { get; set; }

        public abstract IQueryable GetRecords();

        protected IEnumerable<DbQuery<DataModel.Order>> GetQueries()
        {
            var context = new DataModel.Entities();
            return new DbQuery<DataModel.Order>[]
            {
                context.Orders
                          .Include("OrderCustomers"),
                context.Orders
                          .Include("OrderCustomers.OrderItems")
                          .Include("OrderCustomers.OrderItems.OrderItemPrices")
            };

        }

        public abstract string GetWhereClauseString(string orderAlias);
    }
}
