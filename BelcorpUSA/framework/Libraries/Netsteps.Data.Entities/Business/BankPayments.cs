using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Data.Entities.Business
{
     [Table("BankPayments")]
    public class BankPayments 
    {
         [Column("AccountCode")]
         [Display(Name = "AccountCode")] //**
         public int AccountCode { get; set; }

         [Column("AccountName")]
         [Display(Name = "AccountName")] //**
         [Required(ErrorMessage = "Required")]
         public string AccountName { get; set; }

         [Column("BankName")]
         [Display(Name = "BankName")] //**
         [Required(ErrorMessage = "Required")]
         public string BankName { get; set; }

         [Column("TicketNumbers")]
         [Display(Name = "TicketNumbers")] //**
         public int TicketNumber { get; set; }

         [Column("OrderNumber")]
         [Display(Name = "OrderNumber")] //**
         public string OrderNumber { get; set; }

         [Column("Amount")]
         [Display(Name = "Amount")] //**
         public decimal Amount { get; set; }

         [Column("DateReceivedBank")]
         [Display(Name = "DateReceivedBank")] //**
         public DateTime DateReceivedBank { get; set; }

         [Column("DateApplied")]
         [Display(Name = "DateApplied")] //**
         public DateTime DateApplied { get; set; }

         [Column("FileNameBanks")]
         [Display(Name = "FileNameBanks")] //**
         public string FileNameBank { get; set; }

         [Column("FileSequence")]
         [Display(Name = "FileSequence")] //**
         public int FileSequence { get; set; }

         [Column("Applied")]
         [Display(Name = "Applied")] //**
         public int Applied { get; set; }


        [Column("BankPaymentID"),Key]
        [Display(AutoGenerateField = false)]
        public int BankPaymentID { get; set; }        

        [Column("OrderDate")]
        [Display(AutoGenerateField = false)]
        public DateTime OrderDate { get; set; }

        [Column("BankPaymentType")]
        [Display(AutoGenerateField = false)]
        public int BankPaymentType { get; set; }       

        [Column("ResponsibleCode")]
        [Display(AutoGenerateField = false)]
        public int? ResponsibleCode { get; set; }

        [Column("ResponsibleName")]
        [Display(AutoGenerateField = false)]
        public string ResponsibleName { get; set; }

        [Column("BankID")]
        [Display(AutoGenerateField = false)]
        public int BankID { get; set; }

        [Column("logSequence")]
        [Display(AutoGenerateField = false)]
        public int logSequence { get; set; }

        [Column("Credito")]
        [Display(Name = "Credito")] //**
        public string Credito { get; set; }

        [Column("Residual")]
        [Display(Name = "Residual")] //**
        public decimal Residual { get; set; }

    }
}
