using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public static class MockOrderAdjustmentCreator
    {
         public static MockOrderAdjustmentProfile Generate(MockAdjustmentTypes types, int productID, int accountID)
        {
            MockOrderAdjustmentProfile adjustment = new MockOrderAdjustmentProfile();
            adjustment.ExtensionProviderKey = MockOrderAdjustmentProvider.ProviderKey;
            adjustment.Description = types.ToString();
            adjustment.MockOrderAdjustmentProfileID = new Random().Next();
            adjustment.AffectedAccountIDs.Add(accountID);

            if ((types & MockAdjustmentTypes.AddedItem) == MockAdjustmentTypes.AddedItem)
                adjustment.OrderLineModificationTargets.Add(GetAddedItem1Adjustment(productID, accountID));
            if ((types & MockAdjustmentTypes.AddedItemWithQuantity2) == MockAdjustmentTypes.AddedItemWithQuantity2)
                adjustment.OrderLineModificationTargets.Add(GetAddedItem1WithQuantity2Adjustment(productID, accountID));
            if ((types & MockAdjustmentTypes.ReducedProduct1PriceBy14Percent) == MockAdjustmentTypes.ReducedProduct1PriceBy14Percent)
                adjustment.OrderLineModificationTargets.Add(GetReducedProduct1PriceBy14PercentAdjustment(productID, accountID));
            if ((types & MockAdjustmentTypes.ReducedProduct1PriceBy16Flat) == MockAdjustmentTypes.ReducedProduct1PriceBy16Flat)
                adjustment.OrderLineModificationTargets.Add(GetReducedProduct1PriceBy16FlatAdjustment(productID, accountID));
            if ((types & MockAdjustmentTypes.ReducedShippingTotalBy10Flat) == MockAdjustmentTypes.ReducedShippingTotalBy10Flat)
                adjustment.OrderModifications.Add(GetReducedShippingTotalBy10FlatAdjustment(productID, accountID));
            if ((types & MockAdjustmentTypes.ReducedShippingTotalBy20Percent) == MockAdjustmentTypes.ReducedShippingTotalBy20Percent)
                adjustment.OrderModifications.Add(GetReducedShippingTotalBy20PercentAdjustment(productID, accountID));
            if ((types & MockAdjustmentTypes.ReducedSingleProduct1PriceBy23Percent) == MockAdjustmentTypes.ReducedSingleProduct1PriceBy23Percent)
                adjustment.OrderLineModificationTargets.Add(GetReducedSingleProduct1PriceBy23PercentAdjustment(productID, accountID));
            if ((types & MockAdjustmentTypes.ReducedSingleProduct1PriceBy24Flat) == MockAdjustmentTypes.ReducedSingleProduct1PriceBy24Flat)
                adjustment.OrderLineModificationTargets.Add(GetReducedSingleProduct1PriceBy24FlatAdjustment(productID, accountID));
            return adjustment;
        }

        public static IEnumerable<MockOrderAdjustmentProfile> GenerateOneOfEach(int productID, int accountID)
        {
            List<MockOrderAdjustmentProfile> adjustments = new List<MockOrderAdjustmentProfile>();
            adjustments.Add(Generate(MockAdjustmentTypes.AddedItem, productID, accountID));
            adjustments.Add(Generate(MockAdjustmentTypes.AddedItemWithQuantity2, productID, accountID));
            adjustments.Add(Generate(MockAdjustmentTypes.ReducedProduct1PriceBy14Percent, productID, accountID));
            adjustments.Add(Generate(MockAdjustmentTypes.ReducedProduct1PriceBy16Flat, productID, accountID));
            adjustments.Add(Generate(MockAdjustmentTypes.ReducedShippingTotalBy10Flat, productID, accountID));
            adjustments.Add(Generate(MockAdjustmentTypes.ReducedShippingTotalBy20Percent, productID, accountID));
            adjustments.Add(Generate(MockAdjustmentTypes.ReducedSingleProduct1PriceBy23Percent, productID, accountID));
            adjustments.Add(Generate(MockAdjustmentTypes.ReducedSingleProduct1PriceBy24Flat, productID, accountID));
            return adjustments;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetAddedItem1Adjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Added 1 item.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
            target.ProductID = ProductId;
            mod.Property = "Quantity";
            target.Quantity = 1;
            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetAddedItem1WithQuantity2Adjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Added 2 items.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
            target.ProductID = ProductId;
            mod.Property = "Quantity";
            target.Quantity = 2;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderModification GetReducedShippingTotalBy20PercentAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderModification mod = Create.New<IOrderAdjustmentProfileOrderModification>();
            mod.Description = "Reduced shipping total by 20 percent.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderOperationKind.Multiplier;
            mod.Property = "ShippingTotal";
            mod.ModificationValue = .20M;

            return mod;
        }

        private static IOrderAdjustmentProfileOrderModification GetReducedShippingTotalBy10FlatAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderModification mod = Create.New<IOrderAdjustmentProfileOrderModification>();
            mod.Description = "Reduced shipping total by 10 flat.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderOperationKind.FlatAmount;
            mod.Property = "ShippingTotal";
            mod.ModificationValue = 10M;

            return mod;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedProduct1PriceBy14PercentAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item ProductID 1 Price by 14%.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
            target.ProductID = ProductId;
            mod.Property = "ItemPrice";
            mod.ModificationValue = .14M;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedProduct1PriceBy16FlatAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item ProductID 1 Price by 16.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.FlatAmount;
            target.ProductID = ProductId;
            mod.Property = "ItemPrice";
            mod.ModificationValue = 16M;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedProduct1VolumeBy13PercentAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item Product 1 Volume by 13%.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
            target.ProductID = ProductId;
            mod.Property = "Volume";
            mod.ModificationValue = .13M;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedProduct1VolumeBy18FlatAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item Product 1 Volume by 18 Flat.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.FlatAmount;
            target.ProductID = ProductId;
            mod.Property = "Volume";
            mod.ModificationValue = 18M;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedSingleProduct1PriceBy23PercentAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item Product 1 Price by 23%.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
            target.ProductID = ProductId;
            mod.Property = "ItemPrice";
            mod.ModificationValue = .23M;
            target.Quantity = 1;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedSingleProduct1PriceBy24FlatAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item Product 1 Price by 24 Flat.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.FlatAmount;
            target.ProductID = ProductId;
            mod.Property = "ItemPrice";
            mod.ModificationValue = 24M;
            target.Quantity = 1;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedSingleProduct1VolumeBy25PercentAdjustment(int ProductId, int accountID)
        {

            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item Product 1 Volume by 25%.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.Multiplier;
            target.ProductID = ProductId;
            mod.Property = "Volume";
            mod.ModificationValue = .25M;
            target.Quantity = 1;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }

        private static IOrderAdjustmentProfileOrderItemTarget GetReducedSingleProduct1VolumeBy26FlatAdjustment(int ProductId, int accountID)
        {
            IOrderAdjustmentProfileOrderItemTarget target = Create.New<IOrderAdjustmentProfileOrderItemTarget>();
            IOrderAdjustmentProfileOrderLineModification mod = Create.New<IOrderAdjustmentProfileOrderLineModification>();
            mod.Description = "Reduced Item Product 1 Volume by 26 Flat.";
            mod.ModificationOperationID = (int)OrderAdjustmentOrderLineOperationKind.FlatAmount;
            target.ProductID = ProductId;
            mod.Property = "Volume";
            mod.ModificationValue = 26M;
            target.Quantity = 1;

            target.Modifications.Add(mod);
            target.OrderCustomerAccountID = accountID;
            return target;
        }
    }
}
