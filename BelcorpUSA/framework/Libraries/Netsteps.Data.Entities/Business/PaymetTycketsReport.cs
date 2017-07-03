using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    public enum PaymetTycketsReportColumn
    {
            RowNumber	,
            AccountNumber	,
            AccountName	,
            AccountID	,
            TicketNumber	,
            OrderNumber	,
            StartDate	,
            Period,
            DateCreatedUTC	,
            CurrentExpirationDateUTC	,
            DateValidity,
            InitialAmount	,
            FinancialAmount	,
            DiscountedAmount,
            TotalAmount	,
            StatusPaymentName	,
            ProcessOnDateUTC	,
            NegotiationLevelName,
            List_TotalRegistros,
            OrderID,
            OrderPaymentID,
            PaymentTypeID,
            DescPayConf,
            NameExpiration,
            ExpirationDays,
            Forefit,
            ExpirationStatusID

        
    }
   public  class PaymetTycketsReport
    {
        public string  RowNumber{get;set;}
        public string   AccountNumber	{get;set;}
        public string  AccountName	{get;set;}
        public int AccountID	{get;set;}
        public string  TicketNumber	{get;set;}
        public string  OrderNumber	{get;set;}
        public DateTime StartDate	{get;set;}
        public string Period { get; set; }
        public DateTime    DateCreatedUTC	{get;set;}
        public DateTime   CurrentExpirationDateUTC	{get;set;}
        public DateTime DateValidity { get; set; }
        public decimal    InitialAmount	{get;set;}
        public decimal    FinancialAmount	{get;set;}
        public decimal DiscountedAmount { get; set; }
        public decimal TotalAmount	{get;set;}
        public string   StatusPaymentName	{get;set;}
        public DateTime   ProcessOnDateUTC	{get;set;}
        public string  NegotiationLevelName { get; set; }
        public int OrderID { get; set; }
        public int OrderPaymentID { get; set; }
        public int List_TotalRegistros { get; set; }
        public string ViewTicket { get; set; }
        public string PaymentTypeID { get; set; }       
        public string DescPayConf { get; set; }//**
        public string NameExpiration { get; set; }
        public int ExpirationStatusID { get; set; }
        public int ExpirationDays { get; set; }
        public int Forefit { get; set; }

        public PaymetTycketsReport()
        {
            ViewTicket = "ViewTicket";
        }
   }
}
 