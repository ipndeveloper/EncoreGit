using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Foundation.Common;
using System.Data.Metadata.Edm;
using System.Reflection;

namespace NetSteps.Foundation.Common.Tests
{
    [TestClass]
    public class DbExtensionsTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void NewOrSharedConnection_SameContainer_ReturnsSameConnection()
        {
            var test = new
            {
                Container = Create.SharedOrNewContainer(),
                ConnectionStringName = ConnectionStringNames.Core
            };

            var connection1 = DbExtensions.NewOrSharedConnection(test.Container, test.ConnectionStringName);
            var connection2 = DbExtensions.NewOrSharedConnection(test.Container, test.ConnectionStringName);
            
            Assert.AreEqual(connection1, connection2);
        }

        [TestMethod]
        public void NewOrSharedConnection_DifferentContainers_ReturnsDifferentConnections()
        {
            var test = new
            {
                ConnectionStringName = ConnectionStringNames.Core
            };

            var connection1 = DbExtensions.NewOrSharedConnection(Create.NewContainer(), test.ConnectionStringName);
            var connection2 = DbExtensions.NewOrSharedConnection(Create.NewContainer(), test.ConnectionStringName);

            Assert.AreNotEqual(connection1, connection2);
        }

        [TestMethod]
        public void CreateConnection_EntityConnectionString_ReturnsSqlStoreConnection()
        {
            var test = new
            {
                ConnectionStringName = "NetStepsEntities"
            };

            var connection = DbExtensions.CreateConnection(test.ConnectionStringName);

            Assert.IsInstanceOfType(connection, typeof(SqlConnection));
        }

        [TestMethod]
        public void CreateEntityConnection_SqlConnectionString_Succeeds()
        {
            var test = new
            {
                MetadataWorkspace = GetMetadataWorkspace(),
                ConnectionStringName = ConnectionStringNames.Core
            };

            var entityConnection = DbExtensions.CreateEntityConnection(test.MetadataWorkspace, test.ConnectionStringName);

            Assert.IsNotNull(entityConnection);
        }

        [TestMethod]
        public void CreateEntityConnection_EntityConnectionString_Succeeds()
        {
            var test = new
            {
                MetadataWorkspace = GetMetadataWorkspace(),
                ConnectionStringName = "NetStepsEntities"
            };

            var entityConnection = DbExtensions.CreateEntityConnection(test.MetadataWorkspace, test.ConnectionStringName);

            Assert.IsNotNull(entityConnection);
        }

        private MetadataWorkspace GetMetadataWorkspace()
        {
            return new MetadataWorkspace(
                new[]
                {
                    "res://*/DbExtensions.TestEntities.csdl",
                    "res://*/DbExtensions.TestEntities.ssdl",
                    "res://*/DbExtensions.TestEntities.msl"
                },
                new[] { Assembly.GetExecutingAssembly() }
            );
        }
    }
}
