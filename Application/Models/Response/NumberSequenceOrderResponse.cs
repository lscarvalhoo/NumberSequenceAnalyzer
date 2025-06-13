namespace Application.Models.Response
{
    public class NumberSequenceOrderResponse
    {
        public List<int> SortedAscending { get; set; } = new();
        public List<int> SortedDescending { get; set; } = new();
    }
}
