using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    public class XMLProductSearchData
    {
        public string SKU { get; set; }
        public List<Errores> Errores { get; set; }
        public List<ErroresConfirmacionPago> ErroresCP { get; set; }
       
    }

    public class Errores
    {
        public string CampoError { get; set; }
        public string DescError { get; set; }
    }

    public class ErroresConfirmacionPago
    {
        public string CodigoOrdemPagamento { get; set; }
        public string CampoError { get; set; }
        public string DescError { get; set; }
    }
}

