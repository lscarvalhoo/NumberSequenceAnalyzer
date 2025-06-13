using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class NumberSequenceOrderRequest
    {
        public List<int>? Values { get; set; }
    }
}
