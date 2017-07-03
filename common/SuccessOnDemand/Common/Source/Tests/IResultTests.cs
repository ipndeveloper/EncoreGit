using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Representation;

namespace NetSteps.SOD.Common.Tests
{
    [TestClass]
    public class IResultTests
    {
        [TestMethod]
        public void Monkey()
        {
            var test = new {
                DistID = "My Dist ID",
                ID = "My ID",
                Error = (string)null
            };
            var result = Create.New<IResponse>();

            result.DistID = test.DistID;
            result.ID = test.ID;
            result.Error = test.Error;

            Assert.AreEqual(test.DistID, result.DistID);
            Assert.AreEqual(test.ID, result.ID);
            Assert.AreEqual(test.Error, result.Error);
        }
    }
}
