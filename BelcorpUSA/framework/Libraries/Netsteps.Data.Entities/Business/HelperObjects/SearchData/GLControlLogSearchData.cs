using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class GLControlLogSearchData
    {
        [Display(Name = "Reason")]
        public string Reason { get; set; }

        [Display(Name = "Initial Amount")]
        public decimal InitialAmount { get; set; }

        [Display(Name = "Interest Amount")]
	    public decimal InterestAmount { get; set; }

        [Display(Name = "Fine Amount")]
	    public decimal FineAmount { get; set; }

        [Display(Name = "Total Amount")]
	    public decimal TotalAmount { get; set; }

        [Display(Name = "Date")]
	    public DateTime DateModifiedUTC { get; set; }

        //[Display(Name = "Responsible")]
        //public int ModifiedByUserID { get; set; }
        [Display(Name = "Responsible")]
        public string ModifiedByUserName { get; set; }

        [Display(Name = "Ticket#")]
        public int TicketNumber { get; set; }

        
    }
}
