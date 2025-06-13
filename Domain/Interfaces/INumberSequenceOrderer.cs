namespace Domain.Interfaces
{
    public interface INumberSequenceOrderer
    {
        Task<List<int>> OrderAscendingAsync(IAsyncEnumerable<int> values);
        List<int> OrderDescending(List<int> ascendingOrdered);
    }
}
