namespace NetSteps.Data.Entities.Business.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using NetSteps.Data.Entities.Repositories.Interfaces;
    using NetSteps.Data.Entities.Dto;

    public class LogCommissionBusinessLogic
    {
        private LogCommissionBusinessLogic()
        { }

        private static LogCommissionBusinessLogic instance;

        private static ILogCommissionRepository repository;

        public static LogCommissionBusinessLogic Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogCommissionBusinessLogic();
                    //IOC
                    repository = new NetSteps.Data.Entities.Repositories.LogCommissionRepository();
                }

                return instance;
            }

        }

        public LogCommission DtoToBO(LogCommissionDto dto)
        {
            throw new NotImplementedException();
        }

        public LogCommissionDto BOToDto(LogCommission bo)
        {
            return new LogCommissionDto()
            {
                LogId = bo.LogId,
                Description = bo.Description,
                EndTime = bo.EndTime,
                Result = bo.Result,
                StartTime = bo.StartTime
            };
        }

        public void Insert(LogCommission bo)
        {
            repository.Insert(BOToDto(bo));
        }
    }
}