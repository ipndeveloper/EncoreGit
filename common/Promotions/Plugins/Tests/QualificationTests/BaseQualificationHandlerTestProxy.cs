using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.BasePluginTests;
using NetSteps.Promotions.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Data.Common.Context;
using Moq;
using NetSteps.Data.Common.Entities;
using System.Collections.Generic;
using NetSteps.Data.Common.Services;

namespace NetSteps.Promotions.Plugins.Tests.QualificationTests
{
    [TestClass]
    public abstract class BaseQualificationHandlerTestProxy<TQualificationHandler> : BaseQualificationHandlerTest<TQualificationHandler> 
        where TQualificationHandler : IPromotionQualificationHandler
    {
        private IContainer _container;

        [TestInitialize]
        public virtual void Init()
        {
            _container = Create.NewContainer();

            _container.ForType<IPriceTypeService>()
                    .Register<IPriceTypeService>((c, p) => { return new Mock<IPriceTypeService>().Object; })
                    .ResolveAnInstancePerRequest()
                    .End();

            WireupCoordinator.SelfConfigure();
            
                
        }

        [TestCleanup]
        public void Cleanup()
        {
            _container = null;
        }
        [TestMethod]
        public override void BASETEST_PromotionQualification_should_be_constructable_by_the_IoC()
        {
            base.BASETEST_PromotionQualification_should_be_constructable_by_the_IoC();
        }

        [TestMethod]
        public override void  BASETEST_QualificationHandler_should_be_registered_with_data_object_extension_provider()
        {
 	         base.BASETEST_QualificationHandler_should_be_registered_with_data_object_extension_provider();
        } 

        protected IOrderContext GetMockOrderContext(int accountID)
        {
            var orderContextMock = new Mock<IOrderContext>().SetupAllProperties();
            orderContextMock.Object.Order = GetMockOrder(accountID);
            
            return orderContextMock.Object;
        }

        protected IOrder GetMockOrder(int accountID)
        {
            var orderMock = new Mock<IOrder>().SetupAllProperties();
            var orderCustomerList = new List<IOrderCustomer>(new IOrderCustomer[] { GetMockOrderCustomer(accountID) });
            orderMock.SetupGet<IList<IOrderCustomer>>(x => x.OrderCustomers).Returns(orderCustomerList);
            return orderMock.Object;
        }

        protected IOrderCustomer GetMockOrderCustomer(int accountID)
        {
            var orderCustomerMock = new Mock<IOrderCustomer>().SetupAllProperties();
            orderCustomerMock.Object.AccountID = 1;
            orderCustomerMock.SetupGet<IList<IOrderItem>>(x => x.OrderItems).Returns(new List<IOrderItem>());
            return orderCustomerMock.Object;
        }

    }
}
