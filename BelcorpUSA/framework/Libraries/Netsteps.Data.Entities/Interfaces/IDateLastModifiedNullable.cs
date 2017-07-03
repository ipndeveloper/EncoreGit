using System;

namespace NetSteps.Data.Entities.Business.Interfaces
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Interface for Entities with a DateLastModified property.
	/// Created: 11-08-2010
	/// </summary>
	public interface IDateLastModifiedNullable
	{
		Nullable<DateTime> DateLastModified { get; set; }
	}

	public interface IDateLastModified
	{
		DateTime DateLastModified { get; set; }
	}
}
