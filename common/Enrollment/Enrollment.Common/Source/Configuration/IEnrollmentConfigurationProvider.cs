using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Enrollment.Common.Models.Config;

namespace NetSteps.Enrollment.Common.Configuration
{
	[ContractClass(typeof(IEnrollmentConfigurationProviderContracts))]
	public interface IEnrollmentConfigurationProvider
	{
		IEnrollmentConfig GetEnrollmentConfig(int accountTypeID, int siteTypeID);
		string GetOverrideAssemblyName();
		IEnumerable<short> EnrollableAccountTypeIDs { get; }
		bool AccountTypeEnabled(int accountTypeID);
	} 
	
	[ContractClassFor(typeof(IEnrollmentConfigurationProvider))]
	internal abstract class IEnrollmentConfigurationProviderContracts : IEnrollmentConfigurationProvider
	{
		IEnrollmentConfig IEnrollmentConfigurationProvider.GetEnrollmentConfig(int accountTypeID, int siteTypeID)
		{
			Contract.Requires<ArgumentException>(accountTypeID > 0);
			Contract.Requires<ArgumentException>(siteTypeID > 0);
			throw new NotImplementedException();
		}

		string IEnrollmentConfigurationProvider.GetOverrideAssemblyName()
		{
			throw new NotImplementedException();
		}

		IEnumerable<short> IEnrollmentConfigurationProvider.EnrollableAccountTypeIDs
		{
			get { throw new NotImplementedException(); }
		}

		bool IEnrollmentConfigurationProvider.AccountTypeEnabled(int accountTypeID)
		{
			throw new NotImplementedException();
		}
	}
}
