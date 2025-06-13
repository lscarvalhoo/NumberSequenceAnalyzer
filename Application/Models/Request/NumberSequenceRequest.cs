using System.Text.Json.Serialization;

namespace Application.Models.Request
{
    public class NumberSequenceRequest
    {
        [JsonPropertyName("valores")]
        public IAsyncEnumerable<int>? Values { get; set; }
    }
}
