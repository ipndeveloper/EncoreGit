﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Belcorp.Policies.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.EntityClient;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Reflection;
    using NetSteps.Foundation.Common;
    using System.Data.SqlClient;
    
    public partial class CoreEntities : DbContext, ICoreEntities
    {
        private static Lazy<MetadataWorkspace> _metadataWorkspace = new Lazy<MetadataWorkspace>(() => 
                new MetadataWorkspace(
                    new[]
                    {
                        "res://*/PoliciesEntities.csdl",
                        "res://*/PoliciesEntities.ssdl",
                        "res://*/PoliciesEntities.msl"
                    },
                    new[] { Assembly.GetExecutingAssembly() }
                )
            );
    
    		public static string conn() {
    			EntityConnection conEntity = _metadataWorkspace.Value.CreateEntityConnection(ConnectionStringNames.Core);
                    SqlConnection conSQL = (SqlConnection)conEntity.StoreConnection;                
                    string providerString = conSQL.ConnectionString;
                    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                    entityBuilder.Provider = "System.Data.SqlClient";
                    entityBuilder.ProviderConnectionString = providerString;
                    entityBuilder.Metadata = @"res://*/PoliciesEntities.csdl|res://*/PoliciesEntities.ssdl|res://*/PoliciesEntities.msl";
                    return entityBuilder.ToString();
    		}
        public CoreEntities()
            : base(conn())
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<AccountPolicies> AccountPolicies { get; set; }
        public DbSet<AccountPolicyDetails> AccountPolicyDetails { get; set; }
        public DbSet<Policies> Policies { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
    }
}
