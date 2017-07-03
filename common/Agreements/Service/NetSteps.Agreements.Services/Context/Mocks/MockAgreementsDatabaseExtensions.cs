using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Diagnostics.Contracts;

namespace NetSteps.Agreements.Services.Context.Mocks
{
    public static class MockAgreementsDatabaseExtensions
    {
        private static Random _rng = new Random();
        private static int randomInt()
        {
            return _rng.Next(1000000000, int.MaxValue);
        }
        private static readonly DateTime _startDate = new DateTime(2000, 1, 1);
        private static DateTime randomDate()
        {
            return _startDate.AddSeconds(_rng.NextDouble() * ((DateTime.Today - _startDate).TotalSeconds));
        }

        public static MockAgreementsDatabase InitializeData(this MockAgreementsDatabase database)
        {
            Contract.Requires<ArgumentNullException>(database != null);

            database.AgreementKinds.Add(new Entities.AgreementKind { AgreementKindId = 1, TermName = "kind1" });
            database.AgreementKinds.Add(new Entities.AgreementKind { AgreementKindId = 2, TermName = "kind2" });
            database.AgreementKinds.Add(new Entities.AgreementKind { AgreementKindId = 3, TermName = "kind3" });

            var a1 = new Entities.Agreement { AgreementId = 1, TermName = "Mock Agreement 1" };
            database.Agreements.Add(a1);

            var av1 = new Entities.AgreementVersion { AgreementId = 1, AgreementText = "asdf", FilePath = "/asdf", VersionNumber = "1.0", DateReleasedUtc = DateTime.UtcNow.AddDays(-5), AgreementVersionId = 1 };
            var av2 = new Entities.AgreementVersion { AgreementId = 1, AgreementText = "asdf2", FilePath = "/asdf2", VersionNumber = "1.1", DateReleasedUtc = DateTime.UtcNow, AgreementVersionId = 2 };
            database.AgreementVersions.Add(av1);
            database.AgreementVersions.Add(av2);

            var atak1 = new Entities.AgreementToAccountKind { AccountKindId = 1, AgreementId = 1 };
            database.AgreementsToAccountKinds.Add(atak1);

            av1.Agreement = av2.Agreement = a1;
            a1.AgreementVersions = new HashSet<Entities.AgreementVersion>(new Entities.AgreementVersion[] { av1, av2 });
            atak1.Agreement = a1;
            a1.AgreementToAccountKinds = new HashSet<Entities.AgreementToAccountKind>(new Entities.AgreementToAccountKind[] { atak1 });

            var a2 = new Entities.Agreement { AgreementId = 2, TermName = "Mock Agreement 2" };
            database.Agreements.Add(a2);
            var av3 = new Entities.AgreementVersion { AgreementId = 2, AgreementText = "fdsa", FilePath = "/fdsa", VersionNumber = "1", DateReleasedUtc = DateTime.UtcNow, AgreementVersionId = 3 };
            database.AgreementVersions.Add(av3);
            var atak2 = new Entities.AgreementToAccountKind { AccountKindId = 1, AgreementId = 2 };
            var atak3 = new Entities.AgreementToAccountKind { AccountKindId = 2, AgreementId = 2 };
            database.AgreementsToAccountKinds.Add(atak2);
            database.AgreementsToAccountKinds.Add(atak3);

            return database;
        }
    }
}
