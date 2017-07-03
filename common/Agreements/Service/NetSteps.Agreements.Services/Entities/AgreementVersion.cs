using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using NetSteps.Encore.Core.IoC;
using System.Collections.Generic;

namespace NetSteps.Agreements.Services.Entities
{
    [ContainerRegister(typeof(AgreementVersion), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    [Table("AgreementVersions", Schema = "Agreements")]
    public class AgreementVersion
    {
        [Key]
        public int AgreementVersionId { get; set; }
        [Required, ForeignKey("Agreement")]
        public int AgreementId { get; set; }
        [Required]
        public string VersionNumber { get; set; }
        [Required]
        public DateTime DateReleasedUtc { get; set; }
        [Required]
        public string FilePath { get; set; }
        [Required]
        public string AgreementText { get; set; }

        public virtual Agreement Agreement { get; set; }
        public virtual ICollection<AgreementAcceptanceLog> AgreementAcceptanceLogs { get; set; }
    }
}
