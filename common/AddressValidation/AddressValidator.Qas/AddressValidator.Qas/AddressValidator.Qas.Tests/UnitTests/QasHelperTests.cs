using AddressValidator.Qas.Config;

namespace AddressValidator.Qas.Tests.UnitTests
{
	using System.Collections.Generic;
	using System.Linq;
	using com.qas.proweb;
	using Common;
	using Helpers;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using SupportClass = com.qas.proweb.soap;
	using VerifyLevelType = com.qas.proweb.soap.VerifyLevelType;

	[TestClass]
	public class QasHelperTests
	{

		#region Test Helpers

		public IValidationAddress FakeValidationAddress
		{
			get
			{
				var fakeAddress = new Mock<IValidationAddress>();
				fakeAddress.Setup(a => a.Address1).Returns("1234 Poker St");
				fakeAddress.Setup(a => a.Address2).Returns("Address Line 2");
				fakeAddress.Setup(a => a.Address3).Returns("Address Line 3");
				fakeAddress.Setup(a => a.SubDivision).Returns("Salt Lake");
				fakeAddress.Setup(a => a.MainDivision).Returns("UT");
				fakeAddress.Setup(a => a.PostalCode).Returns("84101");
				fakeAddress.Setup(a => a.Country).Returns("US");

				return fakeAddress.Object;
			}

		}

		public SearchResult FakeSearchResult(VerifyLevelType verifyLevelType)
		{
			var qaAddress = new SupportClass.QAAddressType
																							 {
																								 AddressLine = new[]
                                                                           {
                                                                               new SupportClass.AddressLineType {Line = "Line1"},
                                                                               new SupportClass.AddressLineType {Line = "Line2"}
                                                                           }
																							 };

			var searchResult = new SupportClass.QASearchResult
																														{
																															VerifyLevel = verifyLevelType,
																															QAAddress = qaAddress
																														};



			return new SearchResult(searchResult);
		}


		public AddressLine AddressLine(string lineText)
		{
			return new AddressLine(new SupportClass.AddressLineType
			{
				Line = lineText
			});
		}

		#endregion


		[TestMethod]
		public void TranslateToQasAddress_ValidAddress_ConcatenatesWithLineSeparator()
		{
			// Arrange
			var mock = new Mock<IQuickAddress>();

			var target = new QasHelper(mock.Object);

			// Act
			var result = target.TranslateToQasAddress(FakeValidationAddress);

			// Assert
			const string expected = "1234 Poker St|Address Line 2|Address Line 3|Salt Lake|UT|84101";

			Assert.IsNotNull(result);

			Assert.AreEqual(expected, result);
		}


		[TestMethod]
		public void QasVarifyAddress_ValidAddress_CallsQasOnDemandService()
		{
			// Arrange
			var mock = new Mock<IQuickAddress>();

			var target = new QasHelper(mock.Object);

			// Act
			target.QasVerifyAddress(FakeValidationAddress);

			// Assert
			mock.Verify(m => m.Search(It.IsAny<string>(), It.IsAny<string>(), PromptSet.Types.Default, "MSDCRM2011"), Times.Once());
		}

		[TestMethod]
		public void BuildResultByResultType_VerificationFailed_ReturnsEmptyAddress()
		{
			// Arrange
			var mock = new Mock<IQuickAddress>();
			var validationResult = new Mock<IAddressValidationResult>();


			var target = new QasHelper(mock.Object);

			// Act
			var result = target.BuildResultByResultType(validationResult.Object, FakeSearchResult(VerifyLevelType.None));

			// Assert
			Assert.IsTrue(!result.ValidAddresses.Any());
		}

		[TestMethod]
		public void AssignAddressValue_ContainsAllFields_ReturnsNewPopulatedIValidationAddress()
		{
			// Arrange
			var mock = new Mock<IQuickAddress>();
			IEnumerable<AddressLine> lines = new List<AddressLine>
                                                 {
                                                     AddressLine("Line1"),
                                                     AddressLine("Line2"),
                                                     AddressLine("Line3"),
                                                     AddressLine("Salt Lake"),
                                                     AddressLine("UT"),
                                                     AddressLine("84101")
                                                 };

			var target = new QasHelper(mock.Object);

			// Act
			var result = target.AssignAddressValue(lines.ToArray());

			// Assert
			Assert.AreEqual("Line1", result.Address1);
			Assert.AreEqual("Line2", result.Address2);
			Assert.AreEqual("Salt Lake", result.SubDivision);
			Assert.AreEqual("84101", result.PostalCode);
		}

		[TestMethod]
		public void AssignAddressValue_DoesNotContainAllFields_PopulatesTheGivenFields()
		{
			// Arrange
			var mock = new Mock<IQuickAddress>();
			IEnumerable<AddressLine> lines = new List<AddressLine>
                                                 {
                                                     AddressLine("Line1"),
                                                     AddressLine("Line2"),
                                                     AddressLine("Line3"),
                                                     AddressLine("Salt Lake")
                                                 };

			var target = new QasHelper(mock.Object);

			// Act
			var result = target.AssignAddressValue(lines.ToArray());

			// Assert
			Assert.AreEqual("Line1", result.Address1);
			Assert.AreEqual("Line2", result.Address2);
			Assert.AreEqual("Salt Lake", result.SubDivision);
		}

		[TestMethod]
		public void AssignAddressValue_EmptyAddressLines_ReturnsAnEmptyIValidationAddressInstance()
		{
			// Arrange
			var mock = new Mock<IQuickAddress>();

			var target = new QasHelper(mock.Object);

			IEnumerable<AddressLine> emptyLines = Enumerable.Empty<AddressLine>();

			// Act
			var result = target.AssignAddressValue(emptyLines.ToArray());

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IValidationAddress));

			Assert.IsNull(result.Address1);
			Assert.IsNull(result.PostalCode);
		}

		[TestMethod]
		public void QasAddressValidatorConfig_Should_LoadConfigSection()
		{
			Assert.IsFalse(string.IsNullOrWhiteSpace(QasAddressValidatorConfig.Current.EndpointUrl), "QAS Configuration Error: Section or EndpointUrl property not defined");
			Assert.IsFalse(string.IsNullOrWhiteSpace(QasAddressValidatorConfig.Current.UserName), "QAS Configuration Error: Section or UserName property not defined");
			Assert.IsFalse(string.IsNullOrWhiteSpace(QasAddressValidatorConfig.Current.Password), "QAS Configuration Error: Section or Password property not defined");
		}
	}
}