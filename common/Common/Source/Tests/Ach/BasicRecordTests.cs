using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Ach;

namespace NetSteps.Common.Tests.Ach
{
    [TestClass]
    public class BasicRecordTests
    {
        [TestMethod]
        public void TestPadItemLength()
        {
            FileControlRecord fileControl = new FileControlRecord();

            int testStringNewLength = 20;
            string beforeTestString = "LengthTest";
            string afterPadLeftTestString = fileControl.PadItemLength(beforeTestString, testStringNewLength, true, ' ');
            string afterPadRightTestString = fileControl.PadItemLength(beforeTestString, testStringNewLength, false, ' ');

            //Test padleft
            Assert.AreEqual(testStringNewLength, afterPadLeftTestString.Length);
            Assert.AreNotSame(beforeTestString, afterPadLeftTestString);
            Assert.IsTrue(afterPadLeftTestString.Contains(" "));

            //Test padright
            Assert.AreEqual(testStringNewLength, afterPadRightTestString.Length);
            Assert.AreNotSame(beforeTestString, afterPadRightTestString);
            Assert.IsTrue(afterPadLeftTestString.Contains(" "));

            //Test both
            Assert.AreNotSame(afterPadLeftTestString, afterPadRightTestString);
        }
    }
}
