namespace NetSteps.Agreements.Services.Agreements
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    using NetSteps.Agreements.Common;
    using NetSteps.Agreements.Common.Models;
    using NetSteps.Agreements.Services.Entities;
    using NetSteps.Agreements.Services.Models;
    using NetSteps.Encore.Core.IoC;

    /// <summary>
    /// Provides agreements and agreement versions, as well as their respective save functions
    /// </summary>
    public class AgreementsService : IAgreementsService
    {
        /// <summary>
        /// The _context factory.
        /// </summary>
        private readonly Func<IAgreementsContext> contextFactory;

        /// <summary>
        /// The _repository.
        /// </summary>
        private readonly IAgreementsRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AgreementsService"/> class. 
        /// </summary>
        /// <param name="contextFactory">
        /// The context Factory.
        /// </param>
        /// <param name="repository">
        /// The repository.
        /// </param>
        public AgreementsService(Func<IAgreementsContext> contextFactory, IAgreementsRepository repository)
        {
            Contract.Requires<ArgumentNullException>(contextFactory != null);
            Contract.Requires<ArgumentNullException>(repository != null);

            this.contextFactory = contextFactory;
            this.repository = repository;
        }

        /// <summary>
        /// Gets all agreements for the specified account, accountKind, and agreementKind
        /// </summary>
        /// <param name="accountId">
        /// The account Id.
        /// </param>
        /// <param name="accountKindId">
        /// The account Kind Id.
        /// </param>
        /// <param name="agreementKind">
        /// The agreement Kind.
        /// </param>
        /// <returns>
        /// The list of agreements.
        /// </returns>
        public IEnumerable<ISpecificAgreement> GetAgreements(int accountId, int accountKindId, IAgreementKind agreementKind = null)
        {
            using (var context = this.contextFactory())
            {
                // Trying to avoid the implicitly captured closure problem.
                var copiedAccountKindId = accountKindId;
                var copiedAccountId = accountId;

                var agreementsForAccount = context.Agreements.Where(a => a.AgreementToAccountKinds.Any(x => x.AccountKindId == copiedAccountKindId)).ToList();
                if (agreementKind != null)
                {
                    var agreementKindId = agreementKind.AgreementKindId;
                    agreementsForAccount = agreementsForAccount.Where(x => x.AgreementKinds.Any(k => k.AgreementKindId == agreementKindId)).ToList();
                }

                var agreementVersions = agreementsForAccount.Select(a => a.AgreementVersions.OrderByDescending(v => v.DateReleasedUtc).FirstOrDefault());
                var acceptedAgreements = context.AgreementAcceptanceLogs.Where(l => l.AccountId.Equals(copiedAccountId)).ToList();

                var agreements = agreementVersions.ToList().Select(e => new SpecificAgreement
                {
                    File = e.FilePath,
                    Text = e.AgreementText,
                    VersionNumber = e.VersionNumber,
                    VersionId = e.AgreementVersionId,
                    TermName = e.Agreement.TermName,
                    ReleaseDateUtc = e.DateReleasedUtc,
                    AccountId = copiedAccountId,
                    AcceptanceLog = new SpecificAcceptanceLog(acceptedAgreements.Where(l => l.AgreementVersionId.Equals(e.AgreementVersionId)).OrderByDescending(l => l.DateAcceptedUtc).FirstOrDefault())
                });

                return agreements;
            }
        }

        /// <summary>
        /// Saves acceptance log entries for agreements with HasAgreed set to true
        /// </summary>
        /// <param name="agreements">
        /// The agreements.
        /// </param>
        public virtual void SaveAcceptedAgreements(IEnumerable<ISpecificAgreement> agreements)
        {
            this.SaveAcceptedAgreementsAndReturn(agreements);
        }

        public virtual IEnumerable<ISpecificAgreement> SaveAcceptedAgreementsAndReturn(IEnumerable<ISpecificAgreement> agreements)
        {
            using (var context = this.contextFactory())
            {
                var agreementsList = agreements.ToList();
                foreach (var agreement in agreementsList)
                {
                    var e = context.AgreementAcceptanceLogs.FirstOrDefault(a => a.AccountId == agreement.AccountId && a.AgreementVersionId == agreement.VersionId);
                    if (e == default(AgreementAcceptanceLog))
                    {
                        e = Create.New<AgreementAcceptanceLog>();
                        e.AccountId = agreement.AccountId;
                        e.AgreementVersionId = agreement.VersionId;
                        e.DateAcceptedUtc = DateTime.UtcNow;
                        context.AgreementAcceptanceLogs.Add(e);
                    }

                    agreement.AcceptanceLog = new SpecificAcceptanceLog(e);
                }

                context.SaveChanges();

                return agreementsList.AsEnumerable();
            }
        }

        /// <summary>
        /// Gets the agreement version info for the specified agreementKind
        /// </summary>
        /// <param name="agreementKind">
        /// The agreement Kind.
        /// </param>
        /// <returns>
        /// The list of agreement versions.
        /// </returns>
        public virtual IEnumerable<IAgreementVersion> GetAgreementVersions(IAgreementKind agreementKind = null)
        {
            using (var context = this.contextFactory())
            {
                return context.AgreementVersions
                     .Where(x => (agreementKind == null || x.Agreement.AgreementKinds.Any(k => k.AgreementKindId == agreementKind.AgreementKindId)))
                     .ToList().Select(e =>
                     {
                         var m = Create.New<IAgreementVersion>();
                         this.repository.UpdateModel(m, e);
                         return m;
                     })
                     .ToList();
            }
        }

        /// <summary>
        /// Gets the agreement version info for the specified agreementKind
        /// </summary>
        /// <param name="accountKindId">
        /// The account Kind Id.
        /// </param>
        /// <param name="agreementKind">
        /// The agreement Kind.
        /// </param>
        /// <returns>
        /// The list of agreement versions.
        /// </returns>
        public virtual IEnumerable<IAgreementVersion> GetAgreementVersions(int accountKindId, IAgreementKind agreementKind = null)
        {
            int agreementKindId = agreementKind == null ? 0 : agreementKind.AgreementKindId;
            using (var context = this.contextFactory())
            {
                return context.AgreementVersions
                     .Where(x => x.Agreement.AgreementToAccountKinds.Any(k => k.AccountKindId == accountKindId))
                     .Where(x => (agreementKindId == 0 || x.Agreement.AgreementKinds.Any(k => k.AgreementKindId == agreementKindId)))
                     .ToList().Select(e =>
                     {
                         var m = Create.New<IAgreementVersion>();
                         this.repository.UpdateModel(m, e);
                         return m;
                     })
                     .ToList();
            }
        }

        /// <summary>
        /// Saves a new agreement version
        /// </summary>
        /// <param name="agreementVersion">
        /// The agreement Version.
        /// </param>
        public virtual void SaveNewAgreementVersion(IAgreementVersion agreementVersion)
        {
            using (var context = this.contextFactory())
            {
                var newVersion = Create.New<AgreementVersion>();
                newVersion.Agreement = context.Agreements.FirstOrDefault(x => x.AgreementId == agreementVersion.AgreementId);
                this.repository.UpdateEntity(newVersion, agreementVersion);
                context.SaveChanges();
                this.repository.UpdateModel(agreementVersion, newVersion);
            }
        }
    }
}
