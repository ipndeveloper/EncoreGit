using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.Common.ModelConcrete;

namespace NetSteps.Promotions.Common.Tests
{
	[TestClass]
	public class PromotionQualificationResultTest
	{
		[TestMethod]
		public void PromotionQualificationResult_should_return_no_match_if_NoMatch_result_is_anded_with_AllCustomers()
		{
			var match = PromotionQualificationResult.NoMatch;
			match.BitwiseAnd(PromotionQualificationResult.MatchForAllCustomers);
			Assert.IsFalse(match);
			match = PromotionQualificationResult.MatchForAllCustomers;
			match.BitwiseAnd(PromotionQualificationResult.NoMatch);
			Assert.IsFalse(match);
		}

		[TestMethod]
		public void PromotionQualificationResult_should_return_no_match_if_NoMatch_result_is_anded_with_SelectCustomers()
		{
			List<int> used = new List<int>();
			int randomYes = getUniqueRandomNumber(used);
			int randomNo = getUniqueRandomNumber(used);
			var match = PromotionQualificationResult.NoMatch;
			match.BitwiseAnd(PromotionQualificationResult.MatchForSelectCustomers(randomYes));
			Assert.IsFalse(match);
			match = PromotionQualificationResult.MatchForSelectCustomers(randomYes);
			match.BitwiseAnd(PromotionQualificationResult.NoMatch);
			Assert.IsFalse(match);
		}

		[TestMethod]
		public void PromotionQualificationResult_should_return_no_match_if_NoMatch_result_is_anded_with_NoMatch()
		{
			var match = PromotionQualificationResult.NoMatch;
			match.BitwiseAnd(PromotionQualificationResult.NoMatch);
			Assert.IsFalse(match);
			match = PromotionQualificationResult.NoMatch;
			match.BitwiseAnd(PromotionQualificationResult.NoMatch);
			Assert.IsFalse(match);
		}

		[TestMethod]
		public void PromotionQualificationResult_should_return_match_if_AllCustomers_result_is_anded_with_AllCustomers()
		{
			var match = PromotionQualificationResult.MatchForAllCustomers;
			match.BitwiseAnd(PromotionQualificationResult.MatchForAllCustomers);
			Assert.IsTrue(match);
			Assert.IsTrue(match.MatchForCustomerAccountID(new Random().Next()));
			match = PromotionQualificationResult.MatchForAllCustomers;
			match.BitwiseAnd(PromotionQualificationResult.MatchForAllCustomers);
			Assert.IsTrue(match);
			Assert.IsTrue(match.MatchForCustomerAccountID(new Random().Next()));
		}

		[TestMethod]
		public void PromotionQualificationResult_should_return_match_if_AllCustomers_result_is_anded_with_SelectCustomers_and_match_for_the_select_customers()
		{
			List<int> used = new List<int>();
			int randomYes = getUniqueRandomNumber(used);
			int randomNo = getUniqueRandomNumber(used);
			var match = PromotionQualificationResult.MatchForAllCustomers;
			match.BitwiseAnd(PromotionQualificationResult.MatchForSelectCustomers(randomYes));
			Assert.IsTrue(match);
			Assert.IsTrue(match.MatchForCustomerAccountID(randomYes));
			Assert.IsFalse(match.MatchForCustomerAccountID(randomNo));
			match = PromotionQualificationResult.MatchForSelectCustomers(randomYes);
			match.BitwiseAnd(PromotionQualificationResult.MatchForAllCustomers);
			Assert.IsTrue(match);
			Assert.IsTrue(match.MatchForCustomerAccountID(randomYes));
			Assert.IsFalse(match.MatchForCustomerAccountID(randomNo));
		}

		[TestMethod]
		public void PromotionQualificationResult_should_return_match_if_Select_result_is_anded_with_SelectCustomers_and_match_for_the_intersecting_select_customers()
		{
			List<int> used = new List<int>();
			int randomYes = getUniqueRandomNumber(used);
			int randomNo1 = getUniqueRandomNumber(used);
			int randomNo2 = getUniqueRandomNumber(used);
			var match = PromotionQualificationResult.MatchForSelectCustomers(new int[] {randomYes, randomNo1});
			match.BitwiseAnd(PromotionQualificationResult.MatchForSelectCustomers(new int[] {randomYes, randomNo2}));
			Assert.IsTrue(match);
			Assert.IsTrue(match.MatchForCustomerAccountID(randomYes));
			Assert.IsFalse(match.MatchForCustomerAccountID(randomNo1));
			Assert.IsFalse(match.MatchForCustomerAccountID(randomNo2));
			match = PromotionQualificationResult.MatchForSelectCustomers(new int[] {randomYes, randomNo2});
			match.BitwiseAnd(PromotionQualificationResult.MatchForSelectCustomers(new int[] {randomYes, randomNo1}));
			Assert.IsTrue(match);
			Assert.IsTrue(match.MatchForCustomerAccountID(randomYes));
			Assert.IsFalse(match.MatchForCustomerAccountID(randomNo1));
			Assert.IsFalse(match.MatchForCustomerAccountID(randomNo2));
		}

		private int getUniqueRandomNumber(IList<int> used)
		{
			int newRandom = new Random().Next();
			while (used.Contains(newRandom))
				newRandom = new Random().Next();
			used.Add(newRandom);
			return newRandom;
		}
	}
}
