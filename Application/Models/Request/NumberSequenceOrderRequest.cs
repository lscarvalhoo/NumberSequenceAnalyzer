using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class NumberSequenceOrderRequest
    {
        [JsonPropertyName("valores")]
        public List<int>? Values { get; set; }
    }
}
