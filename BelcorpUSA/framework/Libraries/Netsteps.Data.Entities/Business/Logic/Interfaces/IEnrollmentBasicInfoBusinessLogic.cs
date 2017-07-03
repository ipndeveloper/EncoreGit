using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{

    public interface IEnrollmentBasicInfoBusinessLogic
    {
        void ValidateTaxNumber(IEnrollmentBasicInfo basicInfo, Account account);
        void ValidateEmailAvailability(IEnrollmentBasicInfo basicInfo, Account account);
        void GetExistingProspect(IEnrollmentBasicInfo basicInfo, Account account);
        void UpdateAccount(IEnrollmentBasicInfo model, Account account);
        void UpdateUser(IEnrollmentBasicInfo model, Account account);
        void UpdateAddress(IEnrollmentBasicInfo model, Account account);
    }
}
