using Domain.Services;
using FluentAssertions;

namespace Tests.Domain.Services
{
    public class NumberSequenceAnalyzerTests
    {
        private readonly NumberSequenceAnalyzer _analyzer = new NumberSequenceAnalyzer();

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, true)]
        [InlineData(new int[] { 1, 3, 2, 4, 5 }, false)]
        [InlineData(new int[] { }, true)]
        [InlineData(new int[] { 5 }, true)]
        public async Task IsAscendingAsync_ShouldReturnExpectedResult(int[] values, bool expected)
        {
            var asyncValues = values.ToAsyncEnumerable();

            var result = await _analyzer.IsAscendingAsync(asyncValues);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[] { 5, 4, 3, 2, 1 }, true)]
        [InlineData(new int[] { 5, 3, 4, 2, 1 }, false)]
        [InlineData(new int[] { }, true)]
        [InlineData(new int[] { 1 }, true)]
        public async Task IsDescendingAsync_ShouldReturnExpectedResult(int[] values, bool expected)
        {
            var asyncValues = values.ToAsyncEnumerable();

            var result = await _analyzer.IsDescendingAsync(asyncValues);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, false)]
        [InlineData(new int[] { 1, 2, 3, 2, 5 }, true)]
        [InlineData(new int[] { }, false)]
        public async Task HasDuplicatesAsync_ShouldReturnExpectedResult(int[] values, bool expected)
        {
            var asyncValues = values.ToAsyncEnumerable();

            var result = await _analyzer.HasDuplicatesAsync(asyncValues);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[] { 1, 3, 1, 3, 1 }, true)]
        [InlineData(new int[] { 1, 2, 3, 4, 5 }, false)]
        [InlineData(new int[] { 5, 1, 5, 1, 5 }, true)]
        [InlineData(new int[] { 1, 1, 1, 1 }, false)]
        [InlineData(new int[] { 1, 2 }, true)]
        public async Task IsAlternatingAsync_ShouldReturnExpectedResult(int[] values, bool expected)
        {
            var asyncValues = values.ToAsyncEnumerable();

            var result = await _analyzer.IsAlternatingAsync(asyncValues);

            result.Should().Be(expected);
        }

        [Theory]
        [InlineData(new int[] { 2, 3, 5, 7, 11 }, true)]
        [InlineData(new int[] { 2, 4, 6, 8, 10 }, false)]
        [InlineData(new int[] { 2, 3, 5, 8, 11 }, false)]
        [InlineData(new int[] { }, true)]
        public async Task AllPrimesAsync_ShouldReturnExpectedResult(int[] values, bool expected)
        {
            var asyncValues = values.ToAsyncEnumerable();

            var result = await _analyzer.AllPrimesAsync(asyncValues);

            result.Should().Be(expected);
        }
    }
}