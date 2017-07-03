using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Accounts.Downline.Common.Models
{
	/// <summary>
	/// Specialization of IDownlineData that also includes the name of the business entity. 
	/// </summary>
	[DTO]
	public interface IBusinessEntityDownlineData : IDownlineData
	{
		/// <summary>
		/// The name of the Business Entity on the account
		/// </summary>
		string EntityName { get; set; }
	}
}
