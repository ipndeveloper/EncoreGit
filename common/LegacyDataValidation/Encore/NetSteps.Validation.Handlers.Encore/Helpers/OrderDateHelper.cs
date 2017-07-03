using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Handlers.Helpers
{
    public static class OrderDateHelper
    {
        public static DateTime GetOrderDate(IRecord orderRecord, out string orderFieldUsed)
        {
            Contract.Assert(orderRecord != null);
            Contract.Assert(orderRecord.RecordKind.Equals(EncoreFieldNames.TableSingularNames.Order));

            // find the order date
            var orderDate = (DateTime?)orderRecord.Properties[EncoreFieldNames.Order.CommissionDateUTC].OriginalValue;
            if (orderDate != null)
            {
                orderFieldUsed = EncoreFieldNames.Order.CommissionDateUTC;
                return orderDate.Value;
            }
            orderDate = (DateTime?)orderRecord.Properties[EncoreFieldNames.Order.CompleteDateUTC].OriginalValue;
            if (orderDate != null)
            {
                orderFieldUsed = EncoreFieldNames.Order.CompleteDateUTC;
                return orderDate.Value;
            }
            orderDate = (DateTime)orderRecord.Properties[EncoreFieldNames.Order.DateCreatedUTC].OriginalValue;
            if (orderDate != null)
            {
                orderFieldUsed = EncoreFieldNames.Order.DateCreatedUTC;
                return orderDate.Value;
            }
            orderFieldUsed = DateTimeNow;
            return DateTime.Now;
        }

        public const string DateTimeNow = "DateTime.Now";
    }

}
