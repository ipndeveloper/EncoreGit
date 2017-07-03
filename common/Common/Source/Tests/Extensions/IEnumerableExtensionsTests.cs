using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.Tests.Extensions
{
    [TestClass]
    public class IEnumerableExtensionsTests
    {
        [TestMethod()]
        public void SplitWhere_NotConsecutive_ReturnsCorrect()
        {
            var source = new[] { 1, 2, 3, 6, 7, 8, 9, 10, 15, 100 };
            var firstSecondPredicate = new Func<int, int, bool>((first, second) => !second.Equals(first + 1));
            var expected = new[]
            {
                new[] { 1, 2, 3 },
                new[] { 6, 7, 8, 9, 10 },
                new[] { 15 },
                new[] { 100 }
            };
            var actual = IEnumerableExtensions.SplitWhere(source, firstSecondPredicate).ToArray();
            for (int i = 0; i < expected.Count(); i++)
            {
                Assert.IsTrue(expected[i].SequenceEqual(actual[i]));
            }
        }

        [TestMethod()]
        public void SplitWhere_Empty_ReturnsEmpty()
        {
            var source = Enumerable.Empty<int>();
            var firstSecondPredicate = new Func<int, int, bool>((first, second) => !second.Equals(first + 1));
            var expected = 0;
            var actual = IEnumerableExtensions.SplitWhere(source, firstSecondPredicate).Count();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MaxConsecutive_Normal_ReturnsCorrect()
        {
            var values = new[] { 1, 2, 3, 6, 7, 8, 9, 10, 15, 100 };
            int expected = 5;
            int actual;
            actual = IEnumerableExtensions.MaxConsecutive(values);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MaxConsecutive_100000_SpeedTest()
        {
            var values = Enumerable.Range(1, 100000);
            int expected = 100000;
            int actual;
            actual = IEnumerableExtensions.MaxConsecutive(values);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MaxConsecutiveMonths_SomeConsecutive_ReturnsCorrect()
        {
            var dates = new[]
            {
                new DateTime(2000, 1, 5),
                new DateTime(1999, 1, 1),
                new DateTime(1999, 1, 15),
                new DateTime(1999, 2, 7),
                new DateTime(1999, 3, 1),
                new DateTime(1999, 6, 30),
                new DateTime(1999, 7, 1),
                new DateTime(1999, 11, 24),
                new DateTime(1999, 12, 24),
                new DateTime(2000, 2, 1),
                new DateTime(2000, 3, 1),
                new DateTime(2000, 4, 1),
                new DateTime(2000, 5, 31),
                new DateTime(2000, 7, 1)
            };
            int expected = 7;
            int actual;
            actual = IEnumerableExtensions.MaxConsecutiveMonths(dates);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MaxConsecutiveMonths_AllConsecutive_ReturnsCorrect()
        {
            var dates = new[]
            {
                new DateTime(1999, 11, 24),
                new DateTime(1999, 12, 24),
                new DateTime(2000, 1, 5),
                new DateTime(2000, 2, 1),
                new DateTime(2000, 3, 1),
                new DateTime(2000, 4, 1),
                new DateTime(2000, 5, 31)
            };
            int expected = 7;
            int actual;
            actual = IEnumerableExtensions.MaxConsecutiveMonths(dates);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void MaxConsecutiveMonths_Empty_ReturnsZero()
        {
            var dates = Enumerable.Empty<DateTime>();
            int expected = 0;
            int actual;
            actual = IEnumerableExtensions.MaxConsecutiveMonths(dates);
            Assert.AreEqual(expected, actual);
        }
    }
}
