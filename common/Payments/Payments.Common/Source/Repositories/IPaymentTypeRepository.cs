using System;
using NetSteps.Payments.Common.Models;

namespace NetSteps.Payments.Common.Repositories
{
	/// <summary>
	/// Common interface for IPaymentTypeRepository
	/// </summary>
	public interface IPaymentTypeRepository
	{
		/// <summary>
		/// Takes a predicate which is applied against all PaymentTypes in the database
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns>Returns all PaymentTypes that satisfy the predicate</returns>
		IPaymentType[] GetPaymentTypes(Predicate<IPaymentType> predicate);

		/// <summary>
		/// Returns all PaymentTypes
		/// </summary>
		/// <returns></returns>
		IPaymentType[] GetAllPaymentTypes();
	}
}
