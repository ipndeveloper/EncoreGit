using System;

namespace NetSteps.Commissions.Common.Models
{
    /// <summary>
    /// model for the commissions account
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// the account identifier
        /// </summary>
        int AccountId { get; set; }

        /// <summary>
        /// the account number
        /// </summary>
        string AccountNumber { get; set; }

        /// <summary>
        /// the account kind identifier
        /// </summary>
        int AccountKindId { get; set; }

        /// <summary>
        /// gets or sets the first name
        /// </summary>
        string FirstName { get; set; }

        /// <summary>
        /// gets or sets the middle name
        /// </summary>
        string MiddleName { get; set; }

        /// <summary>
        /// gets or sets the last name
        /// </summary>
        string LastName { get; set; }

        /// <summary>
        /// gets or sets the email address
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// gets or sets the sponsor identifier
        /// </summary>
        int? SponsorId { get; set; }

        /// <summary>
        /// gets or sets the enroller identifier
        /// </summary>
        int? EnrollerId { get; set; }

        /// <summary>
        /// gets or sets teh enrollment date
        /// </summary>
        DateTime? EnrollmentDateUtc { get; set; }

        /// <summary>
        /// gets or sets whether the account is an entity
        /// </summary>
        bool IsEntity { get; set; }

        /// <summary>
        /// gets of sets the account status change reason identifier
        /// </summary>
        int? AccountStatusChangeReasonId { get; set; }

        /// <summary>
        /// gets or sets the account status
        /// </summary>
        int AccountStatusId { get; set; }

        /// <summary>
        /// gets or sets the entity name
        /// </summary>
        string EntityName { get; set; }

        /// <summary>
        /// gets or sets the country identifier
        /// </summary>
        int CountryId { get; set; }

        /// <summary>
        /// gets or sets the birthday
        /// </summary>
        DateTime? BirthdayUtc { get; set; }

        /// <summary>
        /// gets or sets the account termination date
        /// </summary>
        DateTime? TerminatedDateUtc { get; set; }

        /// <summary>
        /// gets or sets the account activation date
        /// </summary>
        DateTime? ActivationDateUtc { get; set; }
    }
}
