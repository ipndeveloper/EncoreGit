using System;
using NetSteps.Encore.Core.IoC;
using System.Data.Entity.ModelConfiguration;
using System.Collections.Generic;

namespace NetSteps.Agreements.Services.Entities
{
    [ContainerRegister(typeof(AgreementConfiguation), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class AgreementConfiguation : EntityTypeConfiguration<Agreement>
    {
        public AgreementConfiguation()
        {
            this.HasMany<AgreementKind>(u => (ICollection<AgreementKind>)u.AgreementKinds).WithMany(l => (ICollection<Agreement>)l.Agreements).Map(ul =>
            {
                ul.ToTable("AgreementsToAgreementKinds", "Agreements");
                ul.MapLeftKey("AgreementID");
                ul.MapRightKey("AgreementKindID");
            });
        }
    }
}
