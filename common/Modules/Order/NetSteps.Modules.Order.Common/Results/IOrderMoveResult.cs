using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common
{
	/// <summary>
	/// Order Move Result
	/// </summary>
	[DTO]
	public interface IOrderMoveResult : IResult
	{
        /// <summary>
        /// AccountID
        /// </summary>
        int AccountID { get; set; }
	}
}
