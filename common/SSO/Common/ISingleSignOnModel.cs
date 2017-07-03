using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.SSO.Common
{

    /// <summary>
    /// Model used to encode and decode url parameters
    /// </summary>
    [DTO]
    public interface ISingleSignOnModel
    {
        /// <summary>
        /// Decoded text, text that is going to be encoded
        /// </summary>
        string DecodedText { get; set; }

        /// <summary>
        /// Encoded text, text that is going to be decoded
        /// </summary>
        string EncodedText { get; set; }
    }
}
