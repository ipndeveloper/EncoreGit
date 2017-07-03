using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
    [Flags]
    public enum ClaimSetZeroClaims
    {
        SuperAdministrator	= 1,
        SecurityOfficer		= 1 << 1,
		Reservered03		= 1 << 2,
        Reservered04		= 1 << 3,
        ExpiresOn			= 1 << 4,
        IdentityKind			= 1 << 5,
        Realm				= 1 << 6,
        Tenant				= 1 << 7,
        Application			= 1 << 8,
        UserName			= 1 << 9, 
        Reservered11		= 1 << 10,
        Reservered12		= 1 << 11,
        Reservered13		= 1 << 12,
        Reservered14		= 1 << 13,
        Reservered15		= 1 << 14,
        Reservered16		= 1 << 15,
        Reservered17		= 1 << 16,
        Reservered18		= 1 << 17,
        Reservered19		= 1 << 18,
        Reservered20		= 1 << 19,
        Reservered21		= 1 << 20,
        Reservered22		= 1 << 21,
        Reservered23		= 1 << 22,
        Reservered24		= 1 << 23,
        Reservered25		= 1 << 24,
        Reservered26		= 1 << 25,
        Reservered27		= 1 << 26,
        Reservered28		= 1 << 27,
        Reservered29		= 1 << 28,
        Reservered30		= 1 << 29,
        Reservered31		= 1 << 30,
        Reservered32		= 1 << 31
    }
}
