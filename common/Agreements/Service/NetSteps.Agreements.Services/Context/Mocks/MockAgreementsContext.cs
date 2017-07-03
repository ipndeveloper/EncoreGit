using System.Data.Entity;
using NetSteps.Foundation.Entity;
using NetSteps.Foundation.Entity.Mocks;
using E = NetSteps.Agreements.Services.Entities;

namespace NetSteps.Agreements.Services.Context.Mocks
{
    public class MockAgreementsContext : MockDbContext, IAgreementsContext
    {
        public MockAgreementsContext() : this(new MockAgreementsDatabase()) { }

        public MockAgreementsContext(MockAgreementsDatabase database)
        {
            Agreements = new MockDbSet<E.Agreement>(database.Agreements, x => x.AgreementId);
            AgreementVersions = new MockDbSet<E.AgreementVersion>(database.AgreementVersions, x => x.AgreementVersionId);
            AgreementAcceptanceLogs = new MockDbSet<E.AgreementAcceptanceLog>(database.AgreementAcceptanceLogs, x => x.AccountId, x => x.AgreementVersionId);
            AgreementKinds = new MockDbSet<E.AgreementKind>(database.AgreementKinds, x => x.AgreementKindId);
            AgreementsToAccountKinds = new MockDbSet<E.AgreementToAccountKind>(database.AgreementsToAccountKinds, x => x.AgreementId, x => x.AccountKindId);
        }

        public IDbSet<E.Agreement> Agreements { get; set; }
        public IDbSet<E.AgreementVersion> AgreementVersions { get; set; }
        public IDbSet<E.AgreementAcceptanceLog> AgreementAcceptanceLogs { get; set; }
        public IDbSet<E.AgreementKind> AgreementKinds { get; set; }
        public IDbSet<E.AgreementToAccountKind> AgreementsToAccountKinds { get; set; }
    }
}
