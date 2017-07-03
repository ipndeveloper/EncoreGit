using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{

    /// <summary>
    /// author          : mescobar
    /// company         : CSTI - Peru
    /// create          : 12/18/2015
    /// modified          : new class - DisbursementsPeriod
    ///                               BonusTypes
    ///                               EntryOrigins
    ///                               EntryReasons
    ///                               EntryTypes
    /// </summary>
    public class DisbursementsSearchData
    {
        public string Operacion { get; set; }
        public string Empresa { get; set; }
        public string CPFConsultora { get; set; }
        public string CNPJConsultora { get; set; }
        public string FechaEmision { get; set; }
        public string Campania { get; set; }
        public string Periodo { get; set; }
        public string IdConsultora { get; set; }
        public string IdDisbursement { get; set; }
        public string FechaVencimiento { get; set; }
        public string Valor { get; set; }
        public string Bloqueo { get; set; } 
    }

    public class DisbursementsPeriod
    {
        public int PeriodID { get; set; }
    }

    public class BonusTypes
    {
        public int BonusTypeID { get; set; }
        public String TermName { get; set; }
    }

    public class EntryOrigins
    {
        public int EntryOriginID { get; set; }
        public String Termname { get; set; }
    }

    public class EntryReasons
    {
        public int EntryReasonID { get; set; }
        public String Termname { get; set; }
    }

    public class EntryTypes
    {
        public int EntryTypeID { get; set; }
        public String Termname { get; set; }
    }
}
