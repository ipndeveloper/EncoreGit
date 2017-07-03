using System;
using System.Data;
using System.Data.EntityClient;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Reflection;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common;
using NetSteps.Foundation.Common;

namespace NetSteps.Promotions.Service.EntityModels
{
    [ContainerRegister(typeof(IPromotionUnitOfWork), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public partial class NetStepsPromotionsEntities : IPromotionUnitOfWork
    {
        /// <summary>
        /// The MetadataWorkspace for this context, used to initialize the context using a SQL connection string.
        /// Static-Lazy because it is costly to instantiate one of these.
        /// </summary>
        private static Lazy<MetadataWorkspace> _metadataWorkspace = new Lazy<MetadataWorkspace>(() => 
            new MetadataWorkspace(
                new[]
                {
                    "res://*/EntityModels.PromotionEntities.csdl",
                    "res://*/EntityModels.PromotionEntities.ssdl",
                    "res://*/EntityModels.PromotionEntities.msl"
                },
                new[] { Assembly.GetExecutingAssembly() }
            )
        );

        /// <summary>
        /// Initializes a new <see cref="NetStepsPromotionsEntities"/> object using the "Core" connection string.
        /// </summary>
        public NetStepsPromotionsEntities() : this(_metadataWorkspace.Value.CreateEntityConnection(ConnectionStringNames.Core)) { }

        public new IObjectSet<T> CreateObjectSet<T>() where T : class
        {
            return base.CreateObjectSet<T>();
        }

        public new void SaveChanges()
        {
            SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
        }
    }
}
