
namespace NetSteps.Data.Entities.Extensions
{
    public static class TokenExtensions
    {
        public static string ToPlaceholder(this Constants.Token token)
        {
            return NetSteps.Data.Entities.Cache.SmallCollectionCache.Instance.Tokens.GetById((int)token).Placeholder;
        }
    }
}
