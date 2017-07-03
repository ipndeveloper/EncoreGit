using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Reflection;
using System.Data.Entity.Design.PluralizationServices;
using System.Linq.Expressions;

namespace NetSteps.Integrations.Service.Entities
{
	public class BelcorpIntegrationsDbContext : DbContext
	{
		public DbSet<KitItemValuation> KitItemValuations { get; set; }

		public BelcorpIntegrationsDbContext()
			: base(NetSteps.Data.Entities.NetStepsEntities.CoreConnectionString)
		{ }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Type tDbModelBuilder = typeof(DbModelBuilder);
			Type tEntTypeConfig = typeof(EntityTypeConfiguration<>);
			MethodInfo entity = tDbModelBuilder.GetMethod("Entity");
			foreach (Type ctype in GetContainerTypes())
			{
				MethodInfo genericModelBuilderEntityMethod = entity.MakeGenericMethod(ctype);
				object entityTypeConfiguration = genericModelBuilderEntityMethod.Invoke(modelBuilder, null);

				string schemaName = "BelcorpUSA";

				PluralizationService plural = PluralizationService.CreateService(System.Globalization.CultureInfo.CurrentCulture);
				string tableName = plural.Pluralize(ctype.Name);

				Type tEntTypeConfig_Ctype = tEntTypeConfig.MakeGenericType(ctype);

				tEntTypeConfig_Ctype.GetMethod("ToTable", new Type[] { typeof(string), typeof(string) })
					.Invoke(entityTypeConfiguration, new object[] { tableName, schemaName });

				if (ctype.BaseType == typeof(Object))
				{
					ParameterExpression param = Expression.Parameter(ctype, "ctype");
					tEntTypeConfig_Ctype.GetMethod("HasKey").MakeGenericMethod(typeof(Int32))
						.Invoke(entityTypeConfiguration, new object[] { Expression.Lambda(typeof(Func<,>).MakeGenericType(ctype, typeof(Int32)), Expression.Property(param, String.Concat(ctype.Name, "Id")), param) });
				}
			}
		}

		private IEnumerable<Type> GetContainerTypes()
		{
			return new Type[] {
				typeof(KitItemValuation)
			};
		}
	}
}
