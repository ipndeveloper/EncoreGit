using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Validation.Common
{
    /// <summary>
    /// Represents the result of record validation
    /// </summary>
    [Flags]
    public enum ValidationResultKind
    {
        /// <summary>
        /// The record or property has not yet been validated.
        /// </summary>
        Unvalidated = 0x0,
        /// <summary>
        /// The record or property is factual in nature. (i.e. not participating in validation, used for reference only.)
        /// </summary>
        IsFactual = 0x1,
        /// <summary>
        /// The record or property contains no errors.
        /// </summary>
        IsCorrect = 0x1 << 1,
        /// <summary>
        /// The record or property contains errors but errors are within the accepted range.
        /// </summary>
        IsWithinMarginOfError = 0x1 << 2,
        /// <summary>
        /// The record or property contains breaking errors.
        /// </summary>
        IsIncorrect = 0x1 << 3,
        /// <summary>
        /// The record or property is broken
        /// </summary>
        IsBroken = 0x1 << 4,
        /// <summary>
        /// The record or property is not in the database, and should be added
        /// </summary>
        IsNew = 0x1 << 5,
        
        /// <summary>
        /// All result statuses
        /// </summary>
        All = Unvalidated | IsCorrect | IsIncorrect | IsWithinMarginOfError | IsBroken | IsNew
    }

    public static class ValidationResultExtensions
    {
        public static bool IsIn(this ValidationResultKind kind, ValidationResultKind specificKind)
        {
            return (kind & specificKind) == kind;
        }

        public static ValidationResultKind EscalateTo(this ValidationResultKind kind, ValidationResultKind escalateTo)
        {
            if (escalateTo > kind)
            {
                return escalateTo;
            }
            else
            {
                return kind;
            }
        }
    }
}
