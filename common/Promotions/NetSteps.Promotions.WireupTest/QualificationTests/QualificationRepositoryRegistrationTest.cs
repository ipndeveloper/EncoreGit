using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.EntityModel;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    [TestClass]
    public class QualificationRepositoryRegistrationTest
    {
        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }

        private void mockWireup()
        {
        }

        [TestMethod]
        public void AccountListQualificationRepository_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));

                // test the IOC registration to verify that we've got it correct
                var repository = Create.New<IAccountListQualificationRepository>();
                Assert.IsNotNull(repository);
                Assert.IsInstanceOfType(repository, typeof(IAccountListQualificationRepository));
            }
        }

        [TestMethod]
        public void AccountTypeQualificationRepository_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));

                // test the IOC registration to verify that we've got it correct
                var repository = Create.New<IAccountTypeQualificationRepository>();
                Assert.IsNotNull(repository);
                Assert.IsInstanceOfType(repository, typeof(IAccountTypeQualificationRepository));
            }
        }

        [TestMethod]
        public void ProductInOrderQualificationRepository_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));

                // test the IOC registration to verify that we've got it correct
                var repository = Create.New<IProductInOrderQualificationRepository>();
                Assert.IsNotNull(repository);
                Assert.IsInstanceOfType(repository, typeof(IProductInOrderQualificationRepository));
            }
        }

        [TestMethod]
        public void MarketListQualificationRepository_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));

                // test the IOC registration to verify that we've got it correct
                var repository = Create.New<IMarketListQualificationRepository>();
                Assert.IsNotNull(repository);
                Assert.IsInstanceOfType(repository, typeof(IMarketListQualificationRepository));
            }
        }

        [TestMethod]
        public void CustomerSubtotalRangeQualificationRepository_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));

                // test the IOC registration to verify that we've got it correct
                var repository = Create.New<ICustomerSubtotalRangeQualificationRepository>();
                Assert.IsNotNull(repository);
                Assert.IsInstanceOfType(repository, typeof(ICustomerSubtotalRangeQualificationRepository));
            }
        }

        [TestMethod]
        public void OrderTypeQualificationRepository_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));

                // test the IOC registration to verify that we've got it correct
                var repository = Create.New<IOrderTypeQualificationRepository>();
                Assert.IsNotNull(repository);
                Assert.IsInstanceOfType(repository, typeof(IOrderTypeQualificationRepository));
            }
        }

        [TestMethod]
        public void PromotionCodeQualificationRepository_should_be_registered()
        {
            using (var container = Create.NewContainer())
            {
                var unitOfWork = Create.New<IEncorePromotionsPluginsUnitOfWork>();
                Assert.IsNotNull(unitOfWork);
                Assert.IsInstanceOfType(unitOfWork, typeof(IEncorePromotionsPluginsUnitOfWork));

                // test the IOC registration to verify that we've got it correct
                var repository = Create.New<IPromotionCodeQualificationRepository>();
                Assert.IsNotNull(repository);
                Assert.IsInstanceOfType(repository, typeof(IPromotionCodeQualificationRepository));
            }
        }

    }
}
