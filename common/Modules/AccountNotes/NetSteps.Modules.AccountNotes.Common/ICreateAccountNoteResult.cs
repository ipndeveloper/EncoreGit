using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AccountNotes.Common
{
    /// <summary>
    /// Order Move Result
    /// </summary>
    [DTO]
    public interface ICreateAccountNoteResult
    {
        /// <summary>
        /// Success
        /// </summary>
        bool Success { get; set; }
        /// <summary>
        /// AccountID
        /// </summary>
        int AccountID { get; set; } 
        /// <summary>
        /// NoteID
        /// </summary>
        int NoteID { get; set; }
    }
}
