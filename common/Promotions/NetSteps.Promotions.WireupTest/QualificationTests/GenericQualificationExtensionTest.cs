using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using Promotions.BasePluginTests;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    [TestClass]
    public abstract class GenericQualificationExtensionTest<TQualificationExtension> : BaseQualificationTest<TQualificationExtension>
        where TQualificationExtension : IPromotionQualificationExtension
    {
        public GenericQualificationExtensionTest(Func<TQualificationExtension> constructor) : base(constructor) { }

        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }
        private void mockWireup()
        {
        }

        [TestMethod]
        public override void BASETEST_PromotionQualification_should_have_a_nonnull_AssociatedPropertyNames_list()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); 
                base.BASETEST_PromotionQualification_should_have_a_nonnull_AssociatedPropertyNames_list();
            }
        }

        [TestMethod]
        public override void BASETEST_PromotionQualification_should_return_valid_for_property_if_not_found_in_AssociatedPropertyNames()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); 
                base.BASETEST_PromotionQualification_should_return_valid_for_property_if_not_found_in_AssociatedPropertyNames();
            }
        }

        [TestMethod]
        public override void BASETEST_PromotionQualification_should_be_constructable_by_the_IoC()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); base.BASETEST_PromotionQualification_should_be_constructable_by_the_IoC();
            }
        }

        [TestMethod]
        public override void BASETEST_PromotionQualification_should_have_handler_registered_by_concrete_name_with_dataobjectproviderregistry()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); 
                base.BASETEST_PromotionQualification_should_have_handler_registered_by_concrete_name_with_dataobjectproviderregistry();
            }
        }

        [TestMethod]
        public override void BASETEST_PromotionQualification_should_have_handler_registered_by_interface_name_with_dataobjectproviderregistry()
        {
            using (var container = Create.NewContainer())
            {
                mockWireup(); 
                base.BASETEST_PromotionQualification_should_have_handler_registered_by_interface_name_with_dataobjectproviderregistry();
            }
        }
    }
}
