using System;
using NetSteps.Extensibility.Core;

namespace NetSteps.OrderAdjustments.Common.Test.Mocks
{
    public class MockOrderAdjustmentExtension : IDataObjectExtension
    {
        public int OrderAdjustmentID { get; set; }

        public int MockProfileID { get; set; }
    }
}
