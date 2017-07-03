﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AccountRenewal.Common.Results
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
        /// Error Messages
        /// </summary>
        List<string> ErrorMessages { get; set; }
    }
}