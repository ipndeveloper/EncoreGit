using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
//Modificaciones:
//@1 20151607 BR-CC-012 GYS MD: Se implemento la clase PaymentConfigurations que representa la tabla PaymentConfigurations 
   
namespace NetSteps.Data.Entities.Business
{
     [Table("PaymentConfigurations")]
    public class PaymentConfigurations
    {
        [Column("PaymentConfigurationID"), Key]
        public int PaymentConfigurationID { get; set; }

        [Column("CollectionEntityID")]
        public int CollectionEntityID { get; set; }

        [Column("OrderStatusID")]
        public short OrderStatusID { get; set; }

        [Column("DaysForPayment")]
        public int DaysForPayment { get; set; }

        [Column("FineAndInterestRulesID")]
        public int FineAndInterestRulesID { get; set; }

        [Column("TolerancePercentage")]
        public double? TolerancePercentage { get; set; }

        [Column("ToleranceValue")]
        public int? ToleranceValue { get; set; }

        [Column("PaymentExceeded")]
        public bool? PaymentExceeded { get; set; }


        [Column("Description")]
        public string Description { get; set; } 

        [Column("NumberCuotas")]
        public int? NumberCuotas { get; set; }


         [Column("NumberDayVal")]
        public int? NumberDayVal { get; set; }

         [Column("PaymentCredi")]
         public string PaymentCredit { get; set; } 
    }
}
