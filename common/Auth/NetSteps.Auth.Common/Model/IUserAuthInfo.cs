using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Auth.Common.Model
{
	[DTO]
    public interface IUserAuthInfo
    {
        string UserIdentifier { get; set; }
		AuthenticationStoreField UserIdentifierField { get; set; }
        string PasswordHash { get; set; }
        string PasswordSalt { get; set; }
		string HashAlgorithm { get; set; }
		int UserID { get; set; }
    }
}
