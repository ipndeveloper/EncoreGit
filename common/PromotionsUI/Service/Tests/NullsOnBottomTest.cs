using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.UI.Service.Impl;

namespace Tests
{
    [TestClass]
    public class NullsOnBottomTest : MsTestInteractionContext<NullsOnBottom>
    {
        [TestMethod]
        public void OrderByPutsNullsOnBottom()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var yesterday = today.AddDays(-1);
            var list = new List<FakeEntity>
            {
                new FakeEntity(),
                new FakeEntity{Date = today},
                new FakeEntity(),
                new FakeEntity{Date = tomorrow},
                new FakeEntity(),
                new FakeEntity{Date = yesterday},
            };
            var results = list.OrderBy(x => x.Date, ClassUnderTest).ToList();
            Assert.AreEqual(yesterday, results[0].Date);
            Assert.AreEqual(today, results[1].Date);
            Assert.AreEqual(tomorrow, results[2].Date);
            Assert.IsNull(results[3].Date);
            Assert.IsNull(results[4].Date);
            Assert.IsNull(results[5].Date);
        }

        private class FakeEntity
        {
            public DateTime? Date { get; set; }
        }
         
    }
}