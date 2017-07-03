using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class CommissionBusinessLogic : ICommissionBusinessLogic
    {

        #region Instance
        private static ICommissionBusinessLogic _instance;

        public static ICommissionBusinessLogic Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new CommissionBusinessLogic();
                return _instance;
            }
        }
        #endregion

        public Dictionary<int, string> GetPeriods()
        {
            return CommissionRepository.Instance.GetPeriods();
        }

        public Dictionary<string, string> GetCommissionTypes()
        {
            return CommissionRepository.Instance.GetCommissionTypes();
        }

        public List<CommissionSearchData> GetTotalCommissions(CommissionSearchParameters searchParameters)
        {
            searchParameters.PageNumber++;
            IEnumerable<CommissionSearchData> list = new List<CommissionSearchData>();

            list = CommissionRepository.Instance.GetTotalCommissions(searchParameters);

            switch (searchParameters.OrderColumn)
            {
                case "Period": list = list.OrderBy(c => c.PeriodID); break;
                case "AccountNumber": list = list.OrderBy(c => c.AccountNumber); break;
                case "AccountName": list = list.OrderBy(c => c.AccountName); break;
                case "CommissionType": list = list.OrderBy(c => c.CommissionType); break;
                case "CommissionName": list = list.OrderBy(c => c.CommissionName); break;
                case "CommissionAmmount": list = list.OrderBy(c => c.CommissionAmount); break;
                default: break;
            }

            if (searchParameters.SortDirection == NetSteps.Common.Constants.SortDirection.Descending)
                list = list.Reverse();

            return list.ToList();
        }

        public List<CommissionDetailSearchData> GetDetailCommissions(CommissionDetailSearchParameters searchParameters)
        {
            searchParameters.PageNumber++;
            IEnumerable<CommissionDetailSearchData> list = new List<CommissionDetailSearchData>();

            list = CommissionRepository.Instance.GetDetailCommissions(searchParameters);

            switch (searchParameters.OrderColumn)
            {
                case "SponsorNumber": list = list.OrderBy(c => c.SponsorID); break;
                case "SponsorName": list = list.OrderBy(c => c.SponsorName); break;
                case "AccountNumber": list = list.OrderBy(c => c.AccountNumber); break;
                case "AccountName": list = list.OrderBy(c => c.AccountName); break;
                case "CommissionType": list = list.OrderBy(c => c.CommissionType); break;
                case "CommissionName": list = list.OrderBy(c => c.CommissionName); break;
                case "OrderNumber": list = list.OrderBy(c => c.OrderNumber); break;
                case "CommissionableValue": list = list.OrderBy(c => c.CommissionableValue); break;
                case "Percentage": list = list.OrderBy(c => c.Percentage); break;
                case "PayoutAmount": list = list.OrderBy(c => c.PayoutAmount); break;
                case "Period": list = list.OrderBy(c => c.PeriodID); break;
                default: break;
            }

            if (searchParameters.SortDirection == NetSteps.Common.Constants.SortDirection.Descending)
                list = list.Reverse();

            return list.ToList();
        }

    }
}
