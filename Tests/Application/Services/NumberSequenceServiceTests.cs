using Application.Models.Request;
using Application.Services;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using HelperAsync = Tests.Helper.AsyncEnumerableHelper;

namespace Tests.Application.Services
{ 
    public class NumberSequenceServiceTests
    {
        private readonly INumberSequenceAnalyzer _analyzer = Substitute.For<INumberSequenceAnalyzer>();
        private readonly ILogger<NumberSequenceService> _logger = Substitute.For<ILogger<NumberSequenceService>>();

        private NumberSequenceService CreateService() =>
            new NumberSequenceService(_analyzer, _logger);

        [Fact]
        public async Task AnalyzeSequenceAsync_ShouldReturnCorrectResponse_WhenCalled()
        {
            var values = new List<int> { 1, 2, 3 };
            var request = new NumberSequenceRequest
            {
                Values = values.ToAsyncEnumerable()
            };

            _analyzer.IsAscendingAsync(Arg.Any<IAsyncEnumerable<int>>()).Returns(Task.FromResult(true));
            _analyzer.IsDescendingAsync(Arg.Any<IAsyncEnumerable<int>>()).Returns(Task.FromResult(false));
            _analyzer.HasDuplicatesAsync(Arg.Any<IAsyncEnumerable<int>>()).Returns(Task.FromResult(false));
            _analyzer.IsAlternatingAsync(Arg.Any<IAsyncEnumerable<int>>()).Returns(Task.FromResult(true));
            _analyzer.AllPrimesAsync(Arg.Any<IAsyncEnumerable<int>>()).Returns(Task.FromResult(false));

            var service = CreateService();

            var result = await service.AnalyzeSequenceAsync(request);

            result.Should().NotBeNull();
            result.IsAscending.Should().BeTrue();
            result.IsDescending.Should().BeFalse();
            result.HasDuplicates.Should().BeFalse();
            result.IsAlternating.Should().BeTrue();
            result.AllPrimes.Should().BeFalse();

            await _analyzer.Received(1).IsAscendingAsync(Arg.Any<IAsyncEnumerable<int>>());
            await _analyzer.Received(1).IsDescendingAsync(Arg.Any<IAsyncEnumerable<int>>());
            await _analyzer.Received(1).HasDuplicatesAsync(Arg.Any<IAsyncEnumerable<int>>());
            await _analyzer.Received(1).IsAlternatingAsync(Arg.Any<IAsyncEnumerable<int>>());
            await _analyzer.Received(1).AllPrimesAsync(Arg.Any<IAsyncEnumerable<int>>());
        }

        [Fact]
        public async Task OrderSequenceAsync_ShouldReturnOrderedSequences_WhenCalled()
        {
            var unsorted = new List<int> { 5, 3, 8, 1, 2 };
            var request = new NumberSequenceOrderRequest
            {
                Values = unsorted.ToAsyncEnumerable()
            };

            var service = CreateService();

            var result = await service.OrderSequenceAsync(request);

            result.Should().NotBeNull();
            result.SortedAscending.Should().BeInAscendingOrder();
            result.SortedAscending.Should().BeEquivalentTo(new List<int> { 1, 2, 3, 5, 8 });
            result.SortedDescending.Should().BeInDescendingOrder();
            result.SortedDescending.Should().BeEquivalentTo(new List<int> { 8, 5, 3, 2, 1 });
        }
    }
}