
namespace NetSteps.Common.Interfaces
{
	public interface ITokenValueProviderFactory
	{
		ITokenValueProvider GetTokenProvider(Constants.TokenValueProviderType tokenValueProviderType);
	}
}
