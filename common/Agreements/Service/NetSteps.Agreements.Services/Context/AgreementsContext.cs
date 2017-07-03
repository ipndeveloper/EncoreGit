namespace NetSteps.Agreements.Services
{
	using System.Data.Entity;
	using System.Data.Entity.Infrastructure;

	using NetSteps.Encore.Core.IoC;
	using NetSteps.Foundation.Common;
	using NetSteps.Foundation.Entity;

	/// <summary>
	/// The agreements context.
	/// </summary>
	[ContainerRegister(typeof(IAgreementsContext), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AgreementsContext : DbContext, IAgreementsContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AgreementsContext"/> class.
		/// </summary>
		public AgreementsContext()
			: base("name=" + ConnectionStringNames.Core)
		{
		}

		/// <summary>
		/// Gets or sets the agreements.
		/// </summary>
		public virtual IDbSet<Entities.Agreement> Agreements { get; set; }

		/// <summary>
		/// Gets or sets the agreement versions.
		/// </summary>
		public virtual IDbSet<Entities.AgreementVersion> AgreementVersions { get; set; }

		/// <summary>
		/// Gets or sets the agreement acceptance logs.
		/// </summary>
		public virtual IDbSet<Entities.AgreementAcceptanceLog> AgreementAcceptanceLogs { get; set; }

		/// <summary>
		/// Gets or sets the agreement kinds.
		/// </summary>
		public virtual IDbSet<Entities.AgreementKind> AgreementKinds { get; set; }

		/// <summary>
		/// Gets or sets the agreements to account kinds.
		/// </summary>
		public virtual IDbSet<Entities.AgreementToAccountKind> AgreementsToAccountKinds { get; set; }

		/// <summary>
		/// The set of entities.
		/// </summary>
		/// <typeparam name="TEntity">
		/// The entity.
		/// </typeparam>
		/// <returns>
		/// The database set.
		/// </returns>
		IDbSet<TEntity> IDbContext.Set<TEntity>()
		{
			return this.Set<TEntity>();
		}

		/// <summary>
		/// The on model creating.
		/// </summary>
		/// <param name="modelBuilder">
		/// The model builder.
		/// </param>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Configurations.Add(Create.New<Entities.AgreementConfiguation>());
		}
	}
}
