namespace NetSteps.Agreements.Services.Models
{
	using NetSteps.Agreements.Common.Models;
	using NetSteps.Agreements.Services.Entities;

	/// <summary>
	/// The specific acceptance log.
	/// </summary>
	public class SpecificAcceptanceLog : ISpecificAcceptanceLog
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SpecificAcceptanceLog"/> class.
		/// </summary>
		/// <param name="agreementAcceptanceLog">
		/// The agreement acceptance log.
		/// </param>
		public SpecificAcceptanceLog(AgreementAcceptanceLog agreementAcceptanceLog)
		{
			if (agreementAcceptanceLog == null)
			{
                Accepted = false;
				return;
			}

            Accepted = true;
			this.DateAcceptedUtc = agreementAcceptanceLog.DateAcceptedUtc;
		}

		/// <summary>
		/// Gets or sets the date accepted UTC.
		/// </summary>
		public System.DateTime DateAcceptedUtc { get; set; }

        /// <summary>
        /// Gets or sets if accepted.
        /// </summary>
        public bool Accepted { get; set; }
	}
}
