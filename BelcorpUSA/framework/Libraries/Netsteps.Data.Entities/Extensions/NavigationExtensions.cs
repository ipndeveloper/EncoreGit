namespace NetSteps.Data.Entities.Extensions
{
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
    /// Author: John Egbert
    /// Description: Navigation Extensions
    /// Created: 05-07-2010
    /// </summary>
    public static class NavigationExtensions
    {
        public static List<Navigation> GetByNavigationTypeID(this IEnumerable<Navigation> navigations, int navigationTypeId)
        {
            var links = (from n in navigations
                         where n.NavigationTypeID == navigationTypeId
                         select n).ToList();
            return links;
        }
    }
}
