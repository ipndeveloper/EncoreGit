using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    public class SponsorDataTitleType
    {
        //Obtencion de datos de TitlesType para opcion Restrisct Per Titles?
        //Developed by Kelvin Lopez C. - CSTI
        public int TitleID { get; set; }
        public string Name { get; set; }
        public int TitleTypeID { get; set; }
    }
}
