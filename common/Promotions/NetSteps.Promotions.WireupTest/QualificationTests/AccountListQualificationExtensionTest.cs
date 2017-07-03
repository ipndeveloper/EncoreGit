using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    [TestClass]
    public class AccountListQualificationExtensionTest : GenericQualificationExtensionTest<IAccountListQualificationExtension>
    {
        public AccountListQualificationExtensionTest()
            : base(() => { return new AccountListQualification(); })
        {
        }
    }
}
