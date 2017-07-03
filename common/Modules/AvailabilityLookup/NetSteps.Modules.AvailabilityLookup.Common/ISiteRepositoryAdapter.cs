
namespace NetSteps.Modules.AvailabilityLookup.Common
{
	/// <summary>
	/// adapter for geting sites
	/// </summary>
	public interface ISiteRepositoryAdapter
	{
		/// <summary>
		/// get site by host name
		/// </summary>
		/// <param name="url">host name</param>
		/// <returns></returns>
		ISiteUrlAccount LoadByUrl(string url);
		/// <summary>
		/// Get site by host name and marketID 
		/// </summary>
		/// <param name="marketID"></param>
		/// <param name="url">host name</param>
		/// <returns></returns>
		ISiteUrlAccount LoadByMarketAndUrl(int marketID, string url);		
	}
}
