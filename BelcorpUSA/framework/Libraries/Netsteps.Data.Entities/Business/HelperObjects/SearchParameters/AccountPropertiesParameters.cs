using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Business
{
    public class AccountPropertiesParameters
    {        

        public decimal AccountPropertyID { get; set; }

        public int AccountID { get; set; }

        public int AccountPropertyTypeID { get; set; }

        public int AccountPropertyValueID { get; set; }

        public string PropertyValue { get; set; }

        public bool Active { get; set; }

        public decimal AccountReferencID { get; set; }

        public int RelationShip { get; set; }

        public string ReferenceName { get; set; }

        public Int64 PhoneNumberMain { get; set; }
    }
}
