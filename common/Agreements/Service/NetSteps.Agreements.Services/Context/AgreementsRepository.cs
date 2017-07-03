namespace NetSteps.Agreements.Services
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Linq;
	using System.Linq.Expressions;

	using NetSteps.Agreements.Common;
	using NetSteps.Agreements.Services.Entities;
	using NetSteps.Encore.Core.IoC;
	using NetSteps.Foundation.Entity;

	/// <summary>
	/// The AgreementsRepository interface.
	/// </summary>
	public interface IAgreementsRepository : IEntityModelRepository<AgreementVersion, IAgreementVersion, IAgreementsContext>
	{
	}

	/// <summary>
	/// The agreements repository.
	/// </summary>
	[ContainerRegister(typeof(IAgreementsRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AgreementsRepository : EntityModelRepository<AgreementVersion, IAgreementVersion, IAgreementsContext>, IAgreementsRepository
	{
		/// <summary>
		/// The get predicate for model.
		/// </summary>
		/// <param name="m">
		/// The m.
		/// </param>
		/// <returns>
		/// The <see cref="Expression"/>.
		/// </returns>
		public override Expression<Func<AgreementVersion, bool>> GetPredicateForModel(IAgreementVersion m)
		{
			return e => e.AgreementVersionId == m.AgreementVersionId;
		}

		/// <summary>
		/// The update entity.
		/// </summary>
		/// <param name="e">
		/// The e.
		/// </param>
		/// <param name="m">
		/// The m.
		/// </param>
		public override void UpdateEntity(AgreementVersion e, IAgreementVersion m)
		{
			Contract.Requires<ArgumentNullException>(m != null && e != null);

			e.AgreementText = m.AgreementText;
			e.FilePath = m.AgreementFile;
			e.VersionNumber = m.VersionNumber;
			e.AgreementId = m.AgreementId;
			e.AgreementVersionId = m.AgreementVersionId;
			if (e.DateReleasedUtc == default(DateTime))
			{
				e.DateReleasedUtc = DateTime.UtcNow;
			}

			if (e.Agreement == null)
			{
				e.Agreement = Create.New<Agreement>();
			}

			e.Agreement.TermName = m.TermName;
		}

		/// <summary>
		/// The update model.
		/// </summary>
		/// <param name="m">
		/// The m.
		/// </param>
		/// <param name="e">
		/// The e.
		/// </param>
		public override void UpdateModel(IAgreementVersion m, AgreementVersion e)
		{
			Contract.Requires<ArgumentNullException>(m != null && e != null);

			m.AgreementId = e.AgreementId;
			m.TermName = e.Agreement.TermName;
			m.VersionNumber = e.VersionNumber;
			m.AgreementFile = e.FilePath;
			m.AgreementText = e.AgreementText;
			m.DateReleasedUtc = e.DateReleasedUtc;
			m.AgreementVersionId = e.AgreementVersionId;
			m.AgreementKinds = e.Agreement.AgreementKinds.ToList().Select(x =>
				 {
					 var y = Create.New<IAgreementKind>();
					 y.AgreementKindId = x.AgreementKindId;
					 y.TermName = x.TermName;
					 return y;
				 }).ToList();
			m.AccountKinds = e.Agreement.AgreementToAccountKinds.Select(x => x.AccountKindId).ToList();
		}
	}
}
