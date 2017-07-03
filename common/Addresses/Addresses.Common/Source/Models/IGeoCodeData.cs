using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Contains location coordinates.
	/// </summary>
	[DTO]
	public interface IGeoCodeData
	{
		/// <summary>
		/// The latitude coordinate.
		/// </summary>
		double Latitude { get; set; }

		/// <summary>
		/// The longitude coordinate.
		/// </summary>
		double Longitude { get; set; }
	}
}
