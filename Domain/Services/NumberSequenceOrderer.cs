using Domain.Interfaces;

namespace Domain.Services
{
    public class NumberSequenceOrderer : INumberSequenceOrderer
    {
        public async Task<List<int>> OrderAscendingAsync(IAsyncEnumerable<int> values)
        {
            var list = await values.ToListAsync();
            list.Sort();
            return list;
        }

        public List<int> OrderDescending(List<int> ascendingOrdered)
        {
            return ascendingOrdered.AsEnumerable().Reverse().ToList();
        }
    }
}