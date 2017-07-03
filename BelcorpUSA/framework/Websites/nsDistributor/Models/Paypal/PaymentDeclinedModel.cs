/**
 * Clase: Modelo para el manejo de operaciones de pasarela de pago con PayPal
 * Proyecto: nscore
 * Author: Juan Morales Olivares - CSTI
 * Año: 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nsDistributor.Models.Paypal
{
    public class PaymentDeclinedModel
    {
        public int PaymentDeclinedID  { get; set; }
        public string TypeError       { get; set; }
	    public string PaymentDecDate  { get; set; } 
	    public int OrderID            { get; set; } 
	    public string PaymentDecMon   { get; set; } 
	    public int PaymentDecCuo      { get; set; } 
	    public int AccountId          { get; set; }
        public int PaymentGatewayID   { get; set; } 

    }
}