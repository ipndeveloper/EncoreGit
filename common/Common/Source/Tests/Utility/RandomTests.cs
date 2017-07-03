using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Common.Tests.Utility
{
    [TestClass]
    public class RandomTests
    {
        [TestMethod]
        public void GetString_Should_Generate_Random_String_Of_Provided_Length()
        {
            var length = Random.Next(1,100);
            var a1 = Random.GetString(length);
            var a2 = Random.GetString(length);
            Assert.AreNotEqual(a1, a2);
            //current limitation is 32 characters
            Assert.AreEqual(length > 32 ? 32 : length, a1.Length, "A1 Length did not match");
            Assert.AreEqual(length > 32 ? 32 : length, a2.Length, "A2 Length did not match");
        }
    }
}
