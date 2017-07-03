using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Validation.Common
{
    /// <summary>
    /// Suggests how the property validation has been handled by the record validation handler.
    /// </summary>
    public enum PropertyValidationType
    {
        /// <summary>
        /// The property is assumed to be a fact that is not a validation-calculated value.
        /// </summary>
        TreatedAsFact,
        /// <summary>
        /// The property has a calculation that has been applied during validation by the record validation handler.
        /// </summary>
        CalculatedValue
    }
}
