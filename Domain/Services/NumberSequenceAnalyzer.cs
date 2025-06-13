using Domain.Common.YourProject.Domain.Common;
using Domain.Interfaces;

namespace Domain.Services
{
    public class NumberSequenceAnalyzer : INumberSequenceAnalyzer
    {
        public async Task<bool> IsAscendingAsync(IAsyncEnumerable<int> values)
        {
            int? previous = null;
            await foreach (var current in values)
            {
                if (previous.HasValue && current <= previous.Value)
                    return false;

                previous = current;
            }

            return true;
        }

        public async Task<bool> IsDescendingAsync(IAsyncEnumerable<int> values)
        {
            int? previous = null;
            await foreach (var current in values)
            {
                if (previous.HasValue && current >= previous.Value)
                    return false;

                previous = current;
            }

            return true;
        }


        public async Task<bool> HasDuplicatesAsync(IAsyncEnumerable<int> values)
        {
            var seen = new HashSet<int>();
            await foreach (var value in values)
            {
                if (!seen.Add(value))
                    return true;
            }

            return false;
        }

        public async Task<bool> IsAlternatingAsync(IAsyncEnumerable<int> values)
        {
            var buffer = new Queue<int>();

            await foreach (var value in values)
            {
                buffer.Enqueue(value);

                if (buffer.Count < 3)
                    continue;

                int a = buffer.ElementAt(0);
                int b = buffer.ElementAt(1);
                int c = buffer.ElementAt(2);

                bool isPeak = b > a && b > c;
                bool isValley = b < a && b < c;

                if (!isPeak && !isValley)
                    return false;

                buffer.Dequeue();
            }

            return true;
        }


        public async Task<bool> AllPrimesAsync(IAsyncEnumerable<int> values)
        {
            var tasks = new List<Task<bool>>();

            await foreach (var value in values)
            {
                int localValue = value;
                tasks.Add(Task.Run(() => IsPrime(localValue)));
            }

            var results = await Task.WhenAll(tasks);
            return results.All(x => x);
        }

        private bool IsPrime(int number)
        {
            if (number < SequenceAnalysisConstants.SMALLEST_PRIME)
                return false;

            if (number == SequenceAnalysisConstants.SMALLEST_PRIME)
                return true;

            if (number % SequenceAnalysisConstants.EVEN_NUMBER_DIVISOR == SequenceAnalysisConstants.ZERO)
                return false;

            int boundary = (int)Math.Sqrt(number);

            for (int divisor = SequenceAnalysisConstants.FIRST_ODD_PRIME;
                 divisor <= boundary;
                 divisor += SequenceAnalysisConstants.INCREMENT_BY_TWO)
            {
                if (number % divisor == SequenceAnalysisConstants.ZERO)
                    return false;
            }

            return true;
        }
    }
}