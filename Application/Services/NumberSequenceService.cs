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
        private readonly INumberSequenceOrderer _orderer;
        private readonly ILogger<NumberSequenceService> _logger;

        public NumberSequenceService(
            INumberSequenceAnalyzer analyzer,
            INumberSequenceOrderer orderer,
            ILogger<NumberSequenceService> logger)
        {
            _analyzer = analyzer;
            _orderer = orderer;
            _logger = logger;
        }

        public async Task<NumberSequenceResponse> AnalyzeSequenceAsync(NumberSequenceRequest request)
        {
            var stopwatch = Stopwatch.StartNew();

            var values = await request.Values!.ToListAsync();
            var asyncStream = values.ToAsyncEnumerable();

            var result = new NumberSequenceResponse
            {
                IsAscending = await _analyzer.IsAscendingAsync(asyncStream),
                IsDescending = await _analyzer.IsDescendingAsync(asyncStream),
                HasDuplicates = await _analyzer.HasDuplicatesAsync(asyncStream),
                IsAlternating = await _analyzer.IsAlternatingAsync(asyncStream),
                AllPrimes = await _analyzer.AllPrimesAsync(asyncStream)
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

        private List<int> DescendingSortAsync(List<int> ascending)
        {
            var stopwatchDesc = Stopwatch.StartNew();
            var descending = _orderer.OrderDescending(ascending);
            stopwatchDesc.Stop();

            _logger.LogInformation("Sorted in descending order: {@Descending}", descending);
            _logger.LogInformation("Descending ordering took {ElapsedMilliseconds} ms", stopwatchDesc.ElapsedMilliseconds);

            return descending;
        }

        private async Task<List<int>> AscendingSortAsync(NumberSequenceOrderRequest request)
        {
            var stopwatchAsc = Stopwatch.StartNew();
            var ascending = await _orderer.OrderAscendingAsync(request.Values);
            stopwatchAsc.Stop();

            _logger.LogInformation("Sorted in ascending order: {@Ascending}", ascending);
            _logger.LogInformation("Ascending ordering took {ElapsedMilliseconds} ms", stopwatchAsc.ElapsedMilliseconds);

            return ascending;
        }
    }
}