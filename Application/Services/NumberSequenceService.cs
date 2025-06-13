using Application.Interfaces;
using Application.Models.Request;
using Application.Models.Response;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

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

            var result = new NumberSequenceResponse
            {
                IsAscending = _analyzer.IsAscending(sequence),
                IsDescending = _analyzer.IsDescending(sequence),
                HasDuplicates = _analyzer.HasDuplicates(sequence),
                IsAlternating = _analyzer.IsAlternating(sequence),
                AllPrimes = _analyzer.AllPrimes(sequence)
            };

            _logger.LogInformation("Analysis result: {@Result}", result);

            return result;
        }
    }
}
