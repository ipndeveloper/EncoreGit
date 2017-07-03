using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.Order.Common
{
    /// <summary>
    /// Result
    /// </summary>
    [DTO]   
    public interface IResult
    {
        /// <summary>
        /// Success
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// ErrorMessages
        /// </summary>
        List<string> ErrorMessages { get; set; }
    }
}
