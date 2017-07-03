using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Se implemento la clase LogErrorBankPayments que representa la tabla LogErrorBankPayments 
 namespace NetSteps.Data.Entities.Business
{
    [Table("LogErrorBankPayments")]
    public class LogErrorBankPayments
    {
        [Column("LogErrorBankPaymentID"), Key]
        public int LogErrorBankPaymentID { get; set; }

        [Column("BankPaymentID")]
        public int BankPaymentID { get; set; }

        [Column("BankName")]
        [Required(ErrorMessage = "Required")]
        public string BankName { get; set; }

        [Column("TicketNumber")]
        public int TicketNumber { get; set; }

        [Column("OrderNumber")]
        public string OrderNumber { get; set; }

        [Column("Date")]
        public DateTime Date { get; set; }
    }
}
