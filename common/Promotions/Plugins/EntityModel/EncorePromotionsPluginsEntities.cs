using System;
using System.Data.Metadata.Edm;
using System.Reflection;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using NetSteps.Promotions.Plugins.Common;

namespace NetSteps.Promotions.Plugins.EntityModel
{
    [ContainerRegister(typeof(IEncorePromotionsPluginsUnitOfWork), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public partial class EncorePromotionsPluginsEntities : IEncorePromotionsPluginsUnitOfWork
    {
        /// <summary>
        /// The MetadataWorkspace for this context, used to initialize the context using a SQL connection string.
        /// Static-Lazy because it is costly to instantiate one of these.
        /// </summary>
        private static readonly Lazy<MetadataWorkspace> _metadataWorkspace = new Lazy<MetadataWorkspace>(() => 
            new MetadataWorkspace(
                new[]
                {
                    "res://*/EntityModel.NetStepsPromotionsPlugins.csdl",
                    "res://*/EntityModel.NetStepsPromotionsPlugins.ssdl",
                    "res://*/EntityModel.NetStepsPromotionsPlugins.msl"
                },
                new[] { Assembly.GetExecutingAssembly() }
            )
        );

        /// <summary>
        /// Initializes a new <see cref="EncorePromotionsPluginsEntities"/> object using the "Core" connection string.
        /// </summary>
        public EncorePromotionsPluginsEntities() : this(_metadataWorkspace.Value.CreateEntityConnection(ConnectionStringNames.Core)) { }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public new System.Data.Objects.IObjectSet<T> CreateObjectSet<T>() where T : class
        {
            return base.CreateObjectSet<T>();
        }
    }
}
