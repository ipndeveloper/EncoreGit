using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Service.EntityModels;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;

namespace NetSteps.Promotions.Service.Tests.EntityModels
{
    [TestClass]
    public class NetStepsPromotionsEntitiesTests
    {
        //[ClassInitialize]
        //public static void ClassInitialize(TestContext testContext)
        //{
        //    WireupCoordinator.SelfConfigure();
        //}

        //[TestMethod]
        //public void NetStepsPromotionsEntities_WithStoreConnection_Initializes()
        //{
        //    var storeConnectionString = ConfigurationManager.ConnectionStrings["NetStepsCore"];
        //    var storeConnection = new SqlConnection(storeConnectionString.ConnectionString);

        //    var promotion = new Promotion
        //    {
        //        Description = "Test"
        //    };

        //    using (var context = new NetStepsPromotionsEntities())
        //    {
        //        context.Promotions.AddObject(promotion);
        //        context.SaveChanges();
        //        Assert.AreNotEqual(0, promotion.PromotionID);
        //    }

        //    using (var ts = new TransactionScope())
        //    {
        //        using (var context = new NetStepsPromotionsEntities(storeConnection))
        //        {
        //            var promotion1 = context.Promotions.First(p => p.PromotionID == promotion.PromotionID);
        //            promotion1.Description = "test1";
        //            context.SaveChanges();
        //        }

        //        using (var context = new NetStepsPromotionsEntities(storeConnection))
        //        {
        //            var promotion2 = context.Promotions.First(p => p.PromotionID == promotion.PromotionID);
        //            promotion2.Description = "test2";
        //            context.SaveChanges();
        //        }

        //        ts.Complete();
        //    }

        //    using (var context = new NetStepsPromotionsEntities())
        //    {
        //        var promotion3 = context.Promotions.First(p => p.PromotionID == promotion.PromotionID);
        //        Assert.AreEqual("test2", promotion3.Description);
        //        context.DeleteObject(promotion3);
        //        context.SaveChanges();
        //    }
        //}

        //[TestMethod]
        //public void NetStepsPromotionsEntities_InitializesViaIoC()
        //{
        //    var context = Create.New<IPromotionUnitOfWork>();
        //    Assert.IsNotNull(context);
        //    Assert.IsInstanceOfType(context, typeof(NetStepsPromotionsEntities));
        //}
    }
}
