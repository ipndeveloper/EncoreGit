using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.ModelConcrete;
using Moq;
using NetSteps.Promotions.Common.Model;
using NetSteps.Extensibility.Core;

namespace NetSteps.Promotions.Common.Tests
{
    [TestClass]
    public class BasePromotionTests
    {
        [TestClass]
        public class PromotionTest
        {

            private class MockPromotion : BasePromotion
            {
                public override string PromotionKind
                {
                    get { return "Test"; }
                }
            }

            private class MockQualificationExtension : Mock<IPromotionQualificationExtension>
            {
                public readonly string PropertyTargeted;

                public MockQualificationExtension(string propertyTargeted, string providerKey)
                {
                    PropertyTargeted = propertyTargeted;
                    this.SetupAllProperties();
                    this.SetupGet<string>(x => x.ExtensionProviderKey).Returns(providerKey);
                }
            }

            private class MockQualificationHandler : Mock<ITestPromotionQualificationHandler2>
            {
                private readonly string _targetProperty;

                public MockQualificationHandler(string targetProperty, string providerKey)
                {
                    _targetProperty = targetProperty;
                    this.Setup(x => x.GetProviderKey()).Returns(providerKey);
                    this.Setup(x => x.ValidFor<bool>(It.IsAny<IPromotionQualificationExtension>(), It.IsAny<string>(), It.IsAny<bool>()))
                        .Returns((IPromotionQualificationExtension extension, string property, bool value) =>
                        {
                            if (property.Equals(targetProperty, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return value;
                            }
                            return true;
                        });
                }
            }

            private void RegisterHandler<T>(IContainer container, string providerKey, string targetProperty) where T : IPromotionQualificationHandler
            {
                container.ForType<T>()
                    .Register<T>((c, p) => { return ((T)new MockQualificationHandler(targetProperty, providerKey).Object); })
                    .ResolveAnInstancePerRequest()
                    .End();
                Create.New<IDataObjectExtensionProviderRegistry>().RegisterExtensionProvider<T>(providerKey);
            }

            [TestMethod]
            public void Promotions_should_return_valid_for_anything_when_promotion_has_no_qualifications()
            {
                using (var container = Create.NewContainer())
                {
                    var promotion = new MockPromotion();
                    Assert.IsTrue(promotion.ValidFor<string>("test", "bob"));
                }
            }

            [TestMethod]
            public void Promotions_should_return_validity_correctly_for_a_single_qualification()
            {
                using (var container = Create.NewContainer())
                {   // Scope 1, target property assessment passes
                    var targetProperty = "test";
                    var providerKey = Guid.NewGuid().ToString();

                    var promotion = new MockPromotion();
                    var mockQualificationExtension = new MockQualificationExtension(targetProperty, providerKey);
                    mockQualificationExtension.SetupAllProperties();
                    mockQualificationExtension.SetupGet<string>(x => x.ExtensionProviderKey).Returns(providerKey);
                    promotion.PromotionQualifications.Add(targetProperty, mockQualificationExtension.Object);
                    RegisterHandler<IPromotionQualificationHandler>(container, providerKey, targetProperty);
                    Assert.IsTrue(promotion.ValidFor<bool>(targetProperty, true));
                }

                using (var container = Create.NewContainer())
                {   // Scope 2, target property assessment fails.

                    var targetProperty = "test";
                    var providerKey = Guid.NewGuid().ToString();

                    var promotion = new MockPromotion();
                    var mockQualificationExtension = new MockQualificationExtension(targetProperty, providerKey);
                    mockQualificationExtension.SetupAllProperties();
                    mockQualificationExtension.SetupGet<string>(x => x.ExtensionProviderKey).Returns(providerKey);
                    promotion.PromotionQualifications.Add(targetProperty, mockQualificationExtension.Object);
                    RegisterHandler<IPromotionQualificationHandler>(container, providerKey, targetProperty);
                    Assert.IsFalse(promotion.ValidFor<bool>(targetProperty, false));
                }

                using (var container = Create.NewContainer())
                {   // Scope 3, target property does not match handler.

                    var targetProperty = "test";
                    var providerKey = Guid.NewGuid().ToString();

                    var promotion = new MockPromotion();
                    var mockQualificationExtension = new MockQualificationExtension(targetProperty, providerKey);
                    mockQualificationExtension.SetupAllProperties();
                    mockQualificationExtension.SetupGet<string>(x => x.ExtensionProviderKey).Returns(providerKey);
                    promotion.PromotionQualifications.Add(targetProperty, mockQualificationExtension.Object);
                    RegisterHandler<IPromotionQualificationHandler>(container, providerKey, targetProperty);
                    Assert.IsTrue(promotion.ValidFor<bool>("notthesameproperty", false));
                }
            }

            [TestMethod]
            public void Promotions_should_return_validity_correctly_for_a_multiple_qualifications()
            {
                
                using (var container = Create.NewContainer())
                {   // one false, one true
                    var promotion = new MockPromotion();

                    { // register handler 1
                        var targetProperty = "test1";
                        var providerKey = Guid.NewGuid().ToString();

                        var mockQualificationExtension = new MockQualificationExtension(targetProperty, providerKey);
                        mockQualificationExtension.SetupAllProperties();
                        mockQualificationExtension.SetupGet<string>(x => x.ExtensionProviderKey).Returns(providerKey);
                        promotion.PromotionQualifications.Add(targetProperty, mockQualificationExtension.Object);
                        RegisterHandler<ITestPromotionQualificationHandler1>(container, providerKey, targetProperty);
                    }

                    { // register handler 2
                        var targetProperty = "test2";
                        var providerKey = Guid.NewGuid().ToString();

                        var mockQualificationExtension = new MockQualificationExtension(targetProperty, providerKey);
                        mockQualificationExtension.SetupAllProperties();
                        mockQualificationExtension.SetupGet<string>(x => x.ExtensionProviderKey).Returns(providerKey);
                        promotion.PromotionQualifications.Add(targetProperty, mockQualificationExtension.Object);
                        RegisterHandler<ITestPromotionQualificationHandler2>(container, providerKey, targetProperty);
                    }

                    Assert.IsFalse(promotion.ValidFor<bool>("test1", false));
                    Assert.IsTrue(promotion.ValidFor<bool>("test1", true));
                    Assert.IsFalse(promotion.ValidFor<bool>("test2", false));
                    Assert.IsTrue(promotion.ValidFor<bool>("test2", true));
                    Assert.IsTrue(promotion.ValidFor<bool>("test3", false));
                    Assert.IsTrue(promotion.ValidFor<bool>("test3", true));
                }
            }

            public interface ITestPromotionQualificationHandler1 : IPromotionQualificationHandler
            { }

            public interface ITestPromotionQualificationHandler2 : ITestPromotionQualificationHandler1
            { }

            //[TestMethod]
            //public void Promotions_should_have_a_compiled_list_of_associatedpropertynames_from_their_qualifications()
            //{
            //    using (var container = Create.NewContainer())
            //    {
            //        var promotion = new MockPromotion();
            //        promotion.PromotionQualifications.Add("test", new Mock<IPromotionQualificationExtension>().Object);
            //        Assert.AreEqual(1, promotion.AssociatedPropertyNames.Count());
            //        Assert.IsTrue(promotion.AssociatedPropertyNames.Contains(MockPromotionQualificationExtension.ValidName));
            //    }
            //}

            [TestMethod]
            public void Promotions_should_have_an_empty_associatedpropertynames_if_it_has_no_qualifications()
            {
                using (var container = Create.NewContainer())
                {
                    var promotion = new MockPromotion();
                    Assert.AreEqual(0, promotion.AssociatedPropertyNames.Count());
                }
            }
        }
    }
}
