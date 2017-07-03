using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Miche.Orders.DataModel;
using NetSteps.Validation.Common.Model;
using NetSteps.Validation.Conversion.Core;

namespace Miche.Orders.Converters
{
    public class OrderAdjustmentOrderModificationConverter : BaseRecordConverter<OrderAdjustmentOrderModification>
    {
        public OrderAdjustmentOrderModificationConverter(Func<string, IRecordConverter> converterFactory)
            : base(converterFactory)
        {
            
        }

        protected override string GetKeyFieldName()
        {
            return "OrderAdjustmentOrderModificationID";
        }

        protected override RecordPropertyRole GetRecordPropertyRole(string propertyName)
        {
            switch (propertyName)
            {
                default:
                    return RecordPropertyRole.Fact;
            }
        }

        private const string schemaName = "Order";
        private const string tableName = "OrderAdjustmentOrderModifications";

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
