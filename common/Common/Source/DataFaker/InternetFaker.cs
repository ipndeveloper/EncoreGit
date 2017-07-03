using System.Collections.Generic;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.DataFaker
{
	using System;

	using Random = NetSteps.Common.Random;

	/// <summary>
	/// Author: John Egbert
	/// Description: Methods to generate 'Fake' data for use in testing.
	/// Created: 03-18-2009
	/// </summary>
	public static class InternetFaker
	{
		private static List<string> _domains = "yahoo.com, gmail.com, privacy.net, webmail.com, sn.com, hotmail.com, example.com".ToStringList();
		private static List<string> _pageName = "About Us, Company, News, Meetings, Announcements, Products, Events, Calendar Events, Help, Technical Support, Contact Us, Q & A, Home, Product Line, Products Landing Page, Help, Terms & Conditions, Archives, Document Library, Privacy Policy".ToStringList();
		private static List<string> _weakPasswords = "cash, success, health, sunshine, angel, winner, welcome, money, flower".ToStringList();

		public static string Domain()
		{
			return _domains.GetRandom().ToCleanString();
		}

		public static string Email()
		{
			if (Random.Next(5) == 2)
				return UserName().ToLower() + Random.Next(100000, 999999).ToString() + "@" + Domain();
			else
				return NameFaker.LastName().ToLower() + Random.Next(100000, 999999).ToString() + "@" + Domain();
		}

		public static string Email(Name name)
		{
			return name.FirstName.ToCleanString() + name.LastName.Substring(0, 1).ToCleanString() + Random.Next(100000, 999999).ToString() + "@" + Domain();
		}

		public static string Url()
		{
			return "http://www." + Domain();
		}

		public static string PageName()
		{
			return _pageName.GetRandom().ToCleanString();
		}

		public static string UserName()
		{
			return Guid.NewGuid().ToString("N"); //NameFaker.FirstName().Substring(0, 1) + NameFaker.LastName() + Guid.NewGuid().ToString("N"); // + Random.Next(100000, 999999).ToString();
		}
		public static string UserName(Name name)
		{
			return name.FirstName.Substring(0, 1).ToCleanString() + name.LastName.ToCleanString();
		}

		public static string PasswordWeak()
		{
			return _weakPasswords.GetRandom().ToCleanString();
		}
	}
}
