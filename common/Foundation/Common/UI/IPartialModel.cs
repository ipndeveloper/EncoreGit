using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Foundation.Common
{
    /// <summary>
    /// A model for a partial view
    /// </summary>
    [DTO]
    public interface IPartialModel
    {
        /// <summary>
        /// An optional title that may be displayed on the containing view, outside of the partial view.
        /// </summary>
        string PartialTitle { get; set; }
        
        /// <summary>
        /// The name of the partial view used to render the model.
        /// </summary>
        string PartialName { get; set; }
    }
}
