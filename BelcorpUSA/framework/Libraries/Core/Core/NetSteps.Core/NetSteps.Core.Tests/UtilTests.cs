using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Encore.Core.Tests
{
	[TestClass]
	public class UtilTests
	{
		public void InternTester(string str1, string str2, string str3)
		{
			Assert.IsTrue(string.IsInterned(str1) == null);
			Assert.IsTrue(string.IsInterned(str2) == null);
			Assert.IsTrue(string.IsInterned(str3) == null);

			Assert.IsFalse(((object)str1) == ((object)str2));
			Assert.IsFalse(((object)str1) == ((object)str3));
			Assert.IsFalse(((object)str2) == ((object)str3));

			string internedStr1 = str1.InternIt();
			string internedStr2 = str2.InternIt();
			string internedStr3 = str3.InternIt();

			bool str1IsInterned = string.IsInterned(internedStr1) != null && ((object)internedStr1) == ((object)string.IsInterned(internedStr1));
			Assert.IsTrue(str1IsInterned);

			bool str2IsInterned = string.IsInterned(internedStr2) != null && ((object)internedStr2) == ((object)string.IsInterned(internedStr2));
			Assert.IsTrue(str2IsInterned);

			bool str3IsInterned = string.IsInterned(internedStr3) != null && ((object)internedStr3) == ((object)string.IsInterned(internedStr3));
			Assert.IsTrue(str3IsInterned);

			Assert.IsTrue(((object)internedStr1) == ((object)internedStr2));
			Assert.IsTrue(((object)internedStr1) == ((object)internedStr3));
			Assert.IsTrue(((object)internedStr2) == ((object)internedStr3));
		}

		[TestMethod]
		public void InternIt()
		{
			string it = "it".InternIt();
			Assert.ReferenceEquals("it", it);

			string baseStr = "abc";
			bool isBaseInterned = string.IsInterned(baseStr) != null;
			Assert.IsTrue(isBaseInterned);

			string baseStr2 = "abc";
			Assert.IsTrue(((object)baseStr) == ((object)baseStr2));

			{
				string str1 = (new StringBuilder()).Append("a").Append("b").Append("c").Append("d").ToString();
				string str2 = (new StringBuilder()).Append("a").Append("b").Append("c").Append("d").ToString();
				string str3 = (new StringBuilder()).Append("a").Append("b").Append("c").Append("d").ToString();
				InternTester(str1, str2, str3);
			}

			{
				string typeName1 = (typeof(string)).AssemblyQualifiedName;
				string typeName2 = (typeof(string)).AssemblyQualifiedName;
				string typeName3 = (typeof(string)).AssemblyQualifiedName;
				InternTester(typeName1, typeName2, typeName3);
			}
		}
	}
}
