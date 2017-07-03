using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using NetSteps.Integrations.Service.DataModels;

namespace NetSteps.Integrations.Service.Interfaces
{
	[ServiceContract(Name = "salesAPI", Namespace = "netSteps.sales")]
	public interface ISalesAPI
	{
		[OperationContract]
		OrderModelCollection GetSales(string userName, string password, DateTime startDate, DateTime endDate);

		[OperationContract]
		bool CreateKitItemValuations(string userName, string password, KitItemValuationModelCollection definitions);
	}
}
