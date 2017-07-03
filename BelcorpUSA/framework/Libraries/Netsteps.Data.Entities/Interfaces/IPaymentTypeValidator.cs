// -----------------------------------------------------------------------
// <copyright file="IPaymentTypeValidator.cs" company="NetSteps">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.Interfaces;
namespace NetSteps.Data.Entities.Interfaces
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IPaymentTypeValidator
    {
        BasicResponse IsValidPayment(Order order, IPayment payment, decimal amount);
        decimal DetermineNewPaymentAmount(Order order, IPayment payment, decimal amount);
    }
}
