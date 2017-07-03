using System;
using System.Collections.Specialized;
using System.Configuration;
using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.TaxService;
using NetSteps.Data.Entities.Tax;
using NetSteps.Data.Entities.Tax.Avatax;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.AvataxAPI
{
	/// <summary>
	/// Summary description for Util
	/// </summary>
	public class Util
	{
		public static TaxOverrideType GetTaxOverrideType(string taxOverrideType)
		{
			TaxOverrideType oTaxOverrideType = TaxOverrideType.None;
			switch (taxOverrideType)
			{
				case "None": oTaxOverrideType = TaxOverrideType.None;
					break;
				case "TaxAmount": oTaxOverrideType = TaxOverrideType.TaxAmount;
					break;
				case "Exemption": oTaxOverrideType = TaxOverrideType.Exemption;
					break;
				case "TaxDate": oTaxOverrideType = TaxOverrideType.TaxDate;
					break;
				default: break;
			}
			return oTaxOverrideType;
		}

		//TODO: refactor
		/// <summary>
		/// convert a string to datetime
		/// </summary>
		/// <param name="inputValue"></param>
		/// <returns></returns>
		public static DateTime GetDateFromString(string inputString)
		{
			DateTime returnValue = DateTime.Today;
			DateTime.TryParse(inputString, out returnValue);
			return returnValue;
		}
		/// <summary>
		/// convert a string to Decimal
		/// </summary>
		/// <param name="inputValue"></param>
		/// <returns></returns>
		public static Decimal GetDecimalFromString(string inputString)
		{
			Decimal returnValue = 0.00M;
			Decimal.TryParse(inputString, out returnValue);
			return returnValue;
		}
		/// <summary>
		/// convert a string to int
		/// </summary>
		/// <param name="inputValue"></param>
		/// <returns></returns>
		public static int GetIntFromString(string inputString)
		{
			int returnValue = 0;
			int.TryParse(inputString, out returnValue);
			return returnValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputValue"></param>
		/// <returns></returns>
		public static bool GetBooleanFromString(string inputString)
		{
			bool returnValue = false;
			bool.TryParse(inputString, out returnValue);
			return returnValue;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="resultCode"></param>
		/// <returns></returns>
		public static string ConvertResultCodeToString(SeverityLevel resultCode)
		{
			string retValue = string.Empty;
			switch (resultCode)
			{
				case SeverityLevel.Success:
					retValue = "Success";
					break;
				case SeverityLevel.Error:
					retValue = "Error";
					break;
				case SeverityLevel.Warning:
					retValue = "Warning";
					break;
				case SeverityLevel.Exception:
					retValue = "Exception";
					break;
			}
			return retValue;
		}
		/// <summary>
		/// Read config values from sectionName by key
		/// </summary>
		/// <param name="sectionName"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetConfigValues(string sectionName, string key)
		{
			string value = string.Empty;
			try
			{
				value = ((NameValueCollection)ConfigurationManager.GetSection(sectionName))[key];
			}
			catch (ConfigurationErrorsException)
			{
				value = string.Empty;
			}
			return value;
		}

		/// <summary>
		/// Helper method that will find the Avatax is enabled for or not
		/// </summary>
		public static bool IsAvataxEnabled()
		{
			ITaxService svc = Create.New<ITaxService>();
			return svc is AvataxCalculator;
		}
	}

}
