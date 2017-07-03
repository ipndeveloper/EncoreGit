using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common;
using NetSteps.OrderAdjustments.Common.Component;
using NetSteps.OrderAdjustments.Common.Model.ModelConcrete;
using NetSteps.OrderAdjustments.Service.Test.Mocks;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.OrderAdjustments.Service.Test
{
    [TestClass]
    public class OrderAdjustmentServiceTests
    {
        MockOrderAdjustmentProviderManager ProviderManager { get; set; }
        MockOrderAdjustmentProvider Provider { get; set; }
        int _productID;
        int _accountID;

        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();

			_productID = new Random().Next();
			_accountID = new Random().Next();
			ProviderManager = new MockOrderAdjustmentProviderManager(_productID, _accountID);
			Provider = new MockOrderAdjustmentProvider();
			ProviderManager.RegisterAdjustmentProvider(Provider);
        }

        [TestMethod]
        public void OrderAdjustmentService_should_be_registered()
        {
            // test the IOC registration to verify that we've got it correct
            var adjustmentService = Create.New<IOrderAdjustmentService>();
            Assert.IsNotNull(adjustmentService);
            Assert.IsInstanceOfType(adjustmentService, typeof(IOrderAdjustmentService));
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_no_filters()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(new MockOrderContext(_accountID));
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_order_validator()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);
            
            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            int random = new Random().Next();
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(orderContext, (IOrderContext x) => { 
                                                                                                                        x.Order.OrderID = random;
                                                                                                                        return true; 
                                                                                                                  });
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            Assert.AreEqual(random, orderContext.Order.OrderID);
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_adjustment_validator()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(orderContext, (IOrderAdjustmentProfile x) => 
                                                                                                            {
                                                                                                                x.Description = Provider.GetProviderKey();
                                                                                                                return true; 
                                                                                                            });
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.AreEqual(Provider.GetProviderKey(), adjustment.ExtensionProviderKey);
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_adjustment_delegatefilter()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(orderContext, delegate(IOrderContext filterorder, IQueryable<IOrderAdjustmentProfile> adjustmentSet)
                                                                                                        {
                                                                                                            List<IOrderAdjustmentProfile> adjs = adjustmentSet.ToList();
                                                                                                            foreach (IOrderAdjustmentProfile adjustment in adjs)
                                                                                                                adjustment.Description = "I am a teapot, short and stout.";
                                                                                                            return adjs.AsQueryable();
                                                                                                        });
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.AreEqual("I am a teapot, short and stout.", adjustment.Description);
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_adjustment_objectfilter()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(
                                                                                                orderContext,
                                                                                                new MockOrderAdjustmentObjectFilter()
                                                                                                );

            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.IsTrue(adjustment.Description.EndsWith("Here is my handle, here is my spout."));
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_order_validator_and_adjustment_validator()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            int random = new Random().Next();
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(
                                                                                                orderContext,
                                                                                                (IOrderContext x) =>
                                                                                                {
                                                                                                    x.Order.OrderID = random;
                                                                                                    return true;
                                                                                                },
                                                                                                (IOrderAdjustmentProfile x) =>
                                                                                                {
                                                                                                    x.Description = "I am a teapot";
                                                                                                    return true;
                                                                                                });
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            Assert.AreEqual(random, orderContext.Order.OrderID);
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.IsTrue(adjustment.Description.EndsWith("I am a teapot"));
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_order_validator_and_adjustment_delegatefilter()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            int random = new Random().Next();
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(
                                                                                                orderContext,
                                                                                                (IOrderContext x) =>
                                                                                                {
                                                                                                    x.Order.OrderID = random;
                                                                                                    return true;
                                                                                                },
                                                                                                delegate(IOrderContext filterorder, IQueryable<IOrderAdjustmentProfile> adjustmentSet)
                                                                                                {
                                                                                                    List<IOrderAdjustmentProfile> adjs = adjustmentSet.ToList();
                                                                                                    foreach (IOrderAdjustmentProfile adjustment in adjs)
                                                                                                        adjustment.Description = "I am a teapot, short and stout.";
                                                                                                    return adjs.AsQueryable();
                                                                                                });
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            Assert.AreEqual(random, orderContext.Order.OrderID);
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.IsTrue(adjustment.Description.EndsWith("I am a teapot, short and stout."));
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_adjustment_validator_and_adjustment_delegatefilter()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(
                                                                                                orderContext,
                                                                                                (IOrderAdjustmentProfile x) =>
                                                                                                {
                                                                                                    x.Description += "I am a teapot";
                                                                                                    return true;
                                                                                                },
                                                                                                delegate(IOrderContext filterorder, IQueryable<IOrderAdjustmentProfile> adjustmentSet)
                                                                                                {
                                                                                                    List<IOrderAdjustmentProfile> adjs = adjustmentSet.ToList();
                                                                                                    foreach (IOrderAdjustmentProfile adjustment in adjs)
                                                                                                        adjustment.Description += ", short and stout.";
                                                                                                    return adjs.AsQueryable();
                                                                                                });
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.IsTrue(adjustment.Description.EndsWith("I am a teapot, short and stout."));
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_order_validator_and_adjustment_validator_and_adjustment_delegatefilter()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID);
            int random = new Random().Next();
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(
                                                                                                orderContext,
                                                                                                (IOrderContext x) =>
                                                                                                {
                                                                                                    x.Order.OrderID = random;
                                                                                                    return true;
                                                                                                },
                                                                                                (IOrderAdjustmentProfile x) => 
                                                                                                {
                                                                                                    x.Description += "I am a teapot";
                                                                                                    return true; 
                                                                                                },
                                                                                                delegate(IOrderContext filterorder, IQueryable<IOrderAdjustmentProfile> adjustmentSet)
                                                                                                {
                                                                                                    List<IOrderAdjustmentProfile> adjs = adjustmentSet.ToList();
                                                                                                    foreach (IOrderAdjustmentProfile adjustment in adjs)
                                                                                                        adjustment.Description += ", short and stout.";
                                                                                                    return adjs.AsQueryable();
                                                                                                });
            
            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            Assert.AreEqual(random, orderContext.Order.OrderID);
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.IsTrue(adjustment.Description.EndsWith("I am a teapot, short and stout."));
        }

        [TestMethod]
        public void OrderAdjustmentService_should_retrieve_adjustments_with_order_validator_and_adjustment_validator_and_adjustment_delegatefilter_and_adjustment_objectfilter()
        {
            OrderAdjustmentService service = new OrderAdjustmentService(ProviderManager);

            // add adjustments
            IEnumerable<IOrderAdjustmentProfile> mockAdjustmentProfiles = MockOrderAdjustmentCreator.GenerateOneOfEach(_productID, _accountID);
            foreach (IOrderAdjustmentProfile profile in mockAdjustmentProfiles)
            {
                Provider.AddAdjustment(profile as MockOrderAdjustmentProfile);
            }

            MockOrderContext orderContext = new MockOrderContext(_accountID); 
            int random = new Random().Next();
            IEnumerable<IOrderAdjustmentProfile> adjustments = service.GetApplicableOrderAdjustments(
                                                                                                orderContext,
                                                                                                (IOrderContext x) =>
                                                                                                {
                                                                                                    x.Order.OrderID = random;
                                                                                                    return true;
                                                                                                },
                                                                                                (IOrderAdjustmentProfile x) =>
                                                                                                {
                                                                                                    x.Description += "I am a teapot";
                                                                                                    return true;
                                                                                                },
                                                                                                delegate(IOrderContext filterorder, IQueryable<IOrderAdjustmentProfile> adjustmentSet)
                                                                                                {
                                                                                                    List<IOrderAdjustmentProfile> adjs = adjustmentSet.ToList();
                                                                                                    foreach (IOrderAdjustmentProfile adjustment in adjs)
                                                                                                        adjustment.Description += ", short and stout.";
                                                                                                    return adjs.AsQueryable();
                                                                                                },
                                                                                                new MockOrderAdjustmentObjectFilter()
                                                                                                );

            Assert.AreEqual(mockAdjustmentProfiles.Count(), adjustments.Count());
            Assert.AreEqual(random, orderContext.Order.OrderID);
            foreach (IOrderAdjustmentProfile adjustment in adjustments)
                Assert.IsTrue(adjustment.Description.EndsWith("I am a teapot, short and stout.Here is my handle, here is my spout."));
        }
    }
}
