using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.IoC;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace NetSteps.Agreements.Services.Entities
{
    [ContainerRegister(typeof(AgreementToAccountKind), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    [Table("AgreementsToAccountKinds", Schema = "Agreements")]
    public class AgreementToAccountKind
    {
        [Key, Column(Order = 0), ForeignKey("Agreement")]
        public int AgreementId { get; set; }
        [Key, Column(Order = 1)]
        public int AccountKindId { get; set; }

        public virtual Agreement Agreement { get; set; }
    }
}
