using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using NetSteps.Encore.Core.IoC;
using NetSteps.Agreements.Common;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace NetSteps.Agreements.Services.Entities
{
    [ContainerRegister(typeof(Agreement), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    [Table("Agreements", Schema = "Agreements")]
    public class Agreement
    {
        [Key]
        public int AgreementId { get; set; }
        [Required]
        public string TermName { get; set; }

        public virtual ICollection<AgreementVersion> AgreementVersions { get; set; }
        public virtual ICollection<AgreementKind> AgreementKinds { get; set; }
        public virtual ICollection<AgreementToAccountKind> AgreementToAccountKinds { get; set; }
    }
}
