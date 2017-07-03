using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class SponsorshipSearchData
    {
        //Obtencion de datos de RequerimentTypes para opcion ValidDoc
        //Developed by Kelvin Lopez C. - CSTI
        public int IDTypeID { get; set; }
        public int RestrictionDocumentID { get; set; }
        public int RequirementTypeID { get; set; }
        public string Name { get; set; }
        public string TermName { get; set; }
        public string LogicalOperator { get; set; }
        public int Order { get; set; }

    }

}
