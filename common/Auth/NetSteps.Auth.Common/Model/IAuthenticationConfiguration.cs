﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Model
{
	public interface IAuthenticationConfiguration
	{
		IEnumerable<string> RegisteredProviders { get; }
        IDictionary<string, bool> AdminSettings { get; }
	}
}