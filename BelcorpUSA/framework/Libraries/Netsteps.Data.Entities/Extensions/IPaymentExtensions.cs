namespace NetSteps.Data.Entities.Extensions
{
	using System;
	using System.Text;

	using NetSteps.Common.Extensions;
	using NetSteps.Data.Entities.Business.Interfaces;

	/// <summary>
	/// Author: John Egbert
	/// Description: IPayment Extensions
	/// Created: 10-05-2010
	/// </summary>
	public static class IPaymentExtensions
	{
		public enum DisplayTypes
		{
			Web,
			QAS,
			Windows
		}

		public static string ToDisplay(this IPayment payment)
		{
			return payment.ToDisplay(DisplayTypes.Web, ApplicationContext.Instance.ApplicationDefaultCultureInfo, false);
		}

		public static string ToDisplay(this IPayment payment, IFormatProvider dateCulture)
		{
			return payment.ToDisplay(DisplayTypes.Web, dateCulture, false);
		}

		public static string ToDisplay(this IPayment payment, bool showPhone, bool showAddress, bool showProfileName = false)
		{
			return payment.ToDisplay(DisplayTypes.Web, ApplicationContext.Instance.ApplicationDefaultCultureInfo, showPhone, showAddress, showProfileName);
		}

		public static string ToDisplay(this IPayment payment, DisplayTypes type, IFormatProvider dateCulture, bool showPhone, bool showAddress = true, bool showProfileName = false)
		{
			if (payment == null)
				return NetSteps.Common.Globalization.Translation.GetTerm("NA", "NA");

			string delimiter = string.Empty;
			switch (type)
			{
				case DisplayTypes.Web: delimiter = "<br />"; break;
				case DisplayTypes.QAS: delimiter = "|"; break;
				case DisplayTypes.Windows: delimiter = Environment.NewLine; break;
			}
			StringBuilder builder = new StringBuilder();

			if (showProfileName && !payment.BillingAddress.ProfileName.IsNullOrEmpty())
				builder.Append(payment.BillingAddress.ProfileName).Append(delimiter);

			builder.Append(payment.NameOnCard).Append(delimiter);

			if (!payment.ExpirationDate.IsNullOrEmpty())
				builder.Append(payment.ExpirationDate.ToExpirationStringDisplay(dateCulture)).Append(delimiter);
			if (!string.IsNullOrEmpty(payment.DecryptedAccountNumber))
				builder.Append(payment.DecryptedAccountNumber.MaskString(4)).Append(delimiter);
			if (showAddress)
			{
				if (!payment.BillingAddress.IsEmpty())
					builder.Append(payment.BillingAddress.ToDisplay()).Append(delimiter);
			}
			return builder.ToString();
		}
	}
}