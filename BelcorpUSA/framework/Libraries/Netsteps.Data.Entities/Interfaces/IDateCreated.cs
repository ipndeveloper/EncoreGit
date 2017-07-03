using System;

namespace NetSteps.Data.Entities.Business.Interfaces
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Interface for Entities with a DateCreated property.
	/// Created: 11-08-2010
	/// </summary>
	public interface IDateCreated
	{
		DateTime DateCreated { get; set; }
	}
}
