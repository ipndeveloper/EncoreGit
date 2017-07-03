using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.WebService.Mobile.Tests
{
    [TestClass]
    public class MobileServiceTests
    {
        [TestMethod]
        public void MobileService_Constructor_InstantiatesWithoutError()
        {
            var mobileService = new MobileService();
        }
    }
}
