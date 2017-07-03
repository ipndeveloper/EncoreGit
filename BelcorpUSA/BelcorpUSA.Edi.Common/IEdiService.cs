using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Common
{
	public interface IEdiService
	{
		void ProcessConfirmations();
		void ProcessShipNotices();
		void ProcessOrders();
	}
}
