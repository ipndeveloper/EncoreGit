
namespace NetSteps.Taxes.Common.Models
{
    public interface IEnumConverter<T, U>
    {
        U Convert(T value);
    }
}
