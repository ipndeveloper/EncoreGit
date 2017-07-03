using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Collections;
using System;

namespace NetSteps.Encore.Core.Tests.Collections
{
	[TestClass]
	public class BitFlags32Tests
	{
		[TestMethod]
		public void EmptyInstance()
		{
			var empty = BitFlags32.Empty;
			Assert.IsTrue(empty.IsEmpty);
			for (int i = 0; i < 32; i++)
			{
				Assert.IsFalse(empty[i]);
			}
			Assert.AreEqual("00000000000000000000000000000000", empty.ToString());
			Assert.AreEqual(0, (int)empty);
			Assert.AreEqual(0, empty.TrueFlagCount);
		}

		[TestMethod]
		public void BitFlags32_CanSetEachFlag()
		{
			var empty = BitFlags32.Empty;

			for (int i = 0; i < 32; i++)
			{
				Assert.IsFalse(empty[i]);
				var it = empty.On(i);
				Assert.IsFalse(it.IsEmpty);
				Assert.IsTrue(it[i]);
				var zeros = "00000000000000000000000000000000".ToCharArray();
				zeros[zeros.Length - (i + 1)] = '1';
				Assert.AreEqual(new String(zeros), it.ToString());
			}
		}
	}
}
