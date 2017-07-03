using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Addresses.Common.Models
{
	/// <summary>
	/// Common interface for basic address info.
	/// </summary>
	[DTO]
	[ContractClass(typeof(Contracts.AddressBasicContracts))]
	public interface IAddressBasic
	{
		/// <summary>
		/// The Address1 for this Address.
		/// </summary>
		string Address1 { get; set; }

		/// <summary>
		/// The Address2 for this Address.
		/// </summary>
		string Address2 { get; set; }

		/// <summary>
		/// The Address3 for this Address.
		/// </summary>
		string Address3 { get; set; }

		/// <summary>
		/// The City for this Address.
		/// </summary>
		string City { get; set; }

		/// <summary>
		/// The County for this Address.
		/// </summary>
		string County { get; set; }

		/// <summary>
		/// The State for this Address.
		/// </summary>
		string State { get; set; }

		/// <summary>
		/// The PostalCode for this Address.
		/// </summary>
		string PostalCode { get; set; }

		/// <summary>
		/// The Country for this Address.
		/// </summary>
		string Country { get; }

        ///// <summary>
        ///// The Street for this Address.
        ///// </summary>
        //string Street { get; set; }
	}

	namespace Contracts
	{
		[ContractClassFor(typeof(IAddressBasic))]
		internal abstract class AddressBasicContracts : IAddressBasic
		{
			#region Primitive properties
			string IAddressBasic.Address1
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.Address2
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.Address3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.City
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.County
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.State
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.PostalCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

			string IAddressBasic.Country
			{
				get { throw new NotImplementedException(); }
			}

            //string IAddressBasic.Street
            //{
            //    get { throw new NotImplementedException(); }
            //    set { throw new NotImplementedException(); }
            //}
			#endregion
		}
	}
}
