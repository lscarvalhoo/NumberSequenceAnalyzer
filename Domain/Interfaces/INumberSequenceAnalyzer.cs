namespace Domain.Interfaces
{

    public interface INumberSequenceAnalyzer
    {
        Task<bool> IsAscendingAsync(IAsyncEnumerable<int> values);
        Task<bool> IsDescendingAsync(IAsyncEnumerable<int> values);
        Task<bool> HasDuplicatesAsync(IAsyncEnumerable<int> values);
        Task<bool> IsAlternatingAsync(IAsyncEnumerable<int> values);
        Task<bool> AllPrimesAsync(IAsyncEnumerable<int> values);
    }
}