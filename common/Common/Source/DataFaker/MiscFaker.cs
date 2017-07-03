using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Comparer;
using NetSteps.Common.Extensions;

namespace NetSteps.Common.DataFaker
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Methods to generate 'Fake' data for use in testing.
	/// Created: 03-18-2009
	/// </summary>
	public static class MiscFaker
	{
		private static List<string> _eventName = "Annual Convention, Business Meeting, Board Meeting, Training Meeting, Regional Meeting".ToStringList();
		private static List<string> _personImage = "billherf.jpg, domingo.jpg, napoleon.jpg, Seth.JPG".ToStringList();
		private static List<string> _randomImage = "j0409255.jpg, j0410185.jpg, j0411768.jpg, j0426646.JPG, j0433093.jpg, j0438548.jpg".ToStringList();
		private static List<string> _randomProduct = "Scrub, Anti-Age Regimen, Night Pills Sooth, Essentials, Enhancements, Night Serum, Whitening, Release, Greens, Argi9".ToStringList();
		private static List<string> _randomProductLine = "Skin Care, Hair Care, Weight Loss, Daily Essentials, Fitness".ToStringList();
		private static List<string> _randomNewsType = "Corporate Announcement, Regional News, International News, Newsletter".ToStringList();
		private static List<string> _randomFileType = "PDF, MOV, PNG, BMP, JPG, SWF, DOC".ToStringList();
		private static List<string> _randomArchiveType = "PDF, Movie, Image, Conference Call, Form, Application".ToStringList();

		private static List<string> _randomBusinessNameParts = "Ran, Tri, Am, Cycle, Ben, Gen, Con, Com, Co, Art, Tech, X, Bi, Rock, Tran".ToStringList();
		private static List<string> _randomBusinessPostFix = "LLC, Inc., Co, .com".ToStringList();

		public static string EventName()
		{
			return (Random.GetBoolean()) ? _eventName.GetRandom().ToCleanString() : "Meeting with " + DataFaker.NameFaker.Name();
		}

		public static string PersonImage()
		{
			return _personImage.GetRandom().ToCleanString();
		}

		public static string GetRandomImage()
		{
			return _randomImage.GetRandom().ToCleanString();
		}

		public static double GetPrice(int min, int max)
		{
			double number = double.Parse(string.Format("{0}.{1}", Random.Next(min, max), Random.Next(1, 99)));
			if (number > max)
				number -= 1;
			return number;
		}

		public static string ActiveStatus()
		{
			return (Random.GetBoolean()) ? "Active" : "Inactive";
		}

		public static string ProductName()
		{
			return _randomProduct.GetRandom().ToCleanString();
		}

		public static string ProductLineName()
		{
			return _randomProductLine.GetRandom().ToCleanString();
		}

		public static string NewsType()
		{
			return _randomNewsType.GetRandom();
		}

		public static string FileType()
		{
			return _randomFileType.GetRandom();
		}

		public static string ArchiveType()
		{
			return _randomArchiveType.GetRandom();
		}

		public static string GetCreditCard()
		{
			return Random.GetBoolean() ? "4747474747474747" : "4444333322221111";
		}

		public static DateTime GetExpirationDate()
		{
			DateTime expirationDate = new DateTime(DateTime.Now.ApplicationNow().AddYears(Random.Next(1, 10)).Year, Random.Next(1, 12), 1);
			return expirationDate;
		}

		public static string GetTrackingNumber()
		{
			return string.Format("1ZV503A{0}{1}", Random.Next(1030000, 9030000), Random.Next(15620, 95620));
		}

		public static decimal DollarAmount()
		{
			return NetSteps.Common.Random.Next(50, 100);
		}
		public static decimal ItemDollarAmount()
		{
			return NetSteps.Common.Random.Next(50, 120);
		}
		public static decimal OrderDollarAmount()
		{
			return NetSteps.Common.Random.Next(120, 600);
		}

		public static string SocialSecurityNumber()
		{
			return StringFaker.Randomize("###-##-####");
		}

		public static void FillObjectWithTestValues(object obj, List<PropertyInfo> ignoreProps = null)
		{
			Type type = obj.GetType();
			var properties = type.GetPropertiesCached();

			List<string> dateTimeNowProperties = new List<string> { "DateModified", "DateCreated", "EnrollmentDate", "EffectiveDate", "StartDate", "DateChanged", "DateCreated", "CommissionDate", "AuditDate", "LogDate", "CreatedDate", "PostedDate", "DatePosted", "DateSignedUp", "CreationDate", "LastUpdated", "DateQuoted", "CompleteDate" };
			List<string> futureDateProperties = new List<string> { "EndDateUTC", "DueDateUTC", "ExpirationDate" };

			Type stringType = typeof(string);
			Type dateTimeType = typeof(DateTime);
			Type dateTimeNullType = typeof(DateTime?);
			Type boolType = typeof(bool);
			Type boolNullType = typeof(bool?);
			Type intType = typeof(int);
			Type intNullType = typeof(int?);
			Type shortType = typeof(short);
			Type shortNullType = typeof(short?);
			Type decimalType = typeof(decimal);
			Type decimalNullType = typeof(decimal?);
			Type doubleType = typeof(double);
			Type doubleNullType = typeof(double?);

			var name = NameFaker.NameObject();

			foreach (var pi in properties)
			{
				bool setTestValue = !(ignoreProps != null && ignoreProps.Contains(pi, new LambdaComparer<PropertyInfo>((x, y) => x.Name == y.Name)));
				if (pi.CanWrite && setTestValue)
				{
					if (pi.PropertyType == stringType)
					{
						if (pi.Name.ToUpper() == "TrackingNumber".ToUpper())
							pi.SetValue(obj, GetTrackingNumber(), null);
						else if (pi.Name.ToUpper() == "Email".ToUpper() || pi.Name.ToUpper() == "EmailAddress".ToUpper())
							pi.SetValue(obj, InternetFaker.Email(), null);
						else if (pi.Name.ContainsIgnoreCase("Phone"))
							pi.SetValue(obj, PhoneFaker.Phone(), null);
						else if (pi.Name.ToUpper() == "Description".ToUpper() || pi.Name.ToUpper() == "Notes".ToUpper())
							pi.SetValue(obj, LoremIpsum.GetSentences(2, true), null);
						else if (pi.Name.ToUpper() == "AccountNumber".ToUpper() && type.Name.ContainsIgnoreCase("Payment"))
							pi.SetValue(obj, GetCreditCard(), null);
						else if (pi.Name.ToUpper() == "AccountNumber".ToUpper() && type.Name.ContainsIgnoreCase("Account"))
							pi.SetValue(obj, SocialSecurityNumber(), null);
						else if (pi.Name.ToUpper() == "FirstName".ToUpper())
							pi.SetValue(obj, name.FirstName, null);
						else if (pi.Name.ToUpper() == "LastName".ToUpper())
							pi.SetValue(obj, name.LastName, null);
						else if (pi.Name.ToUpper() == "MiddleName".ToUpper())
							pi.SetValue(obj, name.MiddleName, null);
					}
					else if (pi.PropertyType == dateTimeType)
					{
						if (!pi.Name.ToUpper().Contains("UTC"))
						{
							if (pi.Name.ToUpper() == "ExpirationDate".ToUpper())
								pi.SetValue(obj, GetExpirationDate(), null);
							else if (pi.Name.ToUpper() == "Birthday".ToUpper())
								pi.SetValue(obj, DateTimeFaker.BirthDay(), null);
							else if (dateTimeNowProperties.ContainsIgnoreCase(pi.Name))
								pi.SetValue(obj, DateTime.Now.ApplicationNow(), null);
							else if (futureDateProperties.ContainsIgnoreCase(pi.Name))
								pi.SetValue(obj, DateTimeFaker.DateTime(DateTime.Now.ApplicationNow(), DateTime.Now.ApplicationNow().AddYears(1)), null);
							else
								pi.SetValue(obj, DateTimeFaker.DateTime(), null);
						}
					}
					else if (pi.PropertyType == dateTimeNullType)
					{
						if (!pi.Name.ToUpper().Contains("UTC"))
						{
							if (pi.Name.ToUpper() == "ExpirationDate".ToUpper())
								pi.SetValue(obj, GetExpirationDate().ToDateTimeNullable(), null);
							else if (pi.Name.ToUpper() == "Birthday".ToUpper())
								pi.SetValue(obj, DateTimeFaker.BirthDay().ToDateTimeNullable(), null);
							else if (dateTimeNowProperties.ContainsIgnoreCase(pi.Name))
								pi.SetValue(obj, DateTime.Now.ApplicationNow().ToDateTimeNullable(), null);
							else if (futureDateProperties.ContainsIgnoreCase(pi.Name))
								pi.SetValue(obj, DateTimeFaker.DateTime(DateTime.Now.ApplicationNow(), DateTime.Now.ApplicationNow().AddYears(1)).ToDateTimeNullable(), null);
							else
								pi.SetValue(obj, DateTimeFaker.DateTime().ToDateTimeNullable(), null);
						}
					}
					else if (pi.PropertyType == boolType || pi.PropertyType == boolNullType)
					{
						if (pi.Name.ToUpper() == "Active".ToUpper())
							pi.SetValue(obj, Random.GetBoolean(), null);
					}
					else if (pi.PropertyType == intType || pi.PropertyType == intNullType || pi.PropertyType == shortType || pi.PropertyType == shortNullType)
					{
						if (pi.Name.ToUpper() == "GenderID".ToUpper() && pi.PropertyType == shortNullType)
							pi.SetValue(obj, Random.Next(1, 2).ToShortNullable(), null);
					}
					else if (pi.PropertyType == decimalType || pi.PropertyType == decimalNullType || pi.PropertyType == doubleType || pi.PropertyType == doubleNullType)
					{
						if (pi.Name.ToUpper() == "Amount".ToUpper() || pi.Name.ToUpper() == "Total".ToUpper())
							pi.SetValue(obj, DollarAmount(), null);
						else
							pi.SetValue(obj, DollarAmount(), null);
					}
				}
			}

		}

		public static string GetBusinessName()
		{
			return string.Format("{0}{1} {2}", _randomBusinessNameParts.GetRandom().ToCleanString(), _randomBusinessNameParts.GetRandom().ToCleanString(), Random.GetBoolean() ? _randomBusinessPostFix.GetRandom().ToCleanString() : string.Empty);
		}
	}
}
