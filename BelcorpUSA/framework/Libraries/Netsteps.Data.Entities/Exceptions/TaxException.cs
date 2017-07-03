namespace NetSteps.Common.Exceptions
{
	using System;

	[Serializable]
	public class TaxException : Exception
	{
		public TaxException(
			 string message)
			: base(message)
		{
		}


		public TaxException(
			 Exception exception)
			: base(exception.Message, exception)
		{
		}
	}
}
