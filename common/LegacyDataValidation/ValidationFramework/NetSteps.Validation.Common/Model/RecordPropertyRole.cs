using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Common.Model
{
    public enum RecordPropertyRole
    {
        /// <summary>
        /// The property is a primary key
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// The property is a foreign key
        /// </summary>
        ForeignKey,
        /// <summary>
        /// The property is a validated calculation
        /// </summary>
        ValidatedField,
        /// <summary>
        /// The property is assumed to be factual
        /// </summary>
        Fact
    }
}
