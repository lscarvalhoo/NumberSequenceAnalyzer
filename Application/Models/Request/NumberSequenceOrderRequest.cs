namespace Application.Models.Request
{
    public class NumberSequenceOrderRequest
    {
        public IAsyncEnumerable<int>? Values { get; set; }
    }
}
