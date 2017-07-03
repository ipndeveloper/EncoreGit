using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
	[DataContract(Name = "kitItemValuationModel")]
	public class KitItemValuationModel
	{
		[DataMember(Name = "parentSku", IsRequired = true)]
		public string ParentSku { get; set; }
	
		[DataMember(Name = "childSku", IsRequired = true)]
		public string ChildSku { get; set; }

		[DataMember(Name = "participationPercentage", IsRequired = true)]
		public decimal ParticipationPercentage { get; set; }
	}
}
