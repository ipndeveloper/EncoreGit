using System.Collections.Generic;
using System.IO;
using NetSteps.Encore.Core.Dto;

namespace EFTQuery.NachaGenerator.Common
{
    [DTO]
    public interface INachaGeneratorResult : IResponse
    {
        Stream NachaFile { get; set; }
        List<int> OrderPaymentIDs { get; set; }
    }

    [DTO]
    public interface IResponse
    {
        ResultState Status { get; set; }
        IList<IError> Errors { get; set; }
    }

    [DTO]
    public interface IError
    {
        string Property { get; set; }
        string Error { get; set; }
    }


    public enum ResultState
    {
        Unknown = 0,
        Exception,
        Error,
        Success
    }
}