using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JewelKade.Orders.DataModel;
using NetSteps.Validation.Conversion.Core;
using NetSteps.Validation.Handlers.Helpers;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers;
using NetSteps.Validation.Conversion.Core.Model;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace JewelKade.Orders.Converters
{
    public class OrderItemConverter : BaseRecordConverter<OrderItem>
    {
        public OrderItemConverter(Func<string, IRecordConverter> converterFactory)
            : base(converterFactory)
        {
            
        }

        protected override string GetKeyFieldName()
        {
            return "OrderItemID";
        }

        public override IRecord Convert(object originalObject, IRecord parent)
        {
            var converted = base.Convert(originalObject, parent);
            EnsureOrderItemPrices(converted);
            return converted;
        }

        protected override IDictionary<string, string> GetRecordCommentProperties(IRecord record)
        {
            var result = new Dictionary<string, string>();
            result.Add(EncoreFieldNames.OrderItem.SKU, record.Properties[EncoreFieldNames.OrderItem.SKU].OriginalValue.ToString());
            result.Add(EncoreFieldNames.OrderItem.Quantity, record.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue.ToString());
            return result;
        }

        private void EnsureOrderItemPrices(IRecord converted)
        {
            var orderRecord = converted.Parent.Parent;

            var priceTypeService = ServiceLocator.FindService<IPriceTypeService>();
            var pricingService = ServiceLocator.FindService<IOrderItemPricingService>();

            var priceTypes = priceTypeService.GetAllPriceTypes();
            foreach (var priceType in priceTypes)
            {
                var found = converted.ChildRecords.SingleOrDefault(x => x.RecordKind == EncoreFieldNames.TableSingularNames.OrderItemPrice &&
                    priceType.PriceTypeID == (int)x.Properties[EncoreFieldNames.OrderItemPrice.ProductPriceTypeID].OriginalValue);
                if (found == null)
                {
                    var newOrderItemPrice = new Record(converted, "dbo", "OrderItemPrices");
                    newOrderItemPrice.RecordIdentityField = EncoreFieldNames.OrderItemPrice.OrderItemPriceID;
                    newOrderItemPrice.RecordKind = EncoreFieldNames.TableSingularNames.OrderItemPrice;

                    var orderItemIDProperty = new RecordProperty(EncoreFieldNames.OrderItemPrice.OrderItemID, converted.RecordIdentity, newOrderItemPrice, RecordPropertyRole.ForeignKey, typeof(int));
                    orderItemIDProperty.SetResult(ValidationResultKind.IsNew);
                    newOrderItemPrice.Properties.Add(EncoreFieldNames.OrderItemPrice.OrderItemID, orderItemIDProperty);

                    decimal originalPrice = 0M;
                    string orderDateFieldUsed;
                    var foundPrice = pricingService.GetHistoricalPrice(
                        (int)converted.Properties[EncoreFieldNames.OrderItem.ProductID].OriginalValue,
                        OrderDateHelper.GetOrderDate(orderRecord, out orderDateFieldUsed),
                        priceType.PriceTypeID,
                        (int)orderRecord.Properties[EncoreFieldNames.Order.CurrencyID].OriginalValue,
                        out originalPrice);

                    if (!foundPrice)
                    {
                        newOrderItemPrice.SetResult(newOrderItemPrice.Result.EscalateTo(ValidationResultKind.IsWithinMarginOfError));
                        newOrderItemPrice.AddValidationComment(ValidationCommentKind.Warning, String.Format("No price history for price type {0} when creating missing OrderItemPrice record.", priceType.Name));
                    }
                    if (pricingService.ShouldMultiplyOrderItemPricesByQuantity)
                    {
                        originalPrice *= (int)converted.Properties[EncoreFieldNames.OrderItem.Quantity].OriginalValue;
                    }

                    var originalUnitPriceProperty = new RecordProperty(EncoreFieldNames.OrderItemPrice.OriginalUnitPrice, originalPrice, newOrderItemPrice, RecordPropertyRole.ValidatedField, typeof(decimal?));
                    originalUnitPriceProperty.SetResult(ValidationResultKind.IsNew);
                    newOrderItemPrice.Properties.Add(EncoreFieldNames.OrderItemPrice.OriginalUnitPrice, originalUnitPriceProperty);

                    var unitPriceProperty = new RecordProperty(EncoreFieldNames.OrderItemPrice.UnitPrice, 0M, newOrderItemPrice, RecordPropertyRole.ValidatedField, typeof(decimal?));
                    unitPriceProperty.SetResult(ValidationResultKind.IsNew);
                    newOrderItemPrice.Properties.Add(EncoreFieldNames.OrderItemPrice.UnitPrice, unitPriceProperty);

                    var productPriceTypeIDProperty = new RecordProperty(EncoreFieldNames.OrderItemPrice.ProductPriceTypeID, priceType.PriceTypeID, newOrderItemPrice, RecordPropertyRole.Fact, typeof(int));
                    productPriceTypeIDProperty.SetResult(ValidationResultKind.IsNew);
                    newOrderItemPrice.Properties.Add(EncoreFieldNames.OrderItemPrice.ProductPriceTypeID, productPriceTypeIDProperty);

                    newOrderItemPrice.SetResult(ValidationResultKind.IsNew);
                    
                    newOrderItemPrice.AddValidationComment(ValidationCommentKind.CalculationComment, String.Format("Missing OrderItemPrice for price type {0}", priceType.Name));
                    
                    converted.ChildRecords.Add(newOrderItemPrice);
                }
            }
        }

        protected override RecordPropertyRole GetRecordPropertyRole(string propertyName)
        {
            switch (propertyName)
            {
                case EncoreFieldNames.OrderItem.OrderItemID:
                    return RecordPropertyRole.PrimaryKey;
                case EncoreFieldNames.OrderItem.OrderCustomerID:
                    return RecordPropertyRole.ForeignKey;
                case EncoreFieldNames.OrderItem.ItemPrice:
                case EncoreFieldNames.OrderItem.CommissionableTotal:
                case EncoreFieldNames.OrderItem.AdjustedPrice:
                    return RecordPropertyRole.ValidatedField;
                default:
                    return RecordPropertyRole.Fact;
            }
        }

        private const string schemaName = "dbo";
        private const string tableName = "OrderItems";

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
