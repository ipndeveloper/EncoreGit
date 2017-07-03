using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Encore.Core.IoC;
using System.Data.Entity;

namespace NetSteps.Agreements.Services.Entities
{
    [ContainerRegister(typeof(AgreementKind), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    [Table("AgreementKinds", Schema = "Agreements")]
    public class AgreementKind
    {
        [Key]
        public int AgreementKindId { get; set; }
        [Required]
        public string TermName { get; set; }

        public virtual ICollection<Agreement> Agreements { get; set; }
    }
}
