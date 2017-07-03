using System.Collections.Generic;
using E = NetSteps.Agreements.Services.Entities;

namespace NetSteps.Agreements.Services.Context.Mocks
{
    public class MockAgreementsDatabase
    {
        public HashSet<E.Agreement> Agreements = new HashSet<E.Agreement>();
        public HashSet<E.AgreementVersion> AgreementVersions = new HashSet<E.AgreementVersion>();
        public HashSet<E.AgreementAcceptanceLog> AgreementAcceptanceLogs = new HashSet<E.AgreementAcceptanceLog>();
        public HashSet<E.AgreementKind> AgreementKinds = new HashSet<E.AgreementKind>();
        public HashSet<E.AgreementToAccountKind> AgreementsToAccountKinds = new HashSet<E.AgreementToAccountKind>();
    }
}
