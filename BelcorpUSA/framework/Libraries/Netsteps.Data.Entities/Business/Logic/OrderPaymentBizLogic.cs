using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrderPaymentBizLogic : IOrderPaymentBizLogic
    {

        #region Instance
        private static IOrderPaymentBizLogic _instance;

        public static IOrderPaymentBizLogic Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrderPaymentBizLogic();
                return _instance;
            }
        }

        #endregion


        public List<DebtsPerAgeSearchData> GetTableDebtsPerAge(DebtsPerAgeSearchParameters searchParameters)
        {
            searchParameters.PageNumber++;
            searchParameters.EndBirthDate = searchParameters.EndBirthDate.HasValue ? (DateTime?)searchParameters.EndBirthDate.Value.AddDays(1) : null;
            searchParameters.EndDueDate = searchParameters.EndDueDate.HasValue ? (DateTime?)searchParameters.EndDueDate.Value.AddDays(1) : null;
            IEnumerable<DebtsPerAgeSearchData> list = new List<DebtsPerAgeSearchData>();

            list = new OrderPaymentRepository().GetTableDebtsPerAge(searchParameters);

            switch (searchParameters.OrderBy)
            {
                case "AccountNumber": list = list.OrderBy(d => d.AccountNumber); break;
                case "FirstName": list = list.OrderBy(d => d.FirstName); break;
                case "LastName": list = list.OrderBy(d => d.LastName); break;
                case "PaymentTicketNumber": list = list.OrderBy(d => d.PaymentTicketNumber); break;
                case "OrderNumber": list = list.OrderBy(d => d.OrderNumber); break;
                case "NfeNumber": list = list.OrderBy(d => d.NfeNumber); break;
                case "OrderDate": list = list.OrderBy(d => d.OrderDate); break;
                case "ExpirationDate": list = list.OrderBy(d => d.ExpirationDate); break;
                case "BalanceDate": list = list.OrderBy(d => d.BalanceDate); break;
                case "OriginalBalance": list = list.OrderBy(d => d.OriginalBalance); break;
                case "CurrentBalance": list = list.OrderBy(d => d.CurrentBalance); break;
                case "OverdueDays": list = list.OrderBy(d => d.OverdueDays); break;
                case "Forfeit": list = list.OrderBy(d => d.Forfeit); break;
                case "Period": list = list.OrderBy(d => d.Period); break;
                case "DateOfBirth": list = list.OrderBy(d => d.DateOfBirth); break;
                default: break;
            }

            if (searchParameters.SortOrder == NetSteps.Common.Constants.SortDirection.Descending)
                list = list.Reverse();

            return list.ToList();
        }

        public List<DebtsPerAgeSearchData> TableDebtsPerAgeExport(DebtsPerAgeSearchParameters searchParameters)
        {
            searchParameters.EndBirthDate = searchParameters.EndBirthDate.HasValue ? (DateTime?)searchParameters.EndBirthDate.Value.AddDays(1) : null;
            searchParameters.EndDueDate = searchParameters.EndDueDate.HasValue ? (DateTime?)searchParameters.EndDueDate.Value.AddDays(1) : null;

            return new OrderPaymentRepository().TableDebtsPerAgeExport(searchParameters);
        }

        public List<TicketPaymentPerMonthSearchData> GetTableTicketPaymentsPerMonth(TicketPaymentPerMonthSearchParameters searchParameters)
        {
            searchParameters.PageNumber++;
            searchParameters.EndIssueDate = searchParameters.EndIssueDate.HasValue ? (DateTime?)searchParameters.EndIssueDate.Value.AddDays(1) : null;
            searchParameters.EndDueDate = searchParameters.EndDueDate.HasValue ? (DateTime?)searchParameters.EndDueDate.Value.AddDays(1) : null;
            IEnumerable<TicketPaymentPerMonthSearchData> list = new List<TicketPaymentPerMonthSearchData>();

            list = new OrderPaymentRepository().GetTableTicketPaymentsPerMonth(searchParameters);

            switch (searchParameters.OrderBy)
            {
                case "PaymentTicketNumber": list = list.OrderBy(d => d.PaymentTicketNumber); break;
                case "OrderNumber": list = list.OrderBy(d => d.OrderNumber); break;
                case "NfeNumber": list = list.OrderBy(d => d.NfeNumber); break;
                case "OrderDate": list = list.OrderBy(d => d.OrderDate); break;
                case "ExpirationDate": list = list.OrderBy(d => d.ExpirationDate); break;
                case "BalanceDate": list = list.OrderBy(d => d.BalanceDate); break;
                case "OriginalBalance": list = list.OrderBy(d => d.OriginalBalance); break;
                case "CurrentBalance": list = list.OrderBy(d => d.CurrentBalance); break;
                case "Status": list = list.OrderBy(t => t.Status); break;
                case "OriginalExpirationDate": list = list.OrderBy(t => t.OriginalExpirationDate); break;
                case "AccountNumber": list = list.OrderBy(d => d.AccountNumber); break;
                case "FirstName": list = list.OrderBy(d => d.FirstName); break;
                case "LastName": list = list.OrderBy(d => d.LastName); break;
                case "PhoneNumber": list = list.OrderBy(t => t.PhoneNumber); break;
                default: break;
            }

            if (searchParameters.SortOrder == NetSteps.Common.Constants.SortDirection.Descending)
                list = list.Reverse();

            return list.ToList();
        }

        public List<TicketPaymentPerMonthSearchData> TableTicketPaymentsPerMonthExport(TicketPaymentPerMonthSearchParameters searchParameters)
        {
            searchParameters.EndIssueDate = searchParameters.EndIssueDate.HasValue ? (DateTime?)searchParameters.EndIssueDate.Value.AddDays(1) : null;
            searchParameters.EndDueDate = searchParameters.EndDueDate.HasValue ? (DateTime?)searchParameters.EndDueDate.Value.AddDays(1) : null;

            return new OrderPaymentRepository().TableTicketPaymentsPerMonthExport(searchParameters);
        }

        public List<OrderPaymentVirtualDesktop> TableOrderPaymentVirtualDesktop(int accountID)
        {
           return new OrderPaymentRepository().TableOrderPaymentVirtualDesktop(accountID);
        }

        public Dictionary<int, string> GetDropDownStatuses()
        {
            return new OrderPaymentRepository().GetDropDownStatuses();
        }

        public Dictionary<int, string> GetTicketNumberLookUp(string ticketNumberPrefix)
        {
            Dictionary<int, string> tickets = new Dictionary<int, string>();
            
            if (ticketNumberPrefix.Length > 0)
                tickets = new OrderPaymentRepository().GetTicketNumberLookUp(ticketNumberPrefix);

            return tickets;
        }

    }
}
