using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Modules.AccountNotes.Common
{
    [DTO]
    public interface INote
    {
        #region Properties

        /// <summary>
        /// AccountID
        /// </summary>
        int AccountID { get; set; }
        /// <summary>
        /// NoteID
        /// </summary>
        int NoteID { get; set; }

        #endregion
    }
}
