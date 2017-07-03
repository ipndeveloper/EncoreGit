using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.SSO.Common
{
	/// <summary>
	/// Extension methods that are the same regardless of implementation
	/// </summary>
	public static class AuthenticatorExtensions
	{
		#region Encode
		/// <summary>
		/// Returns a string of encoded text from integer and default key
		/// </summary>
		/// <param name="service"></param>
		/// <param name="toEncode"></param>
		/// <returns></returns>
		public static string EncodeInteger(this IAuthenticator service, int toEncode)
		{
			return service.EncodeInteger(toEncode, null);
		}



		/// <summary>
		/// Returns a string of encoded text from integer and default key
		/// </summary>
		/// <param name="service"></param>
		/// <param name="toEncode"></param>
		/// <param name="exceptionHandler"></param>
		/// <returns></returns>
		public static string EncodeInteger(this IAuthenticator service, int toEncode, Action<Exception> exceptionHandler)
		{
			ISingleSignOnModel model = Create.New<ISingleSignOnModel>();
			model.DecodedText = toEncode.ToString();

			service.Encode(model, exceptionHandler);
			return model.EncodedText;
		}



		/// <summary>
		/// Encodes DecodedText and stores the value in DecodedText
		/// </summary>
		/// <param name="service"></param>
		/// <param name="model"></param>
		public static void Encode(this IAuthenticator service, ISingleSignOnModel model)
		{
			service.Encode(model, null);
		}



		/// <summary>
		/// Encodes DecodedText and stores the value in DecodedText
		/// </summary>
		/// <param name="service"></param>
		/// <param name="model"></param>
		public static void Encode(this IAuthenticator service, ISingleSignOnTimeStampedModel model)
		{
			service.Encode(model, null);
		}



		/// <summary>
		/// Encrypts the accountID into a string using pre-supplied key and salt (Most likely in the configuration file). 
		/// </summary>
		/// <param name="service"></param>
		/// <param name="textToEncrypt"></param>
		/// <param name="exceptionHandler"></param>
		/// <returns></returns>
		public static string Encode(this IAuthenticator service, string textToEncrypt, Action<Exception> exceptionHandler)
		{
			if (string.IsNullOrWhiteSpace(textToEncrypt))
				return "";

			ISingleSignOnModel model = Create.New<ISingleSignOnModel>();
			model.DecodedText = textToEncrypt;

			service.Encode(model, exceptionHandler);
			return model.EncodedText;
		}



		/// <summary>
		/// Encrypts the accountID into a string using pre-supplied key and salt (Most likely in the configuration file). 
		/// </summary>
		/// <param name="service"></param>
		/// <param name="textToEncrypt"></param>
		/// <returns></returns>
		public static string Encode(this IAuthenticator service, string textToEncrypt)
		{
			return service.Encode(textToEncrypt, null);
		}

		#endregion


		#region Decode
		/// <summary>
		/// Decodes encoded text and returns an integer using default key
		/// </summary>
		/// <param name="service"></param>
		/// <param name="toDecode"></param>
		/// <returns></returns>
		public static int DecodeInteger(this IAuthenticator service, string toDecode)
		{
			return service.DecodeInteger(toDecode, null);
		}

		/// <summary>
		/// Decodes encoded text and returns an integer using default key
		/// </summary>
		/// <param name="service"></param>
		/// <param name="toDecode"></param>
		/// <param name="exceptionHandler"></param>
		/// <returns></returns>
		public static int DecodeInteger(this IAuthenticator service, string toDecode, Action<Exception> exceptionHandler)
		{
			ISingleSignOnModel model = Create.New<ISingleSignOnModel>();
			model.EncodedText = toDecode;
			service.Decode(model, exceptionHandler);

			int returnValue = 0;
			int.TryParse(model.DecodedText, out returnValue);

			return returnValue;
		}

		/// <summary>
		/// Decrypts the EncodedText and stores the value in DecodedText
		/// </summary>
		/// <param name="service"></param>
		/// <param name="model"></param>
		public static void Decode(this IAuthenticator service, ISingleSignOnModel model)
		{
			service.Decode(model, null);
		}



		/// <summary>
		/// Decrypts the EncodedText and stores the value in DecodedText
		/// </summary>
		/// <param name="service"></param>
		/// <param name="model"></param>
		public static void Decode(this IAuthenticator service, ISingleSignOnTimeStampedModel model)
		{
			service.Decode(model, null);
		}



		/// <summary>
		/// Decrypts the accountID from the provided string using the pre-supplied key and salt (Most likely in the configuration file).
		/// </summary>
		/// <param name="service"></param>
		/// <param name="textToDecode"></param>
		/// <param name="exceptionHandler"></param>
		/// <returns></returns>
		public static string Decode(this IAuthenticator service, string textToDecode, Action<Exception> exceptionHandler)
		{
			if (string.IsNullOrWhiteSpace(textToDecode))
				return "";


			ISingleSignOnModel model = Create.New<ISingleSignOnModel>();
			model.EncodedText = textToDecode;
			service.Decode(model, exceptionHandler);

			return model.DecodedText;
		}


		/// <summary>
		/// Decrypts the accountID from the provided string using the pre-supplied key and salt (Most likely in the configuration file).
		/// </summary>
		/// <param name="service"></param>
		/// <param name="textToDecode"></param>
		/// <returns></returns>
		public static string Decode(this IAuthenticator service, string textToDecode)
		{
			return service.Decode(textToDecode, null);
		}
		#endregion
	}
}
