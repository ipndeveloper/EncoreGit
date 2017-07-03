using System;
using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Data.Common.Services;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common;
using NetSteps.Enrollment.Common.Models.Context;
using NetSteps.Events.Common.Services;

namespace NetSteps.Enrollment.Service
{
	[ContainerRegister(typeof(IEnrollmentService), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EnrollmentService : IEnrollmentService
	{
		private IEnrollmentRepository _enrollmentRepository;
		protected IEnrollmentRepository EnrollmentRepository
		{
			get
			{
				if (_enrollmentRepository == null)
				{
					_enrollmentRepository = Create.New<IEnrollmentRepository>();
				}
				return _enrollmentRepository;
			}
		}

		private IEventScheduler _schedulerService;
		protected IEventScheduler SchedulerService
		{
			get
			{
				if (_schedulerService == null)
				{
					_schedulerService = Create.New<IEventScheduler>();
				}
				return _schedulerService;
			}
		}


		public EnrollmentService(IEnrollmentRepository enrollmentRepo)
		{
			_enrollmentRepository = enrollmentRepo;
		}

		public EnrollmentService()
			: this(null)
		{
		}


		public virtual bool IsAccountTypeOptOutable(short accountTypeID)
		{
			return accountTypeID != EnrollmentRepository.DistributorAccountID;
		}


		public virtual bool IsAccountTypeSignUpEnabled(short accountTypeID)
		{
			var enabledAccountTypes = EnrollmentRepository.GetEnabledAccountTypes();
			return enabledAccountTypes.Any(at => at == accountTypeID);
		}


		public virtual void OnEnrollmentComplete(DateTime enrollmentCompleteUTC, int enrollingAccountID, short accountTypeID, int? initialOrderID = null)
		{
			if (!initialOrderID.HasValue)
			{
				EnrollmentRepository.AddEnrollmentCompleteEvent(enrollingAccountID, accountTypeID, initialOrderID);
				return;
			}

			// Send Email to the new account holder with order start kit info
			EnrollmentRepository.AddEnrollmentCompleteEvent(enrollingAccountID, initialOrderID.Value);

			// If a distributor enrolled then send an email to the enroller notifying her of the enrollment.
			if (accountTypeID == EnrollmentRepository.DistributorAccountID)
			{
				EnrollmentRepository.AddDistributorJoinsDownlineEvent(initialOrderID.Value, enrollingAccountID);
			}

			SchedulerService.SchedulePostEnrollmentEvents(enrollingAccountID);
		}


		public virtual void OptOut(IEnrollmentContext enrollmentContext)
		{
			if (!IsAccountTypeOptOutable(enrollmentContext.EnrollingAccount.AccountTypeID))
			{
				return;
			}

			var corporateSponsorID = GetCorporateSponsorID();

			enrollmentContext.EnrollingAccount.SponsorID = corporateSponsorID;
			enrollmentContext.EnrollingAccount.EnrollerID = corporateSponsorID;

			enrollmentContext.SponsorID = corporateSponsorID;
			enrollmentContext.EnrollerID = corporateSponsorID;
			enrollmentContext.PlacementID = corporateSponsorID;
			if (enrollmentContext.InitialOrder != null)
			{
				enrollmentContext.InitialOrder.ConsultantID = corporateSponsorID;

				var orderService = Create.New<IOrderService>();
				orderService.DetachFromParty(enrollmentContext.InitialOrder);
			}

			EnrollmentRepository.SaveOptOut(enrollmentContext.EnrollingAccount.EmailAddress);
			enrollmentContext.EnrollingAccount.IsOptedOut = true;
		}

		public virtual int GetCorporateSponsorID()
		{
			return ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.CorporateAccountID);
		}

		public void OnEnrollmentComplete(DateTime enrollmentCompleteUTC, int enrollingAccountID, int accountTypeID, int? initialOrderID = null)
		{
			throw new NotImplementedException();
		}
	}
}
