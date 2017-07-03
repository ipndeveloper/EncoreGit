using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Common.Model
{
    [Flags]
    public enum ValidationCommentKind
    {
        None = 0x0,
        PrimaryRecordIdentifier = 0x1,
        ChildRecordIdentifier = 0x1 << 1,
        Error = 0x1 << 2,
        Warning = 0x1 << 3,
        CalculationComment = 0x1 << 4,
        Performance = 0x1 << 5,
        AllRecordIdentifiers = PrimaryRecordIdentifier | ChildRecordIdentifier,
        Errors = Error | Warning,
        All = AllRecordIdentifiers | Errors | CalculationComment | Performance
    }

    

    public static class ValidationCommentExtensions
    {
        public static bool IsIn(this ValidationCommentKind kind, ValidationCommentKind specificKind)
        {
            return (kind & specificKind) == kind;
        }
    }
}
