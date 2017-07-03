using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Events.Common.Repositories
{
	public interface IVolumeRepository
	{
		decimal GetPersonalVolumeByAccountID(int accountID);
	}
}
