﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business.HelperObjects.SearchData
{
    [Serializable]
    public class CollectionTypesPerBankSearchData
    {
        [Display(Name = "CollectionTypesPerBankID")]
        public int? CollectionTypesPerBankID { get; set; }

        [Display(Name = "BankID")]
        public int? BankID { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "TermName")]
        public string TermName { get; set; }

    }
}
