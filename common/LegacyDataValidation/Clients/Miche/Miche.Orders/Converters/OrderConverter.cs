using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miche.Orders.DataModel;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Conversion.Core;
using NetSteps.Validation.Handlers;
using NetSteps.Validation.Handlers.Helpers;
using NetSteps.Validation.Handlers.Common.Services;


namespace Miche.Orders.Converters
{
    public class OrderConverter : BaseRecordConverter<Order>
    {
        public OrderConverter(Func<string, IRecordConverter> converterFactory)
            : base(converterFactory)
        {

        }

        protected override string GetKeyFieldName()
        {
            return "OrderID";
        }

        protected override IDictionary<string, string> GetRecordCommentProperties(IRecord record)
        {
            var orderTypeService = ServiceLocator.FindService<IOrderTypeService>();
            var result = new Dictionary<string, string>();
            result.Add("OrderType", orderTypeService.GetOrderType((Int16)record.Properties[EncoreFieldNames.Order.OrderTypeID].OriginalValue).Name);
            result.Add(EncoreFieldNames.Order.CurrencyID, record.Properties[EncoreFieldNames.Order.CurrencyID].OriginalValue.ToString());
            result.Add(EncoreFieldNames.Order.GrandTotal, record.Properties[EncoreFieldNames.Order.GrandTotal].OriginalValue.ToString());
            result.Add(EncoreFieldNames.Order.CommissionDateUTC, record.Properties[EncoreFieldNames.Order.CommissionDateUTC].OriginalValue == null ? String.Empty : ((DateTime)record.Properties[EncoreFieldNames.Order.CommissionDateUTC].OriginalValue).ToString("yyyy-MM-dd"));
            return result;
        }

        protected override RecordPropertyRole GetRecordPropertyRole(string propertyName)
        {
            switch (propertyName)
            {
                default:
                    return RecordPropertyRole.Fact;
            }
        }

        private const string schemaName = "dbo";
        private const string tableName = "Orders";

        protected override string SchemaName
        {
            get { return schemaName; }
        }

        protected override string TableName
        {
            get { return tableName; }
        }
    }
}
