namespace Quacker.Filtering.Interfaces
{
    public interface IFilteredEnumerableItem<TEntity>
    {
        TEntity Item { get; }

        int Relevance { get; }

        IFilteredEnumerableItem<TDestination> Cast<TDestination>();
    }
}