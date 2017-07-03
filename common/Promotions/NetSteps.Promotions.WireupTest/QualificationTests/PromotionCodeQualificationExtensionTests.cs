using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Components;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;

namespace NetSteps.Promotions.WireupTest.QualificationTests
{
    
    [TestClass]
    public class PromotionCodeQualificationExtensionTests : GenericQualificationExtensionTest<IPromotionCodeQualificationExtension>
    {
        public PromotionCodeQualificationExtensionTests()
            : base(() => { return new PromotionCodeQualification(); })
        {
        }
    }
}
