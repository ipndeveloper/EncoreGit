using System;
using System.Data.Metadata.Edm;
using System.Reflection;
using NetSteps.Foundation.Common;

namespace NetSteps.Data.Entities.Mail
{
    public partial class MailEntities
    {
        /// <summary>
        /// The MetadataWorkspace for this context, used to initialize the context using a SQL connection string.
        /// Static-Lazy because it is costly to instantiate one of these.
        /// </summary>
        private static readonly Lazy<MetadataWorkspace> _metadataWorkspace = new Lazy<MetadataWorkspace>(() => 
            new MetadataWorkspace(
                new[]
                {
                    "res://*/EntityModels.Mail.MailDB.csdl",
                    "res://*/EntityModels.Mail.MailDB.ssdl",
                    "res://*/EntityModels.Mail.MailDB.msl"
                },
                new[] { Assembly.GetExecutingAssembly() }
            )
        );

        /// <summary>
        /// Initializes a new <see cref="MailEntities"/> object using the "Mail" connection string.
        /// </summary>
        public MailEntities() : this(_metadataWorkspace.Value.CreateEntityConnection(ConnectionStringNames.Mail)) { }
    }
}
