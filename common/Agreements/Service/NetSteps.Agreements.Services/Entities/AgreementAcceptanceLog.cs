using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Agreements.Services.Entities
{
    [ContainerRegister(typeof(AgreementAcceptanceLog), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    [Table("AgreementAcceptanceLog", Schema = "Agreements")]
    public class AgreementAcceptanceLog
    {
        [Key, Column(Order = 0)]
        public int AccountId { get; set; }
        [Key, Column(Order = 1), ForeignKey("AgreementVersion")]
        public int AgreementVersionId { get; set; }
        [Required]
        public DateTime DateAcceptedUtc { get; set; }

        public virtual AgreementVersion AgreementVersion { get; set; }
    }
}
