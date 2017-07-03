using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IOrderPaymentBusinessLogic
    {
        string GetDisplayName(OrderPayment orderPayment);
        BasicResponse IsPaymentValidForAuthorization(IOrderPaymentRepository repository, OrderPayment entity);
        IEnumerable<IOrderPayment> FilterByNachaClassType(IOrderPaymentRepository repository, string nachaClassType);
        IEnumerable<OrderPayment> FilterByNachaClassTypeAndCountryID(IOrderPaymentRepository repository, string nachaClassType, int countryID);
        IEnumerable<IOrderPayment> FilterByDateAndNachaClassType(IOrderPaymentRepository repository, DateTime startDate, DateTime endDate, string nachaClassType);
        IOrderPayment LoadOrderPaymentByPaymentId(IOrderPaymentRepository repository, int orderPaymentId);
    }
}
