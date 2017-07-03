using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name="orderModel")]
    public class OrderModel
    {
		[DataMember(Name = "orderID", IsRequired = true)]
		public int OrderID { get; set; }

        [DataMember(Name = "partyID", IsRequired = true)]
        public int PartyID { get; set; }

        [DataMember(Name = "orderDate", IsRequired = true)]
        public DateTime OrderDate { get; set; }

		[DataMember(Name = "orderType", IsRequired = true)]
		public int OrderType { get; set; }

        [DataMember(Name = "partyHostAccountID", IsRequired = false)]
        public int PartyHostAccountID { get; set; }

        [DataMember(Name = "distributorAccountID", IsRequired = true)]
        public int DistributorAccountID { get; set; }

        [DataMember(Name = "distributorName", IsRequired = true)]
        public string DistributorName { get; set; }

        [DataMember(Name = "distributorState", IsRequired = true)]
        public string DistributorState { get; set; }

        [DataMember(Name = "distributorRegion", IsRequired = true)]
        public string DistributorRegion { get; set; }

		[DataMember(Name = "accountID", IsRequired = true)]
		public int AccountID { get; set; }

		[DataMember(Name = "accountName", IsRequired = true)]
		public string AccountName { get; set; }

		[DataMember(Name = "accountTypeID", IsRequired = true)]
		public int AccountTypeID { get; set; }

		[DataMember(Name = "ccPaymentTotal", IsRequired = true)]
		public decimal CCPaymentTotal { get; set; }

		[DataMember(Name = "ledgerPaymentTotal", IsRequired = true)]
		public decimal LedgerPaymentTotal { get; set; }

		[DataMember(Name = "subtotal", IsRequired = true)]
		public decimal SubTotal { get; set; }

		[DataMember(Name = "taxTotal", IsRequired = true)]
		public decimal TaxTotal { get; set; }

		[DataMember(Name = "shippingTotal", IsRequired = true)]
		public decimal ShippingTotal { get; set; }

		[DataMember(Name = "handlingTotal", IsRequired = true)]
		public decimal HandlingTotal { get; set; }

		[DataMember(Name = "total", IsRequired = true)]
		public decimal Total { get; set; }

		[DataMember(Name = "totalQV", IsRequired = true)]
		public decimal TotalQV { get; set; }

		[DataMember(Name = "totalCV", IsRequired = true)]
		public decimal TotalCV { get; set; }

		[DataMember(Name = "items", IsRequired = true)]
		public OrderItemModelCollection Items { get; set; }
    }
}
