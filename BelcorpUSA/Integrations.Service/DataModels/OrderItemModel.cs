using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
    [DataContract(Name = "orderItem")]
    public class OrderItemModel : OrderItemBaseModel
    {
        /// <summary>
        /// Gets or sets the child items.
        /// </summary>
        /// <value>
        /// The child items.
        /// </value>
        [DataMember(Name = "childItems", IsRequired = false)]
        public OrderKitItemModelCollection ChildItems { get; set; }
    }
}
