using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Agreements.Common
{
    /// <summary>
    /// Agreement kind info
    /// </summary>
    [DTO]
    public interface IAgreementKind
    {
        /// <summary>
        /// Agreement kind identifier
        /// </summary>
        int AgreementKindId { get; set; }

        /// <summary>
        /// Term name for translation
        /// </summary>
        string TermName { get; set; }
    }
}
