
namespace NetSteps.Web.Mvc.Validation
{
	public interface IInputValidator<TInput>
	{
		bool IsValid(TInput input, ValidationErrorCollector collector);
	}
}
