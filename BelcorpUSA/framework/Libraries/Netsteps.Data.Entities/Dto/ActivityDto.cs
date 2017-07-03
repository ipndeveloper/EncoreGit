namespace NetSteps.Data.Entities.Dto
{
    using System;

    /// <summary>
    /// Descripcion de la clase
    /// </summary>
    public partial class ActivityDto
    {
        /// <summary>
        /// Obtiene o establece ActivityID
        /// </summary>
        public long ActivityID { get; set; }

        /// <summary>
        /// Obtiene o establece AccountID
        /// </summary>
        public int AccountID { get; set; }

        /// <summary>
        /// Obtiene o establece ActivityStatusID
        /// </summary>
        public short ActivityStatusID { get; set; }

        /// <summary>
        /// Obtiene o establece PeriodID
        /// </summary>
        public int PeriodID { get; set; }

        /// <summary>
        /// Obtiene o establece IsQualified
        /// </summary>
        public bool IsQualified { get; set; }

        //INI - GR_Encore-07
        /// <summary>
        /// Obtiene o establece AccountConsistencyStatusID
        /// </summary>
        public Int16 AccountConsistencyStatusID { get; set; }

        /// <summary>
        /// Obtiene o establece HasContinuity
        /// </summary>
        public bool HasContinuity { get; set; }
        //FIN - GR_Encore-07
    }
}