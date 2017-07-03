using System;
using JewelKade.Orders;
using JewelKade.Orders.Converters;
using JewelKade.Orders.DataModel;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Validation.Common;
using NetSteps.Validation.Conversion.Core;
using NetSteps.Validation.Handlers.Common.Services.ServiceModels;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using JewelKade.Orders.Services;
using NetSteps.Validation.Handlers.Core;
using NetSteps.Validation.Handlers;

[assembly: Wireup(typeof (JewelKade.Orders.ModuleWireup))]

namespace JewelKade.Orders
{
    public class ModuleWireup : WireupCommand
    {
        protected override void PerformWireup(IWireupCoordinator coordinator)
        {
            Func<IProductPriceType> priceTypeFactory = () => Create.New<IProductPriceType>();

            Container.Root.ForType<IPriceTypeService>()
                     .Register<JewelKadePriceTypeService>(Param.Value(priceTypeFactory))
                     .ResolveAsSingleton()
                     .End();

            Container.Root.ForType<IOrderItemPricingService>()
                     .Register<JewelKadeOrderItemPricingService>()
                     .ResolveAsSingleton()
                     .End();

            Func<string, IRecordConverter> converterFactory = Create.NewNamed<IRecordConverter>;

            RegisterConverter<Order, OrderConverter, NonValidatingHandler>(converterFactory);
            RegisterConverter<OrderCustomer, OrderCustomerConverter, NonValidatingHandler>(converterFactory);
            RegisterConverter<OrderItem, OrderItemConverter, NonValidatingHandler>(converterFactory);
            RegisterConverter<OrderItemPrice, OrderItemPriceConverter, NonValidatingHandler>(converterFactory);

            #region OrderItemPrice
            RegisterPropertyCalculator<ValidationHandlers.OrderItem.JewelKade_OrderItemPrice_UnitPrice_ValidationHandler>
                (EncoreFieldNames.TableSingularNames.OrderItemPrice, EncoreFieldNames.OrderItemPrice.UnitPrice);
            #endregion
        }

        private void RegisterConverter<TEntity, TConverter, TValidator>(Func<string, IRecordConverter> converterFactory)
            where TConverter : IRecordConverter<TEntity>
            where TValidator : IRecordPropertyCalculationHandler
        {
            Container.Root.ForType<IRecordConverter<TEntity>>()
                     .Register<TConverter>(Param.Value(converterFactory))
                     .ResolveAsSingleton()
                     .End();

            Container.Root.ForType<IRecordConverter>()
                     .RegisterWithName<TConverter>(typeof (TEntity).FullName, Param.Value(converterFactory))
                     .ResolveAsSingleton()
                     .End();
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