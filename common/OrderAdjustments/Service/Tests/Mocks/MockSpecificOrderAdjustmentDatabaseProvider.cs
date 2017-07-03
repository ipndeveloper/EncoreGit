using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.OrderAdjustments.Common.Model;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public static class MockSpecificOrderAdjustmentDatabaseProvider
    {
        private static Dictionary<int, IOrderAdjustmentProfile> _mockSpecificOrderAdjustments;

        static MockSpecificOrderAdjustmentDatabaseProvider()
        {
            _mockSpecificOrderAdjustments = new Dictionary<int, IOrderAdjustmentProfile>();
        }

        public static void Save(int orderAdjustmentID, IOrderAdjustmentProfile profile)
        {
            _mockSpecificOrderAdjustments.Add(orderAdjustmentID, profile);
        }

        public static IOrderAdjustmentProfile RetrieveOrderAdjustmentProfile(int OrderAdjustmentProfileID)
        {
            return _mockSpecificOrderAdjustments[OrderAdjustmentProfileID];
        }
    }
}
