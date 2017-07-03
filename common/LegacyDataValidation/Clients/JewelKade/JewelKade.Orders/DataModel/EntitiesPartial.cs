using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Foundation.Common;

namespace JewelKade.Orders.DataModel
{
    public partial class Entities
    {
        /// <summary>
        /// The MetadataWorkspace for this context, used to initialize the context using a SQL connection string.
        /// Static-Lazy because it is costly to instantiate one of these.
        /// </summary>
        private static readonly Lazy<MetadataWorkspace> _metadataWorkspace = new Lazy<MetadataWorkspace>(() => 
            new MetadataWorkspace(
                new[]
                {
                    "res://*/DataModel.Entities.csdl",
                    "res://*/DataModel.Entities.ssdl",
                    "res://*/DataModel.Entities.msl"
                },
                new[] { Assembly.GetExecutingAssembly() }
            )
        );

        /// <summary>
        /// Initializes a new <see cref="Entities"/> object using the "Core" connection string.
        /// </summary>
        public Entities() : base(_metadataWorkspace.Value.CreateEntityConnection(ConnectionStringNames.Core), true)
        {
            Configuration.ProxyCreationEnabled = false;
        }
    }
}
