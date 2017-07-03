using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miche.Orders.DataModel;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Conversion.Core;
using NetSteps.Validation.Handlers;

namespace Miche.Orders.Converters
{
    public class OrderItemPriceConverter : BaseRecordConverter<OrderItemPrice>
    {
        public OrderItemPriceConverter(Func<string, IRecordConverter> converterFactory)
            : base(converterFactory)
        {
            
        }

        protected override string GetKeyFieldName()
        {
            return "OrderItemPriceID";
        }

        protected override RecordPropertyRole GetRecordPropertyRole(string propertyName)
        {
            switch (propertyName)
            {
                case EncoreFieldNames.OrderItemPrice.OrderItemID:
                    return RecordPropertyRole.ForeignKey;
                case EncoreFieldNames.OrderItemPrice.OrderItemPriceID:
                    return RecordPropertyRole.PrimaryKey;
                case EncoreFieldNames.OrderItemPrice.OriginalUnitPrice:
                case EncoreFieldNames.OrderItemPrice.UnitPrice:
                    return RecordPropertyRole.ValidatedField;
                default:
                    return RecordPropertyRole.Fact;
            }
        }

        private const string schemaName = "dbo";
        private const string tableName = "OrderItemPrices";

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
