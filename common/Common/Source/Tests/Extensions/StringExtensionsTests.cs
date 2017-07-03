using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Common.Extensions.Tests
{
    [TestClass]
    public class StringExtensionsTests
    {
        [TestMethod]
        public void ContainsIgnoreCase_BothNull_ReturnsTrue()
        {
            string original = null;
            string value = null;

            bool result = StringExtensions.ContainsIgnoreCase(original, value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsIgnoreCase_FirstNull_ReturnsFalse()
        {
            string original = null;
            string value = "Test";

            bool result = StringExtensions.ContainsIgnoreCase(original, value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsIgnoreCase_SecondNull_ReturnsFalse()
        {
            string original = "Test";
            string value = null;

            bool result = StringExtensions.ContainsIgnoreCase(original, value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsIgnoreCase_BothEmpty_ReturnsTrue()
        {
            string original = string.Empty;
            string value = string.Empty;

            bool result = StringExtensions.ContainsIgnoreCase(original, value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsIgnoreCase_FirstEmpty_ReturnsFalse()
        {
            string original = string.Empty;
            string value = "Test";

            bool result = StringExtensions.ContainsIgnoreCase(original, value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ContainsIgnoreCase_SecondEmpty_ReturnsTrue()
        {
            string original = "Test";
            string value = string.Empty;

            bool result = StringExtensions.ContainsIgnoreCase(original, value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ContainsIgnoreCase_DifferentCase_ReturnsTrue()
        {
            string original = "Testing";
            string value = "TEST";

            bool result = StringExtensions.ContainsIgnoreCase(original, value);

            Assert.IsTrue(result);
        }

		[TestMethod]
		public void ToTitleCase_WordsSeparatedWithMultipleWhiteSpaces_Succeeds()
		{
			string original = "dragon  dragon";
			string expected = "Dragon Dragon";

			string result = StringExtensions.ToTitleCase(original);

			Assert.AreEqual(expected, result);
		}
    }
}
