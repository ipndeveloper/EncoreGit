using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Common interface for Application.
	/// </summary>
	[ContractClass(typeof(Contracts.ApplicationContracts))]
	public interface IApplication
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ApplicationID for this Application.
		/// </summary>
		short ApplicationID { get; set; }
	
		/// <summary>
		/// The Name for this Application.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this Application.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this Application.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ApplicationRunningInstances for this Application.
		/// </summary>
		IEnumerable<IApplicationRunningInstance> ApplicationRunningInstances { get; }
	
		/// <summary>
		/// Adds an <see cref="IApplicationRunningInstance"/> to the ApplicationRunningInstances collection.
		/// </summary>
		/// <param name="item">The <see cref="IApplicationRunningInstance"/> to add.</param>
		void AddApplicationRunningInstance(IApplicationRunningInstance item);
	
		/// <summary>
		/// Removes an <see cref="IApplicationRunningInstance"/> from the ApplicationRunningInstances collection.
		/// </summary>
		/// <param name="item">The <see cref="IApplicationRunningInstance"/> to remove.</param>
		void RemoveApplicationRunningInstance(IApplicationRunningInstance item);

		#endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IApplication))]
		internal abstract class ApplicationContracts : IApplication
		{
		    #region Primitive properties
		
			short IApplication.ApplicationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplication.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IApplication.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IApplication.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IApplicationRunningInstance> IApplication.ApplicationRunningInstances
			{
				get { throw new NotImplementedException(); }
			}
		
			void IApplication.AddApplicationRunningInstance(IApplicationRunningInstance item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IApplication.RemoveApplicationRunningInstance(IApplicationRunningInstance item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
