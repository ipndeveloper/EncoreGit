﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderRules.Core.Model
{
     
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data;
    using System.Data.EntityClient;
    using System.Data.Metadata.Edm;
    using System.Data.Objects;
    using System.Reflection;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Foundation.Common;
    using System.Data.SqlClient;
    
    
    public partial class CoreEntities : DbContext
    {
    	private static Lazy<MetadataWorkspace> _metadataWorkspace = new Lazy<MetadataWorkspace>(() => 
                new MetadataWorkspace(
                    new[]
                    {
                        "res://*/OrderRules.csdl",
                        "res://*/OrderRules.ssdl",
                        "res://*/OrderRules.msl"
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
                    entityBuilder.Metadata = @"res://*/OrderRules.csdl|res://*/OrderRules.ssdl|res://*/OrderRules.msl";
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
    
        public DbSet<Rules> Rules { get; set; }
        public DbSet<RuleStatuses> RuleStatuses { get; set; }
        public DbSet<RuleValidationAccountListItems> RuleValidationAccountListItems { get; set; }
        public DbSet<RuleValidationAccountLists> RuleValidationAccountLists { get; set; }
        public DbSet<RuleValidationAccountTypeListItems> RuleValidationAccountTypeListItems { get; set; }
        public DbSet<RuleValidationAccountTypeLists> RuleValidationAccountTypeLists { get; set; }
        public DbSet<RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts> RuleValidationCustomerPriceTypeTotalRangeCurrencyAmounts { get; set; }
        public DbSet<RuleValidationCustomerPriceTypeTotalRanges> RuleValidationCustomerPriceTypeTotalRanges { get; set; }
        public DbSet<RuleValidationCustomerPriceTypeTotalRangesKeys> RuleValidationCustomerPriceTypeTotalRangesKeys { get; set; }
        public DbSet<RuleValidationCustomerSubtotalRangeCurrencyAmounts> RuleValidationCustomerSubtotalRangeCurrencyAmounts { get; set; }
        public DbSet<RuleValidationCustomerSubtotalRanges> RuleValidationCustomerSubtotalRanges { get; set; }
        public DbSet<RuleValidationOrderTypeListItems> RuleValidationOrderTypeListItems { get; set; }
        public DbSet<RuleValidationOrderTypeLists> RuleValidationOrderTypeLists { get; set; }
        public DbSet<RuleValidationProductListItems> RuleValidationProductListItems { get; set; }
        public DbSet<RuleValidationProductLists> RuleValidationProductLists { get; set; }
        public DbSet<RuleValidationProductTypeListItems> RuleValidationProductTypeListItems { get; set; }
        public DbSet<RuleValidationProductTypeLists> RuleValidationProductTypeLists { get; set; }
        public DbSet<RuleValidations> RuleValidations { get; set; }
        public DbSet<RuleValidationStoreFrontListItems> RuleValidationStoreFrontListItems { get; set; }
        public DbSet<RuleValidationStoreFrontLists> RuleValidationStoreFrontLists { get; set; }
    }
}