using System;
using NetSteps.Common.Base;
using NetSteps.Encore.Core;

namespace NetSteps.Data.Entities.Business
{
	public class OrderSearchParameters : FilterDateRangePaginatedListParameters<Order>
	{
		static readonly int CHashCodeSeed = typeof(OrderSearchParameters).GetKeyForType().GetHashCode();

		public int? OrderStatusID { get; set; }
		public int? OrderTypeID { get; set; }
		public string OrderNumber { get; set; }
        /*CS.20AGO2016.Inicio*/
        //public string InvoiceNumber { get; set; }
        /*CS.20AGO2016.Fin*/
		public int? ConsultantAccountID { get; set; }
        public int? CustomerAccountID { get; set; }
        public int? ConsultantOrCustomerAccountID { get; set; }
		public int? AccountTypeID { get; set; }
		public string CustomerName { get; set; }
		public string CustomerAccountNumber { get; set; }
		public string ConsultantName { get; set; }
		public string ConsultantAccountNumber { get; set; }
		public DateTime? CommissionDate { get; set; }
		public int? AutoshipOrderID { get; set; }
		public bool SearchTemplates { get; set; }
		public string CreditCardLastFourDigits { get; set; }
		public bool? SearchOpenParties { get; set; }
		public int? MarketID { get; set; }
        public int? PeriodID { get; set; }
        public int? AutoshipScheduleID { get; set; }

		/// <summary>
		/// Determines if the instance is equal to another.
		/// </summary>
		/// <param name="other">the other instance</param>
		/// <returns>true if equal; otherwise false.</returns>
		public bool Equals(OrderSearchParameters other)
		{
			return other != null
                && base.Equals(other)
                && OrderStatusID == other.OrderStatusID
				&& OrderTypeID == other.OrderTypeID
				&& String.Equals(OrderNumber, other.OrderNumber, StringComparison.Ordinal)
                /*CS.20AGO2016.Inicio*/
                //&& String.Equals(InvoiceNumber, other.InvoiceNumber, StringComparison.Ordinal)
                /*CS.20AGO2016.Fin*/
                && ConsultantAccountID == other.ConsultantAccountID
                && CustomerAccountID == other.CustomerAccountID
                && ConsultantOrCustomerAccountID == other.ConsultantOrCustomerAccountID
				&& AccountTypeID == other.AccountTypeID
				&& String.Equals(CustomerName, other.CustomerName, StringComparison.Ordinal)
				&& String.Equals(CustomerAccountNumber, other.CustomerAccountNumber, StringComparison.Ordinal)
				&& String.Equals(ConsultantName, other.ConsultantName, StringComparison.Ordinal)
				&& String.Equals(ConsultantAccountNumber, other.ConsultantAccountNumber, StringComparison.Ordinal)
				&& CommissionDate == other.CommissionDate
				&& AutoshipOrderID == other.AutoshipOrderID
                && AutoshipScheduleID == other.AutoshipScheduleID
				&& SearchTemplates == other.SearchTemplates
				&& String.Equals(CreditCardLastFourDigits, other.CreditCardLastFourDigits, StringComparison.Ordinal)
				&& SearchOpenParties == other.SearchOpenParties
				&& MarketID == other.MarketID
                && PeriodID == other.PeriodID;
		}

		/// <summary>
		/// Determines if the instance is equal to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns>true if equal; otherwise false</returns>
		public override bool Equals(object obj)
		{
			return obj is OrderSearchParameters
				&& Equals((OrderSearchParameters)obj);
		}

		/// <summary>
		/// Gets the instance's hashcode.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			int prime = 999067; // a random prime
			int result = CHashCodeSeed * prime;

            result ^= base.GetHashCode() * prime;

            if (OrderStatusID.HasValue)
				result ^= OrderStatusID.Value * prime;
			
            if (OrderTypeID.HasValue)
				result ^= OrderTypeID.Value * prime;
			
            if (OrderNumber != null && OrderNumber.Length > 0)
				result ^= OrderNumber.GetHashCode() * prime;
            
            if (ConsultantAccountID.HasValue)
                result ^= ConsultantAccountID.Value * prime;
            
            if (CustomerAccountID.HasValue)
                result ^= CustomerAccountID.Value * prime;
            
            if (ConsultantOrCustomerAccountID.HasValue)
                result ^= ConsultantOrCustomerAccountID.Value * prime;
			
            if (AccountTypeID.HasValue)
				result ^= AccountTypeID.Value * prime;
			
            if (CustomerName != null && CustomerName.Length > 0)
				result ^= CustomerName.GetHashCode() * prime;
			
            if (CustomerAccountNumber != null && CustomerAccountNumber.Length > 0)
				result ^= CustomerAccountNumber.GetHashCode() * prime;
			
            if (ConsultantName != null && ConsultantName.Length > 0)
				result ^= ConsultantName.GetHashCode() * prime;
			
            if (ConsultantAccountNumber != null && ConsultantAccountNumber.Length > 0)
				result ^= ConsultantAccountNumber.GetHashCode() * prime;
			
            if (CommissionDate.HasValue && CommissionDate.Value > DateTime.MinValue)
				result ^= CommissionDate.GetHashCode() * prime;
			
			if (AutoshipOrderID.HasValue)
				result ^= AutoshipOrderID.Value * prime;

            if (AutoshipScheduleID.HasValue)
                result ^= AutoshipScheduleID.Value * prime;
			
            result ^= SearchTemplates.GetHashCode() * prime;
			
            if (CreditCardLastFourDigits != null && CreditCardLastFourDigits.Length > 0)
				result ^= CreditCardLastFourDigits.GetHashCode() * prime;
			
            if (SearchOpenParties.HasValue)
				result ^= SearchOpenParties.GetHashCode() * prime;
			
            if (MarketID.HasValue)
				result ^= MarketID.Value * prime;

            if (PeriodID.HasValue)
                result ^= PeriodID.Value * prime;

			return result;
		}
	}
}
