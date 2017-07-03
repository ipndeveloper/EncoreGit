using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business
{
    [Table("ReportedOverduePayments")]
    public class ReportedOverduePayment
    {
        [Column("ReportedOverduePaymentID"), Key]
        public int ReportedOverduePaymentID { get; set; }
        
        [Column("AccountID")]
        [Required(ErrorMessage = "Required")]
        public int AccountID { get; set; }
        
        [Column("AccountName")]
        [Required(ErrorMessage = "Required")]
        public string AccountName { get; set; }
        
        [Column("OrderPaymentID")]
        [Required(ErrorMessage = "Required")]
        public int OrderPaymentID { get; set; }
        
        [Column("OrderID")]
        [Required(ErrorMessage = "Required")]
        public int OrderID { get; set; }
        
        [Column("Days")]
        [Required(ErrorMessage = "Required")]
        public int Days { get; set; }
        
        [Column("DateReported")]
        [Required(ErrorMessage = "Required")]
        public DateTime DateReported { get; set; }
        
        [Column("FileCode")]
        [Required(ErrorMessage = "Required")]
        public int FileCode { get; set; }
    }
}
