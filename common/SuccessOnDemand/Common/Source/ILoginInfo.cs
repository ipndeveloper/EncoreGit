using NetSteps.Encore.Core.Dto;

namespace NetSteps.SOD.Common
{
    [DTO]
    public interface ILoginInfo
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}
