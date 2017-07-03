using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.SSO.Common;
using NetSteps.Encore.Core.IoC;


namespace NetSteps.SSO.Service.Tests
{
	[TestClass]
	public class AuthenticatorServiceTests
	{
		[TestMethod]
		public void AuthenticatorService_EncodeAccount_DecodeAccount_Model_SameResult()
		{
			//Arrange
			ISingleSignOnModel model = CreateModel("445", null);
			IAuthenticator service = CreateService();

			//Act
			service.Encode(model);
			model.DecodedText = "";
			service.Decode(model);

			//Assert
			Assert.AreEqual("445", model.DecodedText);
		}



		[TestMethod]
		public void AuthenticatorService_EncodeAccount_DecodeAccount_TimeStampedModel_SameResult()
		{
			//Arrange
			DateTime timestamp = DateTime.Now;
			ISingleSignOnTimeStampedModel model = CreateTimeStampedModel("445", null, timestamp);
			IAuthenticator service = CreateService();

			//Act
			service.Encode(model);
			model.DecodedText = "";
			model.TimeStamp = DateTime.MinValue;
			service.Decode(model);

			//Assert
			Assert.AreEqual("445", model.DecodedText);
			Assert.AreEqual(timestamp, model.TimeStamp);
		}
		
		
		
		[TestMethod]
		public void AuthenticatorService_EncodeAccount_DecodeAccount_String_SameResult()
		{
			//Arrange
			string model = "445";
			IAuthenticator service = CreateService();

			//Act
			string result = service.Encode(model);
			result = service.Decode(result);

			//Assert
			Assert.AreEqual("445", result);
		}

		
		
		[TestMethod]
		public void AuthenticatorService_EncodeInteger_DecodeInteger_SameResult()
		{
			//Arrange
			IAuthenticator service = CreateService();
			int testValue = 5;

			//Act
			string encodedString = service.EncodeInteger(testValue);
			int result = service.DecodeInteger(encodedString);

			//Assert
			Assert.AreEqual(result, testValue);
		}

		
		
		private ISingleSignOnModel CreateModel(string decodedText, string encodedText)
		{
			ISingleSignOnModel model = Create.New<ISingleSignOnModel>();
			model.DecodedText = decodedText;
			model.EncodedText = encodedText;
			return model;
		}



		private ISingleSignOnTimeStampedModel CreateTimeStampedModel(string decodedText, string encodedText, DateTime timeStamp)
		{
			ISingleSignOnTimeStampedModel model = Create.New<ISingleSignOnTimeStampedModel>();
			model.DecodedText = decodedText;
			model.EncodedText = encodedText;
			model.TimeStamp = timeStamp;
			return model;
		}



		private IAuthenticator CreateService()
		{
			return Create.New<IAuthenticator>(typeof(AuthenticatorService));
		}
	}
}
