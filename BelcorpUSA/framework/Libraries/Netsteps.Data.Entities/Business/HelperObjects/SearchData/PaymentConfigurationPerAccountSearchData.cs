using System;
using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Attributes;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// Period Business Entity to Search
    /// </summary>

    public class PaymentConfigurationPerAccountSearchData
    {   

        public int PaymentConfigurationPerAccountID { get; set; }

        public int PaymentConfigurationID { get; set; }

        public short AccountTypeID { get; set; }
    }
}
