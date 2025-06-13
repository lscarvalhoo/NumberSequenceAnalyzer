namespace Application.Models.Response
{
    public class NumberSequenceResponse
    {
        public bool IsAscending { get; set; }
        public bool IsDescending { get; set; }
        public bool HasDuplicates { get; set; }
        public bool IsAlternating { get; set; }
        public bool AllPrimes { get; set; }
    }
}