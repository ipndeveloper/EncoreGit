using System;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business.Logic
{
    [ContainerRegister(typeof(IEnrollmentBasicInfoBusinessLogic), RegistrationBehaviors.Default)]
    public class EnrollmentBasicInfoBusinessLogic : IEnrollmentBasicInfoBusinessLogic
    {
        public void BasicInfo_ValidateTaxNumber(Entities.Interfaces.IEnrollmentBasicInfo basicInfo, Account account)
        {
            throw new NotImplementedException();
        }

        public void BasicInfo_ValidateEmailAvailability(Entities.Interfaces.IEnrollmentBasicInfo basicInfo, Account account)
        {
            throw new NotImplementedException();
        }

        public void BasicInfo_GetExistingProspect(Entities.Interfaces.IEnrollmentBasicInfo basicInfo, Account account)
        {
            throw new NotImplementedException();
        }

        public void BasicInfo_UpdateAccount(Entities.Interfaces.IEnrollmentBasicInfo model, Account account)
        {
            throw new NotImplementedException();
        }

        public void BasicInfo_UpdateUser(Entities.Interfaces.IEnrollmentBasicInfo model, Account account)
        {
            throw new NotImplementedException();
        }

        public void BasicInfo_UpdateAddress(Entities.Interfaces.IEnrollmentBasicInfo model, Account account)
        {
            throw new NotImplementedException();
        }

        public void ValidateTaxNumber(Entities.Interfaces.IEnrollmentBasicInfo basicInfo, Account account)
        {
            throw new NotImplementedException();
        }

        public void ValidateEmailAvailability(Entities.Interfaces.IEnrollmentBasicInfo basicInfo, Account account)
        {
            throw new NotImplementedException();
        }

        public void GetExistingProspect(Entities.Interfaces.IEnrollmentBasicInfo basicInfo, Account account)
        {
            throw new NotImplementedException();
        }

        public void UpdateAccount(Entities.Interfaces.IEnrollmentBasicInfo model, Account account)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(Entities.Interfaces.IEnrollmentBasicInfo model, Account account)
        {
            throw new NotImplementedException();
        }

        public void UpdateAddress(Entities.Interfaces.IEnrollmentBasicInfo model, Account account)
        {
            throw new NotImplementedException();
        }
    }
}
