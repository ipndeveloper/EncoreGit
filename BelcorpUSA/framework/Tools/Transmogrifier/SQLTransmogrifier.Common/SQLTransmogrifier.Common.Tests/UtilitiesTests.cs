namespace SQLTransmogrifier.Common.Tests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using SqlTransmogrifier;

	[TestClass]
	public class UtilitiesTests
	{
		[TestMethod]
		public void Utilities_can_get_string_value_from_arguments()
		{
			var test = new
			{
				Args = new string[] { "-a", "-b", "-cee" }
			};

			var a = Utilities.GetStringValueFromArguments(test.Args, "-a");
			Assert.IsNotNull(a);
			Assert.AreEqual(test.Args[0], a);
			
			var b = Utilities.GetStringValueFromArguments(test.Args, "-b");
			Assert.IsNotNull(b);
			Assert.AreEqual(test.Args[1], b);

			var c = Utilities.GetStringValueFromArguments(test.Args, "-cee");
			Assert.IsNotNull(c);
			Assert.AreEqual(test.Args[2], c);

			var d = Utilities.GetStringValueFromArguments(test.Args, "-d");
			Assert.IsTrue(String.IsNullOrEmpty(d));
		}
	}
}
