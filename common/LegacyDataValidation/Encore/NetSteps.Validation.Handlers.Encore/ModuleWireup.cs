using System;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Validation.BatchProcess.Common;
using NetSteps.Validation.Common;
using NetSteps.Validation.Handlers.Common.Services;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Core;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.Services;
using NetSteps.Validation.Handlers.Services.ServiceModels;

[assembly: Wireup(typeof(NetSteps.Validation.Handlers.ModuleWireup))]

namespace NetSteps.Validation.Handlers
{
    [WireupDependency(typeof(NetSteps.Validation.Common.ModuleWireup))]
    public class ModuleWireup : WireupCommand
    {
        public ModuleWireup()
        {
        }

        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Container.Root.ForType<IProductPriceType>()
                     .Register<ProductPriceType>()
                     .ResolveAnInstancePerRequest()
                     .End();

            Func<IProductPriceType> priceTypeConstructor = () => Create.New<IProductPriceType>();

            Container.Root.ForType<IPriceTypeService>()
                     .Register<PriceTypeService>(Param.Value(priceTypeConstructor))
                     .ResolveAsSingleton()
                     .End();

            Container.Root.ForType<IOrderType>()
                     .Register<OrderType>()
                     .ResolveAnInstancePerRequest()
                     .End();

            Func<IOrderType> orderTypeConstructor = () => Create.New<IOrderType>();

            Container.Root.ForType<IOrderTypeService>()
                     .Register<OrderTypeService>(Param.Value(orderTypeConstructor))
                     .ResolveAsSingleton()
                     .End();

            Container.Root.ForType<IOrderItemPricingService>()
                     .Register<OrderItemPricingService>()
                     .ResolveAsSingleton()
                     .End();

            Container.Root.ForType<IPaymentService>()
                     .Register<PaymentService>()
                     .ResolveAsSingleton()
                     .End();

            Container.Root.ForType<IOrderCommissionService>()
                     .Register<OrderCommissionService>()
                     .ResolveAsSingleton()
                     .End();

            #region Order
            RegisterPropertyCalculator<Order.Order_ParentOrderID_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.Order, EncoreFieldNames.Order.ParentOrderID);
            RegisterPropertyCalculator<Order.Order_CommissionableTotal_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.Order, EncoreFieldNames.Order.CommissionableTotal);
            RegisterPropertyCalculator<Order.Order_Subtotal_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.Order, EncoreFieldNames.Order.Subtotal);
            RegisterPropertyCalculator<Order.Order_GrandTotal_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.Order, EncoreFieldNames.Order.GrandTotal);
            #endregion

            #region OrderCustomer
            RegisterPropertyCalculator<OrderCustomer.OrderCustomer_CommissionableTotal_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.OrderCustomer, EncoreFieldNames.OrderCustomer.CommissionableTotal);
            RegisterPropertyCalculator<OrderCustomer.OrderCustomer_Subtotal_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.OrderCustomer, EncoreFieldNames.OrderCustomer.Subtotal);
            RegisterPropertyCalculator<OrderCustomer.OrderCustomer_Total_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.OrderCustomer, EncoreFieldNames.OrderCustomer.Total);
            #endregion

            #region OrderItem
            RegisterPropertyCalculator<OrderItem.OrderItem_ItemPrice_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.OrderItem, EncoreFieldNames.OrderItem.ItemPrice);
            RegisterPropertyCalculator<OrderItem.OrderItem_CommissionableTotal_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.OrderItem, EncoreFieldNames.OrderItem.CommissionableTotal);
            RegisterPropertyCalculator<OrderItem.OrderItem_ItemPriceActual_ValidationHandler>
                            (EncoreFieldNames.TableSingularNames.OrderItem, EncoreFieldNames.OrderItem.ItemPriceActual);
            #endregion 

            #region OrderItemPrice
            RegisterPropertyCalculator<OrderItemPrice.OrderItemPrice_OriginalUnitPrice_ValidationHandler>
                (EncoreFieldNames.TableSingularNames.OrderItemPrice, EncoreFieldNames.OrderItemPrice.OriginalUnitPrice);
            RegisterPropertyCalculator<OrderItemPrice.OrderItemPrice_UnitPrice_ValidationHandler>
                (EncoreFieldNames.TableSingularNames.OrderItemPrice, EncoreFieldNames.OrderItemPrice.UnitPrice);
            #endregion
        }

        private void RegisterPropertyCalculator<T>(string recordKind, string propertyName) where T : BaseRecordPropertyCalculationHandler, IRecordPropertyCalculationHandler
        {
            Container.Root.ForType<IRecordPropertyCalculationHandler>()
                     .RegisterWithName<T>(string.Format("{0}.{1}", recordKind, propertyName), Param.Resolve<IRecordPropertyCalculationHandlerResolver>())
                     .ResolveAnInstancePerRequest()
                     .End();
        }
    }
}
