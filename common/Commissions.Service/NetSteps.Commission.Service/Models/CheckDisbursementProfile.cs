using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICheckDisbursementProfile), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
    public class CheckDisbursementProfile : ICheckDisbursementProfile
    {
        public int AddressId { get; set; }
        public string NameOnCheck { get; set; }
        public int AccountId { get; set; }
        public DisbursementMethodKind DisbursementMethod { get { return DisbursementMethodKind.Check; } }
        public int DisbursementProfileId { get; set; }
        public bool EnrollmentFormReceived { get; set; }
        public bool IsEnabled { get; set; }
        public string NameOnAccount { get; set; }
        public decimal Percentage { get; set; }
        public int UserId { get; set; }
        public int CurrencyId { get; set; }
    }
}
