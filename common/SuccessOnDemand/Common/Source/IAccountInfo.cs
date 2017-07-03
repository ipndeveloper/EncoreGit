using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.SOD.Common
{
    [DTO]
    public interface IAccountInfo
    {
        /// <summary>
        /// The distributor's ID. Leave blank when calling Create API.
        /// </summary>
        string DistID { get; set; }

        string FirstName { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        string Email { get; set; }
        string Website { get; set; }
        string Phone { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
        string Country { get; set; }
        string Language { get; set; }
        string Active { get; set; }
        string Type { get; set; }
        string Rank { get; set; }
        string ID { get; set; }
    }
}
