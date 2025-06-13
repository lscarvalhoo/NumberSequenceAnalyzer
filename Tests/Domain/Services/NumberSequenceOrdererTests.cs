using Domain.Services;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Domain.Services
{
    public class NumberSequenceOrdererTests
    {
        private readonly NumberSequenceOrderer _orderer = new NumberSequenceOrderer();

        [Theory]
        [InlineData(new int[] { 5, 3, 8, 1, 2 }, new int[] { 1, 2, 3, 5, 8 })]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 1, 2, 3 })]
        [InlineData(new int[] { }, new int[] { })]
        [InlineData(new int[] { 7 }, new int[] { 7 })]
        public async Task OrderAscendingAsync_ShouldSortCorrectly(int[] input, int[] expected)
        {
            var asyncValues = input.ToAsyncEnumerable();

            var result = await _orderer.OrderAscendingAsync(asyncValues);

            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3 }, new int[] { 3, 2, 1 })]
        [InlineData(new int[] { 1, 2 }, new int[] { 2, 1 })]
        [InlineData(new int[] { 9 }, new int[] { 9 })]
        [InlineData(new int[] { }, new int[] { })]
        public void OrderDescending_ShouldReverseListCorrectly(int[] input, int[] expected)
        {
            var result = _orderer.OrderDescending(input.ToList());

            result.Should().BeEquivalentTo(expected, options => options.WithStrictOrdering());
        }
    }
}
