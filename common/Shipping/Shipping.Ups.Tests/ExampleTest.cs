using NetSteps.Shipping.Ups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using NetSteps.Shipping.Ups.ShippingAPI;

namespace Shipping.Ups.Tests
{
    
    
    /// <summary>
    ///This is a test class for ExampleTest and is intended
    ///to contain all ExampleTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExampleTest
    {
        /// <summary>
        ///A test for TestThatReturnsResponse
        ///</summary>
        [TestMethod()]
        public void TestThatReturnsResponseTest()
        {
            // Arrange
            Example target = new Example();
            ShipmentResponse expected = new ShipmentResponse();
            expected.Response = new ResponseType();
            expected.Response.ResponseStatus = new CodeDescriptionType();
            expected.Response.ResponseStatus.Code = "1";

            // Act
            ShipmentResponse actual = target.TestThatReturnsResponse();
            
            // Assert
            Assert.AreEqual(expected.Response.ResponseStatus.Code, actual.Response.ResponseStatus.Code);
        }
    }
}
