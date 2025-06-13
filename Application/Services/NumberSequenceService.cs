using Application.Interfaces;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
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

        public NumberSequenceResponse AnalyzeSequence(NumberSequenceRequest request)
        {
            var sequence = new NumberSequence
            {
                Values = request.Values!
            };

            var stopwatch = Stopwatch.StartNew();
            var result = new NumberSequenceResponse
            {
                IsAscending = _analyzer.IsAscending(sequence),
                IsDescending = _analyzer.IsDescending(sequence),
                HasDuplicates = _analyzer.HasDuplicates(sequence),
                IsAlternating = _analyzer.IsAlternating(sequence),
                AllPrimes = _analyzer.AllPrimes(sequence)
            };
            stopwatch.Stop();

            _logger.LogInformation("Analysis result: {@Result}", result); 
            _logger.LogInformation("Sequence analyzed in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

            return result;
        }

        public NumberSequenceOrderResponse OrderSequence(NumberSequenceOrderRequest request)
        {
            var values = request.Values ?? new List<int>();

            var ascending = new List<int>(values);
            ascending.Sort();
            _logger.LogInformation("Sorted in ascending: {@Ascending}", ascending);

            var descending = new List<int>(ascending);
            descending.Reverse();
            _logger.LogInformation("Sorted in descending: {@Descending}", descending);

            return new NumberSequenceOrderResponse
            {
                SortedAscending = ascending,
                SortedDescending = descending
            };
        }
    }
}