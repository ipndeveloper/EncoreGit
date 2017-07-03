using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Agreements.Common
{
    /// <summary>
    /// Defines the information required to display or save Agreement version data
    /// </summary>
    [DTO]
    public interface IAgreementVersion
    {
        /// <summary>
        /// Gets the term used to describe this agreement
        /// </summary>
        string TermName { get; set; }

        /// <summary>
        /// Gets the base agreement identifier, matching all versions of this agreement
        /// </summary>
        int AgreementId { get; set; }

        /// <summary>
        /// Gets the version identifier, specific to this version of the agreement
        /// </summary>
        int AgreementVersionId { get; set; }

        /// <summary>
        /// Gets the user-set version identifier
        /// </summary>
        string VersionNumber { get; set; }

        /// <summary>
        /// Gets the main text of this agreement's terms and/or conditions
        /// </summary>
        string AgreementText { get; set; }

        /// <summary>
        /// Gets the file containing the Agreement text for download
        /// </summary>
        string AgreementFile { get; set; }

        /// <summary>
        /// Gets the AgreementKinds this Agreement applies to
        /// </summary>
        IEnumerable<IAgreementKind> AgreementKinds { get; set; }

        /// <summary>
        /// Gets the AccountKinds (AccountTypeIds) this Agreement applies to
        /// </summary>
        IEnumerable<int> AccountKinds { get; set; }

        /// <summary>
        /// Gets the date this version of the agreement was released/created
        /// </summary>
        DateTime? DateReleasedUtc { get; set; }
    }
}
