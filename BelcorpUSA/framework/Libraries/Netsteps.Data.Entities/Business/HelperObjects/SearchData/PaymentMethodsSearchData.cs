using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Common.Attributes;
using System.ComponentModel.DataAnnotations;

//@01 20150717 BR-CC-003 G&S LIB: Se crea la clase con sus respectivos métodos

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{

    [Serializable]
    public class PaymentMethodsSearchData
    {
        
        [Display(Name = "Payment Configuration ID")]
        public int PaymentConfigurationID { get; set; }
        
        [Display(Name = "Collection Entities")]
        public string CollectionEntityName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; } 
        
        [Display(Name = "Fine and Interest Rules")]
        public string FineandInterestRules { get; set; }
       
        //[Display(Name = "Order Status")]
        [Display(AutoGenerateField = false)]
        public string OrderStatus { get; set; }

       
        [Display(Name = "Days For Payment")]
        public int DaysForPayment { get; set; }

        [Display(Name = "Days of Validity")]
        public int DaysValidity { get; set; }
        
        [Display(Name = "Tolerance Percentage")]
        public double TolerancePercentage { get; set; }

        
        [Display(Name = "Tolerance Value")]
        public int ToleranceValue { get; set; }

        
        //[Display(Name = "Account Type Restriction")]
        [Display(AutoGenerateField = false)]
        public string AccountTypeRestriction { get; set; }

      
        //[Display(Name = "Order Type Restriction")]
        [Display(AutoGenerateField = false)]
        public string OrderTypeRestriction { get; set; }

  
        //[Display(Name = "State Restriction")]
        [Display(AutoGenerateField = false)]
        public string StateRestriction { get; set; }

       
        //[Display(Name = "City Restriction")]
        [Display(AutoGenerateField = false)]
        public string CityRestriction { get; set; }

       
        //[Display(Name = "County Restriction")]
        [Display(AutoGenerateField = false)]
        public string CountyRestriction { get; set; } 

        [Display(Name = "NumberCuotas")]
        public int NumberCuotas { get; set; }
        [Display(Name = "UtilizaCredito")]
        public string UtilizaCredito { get; set; } 
    }
}
