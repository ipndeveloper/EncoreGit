/**
 * Clase: Modelo para obtener los valores de moneda
 * Proyecto: nscore
 * Author: Juan Morales Olivares - CSTI
 * Año: 2016
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorBackOffice.Areas.Orders.Models
{
    public class CurrencyModel
    {
        public string CurrencyID     { get; set; }
        public string CurrencyCode   { get; set; }
        public string CurrencySymbol { get; set; }
        public string CultureInfo    { get; set; }
    }
}