using System.Collections;
using System.Collections.Generic;
namespace NetSteps.Data.Entities.Dto
{
    /// <summary>
    /// Bonus Type Data Transfer Object
    /// </summary>
    public class MaterialXmlDto
    {
        #region Properties
        /// <summary>
        /// Gets or sets Bonus Type Id
        /// </summary>
        public string SKU { get; set; }
         
        /// <summary>
        /// Gets or sets Bonus Code
        /// </summary>
        public string BPCS { get; set; }

        /// <summary>
        /// Gets or sets Name
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets Term Name
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets whether Bonus Type is Enabled
        /// </summary>
        public string NamePort { get; set; }

        /// <summary>
        /// Gets or sets whether Bonus Type is Editable
        /// </summary>
        public string NameEsp { get; set; }

        /// <summary>
        /// Gets or sets Plan Id
        /// </summary>
        public string Volume { get; set; }

        /// <summary>
        /// Gets or sets Earnings Type Id
        /// </summary>
        public string Weight { get; set; }

        /// <summary>
        /// Gets or sets client Name
        /// </summary>
        public string Hierachy { get; set; }
        #endregion
    }
}
