using System;

namespace NetSteps.Data.Entities.Interfaces
{
    public interface IEnrollmentBasicInfo
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool IsEntity { get; set; }
        string personalIDNumber { get; set; }
        string EntityName { get; set; }
        Constants.Gender Gender { get; set; }
        DateTime Birthday { get; set; }
        string MainPhone { get; set; }
        bool ShowTaxNumber { get; set; }
        int CountryID { get; set; }
    }
}
