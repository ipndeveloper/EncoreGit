using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.ShippingDataImporter.Models
{
	public class ProgressUpdatedEventArgs : EventArgs
	{
		public string Message { get; set; }
		public int CurrentStep { get; set; }
		public int TotalSteps { get; set; }
	}
}
