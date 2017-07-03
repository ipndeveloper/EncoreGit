using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.SSO.Common;
using System.Globalization;
using System.Web;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;
using NetSteps.SSO.Common.Config;
using System.Diagnostics.Contracts;

namespace NetSteps.SSO.Service
{
	/// <summary>
	/// Singleton class that encrypts/decrypts accountIDs for use in URLs
	/// </summary>
	[ContainerRegister(typeof(IAuthenticator), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class AuthenticatorService : IAuthenticator
	{
		byte[] CryptoKey { get; set; }
		string Salt { get; set; }

		#region Constructors

		/// <summary>
		/// Creates an AuthenticatorService using the default Configuration
		/// </summary>
		public AuthenticatorService()
			: this(SsoConfigurationSection.Current)
		{
		}

		/// <summary>
		/// Creates an AuthenticatorService using a Configuration parameter
		/// </summary>
		/// <param name="config"></param>
		public AuthenticatorService(SsoConfigurationSection config)
		{
			Contract.Requires<ArgumentNullException>(config != null);

			this.CryptoKey = Encoding.UTF8.GetBytes(config.KeyName);
			this.Salt = config.Salt;
		}
		#endregion


		#region Encode

		/// <summary>
		/// Encrypts the AccountID and stores the value in the same model
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler"></param>
		public void Encode(ISingleSignOnModel model, Action<Exception> exceptionHandler)
		{
			if (string.IsNullOrWhiteSpace(model.DecodedText))
			{
				model.EncodedText = "";
				return;
			}

			try
			{
				model.EncodedText = EncryptionUtilities.EncryptTripleDES(CryptoKey, model.DecodedText.ToString(), Salt);
			}
			catch (Exception ex)
			{
				if (exceptionHandler != null)
					exceptionHandler.Invoke(ex);
				else
					throw;
			}
		}



		/// <summary>
		/// Encrypts the AccountID and stores the value in the same model
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler"></param>
		public void Encode(ISingleSignOnTimeStampedModel model, Action<Exception> exceptionHandler)
		{
			if (string.IsNullOrWhiteSpace(model.DecodedText))
			{
				model.EncodedText = "";
				return;
			}

			try
			{
				model.EncodedText = EncryptionUtilities.EncryptTripleDES(CryptoKey, new TimeStampedString(model.DecodedText, model.TimeStamp), Salt);
			}
			catch (Exception ex)
			{
				if (exceptionHandler != null)
					exceptionHandler.Invoke(ex);
				else
					throw;
			}
		}

		#endregion

		#region Decode

		/// <summary>
		/// Decrypts the EncodedText and stores the value in AccountID
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler"></param>
		public void Decode(ISingleSignOnModel model, Action<Exception> exceptionHandler)
		{
			if (string.IsNullOrWhiteSpace(model.EncodedText))
			{
				model.DecodedText = "";
				return;
			}

			try
			{
				string decryptToken = EncryptionUtilities.DecryptTripleDES(CryptoKey, model.EncodedText, Salt);
				model.DecodedText = decryptToken;
			}
			catch (Exception ex)
			{
				if (exceptionHandler != null)
					exceptionHandler.Invoke(ex);
				else
					throw;
			}
		}



		/// <summary>
		/// Decrypts the EncodedText and stores the value in AccountID
		/// </summary>
		/// <param name="model"></param>
		/// <param name="exceptionHandler"></param>
		public void Decode(ISingleSignOnTimeStampedModel model, Action<Exception> exceptionHandler)
		{
			if (string.IsNullOrWhiteSpace(model.EncodedText))
			{
				model.DecodedText = "";
				model.TimeStamp = DateTime.MinValue;
				return;
			}

			try
			{
				TimeStampedString decryptToken = EncryptionUtilities.DecryptTripleDES<TimeStampedString>(CryptoKey, model.EncodedText, Salt);
				model.DecodedText = decryptToken.MyString;
				model.TimeStamp = decryptToken.TimeStamp;
			}
			catch (Exception ex)
			{
				if (exceptionHandler != null)
					exceptionHandler.Invoke(ex);
				else
					throw;
			}
		}
		
		#endregion


		private class TimeStampedString
		{
			public string MyString { get; set; }

			public DateTime TimeStamp { get; set; }

			public TimeStampedString( string mystring, DateTime timeStamp)
			{
				this.MyString = mystring;
				this.TimeStamp = timeStamp;
			}
		}
	}
}
