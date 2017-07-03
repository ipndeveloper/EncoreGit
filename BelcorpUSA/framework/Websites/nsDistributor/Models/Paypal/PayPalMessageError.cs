using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsDistributor.Models.Paypal
{
    public class PayPalMessageError
    {
        public int PayPalErrorID    { get; set; }
	    public string PP_Cause      { get; set; }
	    public string PP_Action     { get; set; }
	    public string PP_Message    { get; set; }
	    public string PP_Obs        { get; set; }
    }
}