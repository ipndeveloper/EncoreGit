using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    [TestClass]
    public class UseCountQualificationExtensionTest : GenericQualificationExtensionTest<IUseCountQualificationExtension>
    {
        public UseCountQualificationExtensionTest()
            : base(() => { return new UseCountQualification(); })
        {
        }
    }
}
