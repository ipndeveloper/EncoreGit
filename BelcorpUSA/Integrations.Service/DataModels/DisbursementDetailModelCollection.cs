using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [CollectionDataContract
        (
           Name = "disbursementItems",
           ItemName = "item"
        )
    ]
    [KnownType(typeof(DisbursementDetailACHModel))]
    [KnownType(typeof(DisbursementDetailCheckModel))]
    public class DisbursementDetailModelCollection : Collection<DisbursementDetailBaseModel>
    {
    }
}
