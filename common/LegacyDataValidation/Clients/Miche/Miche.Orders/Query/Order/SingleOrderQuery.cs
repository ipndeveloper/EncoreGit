using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;

namespace Miche.Orders.Query.Order
{
    public class SingleOrderQuery : BaseOrderQuery
    {
        public SingleOrderQuery(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; private set; }

        public override IQueryable GetRecords()
        {
            var queries = GetQueries();
            IQueryable result = null;
            foreach (var query in queries)
            {
                result =
                    (
                        from o in query
                        where o.OrderID == OrderId
                        select o
                    );
            }
            return result;
        }

        public override string GetWhereClauseString(string orderAlias)
        {
            return String.Format("{0}.OrderID = {1}", orderAlias, OrderId);
        }
    }
}
