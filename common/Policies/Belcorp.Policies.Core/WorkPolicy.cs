using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Belcorp.Policies.Data.UnitOfWork.Interface;
using Belcorp.Policies.Entities;
using NetSteps.Encore.Core.IoC;

namespace Belcorp.Policies.Core
{
    public class WorkPolicy : Belcorp.Policies.Core.IWorkPolicy 
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkPolicy(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Policies.Entities.Policies> GetAllPolicies()
        {
            var AllPolicies = _unitOfWork.GetRepository<Policies.Entities.Policies>().GetAll();
            return AllPolicies;
        }

        public IEnumerable<Policies.Entities.Policies> GetAllActivePolicies()
        {
            var AllPolicies = GetAllPolicies()
                                .Where(x=> x.Active);
            return AllPolicies;
        }

        public IEnumerable<Policies.Entities.Policies> GetAllActivePoliciesByLanguage(int pLanguageID)
        {
            var AllPolicies = GetAllPolicies()
                                .Where(x => x.Active && x.LanguageID == pLanguageID);

            return AllPolicies.Any() ? AllPolicies : GetAllActivePolicies().Where(x => x == GetAllActivePolicies().OrderBy(y => y.PolicyID).FirstOrDefault());
        }

        public IEnumerable<AccountPolicies> GetAccountPolicies(int pAccountId)
        {
            var AllPolicies = _unitOfWork.GetRepository<AccountPolicies>()
                                .GetAll()
                                .Where(x => x.AccountID == pAccountId);
            return AllPolicies;
        }

        public bool IsApplicableAccount(int pAccountId, int pTypeBA)
        {
            return _unitOfWork.GetRepository<Accounts>()
                                .GetAll()
                                .Any(x => x.AccountID == pAccountId 
                                    && x.AccountTypeID == pTypeBA);
        }

        public DateTime? DateAcceptedPolicy(int pAccountId)
        {
            var PoliciesByAccount = GetAccountPolicies(pAccountId).Where(x => GetAllActivePolicies().Any(y => y.PolicyID == x.PolicyID));
            if (PoliciesByAccount.Any())
            {
                var DateAcpt = PoliciesByAccount
                        .OrderByDescending(x => x.DateAcceptedUTC)
                        .FirstOrDefault()
                        .DateAcceptedUTC;
                return DateAcpt;
            }
            else
            {
                return null;
            }
            
        }

        public void AddAccountPolicyDetail(int pAccountID, int pPolicyID, string pIPAddress, DateTime? pDateAccepted)
        {
            var NewAccountPolicy = Create.New<AccountPolicies>();
            NewAccountPolicy.AccountID = pAccountID;
            NewAccountPolicy.PolicyID = pPolicyID;
            NewAccountPolicy.DateAcceptedUTC = pDateAccepted;
            _unitOfWork.GetRepository<AccountPolicies>().Add(NewAccountPolicy);
            var NewAccountPolicyDetails = Create.New<AccountPolicyDetails>();
            NewAccountPolicyDetails.AccountPolicyID = NewAccountPolicy.AccountPolicyID;
            NewAccountPolicyDetails.UserIPAddress = pIPAddress;
            _unitOfWork.GetRepository<AccountPolicyDetails>().Add(NewAccountPolicyDetails);
        }

        public void Commit()
        {
            _unitOfWork.Save();            
        }
        /*
        public byte[] GetReport(int pLanguageID)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Application.StartupPath "Reportes/TerminosCondiciones.rdlc";
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
            return bytes;
        }*/
    }
}
