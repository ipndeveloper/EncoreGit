using System.Data.Entity;
using NetSteps.Foundation.Entity;
using E = NetSteps.Agreements.Services.Entities;

namespace NetSteps.Agreements.Services
{
    public interface IAgreementsContext : IDbContext
    {
        IDbSet<E.Agreement> Agreements { get; }
        IDbSet<E.AgreementVersion> AgreementVersions { get; }
        IDbSet<E.AgreementAcceptanceLog> AgreementAcceptanceLogs { get; }
        IDbSet<E.AgreementKind> AgreementKinds { get; set; }
        IDbSet<E.AgreementToAccountKind> AgreementsToAccountKinds { get; set; }
    }
}
