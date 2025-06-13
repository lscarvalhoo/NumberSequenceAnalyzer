using Application.Interfaces;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Services
{
    public class NumberSequenceService : INumberSequenceService
    {
        private readonly INumberSequenceAnalyzer _analyzer;
        private readonly ILogger<NumberSequenceService> _logger;

        public NumberSequenceService(INumberSequenceAnalyzer analyzer, ILogger<NumberSequenceService> logger)
        {
            _analyzer = analyzer;
            _logger = logger;
        }

        public async Task<NumberSequenceResponse> AnalyzeSequenceAsync(NumberSequenceRequest request)
        {
            var stopwatch = Stopwatch.StartNew();

            var buffered = request.Values!.ToListAsync();

            var values = await buffered;
            var asyncStream = values.ToAsyncEnumerable();

            var result = new NumberSequenceResponse
            {
                IsAscending = await _analyzer.IsAscendingAsync(asyncStream),
                IsDescending = await _analyzer.IsDescendingAsync(values.ToAsyncEnumerable()),
                HasDuplicates = await _analyzer.HasDuplicatesAsync(values.ToAsyncEnumerable()),
                IsAlternating = await _analyzer.IsAlternatingAsync(values.ToAsyncEnumerable()),
                AllPrimes = await _analyzer.AllPrimesAsync(values.ToAsyncEnumerable())
            };

            stopwatch.Stop();
            _logger.LogInformation("Analysis result: {@Result}", result);
            _logger.LogInformation("Sequence analyzed in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

            return result;
        }

        public async Task<NumberSequenceOrderResponse> OrderSequenceAsync(NumberSequenceOrderRequest request)
        {
            var stopwatchTotal = Stopwatch.StartNew();
            List<int> ascending = await AscendingSortAsync(request);
            List<int> descending = DescendingSortAsync(ascending);
            stopwatchTotal.Stop();

            _logger.LogInformation("Total ordering time: {ElapsedMilliseconds} ms", stopwatchTotal.ElapsedMilliseconds);

            return new NumberSequenceOrderResponse
            {
                SortedAscending = ascending,
                SortedDescending = descending
            };
        }

        #region Private

        private List<int> DescendingSortAsync(List<int> values)
        {
            var stopwatchDescending = Stopwatch.StartNew();
            var descending = values.AsEnumerable().Reverse().ToList();
            stopwatchDescending.Stop();

            _logger.LogInformation("Sorted in descending order: {@Descending}", descending);
            _logger.LogInformation("Descending ordering took {ElapsedMilliseconds} ms", stopwatchDescending.ElapsedMilliseconds);
            return descending;
        }

        private async Task<List<int>> AscendingSortAsync(NumberSequenceOrderRequest request)
        {
            var values = await request.Values!.ToListAsync();

            var stopwatchAscending = Stopwatch.StartNew();
            values.Sort();
            stopwatchAscending.Stop();

            _logger.LogInformation("Sorted in ascending order: {@Ascending}", values);
            _logger.LogInformation("Ascending ordering took {ElapsedMilliseconds} ms", stopwatchAscending.ElapsedMilliseconds);
            return values;
        }

        #endregion
    }
}