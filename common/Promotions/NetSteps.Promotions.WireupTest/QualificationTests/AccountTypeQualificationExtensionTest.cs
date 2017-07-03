using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    /// <summary>
    /// Summary description for AccountTypeQualificationExtensionTest
    /// </summary>
    [TestClass]
    public class AccountTypeQualificationExtensionTest : GenericQualificationExtensionTest<IAccountTypeQualificationExtension>
    {
        public AccountTypeQualificationExtensionTest()
            : base(() => { return new AccountTypeQualification(); })
        {
        }
    }
}
