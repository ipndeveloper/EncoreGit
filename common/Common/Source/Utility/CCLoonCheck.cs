using System;
using System.Collections.Generic;

namespace NetSteps.Common.Utility
{
	// This does some basic checking on the charge information to help ensure it will go through
	public class CCLoonCheck
	{
		private string CardNum;
		private int ExpMonth;
		private int ExpYear;
		private double Amount;
		private List<CCInfo> CreditCards;

		private enum CardTypes
		{
			UNKNOWN,
			MASTERCARD,
			VISA,
			AMEX,
			DINERS,
			DISCOVER,
			ENROUTE,
			JCB
		}

		private enum CheckAlgorithms
		{
			ANY,
			MOD10
		}

		private class CCInfo
		{
			public string Prefix;
			public int Length;
			public CheckAlgorithms Algorithm;
			public CardTypes Type;

			public CCInfo(string prefix, int length, CheckAlgorithms algorithm, CardTypes type)
			{
				Prefix = prefix;
				Length = length;
				Algorithm = algorithm;
				Type = type;
			}
		}

		public CCLoonCheck(string cardNum, int expMonth, int expYear, double amount)
		{
			// remove whitespace from cardnum
			cardNum = cardNum.Replace(" ", string.Empty);
			cardNum = cardNum.Replace("-", string.Empty);
			CardNum = cardNum.Trim();

			ExpMonth = expMonth;
			ExpYear = expYear;

			Amount = amount;

			CreditCards = new List<CCInfo>();
			// NOTE: This list is in order by the length of the prefix...This is extremely important so
			// that we we loop through we match on the longest prefix first (so we don't wind up thinking
			// we have a JCB when we in fact have a DINERS because they both start with "3")
			CreditCards.Add(new CCInfo("6011", 16, CheckAlgorithms.MOD10, CardTypes.DISCOVER));

			CreditCards.Add(new CCInfo("2014", 15, CheckAlgorithms.ANY, CardTypes.ENROUTE));
			CreditCards.Add(new CCInfo("2149", 15, CheckAlgorithms.ANY, CardTypes.ENROUTE));

			CreditCards.Add(new CCInfo("2131", 16, CheckAlgorithms.MOD10, CardTypes.JCB));
			CreditCards.Add(new CCInfo("1800", 16, CheckAlgorithms.MOD10, CardTypes.JCB));

			CreditCards.Add(new CCInfo("300", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));
			CreditCards.Add(new CCInfo("301", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));
			CreditCards.Add(new CCInfo("302", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));
			CreditCards.Add(new CCInfo("303", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));
			CreditCards.Add(new CCInfo("304", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));
			CreditCards.Add(new CCInfo("305", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));

			CreditCards.Add(new CCInfo("51", 16, CheckAlgorithms.MOD10, CardTypes.MASTERCARD));
			CreditCards.Add(new CCInfo("52", 16, CheckAlgorithms.MOD10, CardTypes.MASTERCARD));
			CreditCards.Add(new CCInfo("53", 16, CheckAlgorithms.MOD10, CardTypes.MASTERCARD));
			CreditCards.Add(new CCInfo("54", 16, CheckAlgorithms.MOD10, CardTypes.MASTERCARD));
			CreditCards.Add(new CCInfo("55", 16, CheckAlgorithms.MOD10, CardTypes.MASTERCARD));

			CreditCards.Add(new CCInfo("34", 15, CheckAlgorithms.MOD10, CardTypes.AMEX));
			CreditCards.Add(new CCInfo("37", 15, CheckAlgorithms.MOD10, CardTypes.AMEX));

			CreditCards.Add(new CCInfo("36", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));
			CreditCards.Add(new CCInfo("38", 14, CheckAlgorithms.MOD10, CardTypes.DINERS));

			CreditCards.Add(new CCInfo("3", 16, CheckAlgorithms.MOD10, CardTypes.JCB));

			CreditCards.Add(new CCInfo("4", 13, CheckAlgorithms.MOD10, CardTypes.VISA));
			CreditCards.Add(new CCInfo("4", 16, CheckAlgorithms.MOD10, CardTypes.VISA));
		}

		public bool Check(out string error)
		{
			error = string.Empty;

			// Verify the amount
			if (Amount < 0)
			{
				error = "Invalid Amount.";
				return false;
			}

			// Verify the Expiration Date
			if (ExpYear < DateTime.Now.Year ||
				(ExpYear == DateTime.Now.Year && ExpMonth < DateTime.Now.Month))
			{
				error = "Invalid Expiration Date";
				return false;
			}

			// Verify the account number
			foreach (CCInfo card in CreditCards)
			{
				if (CardNum.StartsWith(card.Prefix) &&
					CardNum.Length == card.Length &&
					CheckAlgorithm(CardNum, card.Algorithm))
				{
					return true; // we found a valid card
				}
			}

			error = "Invalid Credit Card Number.";
			return false;
		}

		private bool CheckAlgorithm(string cardNum, CheckAlgorithms algorithm)
		{
			// If the algorithm is any then just return true - we have no way to verify
			if (algorithm == CheckAlgorithms.ANY)
				return true;

			/*
				* info from http://www.beachnet.com/~hstiles/cardtype.html
				* 
				LUHN Formula (Mod 10) for Validation of Primary Account Number 
				The following steps are required to validate the primary account number:

				Step 1: Double the value of alternate digits of the primary account number beginning with the 
						second digit from the right (the first right--hand digit is the check digit.) 

				Step 2: Add the individual digits comprising the products obtained in Step 1 to each of the unaffected 
						digits in the original number. 

				Step 3: The total obtained in Step 2 must be a number ending in zero (30, 40, 50, etc.) 
						for the account number to be validated. 

				For example, to validate the primary account number 49927398716: 

				Step 1: 

						4 9 9 2 7 3 9 8 7 1 6
						 x2  x2  x2  x2  x2 
				------------------------------
						 18   4   6  16   2

				Step 2: 4 +(1+8)+ 9 + (4) + 7 + (6) + 9 +(1+6) + 7 + (2) + 6 

				Step 3: Sum = 70 : Card number is validated 

				Note: Card is valid because the 70/10 yields no remainder

				Make sure you have:
					Have started with the rightmost digit (including the check digit) (figure odd and even based upon the rightmost digit being odd, regardless of the length of the Credit Card.) ALWAYS work right to left. 
					The check digit counts as digit #1 (assuming that the rightmost digit is the check digit) and is not doubled 
					Double every second digit (starting with digit # 2 from the right) 
					Remember that when you double a number over 4, (6 for example) you don't add the result to your total, but rather the sum of the digits of the result (in the above example 6*2=12 so you would add 1+2 to your total (not 12). 
					Always include the Visa or M/C/ prefix. 
				*/

			int total = 0;
			for (int index = 0, x = cardNum.Length - 1; x >= 0; x--, index++)
			{
				// every other digit simply add the digit to the total
				if (index % 2 == 0)
					total += int.Parse(cardNum[x].ToString());
				else
				{
					// every other digit add the digit * 2...unless it's over 10 then add the value of each digit in the
					// result (i.e. 12 = 1+2, 18 = 1+8
					int value = int.Parse(cardNum[x].ToString()) * 2;
					if (value > 10)
						total += 1 + (value - 10);
					else
						total += value;
				}
			}

			// if the sum mod 10 = 0 then the # is valid
			return (total % 10) == 0;
		}
	}
}
