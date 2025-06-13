using Application.Models.Request;
using Application.Services;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace Tests.Application.Services
{
    public class NumberSequenceServiceTests
    {
        private readonly INumberSequenceAnalyzer _analyzer = Substitute.For<INumberSequenceAnalyzer>();
        private readonly INumberSequenceOrderer _orderer = Substitute.For<INumberSequenceOrderer>();
        private readonly ILogger<NumberSequenceService> _logger = Substitute.For<ILogger<NumberSequenceService>>();

        private NumberSequenceService CreateService() =>
            new NumberSequenceService(_analyzer, _orderer, _logger);

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
            var sortedAscending = new List<int> { 1, 2, 3, 5, 8 };
            var sortedDescending = new List<int> { 8, 5, 3, 2, 1 };

            var request = new NumberSequenceOrderRequest
            {
                Values = unsorted.ToAsyncEnumerable()
            };

            _orderer.OrderAscendingAsync(Arg.Any<IAsyncEnumerable<int>>()).Returns(sortedAscending);
            _orderer.OrderDescending(Arg.Any<List<int>>()).Returns(sortedDescending);

            var service = CreateService();

            var result = await service.OrderSequenceAsync(request);

            result.Should().NotBeNull();
            result.SortedAscending.Should().BeInAscendingOrder();
            result.SortedAscending.Should().BeEquivalentTo(sortedAscending);
            result.SortedDescending.Should().BeInDescendingOrder();
            result.SortedDescending.Should().BeEquivalentTo(sortedDescending);

            await _orderer.Received(1).OrderAscendingAsync(Arg.Any<IAsyncEnumerable<int>>());
            _orderer.Received(1).OrderDescending(Arg.Any<List<int>>());
        }

        [Fact]
        public async Task OrderSequenceAsync_ShouldReturnSameCountInBothOrders()
        {
            var input = new List<int> { 7, 2, 9, 1 };
            var asc = new List<int> { 1, 2, 7, 9 };
            var desc = new List<int> { 9, 7, 2, 1 };

            var request = new NumberSequenceOrderRequest
            {
                Values = input.ToAsyncEnumerable()
            };

            _orderer.OrderAscendingAsync(Arg.Any<IAsyncEnumerable<int>>()).Returns(asc);
            _orderer.OrderDescending(Arg.Any<List<int>>()).Returns(desc);

            var service = CreateService();

            var result = await service.OrderSequenceAsync(request);

            result.SortedAscending.Count.Should().Be(input.Count);
            result.SortedDescending.Count.Should().Be(input.Count);
        }

    }
}
