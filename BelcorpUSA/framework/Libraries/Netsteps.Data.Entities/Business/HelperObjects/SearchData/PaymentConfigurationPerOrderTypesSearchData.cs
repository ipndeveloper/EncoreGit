using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Period Business Entity to Search
    /// </summary>
    [Serializable]
    public class PaymentConfigurationPerOrderTypesSearchData
    {

        public int PaymentConfigurationPerOrderTypeID  { get; set; }

        public int PaymentConfigurationID { get; set; }

        public short OrderTypeID { get; set; }
    }
}
