using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Repositories
{
	public interface ISearchRepository<T, TResult> where T : PaginatedListParameters
	{
		TResult Search(T searchParams);
	}
}
