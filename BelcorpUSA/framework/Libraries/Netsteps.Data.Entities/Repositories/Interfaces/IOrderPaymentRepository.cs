using System.Collections.Generic;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects;
using System;

namespace NetSteps.Data.Entities.Repositories
{
    #region Modifications
    // @01 GYS - BR-CC-17 - Reporte de Estado Mensual de Boletos de Pago
    #endregion

    public partial interface IOrderPaymentRepository
    {
        List<OrderPaymentResult> LoadOrderPaymentResultsByOrderPaymentID(int orderPaymentID);
        bool HasOrderPaymentResults(int orderPaymentID);
        IEnumerable<IOrderPayment> LoadEftOrderPayments();
        IEnumerable<IOrderPayment> GetUnSubmittedOrderPaymentsByClassType(string classType);
        IEnumerable<OrderPayment> GetUnSubmittedOrderPaymentsByClassTypeAndCountryID(string classType, int countryID);
        IOrderPayment LoadEftOrderPaymentByOrderId(int orderId);
        IOrderPayment LoadOrderPaymentByOrderPaymentId(int orderPaymentId);
        IOrderPayment LoadOrderPaymentByOrderId(int orderId);

        #region Modifications @1
        List<DebtsPerAgeSearchData> GetTableDebtsPerAge(DebtsPerAgeSearchParameters parameters);
        List<DebtsPerAgeSearchData> TableDebtsPerAgeExport(DebtsPerAgeSearchParameters parameters);
        List<TicketPaymentPerMonthSearchData> GetTableTicketPaymentsPerMonth(TicketPaymentPerMonthSearchParameters parameters);
        List<TicketPaymentPerMonthSearchData> TableTicketPaymentsPerMonthExport(TicketPaymentPerMonthSearchParameters parameters);
        List<OrderPaymentVirtualDesktop> TableOrderPaymentVirtualDesktop(int accountID);
        

        Dictionary<int, string> GetDropDownStatuses();
        Dictionary<int, string> GetTicketNumberLookUp(string ticketNumberPrefix);
        #endregion

    }

}
