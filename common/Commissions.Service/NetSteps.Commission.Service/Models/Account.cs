using NetSteps.Commissions.Common.Models;
using System;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IAccount), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
    public class Account : IAccount
    {
        public int AccountId { get; set; }

        public int AccountKindId { get; set; }

        public string AccountNumber { get; set; }

        public int? AccountStatusChangeReasonId { get; set; }

        public DateTime? ActivationDateUtc { get; set; }

        public DateTime? BirthdayUtc { get; set; }

        public int CountryId { get; set; }

        public string EmailAddress { get; set; }

        public int? EnrollerId { get; set; }

        public DateTime? EnrollmentDateUtc { get; set; }

        public string EntityName { get; set; }

        public string FirstName { get; set; }

        public bool IsEntity { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public int? SponsorId { get; set; }

        public DateTime? TerminatedDateUtc { get; set; }

        public int AccountStatusId { get; set; }
    }
}
