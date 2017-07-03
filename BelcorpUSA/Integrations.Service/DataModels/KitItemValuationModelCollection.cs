using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace NetSteps.Integrations.Service.DataModels
{
	[CollectionDataContract(Name = "kitItemValuations", ItemName = "kitItemValuation")]
	public class KitItemValuationModelCollection : Collection<KitItemValuationModel>
	{
	}
}
