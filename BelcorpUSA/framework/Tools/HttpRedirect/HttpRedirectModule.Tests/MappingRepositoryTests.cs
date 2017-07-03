// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MappingRepositoryTests.cs" company="NetSteps">
//   Copyright 2012, NetSteps, LLC
// </copyright>
// <summary>
//   Defines the MappingRepositoryTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Wireup.Meta;

// Indicates this assembly is dependent upon the common's wireup.
[module: WireupDependency(typeof(HttpRedirectModule.Wireup))]

namespace HttpRedirectModule.Tests
{

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// Mapping Repository Tests
    /// </summary>
    [TestClass]
    public class MappingRepositoryTests
    {
        /// <summary>
        /// Initializes the Tests
        /// </summary>
        [TestInitialize]
        public void init()
        {
            // Force wireup.
            WireupCoordinator.SelfConfigure();
        }

        /// <summary>
        /// Test to determine mapped result
        /// </summary>
        [TestMethod]
        public void Repository_should_return_mapping_result()
        {
            // Constructor inject our mock repository...
            var businessLogic = Create.NewWithParams<IMappingBusinessLogic>(LifespanTracking.Automatic, Param.Value<IMappingRepository>(new MockMappingRepository()));
            Assert.IsNotNull(businessLogic.GetNewSiteUrl("PeterParker"));
        }

        /// <summary>
        /// Test to determine unmatched result
        /// </summary>
        [TestMethod]
        public void Repository_should_return_null()
        {
            // Constructor inject our mock repository...
            var businessLogic = Create.NewWithParams<IMappingBusinessLogic>(LifespanTracking.Automatic, Param.Value<IMappingRepository>(new MockMappingRepository()));
            Assert.IsNull(businessLogic.GetNewSiteUrl("NoMatchingValue"));
        }
    }
}
